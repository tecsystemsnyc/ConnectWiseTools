using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using EstimatingLibrary;

namespace EstimatingUtilitiesLibrary
{
    public static class ScopeWordDocumentBuilder
    {
        public static void CreateScopeWordDocument(TECBid bid, string path, bool isEstimate)
        {
            // Create a document by supplying the filepath. 
            using (WordprocessingDocument wordDocument =
                WordprocessingDocument.Create(path, WordprocessingDocumentType.Document))
            {
                // Add a main document part. 
                MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();

                // Create the document structure and add some text.
                mainPart.Document = new Document();
                Body body = mainPart.Document.AppendChild(new Body());
                Paragraph para = body.AppendChild(new Paragraph());
                Run run = para.AppendChild(new Run());
                run.AppendChild(new Text(bid.Name));

                NumberingDefinitionsPart numberingPart = mainPart.AddNewPart<NumberingDefinitionsPart>("mainList");

                Numbering numElement =
                  new Numbering(
                    new AbstractNum(
                      new Level(
                        new NumberingFormat() { Val = NumberFormatValues.CardinalText },
                        new LevelText() { Val = "·" }
                      )
                      { LevelIndex = 0 }
                    )
                    { AbstractNumberId = 1 },
                    new NumberingInstance(
                      new AbstractNumId() { Val = 1 }
                    )
                    { NumberID = 1 });

                numElement.Save(numberingPart);

                body = mainPart.Document.AppendChild(new Body());

                foreach (TECSystem system in bid.Systems)
                {
                    para = body.AppendChild(new Paragraph());
                    para.ParagraphProperties = new ParagraphProperties(
                        new NumberingProperties(
                        new NumberingLevelReference() { Val = 0 },
                        new NumberingId() { Val = 1 }));
                    run = para.AppendChild(new Run());
                    run.AppendChild(new Text(system.Name));
                }
                
            }
        }
    }
}
