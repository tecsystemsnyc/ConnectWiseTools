using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;

namespace EstimatingUtilitiesLibrary.Database
{
    public class SQLiteDatabase
    {
        static private Logger logger = LogManager.GetCurrentClassLogger();

        const int SQLITE_VERSION = 3;

        public SQLiteConnection Connection;
        public string DBPath;

        public SQLiteDatabase(string dbPath)
        {
            DBPath = dbPath;

            logger.Debug("Connecting to file: " + DBPath);

            if (DBPath != null)
            {
                if (!File.Exists(DBPath))
                {
                    SQLiteConnection.CreateFile(DBPath);
                    logger.Debug("Database File Created");
                }

                Connection = buildConnection(DBPath);
                Connection.Open();
            }
            else
            {
                string message = "SQLiteDatabase needs non null path";
                throw new NullReferenceException(message);
            }
        }

        ~SQLiteDatabase()
        {
            try
            {
                Connection.Close();
            }
            catch
            {
                logger.Debug("Deconstructing SQLiteDatabase. Connection already closed.");
            }
        }

        public void OverwriteFile()
        {
            Connection.Close();
            File.Delete(DBPath);

            if (DBPath != null)
            {
                Connection = buildConnection(DBPath);
                Connection.Open();
            }
            else
            {
                string message = "SQLiteDatabase needs non null path";
                throw new NullReferenceException(message);
            }
        }

        public bool Insert(string tableName, Dictionary<string, string> stringData, Dictionary<string, byte[]> byteData = null)
        {
            string commandString = "insert into " + tableName + " (";

            List<string> colNames = new List<string>();
            List<string> stringVals = new List<string>();
            List<string> byteRefs = new List<string>();

            SQLiteCommand command = new SQLiteCommand(Connection);

            foreach (KeyValuePair<string, string> stringParam in stringData)
            {
                colNames.Add(stringParam.Key);
                stringVals.Add("'" + doubleApostraphes(stringParam.Value) + "'");
            }

            if (byteData != null)
            {
                foreach (KeyValuePair<string, byte[]> byteParam in byteData)
                {
                    colNames.Add(byteParam.Key);

                    string byteRef = "@" + byteParam.Key;

                    byteRefs.Add("(" + byteRef + ")");

                    command.Parameters.Add(byteRef, DbType.Binary).Value = byteParam.Value;
                }
            }

            commandString += UtilitiesMethods.CommaSeparatedString(colNames) + ") values ("
                + UtilitiesMethods.CommaSeparatedString(stringVals);

            if (byteData != null)
            {
                commandString += ", " + UtilitiesMethods.CommaSeparatedString(byteRefs);
            }

            commandString += ")";

            command.CommandText = commandString;

            return (command.ExecuteNonQuery() > 0);
        }

        public bool Replace(string tableName, Dictionary<string, string> data)
        {
            string columns = "";
            string values = "";
            foreach (KeyValuePair<String, String> val in data)
            {
                columns += string.Format(" {0},", val.Key.ToString());
                values += string.Format(" '{0}',", doubleApostraphes(val.Value));
            }
            columns = columns.Substring(0, columns.Length - 1);
            values = values.Substring(0, values.Length - 1);

            string command = string.Format("replace into {0}({1}) values({2})", tableName, columns, values);

            try
            {
                NonQueryCommand(command);
                return true;
            }
            catch (Exception fail)
            {
                logger.Error("Replace failed in SQLiteDB. Command: " + command + " Exception: " + fail.Message);
                return false;
            }
        }

        public bool Delete(string tableName, Dictionary<string, string> primaryKeyValues)
        {
            string commandString = "DELETE FROM " + tableName + " WHERE " + "(";
            bool first = true;
            foreach (KeyValuePair<string, string> pk in primaryKeyValues)
            {
                if (!first)
                {
                    commandString += " AND ";
                }
                commandString += pk.Key + " = '" + pk.Value + "'";
                first = false;
            }
            commandString += ");";

            try
            {
                NonQueryCommand(commandString);
                return true;
            }
            catch (Exception e)
            {
                logger.Error("Deletion failed. Command: " + commandString + " Exception: " + e.Message);
                return false;
            }
        }

        public bool Update(string tableName, Tuple<string, string> whereKey, Dictionary<string, string> stringData)
        {
            List<string> setStrings = new List<string>();
            foreach (KeyValuePair<string, string> stringParam in stringData)
            {
                setStrings.Add(String.Format("{0} = '{1}'",
                    stringParam.Key, doubleApostraphes(stringParam.Value)));
            }
            string setString = UtilitiesMethods.CommaSeparatedString(setStrings);

            string commandString = String.Format("update {0} set {1} where {2} = '{3}';",
                tableName, setString, whereKey.Item1, whereKey.Item2);

            SQLiteCommand command = new SQLiteCommand(Connection);
            command.CommandText = commandString;
            if (command.ExecuteNonQuery() > 0)
            {
                return true;
            }
            else
            {
                logger.Error("Update failed. Command: {0}", command.CommandText);
                return false;
            }
        }

        public void NonQueryCommand(string commandText)
        {
            SQLiteCommand command = new SQLiteCommand(commandText, Connection);
            command.ExecuteNonQuery();
        }

        public DataTable GetDataFromTable(string tableName, params string[] fields)
        {
            DataTable data = new DataTable();
            string fieldString;

            if (fields.Count() > 0)
            {
                fieldString = "";
                int x = 1;
                foreach (string field in fields)
                {
                    fieldString += field;
                    if (x < fields.Count())
                        fieldString += (", ");
                    x++;
                }
                fieldString += " ";
            }
            else
                fieldString = "* ";

            string query = "select " + fieldString;
            query += "from " + tableName;

            data = GetDataFromCommand(query);

            return data;
        }

        public DataTable GetDataFromCommand(string commandText)
        {
            DataTable data = new DataTable();
            SQLiteCommand command = new SQLiteCommand(commandText, Connection);
            SQLiteDataReader reader = command.ExecuteReader();
            try
            {
                data.Load(reader);
            }
            catch (Exception e)
            {
                //Log SQL error and throw again
                logger.Error(e);
                throw e;
            }
            reader.Close();
            return data;
        }

        private SQLiteConnection buildConnection(string dbPath)
        {
            SQLiteConnectionStringBuilder connectionBuilder = new SQLiteConnectionStringBuilder();
            connectionBuilder.DataSource = dbPath;
            connectionBuilder.Version = SQLITE_VERSION;

            return new SQLiteConnection(connectionBuilder.ConnectionString);
        }

        private string doubleApostraphes(string str)
        {
            return str.Replace("'", "''");
        }
    }
}
