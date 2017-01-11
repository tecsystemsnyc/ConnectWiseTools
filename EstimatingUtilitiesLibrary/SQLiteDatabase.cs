using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Windows;

namespace EstimatingUtilitiesLibrary
{
    public class SQLiteDatabase
    {
        const int SQLITE_VERSION = 3;

        public SQLiteConnection Connection;
        public string DBPath;

        public SQLiteDatabase(string dbPath)
        {
            DBPath = dbPath;

            if (DBPath != null)
            {
                if (!File.Exists(DBPath))
                {
                    Console.WriteLine("File Created");
                    SQLiteConnection.CreateFile(DBPath);
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
                Console.WriteLine("Connection already closed");
            }
        }

        public void overwriteFile()
        {
            Connection.Close();
            File.Delete(DBPath);

            if (DBPath != null)
            {
                try
                {
                    Connection = buildConnection(DBPath);
                    Connection.Open();
                    //Console.WriteLine("Connection opened");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception thrown while trying to create SQLiteDatabase: " + e.Message);
                }
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

            foreach (KeyValuePair<string,string> stringParam in stringData)
            {
                colNames.Add(stringParam.Key);
                stringVals.Add("'" + stringParam.Value + "'");
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
            try
            {
                return (command.ExecuteNonQuery() > 0);
            }
            catch (Exception e)
            {
                Console.WriteLine("Insert() failed. Error: " + e.Message);
                Console.WriteLine("Failed command string: " + commandString);
                return false;
            }
        }

        public bool Replace(string tableName, Dictionary<string, string> data)
        {
            string columns = "";
            string values = "";
            foreach (KeyValuePair<String, String> val in data)
            {
                columns += string.Format(" {0},", val.Key.ToString());
                values += string.Format(" '{0}',", val.Value);
            }
            columns = columns.Substring(0, columns.Length - 1);
            values = values.Substring(0, values.Length - 1);
            try
            {
                string command = string.Format("replace into {0}({1}) values({2})", tableName, columns, values);
                //Console.WriteLine("Replace command: " + command);
                nonQueryCommand(command);
                return true;
            }
            catch (Exception fail)
            {
                Console.WriteLine("Error: Replace failed. Code: " + fail.Message);
                return false;
            }
        }

        public bool Delete(string tableName, string idName, Guid guid)
        {
            bool returnCode = true;
            string commandString = "DELETE FROM " + tableName + " WHERE ";
            try
            {
                string objectCommandString = commandString + "(" + idName + " = '" + guid + "');";
                nonQueryCommand(objectCommandString);
            }
            catch (Exception e)
            {
                Console.WriteLine("Deletion failed. Error: " + e.Message);
                returnCode = false;
            }
            return returnCode;
        }

        public void nonQueryCommand(string commandText)
        {
            try
            {
                SQLiteCommand command = new SQLiteCommand(commandText, Connection);
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine("NonQueryCommand() failed with CommandText: " + commandText + " Error: " + e.Message);
                throw e;
            }
        }

        public DataTable getDataFromTable(string tableName, params string[] fields)
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

            try
            {
                string query = "select " + fieldString;
                query += "from " + tableName;
                data = getDataFromCommand(query);
            }
            catch (Exception fail)
            {
                Console.WriteLine("Error: " + fail.Message);
            }

            return data;
        }

        public DataTable getDataFromCommand(string commandText)
        {
            DataTable data = new DataTable();
            try
            {
                SQLiteCommand command = new SQLiteCommand(commandText, Connection);
                SQLiteDataReader reader = command.ExecuteReader();
                data.Load(reader);
                reader.Close();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return data;
        }

        private SQLiteConnection buildConnection(string dbPath)
        {
            SQLiteConnectionStringBuilder connectionBuilder = new SQLiteConnectionStringBuilder();
            connectionBuilder.DataSource = dbPath;
            connectionBuilder.Version = SQLITE_VERSION;

            return new SQLiteConnection(connectionBuilder.ConnectionString);
        }
    }
}
