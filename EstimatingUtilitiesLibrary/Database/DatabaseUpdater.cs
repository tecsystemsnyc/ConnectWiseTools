using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingUtilitiesLibrary.Database
{

    internal static class DatabaseUpdater
    {
        public static void Update(string dataBasePath, List<UpdateItem> updates)
        {
            SQLiteDatabase db = new SQLiteDatabase(dataBasePath);
            db.NonQueryCommand("BEGIN TRANSACTION");
            foreach(UpdateItem item in updates)
            {
                if(item.Change == Change.Remove)
                {
                    delete(item, db);
                }
                else if (item.Change == Change.Add)
                {
                    add(item, db);
                }
                else if (item.Change == Change.Edit)
                {
                    edit(item, db);
                }
            }

            db.NonQueryCommand("END TRANSACTION");
            db.Connection.Close();
        }

        private static void add(UpdateItem item, SQLiteDatabase db)
        {
            db.Replace(item.Table, item.FieldData);
        }
        private static void delete(UpdateItem item, SQLiteDatabase db)
        {
            db.Delete(item.Table, item.FieldData);
        }
        private static void edit(UpdateItem item, SQLiteDatabase db)
        {
            if(item.PrimaryKey == null)
            {
                throw new Exception("Must have primary key for edit.");
            }
            db.Update(item.Table, item.PrimaryKey, item.FieldData);
        }
        
    }
}
