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

        public static void GenerateEngineeringExport(string path, TECBid bid, TECEstimator estimate)
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
                body.Append(engSummary(estimate));

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
                foreach(TECSubScope subScope in typical.GetAllSubScope())
                {
                    devices.AddRange(subScope.Devices);
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

            }
            workbook.SaveAs(path);
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

        private static Paragraph engSummary(TECEstimator estimate)
        {
            Paragraph par = new Paragraph();
            Text laborText = new Text(String.Format("Engineering Labor: {0}", estimate.ENGLaborHours));
            Text materialText = new Text(String.Format("Material Cost: {0}", estimate.TECMaterialCost));
            Text subLaborText = new Text(String.Format("SubContractor Labor: {0}", estimate.SubcontractorLaborHours));
            Text subMaterialText = new Text(String.Format("SubContractor Material: {0}", estimate.ElectricalMaterialCost));
            par.Append(new Run(laborText));
            par.Append(new Break());
            par.Append(new Run(materialText));
            par.Append(new Break());
            par.Append(new Run(subLaborText));
            par.Append(new Break());
            par.Append(new Run(subMaterialText));

            return par;
        }
    }
}
