using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using EstimatingLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EstimatingLibrary.Utilities;

//Excel example: https://blogs.msdn.microsoft.com/chrisrae/2011/08/18/creating-a-simple-xlsx-from-scratch-using-the-open-xml-sdk/

namespace EstimatingUtilitiesLibrary.Exports
{
    public class Budget
    {
        
        public static void GenerateReport(string filePath, TECBid bid)
        {
            using (SpreadsheetDocument package = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook))
            {
                CreateParts(package, bid);
            }
        }
        
        private static void CreateParts(SpreadsheetDocument document, TECBid bid)
        {
            WorkbookPart workbook = document.AddWorkbookPart();
            GenerateWorkbookContent(workbook);

            WorksheetPart worksheet = workbook.AddNewPart<WorksheetPart>("rId1");
            GenerateWorksheetContent(worksheet, bid);
        }
        
        private static void GenerateWorkbookContent(WorkbookPart workbook)
        {
            Workbook workbook1 = new Workbook();
            workbook1.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");

            Sheets sheets1 = new Sheets();
            Sheet sheet1 = new Sheet() { Name = "Sheet1", SheetId = (UInt32Value)1U, Id = "rId1" };
            sheets1.Append(sheet1);
            
            //Stylesheet stylesheet = workbook1.WorkbookPart.WorkbookStylesPart.Stylesheet;
            //stylesheet.CellFormats = new CellFormats();
            //stylesheet.CellFormats.Count = 1;
            //CellFormat moneyFormat = stylesheet.CellFormats.AppendChild(new CellFormat());
            //moneyFormat.
            //workbook1.WorkbookPart.WorkbookStylesPart.Stylesheet.Save();

            workbook1.Append(sheets1);
            workbook.Workbook = workbook1;
        }
        
        private static void GenerateWorksheetContent(WorksheetPart worksheet, TECBid bid)
        {
            Worksheet worksheet1 = new Worksheet();
            SheetData sheetData1 = new SheetData();

            Row headerRow = new Row();
            headerRow.AppendChild(new Cell());

            Cell headerCell = new Cell();
            headerCell.DataType = CellValues.String;
            headerCell.CellValue = new CellValue("Quantity");
            headerRow.AppendChild(headerCell);

            headerCell = new Cell();
            headerCell.DataType = CellValues.String;
            headerCell.CellValue = new CellValue("Price");
            headerRow.AppendChild(headerCell);
            sheetData1.AppendChild(headerRow);

            Row systemRow = new Row();
            Cell systemCell = new Cell();
            systemCell.DataType = CellValues.String;
            systemCell.CellValue = new CellValue("Systems");
            systemRow.AppendChild(systemCell);
            sheetData1.AppendChild(systemRow);

            foreach (TECTypical typical in bid.Systems)
            {
                TECEstimator systemEstimate = new TECEstimator(typical, bid.Parameters, new ChangeWatcher(typical));

                Row row = new Row();
                //worksheet1.AppendChild(row);
                Cell cell = new Cell();
                cell.DataType = CellValues.String;
                cell.CellValue = new CellValue(typical.Name);
                row.AppendChild(cell);

                cell = new Cell();
                cell.DataType = CellValues.Number;
                cell.CellValue = new CellValue(typical.Instances.Count.ToString());
                row.AppendChild(cell);

                cell = new Cell();
                cell.DataType = CellValues.Number;
                cell.CellValue = new CellValue(systemEstimate.TotalPrice.ToString());
                row.AppendChild(cell);
                sheetData1.AppendChild(row);
            }

            sheetData1.AppendChild(new Row());

            Row riserRow = new Row();
            Cell riserCell = new Cell();
            riserCell.DataType = CellValues.String;
            riserCell.CellValue = new CellValue("BMS Network");
            riserRow.AppendChild(riserCell);
            sheetData1.AppendChild(riserRow);

            foreach (TECController controller in bid.Controllers)
            {
                TECEstimator systemEstimate = new TECEstimator(controller, bid.Parameters, new ChangeWatcher(controller));

                Row row = new Row();
                Cell cell = new Cell();
                cell.DataType = CellValues.String;
                cell.CellValue = new CellValue(controller.Name);
                row.AppendChild(cell);

                cell = new Cell();
                cell.DataType = CellValues.String;
                cell.CellValue = new CellValue("1");
                row.AppendChild(cell);

                cell = new Cell();
                cell.DataType = CellValues.String;
                cell.CellValue = new CellValue(String.Format("{0:C}", systemEstimate.TotalPrice));
                row.AppendChild(cell);
                sheetData1.AppendChild(row);
            }

            foreach (TECPanel panel in bid.Panels)
            {
                TECEstimator systemEstimate = new TECEstimator(panel, bid.Parameters, new ChangeWatcher(panel));

                Row row = new Row();
                //worksheet1.AppendChild(row);
                Cell cell = new Cell();
                cell.DataType = CellValues.String;
                cell.CellValue = new CellValue(panel.Name);
                row.AppendChild(cell);

                cell = new Cell();
                cell.DataType = CellValues.String;
                cell.CellValue = new CellValue("1");
                row.AppendChild(cell);

                cell = new Cell();
                cell.DataType = CellValues.String;
                cell.CellValue = new CellValue(String.Format("{0:C}", systemEstimate.TotalPrice));
                row.AppendChild(cell);
                sheetData1.AppendChild(row);
            }

            sheetData1.AppendChild(new Row());

            Row miscRow = new Row();
            Cell miscCell = new Cell();
            miscCell.DataType = CellValues.String;
            miscCell.CellValue = new CellValue("Miscellaneous");
            miscRow.AppendChild(miscCell);
            sheetData1.AppendChild(miscRow);

            foreach (TECMisc misc in bid.MiscCosts)
            {
                TECEstimator systemEstimate = new TECEstimator(misc, bid.Parameters, new ChangeWatcher(misc));

                Row row = new Row();
                Cell cell = new Cell();
                cell.DataType = CellValues.String;
                cell.CellValue = new CellValue(misc.Name);
                row.AppendChild(cell);

                cell = new Cell();
                cell.DataType = CellValues.String;
                cell.CellValue = new CellValue(misc.Quantity.ToString());
                row.AppendChild(cell);

                cell = new Cell();
                cell.DataType = CellValues.String;
                cell.CellValue = new CellValue(String.Format("{0:C}", systemEstimate.TotalPrice));
                row.AppendChild(cell);
                sheetData1.AppendChild(row);
            }

            sheetData1.AppendChild(new Row());

            Row totalRow = new Row();
            totalRow.AppendChild(new Cell());
            Cell totalCell = new Cell();
            totalCell.DataType = CellValues.String;
            totalCell.CellValue = new CellValue("Total: ");
            totalRow.AppendChild(totalCell);

            totalCell = new Cell();
            totalCell.DataType = CellValues.String;
            totalCell.CellValue = new CellValue(String.Format("{0:C}", new TECEstimator(bid, new ChangeWatcher(bid)).TotalPrice));
            totalRow.AppendChild(totalCell);

            sheetData1.AppendChild(totalRow);

            worksheet1.Append(sheetData1);
            worksheet.Worksheet = worksheet1;
        }
        
    }
}
