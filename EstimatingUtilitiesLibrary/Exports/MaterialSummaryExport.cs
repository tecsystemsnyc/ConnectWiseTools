using ClosedXML.Excel;
using EstimatingLibrary;
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

            IXLCell titleCell = worksheet.Cell(1, "A");
            titleCell.Value = sheetName;
            titleCell.Style.Font.SetBold();

            int row = 3;
            insertHardwareHeaders(worksheet, row);
            row++;
            foreach(HardwareSummaryItem controller in controllerItems)
            {
                insertHardwareItem(controller, worksheet, row);
                row++;
            }

            row += 2;
            insertCostHeaders(worksheet, row);
            row++;
            foreach(CostSummaryItem cost in costItems)
            {
                insertCostItem(cost, worksheet, row);
            }
        }
        internal static void AddPanelsSheet(XLWorkbook workbook, TECBid bid, string sheetName = "Panels")
        {
            throw new NotImplementedException();
        }
        internal static void AddDevicesSheet(XLWorkbook workbook, TECBid bid, string sheetName = "Devices")
        {
            IXLWorksheet worksheet = workbook.Worksheets.Add(sheetName);

            throw new NotImplementedException();
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

        #region Add Row Methods
        private static void insertHardwareHeaders(IXLWorksheet worksheet, int row)
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
        }
        private static void insertCostHeaders(IXLWorksheet worksheet, int row)
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
        }

        private static void insertHardwareItem(HardwareSummaryItem item, IXLWorksheet worksheet, int row)
        {
            IXLRow itemRow = worksheet.Row(row);

            itemRow.Cell("A").Value = item.Hardware.Name;
            itemRow.Cell("B").Value = item.Hardware.Description;
            itemRow.Cell("C").Value = item.Hardware.Manufacturer;
            itemRow.Cell("D").Value = item.Quantity;
            itemRow.Cell("E").Value = item.Hardware.Price;
            itemRow.Cell("F").Value = item.Hardware.Cost;
            itemRow.Cell("G").Value = item.TotalCost;
            itemRow.Cell("H").Value = item.Hardware.Labor;
            itemRow.Cell("I").Value = item.TotalLabor;
        }
        private static void insertCostItem(CostSummaryItem item, IXLWorksheet worksheet, int row)
        {
            IXLRow itemRow = worksheet.Row(row);

            itemRow.Cell("A").Value = item.Cost.Name;
            itemRow.Cell("B").Value = item.Quantity;
            itemRow.Cell("C").Value = item.Cost.Type;
            itemRow.Cell("D").Value = item.Cost.Cost;
            itemRow.Cell("E").Value = item.TotalCost;
            itemRow.Cell("F").Value = item.Cost.Labor;
            itemRow.Cell("G").Value = item.TotalLabor;
        }
        #endregion

        #region Get Material Methods
        private static List<TECController> getAllControllers(TECBid bid)
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
        #endregion

    }
}
