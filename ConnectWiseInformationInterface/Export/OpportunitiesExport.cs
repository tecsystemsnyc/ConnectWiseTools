using ClosedXML.Excel;
using ConnectWiseDotNetSDK.ConnectWise.Client.Sales.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectWiseInformationInterface.Export
{
    public static class OpportunitiesExport
    {
        public static void ExportOpportunities(string filePath, IEnumerable<Opportunity> opps)
        {
            XLWorkbook workbook = new XLWorkbook();
            IXLWorksheet worksheet = workbook.Worksheets.Add("SALES LOG");

            worksheet.insertHeaders(1);

            int i = 1;
            foreach(Opportunity opp in opps)
            {
                IXLRow row = worksheet.Row(i);

                double probability = opp.Probability;

                row.Cell("A").Value = opp.

                i++;
            }
        }

        private static void insertHeaders(this IXLWorksheet ws, int row)
        {
            IXLRow headerRow = ws.Row(row);
            headerRow.Style.Font.SetBold();

            headerRow.Cell("A").Value = "Opportunity Number";
            headerRow.Cell("B").Value = "Opportunity Name";
            headerRow.Cell("C").Value = "Sales Person";
            headerRow.Cell("D").Value = "Opportunity Type";
            headerRow.Cell("E").Value = "Probability";
            headerRow.Cell("F").Value = "Expected Close Date";
            headerRow.Cell("G").Value = "Engineering Estimate";
            headerRow.Cell("H").Value = "Engineering Forecast";
            headerRow.Cell("I").Value = "Programming Estimate";
            headerRow.Cell("J").Value = "Programming Forecast";
            headerRow.Cell("K").Value = "Graphics Estimate";
            headerRow.Cell("L").Value = "Graphics Forecast";
            headerRow.Cell("M").Value = "Technician Estimate";
            headerRow.Cell("N").Value = "Technician Forecast";
            headerRow.Cell("O").Value = "PM Estimate";
            headerRow.Cell("P").Value = "PM Forecast";
        }
    }
}
