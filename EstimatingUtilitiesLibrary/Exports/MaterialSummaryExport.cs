using ClosedXML.Excel;
using EstimatingLibrary;
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

            throw new NotImplementedException();
        }

        internal static void AddControllersSheet(XLWorkbook workbook, TECBid bid, string sheetName = "Controllers")
        {
            IXLWorksheet worksheet = workbook.Worksheets.Add(sheetName);


            throw new NotImplementedException();
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
    }
}
