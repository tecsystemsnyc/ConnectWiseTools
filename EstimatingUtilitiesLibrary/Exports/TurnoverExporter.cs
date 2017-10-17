using DocumentFormat.OpenXml.Packaging;
using EstimatingLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Wordprocessing;

namespace EstimatingUtilitiesLibrary.Exports
{
    public static class TurnoverExporter
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
