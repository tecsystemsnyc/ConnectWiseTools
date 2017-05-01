using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using MigraDoc;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using MigraDoc.RtfRendering;
using PdfSharp.Pdf;
using System.Data;
using System.IO;
using MigraDoc.DocumentObjectModel.Shapes;
using EstimatingLibrary;
using EstimatingUtilitiesLibrary;
using System.Windows;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.ObjectModel;
using DebugLibrary;


namespace EstimatingUtilitiesLibrary
{
    public static class ScopeDocumentBuilder
    {
        private const string tabSize = "0.5cm";
        private const string beforeParagraphSize = "0.5cm";
        private static List<string> itemLetters = new List<string>(new string[] 
        {
            "A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z","AA","AB","AC","AD","AE","AF","AG","AH","AI","AJ","AK","AL","AM","AN","AO","AP"
        });
        private static List<string> itemNumerals = new List<string>(new string[]
        {
            "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X", "XI", "XII", "XIII", "XIV", "XV", "XVI", "XVII", "XVIII", "XIX", "XX"
        });

        public static void CreateScopeDocument(TECBid bid, string path, bool isEstimate)
        {
            Document scopeDocument = new Document();
            defineStyles(scopeDocument);

            scopeDocument.AddSection();
            createHeader(scopeDocument);
            createBidInfo(scopeDocument, bid.Name, bid.BidNumber, bid.Salesperson, bid.Estimator);
            createIntroduction(scopeDocument);
            createDocumentList(scopeDocument, bid);
            createScope(scopeDocument, bid);
            //if (isEstimate)
            //{ createPricing(scopeDocument, bid.TotalPrice); }
            //else
            //{ createPricing(scopeDocument, bid.BudgetPrice); }
            
            createNotesAndExclusions(scopeDocument, bid.Notes.ToList(), bid.Exclusions.ToList());
            createSignature(scopeDocument, bid.Salesperson);
            createFooter(scopeDocument);

            RtfDocumentRenderer rtfRenderer = new RtfDocumentRenderer();

            #region Gross String Processing
            var pathstring = path.Split('\\');
            var pathstringlength = pathstring.Length;

            var fileName = pathstring[pathstringlength - 1];
            var onlyPath = "";
            foreach (string pathChunk in pathstring){
                if (pathChunk != fileName)
                {
                    onlyPath += pathChunk;
                    onlyPath += '\\';
                }
            }
            #endregion //Gross String Processing

            rtfRenderer.Render(scopeDocument, fileName, onlyPath);
        }
        private static void createHeader(Document document)
        {
            //Add Logo
            try
            {
                string path = Path.GetTempFileName();

                path = path.Substring(0, path.Length - 3);
                path += "png";

                (Properties.Resources.TECLogo).Save(path, ImageFormat.Png);

                MigraDoc.DocumentObjectModel.Shapes.Image image = document.LastSection.AddImage(path);
                image.Height = "2.5cm";
                image.LockAspectRatio = true;
                image.RelativeVertical = RelativeVertical.Line;
                image.RelativeHorizontal = RelativeHorizontal.Margin;
                image.Top = ShapePosition.Top;
                image.Left = ShapePosition.Left;
                image.WrapFormat.Style = WrapStyle.Through;
            }
            catch (IOException e)
            {
                DebugHandler.LogError("Could not load TECLogo. Exception: " + e.Message);
            }


            //Add Address
            try
            {
                string address = Properties.Resources.TECAddress;
                var addressFrame = document.LastSection.AddTextFrame();
                addressFrame.Height = "2.5cm";
                addressFrame.Width = "4cm";
                addressFrame.Left = ShapePosition.Right;
                addressFrame.RelativeHorizontal = RelativeHorizontal.Margin;
                addressFrame.Top = ShapePosition.Top;
                addressFrame.RelativeVertical = RelativeVertical.Line;

                Paragraph addressParagraph = addressFrame.AddParagraph();
                addressParagraph.AddText(address);
            }
            catch (IOException e)
            {
                DebugHandler.LogError("Could not find TECAddress. Exception: " + e.Message);
            }
        }
        private static void createBidInfo(Document document, string bidName, string bidNo, string salesperson, string estimator)
        {
            //Add Name, Bid No and Date
            string date = DateTime.Now.Date.ToString().Split(' ')[0];
            Paragraph dateParagraph = document.LastSection.AddParagraph();
            dateParagraph.Format.Alignment = ParagraphAlignment.Right;
            dateParagraph.AddFormattedText(date);

            string bidInfo = bidName + '\n' + bidNo + "\nSalesperson: " + salesperson + "\nEstimator: " + estimator;
            Paragraph infoParagraph = document.LastSection.AddParagraph();
            infoParagraph.AddFormattedText(bidInfo);
            infoParagraph.AddLineBreak();
            infoParagraph.AddLineBreak();
        }
        private static void createIntroduction(Document document)
        {
            Paragraph paragraph = document.LastSection.AddParagraph();
            paragraph.AddFormattedText("To All Bidders:");
            paragraph = document.LastSection.AddParagraph();
            paragraph.AddLineBreak();
            paragraph.AddLineBreak();
            paragraph.AddFormattedText("    As an authorized representative of Honeywell, " +
                "Inc., T.E.C. Systems, Inc. is pleased to provide this quotation for" +
                " the Automatic Temperature Controls and Building Automation Systems as Specified." +
                " This proposal is based upon our review of the following documents:");

        }
        private static void createDocumentList(Document document, TECBid bid)
        {
            Table table = new Table();
            table.Borders.Width = 0;

            table.Format.SpaceBefore = "0.3cm";

            table.AddColumn(Unit.FromCentimeter(5.5));
            table.AddColumn(Unit.FromCentimeter(5.5));
            table.AddColumn(Unit.FromCentimeter(5.5));

            foreach (TECDrawing drawing in bid.Drawings)
            {
                Row row = table.AddRow();
                Cell cell = row.Cells[0];
                cell.AddParagraph("Drawing:");
                cell = row.Cells[1];
                cell.AddParagraph(drawing.Name);
                cell = row.Cells[2];
                cell.AddParagraph("DATE NOT IMPLEMENTED");
            }

            document.LastSection.Add(table);
        }
        private static void createScope(Document document, TECBid bid)
        {
            Paragraph paragraph = document.LastSection.AddParagraph("Scope of Work:", "Heading2");
            paragraph.Format.SpaceBefore = beforeParagraphSize;
            paragraph.Format.Shading.Color = Colors.LightGray;
            paragraph.AddLineBreak();

            int i = 0;
            foreach (TECScopeBranch branch in bid.ScopeTree)
            {
                paragraph = document.LastSection.AddParagraph();
                addScopeBranch(branch, document, paragraph, 0, (itemLetters[i] + "."));
                i++;
            }

            paragraph = document.LastSection.AddParagraph();
            paragraph.AddFormattedText(itemLetters[i] + ".");
            paragraph.AddSpace(2);
            paragraph.AddFormattedText("Provide a BMS and Automatic Temperature functions for the following mechanical systems:");
            paragraph.AddLineBreak();
            createSystemTree(document, bid);
        }
        private static void addScopeBranch(TECScopeBranch branch, Document document, Paragraph paragraph, int tabs, string tabChar = "• ")
        {
            string scopeString = branch.Name;
            if (branch.Description != "")
            {
                scopeString += ": " + branch.Description;
            }
            for (int i = 0; i < tabs; i++)
            {
                paragraph.AddTab();
            }
            paragraph.AddFormattedText(tabChar);
            if (tabChar != "• ")
            {
                paragraph.AddSpace(2);
            }
            paragraph.AddFormattedText(scopeString);
            
            foreach (TECScopeBranch childBranch in branch.Branches)
            {
                paragraph = document.LastSection.AddParagraph();
                addScopeBranch(childBranch, document, paragraph, (tabs + 1));
            }
        }
        private static void createSystemTree(Document document, TECBid bid)
        {
            Paragraph paragraph = new Paragraph();
            int sysNum = 1;
            foreach (TECProposalScope systemProp in bid.ProposalScope)
            {
                if (systemProp.IsProposed)
                {
                    paragraph = document.LastSection.AddParagraph();
                    TECScope system = systemProp.Scope;
                    paragraph.AddTab();
                    string systemString = system.Name;
                    if (system.Quantity > 1)
                    {
                        systemString += " (" + system.Quantity.ToString() + ")";
                    }
                    if (system.Description != "")
                    {
                        systemString += ": " + system.Description;
                    }

                    paragraph.AddFormattedText(sysNum.ToString() + ". ");
                    paragraph.AddFormattedText(systemString);

                    foreach (TECScopeBranch branch in systemProp.Notes)
                    {
                        paragraph = document.LastSection.AddParagraph();
                        addScopeBranch(branch, document, paragraph, 2);
                    }

                    int equipIndex = 0;
                    foreach (TECProposalScope equipProp in systemProp.Children)
                    {
                        if (equipProp.IsProposed)
                        {
                            paragraph = document.LastSection.AddParagraph();
                            TECScope equipment = equipProp.Scope;
                            paragraph.AddTab();
                            string equipmentString = equipment.Name;
                            if (equipment.Quantity > 1)
                            {
                                equipmentString += " (" + equipment.Quantity.ToString() + ")";
                            }
                            if (equipment.Description != "")
                            {
                                equipmentString += ": " + equipment.Description;
                            }

                            paragraph.AddTab();
                            paragraph.AddFormattedText(itemLetters[equipIndex].ToLower() + ". ");
                            paragraph.AddFormattedText(equipmentString);

                            foreach(TECScopeBranch branch in equipProp.Notes)
                            {
                                paragraph = document.LastSection.AddParagraph();
                                addScopeBranch(branch, document, paragraph, 3);
                            }
                            
                            int ssIndex = 0;
                            foreach (TECProposalScope ssProp in equipProp.Children)
                            {
                                if (ssProp.IsProposed)
                                {
                                    paragraph = document.LastSection.AddParagraph();
                                    TECScope subScope = ssProp.Scope;
                                    paragraph.AddTab();
                                    string subScopeString = subScope.Name;

                                    paragraph.AddTab();
                                    paragraph.AddTab();
                                    paragraph.AddFormattedText(itemNumerals[ssIndex].ToLower() + ". ");
                                    paragraph.AddFormattedText(subScopeString);

                                    foreach(TECScopeBranch branch in ssProp.Notes)
                                    {
                                        paragraph = document.LastSection.AddParagraph();
                                        addScopeBranch(branch, document, paragraph, 4);
                                    }
                                    ssIndex++;
                                }
                            }
                            equipIndex++;
                        }
                        
                    }
                    paragraph.AddLineBreak();
                    sysNum++;
                }
                
            }
        }
        private static void createPricing(Document document, double price)
        {
            Paragraph paragraph = document.LastSection.AddParagraph("Pricing:", "Heading2");
            paragraph.Format.SpaceBefore = beforeParagraphSize;
            paragraph.Format.Shading.Color = Colors.LightGray;
            
            Table table = new Table();
            table.Borders.Width = 0;

            table.Format.SpaceBefore = "0.3cm";

            table.AddColumn(Unit.FromCentimeter(8));
            table.AddColumn(Unit.FromCentimeter(8));
            Row row = table.AddRow();
            Cell cell = row.Cells[0];
            paragraph = new Paragraph();
            paragraph.AddFormattedText("Base Scope", TextFormat.Bold);
            cell.Add(paragraph);
            cell = row.Cells[1];
            paragraph = new Paragraph();
            paragraph.AddFormattedText(string.Format("{0:C}", price));
            paragraph.Format.Alignment = ParagraphAlignment.Right;
            cell.Add(paragraph);
            document.LastSection.Add(table);
        }
        private static void createNotesAndExclusions(Document document, List<TECNote> notes, List<TECExclusion> exclusions)
        {
            Paragraph paragraph = document.LastSection.AddParagraph("Notes:", "Heading2");
            paragraph.Format.SpaceBefore = beforeParagraphSize;
            paragraph.Format.Shading.Color = Colors.LightGray;
            
            foreach (TECNote note in notes)
            {
                paragraph = document.LastSection.AddParagraph();
                paragraph.AddFormattedText("•   " + note.Text);
            }
            paragraph=document.LastSection.AddParagraph("Exclusions:", "Heading2");
            paragraph.Format.SpaceBefore = beforeParagraphSize;
            paragraph.Format.Shading.Color = Colors.LightGray;
            
            foreach (TECExclusion exclusion in exclusions)
            {
                paragraph = document.LastSection.AddParagraph();
                paragraph.AddFormattedText("•   " + exclusion.Text);
                paragraph.AddLineBreak();
            }
        }
        private static void createSignature(Document document, string salesperson)
        {
            Paragraph paragraph = document.LastSection.AddParagraph();
            paragraph.Format.SpaceBefore = beforeParagraphSize;
            paragraph.AddFormattedText("If we can be of any further assistance, please contact our office.");
            paragraph.AddLineBreak();
            paragraph.AddLineBreak();
            paragraph.AddFormattedText("Very truly yours,");
            paragraph.AddLineBreak();
            paragraph.AddFormattedText("TEC Systems, Inc.", TextFormat.Bold);
            paragraph.AddLineBreak();
            paragraph.AddLineBreak();
            paragraph.AddFormattedText(salesperson);
        }
        private static void createFooter(Document document)
        {
            try
            {
                string footer = Properties.Resources.TECFooter;
                Paragraph footerParagraph = document.LastSection.Footers.Primary.AddParagraph();
                footerParagraph.AddText(footer);
                footerParagraph.Format.Font.Size = 9;
                footerParagraph.Format.Alignment = ParagraphAlignment.Center;
            }
            catch (IOException e)
            {
                DebugHandler.LogError("Could not find TECFooter. Exception: " + e.Message);
            }
        }
        private static void defineStyles(Document document)
        {
            // Get the predefined style Normal.
            MigraDoc.DocumentObjectModel.Style style = document.Styles["Normal"];
            // Because all styles are derived from Normal, the next line changes the 
            // font of the whole document. Or, more exactly, it changes the font of
            // all styles and paragraphs that do not redefine the font.
            style.Font.Name = "Times New Roman";

            // Heading1 to Heading9 are predefined styles with an outline level. An outline level
            // other than OutlineLevel.BodyText automatically creates the outline (or bookmarks) 
            // in PDF.

            style = document.Styles["Heading1"];
            style.Font.Name = "Tahoma";
            style.Font.Size = 14;
            style.Font.Bold = true;
            style.Font.Color = Colors.DarkBlue;

            style = document.Styles["Heading2"];
            style.Font.Size = 12;
            style.Font.Bold = true;

            style = document.Styles["Heading3"];
            style.Font.Size = 10;
            style.Font.Bold = true;
            style.Font.Italic = true;

            style = document.Styles[StyleNames.Header];
            style.ParagraphFormat.AddTabStop("16cm", TabAlignment.Right);

            style = document.Styles[StyleNames.Footer];
            style.ParagraphFormat.AddTabStop("8cm", TabAlignment.Center);

            // Create a new style called TextBox based on style Normal
            style = document.Styles.AddStyle("TextBox", "Normal");
            style.ParagraphFormat.Alignment = ParagraphAlignment.Justify;
            style.ParagraphFormat.Borders.Width = 2.5;
            style.ParagraphFormat.Borders.Distance = "3pt";
            style.ParagraphFormat.Shading.Color = Colors.SkyBlue;

            // Create a new style called TOC based on style Normal
            style = document.Styles.AddStyle("TOC", "Normal");
            style.ParagraphFormat.AddTabStop("16cm", TabAlignment.Right, TabLeader.Dots);
            style.ParagraphFormat.Font.Color = Colors.Blue;
            
            //List Style
            style = document.AddStyle("BulletList", "Normal");
            style.ParagraphFormat.LeftIndent = "0.5cm";
            style.ParagraphFormat.Alignment = ParagraphAlignment.Left;

        }
        private static List<string> readFromFile(string path)
        {
            string line;
            List<string> fileLines = new List<string>();
            System.IO.StreamReader file = new System.IO.StreamReader(path);
            while ((line = file.ReadLine()) != null)
            {
                fileLines.Add(line);
            }
            return fileLines;
        }
        
    }
}
