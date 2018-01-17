using EstimatingLibrary;
using System;
using System.Collections.Generic;

namespace EstimatingUtilitiesLibrary.Database
{

    internal static class DatabaseUpdater
    {
        public static bool Update(string dataBasePath, List<UpdateItem> updates)
        {
            List<string> omitErrors = new List<string> {
                "Note",
                "Exclusion",
                "ProposalScope",
                "Location"
            };

            SQLiteDatabase db = new SQLiteDatabase(dataBasePath);
            db.NonQueryCommand("BEGIN TRANSACTION");
            bool success = true;
            foreach(UpdateItem item in updates)
            {
                if(item.Change == Change.Remove)
                {
                    bool delSuccess = delete(item, db);
                    if (!delSuccess)
                    {
                        success = false;
                    }
                }
                else if (item.Change == Change.Add)
                {
                    bool addSuccess = add(item, db);
                    if (!addSuccess)
                    {
                        success = false;
                    }
                }
                else if (item.Change == Change.Edit)
                {
                    bool editSuccess = edit(item, db);
                    if (!editSuccess && !(omitErrors.Contains(item.Table)))
                    {
                        success = false;
                    }
                }
            }

            db.NonQueryCommand("END TRANSACTION");
            db.Connection.Close();

            return success;
        }

        private static bool add(UpdateItem item, SQLiteDatabase db)
        {
            return db.Replace(item.Table, item.FieldData);
        }
        private static bool delete(UpdateItem item, SQLiteDatabase db)
        {
            return db.Delete(item.Table, item.FieldData);
        }
        private static bool edit(UpdateItem item, SQLiteDatabase db)
        {
            if(item.PrimaryKey == null)
            {
                throw new Exception("Must have primary key for edit.");
            }
            return db.Update(item.Table, item.PrimaryKey, item.FieldData);
        }
        
    }
}
