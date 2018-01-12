using ClosedXML.Excel;
using ConnectWiseDotNetSDK.ConnectWise.Client.Common.Model;
using ConnectWiseDotNetSDK.ConnectWise.Client.Sales.Model;
using ConnectWiseInformationInterface.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ConnectWiseInformationInterface.Export
{
    public static class OpportunitiesExport
    {
        public static void ExportOpportunities(string filePath, OppFilterManager oppManager)
        {
            XLWorkbook workbook = new XLWorkbook();

            workbook.addSummarySheet(oppManager);
            workbook.addDetailsSheet(oppManager);

            try
            {
                workbook.SaveAs(filePath);
                Process.Start(filePath);
            }
            catch
            {
                MessageBox.Show("Excel file couldn't save. File may be open elsewhere.");
            }
        }

        private static void addSummarySheet(this XLWorkbook book, OppFilterManager manager)
        {
            IXLWorksheet ws = book.Worksheets.Add("Summary");

            IXLColumn col = ws.Column("A");

            col.Cell(1).Value = "From Date:";
            col.Cell(2).Value = "To Date:";
            col.Cell(3);
        }
        private static void addDetailsSheet(this XLWorkbook book, OppFilterManager manager)
        {
            //IXLWorksheet worksheet = workbook.Worksheets.Add("SALES LOG");

            worksheet.insertHeaders(1);

            int i = 2;
            //foreach(Opportunity opp in opps)
            //{
            //    IXLRow row = worksheet.Row(i);

            //    int probability = 0;
            //    bool probabilityFound = false;
            //    foreach(SalesProbability prob in oppManager.p)
            //    {
            //        if (prob.Id == opp.Probability.Id)
            //        {
            //            probability = (int)prob.Probability;
            //            probabilityFound = true;
            //        }
            //    }
            //    if (!probabilityFound) { Console.WriteLine(string.Format("Probability not found in {0}", opp.Name)); }

            //    int engineering = 0, programming = 0, graphics = 0, technician = 0, pm = 0;

            //    foreach(CustomFieldValue custom in opp.CustomFields)
            //    {
            //        switch (custom.Caption) {
            //            case "FC: Graphics":
            //                if (custom.Value != null)
            //                {
            //                    graphics = custom.Value.CastTo<int>();
            //                }
            //                break;
            //            case "FC: Software":
            //                if (custom.Value != null)
            //                {
            //                    programming = custom.Value.CastTo<int>();
            //                }
            //                break;
            //            case "FC:TechLabor":
            //                if (custom.Value != null)
            //                {
            //                    technician = custom.Value.CastTo<int>();
            //                }
            //                break;
            //            case "FC:PrjctMgmt":
            //                if (custom.Value != null)
            //                {
            //                    pm = custom.Value.CastTo<int>();
            //                }
            //                break;
            //            case "FC: Engineer":
            //                if (custom.Value != null)
            //                {
            //                    engineering = custom.Value.CastTo<int>();
            //                }
            //                break;
            //            default:
            //                Console.WriteLine(string.Format("Unknown custom field: {0}", custom.Caption));
            //                break;
            //        }
            //    }

            //    row.Cell("A").Value = opp.Id;
            //    row.Cell("B").Value = opp.Name;
            //    row.Cell("C").Value = opp.PrimarySalesRep.Name;
            //    row.Cell("D").Value = opp.Type.Name;
            //    row.Cell("E").Value = string.Format("{0}%", probability);
            //    row.Cell("F").Value = opp.ExpectedCloseDate.Value.Date;
            //    row.Cell("G").Value = engineering;
            //    row.Cell("H").Value = engineering * probability/100;
            //    row.Cell("I").Value = programming;
            //    row.Cell("J").Value = programming * probability/100;
            //    row.Cell("K").Value = graphics;
            //    row.Cell("L").Value = graphics * probability/100;
            //    row.Cell("M").Value = technician;
            //    row.Cell("N").Value = technician * probability/100;
            //    row.Cell("O").Value = pm;
            //    row.Cell("P").Value = pm * probability/100;

            //    i++;
            //}

            worksheet.Columns().AdjustToContents();
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
