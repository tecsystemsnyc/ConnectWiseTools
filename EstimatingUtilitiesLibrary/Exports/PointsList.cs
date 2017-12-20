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
        public static XLWorkbook GeneratePointsList(TECBid bid)
        {
            XLWorkbook workbook = new XLWorkbook();

            IXLWorksheet workSheet = workbook.Worksheets.Add("Points List");
            int row = 1;

            IXLCell titleCell = workSheet.Cell(row, "A");
            titleCell.Value = "Points List";
            titleCell.Style.Font.SetBold();

            row += 2;

            workSheet.addBySystemHeaderRow(row);

            row++;

            IXLRow xlRow = workSheet.Row(row);
            foreach (TECTypical typ in bid.Systems)
            {
                foreach(TECSystem sys in typ.Instances)
                {
                    xlRow.Cell("A").Value = sys.Name;
                    foreach(TECEquipment equip in sys.Equipment)
                    {
                        xlRow.Cell("B").Value = equip.Name;
                        foreach(TECSubScope ss in equip.SubScope)
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

                                xlRow = xlRow.RowBelow();
                            }
                            xlRow = xlRow.RowBelow();
                        }
                        xlRow = xlRow.RowBelow();
                    }
                    xlRow = xlRow.RowBelow();
                }
            }

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
