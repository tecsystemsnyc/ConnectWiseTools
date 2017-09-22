using EstimatingUtilitiesLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using EstimatingUtilitiesLibrary.Database;

namespace EstimatingUtilitiesLibraryTests
{
    public static class TestDBHelper
    {
        static private SQLiteDatabase SQLiteDB;

        public static void CreateTestBid(string path)
        {
            DatabaseGenerator.CreateBidDatabase(path);
            SQLiteDB = new SQLiteDatabase(path);
            SQLiteDB.NonQueryCommand("BEGIN TRANSACTION");

            addToBidInfoTable();
            addToParametersTable();
            addToExtraLaborTable();
            addToNoteTable();
            addToExlusionTable();
            addToScopeBranchTable();
            addToSystemTable();
            addToEquipmentTable();
            addToSubScopeTable();
            addToDeviceTable();
            addToPointTable();
            addToTagTable();
            addToManufacturerTable();
            addToDrawingTable();
            addToPageTable();
            addToLocationTable();
            addToVisualScopeTable();
            addToConnectionTypeTable();
            addToConduitTypeTable();
            addToAssociatedCostTable();
            addToSubScopeConnectionTable();
            addToNetworkConnectionTable();
            addToControllerTable();
            addToMiscTable();
            addToPanelTypeTable();
            addToPanelTable();
            addToIOModuleTable();
            addToIOTable();
            addToControllerTypeTable();

            addToBidScopeBranchTable();
            addToBidMiscTable();
            addToHardwareManufacturerTable();
            addToIOIOModuleTable();
            addToControllerConnectionTable();
            addToScopeBranchHierarchyTable();
            addToBidSystemTable();
            addToSystemEquipmentTable();
            addToEquipmentSubScopeTable();
            addToSubScopeDeviceTable();
            addToSubScopePointTable();
            addToScopeTagTable();
            addToDeviceConnectionTypeTable();
            addToLocationScopeTable();
            addToScopeAssociatedCostTable();
            addElectricalComponentRatedCostTable();
            addToConnectionConduitTypeTable();
            addToNetworkConnectionConnectionTypeTable();
            addToNetworkConnectionControllerTable();
            addToSubScopeConnectionChildrenTable();
            addToPanelPanelTypeTable();
            addToPanelControllerTable();
            addToSystemControllerTable();
            addToSystemPanelTable();
            addToSystemScopeBranchTable();
            addToSystemHierarchyTable();
            addToSystemMiscTable();
            addToCharacteristicScopeInstanceScopeTable();
            addToControllerTypeIOTable();
            addToControllerControllerTypeTable();
            
            SQLiteDB.NonQueryCommand("END TRANSACTION");
            SQLiteDB.Connection.Close();
        }

        public static void CreateTestTemplates(string path)
        {
            DatabaseGenerator.CreateTemplateDatabase(path);
            SQLiteDB = new SQLiteDatabase(path);
            SQLiteDB.NonQueryCommand("BEGIN TRANSACTION");

            addToTemplatesInfoTable();
            addToSystemTable();
            addToEquipmentTable();
            addToSubScopeTable();
            addToDeviceTable();
            addToPointTable();
            addToTagTable();
            addToManufacturerTable();
            addToConnectionTypeTable();
            addToConduitTypeTable();
            addToAssociatedCostTable();
            addToSubScopeConnectionTable();
            addToControllerTable();
            addToMiscTable();
            addToPanelTypeTable();
            addToPanelTable();
            addToIOModuleTable();
            addToIOTable();
            addToScopeBranchTable();
            addToControllerTypeTable();
            addToParametersTable();

            addToHardwareManufacturerTable();
            addToIOIOModuleTable();
            addToControllerConnectionTable();
            addToSystemEquipmentTable();
            addToEquipmentSubScopeTable();
            addToScopeTagTable();
            addToScopeAssociatedCostTable();
            addElectricalComponentRatedCostTable();
            addToConnectionConduitTypeTable();
            addToSubScopeConnectionChildrenTable();
            addToPanelPanelTypeTable();
            addToPanelControllerTable();
            addToSystemControllerTable();
            addToSystemPanelTable();
            addToSystemHierarchyTable();
            addToSystemMiscTable();
            
            addToSubScopeDeviceTable();
            addToSubScopePointTable();
            addToSystemScopeBranchTable();
            addToScopeBranchHierarchyTable();
            addToDeviceConnectionTypeTable();
            addToControllerTypeIOTable();
            addToControllerControllerTypeTable();

            SQLiteDB.NonQueryCommand("END TRANSACTION");
            SQLiteDB.Connection.Close();
        }
        
        private static void addDataToTable(TableBase table, List<string> values)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            if(table.Fields.Count != values.Count)
            {
                throw new Exception("There must be one value per field");
            }
            for(int x = 0; x < table.Fields.Count; x++)
            {
                var field = table.Fields[x];
                var value = values[x];
                data[field.Name] = value;
            }

            SQLiteDB.Insert(table.NameString, data);
        }

