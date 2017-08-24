using EstimatingLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EstimatingUtilitiesLibrary.Database
{
    internal class DatabaseVersionManager
    {
        public static bool CheckAndUpdate(string path, DataTable versionDefintion)
        {
            SQLiteDatabase db = new SQLiteDatabase(path);
            if (!isUpToDate(db))
            {
                try
                {
                    updateDatabase(db, versionDefintion);
                    db.Connection.Close();
                    return true;
                }
                catch
                {
                    db.Connection.Close();

                    return false;
                }
            }
            db.Connection.Close();
            return true;
        }

        #region Database Version Update Methods
        static private bool isUpToDate(SQLiteDatabase db)
        {
            int currentVersion = Properties.Settings.Default.Version;
            DataTable infoDT = db.GetDataFromTable(MetadataTable.TableName);

            if (infoDT.Rows.Count < 1)
            {
                throw new DataException("Could not load database data.");
            }
            else if (infoDT.Rows.Count == 1)
            {
                DataRow infoRow = infoDT.Rows[0];
                if (infoDT.Columns.Contains(MetadataTable.Version.Name))
                {
                    int version = infoRow[MetadataTable.Version.Name].ToString().ToInt();
                    return (version == currentVersion);
                }
                else
                { return false; }
            } else
            {
                throw new DataException("Improperly formatted database data.");
            }
        }
        static private void updateDatabase(SQLiteDatabase db, DataTable versionDefinition)
        {
            Dictionary<string, string> tableMap = new Dictionary<string, string>();
            List<string> tableNames = DatabaseHelper.TableNames(db);
            List<TableBase> databaseTableList = new List<TableBase>();
            if (tableNames.Contains(BidInfoTable.TableName))
            { databaseTableList = AllBidTables.Tables; }
            else if (tableNames.Contains(TemplatesInfoTable.TableName))
            { databaseTableList = AllTemplateTables.Tables; }
            else
            { throw new ArgumentException("updateDatabase() can't determine db type"); }
            foreach (TableBase table in databaseTableList)
            {
                TableInfo info = new TableInfo(table);
                tableMap[info.Name] = DatabaseGenerator.CreateTempTableFromDefinition(table, db);
            }
            int updateVerison = Properties.Settings.Default.Version;
            DataTable infoDT = db.GetDataFromTable(MetadataTable.TableName);
            int originalVerion = infoDT.Rows[0][MetadataTable.Version.Name].ToString().ToInt();
            updateToVersion(versionDefinition, db, originalVerion, updateVerison, tableMap);
            migrateFromTempTables(databaseTableList, db);
            removeOldTables(tableNames, db);
            updateVersionNumber(db);

        }
        private static void updateToVersion(DataTable dataTable, SQLiteDatabase db, int originalVersion, int updateVersion, Dictionary<string, string> tableMap)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                migrateData(row[tableString(originalVersion)] as string, row[fieldString(originalVersion)] as string,
                    tableMap[row[tableString(updateVersion)] as string], row[fieldString(updateVersion)] as string);
            }
        }
        static private void migrateFromTempTables(List<TableBase> tables, SQLiteDatabase db)
        {
            string commandString;
            foreach (TableBase table in tables)
            {
                var tableInfo = new TableInfo(table);
                string tableName = tableInfo.Name;
                string tempName = "temp_" + tableName;
                commandString = "insert into '" + tableName + "' select * from '" + tempName + "'";
                db.NonQueryCommand(commandString);
                commandString = "drop table '" + tempName + "'";
                db.NonQueryCommand(commandString);
            }
        }
        static private void removeOldTables(List<string> tableNames, SQLiteDatabase db)
        {
            foreach(string table in tableNames)
            {
                string commandString = "drop table '" + table + "'";
                db.NonQueryCommand(commandString);
            }
        }
        private static void updateVersionNumber(SQLiteDatabase db)
        {
            var infoBid = DatabaseLoader.GetBidInfo(db);
            string commandString = "update " + MetadataTable.TableName + " set " + MetadataTable.Version.Name + " = '" + Properties.Settings.Default.Version + "' ";
            db.NonQueryCommand(commandString);
        }
        private static void migrateData(string originalTable, string originalField, string updateTable, string updateField)
        {
            string commandString = String.Format("insert into {0} '({1})' select {2} from '{3}'", updateTable, updateField, originalField, originalTable);
        }
        private static string fieldString(int version)
        {
            return "field " + version;
        }
        private static string tableString(int version)
        {
            return "table " + version;
        }
        #endregion
    }
}
