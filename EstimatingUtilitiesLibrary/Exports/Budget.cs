
using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using ClosedXML.Excel;


namespace EstimatingUtilitiesLibrary.Exports
{
    public class Budget
    {
        
        public static void GenerateReport(string filePath, TECBid bid)
        {
            XLWorkbook workbook = new XLWorkbook();
            IXLWorksheet worksheet = workbook.Worksheets.Add("Summary");

            worksheet.Cell(1, 2).Value = "Quantity";
            worksheet.Cell(1, 2).Style.Border.BottomBorder = XLBorderStyleValues.Thick;
            worksheet.Cell(1, 3).Value = "Price";
            worksheet.Cell(1, 3).Style.Border.BottomBorder = XLBorderStyleValues.Thick;
            
            worksheet.Cell(2, 1).Value = "Systems";
            worksheet.Cell(2, 1).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            int x = 3;
            for(int i = 0; i < bid.Systems.Count; i++, x++)
            {
                TECTypical typical = bid.Systems[i];
                TECEstimator systemEstimate = new TECEstimator(typical, bid.Parameters, new ChangeWatcher(typical));
                worksheet.Cell(x, 1).Value = typical.Name;
                worksheet.Cell(x, 2).Value = typical.Instances.Count;
                worksheet.Cell(x, 3).Value = systemEstimate.TotalPrice;
                worksheet.Cell(x, 3).Style.NumberFormat.Format = "$ #,##0.00";
            }
            x++;
            worksheet.Cell(x, 1).Value = "BMS Network";
            worksheet.Cell(x, 1).Style.Border.BottomBorder = XLBorderStyleValues.Thin;

            x++;
            for (int i = 0; i < bid.Controllers.Count; i++, x++)
            {
                TECController controller= bid.Controllers[i];
                TECEstimator systemEstimate = new TECEstimator(controller, bid.Parameters, new ChangeWatcher(controller));
                worksheet.Cell(x, 1).Value = controller.Name;
                worksheet.Cell(x, 2).Value = "1";
                worksheet.Cell(x, 3).Value = systemEstimate.TotalPrice;
                worksheet.Cell(x, 3).Style.NumberFormat.Format = "$ #,##0.00";
            }
            for (int i = 0; i < bid.Panels.Count; i++, x++)
            {
                TECPanel typical = bid.Panels[i];
                TECEstimator systemEstimate = new TECEstimator(typical, bid.Parameters, new ChangeWatcher(typical));
                worksheet.Cell(x, 1).Value = typical.Name;
                worksheet.Cell(x, 2).Value = "1";
                worksheet.Cell(x, 3).Value = systemEstimate.TotalPrice;
                worksheet.Cell(x, 3).Style.NumberFormat.Format = "$ #,##0.00";
            }
            x++;
            worksheet.Cell(x, 1).Value = "Miscellaneous";
            worksheet.Cell(x, 1).Style.Border.BottomBorder = XLBorderStyleValues.Thin;

            x++;
            for (int i = 0; i < bid.MiscCosts.Count; i++, x++)
            {
                TECMisc typical = bid.MiscCosts[i];
                TECEstimator systemEstimate = new TECEstimator(typical, bid.Parameters, new ChangeWatcher(typical));
                worksheet.Cell(x, 1).Value = typical.Name;
                worksheet.Cell(x, 2).Value = typical.Quantity;
                worksheet.Cell(x, 3).Value = systemEstimate.TotalPrice;
                worksheet.Cell(x, 3).Style.NumberFormat.Format = "$ #,##0.00";
            }
            TECEstimator extraLaborEstimate = new TECEstimator(bid.ExtraLabor, bid.Parameters, new ChangeWatcher(bid.ExtraLabor));
            worksheet.Cell(x, 1).Value = "Other Labor";
            worksheet.Cell(x, 2).Value = "1";
            worksheet.Cell(x, 3).Value = extraLaborEstimate.TotalPrice;
            worksheet.Cell(x, 3).Style.NumberFormat.Format = "$ #,##0.00";
            x+=2;
            worksheet.Cell(x, 2).Value = "Total: ";
            worksheet.Cell(x, 2).Style.Border.BottomBorder = XLBorderStyleValues.Thick;

            worksheet.Cell(x, 3).FormulaA1 = "=SUM(C3:C" + (x-1) + ")";
            worksheet.Cell(x, 3).Style.NumberFormat.Format = "$ #,##0.00";
            workbook.SaveAs(filePath);
        }
    }
}
