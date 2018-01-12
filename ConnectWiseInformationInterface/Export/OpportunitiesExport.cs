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

            //Column A
            IXLColumn col = ws.Column("A");
            
            col.Cell(1).Value = "Opportunity Types Included:";
            col.Cell(1).Style.Font.SetBold();
            int row = 2;
            foreach(OpportunityType type in manager.OpportunityTypes)
            {
                col.Cell(row).Value = type.Description;
                row++;
            }

            //Column B
            col = ws.Column("B");
            col.Width = 50;

            //Column C
            col = ws.Column("C");

            col.Cell(1).Value = "From Date:";
            col.Cell(2).Value = "To Date:";

            col.Cell(4).Value = "# of Opportunities:";

            col.Style.Font.SetBold();

            //Column D
            col = ws.Column("D");

            if (manager.StartDate.HasValue)
            {
                col.Cell(1).Value = manager.StartDate;
            }
            else
            {
                col.Cell(1).Value = "Any";
            }

            if (manager.EndDate.HasValue)
            {
                col.Cell(2).Value = manager.EndDate;
            }
            else
            {
                col.Cell(2).Value = "Any";
            }

            col.Cell(4).Value = manager.FilteredOpportunities.Count;

            //Column E
            col = ws.Column("E");
            col.Width = 50;

            //Column F
            col = ws.Column("F");

            col.Cell(1).Value = "Forecasted Hours";

            col.Cell(3).Value = "Engineering:";
            col.Cell(4).Value = "Software:";
            col.Cell(5).Value = "Graphics:";
            col.Cell(6).Value = "Technician:";
            col.Cell(7).Value = "Project Management:";

            col.Style.Font.SetBold();

            //Column G
            col = ws.Column("G");
            
            double engineering = 0, programming = 0, graphics = 0, technician = 0, pm = 0;
            foreach (Opportunity opp in manager.FilteredOpportunities)
            {
                double probability = (opp.GetProbability(manager.Probabilities) / 100.0);
                engineering += (opp.GetEngineeringHours() * probability);
                programming += (opp.GetProgrammingHours() * probability);
                graphics += (opp.GetGraphicsHours() * probability);
                technician += (opp.GetTechnicianHours() * probability);
                pm += (opp.GetPMHours() * probability);
            }

            col.Cell(3).Value = engineering;
            col.Cell(4).Value = programming;
            col.Cell(5).Value = graphics;
            col.Cell(6).Value = technician;
            col.Cell(7).Value = pm;

            ws.Columns().AdjustToContents();
        }
        private static void addDetailsSheet(this XLWorkbook book, OppFilterManager manager)
        {
            IXLWorksheet worksheet = book.Worksheets.Add("Details");

            worksheet.insertHeaders(1);

            int i = 2;
            foreach(Opportunity opp in manager.FilteredOpportunities)
            {
                IXLRow row = worksheet.Row(i);

                string typeName = "None";
                if (opp.Type != null)
                {
                    typeName = opp.Type.Name;
                }

                int probability = opp.GetProbability(manager.Probabilities);
                int engineering = opp.GetEngineeringHours();
                int programming = opp.GetProgrammingHours();
                int graphics = opp.GetGraphicsHours();
                int technician = opp.GetTechnicianHours();
                int pm = opp.GetProgrammingHours();
                
                row.Cell("A").Value = opp.Name;
                row.Cell("B").Value = opp.PrimarySalesRep.Name;
                row.Cell("C").Value = typeName;
                row.Cell("D").Value = string.Format("{0}%", probability);
                row.Cell("E").Value = opp.ExpectedCloseDate.Value.Date;
                row.Cell("F").Value = opp.GetEngineeringHours();
                row.Cell("G").Value = engineering * (probability / 100.0);
                row.Cell("H").Value = programming;
                row.Cell("I").Value = programming * (probability / 100.0);
                row.Cell("J").Value = graphics;
                row.Cell("K").Value = graphics * (probability / 100.0);
                row.Cell("L").Value = technician;
                row.Cell("M").Value = technician * (probability / 100.0);
                row.Cell("N").Value = pm;
                row.Cell("O").Value = pm * (probability / 100.0);

                i++;
            }

            worksheet.Columns().AdjustToContents();
        }

        private static void insertHeaders(this IXLWorksheet ws, int row)
        {
            IXLRow headerRow = ws.Row(row);
            headerRow.Style.Font.SetBold();

            headerRow.Cell("A").Value = "Opportunity Name";
            headerRow.Cell("B").Value = "Sales Person";
            headerRow.Cell("C").Value = "Opportunity Type";
            headerRow.Cell("D").Value = "Probability";
            headerRow.Cell("E").Value = "Expected Close Date";
            headerRow.Cell("F").Value = "Engineering Estimate";
            headerRow.Cell("G").Value = "Engineering Forecast";
            headerRow.Cell("H").Value = "Programming Estimate";
            headerRow.Cell("I").Value = "Programming Forecast";
            headerRow.Cell("J").Value = "Graphics Estimate";
            headerRow.Cell("K").Value = "Graphics Forecast";
            headerRow.Cell("L").Value = "Technician Estimate";
            headerRow.Cell("M").Value = "Technician Forecast";
            headerRow.Cell("N").Value = "PM Estimate";
            headerRow.Cell("O").Value = "PM Forecast";
        }
    }
}
