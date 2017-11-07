using System.Data;
using System.IO;

namespace EstimatingUtilitiesLibrary
{
    public static class CSVReader
    {
        public static DataTable Read(string content)
        {
            DataTable data = new DataTable();
            StringReader reader = new StringReader(content);
            string header = reader.ReadLine();
            string[] columns = header.Split(',');
            foreach(string column in columns)
            {
                data.Columns.Add(column);
            }
            string readLine;
            while ((readLine = reader.ReadLine())!= null)
            {
                string[] line = readLine.Split(',');
                DataRow row = data.NewRow();
                for (int x = 0; x < line.Length; x++)
                {
                    row[data.Columns[x]] = line[x];
                }
                data.Rows.Add(row);
            }
            reader.Close();
            return data;
        }
    }
}
