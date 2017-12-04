using ClosedXML.Excel;
using EstimatingLibrary;
using EstimatingLibrary.Interfaces;
using EstimatingUtilitiesLibrary.SummaryItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingUtilitiesLibrary.Exports
{
    public static class MaterialSummaryExport
    {
        public static void GenerateAndExport(string path, TECBid bid)
        {
            XLWorkbook workbook = new XLWorkbook();
            AddControllersSheet(workbook, bid);

            throw new NotImplementedException();
        }

        internal static void AddControllersSheet(XLWorkbook workbook, TECBid bid, string sheetName = "Controllers")
        {
            List<TECController> controllers = getAllControllers(bid);
            List<HardwareSummaryItem> controllerItems = consolidateHardware(controllers.Select(controller => controller.Type));
            List<CostSummaryItem> costItems = consolidateCostInControllers(controllers);

            IXLWorksheet worksheet = workbook.Worksheets.Add(sheetName);
            int row = 1;

            row = worksheet.insertTitleRow(sheetName, row);
            row++;

            row = worksheet.insertHardwareItemSummary(row, controllerItems);
            row++;

            row = worksheet.insertTitleRow("Associated Costs", row);
            row++;

            row = worksheet.insertCostItemSummary(row, costItems);

            worksheet.Columns().AdjustToContents();
        }
        internal static void AddPanelsSheet(XLWorkbook workbook, TECBid bid, string sheetName = "Panels")
        {
            List<TECPanel> panels = getAllPanels(bid);
            List<HardwareSummaryItem> panelItems = consolidateHardware(panels.Select(panel => panel.Type));
            List<CostSummaryItem> costItems = consolidateCostInPanels(panels);

            IXLWorksheet worksheet = workbook.Worksheets.Add(sheetName);
            int row = 1;

            row = worksheet.insertTitleRow(sheetName, row);
            row++;

            row = worksheet.insertHardwareItemSummary(row, panelItems);
            row++;

            row = worksheet.insertTitleRow("AssociatedCosts", row);
            row++;

            row = worksheet.insertCostItemSummary(row, costItems);

            worksheet.Columns().AdjustToContents();
        }
        internal static void AddDevicesSheet(XLWorkbook workbook, TECBid bid, string sheetName = "Devices")
        {
            List<TECDevice> devices = getAllDevices(bid);
            List<HardwareSummaryItem> deviceItems = consolidateHardware(devices);
            List<CostSummaryItem> costItems = consolidateCostInDevices(devices);

            IXLWorksheet worksheet = workbook.Worksheets.Add(sheetName);
            int row = 1;

            row = worksheet.insertTitleRow(sheetName, row);
            row++;

            row = worksheet.insertHardwareItemSummary(row, deviceItems);
            row++;

            row = worksheet.insertTitleRow("AssociatedCosts", row);
            row++;

            row = worksheet.insertCostItemSummary(row, costItems);

            worksheet.Columns().AdjustToContents();
        }
        internal static void AddValvesSheet(XLWorkbook workbook, TECBid bid, string sheetName = "Valves")
        {
            throw new NotImplementedException();
        }
        internal static void AddElectricalMaterialSheet(XLWorkbook workbook, TECBid bid, string sheetName = "Wire and Conduit")
        {
            throw new NotImplementedException();
        }
        internal static void AddMiscCostsSheet(XLWorkbook workbook, TECBid bid, string sheetName = "Misc Costs")
        {
            throw new NotImplementedException();
        }

        #region Insert to Sheet Methods
        private static int insertHardwareItemSummary(this IXLWorksheet worksheet, int row, IEnumerable<HardwareSummaryItem> hardwareItems)
        {
            insertHardwareHeaders(worksheet, row);
            row++;
            foreach (HardwareSummaryItem item in hardwareItems)
            {
                row = worksheet.insertHardwareItem(item, row);
            }
            return row;
        }
        private static int insertCostItemSummary(this IXLWorksheet worksheet, int row, IEnumerable<CostSummaryItem> costItems)
        {
            insertCostHeaders(worksheet, row);
            row++;
            foreach (CostSummaryItem cost in costItems)
            {
                row = worksheet.insertCostItem(cost, row);
            }
            return row;
        }
        
        private static int insertTitleRow(this IXLWorksheet worksheet, string title, int row)
        {
            IXLCell titleCell = worksheet.Cell(row, "A");
            titleCell.Value = title;
            titleCell.Style.Font.SetBold();
            row++;
            return row;
        }

        private static int insertHardwareHeaders(this IXLWorksheet worksheet, int row)
        {
            IXLRow headerRow = worksheet.Row(row);
            headerRow.Style.Font.SetBold();

            headerRow.Cell("A").Value = "Name";
            headerRow.Cell("B").Value = "Model Number";
            headerRow.Cell("C").Value = "Manufacturer";
            headerRow.Cell("D").Value = "Quantity";
            headerRow.Cell("E").Value = "List Price";
            headerRow.Cell("F").Value = "Unit Cost";
            headerRow.Cell("G").Value = "Total Cost";
            headerRow.Cell("H").Value = "Unit Labor";
            headerRow.Cell("I").Value = "Total Labor";

            row++;
            return row;
        }
        private static int insertCostHeaders(this IXLWorksheet worksheet, int row)
        {
            IXLRow headerRow = worksheet.Row(row);
            headerRow.Style.Font.SetBold();

            headerRow.Cell("A").Value = "Name";
            headerRow.Cell("B").Value = "Quantity";
            headerRow.Cell("C").Value = "Type";
            headerRow.Cell("D").Value = "Unit Cost";
            headerRow.Cell("E").Value = "Total Cost";
            headerRow.Cell("F").Value = "Unit Labor";
            headerRow.Cell("G").Value = "Total Labor";
            row++;

            return row;
        }

        private static int insertHardwareItem(this IXLWorksheet worksheet, HardwareSummaryItem item, int row)
        {
            IXLRow itemRow = worksheet.Row(row);

            itemRow.Cell("A").Value = item.Hardware.Name;
            itemRow.Cell("B").Value = item.Hardware.Description;
            itemRow.Cell("C").Value = item.Hardware.Manufacturer.Label;
            itemRow.Cell("D").Value = item.Quantity;
            itemRow.Cell("E").Value = item.Hardware.Price;
            itemRow.Cell("F").Value = string.Format("{0:C}", item.Hardware.Cost);
            itemRow.Cell("G").Value = string.Format("{0:C}", item.TotalCost);
            itemRow.Cell("H").Value = string.Format("{0:F2}", item.Hardware.Labor);
            itemRow.Cell("I").Value = string.Format("{0:F2}", item.TotalLabor);
            row++;
            return row;
        }
        private static int insertCostItem(this IXLWorksheet worksheet, CostSummaryItem item, int row)
        {
            IXLRow itemRow = worksheet.Row(row);

            itemRow.Cell("A").Value = item.Cost.Name;
            itemRow.Cell("B").Value = item.Quantity;
            itemRow.Cell("C").Value = item.Cost.Type;
            itemRow.Cell("D").Value = string.Format("{0:C}", item.Cost.Cost);
            itemRow.Cell("E").Value = string.Format("{0:C}", item.TotalCost);
            itemRow.Cell("F").Value = string.Format("{0:F2}", item.Cost.Labor);
            itemRow.Cell("G").Value = string.Format("{0:F2}", item.TotalLabor);
            row++;
            return row;
        }
        #endregion

        #region Get Material Methods
        private static List<TECController> getAllControllers(this TECBid bid)
        {
            List<TECController> controllers = new List<TECController>();
            controllers.AddRange(bid.Controllers);
            foreach(TECTypical typ in bid.Systems)
            {
                foreach(TECSystem sys in typ.Instances)
                {
                    controllers.AddRange(sys.Controllers);
                }
            }
            return controllers;
        }
        private static List<TECPanel> getAllPanels(this TECBid bid)
        {
            List<TECPanel> panels = new List<TECPanel>();
            panels.AddRange(bid.Panels);
            foreach(TECTypical typ in bid.Systems)
            {
                foreach(TECSystem sys in typ.Instances)
                {
                    panels.AddRange(sys.Panels);
                }
            }
            return panels;
        }
        private static List<TECDevice> getAllDevices(this TECBid bid)
        {
            List<TECDevice> devices = new List<TECDevice>();
            foreach(TECTypical typ in bid.Systems)
            {
                foreach (TECSystem instance in typ.Instances)
                {
                    foreach (TECEquipment equip in instance.Equipment)
                    {
                        foreach (TECSubScope ss in equip.SubScope)
                        {
                            foreach (IEndDevice endDev in ss.Devices)
                            {
                                if (endDev is TECDevice dev)
                                {
                                    devices.Add(dev);
                                }
                            }
                        }
                    }
                }
            }
            return devices;
        }

        private static List<HardwareSummaryItem> consolidateHardware(IEnumerable<TECHardware> hardware)
        {
            Dictionary<TECHardware, HardwareSummaryItem> dictionary = new Dictionary<TECHardware, HardwareSummaryItem>();
            List<HardwareSummaryItem> items = new List<HardwareSummaryItem>();

            foreach(TECHardware hw in hardware)
            {
                if (dictionary.ContainsKey(hw))
                {
                    dictionary[hw].Increment();
                }
                else
                {
                    HardwareSummaryItem item = new HardwareSummaryItem(hw);
                    dictionary.Add(hw, item);
                    items.Add(item);
                }
            }

            return items;
        }
        private static List<CostSummaryItem> consolidateCostInControllers(IEnumerable<TECController> controllers)
        {
            Dictionary<TECCost, CostSummaryItem> dictionary = new Dictionary<TECCost, CostSummaryItem>();
            List<CostSummaryItem> items = new List<CostSummaryItem>();

            List<TECCost> costs = new List<TECCost>();
            foreach(TECController controller in controllers)
            {
                costs.AddRange(controller.AssociatedCosts);
                costs.AddRange(controller.Type.AssociatedCosts);
            }

            foreach (TECCost cost in costs)
            {
                if (dictionary.ContainsKey(cost))
                {
                    dictionary[cost].AddQuantity(1);
                }
                else
                {
                    CostSummaryItem item = new CostSummaryItem(cost);
                    dictionary.Add(cost, item);
                    items.Add(item);
                }
            }

            return items;
        }
        private static List<CostSummaryItem> consolidateCostInPanels(IEnumerable<TECPanel> panels)
        {
            Dictionary<TECCost, CostSummaryItem> dictionary = new Dictionary<TECCost, CostSummaryItem>();
            List<CostSummaryItem> items = new List<CostSummaryItem>();

            List<TECCost> costs = new List<TECCost>();
            foreach (TECPanel panel in panels)
            {
                costs.AddRange(panel.AssociatedCosts);
                costs.AddRange(panel.Type.AssociatedCosts);
            }

            foreach (TECCost cost in costs)
            {
                if (dictionary.ContainsKey(cost))
                {
                    dictionary[cost].AddQuantity(1);
                }
                else
                {
                    CostSummaryItem item = new CostSummaryItem(cost);
                    dictionary.Add(cost, item);
                    items.Add(item);
                }
            }

            return items;
        }
        private static List<CostSummaryItem> consolidateCostInDevices(IEnumerable<TECDevice> devices)
        {
            Dictionary<TECCost, CostSummaryItem> dictionary = new Dictionary<TECCost, CostSummaryItem>();
            List<CostSummaryItem> items = new List<CostSummaryItem>();

            List<TECCost> costs = new List<TECCost>();
            foreach(TECDevice device in devices)
            {
                costs.AddRange(device.AssociatedCosts);
            }

            foreach(TECCost cost in costs)
            {
                if (dictionary.ContainsKey(cost))
                {
                    dictionary[cost].AddQuantity(1);
                }
                else
                {
                    CostSummaryItem item = new CostSummaryItem(cost);
                    dictionary.Add(cost, item);
                    items.Add(item);
                }
            }
            return items;
        }
        #endregion

    }
}
