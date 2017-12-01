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
        }

        internal static void AddDevicesSheet(XLWorkbook workbook, TECBid bid)
        {
            const string sheetName = "Devices";

            IXLWorksheet worksheet = workbook.Worksheets.Add(sheetName);
        }
    }
}
