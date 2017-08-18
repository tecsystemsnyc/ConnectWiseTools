using EstimatingLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EstimatingUtilitiesLibrary.DatabaseHelpers
{
    class DatabaseVersionManager
    {
        private static bool isCompatbile;
        #region Database Version Update Methods
        static private void checkAndUpdateDB(Type type, SQLiteDatabase db)
        {
            bool isUpToDate;
            isUpToDate = checkDatabaseVersion(type, db);
            if (!isCompatbile)
            {
                MessageBox.Show("This database is not compatible with this version of the program.");
                return;
            }
            else if (!isUpToDate)
            {
                updateDatabase(type, db);
                updateVersionNumber(type, db);
            }
        }
        static private bool checkDatabaseVersion(Type type, SQLiteDatabase db)
        {
            string currentVersion = Properties.Settings.Default.Version;
            string lowestCompatible = "1.6.0.11";
            DataTable infoDT = new DataTable();
            if (type == typeof(TECBid))
            { infoDT = db.getDataFromTable(BidInfoTable.TableName); }
            else if (type == typeof(TECTemplates))
            {
                try
                {
                    infoDT = db.getDataFromTable(TemplatesInfoTable.TableName);
                }
                catch
                {
                    killTemplatesInfo(db);
                    return false;
                }
            }
            else
            { throw new ArgumentException("checkDatabaseVersion given invalid type"); }

            if (infoDT.Rows.Count < 1)
            {
                if (type == typeof(TECBid))
                {
                    isCompatbile = false;
                    throw new DataException("Could not load from TECBidInfo");
                }
                else if (type == typeof(TECTemplates))
                {
                    isCompatbile = false;
                    return false;
                }
                else
                { return false; }
            }
            else if ((infoDT.Rows.Count == 1) || (type == typeof(TECBid)))
            {
                DataRow infoRow = infoDT.Rows[0];
                if (infoDT.Columns.Contains(BidInfoTable.DBVersion.Name) || infoDT.Columns.Contains(TemplatesInfoTable.DBVersion.Name))
                {
                    string version = infoRow[BidInfoTable.DBVersion.Name].ToString();
                    if (UtilitiesMethods.IsLowerVersion(lowestCompatible, version))
                    {
                        isCompatbile = false;
                        return false;
                    }
                    isCompatbile = true;
                    return (!UtilitiesMethods.IsLowerVersion(currentVersion, version));
                }
                else
                { return false; }
            }
            else if ((infoDT.Rows.Count > 1) && (type == typeof(TECTemplates)))
            {
                killTemplatesInfo(db);
                return false;
            }
            else
            {
                throw new NotImplementedException();
            }
        }
        static private void updateDatabase(Type type, SQLiteDatabase db)
        {
            List<string> tableNames = DatabaseHelper.TableNames(db);
            List<object> databaseTableList = new List<object>();
            if (type == typeof(TECBid))
            { databaseTableList = AllBidTables.Tables; }
            else if (type == typeof(TECTemplates))
            { databaseTableList = AllTemplateTables.Tables; }
            else
            { throw new ArgumentException("updateDatabase() given invalid type"); }
            foreach (TableBase table in databaseTableList)
            {
                var tableInfo = new TableInfo(table);
                if (tableNames.Contains(tableInfo.Name))
                { updateTableFromType(table, db); }
                else
                { DatabaseGenerator.CreateTableFromDefinition(table, db); }
            }
        }
        static private void updateTableFromType(TableBase table, SQLiteDatabase db)
        {
            var tableInfo = new TableInfo(table);
            string tableName = tableInfo.Name;
            string tempName = "temp_" + tableName;
            List<TableField> primaryKeys = tableInfo.PrimaryFields;
            List<TableField> fields = tableInfo.Fields;

            List<string> currentFields = DatabaseHelper.TableFields(tableName, db);
            List<string> currentPrimaryKeys = DatabaseHelper.PrimaryKeys(tableName, db);
            List<string> commonFields = new List<string>();
            foreach (TableField field in fields)
            {
                if (currentFields.Contains(field.Name))
                { commonFields.Add(field.Name); }
            }
            List<string> currentFieldNames = new List<string>();
            List<string> newFieldNames = new List<string>();
            foreach (string field in commonFields)
            {
                currentFieldNames.Add(field);
                newFieldNames.Add(field);
            }

            if (currentPrimaryKeys.Count == 1 && !commonFields.Contains(currentPrimaryKeys[0]) && (primaryKeys.Count == 1))
            {
                currentFieldNames.Add(currentPrimaryKeys[0]);
                newFieldNames.Add(primaryKeys[0].Name);
            }

            string currentCommonString = UtilitiesMethods.CommaSeparatedString(currentFieldNames);
            string newCommonString = UtilitiesMethods.CommaSeparatedString(newFieldNames);

            DatabaseGenerator.CreateTempTableFromDefinition(table, db);

            string commandString;
            if (commonFields.Count > 0)
            {
                commandString = "insert or ignore into '" + tempName + "' (" + newCommonString + ") select " + currentCommonString + " from '" + tableName + "'";
                db.nonQueryCommand(commandString);
            }

            commandString = "drop table '" + tableName + "'";
            db.nonQueryCommand(commandString);
            DatabaseGenerator.CreateTableFromDefinition(table, db);

            commandString = "insert into '" + tableName + "' select * from '" + tempName + "'";
            db.nonQueryCommand(commandString);
            commandString = "drop table '" + tempName + "'";
            db.nonQueryCommand(commandString);

        }
        private static void updateVersionNumber(Type type, SQLiteDatabase db)
        {
            if (type == typeof(TECBid) || type == typeof(TECTemplates))
            {
                Dictionary<string, string> Data = new Dictionary<string, string>();
                if (type == typeof(TECBid))
                {
                    var infoBid = DatabaseLoader.GetBidInfo(db);
                    string commandString = "update " + BidInfoTable.TableName + " set " + BidInfoTable.DBVersion.Name + " = '" + Properties.Settings.Default.Version + "' ";
                    commandString += "where " + BidInfoTable.ID.Name + " = '" + infoBid.Guid.ToString() + "'";
                    db.nonQueryCommand(commandString);
                }
                else if (type == typeof(TECTemplates))
                {
                    var templateGuid = DatabaseLoader.GetTemplatesInfo(db).Guid;

                    Dictionary<string, string> data = new Dictionary<string, string>();

                    data.Add(TemplatesInfoTable.DBVersion.Name, Properties.Settings.Default.Version);
                    data.Add(TemplatesInfoTable.ID.Name, templateGuid.ToString());

                    db.Replace(TemplatesInfoTable.TableName, data);
                }
            }
        }
        private static void killTemplatesInfo(SQLiteDatabase db)
        {
            string commandString = commandString = "drop table '" + TemplatesInfoTable.TableName + "'";
            db.nonQueryCommand(commandString);
        }
        #endregion
    }
}