        #region Object Tables
        private static void addToMetadataTable()
        {
            List<string> values = new List<string>();
            values.Add("2");
            addDataToTable(new MetadataTable(), values);
        }
        private static void addToBidInfoTable()
        {
            List<string> values = new List<string>();
            values.Add("Testimate");
            values.Add("d8788062-92d2-4889-b9f2-02a7a28aff05");
            values.Add("7357");
            values.Add("1969-07-20T00:00:00.0000000");
            values.Add("Mrs. Salesperson");
            values.Add("Mr. Estimator");
            addDataToTable(new BidInfoTable(), values);

        }
        private static void addToTemplatesInfoTable()
        {
            List<string> values = new List<string>();
            values.Add("28561e73-2843-4f56-9c47-2b32031472f2");
            addDataToTable(new TemplatesInfoTable(), values);
        }
        private static void addToParametersTable()
        {
            List<string> values = new List<string>();
            values.Add("655ed4a6-4ce4-431f-ae4b-7185e28d20ef");
            values.Add("10");
            values.Add("20");
            values.Add("20");
            values.Add("20");
            values.Add("10");
            values.Add("0");
            values.Add("0");
            values.Add("0");

            values.Add("2");
            values.Add("30");
            values.Add("2");
            values.Add("40");
            values.Add("2");
            values.Add("50");
            values.Add("2");
            values.Add("60");
            values.Add("2");
            values.Add("70");

            values.Add("50");
            values.Add("60");
            values.Add("30");
            values.Add("40");
            values.Add("0.25");
            values.Add("0");
            values.Add("1");
            addDataToTable(new ParametersTable(), values);

        }
        private static void addToExtraLaborTable()
        {
            List<string> values = new List<string>();
            values.Add("d8788062-92d2-4889-b9f2-02a7a28aff05");
            values.Add("120");
            values.Add("110");
            values.Add("100");
            values.Add("90");
            values.Add("80");
            addDataToTable(new ExtraLaborTable(), values);
        }
        private static void addToNoteTable()
        {
            List<string> values = new List<string>();
            values.Add("50f3a707-fc1b-4eb3-9413-1dbde57b1d90");
            values.Add("Test Note");
            addDataToTable(new NoteTable(), values);
        }
        private static void addToExlusionTable()
        {
            List<string> values = new List<string>();
            values.Add("15692e12-e728-4f1b-b65c-de365e016e7a");
            values.Add("Test Exclusion");
            addDataToTable(new ExclusionTable(), values);
        }
        private static void addToScopeBranchTable()
        {
            List<string> values = new List<string>();
            values.Add("25e815fa-4ac7-4b69-9640-5ae220f0cd40");
            values.Add("Bid Scope Branch");
            addDataToTable(new ScopeBranchTable(), values);

            values = new List<string>();
            values.Add("814710f1-f2dd-4ae6-9bc4-9279288e4994");
            values.Add("System Scope Branch");
            addDataToTable(new ScopeBranchTable(), values);

            values = new List<string>();
            values.Add("81adfc62-20ec-466f-a2a0-430e1223f64f");
            values.Add("Bid Child Branch");
            addDataToTable(new ScopeBranchTable(), values);

            values = new List<string>();
            values.Add("542802f6-a7b1-4020-9be4-e58225c433a8");
            values.Add("System Child Branch");
            addDataToTable(new ScopeBranchTable(), values);
        }
        private static void addToSystemTable()
        {
            List<string> values = new List<string>();
            values.Add("ebdbcc85-10f4-46b3-99e7-d896679f874a");
            values.Add("Typical System");
            values.Add("Typical System Description");
            values.Add("1");
            addDataToTable(new SystemTable(), values);

            values = new List<string>();
            values.Add("ba2e71d4-a2b9-471a-9229-9fbad7432bf7");
            values.Add("Instance System");
            values.Add("Instance System Description");
            values.Add("0");
            addDataToTable(new SystemTable(), values);
        }
        private static void addToEquipmentTable()
        {
            List<string> values = new List<string>();
            values.Add("8a9bcc02-6ae2-4ac9-bbe1-e33d9a590b0e");
            values.Add("Typical Equip");
            values.Add("Typical Equip Description");
            addDataToTable(new EquipmentTable(), values);

            values = new List<string>();
            values.Add("cdd9d7f7-ff3e-44ff-990f-c1b721e0ff8d");
            values.Add("Instance Equip");
            values.Add("Instance Equip Description");
            addDataToTable(new EquipmentTable(), values);

            values = new List<string>();
            values.Add("1645886c-fce7-4380-a5c3-295f91961d16");
            values.Add("Template Equip");
            values.Add("Template Equip Description");
            addDataToTable(new EquipmentTable(), values);
        }
        private static void addToSubScopeTable()
        {
            List<string> values = new List<string>();
            values.Add("fbe0a143-e7cd-4580-a1c4-26eff0cd55a6");
            values.Add("Typical SS");
            values.Add("Typical SS Description");
            addDataToTable(new SubScopeTable(), values);

            values = new List<string>();
            values.Add("94726d87-b468-46a8-9421-3ff9725d5239");
            values.Add("Instance SS");
            values.Add("Instance SS Description");
            addDataToTable(new SubScopeTable(), values);

            values = new List<string>();
            values.Add("3ebdfd64-5249-4332-a832-ff3cc0cdb309");
            values.Add("Template SS");
            values.Add("Template SS Description");
            addDataToTable(new SubScopeTable(), values);

            values = new List<string>();
            values.Add("214dc8d1-22be-4fbf-8b6b-d66c21105f61");
            values.Add("Child SS");
            values.Add("Child SS Description");
            addDataToTable(new SubScopeTable(), values);
        }
        private static void addToDeviceTable()
        {
            List<string> values = new List<string>();
            values.Add("95135fdf-7565-4d22-b9e4-1f177febae15");
            values.Add("Test Device");
            values.Add("Test Device Description");
            values.Add("123.45");
            addDataToTable(new DeviceTable(), values);
        }
        private static void addToPointTable()
        {
            List<string> values = new List<string>();
            values.Add("03a16819-9205-4e65-a16b-96616309f171");
            values.Add("Typical Point");
            values.Add("1");
            values.Add("AI");
            addDataToTable(new PointTable(), values);

            values = new List<string>();
            values.Add("e60437bc-09a1-47eb-9fd5-78711d942a12");
            values.Add("Instance Point");
            values.Add("1");
            values.Add("AI");
            addDataToTable(new PointTable(), values);

            values = new List<string>();
            values.Add("6776a30b-0325-42ad-8aa3-3c065b4bb908");
            values.Add("Child Point");
            values.Add("1");
            values.Add("BO");
            addDataToTable(new PointTable(), values);
        }
        private static void addToTagTable()
        {
            List<string> values = new List<string>();
            values.Add("09fd531f-94f9-48ee-8d16-00e80c1d58b9");
            values.Add("Test Tag");
            addDataToTable(new TagTable(), values);
        }
        private static void addToManufacturerTable()
        {
            List<string> values = new List<string>();
            values.Add("90cd6eae-f7a3-4296-a9eb-b810a417766d");
            values.Add("Test Manufacturer");
            values.Add("0.5");
            addDataToTable(new ManufacturerTable(), values);
        }
        private static void addToDrawingTable()
        {

        }
        private static void addToPageTable()
        {

        }
        private static void addToLocationTable()
        {
            List<string> values = new List<string>();
            values.Add("4175d04b-82b1-486b-b742-b2cc875405cb");
            values.Add("Test Location");
            addDataToTable(new LocationTable(), values);
        }
        private static void addToVisualScopeTable()
        {

        }
        private static void addToConnectionTypeTable()
        {
            List<string> values = new List<string>();
            values.Add("f38867c8-3846-461f-a6fa-c941aeb723c7");
            values.Add("Test Connection Type");
            values.Add("12.48");
            values.Add("84.21");
            addDataToTable(new ConnectionTypeTable(), values);
        }
        private static void addToConduitTypeTable()
        {
            List<string> values = new List<string>();
            values.Add("8d442906-efa2-49a0-ad21-f6b27852c9ef");
            values.Add("Test Conduit Type");
            values.Add("45.67");
            values.Add("76.54");
            addDataToTable(new ConduitTypeTable(), values);
        }
        private static void addToAssociatedCostTable()
        {
            List<string> values = new List<string>();
            values.Add("1c2a7631-9e3b-4006-ada7-12d6cee52f08");
            values.Add("Test TEC Associated Cost");
            values.Add("31");
            values.Add("13");
            values.Add("TEC");
            addDataToTable(new AssociatedCostTable(), values);

            values = new List<string>();
            values.Add("63ed1eb7-c05b-440b-9e15-397f64ff05c7");
            values.Add("Test Electrical Associated Cost");
            values.Add("42");
            values.Add("24");
            values.Add("Electrical");
            addDataToTable(new AssociatedCostTable(), values);

            values = new List<string>();
            values.Add("b7c01526-c195-442f-a1f1-28d07db61144");
            values.Add("Rated Cost");
            values.Add("10");
            values.Add("5");
            values.Add("Electrical");
            addDataToTable(new AssociatedCostTable(), values);
        }
        private static void addToSubScopeConnectionTable()
        {
            List<string> values = new List<string>();
            values.Add("5723e279-ac5c-4ee0-ae01-494a0c524b5c");
            values.Add("40");
            values.Add("20");
            addDataToTable(new SubScopeConnectionTable(), values);

            values = new List<string>();
            values.Add("560ffd84-444d-4611-a346-266074f62f6f");
            values.Add("50");
            values.Add("30");
            addDataToTable(new SubScopeConnectionTable(), values);
        }
        private static void addToNetworkConnectionTable()
        {
            List<string> values = new List<string>();
            values.Add("4f93907a-9aab-4ed5-8e55-43aab2af5ef8");
            values.Add("100");
            values.Add("80");
            values.Add("BACnetIP");
            addDataToTable(new NetworkConnectionTable(), values);

            values = new List<string>();
            values.Add("99aea45e-ebeb-4c1a-8407-1d1a3540ceeb");
            values.Add("90");
            values.Add("70");
            values.Add("BACnetIP");
            addDataToTable(new NetworkConnectionTable(), values);

            values = new List<string>();
            values.Add("6aca8c22-5115-4534-a5b1-698b7e42d6c2");
            values.Add("80");
            values.Add("60");
            values.Add("BACnetIP");
            addDataToTable(new NetworkConnectionTable(), values);

            values = new List<string>();
            values.Add("e503fdd4-f299-4618-8d54-6751c3b2bc25");
            values.Add("70");
            values.Add("50");
            values.Add("BACnetIP");
            addDataToTable(new NetworkConnectionTable(), values);
        }
        private static void addToControllerTable()
        {
            List<string> values = new List<string>();
            values.Add("98e6bc3e-31dc-4394-8b54-9ca53c193f46");
            values.Add("Bid Controller");
            values.Add("Bid Controller Description");
            values.Add("Server");
            addDataToTable(new ControllerTable(), values);

            values = new List<string>();
            values.Add("1bb86714-2512-4fdd-a80f-46969753d8a0");
            values.Add("Typical Controller");
            values.Add("Typical Controller Description");
            values.Add("0");
            addDataToTable(new ControllerTable(), values);

            values = new List<string>();
            values.Add("f22913a6-e348-4a77-821f-80447621c6e0");
            values.Add("Instance Controller");
            values.Add("Instance Controller Description");
            values.Add("DDC");
            addDataToTable(new ControllerTable(), values);

            values = new List<string>();
            values.Add("973e6100-31f7-40b0-bfe7-9d64630c1c56");
            values.Add("Child Bid Controller");
            values.Add("");
            values.Add("0");
            addDataToTable(new ControllerTable(), values);

            values = new List<string>();
            values.Add("ec965fe3-b1f7-4125-a545-ec47cc1e671b");
            values.Add("Child Instance Controller");
            values.Add("");
            values.Add("0");
            addDataToTable(new ControllerTable(), values);

            values = new List<string>();
            values.Add("bf17527a-18ba-4765-a01e-8ab8de5664a3");
            values.Add("Daisy 1");
            values.Add("");
            values.Add("0");
            addDataToTable(new ControllerTable(), values);

            values = new List<string>();
            values.Add("7b6825df-57da-458a-a859-a9459c15907b");
            values.Add("Daisy 2");
            values.Add("");
            values.Add("0");
            addDataToTable(new ControllerTable(), values);

            values = new List<string>();
            values.Add("95032348-c661-470f-9bea-47dd750a47a5");
            values.Add("Child Typical Controller");
            values.Add("");
            values.Add("0");
            addDataToTable(new ControllerTable(), values);
        }
        private static void addToMiscTable()
        {
            List<string> values = new List<string>();
            values.Add("5df99701-1d7b-4fbe-843d-40793f4145a8");
            values.Add("Bid Misc");
            values.Add("1298");
            values.Add("8921");
            values.Add("2");
            values.Add("Electrical");
            addDataToTable(new MiscTable(), values);

            values = new List<string>();
            values.Add("e3ecee54-1f90-415a-b493-90a78f618476");
            values.Add("System Misc");
            values.Add("1492");
            values.Add("2941");
            values.Add("3");
            values.Add("TEC");
            addDataToTable(new MiscTable(), values);
        }
        private static void addToPanelTypeTable()
        {
            List<string> values = new List<string>();
            values.Add("04e3204c-b35f-4e1a-8a01-db07f7eb055e");
            values.Add("Test Panel Type");
            values.Add("1324");
            values.Add("4231");
            addDataToTable(new PanelTypeTable(), values);
        }
        private static void addToPanelTable()
        {
            List<string> values = new List<string>();
            values.Add("a8cdd31c-e690-4eaa-81ea-602c72904391");
            values.Add("Bid Panel");
            values.Add("Bid Panel Description");
            addDataToTable(new PanelTable(), values);

            values = new List<string>();
            values.Add("e7695d68-d79f-44a2-92f5-b303436186af");
            values.Add("Typical Panel");
            values.Add("Typical Panel Description");
            addDataToTable(new PanelTable(), values);

            values = new List<string>();
            values.Add("10b07f6c-4374-49fc-ba6f-84db65b61ffa");
            values.Add("Instance Panel");
            values.Add("Instance Panel Description");
            addDataToTable(new PanelTable(), values);
        }
        private static void addToIOModuleTable()
        {
            List<string> values = new List<string>();
            values.Add("b346378d-dc72-4dda-b275-bbe03022dd12");
            values.Add("Test IO Module");
            values.Add("Test IO Module Description");
            values.Add("2233");
            addDataToTable(new IOModuleTable(), values);

        }
        private static void addToIOTable()
        {
            List<string> values = new List<string>();
            values.Add("1f6049cc-4dd6-4b50-a9d5-045b629ae6fb");
            values.Add("BACnetIP");
            values.Add("2");
            addDataToTable(new IOTable(), values);
            
        }
        private static void addToControllerTypeTable()
        {
            var values = new List<string>();
            values.Add("7201ca48-f885-4a87-afa7-61b3e6942697");
            values.Add("Test Controller Type");
            values.Add("Test controller type description");
            values.Add("142");
            values.Add("12");
            values.Add("DDC");
            addDataToTable(new ControllerTypeTable(), values);
        }
        #endregion

