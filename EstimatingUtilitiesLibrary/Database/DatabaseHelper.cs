using EstimatingLibrary;
using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;

namespace EstimatingUtilitiesLibrary.Database
{
    public enum DBType { Bid = 1, Templates }

    internal static class DatabaseHelper
    {
        public static List<string> TableNames(SQLiteDatabase db)
        {
            string command = "select name from sqlite_master where type = 'table' order by 1";
            DataTable tables = db.GetDataFromCommand(command);
            List<string> tableNames = new List<string>();
            foreach (DataRow row in tables.Rows)
            {
                tableNames.Add(row["Name"].ToString());
            }
            return tableNames;
        }
        public static List<string> TableFields(string tableName, SQLiteDatabase db)
        {
            string command = "select * from " + tableName + " limit 1";
            DataTable data = db.GetDataFromCommand(command);
            List<string> tableFields = new List<string>();
            foreach (DataColumn col in data.Columns)
            {
                tableFields.Add(col.ColumnName);
            }
            return tableFields;
        }
        public static List<string> PrimaryKeys(string tableName, SQLiteDatabase db)
        {
            string command = "PRAGMA table_info(" + tableName + ")";
            DataTable data = db.GetDataFromCommand(command);
            List<string> primaryKeys = new List<string>();
            foreach (DataRow row in data.Rows)
            {
                if (row["pk"].ToString() != "0")
                {
                    primaryKeys.Add(row["name"].ToString());
                }

            }
            return primaryKeys;
        }

        public static void Explain(string command, SQLiteDatabase db)
        {
            var explainer = "explain query plan " + command;
            var explainDT = db.GetDataFromCommand(explainer);
            foreach (DataRow row in explainDT.Rows)
            {
                logger.Debug(row["detail"]);
            }
        }
        public static string AllFieldsInTableString(TableBase table)
        {
            string command = "";
            for (int x = 0; x < table.Fields.Count; x++)
            {
                if (x != table.Fields.Count - 1)
                {
                    command += table.NameString + "." + table.Fields[x].Name + ", ";
                }
                else
                {
                    command += table.NameString + "." + table.Fields[x].Name;
                }
            }
            return command;
        }
        public static string FieldsString(List<string> fields)
        {
            string command = "";
            for (int x = 0; x < fields.Count; x++)
            {
                if (x != fields.Count - 1)
                {
                    command += fields[x] + ", ";
                }
                else
                {
                    command += fields[x];
                }
            }
            return command;
        }
        public static string ValuesString(List<string> values)
        {
            string command = "";
            for (int x = 0; x < values.Count; x++)
            {
                if (x != values.Count - 1)
                {
                    command += "'" + values[x] + "', ";
                }
                else
                {
                    command += "'" + values[x] + "'";
                }
            }
            return command;
        }

        public static void CreateBackup(string originalPath)
        {
            logger.Trace("Backing up...");

            var date = DateTime.Now;

            string APPDATA_FOLDER = @"TECSystems\Backups\";
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string backupFolder = Path.Combine(appData, APPDATA_FOLDER);

            CultureInfo culture = CultureInfo.CreateSpecificCulture("ja-JP");
            DateTimeFormatInfo dtfi = culture.DateTimeFormat;
            dtfi.DateSeparator = "\\";
            backupFolder += date.ToString("d", dtfi);

            if (!Directory.Exists(backupFolder))
            { Directory.CreateDirectory(backupFolder); }

            string backupFileName = Path.GetFileNameWithoutExtension(originalPath);
            backupFileName += "-";
            culture = CultureInfo.CreateSpecificCulture("hr-HR");
            dtfi = culture.DateTimeFormat;
            dtfi.TimeSeparator = "-";
            backupFileName += date.ToString("T", dtfi);
            var backupPath = Path.Combine(backupFolder, backupFileName);

            File.Copy(originalPath, backupPath);

            logger.Trace("Finished backup. Backup path: " + backupPath);
        }

        public static List<TableBase> GetTables(List<TECObject> items, string propertyName, DBType type = 0)
        {
            List<TableBase> tables = new List<TableBase>();
            if (items.Count > 2 || items.Count == 0)
            { throw new NotImplementedException(); }

            List<TableBase> allTables = AllTables.Tables;
            if (type == DBType.Bid)
            {
                allTables = AllBidTables.Tables;
            }
            else if (type == DBType.Templates)
            {
                allTables = AllTemplateTables.Tables;
            }

            foreach (TableBase table in allTables)
            {
                if (matchesAllTypes(items, table.Types) && matchesPropertyName(propertyName, table.PropertyNames))
                {
                    tables.Add(table);
                }

            }
            return tables;
        }
        public static List<TableBase> GetTables(TECObject item, DBType type = 0)
        {
            List<TableBase> tables = new List<TableBase>();
            List<TableBase> allTables = AllTables.Tables;
            if (type == DBType.Bid)
            {
                allTables = AllBidTables.Tables;
            }
            else if (type == DBType.Templates)
            {
                allTables = AllTemplateTables.Tables;
            }

            foreach (TableBase table in allTables)
            {
                if (matchesObjectType(item, table.Types))
                {
                    tables.Add(table);
                }
            }
            return tables;
        }
        public static TableField GetField(TableBase table, string propertyName)
        {
            foreach (TableField field in table.Fields)
            {
                if (field.Property.Name == propertyName)
                {
                    return field;
                }
            }
            return null;
        }

