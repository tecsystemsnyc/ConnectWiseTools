using ClosedXML.Excel;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using EstimatingLibrary;
using EstimatingLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EstimatingUtilitiesLibrary.Exports
{
    public static class Turnover
    {

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
            List<String> sheetNames = new List<string>();
            XLWorkbook workbook = new XLWorkbook();
            int postfix = 1;
            foreach(TECTypical typical in bid.Systems.Where(typ => typ.Instances.Count > 0))
            {
                List<TECCost> associatedCosts = new List<TECCost>();
                string sheetName = typical.Instances.Count > 1 ? typical.Name : typical.Instances[0].Name;
                if(sheetName == "")
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
                foreach(TECEquipment equipment in typical.Equipment)
                {
                    associatedCosts.AddRange(equipment.AssociatedCosts);
                    foreach(TECSubScope subScope in equipment.SubScope)
                    {
                        associatedCosts.AddRange(subScope.AssociatedCosts);
                        devices.AddRange(subScope.Devices);
                    }
                }
                foreach(IEndDevice device in devices.Distinct())
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
                foreach(TECController controller in typical.Controllers)
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
                foreach(TECPanel panel in typical.Panels)
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
                foreach(TECCost cost in associatedCosts.Distinct())
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

            workbook.SaveAs(path);
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
