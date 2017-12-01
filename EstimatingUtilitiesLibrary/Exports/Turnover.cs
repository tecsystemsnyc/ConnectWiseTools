using ClosedXML.Excel;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using EstimatingLibrary;
using EstimatingLibrary.Interfaces;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EstimatingUtilitiesLibrary.Exports
{
    public static class Turnover
    {
        public static void GenerateTurnoverExport(string path, TECBid bid, TECEstimator estimate)
        {
            XLWorkbook workbook = new XLWorkbook();
            createSummarySheet(workbook, bid, estimate);
            MaterialSummaryExport.AddControllersSheet(workbook, bid);
            //MaterialSummaryExport.AddPanelsSheet(workbook, bid);
            //MaterialSummaryExport.AddDevicesSheet(workbook, bid);
            //MaterialSummaryExport.AddValvesSheet(workbook, bid);
            //MaterialSummaryExport.AddElectricalMaterialSheet(workbook, bid);
            //MaterialSummaryExport.AddMiscCostsSheet(workbook, bid);
            createBomSheets(workbook, bid);
            workbook.SaveAs(path);
        }

        public static void GenerateSummaryExport(string path, TECBid bid, TECEstimator estimate)
        {
            using (WordprocessingDocument package = WordprocessingDocument.Create(path, 
                DocumentFormat.OpenXml.WordprocessingDocumentType.Document))
            {
                package.AddMainDocumentPart();
                Document document = new Document();
                Body body = new Body();
                document.Append(body);
                package.MainDocumentPart.Document = document;

                body.Append(introParagraph(bid));
                body.Append(laborSummary(estimate));
                body.Append(costSummary(estimate));
                body.Append(saleSummary(estimate));

                package.MainDocumentPart.Document.Save();
            }
        }

        public static void GenerateBOM(string path, TECBid bid)
        {
            XLWorkbook workbook = new XLWorkbook();
            createBomSheets(workbook, bid);
            workbook.SaveAs(path);
        }

        private static void createBomSheets(XLWorkbook workbook, TECBid bid)
        {
            int postfix = 1;
            List<String> sheetNames = new List<string>();
            foreach (TECTypical typical in bid.Systems.Where(typ => typ.Instances.Count > 0))
            {
                List<TECCost> associatedCosts = new List<TECCost>();
                string sheetName = typical.Instances.Count > 1 ? typical.Name : typical.Instances[0].Name;
                if (sheetName == "")
                {
                    sheetName = "Untitled";
                }
                if (sheetNames.Contains(sheetName))
                {
                    sheetName = sheetName + "(" + postfix + ")";
                    postfix++;
                }
                sheetNames.Add(sheetName);
                IXLWorksheet worksheet = workbook.Worksheets.Add(sheetName);
                worksheet.Cell(1, 4).Value = sheetName;
                worksheet.Cell(1, 4).Style.Font.SetBold();
                if (typical.Instances.Count > 1)
                {
                    worksheet.Cell(1, 5).Value = "(Quantity: " + typical.Instances.Count + ")";
                }
                int x = 3;
                worksheet.Cell(x, 4).Value = "FIELD MATERIAL";
                x++;
                worksheet.Cell(x, 1).Value = "TAG";
                worksheet.Cell(x, 1).Style.Font.SetBold();
                worksheet.Cell(x, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(x, 2).Value = "QTY.";
                worksheet.Cell(x, 2).Style.Font.SetBold();
                worksheet.Cell(x, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(x, 3).Value = "MANUFACTURER";
                worksheet.Cell(x, 3).Style.Font.SetBold();
                worksheet.Cell(x, 3).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(x, 4).Value = "MODEL NO.";
                worksheet.Cell(x, 4).Style.Font.SetBold();
                worksheet.Cell(x, 4).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(x, 5).Value = "DESCRIPTION";
                worksheet.Cell(x, 5).Style.Font.SetBold();
                worksheet.Cell(x, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(x, 6).Value = "REMARKS";
                worksheet.Cell(x, 6).Style.Font.SetBold();
                worksheet.Cell(x, 6).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                x++;
                List<IEndDevice> devices = new List<IEndDevice>();
                associatedCosts.AddRange(typical.AssociatedCosts);
                foreach (TECEquipment equipment in typical.Equipment)
                {
                    associatedCosts.AddRange(equipment.AssociatedCosts);
                    foreach (TECSubScope subScope in equipment.SubScope)
                    {
                        associatedCosts.AddRange(subScope.AssociatedCosts);
                        devices.AddRange(subScope.Devices);
                    }
                }
                foreach (IEndDevice device in devices.Distinct())
                {
                    worksheet.Cell(x, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(x, 2).Value = devices.Count(item => item == device);
                    worksheet.Cell(x, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(x, 3).Value = device.Manufacturer.Label;
                    worksheet.Cell(x, 3).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(x, 4).Value = device.Name;
                    worksheet.Cell(x, 4).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(x, 5).Value = device.Description;
                    worksheet.Cell(x, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(x, 6).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    x++;
                }
                foreach (TECController controller in typical.Controllers)
                {
                    associatedCosts.AddRange(controller.AssociatedCosts);
                    worksheet.Cell(x, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(x, 2).Value = "1";
                    worksheet.Cell(x, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(x, 3).Value = controller.Type.Manufacturer.Label;
                    worksheet.Cell(x, 3).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(x, 4).Value = controller.Type.Name;
                    worksheet.Cell(x, 4).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(x, 5).Value = controller.Type.Description;
                    worksheet.Cell(x, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(x, 6).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    x++;
                }
                foreach (TECPanel panel in typical.Panels)
                {
                    associatedCosts.AddRange(panel.AssociatedCosts);
                    worksheet.Cell(x, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(x, 2).Value = "1";
                    worksheet.Cell(x, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(x, 3).Value = panel.Type.Manufacturer.Label;
                    worksheet.Cell(x, 3).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(x, 4).Value = panel.Type.Name;
                    worksheet.Cell(x, 4).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(x, 5).Value = panel.Type.Description;
                    worksheet.Cell(x, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(x, 6).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    x++;
                }
                x++;
                worksheet.Cell(x, 4).Value = "OTHER MATERIAL";
                x++;
                worksheet.Cell(x, 1).Value = "Name";
                worksheet.Cell(x, 1).Style.Font.SetBold();
                worksheet.Cell(x, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(x, 2).Value = "QTY.";
                worksheet.Cell(x, 2).Style.Font.SetBold();
                worksheet.Cell(x, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(x, 3).Value = "DESCRIPTION";
                worksheet.Cell(x, 3).Style.Font.SetBold();
                worksheet.Cell(x, 3).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                x++;
                foreach (TECCost cost in associatedCosts.Distinct())
                {
                    worksheet.Cell(x, 1).Value = cost.Name;
                    worksheet.Cell(x, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(x, 2).Value = associatedCosts.Count(item => item == cost);
                    worksheet.Cell(x, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(x, 3).Value = cost.Description;
                    worksheet.Cell(x, 3).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    x++;
                }
                worksheet.Columns().AdjustToContents();

            }

            createMiscBOMSheet(workbook, bid);

        }

        private static void createMiscBOMSheet(XLWorkbook workbook, TECBid bid)
        {
            List<TECCost> associatedCosts = new List<TECCost>();
            IXLWorksheet worksheet = workbook.Worksheets.Add("Misc.");
            worksheet.Cell(1, 4).Value = "Misc.";
            worksheet.Cell(1, 4).Style.Font.SetBold();
            int x = 3;
            worksheet.Cell(x, 4).Value = "FIELD MATERIAL";
            x++;
            worksheet.Cell(x, 1).Value = "TAG";
            worksheet.Cell(x, 1).Style.Font.SetBold();
            worksheet.Cell(x, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Cell(x, 2).Value = "QTY.";
            worksheet.Cell(x, 2).Style.Font.SetBold();
            worksheet.Cell(x, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Cell(x, 3).Value = "MANUFACTURER";
            worksheet.Cell(x, 3).Style.Font.SetBold();
            worksheet.Cell(x, 3).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Cell(x, 4).Value = "MODEL NO.";
            worksheet.Cell(x, 4).Style.Font.SetBold();
            worksheet.Cell(x, 4).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Cell(x, 5).Value = "DESCRIPTION";
            worksheet.Cell(x, 5).Style.Font.SetBold();
            worksheet.Cell(x, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Cell(x, 6).Value = "REMARKS";
            worksheet.Cell(x, 6).Style.Font.SetBold();
            worksheet.Cell(x, 6).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            x++;
            
            foreach (TECController controller in bid.Controllers)
            {
                associatedCosts.AddRange(controller.AssociatedCosts);
                worksheet.Cell(x, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(x, 2).Value = "1";
                worksheet.Cell(x, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(x, 3).Value = controller.Type.Manufacturer.Label;
                worksheet.Cell(x, 3).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(x, 4).Value = controller.Type.Name;
                worksheet.Cell(x, 4).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(x, 5).Value = controller.Type.Description;
                worksheet.Cell(x, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(x, 6).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                x++;
            }
            foreach (TECPanel panel in bid.Panels)
            {
                associatedCosts.AddRange(panel.AssociatedCosts);
                worksheet.Cell(x, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(x, 2).Value = "1";
                worksheet.Cell(x, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(x, 3).Value = panel.Type.Manufacturer.Label;
                worksheet.Cell(x, 3).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(x, 4).Value = panel.Type.Name;
                worksheet.Cell(x, 4).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(x, 5).Value = panel.Type.Description;
                worksheet.Cell(x, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(x, 6).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                x++;
            }
            x++;
            worksheet.Cell(x, 4).Value = "OTHER MATERIAL";
            x++;
            worksheet.Cell(x, 1).Value = "Name";
            worksheet.Cell(x, 1).Style.Font.SetBold();
            worksheet.Cell(x, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Cell(x, 2).Value = "QTY.";
            worksheet.Cell(x, 2).Style.Font.SetBold();
            worksheet.Cell(x, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Cell(x, 3).Value = "DESCRIPTION";
            worksheet.Cell(x, 3).Style.Font.SetBold();
            worksheet.Cell(x, 3).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            x++;
            foreach (TECCost cost in associatedCosts.Distinct())
            {
                worksheet.Cell(x, 1).Value = cost.Name;
                worksheet.Cell(x, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(x, 2).Value = associatedCosts.Count(item => item == cost);
                worksheet.Cell(x, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(x, 3).Value = cost.Description;
                worksheet.Cell(x, 3).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                x++;
            }
            worksheet.Columns().AdjustToContents();

        }

        private static void createSummarySheet(XLWorkbook workbook, TECBid bid, TECEstimator estimate)
        {
            IXLWorksheet worksheet = workbook.Worksheets.Add("Summary");
            worksheet.Cell(1, 1).Value = "Project Information";
            worksheet.Cell(1, 1).Style.Font.SetBold();
            worksheet.Cell(1, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            worksheet.Cell(2, 1).Value = "Name";
            worksheet.Cell(2, 2).Value = bid.Name;
            worksheet.Cell(3, 1).Value = "Bid Number";
            worksheet.Cell(3, 2).Value = bid.BidNumber;
            worksheet.Cell(4, 1).Value = "Salesperson";
            worksheet.Cell(4, 2).Value = bid.Salesperson;
            worksheet.Cell(5, 1).Value = "Estimator";
            worksheet.Cell(5, 2).Value = bid.Estimator;

            worksheet.Cell(2, 3).Value = "Tax Exempt";
            worksheet.Cell(2, 4).Value = bid.Parameters.IsTaxExempt ? "Yes" : "No";
            worksheet.Cell(3, 3).Value = "Bond Required";
            worksheet.Cell(3, 4).Value = bid.Parameters.RequiresBond ? "Yes" : "No";

            worksheet.Cell(7, 1).Value = "Labor Summary";
            worksheet.Cell(7, 1).Style.Font.SetBold();
            worksheet.Cell(7, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            worksheet.Cell(8, 1).Value = "Project Management";
            worksheet.Cell(8, 2).Value = String.Format("{0:F} hours", estimate.PMLaborHours);
            worksheet.Cell(9, 1).Value = "Engineering";
            worksheet.Cell(9, 2).Value = String.Format("{0:F} hours", estimate.ENGLaborHours);
            worksheet.Cell(10, 1).Value = "Software";
            worksheet.Cell(10, 2).Value = String.Format("{0:F} hours", estimate.SoftLaborHours);
            worksheet.Cell(11, 1).Value = "Commissioning";
            worksheet.Cell(11, 2).Value = String.Format("{0:F} hours", estimate.CommLaborHours);
            worksheet.Cell(12, 1).Value = "Graphics";
            worksheet.Cell(12, 2).Value = String.Format("{0:F} hours", estimate.GraphLaborHours);
            worksheet.Cell(13, 1).Value = "Field";
            worksheet.Cell(13, 2).Value = String.Format("{0:F} hours", estimate.TECFieldHours);

            worksheet.Cell(14, 1).Value = "Total Hours";
            worksheet.Cell(14, 1).Style.Font.SetBold();

            worksheet.Cell(14, 2).Value = String.Format("{0:F} hours", estimate.TECLaborHours);
            worksheet.Cell(14, 2).Style.Font.SetBold();

            worksheet.Cell(15, 1).Value = "Cost";
            worksheet.Cell(15, 1).Style.Font.SetBold();

            worksheet.Cell(15, 2).Value = String.Format("{0:C} ", estimate.TECLaborCost);
            worksheet.Cell(15, 2).Style.Font.SetBold();

            worksheet.Cell(17, 1).Value = "Costs Summary";
            worksheet.Cell(17, 1).Style.Font.SetBold();
            worksheet.Cell(17, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            worksheet.Cell(18, 1).Value = "Material Cost";
            worksheet.Cell(18, 2).Value = String.Format("{0:C}", estimate.TECMaterialCost);
            worksheet.Cell(19, 1).Value = "Subcontractor Labor";
            worksheet.Cell(19, 2).Value = String.Format("{0:F} hours", estimate.SubcontractorLaborHours);
            worksheet.Cell(20, 1).Value = "Subcontractor Material";
            worksheet.Cell(20, 2).Value = String.Format("{0:C}", estimate.ElectricalMaterialCost);
            worksheet.Cell(21, 1).Value = "Subcontractor Subtotal";
            worksheet.Cell(21, 2).Value = String.Format("{0:C}", estimate.SubcontractorSubtotal);

            worksheet.Cell(23, 1).Value = "Sale Summary";
            worksheet.Cell(23, 1).Style.Font.SetBold();
            worksheet.Cell(23, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            worksheet.Cell(24, 1).Value = "Price";
            worksheet.Cell(24, 2).Value = String.Format("{0:C}", estimate.TotalPrice);
            worksheet.Cell(25, 1).Value = "Margin";
            worksheet.Cell(25, 2).Value = String.Format("%{0:F2}", estimate.Margin);

            var image = worksheet.AddPicture(createPlotImage(estimate));
            image.MoveTo(worksheet.Cell(7, 4).Address);

            worksheet.Columns().AdjustToContents();

        }

        private static string createPlotImage(TECEstimator estimate)
        {
            string path = Path.GetTempFileName();
            var pngExporter = new PngExporter { Width = 600, Height = 400, Background = OxyColors.White };
            PlotModel plotModel = new PlotModel { Title = "Cost Distribution" };
            OxyPlot.Series.PieSeries pieSeries = new OxyPlot.Series.PieSeries { StrokeThickness = 2.0, InsideLabelPosition = 0.8, AngleSpan = 360, StartAngle = 0 };
            pieSeries.Slices.Add(new PieSlice("Material Cost", estimate.TECMaterialCost) { IsExploded = false });
            pieSeries.Slices.Add(new PieSlice("Labor Cost", estimate.TECLaborCost) { IsExploded = false });
            pieSeries.Slices.Add(new PieSlice("Sub. Labor Cost", estimate.SubcontractorLaborCost) { IsExploded = false });
            pieSeries.Slices.Add(new PieSlice("Sub. Material Cost", estimate.ElectricalMaterialCost) { IsExploded = false });
            plotModel.Series.Add(pieSeries);

            pngExporter.ExportToFile(plotModel, path);
            return path;
        }

        private static Paragraph introParagraph(TECBid bid)
        {
            Paragraph par = new Paragraph();
            Text nameText = new Text(String.Format("Bid: {0}", bid.Name));
            Text numberText = new Text(String.Format("Number: {0}", bid.BidNumber));
            Text salespersonText = new Text(String.Format("Salesperson: {0}", bid.Salesperson));
            Text estimatorText = new Text(String.Format("Estimator: {0}", bid.Estimator));
            par.Append(new Run(nameText));
            par.Append(new Break());
            par.Append(new Run(numberText));
            par.Append(new Break());
            par.Append(new Run(salespersonText));
            par.Append(new Break());
            par.Append(new Run(estimatorText));
            par.Append(new Break());
            return par;
        }

        private static Paragraph laborSummary(TECEstimator estimate)
        {
            Paragraph par = new Paragraph();
            Text pmLaborText = new Text(String.Format("Project Management Labor: {0:F} hours", estimate.PMLaborHours));
            Text engLaborText = new Text(String.Format("Engineering Labor: {0:F} hours", estimate.ENGLaborHours));
            Text softLaborText = new Text(String.Format("Software Labor: {0:F} hours", estimate.SoftLaborHours));
            Text commLaborText = new Text(String.Format("Commissioning Labor: {0:F} hours", estimate.CommLaborHours));
            Text graphLaborText = new Text(String.Format("Graphics Labor: {0:F} hours", estimate.GraphLaborHours));
        
            par.Append(new Run(pmLaborText));
            par.Append(new Break());
            par.Append(new Run(engLaborText));
            par.Append(new Break());
            par.Append(new Run(softLaborText));
            par.Append(new Break());
            par.Append(new Run(commLaborText));
            par.Append(new Break());
            par.Append(new Run(graphLaborText));

            return par;
        }

        private static Paragraph costSummary(TECEstimator estimate)
        {
            Paragraph par = new Paragraph();
            Text materialText = new Text(String.Format("Material Cost: {0:C}", estimate.TECMaterialCost));
            Text subLaborText = new Text(String.Format("Subcontractor Labor: {0:C}", estimate.SubcontractorLaborHours));
            Text subMaterialText = new Text(String.Format("Subcontractor Material: {0:C}", estimate.ElectricalMaterialCost));
            
            par.Append(new Break());
            par.Append(new Run(materialText));
            par.Append(new Break());
            par.Append(new Run(subLaborText));
            par.Append(new Break());
            par.Append(new Run(subMaterialText));

            return par;
        }

        private static Paragraph saleSummary(TECEstimator estimate)
        {
            Paragraph par = new Paragraph();
            Text saleText = new Text(String.Format("Sale Price: {0:C}", estimate.TotalPrice));
            Text marginText = new Text(String.Format("Margin: %{0:F2}", estimate.Margin));

            par.Append(new Break());
            par.Append(new Run(saleText));
            par.Append(new Break());
            par.Append(new Run(marginText));

            return par;
        }
    }
}
