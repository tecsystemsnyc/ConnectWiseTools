using EstimatingLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingUtilitiesLibrary
{

    public class DatabaseUpdater
    {
        private List<UpdateItem> stack;

        public DatabaseUpdater(List<UpdateItem> updates)
        {
            stack = updates;
        }

        public void Update(string dataBasePath)
        {
            SQLiteDatabase db = new SQLiteDatabase(dataBasePath);
            db.nonQueryCommand("BEGIN TRANSACTION");
            foreach(UpdateItem item in stack)
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

            db.nonQueryCommand("END TRANSACTION");
            db.Connection.Close();
        }

        private void add(UpdateItem item, SQLiteDatabase db)
        {
            db.Insert(item.Table, item.FieldData);
        }
        private void delete(UpdateItem item, SQLiteDatabase db)
        {
            db.Delete(item.Table, item.FieldData);
        }
        private void edit(UpdateItem item, SQLiteDatabase db)
        {
            db.Insert(item.Table, item.FieldData);
        }
        
    }
}
