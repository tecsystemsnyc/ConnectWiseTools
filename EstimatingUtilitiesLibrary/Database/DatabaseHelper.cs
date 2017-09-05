using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EstimatingLibrary;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Drawing;
using System.Reflection;
using System.Collections;
using DebugLibrary;
using EstimatingLibrary.Interfaces;
using EstimatingLibrary.Utilities;

namespace EstimatingUtilitiesLibrary.Database
{
    internal static class DatabaseHelper
    {
        public enum DBType { Bid, Templates }

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
            if (DebugBooleans.VerboseSQLite)
            {
                Console.WriteLine("");
                var explainer = "explain query plan " + command;
                var explainDT = db.GetDataFromCommand(explainer);
                foreach (DataRow row in explainDT.Rows)
                {
                    Console.WriteLine(row["detail"]);
                }
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

        public static void CreateBackup(string originalPath)
        {
            DebugHandler.LogDebugMessage("Backing up...");

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

            DebugHandler.LogDebugMessage("Finished backup. Backup path: " + backupPath);
        }

    }
}