        #region Relationship Tables
        private static void addToBidScopeBranchTable()
        {
            List<string> values = new List<string>();
            values.Add("d8788062-92d2-4889-b9f2-02a7a28aff05");
            values.Add("25e815fa-4ac7-4b69-9640-5ae220f0cd40");
            addDataToTable(new BidScopeBranchTable(), values);
        }
        private static void addToBidMiscTable()
        {
            List<string> values = new List<string>();
            values.Add("d8788062-92d2-4889-b9f2-02a7a28aff05");
            values.Add("5df99701-1d7b-4fbe-843d-40793f4145a8");
            addDataToTable(new BidMiscTable(), values);
        }
        private static void addToIOIOModuleTable()
        {
            List<string> values = new List<string>();
            values.Add("b346378d-dc72-4dda-b275-bbe03022dd12");
            values.Add("1f6049cc-4dd6-4b50-a9d5-045b629ae6fb");
            addDataToTable(new IOModuleIOTable(), values);

            values = new List<string>();
            values.Add("b346378d-dc72-4dda-b275-bbe03022dd12");
            values.Add("fbae3851-3320-4e94-a674-ddec86bc4964");
            addDataToTable(new IOModuleIOTable(), values);

            values = new List<string>();
            values.Add("b346378d-dc72-4dda-b275-bbe03022dd12");
            values.Add("434bc312-f933-40c8-b8bd-f4e22f19f606");
            addDataToTable(new IOModuleIOTable(), values);
        }
        private static void addToControllerConnectionTable()
        {
            List<string> values = new List<string>();
            values.Add("98e6bc3e-31dc-4394-8b54-9ca53c193f46");
            values.Add("4f93907a-9aab-4ed5-8e55-43aab2af5ef8");
            addDataToTable(new ControllerConnectionTable(), values);

            values = new List<string>();
            values.Add("1bb86714-2512-4fdd-a80f-46969753d8a0");
            values.Add("5723e279-ac5c-4ee0-ae01-494a0c524b5c");
            addDataToTable(new ControllerConnectionTable(), values);

            values = new List<string>();
            values.Add("f22913a6-e348-4a77-821f-80447621c6e0");
            values.Add("560ffd84-444d-4611-a346-266074f62f6f");
            addDataToTable(new ControllerConnectionTable(), values);

            values = new List<string>();
            values.Add("98e6bc3e-31dc-4394-8b54-9ca53c193f46");
            values.Add("6aca8c22-5115-4534-a5b1-698b7e42d6c2");
            addDataToTable(new ControllerConnectionTable(), values);

            values = new List<string>();
            values.Add("98e6bc3e-31dc-4394-8b54-9ca53c193f46");
            values.Add("99aea45e-ebeb-4c1a-8407-1d1a3540ceeb");
            addDataToTable(new ControllerConnectionTable(), values);

            values = new List<string>();
            values.Add("f22913a6-e348-4a77-821f-80447621c6e0");
            values.Add("e503fdd4-f299-4618-8d54-6751c3b2bc25");
            addDataToTable(new ControllerConnectionTable(), values);
        }
        private static void addToScopeBranchHierarchyTable()
        {
            List<string> values = new List<string>();
            values.Add("25e815fa-4ac7-4b69-9640-5ae220f0cd40");
            values.Add("81adfc62-20ec-466f-a2a0-430e1223f64f");
            addDataToTable(new ScopeBranchHierarchyTable(), values);

            values = new List<string>();
            values.Add("814710f1-f2dd-4ae6-9bc4-9279288e4994");
            values.Add("542802f6-a7b1-4020-9be4-e58225c433a8");
            addDataToTable(new ScopeBranchHierarchyTable(), values);
        }
        private static void addToBidSystemTable()
        {
            List<string> values = new List<string>();
            values.Add("d8788062-92d2-4889-b9f2-02a7a28aff05");
            values.Add("ebdbcc85-10f4-46b3-99e7-d896679f874a");
            values.Add("0");
            addDataToTable(new BidSystemTable(), values);
        }
        private static void addToSystemEquipmentTable()
        {
            List<string> values = new List<string>();
            values.Add("ebdbcc85-10f4-46b3-99e7-d896679f874a");
            values.Add("8a9bcc02-6ae2-4ac9-bbe1-e33d9a590b0e");
            values.Add("0");
            addDataToTable(new SystemEquipmentTable(), values);

            values = new List<string>();
            values.Add("ba2e71d4-a2b9-471a-9229-9fbad7432bf7");
            values.Add("cdd9d7f7-ff3e-44ff-990f-c1b721e0ff8d");
            values.Add("0");
            addDataToTable(new SystemEquipmentTable(), values);
        }
        private static void addToEquipmentSubScopeTable()
        {
            List<string> values = new List<string>();
            values.Add("8a9bcc02-6ae2-4ac9-bbe1-e33d9a590b0e");
            values.Add("fbe0a143-e7cd-4580-a1c4-26eff0cd55a6");
            values.Add("0");
            addDataToTable(new EquipmentSubScopeTable(), values);

            values = new List<string>();
            values.Add("cdd9d7f7-ff3e-44ff-990f-c1b721e0ff8d");
            values.Add("94726d87-b468-46a8-9421-3ff9725d5239");
            values.Add("0");
            addDataToTable(new EquipmentSubScopeTable(), values);

            values = new List<string>();
            values.Add("1645886c-fce7-4380-a5c3-295f91961d16");
            values.Add("214dc8d1-22be-4fbf-8b6b-d66c21105f61");
            values.Add("0");
            addDataToTable(new EquipmentSubScopeTable(), values);
        }
        private static void addToSubScopeDeviceTable()
        {
            List<string> values = new List<string>();
            values.Add("fbe0a143-e7cd-4580-a1c4-26eff0cd55a6");
            values.Add("95135fdf-7565-4d22-b9e4-1f177febae15");
            values.Add("2");
            values.Add("0");
            addDataToTable(new SubScopeDeviceTable(), values);

            values = new List<string>();
            values.Add("94726d87-b468-46a8-9421-3ff9725d5239");
            values.Add("95135fdf-7565-4d22-b9e4-1f177febae15");
            values.Add("2");
            values.Add("0");
            addDataToTable(new SubScopeDeviceTable(), values);

            values = new List<string>();
            values.Add("3ebdfd64-5249-4332-a832-ff3cc0cdb309");
            values.Add("95135fdf-7565-4d22-b9e4-1f177febae15");
            values.Add("3");
            values.Add("0");
            addDataToTable(new SubScopeDeviceTable(), values);
        }
        private static void addToSubScopePointTable()
        {
            List<string> values = new List<string>();
            values.Add("fbe0a143-e7cd-4580-a1c4-26eff0cd55a6");
            values.Add("03a16819-9205-4e65-a16b-96616309f171");
            addDataToTable(new SubScopePointTable(), values);

            values = new List<string>();
            values.Add("94726d87-b468-46a8-9421-3ff9725d5239");
            values.Add("e60437bc-09a1-47eb-9fd5-78711d942a12");
            addDataToTable(new SubScopePointTable(), values);

            values = new List<string>();
            values.Add("3ebdfd64-5249-4332-a832-ff3cc0cdb309");
            values.Add("6776a30b-0325-42ad-8aa3-3c065b4bb908");
            addDataToTable(new SubScopePointTable(), values);
        }
        private static void addToScopeTagTable()
        {
            List<string> values = new List<string>();
            values.Add("98e6bc3e-31dc-4394-8b54-9ca53c193f46");
            values.Add("09fd531f-94f9-48ee-8d16-00e80c1d58b9");
            addDataToTable(new ScopeTagTable(), values);

            values = new List<string>();
            values.Add("a8cdd31c-e690-4eaa-81ea-602c72904391");
            values.Add("09fd531f-94f9-48ee-8d16-00e80c1d58b9");
            addDataToTable(new ScopeTagTable(), values);

            values = new List<string>();
            values.Add("5df99701-1d7b-4fbe-843d-40793f4145a8");
            values.Add("09fd531f-94f9-48ee-8d16-00e80c1d58b9");
            addDataToTable(new ScopeTagTable(), values);

            values = new List<string>();
            values.Add("25e815fa-4ac7-4b69-9640-5ae220f0cd40");
            values.Add("09fd531f-94f9-48ee-8d16-00e80c1d58b9");
            addDataToTable(new ScopeTagTable(), values);

            values = new List<string>();
            values.Add("ebdbcc85-10f4-46b3-99e7-d896679f874a");
            values.Add("09fd531f-94f9-48ee-8d16-00e80c1d58b9");
            addDataToTable(new ScopeTagTable(), values);

            values = new List<string>();
            values.Add("8a9bcc02-6ae2-4ac9-bbe1-e33d9a590b0e");
            values.Add("09fd531f-94f9-48ee-8d16-00e80c1d58b9");
            addDataToTable(new ScopeTagTable(), values);

            values = new List<string>();
            values.Add("fbe0a143-e7cd-4580-a1c4-26eff0cd55a6");
            values.Add("09fd531f-94f9-48ee-8d16-00e80c1d58b9");
            addDataToTable(new ScopeTagTable(), values);

            values = new List<string>();
            values.Add("03a16819-9205-4e65-a16b-96616309f171");
            values.Add("09fd531f-94f9-48ee-8d16-00e80c1d58b9");
            addDataToTable(new ScopeTagTable(), values);

            values = new List<string>();
            values.Add("1bb86714-2512-4fdd-a80f-46969753d8a0");
            values.Add("09fd531f-94f9-48ee-8d16-00e80c1d58b9");
            addDataToTable(new ScopeTagTable(), values);

            values = new List<string>();
            values.Add("e7695d68-d79f-44a2-92f5-b303436186af");
            values.Add("09fd531f-94f9-48ee-8d16-00e80c1d58b9");
            addDataToTable(new ScopeTagTable(), values);

            values = new List<string>();
            values.Add("e3ecee54-1f90-415a-b493-90a78f618476");
            values.Add("09fd531f-94f9-48ee-8d16-00e80c1d58b9");
            addDataToTable(new ScopeTagTable(), values);

            values = new List<string>();
            values.Add("814710f1-f2dd-4ae6-9bc4-9279288e4994");
            values.Add("09fd531f-94f9-48ee-8d16-00e80c1d58b9");
            addDataToTable(new ScopeTagTable(), values);

            values = new List<string>();
            values.Add("ba2e71d4-a2b9-471a-9229-9fbad7432bf7");
            values.Add("09fd531f-94f9-48ee-8d16-00e80c1d58b9");
            addDataToTable(new ScopeTagTable(), values);

            values = new List<string>();
            values.Add("cdd9d7f7-ff3e-44ff-990f-c1b721e0ff8d");
            values.Add("09fd531f-94f9-48ee-8d16-00e80c1d58b9");
            addDataToTable(new ScopeTagTable(), values);

            values = new List<string>();
            values.Add("94726d87-b468-46a8-9421-3ff9725d5239");
            values.Add("09fd531f-94f9-48ee-8d16-00e80c1d58b9");
            addDataToTable(new ScopeTagTable(), values);

            values = new List<string>();
            values.Add("e60437bc-09a1-47eb-9fd5-78711d942a12");
            values.Add("09fd531f-94f9-48ee-8d16-00e80c1d58b9");
            addDataToTable(new ScopeTagTable(), values);

            values = new List<string>();
            values.Add("f22913a6-e348-4a77-821f-80447621c6e0");
            values.Add("09fd531f-94f9-48ee-8d16-00e80c1d58b9");
            addDataToTable(new ScopeTagTable(), values);

            values = new List<string>();
            values.Add("10b07f6c-4374-49fc-ba6f-84db65b61ffa");
            values.Add("09fd531f-94f9-48ee-8d16-00e80c1d58b9");
            addDataToTable(new ScopeTagTable(), values);

            values = new List<string>();
            values.Add("95135fdf-7565-4d22-b9e4-1f177febae15");
            values.Add("09fd531f-94f9-48ee-8d16-00e80c1d58b9");
            addDataToTable(new ScopeTagTable(), values);

            values = new List<string>();
            values.Add("81adfc62-20ec-466f-a2a0-430e1223f64f");
            values.Add("09fd531f-94f9-48ee-8d16-00e80c1d58b9");
            addDataToTable(new ScopeTagTable(), values);

            values = new List<string>();
            values.Add("542802f6-a7b1-4020-9be4-e58225c433a8");
            values.Add("09fd531f-94f9-48ee-8d16-00e80c1d58b9");
            addDataToTable(new ScopeTagTable(), values);

            values = new List<string>();
            values.Add("1645886c-fce7-4380-a5c3-295f91961d16");
            values.Add("09fd531f-94f9-48ee-8d16-00e80c1d58b9");
            addDataToTable(new ScopeTagTable(), values);

            values = new List<string>();
            values.Add("3ebdfd64-5249-4332-a832-ff3cc0cdb309");
            values.Add("09fd531f-94f9-48ee-8d16-00e80c1d58b9");
            addDataToTable(new ScopeTagTable(), values);
        }
        private static void addToHardwareManufacturerTable()
        {
            List<string> values = new List<string>();
            values.Add("95135fdf-7565-4d22-b9e4-1f177febae15");
            values.Add("90cd6eae-f7a3-4296-a9eb-b810a417766d");
            addDataToTable(new HardwareManufacturerTable(), values);

            values = new List<string>();
            values.Add("04e3204c-b35f-4e1a-8a01-db07f7eb055e");
            values.Add("90cd6eae-f7a3-4296-a9eb-b810a417766d");
            addDataToTable(new HardwareManufacturerTable(), values);

            values = new List<string>();
            values.Add("7201ca48-f885-4a87-afa7-61b3e6942697");
            values.Add("90cd6eae-f7a3-4296-a9eb-b810a417766d");
            addDataToTable(new HardwareManufacturerTable(), values);

            values = new List<string>();
            values.Add("b346378d-dc72-4dda-b275-bbe03022dd12");
            values.Add("90cd6eae-f7a3-4296-a9eb-b810a417766d");
            addDataToTable(new HardwareManufacturerTable(), values);
        }
        private static void addToDeviceConnectionTypeTable()
        {
            List<string> values = new List<string>();
            values.Add("95135fdf-7565-4d22-b9e4-1f177febae15");
            values.Add("f38867c8-3846-461f-a6fa-c941aeb723c7");
            values.Add("2");
            addDataToTable(new DeviceConnectionTypeTable(), values);
        }
        private static void addToLocationScopeTable()
        {
            List<string> values = new List<string>();
            values.Add("ba2e71d4-a2b9-471a-9229-9fbad7432bf7");
            values.Add("4175d04b-82b1-486b-b742-b2cc875405cb");
            addDataToTable(new LocatedLocationTable(), values);

            values = new List<string>();
            values.Add("cdd9d7f7-ff3e-44ff-990f-c1b721e0ff8d");
            values.Add("4175d04b-82b1-486b-b742-b2cc875405cb");
            addDataToTable(new LocatedLocationTable(), values);

            values = new List<string>();
            values.Add("94726d87-b468-46a8-9421-3ff9725d5239");
            values.Add("4175d04b-82b1-486b-b742-b2cc875405cb");
            addDataToTable(new LocatedLocationTable(), values);
        }
        private static void addToScopeAssociatedCostTable()
        {
            string tecCostGuid = "1c2a7631-9e3b-4006-ada7-12d6cee52f08";
            string electricalCostGuid = "63ed1eb7-c05b-440b-9e15-397f64ff05c7";
            string scopeGuid = "";

            List<string> values = new List<string>();
            scopeGuid = "98e6bc3e-31dc-4394-8b54-9ca53c193f46";
            values.Add(scopeGuid);
            values.Add(tecCostGuid);
            values.Add("1");
            addDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            values.Add(scopeGuid);
            values.Add(electricalCostGuid);
            values.Add("1");
            addDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            scopeGuid = "a8cdd31c-e690-4eaa-81ea-602c72904391";
            values.Add(scopeGuid);
            values.Add(tecCostGuid);
            values.Add("1");
            addDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            values.Add(scopeGuid);
            values.Add(electricalCostGuid);
            values.Add("1");
            addDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            scopeGuid = "ebdbcc85-10f4-46b3-99e7-d896679f874a";
            values.Add(scopeGuid);
            values.Add(tecCostGuid);
            values.Add("1");
            addDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            values.Add(scopeGuid);
            values.Add(electricalCostGuid);
            values.Add("1");
            addDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            scopeGuid = "8a9bcc02-6ae2-4ac9-bbe1-e33d9a590b0e";
            values.Add(scopeGuid);
            values.Add(tecCostGuid);
            values.Add("1");
            addDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            values.Add(scopeGuid);
            values.Add(electricalCostGuid);
            values.Add("1");
            addDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            scopeGuid = "fbe0a143-e7cd-4580-a1c4-26eff0cd55a6";
            values.Add(scopeGuid);
            values.Add(tecCostGuid);
            values.Add("1");
            addDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            values.Add(scopeGuid);
            values.Add(electricalCostGuid);
            values.Add("1");
            addDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            scopeGuid = "03a16819-9205-4e65-a16b-96616309f171";
            values.Add(scopeGuid);
            values.Add(tecCostGuid);
            values.Add("1");
            addDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            values.Add(scopeGuid);
            values.Add(electricalCostGuid);
            values.Add("1");
            addDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            scopeGuid = "1bb86714-2512-4fdd-a80f-46969753d8a0";
            values.Add(scopeGuid);
            values.Add(tecCostGuid);
            values.Add("1");
            addDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            values.Add(scopeGuid);
            values.Add(electricalCostGuid);
            values.Add("1");
            addDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            scopeGuid = "e7695d68-d79f-44a2-92f5-b303436186af";
            values.Add(scopeGuid);
            values.Add(tecCostGuid);
            values.Add("1");
            addDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            values.Add(scopeGuid);
            values.Add(electricalCostGuid);
            values.Add("1");
            addDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            scopeGuid = "ba2e71d4-a2b9-471a-9229-9fbad7432bf7";
            values.Add(scopeGuid);
            values.Add(tecCostGuid);
            values.Add("1");
            addDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            values.Add(scopeGuid);
            values.Add(electricalCostGuid);
            values.Add("1");
            addDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            scopeGuid = "cdd9d7f7-ff3e-44ff-990f-c1b721e0ff8d";
            values.Add(scopeGuid);
            values.Add(tecCostGuid);
            values.Add("1");
            addDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            values.Add(scopeGuid);
            values.Add(electricalCostGuid);
            values.Add("1");
            addDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            scopeGuid = "94726d87-b468-46a8-9421-3ff9725d5239";
            values.Add(scopeGuid);
            values.Add(tecCostGuid);
            values.Add("1");
            addDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            values.Add(scopeGuid);
            values.Add(electricalCostGuid);
            values.Add("1");
            addDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            scopeGuid = "e60437bc-09a1-47eb-9fd5-78711d942a12";
            values.Add(scopeGuid);
            values.Add(tecCostGuid);
            values.Add("1");
            addDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            values.Add(scopeGuid);
            values.Add(electricalCostGuid);
            values.Add("1");
            addDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            scopeGuid = "f22913a6-e348-4a77-821f-80447621c6e0";
            values.Add(scopeGuid);
            values.Add(tecCostGuid);
            values.Add("1");
            addDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            values.Add(scopeGuid);
            values.Add(electricalCostGuid);
            values.Add("1");
            addDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            scopeGuid = "10b07f6c-4374-49fc-ba6f-84db65b61ffa";
            values.Add(scopeGuid);
            values.Add(tecCostGuid);
            values.Add("1");
            addDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            values.Add(scopeGuid);
            values.Add(electricalCostGuid);
            values.Add("1");
            addDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            scopeGuid = "95135fdf-7565-4d22-b9e4-1f177febae15";
            values.Add(scopeGuid);
            values.Add(tecCostGuid);
            values.Add("1");
            addDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            values.Add(scopeGuid);
            values.Add(electricalCostGuid);
            values.Add("1");
            addDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            scopeGuid = "f38867c8-3846-461f-a6fa-c941aeb723c7";
            values.Add(scopeGuid);
            values.Add(tecCostGuid);
            values.Add("1");
            addDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            values.Add(scopeGuid);
            values.Add(electricalCostGuid);
            values.Add("1");
            addDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            scopeGuid = "8d442906-efa2-49a0-ad21-f6b27852c9ef";
            values.Add(scopeGuid);
            values.Add(tecCostGuid);
            values.Add("1");
            addDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            values.Add(scopeGuid);
            values.Add(electricalCostGuid);
            values.Add("1");
            addDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            scopeGuid = "04e3204c-b35f-4e1a-8a01-db07f7eb055e";
            values.Add(scopeGuid);
            values.Add(tecCostGuid);
            values.Add("1");
            addDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            values.Add(scopeGuid);
            values.Add(electricalCostGuid);
            values.Add("1");
            addDataToTable(new ScopeAssociatedCostTable(), values);

            //Template Equipment
            scopeGuid = "1645886c-fce7-4380-a5c3-295f91961d16";
            values = new List<string>();
            values.Add(scopeGuid);
            values.Add(tecCostGuid);
            values.Add("1");
            addDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            values.Add(scopeGuid);
            values.Add(electricalCostGuid);
            values.Add("1");
            addDataToTable(new ScopeAssociatedCostTable(), values);

            //Template SubScope
            scopeGuid = "3ebdfd64-5249-4332-a832-ff3cc0cdb309";
            values = new List<string>();
            values.Add(scopeGuid);
            values.Add(tecCostGuid);
            values.Add("1");
            addDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            values.Add(scopeGuid);
            values.Add(electricalCostGuid);
            values.Add("1");
            addDataToTable(new ScopeAssociatedCostTable(), values);

        }
        private static void addElectricalComponentRatedCostTable()
        {
            List<string> values = new List<string>();
            values.Add("f38867c8-3846-461f-a6fa-c941aeb723c7");
            values.Add("b7c01526-c195-442f-a1f1-28d07db61144");
            values.Add("1");
            addDataToTable(new ElectricalMaterialRatedCostTable(), values);

            values = new List<string>();
            values.Add("8d442906-efa2-49a0-ad21-f6b27852c9ef");
            values.Add("b7c01526-c195-442f-a1f1-28d07db61144");
            values.Add("1");
            addDataToTable(new ElectricalMaterialRatedCostTable(), values);
            
        }
        private static void addToConnectionConduitTypeTable()
        {
            List<string> values = new List<string>();
            values.Add("4f93907a-9aab-4ed5-8e55-43aab2af5ef8");
            values.Add("8d442906-efa2-49a0-ad21-f6b27852c9ef");
            addDataToTable(new ConnectionConduitTypeTable(), values);

            values = new List<string>();
            values.Add("5723e279-ac5c-4ee0-ae01-494a0c524b5c");
            values.Add("8d442906-efa2-49a0-ad21-f6b27852c9ef");
            addDataToTable(new ConnectionConduitTypeTable(), values);

            values = new List<string>();
            values.Add("560ffd84-444d-4611-a346-266074f62f6f");
            values.Add("8d442906-efa2-49a0-ad21-f6b27852c9ef");
            addDataToTable(new ConnectionConduitTypeTable(), values);
        }
        private static void addToNetworkConnectionConnectionTypeTable()
        {
            List<string> values = new List<string>();
            values.Add("4f93907a-9aab-4ed5-8e55-43aab2af5ef8");
            values.Add("f38867c8-3846-461f-a6fa-c941aeb723c7");
            addDataToTable(new NetworkConnectionConnectionTypeTable(), values);

            values = new List<string>();
            values.Add("6aca8c22-5115-4534-a5b1-698b7e42d6c2");
            values.Add("f38867c8-3846-461f-a6fa-c941aeb723c7");
            addDataToTable(new NetworkConnectionConnectionTypeTable(), values);

            values = new List<string>();
            values.Add("99aea45e-ebeb-4c1a-8407-1d1a3540ceeb");
            values.Add("f38867c8-3846-461f-a6fa-c941aeb723c7");
            addDataToTable(new NetworkConnectionConnectionTypeTable(), values);

            values = new List<string>();
            values.Add("e503fdd4-f299-4618-8d54-6751c3b2bc25");
            values.Add("f38867c8-3846-461f-a6fa-c941aeb723c7");
            addDataToTable(new NetworkConnectionConnectionTypeTable(), values);
        }
        private static void addToNetworkConnectionControllerTable()
        {
            List<string> values = new List<string>();
            values.Add("4f93907a-9aab-4ed5-8e55-43aab2af5ef8");
            values.Add("f22913a6-e348-4a77-821f-80447621c6e0");
            addDataToTable(new NetworkConnectionChildrenTable(), values);

            values = new List<string>();
            values.Add("6aca8c22-5115-4534-a5b1-698b7e42d6c2");
            values.Add("973e6100-31f7-40b0-bfe7-9d64630c1c56");
            addDataToTable(new NetworkConnectionChildrenTable(), values);

            values = new List<string>();
            values.Add("99aea45e-ebeb-4c1a-8407-1d1a3540ceeb");
            values.Add("bf17527a-18ba-4765-a01e-8ab8de5664a3");
            addDataToTable(new NetworkConnectionChildrenTable(), values);

            values = new List<string>();
            values.Add("99aea45e-ebeb-4c1a-8407-1d1a3540ceeb");
            values.Add("7b6825df-57da-458a-a859-a9459c15907b");
            addDataToTable(new NetworkConnectionChildrenTable(), values);

            values = new List<string>();
            values.Add("e503fdd4-f299-4618-8d54-6751c3b2bc25");
            values.Add("ec965fe3-b1f7-4125-a545-ec47cc1e671b");
            addDataToTable(new NetworkConnectionChildrenTable(), values);
        }
        private static void addToSubScopeConnectionChildrenTable()
        {
            List<string> values = new List<string>();
            values.Add("5723e279-ac5c-4ee0-ae01-494a0c524b5c");
            values.Add("fbe0a143-e7cd-4580-a1c4-26eff0cd55a6");
            addDataToTable(new SubScopeConnectionChildrenTable(), values);

            values = new List<string>();
            values.Add("560ffd84-444d-4611-a346-266074f62f6f");
            values.Add("94726d87-b468-46a8-9421-3ff9725d5239");
            addDataToTable(new SubScopeConnectionChildrenTable(), values);
        }
        private static void addToPanelPanelTypeTable()
        {
            List<string> values = new List<string>();
            values.Add("a8cdd31c-e690-4eaa-81ea-602c72904391");
            values.Add("04e3204c-b35f-4e1a-8a01-db07f7eb055e");
            addDataToTable(new PanelPanelTypeTable(), values);

            values = new List<string>();
            values.Add("e7695d68-d79f-44a2-92f5-b303436186af");
            values.Add("04e3204c-b35f-4e1a-8a01-db07f7eb055e");
            addDataToTable(new PanelPanelTypeTable(), values);

            values = new List<string>();
            values.Add("10b07f6c-4374-49fc-ba6f-84db65b61ffa");
            values.Add("04e3204c-b35f-4e1a-8a01-db07f7eb055e");
            addDataToTable(new PanelPanelTypeTable(), values);
        }
        private static void addToPanelControllerTable()
        {
            List<string> values = new List<string>();
            values.Add("a8cdd31c-e690-4eaa-81ea-602c72904391");
            values.Add("98e6bc3e-31dc-4394-8b54-9ca53c193f46");
            addDataToTable(new PanelControllerTable(), values);

            values = new List<string>();
            values.Add("e7695d68-d79f-44a2-92f5-b303436186af");
            values.Add("1bb86714-2512-4fdd-a80f-46969753d8a0");
            addDataToTable(new PanelControllerTable(), values);

            values = new List<string>();
            values.Add("10b07f6c-4374-49fc-ba6f-84db65b61ffa");
            values.Add("f22913a6-e348-4a77-821f-80447621c6e0");
            addDataToTable(new PanelControllerTable(), values);
        }
        private static void addToSystemControllerTable()
        {
            List<string> values = new List<string>();
            values.Add("ebdbcc85-10f4-46b3-99e7-d896679f874a");
            values.Add("1bb86714-2512-4fdd-a80f-46969753d8a0");
            addDataToTable(new SystemControllerTable(), values);

            values = new List<string>();
            values.Add("ba2e71d4-a2b9-471a-9229-9fbad7432bf7");
            values.Add("f22913a6-e348-4a77-821f-80447621c6e0");
            addDataToTable(new SystemControllerTable(), values);

            values = new List<string>();
            values.Add("ebdbcc85-10f4-46b3-99e7-d896679f874a");
            values.Add("95032348-c661-470f-9bea-47dd750a47a5");
            addDataToTable(new SystemControllerTable(), values);

            values = new List<string>();
            values.Add("ba2e71d4-a2b9-471a-9229-9fbad7432bf7");
            values.Add("ec965fe3-b1f7-4125-a545-ec47cc1e671b");
            addDataToTable(new SystemControllerTable(), values);
        }
        private static void addToSystemPanelTable()
        {
            List<string> values = new List<string>();
            values.Add("ebdbcc85-10f4-46b3-99e7-d896679f874a");
            values.Add("e7695d68-d79f-44a2-92f5-b303436186af");
            addDataToTable(new SystemPanelTable(), values);

            values = new List<string>();
            values.Add("ba2e71d4-a2b9-471a-9229-9fbad7432bf7");
            values.Add("10b07f6c-4374-49fc-ba6f-84db65b61ffa");
            addDataToTable(new SystemPanelTable(), values);
        }
        private static void addToSystemScopeBranchTable()
        {
            List<string> values = new List<string>();
            values.Add("ebdbcc85-10f4-46b3-99e7-d896679f874a");
            values.Add("814710f1-f2dd-4ae6-9bc4-9279288e4994");
            addDataToTable(new SystemScopeBranchTable(), values);
        }
        private static void addToSystemHierarchyTable()
        {
            List<string> values = new List<string>();
            values.Add("ebdbcc85-10f4-46b3-99e7-d896679f874a");
            values.Add("ba2e71d4-a2b9-471a-9229-9fbad7432bf7");
            addDataToTable(new SystemHierarchyTable(), values);
        }
        private static void addToSystemMiscTable()
        {
            List<string> values = new List<string>();
            values.Add("ebdbcc85-10f4-46b3-99e7-d896679f874a");
            values.Add("e3ecee54-1f90-415a-b493-90a78f618476");
            addDataToTable(new SystemMiscTable(), values);
        }
        private static void addToCharacteristicScopeInstanceScopeTable()
        {
            List<string> values = new List<string>();
            values.Add("8a9bcc02-6ae2-4ac9-bbe1-e33d9a590b0e");
            values.Add("cdd9d7f7-ff3e-44ff-990f-c1b721e0ff8d");
            addDataToTable(new TypicalInstanceTable(), values);

            values = new List<string>();
            values.Add("fbe0a143-e7cd-4580-a1c4-26eff0cd55a6");
            values.Add("94726d87-b468-46a8-9421-3ff9725d5239");
            addDataToTable(new TypicalInstanceTable(), values);

            values = new List<string>();
            values.Add("03a16819-9205-4e65-a16b-96616309f171");
            values.Add("e60437bc-09a1-47eb-9fd5-78711d942a12");
            addDataToTable(new TypicalInstanceTable(), values);

            values = new List<string>();
            values.Add("1bb86714-2512-4fdd-a80f-46969753d8a0");
            values.Add("f22913a6-e348-4a77-821f-80447621c6e0");
            addDataToTable(new TypicalInstanceTable(), values);

            values = new List<string>();
            values.Add("e7695d68-d79f-44a2-92f5-b303436186af");
            values.Add("10b07f6c-4374-49fc-ba6f-84db65b61ffa");
            addDataToTable(new TypicalInstanceTable(), values);

            values = new List<string>();
            values.Add("95032348-c661-470f-9bea-47dd750a47a5");
            values.Add("ec965fe3-b1f7-4125-a545-ec47cc1e671b");
            addDataToTable(new TypicalInstanceTable(), values);
        }
        private static void addToControllerTypeIOTable()
        {
            var values = new List<string>();
            values.Add("7201ca48-f885-4a87-afa7-61b3e6942697");
            values.Add("1f6049cc-4dd6-4b50-a9d5-045b629ae6fb");
            addDataToTable(new ControllerTypeIOTable(), values);
        }
        private static void addToControllerControllerTypeTable()
        {
            List<string> values = new List<string>();
            values.Add("98e6bc3e-31dc-4394-8b54-9ca53c193f46");
            values.Add("7201ca48-f885-4a87-afa7-61b3e6942697");
            addDataToTable(new ControllerControllerTypeTable(), values);

            values = new List<string>();
            values.Add("1bb86714-2512-4fdd-a80f-46969753d8a0");
            values.Add("7201ca48-f885-4a87-afa7-61b3e6942697");
            addDataToTable(new ControllerControllerTypeTable(), values);

            values = new List<string>();
            values.Add("f22913a6-e348-4a77-821f-80447621c6e0");
            values.Add("7201ca48-f885-4a87-afa7-61b3e6942697");
            addDataToTable(new ControllerControllerTypeTable(), values);

            values = new List<string>();
            values.Add("973e6100-31f7-40b0-bfe7-9d64630c1c56");
            values.Add("7201ca48-f885-4a87-afa7-61b3e6942697");
            addDataToTable(new ControllerControllerTypeTable(), values);

            values = new List<string>();
            values.Add("ec965fe3-b1f7-4125-a545-ec47cc1e671b");
            values.Add("7201ca48-f885-4a87-afa7-61b3e6942697");
            addDataToTable(new ControllerControllerTypeTable(), values);

            values = new List<string>();
            values.Add("bf17527a-18ba-4765-a01e-8ab8de5664a3");
            values.Add("7201ca48-f885-4a87-afa7-61b3e6942697");
            addDataToTable(new ControllerControllerTypeTable(), values);

            values = new List<string>();
            values.Add("7b6825df-57da-458a-a859-a9459c15907b");
            values.Add("7201ca48-f885-4a87-afa7-61b3e6942697");
            addDataToTable(new ControllerControllerTypeTable(), values);

            values = new List<string>();
            values.Add("95032348-c661-470f-9bea-47dd750a47a5");
            values.Add("7201ca48-f885-4a87-afa7-61b3e6942697");
            addDataToTable(new ControllerControllerTypeTable(), values);
        }
        #endregion
    }
}
