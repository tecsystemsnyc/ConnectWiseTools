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

            row = worksheet.insertHardwareHeaders(row);
            foreach(HardwareSummaryItem item in controllerItems)
            {
                row = worksheet.insertHardwareItem(item, row);
            }
            row++;

            row = worksheet.insertTitleRow("Associated Costs", row);
            row++;

            row = worksheet.insertCostHeaders(row);
            foreach(CostSummaryItem item in costItems)
            {
                row = worksheet.insertCostItem(item, row);
            }
            row++;

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

            row = worksheet.insertHardwareHeaders(row);
            foreach(HardwareSummaryItem item in panelItems)
            {
                row = worksheet.insertHardwareItem(item, row);
            }
            row++;

            row = worksheet.insertTitleRow("AssociatedCosts", row);
            row++;

            row = worksheet.insertCostHeaders(row);
            foreach(CostSummaryItem item in costItems)
            {
                row = worksheet.insertCostItem(item, row);
            }
            row++;

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

            row = worksheet.insertHardwareHeaders(row);
            foreach(HardwareSummaryItem item in deviceItems)
            {
                worksheet.insertHardwareItem(item, row);
            }
            row++;

            row = worksheet.insertTitleRow("AssociatedCosts", row);
            row++;

            row = worksheet.insertCostHeaders(row);
            foreach(CostSummaryItem item in costItems)
            {
                worksheet.insertCostItem(item, row);
            }
            row++;

            worksheet.Columns().AdjustToContents();
        }
        internal static void AddValvesSheet(XLWorkbook workbook, TECBid bid, string sheetName = "Valves")
        {
            List<TECValve> valves = getAllValves(bid);
            List<HardwareSummaryItem> valveItems = consolidateHardware(valves);
            List<HardwareSummaryItem> actuatorItems = consolidateHardware(valves.Select(valve => (valve.Actuator)));
            List<CostSummaryItem> costItems = consolidateCostInValves(valves);

            IXLWorksheet worksheet = workbook.Worksheets.Add(sheetName);
            int row = 1;

            row = worksheet.insertTitleRow(sheetName, row);
            row++;

            row = worksheet.insertHardwareHeaders(row);
            foreach(HardwareSummaryItem item in valveItems)
            {
                row = worksheet.insertHardwareItem(item, row);
            }
            row++;

            row = worksheet.insertTitleRow("Actuators", row);
            row++;

            row = worksheet.insertHardwareHeaders(row);
            foreach(HardwareSummaryItem item in valveItems)
            {
                row = worksheet.insertHardwareItem(item, row);
            }
            row++;

            row = worksheet.insertTitleRow("Associated Costs", row);
            row++;

            row = worksheet.insertCostHeaders(row);
            foreach(CostSummaryItem item in costItems)
            {
                row = worksheet.insertCostItem(item, row);
            }
            row++;

            worksheet.Columns().AdjustToContents();
        }
        internal static void AddElectricalMaterialSheet(XLWorkbook workbook, TECBid bid, string sheetName = "Wire and Conduit")
        {
            List<TECConnection> connections = getAllConnections(bid);
            List<LengthSummaryItem> wireItems = consolidateWire(connections);
            List<LengthSummaryItem> conduitItems = consolidateConduit(connections);
            List<CostSummaryItem> costItems = consolidateCostInConnections(connections);
            List<RatedCostSummaryItem> ratedItems = consolidateRatedCostInConnections(connections);

            IXLWorksheet worksheet = workbook.Worksheets.Add(sheetName);
            int row = 1;

            row = worksheet.insertTitleRow("Wire", row);
            row++;

            row = worksheet.insertLengthHeaders(row);
            foreach(LengthSummaryItem item in wireItems)
            {
                row = worksheet.insertLengthItem(item, row);
            }
            row++;

            row = worksheet.insertTitleRow("Conduit", row);
            row++;

            row = worksheet.insertLengthHeaders(row);
            foreach(LengthSummaryItem item in conduitItems)
            {
                row = worksheet.insertLengthItem(item, row);
            }
            row++;

            row = worksheet.insertTitleRow("Associated Costs", row);
            row++;

            row = worksheet.insertCostHeaders(row);
            foreach(CostSummaryItem item in costItems)
            {
                row = worksheet.insertCostItem(item, row);
            }
            row++;

            row = worksheet.insertTitleRow("Rated Costs", row);
            row++;

            row = worksheet.insertRatedCostHeaders(row);
            foreach(RatedCostSummaryItem item in ratedItems)
            {
                row = worksheet.insertRatedCostItem(item, row);
            }
            row++;

            worksheet.Columns().AdjustToContents();
        }
        internal static void AddMiscCostsSheet(XLWorkbook workbook, TECBid bid, string sheetName = "Misc Costs")
        {
            throw new NotImplementedException();
        }

        #region Insert to Sheet Methods
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
        private static int insertLengthHeaders(this IXLWorksheet worksheet, int row)
        {
            IXLRow headerRow = worksheet.Row(row);
            headerRow.Style.Font.SetBold();

            headerRow.Cell("A").Value = "Name";
            headerRow.Cell("B").Value = "Length";
            headerRow.Cell("C").Value = "Cost per Foot";
            headerRow.Cell("D").Value = "Total Cost";
            headerRow.Cell("E").Value = "Labor per Foot";
            headerRow.Cell("F").Value = "Total Labor";
            row++;

            return row;
        }
        private static int insertRatedCostHeaders(this IXLWorksheet worksheet, int row)
        {
            IXLRow headerRow = worksheet.Row(row);
            headerRow.Style.Font.SetBold();

            headerRow.Cell("A").Value = "Name";
            headerRow.Cell("B").Value = "Length";
            headerRow.Cell("C").Value = "Type";
            headerRow.Cell("D").Value = "Cost per Foot";
            headerRow.Cell("E").Value = "Total Cost";
            headerRow.Cell("F").Value = "Labor per Foot";
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
            itemRow.Cell("E").insertDollarDouble(item.Hardware.Price);
            itemRow.Cell("F").insertDollarDouble(item.Hardware.Cost);
            itemRow.Cell("G").insertDollarDouble(item.TotalCost);
            itemRow.Cell("H").insertDouble(item.Hardware.Labor);
            itemRow.Cell("I").insertDouble(item.TotalLabor);
            row++;
            return row;
        }
        private static int insertCostItem(this IXLWorksheet worksheet, CostSummaryItem item, int row)
        {
            IXLRow itemRow = worksheet.Row(row);

            itemRow.Cell("A").Value = item.Cost.Name;
            itemRow.Cell("B").Value = item.Quantity;
            itemRow.Cell("C").Value = item.Cost.Type;
            itemRow.Cell("D").insertDollarDouble(item.Cost.Cost);
            itemRow.Cell("E").insertDollarDouble(item.TotalCost);
            itemRow.Cell("F").insertDouble(item.Cost.Labor);
            itemRow.Cell("G").insertDouble(item.TotalLabor);
            row++;
            return row;
        }
        private static int insertLengthItem(this IXLWorksheet worksheet, LengthSummaryItem item, int row)
        {
            IXLRow itemRow = worksheet.Row(row);

            itemRow.Cell("A").Value = item.Material.Name;
            itemRow.Cell("B").insertDouble(item.Length);
            itemRow.Cell("C").insertDollarDouble(item.Material.Cost);
            itemRow.Cell("D").insertDollarDouble(item.TotalCost);
            itemRow.Cell("E").insertDouble(item.Material.Labor);
            itemRow.Cell("F").insertDouble(item.TotalLabor);
            row++;
            return row;
        }
        private static int insertRatedCostItem(this IXLWorksheet worksheet, RatedCostSummaryItem item, int row)
        {
            IXLRow itemRow = worksheet.Row(row);

            itemRow.Cell("A").Value = item.RatedCost.Name;
            itemRow.Cell("B").insertDouble(item.Length);
            itemRow.Cell("C").Value = item.RatedCost.Type;
            itemRow.Cell("D").insertDollarDouble(item.RatedCost.Cost);
            itemRow.Cell("E").insertDollarDouble(item.TotalCost);
            itemRow.Cell("F").insertDouble(item.RatedCost.Labor);
            itemRow.Cell("G").insertDouble(item.TotalLabor);
            row++;
            return row;
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
        private static List<TECPanel> getAllPanels(TECBid bid)
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
        private static List<TECDevice> getAllDevices(TECBid bid)
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
        private static List<TECValve> getAllValves(TECBid bid)
        {
            List<TECValve> valves = new List<TECValve>();
            foreach (TECTypical typ in bid.Systems)
            {
                foreach (TECSystem instance in typ.Instances)
                {
                    foreach (TECEquipment equip in instance.Equipment)
                    {
                        foreach (TECSubScope ss in equip.SubScope)
                        {
                            foreach (IEndDevice endDev in ss.Devices)
                            {
                                if (endDev is TECValve valve)
                                {
                                    valves.Add(valve);
                                }
                            }
                        }
                    }
                }
            }
            return valves;
        }
        private static List<TECConnection> getAllConnections(TECBid bid)
        {
            List<TECConnection> connections = new List<TECConnection>();
            foreach (TECController controller in bid.Controllers)
            {
                connections.AddRange(controller.ChildrenConnections);
            }
            foreach (TECTypical typ in bid.Systems)
            {
                foreach(TECSystem instance in typ.Instances)
                {
                    foreach(TECController controller in instance.Controllers)
                    {
                        connections.AddRange(controller.ChildrenConnections);
                    }
                }
            }
            return connections;
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
        private static List<LengthSummaryItem> consolidateWire(IEnumerable<TECConnection> connections)
        {
            Dictionary<TECElectricalMaterial, LengthSummaryItem> dictionary = new Dictionary<TECElectricalMaterial, LengthSummaryItem>();
            List<LengthSummaryItem> items = new List<LengthSummaryItem>();

            foreach(TECConnection connection in connections)
            {
                foreach(TECElectricalMaterial type in connection.GetConnectionTypes())
                {
                    if (dictionary.ContainsKey(type))
                    {
                        dictionary[type].AddLength(connection.Length);
                    }
                    else
                    {
                        LengthSummaryItem item = new LengthSummaryItem(type, connection.Length);
                        dictionary.Add(type, item);
                        items.Add(item);
                    }
                }
            }

            return items;
        }
        private static List<LengthSummaryItem> consolidateConduit(IEnumerable<TECConnection> connections)
        {
            Dictionary<TECElectricalMaterial, LengthSummaryItem> dictionary = new Dictionary<TECElectricalMaterial, LengthSummaryItem>();
            List<LengthSummaryItem> items = new List<LengthSummaryItem>();

            foreach (TECConnection connection in connections)
            {
                TECElectricalMaterial type = connection.ConduitType;
                if (type != null)
                {
                    if (dictionary.ContainsKey(type))
                    {
                        dictionary[type].AddLength(connection.ConduitLength);
                    }
                    else
                    {
                        LengthSummaryItem item = new LengthSummaryItem(type, connection.ConduitLength);
                        dictionary.Add(type, item);
                        items.Add(item);
                    }
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
        private static List<CostSummaryItem> consolidateCostInValves(IEnumerable<TECValve> valves)
        {
            Dictionary<TECCost, CostSummaryItem> dictionary = new Dictionary<TECCost, CostSummaryItem>();
            List<CostSummaryItem> items = new List<CostSummaryItem>();

            List<TECCost> costs = new List<TECCost>();
            foreach (TECValve valve in valves)
            {
                costs.AddRange(valve.AssociatedCosts);
                costs.AddRange(valve.Actuator.AssociatedCosts);
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
        private static List<CostSummaryItem> consolidateCostInConnections(IEnumerable<TECConnection> connections)
        {
            Dictionary<TECCost, CostSummaryItem> dictionary = new Dictionary<TECCost, CostSummaryItem>();
            List<CostSummaryItem> items = new List<CostSummaryItem>();

            List<TECCost> costs = new List<TECCost>();
            foreach(TECConnection connection in connections)
            {
                foreach(TECElectricalMaterial mat in connection.GetConnectionTypes())
                {
                    costs.AddRange(mat.AssociatedCosts);
                }
                if (connection.ConduitType != null)
                {
                    costs.AddRange(connection.ConduitType.AssociatedCosts);
                }
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
        private static List<RatedCostSummaryItem> consolidateRatedCostInConnections(IEnumerable<TECConnection> connections)
        {
            Dictionary<TECCost, RatedCostSummaryItem> dictionary = new Dictionary<TECCost, RatedCostSummaryItem>();
            List<RatedCostSummaryItem> items = new List<RatedCostSummaryItem>();

            List<Tuple<TECCost, double>> costs = new List<Tuple<TECCost, double>>();
            foreach (TECConnection connection in connections)
            {
                foreach(TECElectricalMaterial mat in connection.GetConnectionTypes())
                {
                    foreach(TECCost rated in mat.RatedCosts)
                    {
                        costs.Add(new Tuple<TECCost, double>(rated, connection.Length));
                    }
                }
                if (connection.ConduitType != null)
                {
                    foreach (TECCost rated in connection.ConduitType.RatedCosts)
                    {
                        costs.Add(new Tuple<TECCost, double>(rated, connection.ConduitLength));
                    }
                }
            }

            foreach(Tuple<TECCost, double> cost in costs)
            {
                if (dictionary.ContainsKey(cost.Item1))
                {
                    dictionary[cost.Item1].AddLength(cost.Item2);
                }
                else
                {
                    RatedCostSummaryItem item = new RatedCostSummaryItem(cost.Item1, cost.Item2);
                    dictionary.Add(cost.Item1, item);
                    items.Add(item);
                }
            }
            return items;
        }
        #endregion

        #region Cell Format Insert Methods
        private static void insertDouble(this IXLCell cell, double doub)
        {
            cell.DataType = XLCellValues.Number;
            cell.Style.NumberFormat.Format = "0.00";
            cell.Value = doub;
        }
        private static void insertDollarDouble(this IXLCell cell, double doub)
        {
            cell.DataType = XLCellValues.Number;
            cell.Style.NumberFormat.Format = Turnover.accountingFormat;
            cell.Value = doub;
        }
        #endregion
    }
}
