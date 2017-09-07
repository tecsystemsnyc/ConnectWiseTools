using EstimatingLibrary;
using System;
using System.Collections;
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
                //try
                //{
                    updateDatabase(db, versionDefintion);
                    db.Connection.Close();
                    return true;
                //}
                //catch
                //{
                //    db.Connection.Close();

                //    return false;
                //}
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
            if (tableNames.Contains(BidInfoTable.TableName) || tableNames.Contains("TECBidInfo"))
            { databaseTableList = AllBidTables.Tables; }
            else if (tableNames.Contains(TemplatesInfoTable.TableName))
            { databaseTableList = AllTemplateTables.Tables; }
            else
            { throw new ArgumentException("updateDatabase() can't determine db type"); }
            foreach (TableBase table in databaseTableList)
            {
                tableMap[table.NameString] = DatabaseGenerator.CreateTempTableFromDefinition(table, db);
            }
            int updateVerison = Properties.Settings.Default.Version;
            DataTable infoDT = db.GetDataFromTable(MetadataTable.TableName);
            int originalVerion = infoDT.Rows[0][MetadataTable.Version.Name].ToString().ToInt();
            updateToVersion(versionDefinition, db, originalVerion, updateVerison, tableMap);
            removeOldTables(tableNames, db);
            foreach (TableBase table in databaseTableList)
            {
                DatabaseGenerator.CreateTableFromDefinition(table, db);
            }
            migrateFromTempTables(tableMap, db);
            UpdateVersionNumber(db);

        }
        private static void updateToVersion(DataTable dataTable, SQLiteDatabase db, int originalVersion, int updateVersion, Dictionary<string, string> tempMap)
        {
            TableMapList mapList = buildMap(dataTable, originalVersion, updateVersion, tempMap);
            foreach(TableMap map in mapList)
            {
                if(map.OriginalTableNames.Count == 1)
                {
                    migrateData(map.OriginalTableNames[0], DatabaseHelper.FieldsString(map.OriginalFields),
                        map.UpdateTableName, DatabaseHelper.FieldsString(map.UpdateFields), db);
                } 
            }
        }
        static private void migrateFromTempTables(Dictionary<string, string> tableMap, SQLiteDatabase db)
        {
            string commandString;
            foreach (KeyValuePair<string, string> tablePair in tableMap)
            {
                string tableName = tablePair.Key;
                string tempName = tablePair.Value ;
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
        public static void UpdateVersionNumber(SQLiteDatabase db)
        {
            string commandString = "update " + MetadataTable.TableName + " set " + MetadataTable.Version.Name + " = '" + Properties.Settings.Default.Version + "' ";
            db.NonQueryCommand(commandString);
        }
        private static void migrateData(string originalTable, string originalFields, string updateTable, string updateFields, SQLiteDatabase db)
        {
            string commandString = String.Format("insert into {0} ({1}) select {2} from '{3}'", updateTable, updateFields, originalFields, originalTable);
            db.NonQueryCommand(commandString);
        }
        private static string fieldString(int version)
        {
            return "Field " + version;
        }
        private static string tableString(int version)
        {
            return "Table " + version;
        }

        private static TableMapList buildMap(DataTable dt, int originalVersion, int updateVersion, Dictionary<string, string> tempMap)
        {
            string originalTableColumn = tableString(originalVersion);
            string originalFieldColumn = fieldString(originalVersion);
            string updateTableColumn = tableString(updateVersion);
            string updateFieldColumn = fieldString(updateVersion);

            TableMapList mapList = new TableMapList();
            foreach(DataRow row in dt.Rows)
            {
                if (tempMap.ContainsKey(row[updateTableColumn].ToString()))
                {
                    string originalTable = row[originalTableColumn].ToString();
                    string originalField = row[originalFieldColumn].ToString();
                    string updateTempTable = tempMap[row[updateTableColumn].ToString()];
                    string updateField = row[updateFieldColumn].ToString();

                    if (row[originalFieldColumn].ToString() != "NONE" || row[updateFieldColumn].ToString() != "NONE")
                    {
                        if (mapList.ContainsTable(updateTempTable))
                        {
                            TableMap map = mapList.GetMap(updateTempTable);
                            map.OriginalFields.Add(originalField);
                            map.UpdateFields.Add(updateField);
                            if (!map.OriginalTableNames.Contains(originalTable))
                            {
                                map.OriginalTableNames.Add(originalTable);
                            }
                            
                        }
                        else
                        {
                            TableMap map = new TableMap();
                            map.UpdateTableName = updateTempTable;
                            map.OriginalFields.Add(originalField);
                            map.UpdateFields.Add(updateField);
                            map.OriginalTableNames.Add(originalTable);
                            mapList.Add(map);
                        }
                    }
                }
            }
            return mapList;
        }

        private class TableMapList : IEnumerable
        {
            List<TableMap> mapList;
            Dictionary<string, TableMap> dictionary;
            public TableMapList()
            {
                mapList = new List<TableMap>();
                dictionary = new Dictionary<string, TableMap>();
            }
            public TableMap this[int index]
            {
                get { return mapList[index]; }
            }

            public IEnumerator GetEnumerator()
            {
                return mapList.GetEnumerator();
            }
            
            public void Add(TableMap item)
            {
                mapList.Add(item);
                dictionary[item.UpdateTableName] = item;
            }

            public bool ContainsTable(string tableName)
            {
                return dictionary.ContainsKey(tableName);
            }

            public TableMap GetMap(string tableName)
            {
                return dictionary[tableName];
            }
        }

        private class TableMap
        {
            public string UpdateTableName;
            public List<string> OriginalTableNames;
            public List<string> UpdateFields;
            public List<string> OriginalFields;
            public TableMap()
            {
                OriginalFields = new List<string>();
                OriginalTableNames = new List<string>();
                UpdateFields = new List<string>();
            }
        }
        #endregion
    }
}