        public static Dictionary<string, string> PrepareDataForObjectTable(List<TableField> fields, object item)
        {
            Dictionary<string, string> fieldData = new Dictionary<string, string>();
            foreach (TableField field in fields)
            {
                if (field.Property.DeclaringType.IsInstanceOfType(item))
                {
                    var dataString = objectToDBString(field.Property.GetValue(item, null));
                    fieldData.Add(field.Name, dataString);
                }
            }
            return fieldData;
        }
        public static Dictionary<string, string> PrepareDataForRelationTable(List<TableField> fields, object item, object child)
        {
            Dictionary<string, string> fieldData = new Dictionary<string, string>();
            var parentField = fields[0];
            var childField = fields[1];
            if (parentField.Property.DeclaringType.IsInstanceOfType(item))
            {
                var dataString = objectToDBString(parentField.Property.GetValue(item, null));
                fieldData.Add(parentField.Name, dataString);
            }
            if (childField.Property.DeclaringType.IsInstanceOfType(child))
            {
                var dataString = objectToDBString(childField.Property.GetValue(child, null));
                fieldData.Add(childField.Name, dataString);
            }
            foreach (TableField field in fields)
            {
                if (field.Property.ReflectedType == typeof(HelperProperties))
                {
                    var dataString = objectToDBString(helperObject(field, item, child));
                    fieldData.Add(field.Name, dataString);
                }
            }

            return fieldData;
        }
        public static Dictionary<string, string> PrepareDataForEditObject(List<TableField> fields, TECObject item, string propertyName, object value)
        {
            foreach (TableField field in fields)
            {
                if (field.Property.DeclaringType.IsInstanceOfType(item) &&
                    field.Property.Name == propertyName)
                {
                    Dictionary<string, string> outData = new Dictionary<string, string>();
                    outData[field.Name] = objectToDBString(value);
                    return outData;
                }
            }
            return null;
        }
        public static Tuple<string, string> PrimaryKeyData(TableBase table, TECObject item)
        {
            if (table.PrimaryKeys.Count != 1)
            {
                throw new Exception("Must have one primary key in table being updated.");
            }
            TableField field = table.PrimaryKeys[0];
            if (field.Property.DeclaringType.IsInstanceOfType(item))
            {
                Tuple<string, string> outData = new Tuple<string, string>(field.Name, objectToDBString(item.Guid));
                return outData;
            }
            else
            {
                throw new Exception("Item does not match type of key.");
            }
        }

        private static bool matchesAllTypes(List<TECObject> items, List<Type> tableTypes)
        {
            if (items.Count == tableTypes.Count)
            {
                for (int x = 0; x < items.Count; x++)
                {
                    bool isTableType = tableTypes[x].IsInstanceOfType(items[x]);

                    if (!isTableType)
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }
        private static bool matchesPropertyName(string propertyName, List<string> tablePropertyNames)
        {
            return (tablePropertyNames.Contains(propertyName));
        }
        private static bool matchesObjectType(TECObject item, List<Type> tableTypes)
        {
            if (tableTypes.Count != 1)
            {
                return false;
            }
            Type tableType = tableTypes[0];
            return (item.GetType() == tableType || item.GetType().BaseType == tableType);
        }

        private static string objectToDBString(Object inObject)
        {
            string outstring = "";
            if (inObject is bool)
            {
                outstring = ((bool)inObject).ToInt().ToString();
            }
            else if (inObject is DateTime dateTime)
            {
                outstring = dateTime.ToString("O");
            }
            else
            {
                outstring = inObject.ToString();
            }

            return outstring;
        }
        private static object helperObject(TableField field, object item, object child)
        {
            if (field.Property.Name == "Quantity")
            {

                var childCollection = item.GetType().GetProperty(field.HelperContext).GetValue(item) as IEnumerable;
                int count = 0;
                foreach (object thing in childCollection)
                {
                    if (thing == child)
                    {
                        count++;
                    }
                }
                return count;
            }
            else if (field.Property.Name == "Index")
            {
                var childCollection = item.GetType().GetProperty(field.HelperContext).GetValue(item) as IEnumerable;
                int x = 0;
                foreach (object thing in childCollection)
                {
                    if (thing == item)
                    {
                        return x;
                    }
                    x++;
                }
                return 0;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private static Logger logger = LogManager.GetCurrentClassLogger();
    }
}
