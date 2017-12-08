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
        private static void createControllerTypeSheet(XLWorkbook workbook, TECTemplates templates)
        {
            IXLWorksheet worksheet = workbook.Worksheets.Add("Controller Types");

            int x = 1, y = 2;
            y = addHardwareHeader(worksheet, x, y).nextColumn;

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
            y = addHardwareHeader(worksheet, x, y).nextColumn;

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
            y = addHardwareHeader(worksheet, x, y).nextColumn;

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
            y = addCostHeader(worksheet, x, y).nextColumn;

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
            y = addCostHeader(worksheet, x, y).nextColumn;

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
            x = addScopeHeader(worksheet, x, y).nextRow;

            foreach(TECSubScope scope in templates.SubScopeTemplates)
            {
                x = addScopeRow(worksheet, scope, x, y).nextRow;
            }

            worksheet.Columns().AdjustToContents();
            worksheet.PageSetup.PageOrientation = XLPageOrientation.Landscape;
            worksheet.PageSetup.FitToPages(1, 1);
        }
        private static void createEquipmentSheet(XLWorkbook workbook, TECTemplates templates)
        {
            IXLWorksheet worksheet = workbook.Worksheets.Add("Equipment");

            int x = 1, y = 2;
            x = addScopeHeader(worksheet, x, y).nextRow;
            foreach(TECEquipment equipment in templates.EquipmentTemplates)
            {

                foreach (TECSubScope scope in equipment.SubScope)
                {
                    y = 2;
                    y = addScopeRow(worksheet, equipment, x, y).nextColumn;
                    x = addScopeRow(worksheet, scope, x, y).nextRow;
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
            x = addScopeHeader(worksheet, x, y).nextRow;
            foreach(TECSystem system in templates.SystemTemplates)
            {
                foreach (TECEquipment equipment in system.Equipment)
                {
                    foreach (TECSubScope scope in equipment.SubScope)
                    {
                        y = 2;
                        y = addScopeRow(worksheet, system, x, y).nextColumn;
                        y = addScopeRow(worksheet, equipment, x, y).nextColumn;
                        x = addScopeRow(worksheet, scope, x, y).nextRow;
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

        private static (int nextRow, int nextColumn) addCostHeader(IXLWorksheet worksheet, int startRow, int startColumn)
        {
            int x = startRow;
            int y = startColumn;
            worksheet.Cell(x, y).Value = "Name";
            worksheet.Cell(x, y).Style.Font.SetBold();
            worksheet.Cell(x, y).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Cell(x, y + 1).Value = "Description";
            worksheet.Cell(x, y + 1).Style.Font.SetBold();
            worksheet.Cell(x, y + 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Cell(x, y + 2).Value = "Cost";
            worksheet.Cell(x, y + 2).Style.Font.SetBold();
            worksheet.Cell(x, y + 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Cell(x, y + 3).Value = "Labor";
            worksheet.Cell(x, y + 3).Style.Font.SetBold();
            worksheet.Cell(x, y + 3).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Cell(x, y + 4).Value = "Type";
            worksheet.Cell(x, y + 4).Style.Font.SetBold();
            worksheet.Cell(x, y + 4).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            return (x + 1, y + 1);
        }
        private static (int nextRow, int nextColumn) addCostRow(IXLWorksheet worksheet, TECCost cost, int startRow, int startColumn)
        {
            int x = startRow;
            int y = startColumn;
            worksheet.Cell(x, y).Value = cost.Name;
            worksheet.Cell(x, y + 1).Value = cost.Description;
            worksheet.Cell(x, y + 2).Value = cost.Cost;
            worksheet.Cell(x, y + 3).Value = cost.Labor;
            worksheet.Cell(x, y + 4).Value = cost.Type;
            return (x + 1, y + 1);
        }

        private static (int nextRow, int nextColumn) addScopeHeader(IXLWorksheet worksheet, int startRow, int startColumn)
        {
            int x = startRow;
            int y = startColumn;
            worksheet.Cell(x, y).Value = "Name";
            worksheet.Cell(x, y).Style.Font.SetBold();
            worksheet.Cell(x, y).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Cell(x, y + 1).Value = "Description";
            worksheet.Cell(x, y + 1).Style.Font.SetBold();
            worksheet.Cell(x, y + 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            return (x + 1, y + 1);
        }
        private static (int nextRow, int nextColumn) addScopeRow(IXLWorksheet worksheet, TECScope scope, int startRow, int startColumn)
        {
            int x = startRow;
            int y = startColumn;
            worksheet.Cell(x, y).Value = scope.Name;
            worksheet.Cell(x, y + 1).Value = scope.Description;
            return (x + 1, y + 1);
        }
    }
}
