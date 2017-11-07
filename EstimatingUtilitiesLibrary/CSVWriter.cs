using EstimatingLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EstimatingUtilitiesLibrary
{
    /// <summary>
    /// Class to store one CSV row
    /// </summary>
    public class CsvRow : List<string>
    {
        public string LineText { get; set; }
    }

    /// <summary>
    /// Class to write to a CSV
    /// </summary>
    public class CSVWriter
    {

        public string Path { get; set; }

        public CSVWriter(String path)
        {
            Path = path;
        }

        public void WriteRow(StreamWriter writer, CsvRow row)
        {
            StringBuilder builder = new StringBuilder();
            bool firstColumn = true;
            foreach (string value in row)
            {
                // Add separator if this isn't the first value
                if (!firstColumn)
                    builder.Append(',');
                // Implement special handling for values that contain comma or quote
                // Enclose in quotes and double up any double quotes
                if (value.IndexOfAny(new char[] { '"', ',' }) != -1)
                    builder.AppendFormat("\"{0}\"", value.Replace("\"", "\"\""));
                else
                    builder.Append(value);
                firstColumn = false;
            }
            row.LineText = builder.ToString();
            writer.WriteLine(row.LineText);
        }

        public void BidPointsToCSV(TECBid bid)
        {
            StreamWriter writer = new StreamWriter(Path);
            foreach (TECTypical system in bid.Systems)
            {
                if (system.Equipment.Count > 0)
                {
                    foreach (TECEquipment equipment in system.Equipment)
                    {
                        if (equipment.SubScope.Count > 0)
                        {
                            foreach (TECSubScope subScope in equipment.SubScope)
                            {
                                CsvRow row = new CsvRow();
                                row.Add(system.Name);
                                row.Add(equipment.Name);
                                row.Add(subScope.Name);
                                string deviceString = "";
                                foreach (TECDevice device in subScope.Devices)
                                {
                                    deviceString += " (";
                                    deviceString += device.Name;
                                    deviceString += ") ";
                                }
                                row.Add(deviceString);
                                int AI = 0;
                                int BI = 0;
                                int AO = 0;
                                int BO = 0;
                                int serial = 0;

                                foreach (TECPoint point in subScope.Points)
                                {
                                    if (point.Type == IOType.AI) { AI += point.Quantity; }
                                    else if (point.Type == IOType.DI) { BI += point.Quantity; }
                                    else if (point.Type == IOType.AO) { AO += point.Quantity; }
                                    else if (point.Type == IOType.DO) { BO += point.Quantity; }
                                    else if (TECIO.NetworkIO.Contains(point.Type) ){ serial += point.Quantity; }
                                }
                                row.Add(AI.ToString());
                                row.Add(BI.ToString());
                                row.Add(AO.ToString());
                                row.Add(BO.ToString());
                                row.Add(serial.ToString());
                                for (int x = 0; x < 22; x++)
                                {
                                    row.Add("");
                                }
                                row.Add((system.Instances.Count).ToString());
                                WriteRow(writer, row);
                            }
                        }
                        else
                        {
                            CsvRow row = new CsvRow();
                            row.Add(system.Name);
                            row.Add(equipment.Name);
                            for (int x = 0; x < 29; x++)
                            {
                                row.Add("");
                            }
                            row.Add((system.Instances.Count).ToString());
                            WriteRow(writer, row);
                        }
                    }
                }
                else
                {
                    CsvRow row = new CsvRow();
                    row.Add(system.Name);
                    for (int x = 0; x < 30; x++)
                    {
                        row.Add("");
                    }
                    row.Add((system.Instances.Count).ToString());
                    WriteRow(writer, row);
                }

            }
            writer.Close();
        }


    }
}
