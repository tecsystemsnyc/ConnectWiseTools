using ClosedXML.Excel;
using EstimatingLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingUtilitiesLibrary.Exports
{
    public static class PointsList
    {
        public static void ExportPointsList(string path, TECBid bid, bool openOnComplete = true)
        {
            XLWorkbook workbook = GeneratePointsList(bid);
            workbook.SaveAs(path);
            if (openOnComplete)
            {
                System.Diagnostics.Process.Start(path);
            }
        }

        public static XLWorkbook GeneratePointsList(TECBid bid)
        {
            XLWorkbook workbook = new XLWorkbook();

            IXLWorksheet worksheet = workbook.Worksheets.Add("Points List");
            int row = 1;

            IXLCell titleCell = worksheet.Cell(row, "A");
            titleCell.Value = "Points List";
            titleCell.Style.Font.SetBold();

            row += 2;

            worksheet.addBySystemHeaderRow(row);

            row++;

            bool rowWritten = false;
            IXLRow xlRow = worksheet.Row(row);
            foreach (TECTypical typ in bid.Systems)
            {
                foreach(TECSystem sys in typ.Instances)
                {
                    xlRow.Cell("A").Value = sys.Name;
                    rowWritten = true;

                    foreach (TECEquipment equip in sys.Equipment)
                    {
                        xlRow.Cell("B").Value = equip.Name;
                        rowWritten = true;

                        foreach (TECSubScope ss in equip.SubScope)
                        {
                            xlRow.Cell("C").Value = ss.Name;

                            string deviceString = "";
                            foreach (TECDevice device in ss.Devices)
                            {
                                deviceString += " (";
                                deviceString += device.Name;
                                deviceString += ") ";
                            }
                            xlRow.Cell("D").Value = deviceString;
                            rowWritten = true;

                            foreach (TECPoint point in ss.Points)
                            {
                                xlRow.Cell("E").Value = point.Label;
                                if (TECIO.NetworkIO.Contains(point.Type))
                                {
                                    xlRow.Cell("F").Value = "Serial";
                                }
                                else
                                {
                                    xlRow.Cell("F").Value = point.Type.ToString();
                                }
                                xlRow.Cell("G").Value = point.Quantity;
                                rowWritten = true;

                                if (rowWritten)
                                {
                                    xlRow = xlRow.RowBelow();
                                    rowWritten = false;
                                }
                            }
                            if (rowWritten)
                            {
                                xlRow = xlRow.RowBelow();
                                rowWritten = false;
                            }
                        }
                        if (rowWritten)
                        {
                            xlRow = xlRow.RowBelow();
                            rowWritten = false;
                        }
                    }
                    xlRow = xlRow.RowBelow();
                }
            }
            worksheet.Columns().AdjustToContents();

            return workbook;
        }

        private static void addBySystemHeaderRow(this IXLWorksheet worksheet, int row)
        {
            IXLRow headerRow = worksheet.Row(row);
            headerRow.Style.Font.SetBold();

            headerRow.Cell("A").Value = "System";
            headerRow.Cell("B").Value = "Equipment";
            headerRow.Cell("C").Value = "Point";
            headerRow.Cell("D").Value = "Devices";
            headerRow.Cell("E").Value = "IO";
            headerRow.Cell("F").Value = "Type";
            headerRow.Cell("G").Value = "Quantity";
        }
    }
}
