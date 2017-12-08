using ClosedXML.Excel;
using EstimatingLibrary;
using EstimatingLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingUtilitiesLibrary.Exports
{
    public class Templates
    {
        public static void Export(string path, TECTemplates templates, bool openOnComplete = true)
        {
            XLWorkbook workbook = new XLWorkbook();
            createDeviceSheet(workbook, templates);
            
            workbook.SaveAs(path);
            if (openOnComplete)
            {
                System.Diagnostics.Process.Start(path);
            }
        }

        private static void createDeviceSheet(XLWorkbook workbook, TECTemplates templates)
        {
            IXLWorksheet worksheet = workbook.Worksheets.Add("Devices");

            int x = 1, y = 2;
            y = addHardwareHeader(worksheet, x, y).nextColumn;
            (x, y)= addEndDeviceHeader(worksheet, x, y);

            int startY = 2;
            foreach (TECDevice device in templates.Catalogs.Devices)
            {
                y = addHardwareRow(worksheet, device, x, y).nextColumn;
                x = addEndDeviceRow(worksheet, device, x, y).nextRow;
            }

            worksheet.Columns().AdjustToContents();
            worksheet.PageSetup.PageOrientation = XLPageOrientation.Landscape;
            worksheet.PageSetup.FitToPages(1, 1);
        }
        private static void createValvesSheet(XLWorkbook workbook, TECTemplates templates)
        {
            IXLWorksheet worksheet = workbook.Worksheets.Add("Valves");

            int x = 1, y = 2;
            y = addHardwareHeader(worksheet, x, y).nextColumn;
            (x, y) = addEndDeviceHeader(worksheet, x, y);
            worksheet.Cell(x, y).Value = "Actuator";
            worksheet.Cell(x, y).Style.Font.SetBold();
            worksheet.Cell(x, y).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            x++;
            int startY = 2;
            foreach (TECValve valve in templates.Catalogs.Valves)
            {
                int rowY = startY;
                startY = addHardwareRow(worksheet, valve, x, startY).nextColumn;
                startY = addEndDeviceRow(worksheet, valve, x, startY).nextColumn;
                worksheet.Cell(x, startY).Value = valve.Actuator.Name;
                x++;
            }

            worksheet.Columns().AdjustToContents();
            worksheet.PageSetup.PageOrientation = XLPageOrientation.Landscape;
            worksheet.PageSetup.FitToPages(1, 1);
        }

        private static (int nextRow, int nextColumn) addHardwareHeader(IXLWorksheet worksheet, int startRow, int startColumn)
        {
            int x = startRow;
            int y = startColumn;
            worksheet.Cell(x, y).Value = "Name";
            worksheet.Cell(x, y).Style.Font.SetBold();
            worksheet.Cell(x, y).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Cell(x, y + 1).Value = "Description";
            worksheet.Cell(x, y + 1).Style.Font.SetBold();
            worksheet.Cell(x, y + 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Cell(x, y + 2).Value = "Manufacturer";
            worksheet.Cell(x, y + 2).Style.Font.SetBold();
            worksheet.Cell(x, y + 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Cell(x, y + 3).Value = "List Price";
            worksheet.Cell(x, y + 3).Style.Font.SetBold();
            worksheet.Cell(x, y + 3).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Cell(x, y + 4).Value = "Cost";
            worksheet.Cell(x, y + 4).Style.Font.SetBold();
            worksheet.Cell(x, y + 4).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            return (x + 1, y + 1);
        }
        private static (int nextRow, int nextColumn) addHardwareRow(IXLWorksheet worksheet, TECHardware hardware, int startRow, int startColumn)
        {
            int x = startRow;
            int y = startColumn;
            worksheet.Cell(x, y).Value = hardware.Name;
            worksheet.Cell(x, y + 1).Value = hardware.Description;
            worksheet.Cell(x, y + 2).Value = hardware.Manufacturer.Label;
            worksheet.Cell(x, y + 3).Value = hardware.Price;
            worksheet.Cell(x, y + 4).Value = hardware.Cost;
            return (x + 1, y + 1);
        }

        private static (int nextRow, int nextColumn) addEndDeviceHeader(IXLWorksheet worksheet, int startRow, int startColumn)
        {
            int x = startRow;
            int y = startColumn;
            worksheet.Cell(x, y).Value = "Connection Types";
            worksheet.Cell(x, y).Style.Font.SetBold();
            worksheet.Cell(x, y).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            return (x + 1, y + 1);
        }
        private static (int nextRow, int nextColumn) addEndDeviceRow(IXLWorksheet worksheet, IEndDevice endDevice, int startRow, int startColumn)
        {
            int x = startRow;
            int y = startColumn;
            string connectionString = "";
            foreach(TECConnectionType type in endDevice.ConnectionTypes.Distinct())
            {
                connectionString += String.Format("({0} Qty. {1})", type,
                        endDevice.ConnectionTypes.Count(item => item == type));
            }
            worksheet.Cell(x, y).Value = connectionString;
            return (x + 1, y + 1);
        }
    }
}
