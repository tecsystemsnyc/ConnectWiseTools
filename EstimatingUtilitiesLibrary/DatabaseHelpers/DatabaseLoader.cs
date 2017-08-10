using EstimatingLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EstimatingUtilitiesLibrary;
using System.Windows;

namespace EstimatingUtilitiesLibrary.DatabaseHelpers
{
    public class DatabaseLoader
    {
        public static TECScopeManager Load(string path)
        {
            TECScopeManager workingScopeManager = null;
            var SQLiteDB = new SQLiteDatabase(path);
            SQLiteDB.nonQueryCommand("BEGIN TRANSACTION");

            var tableNames = DatabaseHelper.TableNames(SQLiteDB);
            if (tableNames.Contains("TECBidInfo"))
            {
                //workingScopeManager = loadBid();
            }
            else if (tableNames.Contains("TECTemplatesInfo"))
            {
                //workingScopeManager = loadTemplates();
            }
            else
            {
                MessageBox.Show("File is not a compatible database.");
                return null;
            }

            SQLiteDB.nonQueryCommand("END TRANSACTION");
            SQLiteDB.Connection.Close();
            GC.Collect();
            GC.WaitForPendingFinalizers();

            return workingScopeManager;
        }

        //private TECObject loadObject(TableBase table, )
        //{

        //}

    }
}
