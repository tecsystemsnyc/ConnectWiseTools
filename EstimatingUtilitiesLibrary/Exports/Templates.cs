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
            createValvesSheet(workbook, templates);
            createControllerTypeSheet(workbook, templates);
            createPanelTypeSheet(workbook, templates);
            createIOModuleSheet(workbook, templates);
            createAssociatedCostSheet(workbook, templates);
            createConnectionTypesSheet(workbook, templates);
            createConduitTypesSheet(workbook, templates);
            createSubScopeSheet(workbook, templates);
            createEquipmentSheet(workbook, templates);
            createSystemSheet(workbook, templates);

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
                int rowY = startY;
                rowY = addHardwareRow(worksheet, device, x, rowY).nextColumn;
                x = addEndDeviceRow(worksheet, device, x, rowY).nextRow;
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
            y = addEndDeviceHeader(worksheet, x, y).nextColumn;
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
        private static void createControllerTypeSheet(XLWorkbook workbook, TECTemplates templates)
        {
            IXLWorksheet worksheet = workbook.Worksheets.Add("Controller Types");

            int x = 1, y = 2;
            (x, y) = addHardwareHeader(worksheet, x, y);

            int startY = 2;
            foreach (TECControllerType item in templates.Catalogs.ControllerTypes)
            {
                int rowY = startY;
                x = addHardwareRow(worksheet, item, x, rowY).nextRow;
            }

            worksheet.Columns().AdjustToContents();
            worksheet.PageSetup.PageOrientation = XLPageOrientation.Landscape;
            worksheet.PageSetup.FitToPages(1, 1);
        }
        private static void createPanelTypeSheet(XLWorkbook workbook, TECTemplates templates)
        {
            IXLWorksheet worksheet = workbook.Worksheets.Add("Panel Types");

            int x = 1, y = 2;
            (x, y) = addHardwareHeader(worksheet, x, y);

            int startY = 2;
            foreach (TECPanelType item in templates.Catalogs.PanelTypes)
            {
                int rowY = startY;
                x = addHardwareRow(worksheet, item, x, rowY).nextRow;
            }

            worksheet.Columns().AdjustToContents();
            worksheet.PageSetup.PageOrientation = XLPageOrientation.Landscape;
            worksheet.PageSetup.FitToPages(1, 1);
        }
        private static void createIOModuleSheet(XLWorkbook workbook, TECTemplates templates)
        {
            IXLWorksheet worksheet = workbook.Worksheets.Add("IO Modules");

            int x = 1, y = 2;
            (x,y) = addHardwareHeader(worksheet, x, y);

            int startY = 2;
            foreach (TECIOModule item in templates.Catalogs.IOModules)
            {
                int rowY = startY;
                x = addHardwareRow(worksheet, item, x, rowY).nextRow;
            }

            worksheet.Columns().AdjustToContents();
            worksheet.PageSetup.PageOrientation = XLPageOrientation.Landscape;
            worksheet.PageSetup.FitToPages(1, 1);
        }
        private static void createAssociatedCostSheet(XLWorkbook workbook, TECTemplates templates)
        {
            IXLWorksheet worksheet = workbook.Worksheets.Add("AssociatedCosts");

            int x = 1, y = 2;
            (x, y) = addCostHeader(worksheet, x, y);

            int startY = 2;
            foreach (TECCost item in templates.Catalogs.AssociatedCosts)
            {
                int rowY = startY;
                x = addCostRow(worksheet, item, x, rowY).nextRow;
            }

            worksheet.Columns().AdjustToContents();
            worksheet.PageSetup.PageOrientation = XLPageOrientation.Landscape;
            worksheet.PageSetup.FitToPages(1, 1);
        }
        private static void createConnectionTypesSheet(XLWorkbook workbook, TECTemplates templates)
        {
            IXLWorksheet worksheet = workbook.Worksheets.Add("Connection Types");

            int x = 1, y = 2;
            (x, y) = addCostHeader(worksheet, x, y);

            int startY = 2;
            foreach (TECConnectionType item in templates.Catalogs.ConnectionTypes)
            {
                int rowY = startY;
                x = addCostRow(worksheet, item, x, rowY).nextRow;
            }

            worksheet.Columns().AdjustToContents();
            worksheet.PageSetup.PageOrientation = XLPageOrientation.Landscape;
            worksheet.PageSetup.FitToPages(1, 1);
        }
        private static void createConduitTypesSheet(XLWorkbook workbook, TECTemplates templates)
        {
            IXLWorksheet worksheet = workbook.Worksheets.Add("Conduit Types");

            int x = 1, y = 2;
            (x, y) = addCostHeader(worksheet, x, y);

            int startY = 2;
            foreach (TECElectricalMaterial item in templates.Catalogs.ConduitTypes)
            {
                int rowY = startY;
                x = addCostRow(worksheet, item, x, rowY).nextRow;
            }

            worksheet.Columns().AdjustToContents();
            worksheet.PageSetup.PageOrientation = XLPageOrientation.Landscape;
            worksheet.PageSetup.FitToPages(1, 1);
        }
        private static void createSubScopeSheet(XLWorkbook workbook, TECTemplates templates)
        {
            IXLWorksheet worksheet = workbook.Worksheets.Add("Points");

            int x = 1, y = 2;
            x = addSubScopeHeader(worksheet, x, y).nextRow;

            foreach(TECSubScope scope in templates.SubScopeTemplates)
            {
                x = addSubScopeRow(worksheet, scope, x, y).nextRow;
            }

            worksheet.Columns().AdjustToContents();
            worksheet.PageSetup.PageOrientation = XLPageOrientation.Landscape;
            worksheet.PageSetup.FitToPages(1, 1);
        }
        private static void createEquipmentSheet(XLWorkbook workbook, TECTemplates templates)
        {
            IXLWorksheet worksheet = workbook.Worksheets.Add("Equipment");

            int x = 1, y = 2;
            y = addScopeHeader(worksheet, x, y).nextColumn;
            x = addSubScopeHeader(worksheet, x, y).nextRow;
            foreach(TECEquipment equipment in templates.EquipmentTemplates)
            {

                foreach (TECSubScope scope in equipment.SubScope)
                {
                    y = 2;
                    y = addScopeRow(worksheet, equipment, x, y).nextColumn;
                    x = addSubScopeRow(worksheet, scope, x, y).nextRow;
                }
            }
            

            worksheet.Columns().AdjustToContents();
            worksheet.PageSetup.PageOrientation = XLPageOrientation.Landscape;
            worksheet.PageSetup.FitToPages(1, 1);
        }
        private static void createSystemSheet(XLWorkbook workbook, TECTemplates templates)
        {
            IXLWorksheet worksheet = workbook.Worksheets.Add("Systems");

            int x = 1, y = 2;
            y = addScopeHeader(worksheet, x, y).nextColumn;
            y = addScopeHeader(worksheet, x, y).nextColumn;
            x = addSubScopeHeader(worksheet, x, y).nextRow;
            foreach (TECSystem system in templates.SystemTemplates)
            {
                foreach (TECEquipment equipment in system.Equipment)
                {
                    foreach (TECSubScope scope in equipment.SubScope)
                    {
                        y = 2;
                        y = addScopeRow(worksheet, system, x, y).nextColumn;
                        y = addScopeRow(worksheet, equipment, x, y).nextColumn;
                        x = addSubScopeRow(worksheet, scope, x, y).nextRow;
                    }
                }
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
            y++;
            worksheet.Cell(x, y).Value = "Description";
            worksheet.Cell(x, y).Style.Font.SetBold();
            worksheet.Cell(x, y ).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            y++;
            worksheet.Cell(x, y ).Value = "Manufacturer";
            worksheet.Cell(x, y ).Style.Font.SetBold();
            worksheet.Cell(x, y ).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            y++;
            worksheet.Cell(x, y ).Value = "List Price";
            worksheet.Cell(x, y ).Style.Font.SetBold();
            worksheet.Cell(x, y ).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            y++;
            worksheet.Cell(x, y ).Value = "Cost";
            worksheet.Cell(x, y ).Style.Font.SetBold();
            worksheet.Cell(x, y ).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            y++;
            return (x + 1, y);
        }
        private static (int nextRow, int nextColumn) addHardwareRow(IXLWorksheet worksheet, TECHardware hardware, int startRow, int startColumn)
        {
            int x = startRow;
            int y = startColumn;
            worksheet.Cell(x, y).Value = hardware.Name;
            y++;
            worksheet.Cell(x, y).Value = hardware.Description;
            y++;
            worksheet.Cell(x, y).Value = hardware.Manufacturer.Label;
            y++;
            worksheet.Cell(x, y).Value = hardware.Price;
            y++;
            worksheet.Cell(x, y).Value = hardware.Cost;
            y++;
            return (x + 1, y);
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
                connectionString += String.Format("({0} Qty. {1})", type.Name,
                        endDevice.ConnectionTypes.Count(item => item == type));
            }
            worksheet.Cell(x, y).Value = connectionString;
            return (x + 1, y + 1);
        }

        private static (int nextRow, int nextColumn) addCostHeader(IXLWorksheet worksheet, int startRow, int startColumn)
        {
            int x = startRow;
            int y = startColumn;
            worksheet.Cell(x, y).Value = "Name";
            worksheet.Cell(x, y).Style.Font.SetBold();
            worksheet.Cell(x, y).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            y++;
            worksheet.Cell(x, y ).Value = "Description";
            worksheet.Cell(x, y ).Style.Font.SetBold();
            worksheet.Cell(x, y ).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            y++;
            worksheet.Cell(x, y ).Value = "Cost";
            worksheet.Cell(x, y ).Style.Font.SetBold();
            worksheet.Cell(x, y).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            y++;
            worksheet.Cell(x, y ).Value = "Labor";
            worksheet.Cell(x, y ).Style.Font.SetBold();
            worksheet.Cell(x, y ).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            y++;
            worksheet.Cell(x, y).Value = "Type";
            worksheet.Cell(x, y).Style.Font.SetBold();
            worksheet.Cell(x, y ).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            y++;
            return (x + 1, y);
        }
        private static (int nextRow, int nextColumn) addCostRow(IXLWorksheet worksheet, TECCost cost, int startRow, int startColumn)
        {
            int x = startRow;
            int y = startColumn;
            worksheet.Cell(x, y).Value = cost.Name;
            y++;
            worksheet.Cell(x, y ).Value = cost.Description;
            y++;
            worksheet.Cell(x, y).Value = cost.Cost;
            y++;
            worksheet.Cell(x, y).Value = cost.Labor;
            y++;
            worksheet.Cell(x, y ).Value = cost.Type;
            y++;
            return (x + 1, y);
        }

        private static (int nextRow, int nextColumn) addScopeHeader(IXLWorksheet worksheet, int startRow, int startColumn)
        {
            int x = startRow;
            int y = startColumn;
            worksheet.Cell(x, y).Value = "Name";
            worksheet.Cell(x, y).Style.Font.SetBold();
            worksheet.Cell(x, y).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            y++;
            worksheet.Cell(x, y ).Value = "Description";
            worksheet.Cell(x, y ).Style.Font.SetBold();
            worksheet.Cell(x, y ).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            y++;
            return (x + 1, y );
        }
        private static (int nextRow, int nextColumn) addScopeRow(IXLWorksheet worksheet, TECScope scope, int startRow, int startColumn)
        {
            int x = startRow;
            int y = startColumn;
            worksheet.Cell(x, y).Value = scope.Name;
            y++;
            worksheet.Cell(x, y).Value = scope.Description;
            return (x + 1, y + 1);
        }

        private static (int nextRow, int nextColumn) addSubScopeHeader(IXLWorksheet worksheet, int startRow, int startColumn)
        {
            int x = startRow;
            int y = startColumn;
            y = addScopeHeader(worksheet, startRow, startColumn).nextColumn;
            worksheet.Cell(x, y).Value = "Devices";
            worksheet.Cell(x, y).Style.Font.SetBold();
            worksheet.Cell(x, y).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            y++;
            worksheet.Cell(x, y).Value = "IO";
            worksheet.Cell(x, y).Style.Font.SetBold();
            worksheet.Cell(x, y).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            y++;
            return (x + 1, y);
        }
        private static (int nextRow, int nextColumn) addSubScopeRow(IXLWorksheet worksheet, TECSubScope subScope, int startRow, int startColumn)
        {
            int x = startRow;
            int y = startColumn;
            y = addScopeRow(worksheet, subScope, x, y).nextColumn;
            string deviceString = "";
            foreach(IEndDevice device in subScope.Devices.Distinct())
            {
                deviceString += String.Format("({0} Qty. {1})", device.Name,
                        subScope.Devices.Count(item => item == device));
            }
            worksheet.Cell(x, y).Value = deviceString;
            y++;
            string pointsString = "";
            foreach (TECPoint point in subScope.Points)
            {
                pointsString += String.Format("({0})", pointString(point));
            }
            worksheet.Cell(x, y).Value = pointsString;
            return (x + 1, y + 1);
        }

        private static string pointString(TECPoint point)
        {
            string returnString = String.Format("{0}: {1} {2}",
                point.Label, point.Quantity, point.Type);
            return returnString;
        }


    }
}
