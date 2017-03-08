using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using EstimatingLibrary;
using System.Collections.ObjectModel;

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
    public class CSVWriter { 

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
            foreach (TECSystem system in bid.Systems)
            {
                if (system.Equipment.Count > 0)
                {
                    foreach(TECEquipment equipment in system.Equipment)
                    {
                        if (equipment.SubScope.Count > 0)
                        {
                            foreach(TECSubScope subScope in equipment.SubScope)
                            {
                                CsvRow row = new CsvRow();
                                row.Add(system.Name);
                                row.Add(equipment.Name);
                                row.Add(subScope.Name);
                                string deviceString = "";
                                foreach(TECDevice device in subScope.Devices)
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
                                    if (point.Type == PointTypes.AI){ AI++; }
                                    else if (point.Type == PointTypes.BI) { BI++; }
                                    else if (point.Type == PointTypes.AO) { AO++; }
                                    else if (point.Type == PointTypes.BO) { BO++; }
                                    else if (point.Type == PointTypes.Serial) { serial++; }
                                }
                                row.Add(AI.ToString());
                                row.Add(BI.ToString());
                                row.Add(AO.ToString());
                                row.Add(BO.ToString());
                                row.Add(serial.ToString());
                                for(int x = 0; x < 22; x++)
                                {
                                    row.Add("");
                                }
                                row.Add((subScope.Quantity * equipment.Quantity * system.Quantity).ToString());
                                WriteRow(writer, row);
                            }
                        } else
                        {
                            CsvRow row = new CsvRow();
                            row.Add(system.Name);
                            row.Add(equipment.Name);
                            for (int x = 0; x < 29; x++)
                            {
                                row.Add("");
                            }
                            row.Add((equipment.Quantity * system.Quantity).ToString());
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
                    row.Add((system.Quantity).ToString());
                    WriteRow(writer, row);
                }
                
            }
            writer.Close();
        }

        public void BudgetToCSV(ObservableCollection<TECSystem> systems, double margin)
        {
            StreamWriter writer = new StreamWriter(Path);

            List<CsvRow> budgetedRows = new List<CsvRow>();
            List<CsvRow> unbudgetedRows = new List<CsvRow>();

            CsvRow row;
            CsvRow blankRow = new CsvRow();

            double totalPrice = 0;

            foreach (TECSystem system in systems)
            {
                CsvRow systemRow = new CsvRow();
                systemRow.Add(system.Name);
                systemRow.Add("");
                systemRow.Add("");
                systemRow.Add(system.Quantity.ToString());
                if (system.BudgetUnitPrice >= 0)
                {
                    systemRow.Add("$" + system.BudgetUnitPrice.ToString());
                    systemRow.Add("");
                    systemRow.Add("$" + system.TotalBudgetPrice.ToString());
                    budgetedRows.Add(systemRow);

                    foreach (TECEquipment equip in system.Equipment)
                    {
                        CsvRow equipmentRow = new CsvRow();
                        equipmentRow.Add("");
                        equipmentRow.Add(equip.Name);
                        equipmentRow.Add("");
                        equipmentRow.Add(equip.Quantity.ToString());
                        if (equip.BudgetUnitPrice >= 0)
                        {
                            equipmentRow.Add("$" + equip.BudgetUnitPrice.ToString());
                            equipmentRow.Add("$" + equip.TotalBudgetPrice.ToString());
                        }
                        else
                        {
                            equipmentRow.Add("$0");
                            equipmentRow.Add("$0");
                        }
                       
                        budgetedRows.Add(equipmentRow);
                    }

                    totalPrice += system.TotalBudgetPrice;
                }
                else
                {
                    unbudgetedRows.Add(systemRow);

                    foreach (TECEquipment equip in system.Equipment)
                    {
                        CsvRow equipmentRow = new CsvRow();
                        equipmentRow.Add("");
                        equipmentRow.Add(equip.Name);
                        equipmentRow.Add("");
                        equipmentRow.Add(equip.Quantity.ToString());
                        unbudgetedRows.Add(equipmentRow);
                    }
                }
            }

            row = new CsvRow();
            row.Add("Budgeted Systems");
            WriteRow(writer, row);

            WriteRow(writer, blankRow);

            row = new CsvRow();
            row.Add("System");
            row.Add("Equipment");
            row.Add("");
            row.Add("Quantity");
            row.Add("Unit Price");
            row.Add("Equipment Subtotal");
            row.Add("System Subtotal");
            WriteRow(writer, row);

            foreach (CsvRow budgetedRow in budgetedRows)
            {
                WriteRow(writer, budgetedRow);
            }

            WriteRow(writer, blankRow);

            row = new CsvRow();
            row.Add("Subtotal: ");
            row.Add("$" + totalPrice.ToString());
            WriteRow(writer, row);

            WriteRow(writer, blankRow);

            row = new CsvRow();
            row.Add("Margin: ");
            row.Add(margin.ToString() + "%");
            WriteRow(writer, row);

            WriteRow(writer, blankRow);

            totalPrice = totalPrice / (1 - margin);

            row = new CsvRow();
            row.Add("Total: ");
            row.Add("$" + totalPrice.ToString());
            WriteRow(writer, row);

            WriteRow(writer, blankRow);

            
            row = new CsvRow();
            row.Add("Unbudgeted Systems");
            WriteRow(writer, row);

            WriteRow(writer, blankRow);

            row = new CsvRow();
            row.Add("System");
            row.Add("Equipment");
            row.Add("");
            row.Add("Quantity");
            WriteRow(writer, row);

            foreach (CsvRow unbudgetedRow in unbudgetedRows)
            {
                WriteRow(writer, unbudgetedRow);
            }
            WriteRow(writer, blankRow);

            writer.Close();


        }
    }
}
