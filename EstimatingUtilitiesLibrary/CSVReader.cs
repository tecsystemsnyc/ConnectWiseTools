using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingUtilitiesLibrary
{
    public static class CSVReader
    {
        public static DataTable Read(string path)
        {
            DataTable data = new DataTable();
            StreamReader reader = new StreamReader(path);
            string header = reader.ReadLine();
            string[] columns = header.Split(',');
            foreach(string column in columns)
            {
                data.Columns.Add(column);
            }
            while (!reader.EndOfStream)
            {
                string[] line = reader.ReadLine().Split(',');
                DataRow row = data.NewRow();
                for (int x = 0; x < line.Length; x++)
                {
                    row[data.Columns[x]] = line[x];
                }
            }
            reader.Close();
            return data;
        }
    }
}
