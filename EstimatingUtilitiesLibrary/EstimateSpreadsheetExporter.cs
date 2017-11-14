using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using EstimatingLibrary;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EstimatingUtilitiesLibrary
{
    //Reference: http://stackoverflow.com/questions/527028/open-xml-sdk-2-0-how-to-update-a-cell-in-a-spreadsheet
    public static class EstimateSpreadsheetExporter
    {

        public static void Export(TECBid bid, string path)
        {
            File.Copy(@"C:\Users\dtaylor\Dropbox (TEC Systems)\Sales\Estimating tools\REV.5.Estimate 2017.01.xlsx", path);
            using (SpreadsheetDocument spreadSheet = SpreadsheetDocument.Open(path, true))
            {
                WorksheetPart worksheetPart = GetWorksheetPartByName(spreadSheet, "Points List");
                if (worksheetPart != null)
                {
                    int x = 5;
                    foreach (TECSystem system in bid.Systems)
                    {
                        var currentSystem = system;
                        foreach (TECEquipment equipment in system.Equipment)
                        {
                            var currentEquipment = equipment;
                            foreach (TECSubScope subScope in equipment.SubScope)
                            {
                                worksheetPart.Worksheet.InsertAt(new Row(), x);
                                var subScopeInsert = new SubScopeRow();
                                subScopeInsert.System = currentSystem;
                                subScopeInsert.Equipment = currentEquipment;
                                subScopeInsert.SubScope = subScope;
                                insertSubScopeIntoSheet(subScopeInsert, x, worksheetPart.Worksheet);
                                x++;

                            }
                        }
                    }
                    Cell cell = GetCell(worksheetPart.Worksheet, "C", 5);

                    cell.CellValue = new CellValue("test");
                    cell.DataType = new EnumValue<CellValues>(CellValues.Number);

                    // Save the worksheet.
                    worksheetPart.Worksheet.Save();
                }
            }
        }

        private static WorksheetPart GetWorksheetPartByName(SpreadsheetDocument document, string sheetName)
        {
            IEnumerable<Sheet> sheets =
               document.WorkbookPart.Workbook.GetFirstChild<Sheets>().
               Elements<Sheet>().Where(s => s.Name == sheetName);

            if (sheets.Count() == 0)
            {
                // The specified worksheet does not exist.

                return null;
            }

            string relationshipId = sheets.First().Id.Value;
            WorksheetPart worksheetPart = (WorksheetPart)
                 document.WorkbookPart.GetPartById(relationshipId);
            return worksheetPart;

        }

        // Given a worksheet, a column name, and a row index, 
        // gets the cell at the specified column and 
        private static Cell GetCell(Worksheet worksheet, string columnName, int rowIndex)
        {
            Row row = GetRow(worksheet, rowIndex);

            if (row == null)
                return null;

            return row.Elements<Cell>().Where(c => string.Compare(c.CellReference.Value, columnName + rowIndex, true) == 0).First();
        }

        // Given a worksheet and a row index, return the row.
        private static Row GetRow(Worksheet worksheet, int rowIndex)
        {
            return worksheet.GetFirstChild<SheetData>().Elements<Row>().Where(r => r.RowIndex == rowIndex).First();
        }

        private static void insertSubScopeIntoSheet(SubScopeRow subScope, int rowIndex, Worksheet workSheet)
        {
            Cell cell = GetCell(workSheet, "A", rowIndex);
            if (subScope.SubScope.Connection.ParentController != null)
            {
                cell.CellValue = new CellValue(subScope.SubScope.Connection.ParentController.Name.ToString());
                cell.DataType = new EnumValue<CellValues>(CellValues.String);
            }

            cell = GetCell(workSheet, "B", rowIndex);
            cell.CellValue = new CellValue(subScope.System.Name.ToString());
            cell.DataType = new EnumValue<CellValues>(CellValues.String);

            cell = GetCell(workSheet, "C", rowIndex);
            cell.CellValue = new CellValue(subScope.SubScope.Name.ToString());
            cell.DataType = new EnumValue<CellValues>(CellValues.String);

            cell = GetCell(workSheet, "D", rowIndex);
            string deviceString = "";
            foreach (TECDevice device in subScope.SubScope.Devices)
            {
                deviceString += "(";
                deviceString += device.Name;
                deviceString += ") ";
            }
            cell.CellValue = new CellValue(deviceString.ToString());
            cell.DataType = new EnumValue<CellValues>(CellValues.String);

            cell = GetCell(workSheet, "E", rowIndex);
            int num = 0;
            foreach (TECPoint point in subScope.SubScope.Points)
            {
                if (point.Type == IOType.AI)
                {
                    num += point.Quantity;
                }
            }
            cell.CellValue = new CellValue(num.ToString());
            cell.DataType = new EnumValue<CellValues>(CellValues.Number);

            cell = GetCell(workSheet, "F", rowIndex);
            num = 0;
            foreach (TECPoint point in subScope.SubScope.Points)
            {
                if (point.Type == IOType.DI)
                {
                    num += point.Quantity;
                }
            }
            cell.CellValue = new CellValue(num.ToString());
            cell.DataType = new EnumValue<CellValues>(CellValues.Number);

            cell = GetCell(workSheet, "G", rowIndex);
            num = 0;
            foreach (TECPoint point in subScope.SubScope.Points)
            {
                if (point.Type == IOType.AO)
                {
                    num += point.Quantity;
                }
            }
            cell.CellValue = new CellValue(num.ToString());
            cell.DataType = new EnumValue<CellValues>(CellValues.Number);


            cell = GetCell(workSheet, "H", rowIndex);
            num = 0;
            foreach (TECPoint point in subScope.SubScope.Points)
            {
                if (point.Type == IOType.DO)
                {
                    num += point.Quantity;
                }
            }
            cell.CellValue = new CellValue(num.ToString());
            cell.DataType = new EnumValue<CellValues>(CellValues.Number);

            cell = GetCell(workSheet, "I", rowIndex);
            num = 0;
            foreach (TECPoint point in subScope.SubScope.Points)
            {
                if (TECIO.NetworkIO.Contains(point.Type))
                {
                    num += point.Quantity;
                }
            }
            cell.CellValue = new CellValue(num.ToString());
            cell.DataType = new EnumValue<CellValues>(CellValues.Number);

            //cell = GetCell(workSheet, "J", rowIndex);
            //cell.CellValue = new CellValue(subScope.SubScope.Connection.Terminations.ToString());
            //cell.DataType = new EnumValue<CellValues>(CellValues.Number);

            var conduitString = "";
            var flexNum = 0;
            if (subScope.SubScope.Connection.ConduitType != null)
            {
                conduitString = subScope.SubScope.Connection.ConduitType.Name;
                foreach (TECCost cost in subScope.SubScope.Connection.ConduitType.AssociatedCosts)
                {
                    if (cost.Name.ToUpper().Contains("FLEX"))
                    {
                        flexNum++;
                    }
                }
            }
            cell = GetCell(workSheet, "K", rowIndex);
            cell.CellValue = new CellValue(flexNum.ToString());
            cell.DataType = new EnumValue<CellValues>(CellValues.Number);

            if (conduitString.Contains("3/4"))
            {
                cell = GetCell(workSheet, "L", rowIndex);
                cell.CellValue = new CellValue(subScope.SubScope.Connection.ConduitLength.ToString());
                cell.DataType = new EnumValue<CellValues>(CellValues.Number);
            }

            if (conduitString.Contains("1\""))
            {
                cell = GetCell(workSheet, "M", rowIndex);
                cell.CellValue = new CellValue(subScope.SubScope.Connection.ConduitLength.ToString());
                cell.DataType = new EnumValue<CellValues>(CellValues.Number);
            }
            if (conduitString.Contains("1.5"))
            {
                cell = GetCell(workSheet, "N", rowIndex);
                cell.CellValue = new CellValue(subScope.SubScope.Connection.ConduitLength.ToString());
                cell.DataType = new EnumValue<CellValues>(CellValues.Number);
            }
            if (conduitString.Contains("2"))
            {
                cell = GetCell(workSheet, "O", rowIndex);
                cell.CellValue = new CellValue(subScope.SubScope.Connection.ConduitLength.ToString());
                cell.DataType = new EnumValue<CellValues>(CellValues.Number);
            }
            cell = GetCell(workSheet, "P", rowIndex);
            if (conduitString.ToUpper().Contains("EMT"))
            {
                cell.CellValue = new CellValue("EMT");
                cell.DataType = new EnumValue<CellValues>(CellValues.String);
            }
            else if (conduitString.ToUpper().Contains("RIG"))
            {
                cell.CellValue = new CellValue("RGS");
                cell.DataType = new EnumValue<CellValues>(CellValues.String);
            }


            cell = GetCell(workSheet, "Q", rowIndex);
            cell.CellValue = new CellValue("instr");
            cell.DataType = new EnumValue<CellValues>(CellValues.String);

            double length = 0;
            foreach (TECElectricalMaterial type in subScope.SubScope.Connection.GetConnectionTypes())
            {
                length += subScope.SubScope.Connection.Length;
            }
            cell = GetCell(workSheet, "R", rowIndex);
            cell.CellValue = new CellValue(length.ToString());
            cell.DataType = new EnumValue<CellValues>(CellValues.Number);

        }

    }

    public class SubScopeRow
    {
        public TECSystem System;
        public TECEquipment Equipment;
        public TECSubScope SubScope;

        public SubScopeRow()
        {

        }
    }
}
