using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingUtilitiesLibrary.DatabaseHelpers
{
    public class DatabaseGenerator
    {
        public static void CreateBidDatabase(string path)
        {
            var db = new SQLiteDatabase(path);
            db.nonQueryCommand("BEGIN TRANSACTION");
            createAllBidTables(db);
            db.nonQueryCommand("END TRANSACTION");
            db.Connection.Close();
        }
        public static void CreateTemplateDatabase(string path)
        {
            var db = new SQLiteDatabase(path);
            db.nonQueryCommand("BEGIN TRANSACTION");
            createAllTemplateTables(db);
            db.nonQueryCommand("END TRANSACTION");
            db.Connection.Close();
        }

        #region Generic Create Methods
        public static void CreateTableFromDefinition(TableBase table, SQLiteDatabase db)
        {
            var tableInfo = new TableInfo(table);
            string tableName = tableInfo.Name;
            List<TableField> primaryKey = tableInfo.PrimaryFields;
            List<TableField> fields = tableInfo.Fields;

            string createString = "CREATE TABLE '" + tableName + "' (";
            foreach (TableField field in fields)
            {
                createString += "'" + field.Name + "' " + field.FieldType;
                if (fields.IndexOf(field) < (fields.Count - 1))
                { createString += ", "; }
            }
            if (primaryKey.Count != 0)
            { createString += ", PRIMARY KEY("; }
            foreach (TableField pk in primaryKey)
            {
                createString += "'" + pk.Name + "' ";
                if (primaryKey.IndexOf(pk) < (primaryKey.Count - 1))
                { createString += ", "; }
                else
                { createString += ")"; }
            }
            createString += ")";
            db.nonQueryCommand(createString);
        }
        public static void CreateTempTableFromDefinition(TableBase table, SQLiteDatabase db)
        {
            var tableInfo = new TableInfo(table);
            string tableName = "temp_" + tableInfo.Name;
            List<TableField> primaryKey = tableInfo.PrimaryFields;
            List<TableField> fields = tableInfo.Fields;

            string createString = "CREATE TEMPORARY TABLE '" + tableName + "' (";
            foreach (TableField field in fields)
            {
                createString += "'" + field.Name + "' " + field.FieldType;
                if (fields.IndexOf(field) < (fields.Count - 1))
                { createString += ", "; }
            }
            if (primaryKey.Count != 0)
            { createString += ", PRIMARY KEY("; }
            foreach (TableField pk in primaryKey)
            {
                createString += "'" + pk.Name + "' ";
                if (primaryKey.IndexOf(pk) < (primaryKey.Count - 1))
                { createString += ", "; }
                else
                { createString += ")"; }
            }
            createString += ")";
            db.nonQueryCommand(createString);
        }
        static private void createAllBidTables(SQLiteDatabase db)
        {
            foreach (TableBase table in AllBidTables.Tables)
            {
                CreateTableFromDefinition(table, db);
            }
        }
        static private void createAllTemplateTables(SQLiteDatabase db)
        {
            foreach (TableBase table in AllTemplateTables.Tables)
            {
                CreateTableFromDefinition(table, db);
            }
        }
        #endregion
    }
}
