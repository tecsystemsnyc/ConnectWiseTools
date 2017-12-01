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
        public static void Generate(string path, TECBid bid)
        {
            XLWorkbook workbook = new XLWorkbook();
        }

        private static void addDevicesSheet(XLWorkbook workbook, TECBid bid)
        {
            const string sheetName = "Devices";

            IXLWorksheet worksheet = workbook.Worksheets.Add(sheetName);
        }
    }
}
