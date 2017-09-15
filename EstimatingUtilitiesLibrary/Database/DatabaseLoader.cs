using EstimatingLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EstimatingUtilitiesLibrary;
using System.Windows;
using System.Collections.ObjectModel;
using System.Data;
using DebugLibrary;
using EstimatingLibrary.Utilities;
using System.Globalization;
using EstimatingLibrary.Interfaces;

namespace EstimatingUtilitiesLibrary.Database
{
    internal class DatabaseLoader
    {
        //FMT is used by DateTime to convert back and forth between the DateTime type and string
        private const string DB_FMT = "O";

        static private SQLiteDatabase SQLiteDB;

        static private bool justUpdated;
        static private TECManufacturer tempManufacturer;
        static private TECPanelType tempPanelType;
        static private TECControllerType tempControllerType;

        public static TECScopeManager Load(string path, bool versionUpdated = false)
        {
            justUpdated = versionUpdated;
            if (justUpdated)
            {
                setupTemps();
            }
            TECScopeManager workingScopeManager = null;
            SQLiteDB = new SQLiteDatabase(path);
            SQLiteDB.NonQueryCommand("BEGIN TRANSACTION");

            var tableNames = DatabaseHelper.TableNames(SQLiteDB);
            if (tableNames.Contains("BidInfo"))
            {
                workingScopeManager = loadBid();
            }
            else if (tableNames.Contains("TemplatesInfo"))
            {
                workingScopeManager = loadTemplates();
            }
            else
            {
                MessageBox.Show("File is not a compatible database.");
                return null;
            }

            SQLiteDB.NonQueryCommand("END TRANSACTION");
            SQLiteDB.Connection.Close();
            GC.Collect();
            GC.WaitForPendingFinalizers();

            return workingScopeManager;
        }

        #region Loading from DB Methods
        static private TECBid loadBid()
        {
            TECBid bid = GetBidInfo(SQLiteDB);
            //updateCatalogs(bid, templates);

            getScopeManagerProperties(bid);

            bid.Parameters = getBidParameters(bid);
            bid.ExtraLabor = getExtraLabor(bid);
            bid.ScopeTree = getBidScopeBranches();
            bid.Systems = getAllSystemsInBid();
            bid.Locations = getAllLocations();
            bid.Notes = getNotes();
            bid.Exclusions = getExclusions();
            bid.Controllers = getOrphanControllers();
            bid.MiscCosts = getMiscInBid();
            bid.Panels = getOrphanPanels();
            var placeholderDict = getCharacteristicInstancesList();

            ModelLinkingHelper.LinkBid(bid, placeholderDict);
            //Breaks Visual Scope in a page
            //populatePageVisualConnections(bid.Drawings, bid.Connections);

            return bid;
        }
        static private TECTemplates loadTemplates()
        {
            TECTemplates templates = new TECTemplates();
            templates = GetTemplatesInfo(SQLiteDB);
            getScopeManagerProperties(templates);
            templates.SystemTemplates = getSystems();
            templates.EquipmentTemplates = getOrphanEquipment();
            templates.SubScopeTemplates = getOrphanSubScope();
            templates.ControllerTemplates = getOrphanControllers();
            templates.MiscCostTemplates = getMisc();
            templates.PanelTemplates = getOrphanPanels();
            templates.Parameters = getTemplatesParameters();
            ModelLinkingHelper.LinkTemplates(templates);
            return templates;
        }

        static private void getScopeManagerProperties(TECScopeManager scopeManager)
        {
            scopeManager.Catalogs = getCatalogs();
            if (justUpdated)
            {
                scopeManager.Catalogs.Manufacturers.Add(tempManufacturer);
                scopeManager.Catalogs.PanelTypes.Add(tempPanelType);
                scopeManager.Catalogs.ControllerTypes.Add(tempControllerType);
            }
        }
        
        static private void setupTemps()
        {
            tempManufacturer = new TECManufacturer();
            tempManufacturer.Label = "TEMPORARY";

            tempControllerType = new TECControllerType(tempManufacturer);
            tempPanelType = new TECPanelType(tempManufacturer);
        }
        #region Catalogs
        static private TECCatalogs getCatalogs()
        {
            TECCatalogs catalogs = new TECCatalogs();
            catalogs.Devices = getAllDevices();
            catalogs.Manufacturers = getAllManufacturers();
            catalogs.ConnectionTypes = getConnectionTypes();
            catalogs.ConduitTypes = getConduitTypes();
            catalogs.AssociatedCosts = getAssociatedCosts();
            catalogs.PanelTypes = getPanelTypes();
            catalogs.IOModules = getIOModules();
            catalogs.Tags = getAllTags();
            catalogs.ControllerTypes = getControllerTypes();
            return catalogs;
        }
        static private ObservableCollection<TECDevice> getAllDevices()
        {
            ObservableCollection<TECDevice> devices = new ObservableCollection<TECDevice>();
            string command = string.Format("select {0} from {1}", DatabaseHelper.AllFieldsInTableString(new DeviceTable()), DeviceTable.TableName);
            DataTable devicesDT = SQLiteDB.GetDataFromCommand(command);

            foreach (DataRow row in devicesDT.Rows)
            { devices.Add(getDeviceFromRow(row)); }
            return devices;
        }
        static private ObservableCollection<TECManufacturer> getAllManufacturers()
        {
            ObservableCollection<TECManufacturer> manufacturers = new ObservableCollection<TECManufacturer>();
            string command = string.Format("select {0} from {1}", DatabaseHelper.AllFieldsInTableString(new ManufacturerTable()), ManufacturerTable.TableName);
            DataTable manufacturersDT = SQLiteDB.GetDataFromCommand(command);
            foreach (DataRow row in manufacturersDT.Rows)
            { manufacturers.Add(getManufacturerFromRow(row)); }
            return manufacturers;
        }
        static private ObservableCollection<TECElectricalMaterial> getConduitTypes()
        {
            ObservableCollection<TECElectricalMaterial> conduitTypes = new ObservableCollection<TECElectricalMaterial>();
            string command = string.Format("select {0} from {1}", DatabaseHelper.AllFieldsInTableString(new ConduitTypeTable()), ConduitTypeTable.TableName);
            DataTable conduitTypesDT = SQLiteDB.GetDataFromCommand(command);
            foreach (DataRow row in conduitTypesDT.Rows)
            { conduitTypes.Add(getConduitTypeFromRow(row)); }
            return conduitTypes;
        }
        static private ObservableCollection<TECPanelType> getPanelTypes()
        {
            ObservableCollection<TECPanelType> panelTypes = new ObservableCollection<TECPanelType>();
            string command = string.Format("select {0} from {1}", DatabaseHelper.AllFieldsInTableString(new PanelTypeTable()), PanelTypeTable.TableName);
            DataTable panelTypesDT = SQLiteDB.GetDataFromCommand(command);
            foreach (DataRow row in panelTypesDT.Rows)
            {
                panelTypes.Add(getPanelTypeFromRow(row));
            }

            return panelTypes;
        }
        static private ObservableCollection<TECElectricalMaterial> getConnectionTypes()
        {
            ObservableCollection<TECElectricalMaterial> connectionTypes = new ObservableCollection<TECElectricalMaterial>();
            string command = string.Format("select {0} from {1}", DatabaseHelper.AllFieldsInTableString(new ConnectionTypeTable()), ConnectionTypeTable.TableName);
            DataTable connectionTypesDT = SQLiteDB.GetDataFromCommand(command);
            foreach (DataRow row in connectionTypesDT.Rows)
            { connectionTypes.Add(getConnectionTypeFromRow(row)); }
            return connectionTypes;
        }
        static private ObservableCollection<TECCost> getRatedCostsInComponent(Guid componentID)
        {

            string command = "select " + ElectricalMaterialRatedCostTable.CostID.Name + ", " + ElectricalMaterialRatedCostTable.Quantity.Name + " from " + ElectricalMaterialRatedCostTable.TableName + " where ";
            command += ElectricalMaterialRatedCostTable.ComponentID.Name + " = '" + componentID;
            command += "'";
            DataTable DT = SQLiteDB.GetDataFromCommand(command);
            var costs = new ObservableCollection<TECCost>();
            foreach (DataRow row in DT.Rows)
            {
                TECCost costToAdd = getPlaceholderRatedCostFromRow(row);
                int quantity = row[ElectricalMaterialRatedCostTable.Quantity.Name].ToString().ToInt();
                for (int x = 0; x < quantity; x++) { costs.Add(costToAdd); }
            }
            return costs;
        }
        static private ObservableCollection<TECControllerType> getControllerTypes()
        {
            ObservableCollection<TECControllerType> controllerTypes = new ObservableCollection<TECControllerType>();
            string command = string.Format("select {0} from {1}", DatabaseHelper.AllFieldsInTableString(new ControllerTypeTable()), ControllerTypeTable.TableName);
            DataTable dt = SQLiteDB.GetDataFromCommand(command);
            foreach (DataRow row in dt.Rows)
            {
                controllerTypes.Add(getControllerTypeFromRow(row));
            }

            return controllerTypes;
        }
        static private ObservableCollection<TECValve> getValves()
        {
            ObservableCollection<TECValve> valves = new ObservableCollection<TECValve>();
            string command = string.Format("select {0} from {1}", DatabaseHelper.AllFieldsInTableString(new ValveTable()), ValveTable.TableName);
            DataTable dt = SQLiteDB.GetDataFromCommand(command);

            foreach (DataRow row in dt.Rows)
            { valves.Add(getValveFromRow(row)); }
            return valves;
        }
        #endregion
        #region System Components
        static private ObservableCollection<TECPanel> getPanelsInSystem(Guid guid)
        {
            ObservableCollection<TECPanel> panels = new ObservableCollection<TECPanel>();
            string command = "select " + DatabaseHelper.AllFieldsInTableString(new PanelTable()) + " from " + PanelTable.TableName + " where " + PanelTable.ID.Name + " in ";
            command += "(select " + SystemPanelTable.PanelID.Name + " from " + SystemPanelTable.TableName + " where ";
            command += SystemPanelTable.SystemID.Name + " = '" + guid;
            command += "')";
            DataTable dt = SQLiteDB.GetDataFromCommand(command);
            foreach (DataRow row in dt.Rows)
            { panels.Add(getPanelFromRow(row)); }

            return panels;
        }
        static private ObservableCollection<TECEquipment> getEquipmentInSystem(Guid systemID)
        {
            ObservableCollection<TECEquipment> equipment = new ObservableCollection<TECEquipment>();

            string command = "select " +
                DatabaseHelper.AllFieldsInTableString(new EquipmentTable());
            command += " from (" + EquipmentTable.TableName + " inner join ";
            command += SystemEquipmentTable.TableName + " on ";
            command += "(Equipment.ID = SystemEquipment.EquipmentID";
            command += " and " + SystemEquipmentTable.SystemID.Name + " = '";
            command += systemID;
            command += "')) order by " + SystemEquipmentTable.ScopeIndex.Name;
            DatabaseHelper.Explain(command, SQLiteDB);
            //string command = string.Format("select * from {0} where {1} in (select {2} from {3} indexed by {4} where {5} = '{6}')",
            //    EquipmentTable.TableName, EquipmentTable.EquipmentID.Name, SystemEquipmentTable.EquipmentID.Name, SystemEquipmentTable.TableName,
            //    systemEquipmentIndex, SystemEquipmentTable.SystemID.Name, systemID);

            DataTable equipmentDT = SQLiteDB.GetDataFromCommand(command);
            foreach (DataRow row in equipmentDT.Rows)
            { equipment.Add(getEquipmentFromRow(row)); }
            return equipment;
        }
        static private ObservableCollection<TECSystem> getChildrenSystems(Guid parentID)
        {
            ObservableCollection<TECSystem> children = new ObservableCollection<TECSystem>();

            string command = "select " + DatabaseHelper.AllFieldsInTableString(new SystemTable()) + " from " + SystemTable.TableName;
            command += " where " + SystemTable.ID.Name + " in ";
            command += "(select " + SystemHierarchyTable.ChildID.Name + " from " + SystemHierarchyTable.TableName;
            command += " where " + SystemHierarchyTable.ParentID.Name + " = '";
            command += parentID;
            command += "')";
            DatabaseHelper.Explain(command, SQLiteDB);
            DataTable childDT = SQLiteDB.GetDataFromCommand(command);
            foreach (DataRow row in childDT.Rows)
            {
                children.Add(getSystemFromRow(row));
            }

            return children;
        }
        static private ObservableCollection<TECController> getControllersInSystem(Guid guid)
        {
            ObservableCollection<TECController> controllers = new ObservableCollection<TECController>();
            string command = "select " + DatabaseHelper.AllFieldsInTableString(new ControllerTable()) + " from " + ControllerTable.TableName + " where " + ControllerTable.ID.Name + " in ";
            command += "(select " + SystemControllerTable.ControllerID.Name + " from " + SystemControllerTable.TableName + " where ";
            command += SystemControllerTable.SystemID.Name + " = '" + guid;
            command += "')";
            DatabaseHelper.Explain(command, SQLiteDB);
            DataTable controllerDT = SQLiteDB.GetDataFromCommand(command);
            foreach (DataRow row in controllerDT.Rows)
            {
                var controller = getControllerFromRow(row);
                controller.IsGlobal = false;
                controllers.Add(controller);
            }

            return controllers;
        }
        static private ObservableCollection<TECScopeBranch> getScopeBranchesInSystem(Guid guid)
        {
            ObservableCollection<TECScopeBranch> branches = new ObservableCollection<TECScopeBranch>();
            string command = "select " + DatabaseHelper.AllFieldsInTableString(new ScopeBranchTable()) + " from " + ScopeBranchTable.TableName + " where " + ScopeBranchTable.ID.Name + " in ";
            command += "(select " + SystemScopeBranchTable.BranchID.Name + " from " + SystemScopeBranchTable.TableName + " where ";
            command += SystemScopeBranchTable.SystemID.Name + " = '" + guid;
            command += "')";
            DataTable branchDT = SQLiteDB.GetDataFromCommand(command);
            foreach (DataRow row in branchDT.Rows)
            { branches.Add(getScopeBranchFromRow(row)); }

            return branches;
        }
        static private Dictionary<Guid, List<Guid>> getCharacteristicInstancesList()
        {
            Dictionary<Guid, List<Guid>> outDict = new Dictionary<Guid, List<Guid>>();
            DataTable dictDT = SQLiteDB.GetDataFromTable(TypicalInstanceTable.TableName);
            foreach (DataRow row in dictDT.Rows)
            {
                addRowToPlaceholderDict(row, outDict);
            }
            return outDict;
        }
        static private ObservableCollection<TECMisc> getMiscInSystem(Guid guid)
        {
            ObservableCollection<TECMisc> misc = new ObservableCollection<TECMisc>();
            string command = "select " + DatabaseHelper.AllFieldsInTableString(new MiscTable()) + " from " + MiscTable.TableName + " where " + MiscTable.ID.Name + " in ";
            command += "(select " + SystemMiscTable.MiscID.Name + " from " + SystemMiscTable.TableName + " where ";
            command += SystemMiscTable.SystemID.Name + " = '" + guid;
            command += "')";
            DatabaseHelper.Explain(command, SQLiteDB);
            DataTable miscDT = SQLiteDB.GetDataFromCommand(command);
            foreach (DataRow row in miscDT.Rows)
            {
                misc.Add(getMiscFromRow(row));
            }

            return misc;
        }

        #endregion
        #region Scope Children
        static private ObservableCollection<TECLabeled> getTagsInScope(Guid scopeID)
        {
            ObservableCollection<TECLabeled> tags = new ObservableCollection<TECLabeled>();
            //string command = "select * from "+TagTable.TableName+" where "+TagTable.TagID.Name+" in ";
            //command += "(select "+ScopeTagTable.TagID.Name+" from "+ScopeTagTable.TableName+" where ";
            //command += ScopeTagTable.ScopeID.Name + " = '"+scopeID;
            //command += "')";
            string command = "select " + ScopeTagTable.TagID.Name + " from " + ScopeTagTable.TableName + " where ";
            command += ScopeTagTable.ScopeID.Name + " = '" + scopeID;
            command += "'";
            DataTable tagsDT = SQLiteDB.GetDataFromCommand(command);
            foreach (DataRow row in tagsDT.Rows)
            { tags.Add(getPlaceholderTagFromRow(row)); }
            return tags;
        }
        static private ObservableCollection<TECCost> getAssociatedCostsInScope(Guid scopeID)
        {
            //string command = "select * from " + AssociatedCostTable.TableName + " where " + AssociatedCostTable.AssociatedCostID.Name + " in ";
            //command += "(select " + AssociatedCostTable.AssociatedCostID.Name + " from " + ScopeAssociatedCostTable.TableName + " where ";
            //command += ScopeAssociatedCostTable.ScopeID.Name + " = '" + scopeID;
            //command += "')";
            string command = "select " + ScopeAssociatedCostTable.AssociatedCostID.Name + ", " + ScopeAssociatedCostTable.Quantity.Name + " from " + ScopeAssociatedCostTable.TableName + " where ";
            command += ScopeAssociatedCostTable.ScopeID.Name + " = '" + scopeID;
            command += "'";
            DataTable DT = SQLiteDB.GetDataFromCommand(command);
            var associatedCosts = new ObservableCollection<TECCost>();
            foreach (DataRow row in DT.Rows)
            {
                TECCost costToAdd = getPlaceholderAssociatedCostFromRow(row);
                int quantity = row[ScopeAssociatedCostTable.Quantity.Name].ToString().ToInt();
                for (int x = 0; x < quantity; x++) { associatedCosts.Add(costToAdd); }
            }
            return associatedCosts;
        }
        static private TECLabeled getLocationInLocated(Guid ScopeID)
        {
            var tables = DatabaseHelper.TableNames(SQLiteDB);
            if (tables.Contains(LocationTable.TableName))
            {
                string command = "select " + LoactedLocationTable.LocationID.Name + " from " + LoactedLocationTable.TableName + " where ";
                command += LoactedLocationTable.ScopeID.Name + " = '" + ScopeID;
                command += "'";
                DataTable locationDT = SQLiteDB.GetDataFromCommand(command);
                if (locationDT.Rows.Count > 0)
                { return getPlaceholderLocationFromRow(locationDT.Rows[0]); }
                else
                { return null; }
            }
            else
            { return null; }
        }
        #endregion

        public static TECBid GetBidInfo(SQLiteDatabase db)
        {
            DataTable bidInfoDT = db.GetDataFromTable(BidInfoTable.TableName);
            if (bidInfoDT.Rows.Count < 1)
            {
                DebugHandler.LogError("Bid info not found in database. Bid info and labor will be missing.");
                return new TECBid();
            }

            DataRow bidInfoRow = bidInfoDT.Rows[0];

            TECBid outBid = new TECBid(new Guid(bidInfoRow[BidInfoTable.ID.Name].ToString()));
            assignValuePropertiesFromTable(outBid, new BidInfoTable(), bidInfoRow);

            string dueDateString = bidInfoRow[BidInfoTable.DueDate.Name].ToString();
            outBid.DueDate = DateTime.ParseExact(dueDateString, DB_FMT, CultureInfo.InvariantCulture);
            
            return outBid;
        }
        public static TECTemplates GetTemplatesInfo(SQLiteDatabase db)
        {
            DataTable templateInfoDT = db.GetDataFromTable(TemplatesInfoTable.TableName);

            if (templateInfoDT.Rows.Count < 1)
            {
                DebugHandler.LogError("Template info not found in database.");
                return new TECTemplates();
            }
            DataRow templateInfoRow = templateInfoDT.Rows[0];

            Guid infoGuid = new Guid(templateInfoRow[TemplatesInfoTable.ID.Name].ToString());

            return new TECTemplates(infoGuid);
        }
        static private TECExtraLabor getExtraLabor(TECBid bid)
        {
            DataTable DT = SQLiteDB.GetDataFromTable(ExtraLaborTable.TableName);
            if (DT.Rows.Count > 1)
            {
                DebugHandler.LogError("Multiple rows found in extra labor table. Using first found.");
            }
            else if (DT.Rows.Count < 1)
            {
                DebugHandler.LogError("Extra labor not found in database, using default values. Reload labor constants from loaded templates in the labor tab.");
                return new TECExtraLabor(bid.Guid);
            }
            return getExtraLaborFromRow(DT.Rows[0]);
        }
        static private ObservableCollection<TECScopeBranch> getBidScopeBranches()
        {
            ObservableCollection<TECScopeBranch> mainBranches = new ObservableCollection<TECScopeBranch>();

            string command = "select " + DatabaseHelper.AllFieldsInTableString(new ScopeBranchTable()) + " from " + ScopeBranchTable.TableName;
            command += " where " + ScopeBranchTable.ID.Name;
            command += " in (select " + BidScopeBranchTable.ScopeBranchID.Name;
            command += " from " + BidScopeBranchTable.TableName + " where " + BidScopeBranchTable.ScopeBranchID.Name + " not in ";
            command += "(select " + ScopeBranchHierarchyTable.ChildID.Name + " from " + ScopeBranchHierarchyTable.TableName + "))";

            DataTable mainBranchDT = SQLiteDB.GetDataFromCommand(command);

            foreach (DataRow row in mainBranchDT.Rows)
            {
                mainBranches.Add(getScopeBranchFromRow(row));
            }

            return mainBranches;
        }
        static private ObservableCollection<TECScopeBranch> getChildBranchesInBranch(Guid parentID)
        {
            ObservableCollection<TECScopeBranch> childBranches = new ObservableCollection<TECScopeBranch>();

            string command = "select " + DatabaseHelper.AllFieldsInTableString(new ScopeBranchTable()) + " from " + ScopeBranchTable.TableName;
            command += " where " + ScopeBranchTable.ID.Name + " in ";
            command += "(select " + ScopeBranchHierarchyTable.ChildID.Name + " from " + ScopeBranchHierarchyTable.TableName;
            command += " where " + ScopeBranchHierarchyTable.ParentID.Name + " = '";
            command += parentID;
            command += "')";

            DataTable childBranchDT = SQLiteDB.GetDataFromCommand(command);
            foreach (DataRow row in childBranchDT.Rows)
            {
                childBranches.Add(getScopeBranchFromRow(row));
            }

            return childBranches;
        }
        static private ObservableCollection<TECTypical> getAllSystemsInBid()
        {
            ObservableCollection<TECTypical> systems = new ObservableCollection<TECTypical>();

            string command = "select " + DatabaseHelper.AllFieldsInTableString(new SystemTable()) + " from ("
                + SystemTable.TableName
                + " inner join "
                + BidSystemTable.TableName
                + " on ("
                + SystemTable.TableName + "." + SystemTable.ID.Name
                + " = "
                + BidSystemTable.TableName + "." + BidSystemTable.SystemID.Name
                + ")) order by "
                + BidSystemTable.Index.Name;

            DataTable systemsDT = SQLiteDB.GetDataFromCommand(command);
            //if (systemsDT.Rows.Count < 1)
            //{
            //    command = "select " + DatabaseHelper.AllFieldsInTableString(new SystemTable()) + " from " + SystemTable.TableName;
            //    systemsDT = SQLiteDB.GetDataFromCommand(command);
            //}
            foreach (DataRow row in systemsDT.Rows)
            { systems.Add(getTypicalFromRow(row)); }
            return systems;
        }

        static private ObservableCollection<TECEquipment> getOrphanEquipment()
        {
            ObservableCollection<TECEquipment> equipment = new ObservableCollection<TECEquipment>();

            string command = "select " + DatabaseHelper.AllFieldsInTableString(new EquipmentTable()) + " from " + EquipmentTable.TableName;
            command += " where " + EquipmentTable.ID.Name + " not in ";
            command += "(select " + SystemEquipmentTable.EquipmentID.Name;
            command += " from " + SystemEquipmentTable.TableName + ")";

            DataTable equipmentDT = SQLiteDB.GetDataFromCommand(command);
            foreach (DataRow row in equipmentDT.Rows)
            { equipment.Add(getEquipmentFromRow(row)); }

            return equipment;
        }
        static private ObservableCollection<TECSubScope> getOrphanSubScope()
        {
            ObservableCollection<TECSubScope> subScope = new ObservableCollection<TECSubScope>();
            string command = "select " + DatabaseHelper.AllFieldsInTableString(new SubScopeTable()) + " from " + SubScopeTable.TableName;
            command += " where " + SubScopeTable.ID.Name + " not in ";
            command += "(select " + EquipmentSubScopeTable.SubScopeID.Name + " from " + EquipmentSubScopeTable.TableName + ")";
            DataTable subScopeDT = SQLiteDB.GetDataFromCommand(command);
            foreach (DataRow row in subScopeDT.Rows)
            { subScope.Add(getSubScopeFromRow(row)); }
            return subScope;
        }
        static private ObservableCollection<TECLabeled> getAllLocations()
        {
            ObservableCollection<TECLabeled> locations = new ObservableCollection<TECLabeled>();
            DataTable locationsDT = SQLiteDB.GetDataFromTable(LocationTable.TableName);
            foreach (DataRow row in locationsDT.Rows)
            { locations.Add(getLocationFromRow(row)); }
            return locations;
        }
        static private ObservableCollection<TECCost> getAssociatedCosts()
        {
            ObservableCollection<TECCost> associatedCosts = new ObservableCollection<TECCost>();
            DataTable associatedCostsDT = SQLiteDB.GetDataFromTable(AssociatedCostTable.TableName);
            foreach (DataRow row in associatedCostsDT.Rows)
            { associatedCosts.Add(getAssociatedCostFromRow(row)); }
            return associatedCosts;
        }
        static private ObservableCollection<TECSubScope> getSubScopeInEquipment(Guid equipmentID)
        {
            ObservableCollection<TECSubScope> subScope = new ObservableCollection<TECSubScope>();
            string command = "select " + DatabaseHelper.AllFieldsInTableString(new SubScopeTable()) + " from (SubScope inner join " + EquipmentSubScopeTable.TableName + " on ";
            command += "(SubScope.ID = EquipmentSubScope.SubScopeID and ";
            command += EquipmentSubScopeTable.EquipmentID.Name + "= '" + equipmentID;
            command += "')) order by " + EquipmentSubScopeTable.ScopeIndex.Name + "";
            DatabaseHelper.Explain(command, SQLiteDB);
            DataTable subScopeDT = SQLiteDB.GetDataFromCommand(command);
            foreach (DataRow row in subScopeDT.Rows)
            { subScope.Add(getSubScopeFromRow(row)); }
            return subScope;
        }
        static private ObservableCollection<ITECConnectable> getDevicesInSubScope(Guid subScopeID)
        {
            ObservableCollection<ITECConnectable> devices = new ObservableCollection<ITECConnectable>();

            string command = string.Format("select {0}, {4} from {1} where {2} = '{3}'",
                SubScopeDeviceTable.DeviceID.Name, SubScopeDeviceTable.TableName,
                SubScopeDeviceTable.SubScopeID.Name, subScopeID, SubScopeDeviceTable.Quantity.Name);
            DataTable devicesDT = SQLiteDB.GetDataFromCommand(command);
            foreach (DataRow row in devicesDT.Rows)
            {
                var deviceToAdd = getPlaceholderSubScopeDeviceFromRow(row);
                int quantity = row[SubScopeDeviceTable.Quantity.Name].ToString().ToInt();
                for (int x = 0; x < quantity; x++)
                { devices.Add(deviceToAdd); }
            }

            return devices;
        }
        static private ObservableCollection<TECPoint> getPointsInSubScope(Guid subScopeID)
        {
            ObservableCollection<TECPoint> points = new ObservableCollection<TECPoint>();

            string command = "select " + DatabaseHelper.AllFieldsInTableString(new PointTable()) + " from (" + PointTable.TableName + " inner join " + SubScopePointTable.TableName + " on ";
            command += "(Point.ID = SubScopePoint.PointID and ";
            command += SubScopePointTable.SubScopeID.Name + " = '" + subScopeID;
            command += "'))";
            DatabaseHelper.Explain(command, SQLiteDB);
            DataTable pointsDT = SQLiteDB.GetDataFromCommand(command);
            foreach (DataRow row in pointsDT.Rows)
            { points.Add(getPointFromRow(row)); }

            return points;
        }
        static private ObservableCollection<TECElectricalMaterial> getConnectionTypesInDevice(Guid deviceID)
        {
            ObservableCollection<TECElectricalMaterial> connectionTypes = new ObservableCollection<TECElectricalMaterial>();
            string command = string.Format("select {0}, {1} from {2} where {3} = '{4}'",
                DeviceConnectionTypeTable.TypeID.Name, DeviceConnectionTypeTable.Quantity.Name, DeviceConnectionTypeTable.TableName,
                DeviceConnectionTypeTable.DeviceID.Name, deviceID);

            DataTable connectionTypeTable = SQLiteDB.GetDataFromCommand(command);
            foreach (DataRow row in connectionTypeTable.Rows)
            {
                var connectionTypeToAdd = new TECElectricalMaterial(new Guid(row[DeviceConnectionTypeTable.TypeID.Name].ToString()));
                int quantity = row[DeviceConnectionTypeTable.Quantity.Name].ToString().ToInt(1);
                for (int x = 0; x < quantity; x++)
                { connectionTypes.Add(connectionTypeToAdd); }
            }
            return connectionTypes;
        }
        static private TECElectricalMaterial getConduitTypeInConnection(Guid connectionID)
        {
            string command = "select " + DatabaseHelper.AllFieldsInTableString(new ConduitTypeTable()) + " from " + ConduitTypeTable.TableName + " where " + ConduitTypeTable.ID.Name + " in ";
            command += "(select " + ConnectionConduitTypeTable.TypeID.Name + " from " + ConnectionConduitTypeTable.TableName + " where ";
            command += ConnectionConduitTypeTable.ConnectionID.Name + " = '" + connectionID;
            command += "')";

            DataTable conduitTypeTable = SQLiteDB.GetDataFromCommand(command);
            if (conduitTypeTable.Rows.Count > 0)
            { return (getConduitTypeFromRow(conduitTypeTable.Rows[0])); }
            else
            { return null; }
        }
        static private TECElectricalMaterial getConnectionTypeInNetworkConnection(Guid netConnectionID)
        {
            string command = "select " + DatabaseHelper.AllFieldsInTableString(new ConnectionTypeTable()) + " from " + ConnectionTypeTable.TableName + " where " + ConnectionTypeTable.ID.Name + " in ";
            command += "(select " + NetworkConnectionConnectionTypeTable.TypeID.Name + " from " + NetworkConnectionConnectionTypeTable.TableName + " where ";
            command += NetworkConnectionConnectionTypeTable.ConnectionID.Name + " = '" + netConnectionID;
            command += "')";

            DataTable connectionTypeDT = SQLiteDB.GetDataFromCommand(command);
            if (connectionTypeDT.Rows.Count > 0)
            {
                return (getConnectionTypeFromRow(connectionTypeDT.Rows[0]));
            }
            else
            {
                return null;
            }
        }
        static private ObservableCollection<TECLabeled> getNotes()
        {
            ObservableCollection<TECLabeled> notes = new ObservableCollection<TECLabeled>();
            DataTable notesDT = SQLiteDB.GetDataFromTable(NoteTable.TableName);
            foreach (DataRow row in notesDT.Rows)
            { notes.Add(getNoteFromRow(row)); }
            return notes;
        }
        static private ObservableCollection<TECLabeled> getExclusions()
        {
            ObservableCollection<TECLabeled> exclusions = new ObservableCollection<TECLabeled>();
            DataTable exclusionsDT = SQLiteDB.GetDataFromTable(ExclusionTable.TableName);
            foreach (DataRow row in exclusionsDT.Rows)
            { exclusions.Add(getExclusionFromRow(row)); }
            return exclusions;
        }
        static private ObservableCollection<TECLabeled> getAllTags()
        {
            ObservableCollection<TECLabeled> tags = new ObservableCollection<TECLabeled>();
            DataTable tagsDT = SQLiteDB.GetDataFromTable(TagTable.TableName);
            foreach (DataRow row in tagsDT.Rows)
            { tags.Add(getTagFromRow(row)); }
            return tags;
        }
        static private ObservableCollection<TECController> getOrphanControllers()
        {
            //Returns the controllers that are not in the ControlledScopeController table.
            ObservableCollection<TECController> controllers = new ObservableCollection<TECController>();

            string command = "select " + DatabaseHelper.AllFieldsInTableString(new ControllerTable()) + " from " + ControllerTable.TableName;
            command += " where " + ControllerTable.ID.Name + " not in ";
            command += "(select " + SystemControllerTable.ControllerID.Name;
            command += " from " + SystemControllerTable.TableName + ")";

            DataTable controllersDT = SQLiteDB.GetDataFromCommand(command);
            foreach (DataRow row in controllersDT.Rows)
            {
                var controller = getControllerFromRow(row);
                controller.IsGlobal = true;
                controllers.Add(controller);
            }

            return controllers;
        }
        static private ObservableCollection<TECIO> getIOInControllerType(Guid typeId)
        {
            ObservableCollection<TECIO> outIO = new ObservableCollection<TECIO>();
            string command = "select " + DatabaseHelper.AllFieldsInTableString(new IOTable()) + " from " + IOTable.TableName + " where " + IOTable.ID.Name + " in ";
            command += "(select " + ControllerTypeIOTable.IOID.Name + " from " + ControllerTypeIOTable.TableName + " where ";
            command += ControllerTypeIOTable.TypeID.Name + " = '" + typeId;
            command += "')";

            DataTable typeDT = SQLiteDB.GetDataFromCommand(command);
            foreach (DataRow row in typeDT.Rows)
            { outIO.Add(getIOFromRow(row)); }
            return outIO;
        }
        static private ObservableCollection<TECIOModule> getIOModules()
        {
            ObservableCollection<TECIOModule> ioModules = new ObservableCollection<TECIOModule>();
            DataTable ioModuleDT = SQLiteDB.GetDataFromTable(IOModuleTable.TableName);

            foreach (DataRow row in ioModuleDT.Rows)
            { ioModules.Add(getIOModuleFromRow(row)); }
            return ioModules;
        }

        static private TECSubScope getSubScopeInSubScopeConnection(Guid connectionID)
        {
            TECSubScope outScope = null;

            //string command = "select * from " + SubScopeTable.TableName + " where " + SubScopeTable.SubScopeID.Name + " in ";
            //command += "(select " + SubScopeConnectionChildrenTable.ChildID.Name + " from " + SubScopeConnectionChildrenTable.TableName + " where ";
            //command += SubScopeConnectionChildrenTable.ConnectionID.Name + " = '" + connectionID;
            //command += "')";
            string command = "select " + SubScopeConnectionChildrenTable.ChildID.Name + " from " + SubScopeConnectionChildrenTable.TableName + " where ";
            command += SubScopeConnectionChildrenTable.ConnectionID.Name + " = '" + connectionID;
            command += "'";

            DataTable scopeDT = SQLiteDB.GetDataFromCommand(command);
            if (scopeDT.Rows.Count > 0)
            {
                return getSubScopeConnectionChildPlaceholderFromRow(scopeDT.Rows[0]);
            }

            return outScope;
        }
        static private ObservableCollection<TECController> getControllersInNetworkConnection(Guid connectionID)
        {
            var outScope = new ObservableCollection<TECController>();

            string command = "select " + DatabaseHelper.AllFieldsInTableString(new ControllerTable()) + " from " + ControllerTable.TableName + " where " + ControllerTable.ID.Name + " in ";
            command += "(select " + NetworkConnectionControllerTable.ControllerID.Name + " from " + NetworkConnectionControllerTable.TableName + " where ";
            command += NetworkConnectionControllerTable.ConnectionID.Name + " = '" + connectionID;
            command += "')";

            DataTable scopeDT = SQLiteDB.GetDataFromCommand(command);
            foreach (DataRow row in scopeDT.Rows)
            { outScope.Add(getControllerPlaceholderFromRow(row)); }

            return outScope;
        }

        static private TECControllerType getTypeInController(Guid controllerID)
        {

            string command = "select " + ControllerControllerTypeTable.TypeID.Name + " from " + ControllerControllerTypeTable.TableName;
            command += " where " + ControllerControllerTypeTable.ControllerID.Name + " = '";
            command += controllerID;
            command += "'";

            DataTable manTable = SQLiteDB.GetDataFromCommand(command);
            if (manTable.Rows.Count > 0)
            { return getPlaceholderControllerTypeFromRow(manTable.Rows[0]); }
            else if (justUpdated)
            {
                return tempControllerType;
            }
            else
            { return null; }
        }
        static private TECParameters getBidParameters(TECBid bid)
        {
            string constsCommand = "select " + DatabaseHelper.AllFieldsInTableString(new ParametersTable()) + " from " + ParametersTable.TableName;

            DataTable DT = SQLiteDB.GetDataFromCommand(constsCommand);

            if (DT.Rows.Count > 1)
            {
                DebugHandler.LogError("Multiple rows found in bid paramters table. Using first found.");
            }
            else if (DT.Rows.Count < 1)
            {
                DebugHandler.LogError("Bid paramters not found in database, using default values. Reload labor constants from loaded templates in the labor tab.");
                return new TECParameters(bid.Guid);
            }
            return getBidParametersFromRow(DT.Rows[0]);
        }
        static private ObservableCollection<TECParameters> getTemplatesParameters()
        {
            ObservableCollection<TECParameters> outParameters = new ObservableCollection<TECParameters>();
            string constsCommand = "select " + DatabaseHelper.AllFieldsInTableString(new ParametersTable()) + " from " + ParametersTable.TableName;
            
            DataTable DT = SQLiteDB.GetDataFromCommand(constsCommand);
            foreach(DataRow row in DT.Rows)
            {
                outParameters.Add(getBidParametersFromRow(DT.Rows[0]));
            }
            return outParameters;
        }
        static private ObservableCollection<TECMisc> getMisc()
        {
            ObservableCollection<TECMisc> misc = new ObservableCollection<TECMisc>();

            DataTable miscDT = SQLiteDB.GetDataFromTable(MiscTable.TableName);
            foreach (DataRow row in miscDT.Rows)
            {
                misc.Add(getMiscFromRow(row));
            }

            return misc;
        }
        static private ObservableCollection<TECPanel> getOrphanPanels()
        {
            //Returns the panels that are not in the ControlledScopePanel table.
            ObservableCollection<TECPanel> panels = new ObservableCollection<TECPanel>();

            string command = "select " + DatabaseHelper.AllFieldsInTableString(new PanelTable()) + " from " + PanelTable.TableName;
            command += " where " + PanelTable.ID.Name + " not in ";
            command += "(select " + SystemPanelTable.PanelID.Name;
            command += " from " + SystemPanelTable.TableName + ")";

            DataTable panelsDT = SQLiteDB.GetDataFromCommand(command);
            foreach (DataRow row in panelsDT.Rows)
            {
                panels.Add(getPanelFromRow(row));
            }

            return panels;
        }
        static private ObservableCollection<TECSystem> getSystems()
        {
            ObservableCollection<TECSystem> systems = new ObservableCollection<TECSystem>();

            string command = "select " + DatabaseHelper.AllFieldsInTableString(new SystemTable()) + " from " + SystemTable.TableName;
            command += " where " + SystemTable.ID.Name;
            command += " in (select " + SystemTable.ID.Name;
            command += " from " + SystemTable.TableName + " where " + SystemTable.ID.Name + " not in ";
            command += "(select " + SystemHierarchyTable.ChildID.Name + " from " + SystemHierarchyTable.TableName + "))";

            DatabaseHelper.Explain(command, SQLiteDB);
            DataTable systemsDT = SQLiteDB.GetDataFromCommand(command);

            foreach (DataRow row in systemsDT.Rows)
            {
                systems.Add(getSystemFromRow(row));
            }
            return systems;
        }
        static private TECPanelType getPanelTypeInPanel(Guid guid)
        {
            string command = "select " + DatabaseHelper.AllFieldsInTableString(new PanelTypeTable()) + " from " + PanelTypeTable.TableName + " where " + PanelTypeTable.ID.Name + " in ";
            command += "(select " + PanelPanelTypeTable.PanelTypeID.Name + " from " + PanelPanelTypeTable.TableName;
            command += " where " + PanelPanelTypeTable.PanelID.Name + " = '";
            command += guid;
            command += "')";

            DataTable manTable = SQLiteDB.GetDataFromCommand(command);
            if (manTable.Rows.Count > 0)
            { return getPanelTypeFromRow(manTable.Rows[0]); }
            else if (justUpdated)
            {
                return tempPanelType;
            }
            else
            { return null; }
        }
        static private ObservableCollection<TECController> getControllersInPanel(Guid guid)
        {
            ObservableCollection<TECController> controllers = new ObservableCollection<TECController>();
            string command = String.Format("select {0} from {1} where {2} = '{3}'",
                PanelControllerTable.ControllerID.Name, PanelControllerTable.TableName,
                PanelControllerTable.PanelID.Name, guid);

            DataTable controllerDT = SQLiteDB.GetDataFromCommand(command);
            foreach (DataRow row in controllerDT.Rows)
            { controllers.Add(getPlaceholderPanelControllerFromRow(row)); }

            return controllers;
        }

        static private ObservableCollection<TECConnection> getConnectionsInController(TECController controller)
        {
            var tables = DatabaseHelper.TableNames(SQLiteDB);
            ObservableCollection<TECConnection> outScope = new ObservableCollection<TECConnection>();
            string command;
            DataTable scopeDT;

            if (tables.Contains(NetworkConnectionTable.TableName))
            {
                command = "select " + DatabaseHelper.AllFieldsInTableString(new NetworkConnectionTable()) + " from " + NetworkConnectionTable.TableName + " where " + NetworkConnectionTable.ID.Name + " in ";
                command += "(select " + ControllerConnectionTable.ConnectionID.Name + " from " + ControllerConnectionTable.TableName + " where ";
                command += ControllerConnectionTable.ControllerID.Name + " = '" + controller.Guid;
                command += "')";

                scopeDT = SQLiteDB.GetDataFromCommand(command);
                foreach (DataRow row in scopeDT.Rows)
                {
                    var networkConnection = getNetworkConnectionFromRow(row);
                    networkConnection.ParentController = controller;

                    outScope.Add(networkConnection);
                }
            }

            command = "select " + DatabaseHelper.AllFieldsInTableString(new SubScopeConnectionTable()) + " from " + SubScopeConnectionTable.TableName + " where " + SubScopeConnectionTable.ID.Name + " in ";
            command += "(select " + ControllerConnectionTable.ConnectionID.Name + " from " + ControllerConnectionTable.TableName + " where ";
            command += ControllerConnectionTable.ControllerID.Name + " = '" + controller.Guid;
            command += "')";

            scopeDT = SQLiteDB.GetDataFromCommand(command);
            foreach (DataRow row in scopeDT.Rows)
            {
                var subScopeConnection = getSubScopeConnectionFromRow(row);
                subScopeConnection.ParentController = controller;
                outScope.Add(subScopeConnection);
            }

            return outScope;
        }
        static private TECIOModule getModuleInIO(Guid ioID)
        {
            //string command = "select * from " + IOModuleTable.TableName + " where " + IOModuleTable.IOModuleID.Name + " in ";
            //command += "(select " + IOIOModuleTable.ModuleID.Name + " from " + IOIOModuleTable.TableName;
            //command += " where " + IOIOModuleTable.IOID.Name + " = '";
            //command += ioID;
            //command += "')";
            string command = "select " + IOIOModuleTable.ModuleID.Name + " from " + IOIOModuleTable.TableName;
            command += " where " + IOIOModuleTable.IOID.Name + " = '";
            command += ioID;
            command += "'";

            DataTable moduleTable = SQLiteDB.GetDataFromCommand(command);
            if (moduleTable.Rows.Count > 0)
            { return getPlaceholderIOModuleFromRow(moduleTable.Rows[0]); }
            else
            { return null; }
        }

        static private ObservableCollection<TECMisc> getMiscInBid()
        {
            ObservableCollection<TECMisc> misc = new ObservableCollection<TECMisc>();
            string command = "select " + DatabaseHelper.AllFieldsInTableString(new MiscTable()) + " from " + MiscTable.TableName + " where " + MiscTable.ID.Name + " in ";
            command += "(select " + BidMiscTable.MiscID.Name + " from " + BidMiscTable.TableName;
            command += ")";
            DataTable miscDT = SQLiteDB.GetDataFromCommand(command);
            foreach (DataRow row in miscDT.Rows)
            {
                misc.Add(getMiscFromRow(row));
            }

            return misc;
        }

        static private TECManufacturer getPlaceholderManufacturer(Guid hardwareGuid)
        {
            string command = "select " + HardwareManufacturerTable.ManufacturerID.Name + " from " + HardwareManufacturerTable.TableName;
            command += " where " + HardwareManufacturerTable.HardwareID.Name + " = '";
            command += hardwareGuid;
            command += "'";

            DataTable manTable = SQLiteDB.GetDataFromCommand(command);
            if (manTable.Rows.Count > 0)
            { return getPlaceholderManufacturerFromRow(manTable.Rows[0]); }
            else if (justUpdated)
            {
                return tempManufacturer;
            }
            else
            { return null; }
        }
        static private TECDevice getPlaceholderActuator(Guid valveID)
        {
            string command = String.Format("select {0} from {1} where {2} = '{3}'",
                ValveActuatorTable.ActuatorID.Name, ValveActuatorTable.TableName, ValveActuatorTable.ValveID, valveID);

            DataTable dt = SQLiteDB.GetDataFromCommand(command);
            if (dt.Rows.Count > 0)
            { return getPlaceholderActuatorFromRow(dt.Rows[0]); }
            else
            { return null; }
        }

        #endregion //Loading from DB Methods

        #region Row to Object Methods
        #region Base Scope
        private static TECTypical getTypicalFromRow(DataRow row)
        {
            Guid guid = new Guid(row[SystemTable.ID.Name].ToString());
            TECTypical system = new TECTypical(guid);

            assignValuePropertiesFromTable(system, new SystemTable(), row);
            system.Controllers = getControllersInSystem(guid);
            system.Equipment = getEquipmentInSystem(guid);
            system.Panels = getPanelsInSystem(guid);
            system.Instances = getChildrenSystems(guid);
            system.MiscCosts = getMiscInSystem(guid);
            system.ScopeBranches = getScopeBranchesInSystem(guid);
            getLocatedChildren(system);

            return system;
        }
        private static TECSystem getSystemFromRow(DataRow row)
        {
            Guid guid = new Guid(row[SystemTable.ID.Name].ToString());
            TECSystem system = new TECSystem(guid);

            assignValuePropertiesFromTable(system, new SystemTable(), row);
            system.Controllers = getControllersInSystem(guid);
            system.Equipment = getEquipmentInSystem(guid);
            system.Panels = getPanelsInSystem(guid);
            system.MiscCosts = getMiscInSystem(guid);
            system.ScopeBranches = getScopeBranchesInSystem(guid);
            getLocatedChildren(system);

            return system;
        }

        private static TECEquipment getEquipmentFromRow(DataRow row)
        {
            Guid equipmentID = new Guid(row[EquipmentTable.ID.Name].ToString());
            TECEquipment equipmentToAdd = new TECEquipment(equipmentID);
            assignValuePropertiesFromTable(equipmentToAdd, new EquipmentTable(), row);
            getLocatedChildren(equipmentToAdd);
            equipmentToAdd.SubScope = getSubScopeInEquipment(equipmentID);
            return equipmentToAdd;
        }
        private static TECSubScope getSubScopeFromRow(DataRow row)
        {
            Guid subScopeID = new Guid(row[SubScopeTable.ID.Name].ToString());
            TECSubScope subScopeToAdd = new TECSubScope(subScopeID);
            assignValuePropertiesFromTable(subScopeToAdd, new SubScopeTable(), row);
            subScopeToAdd.Devices = getDevicesInSubScope(subScopeID);
            subScopeToAdd.Points = getPointsInSubScope(subScopeID);
            getLocatedChildren(subScopeToAdd);
            return subScopeToAdd;
        }
        private static TECPoint getPointFromRow(DataRow row)
        {
            Guid pointID = new Guid(row[PointTable.ID.Name].ToString());
            TECPoint pointToAdd = new TECPoint(pointID);
            assignValuePropertiesFromTable(pointToAdd, new PointTable(), row);
            pointToAdd.Type = TECPoint.convertStringToType(row[PointTable.Type.Name].ToString());
            return pointToAdd;
        }
        #endregion
        #region Catalogs
        private static TECElectricalMaterial getConnectionTypeFromRow(DataRow row)
        {
            Guid guid = new Guid(row[ConnectionTypeTable.ID.Name].ToString());
            var outConnectionType = new TECElectricalMaterial(guid);
            assignValuePropertiesFromTable(outConnectionType, new ConnectionTypeTable(), row);
            getScopeChildren(outConnectionType);
            outConnectionType.RatedCosts = getRatedCostsInComponent(outConnectionType.Guid);
            return outConnectionType;
        }
        private static TECElectricalMaterial getConduitTypeFromRow(DataRow row)
        {
            Guid conduitGuid = new Guid(row[ConduitTypeTable.ID.Name].ToString());
            var conduitType = new TECElectricalMaterial(conduitGuid);
            assignValuePropertiesFromTable(conduitType, new ConduitTypeTable(), row);
            getScopeChildren(conduitType);
            conduitType.RatedCosts = getRatedCostsInComponent(conduitType.Guid);
            return conduitType;
        }

        private static TECCost getAssociatedCostFromRow(DataRow row)
        {
            Guid guid = new Guid(row[AssociatedCostTable.ID.Name].ToString());
            CostType type = UtilitiesMethods.StringToEnum<CostType>(row[AssociatedCostTable.Type.Name].ToString());
            var associatedCost = new TECCost(guid, type);
            assignValuePropertiesFromTable(associatedCost, new AssociatedCostTable(), row);
            return associatedCost;
        }
        private static TECDevice getDeviceFromRow(DataRow row)
        {
            Guid deviceID = new Guid(row[DeviceTable.ID.Name].ToString());
            ObservableCollection<TECElectricalMaterial> connectionType = getConnectionTypesInDevice(deviceID);
            TECManufacturer manufacturer = getPlaceholderManufacturer(deviceID);
            TECDevice deviceToAdd = new TECDevice(deviceID, connectionType, manufacturer);
            assignValuePropertiesFromTable(deviceToAdd, new DeviceTable(), row);
            getScopeChildren(deviceToAdd);
            return deviceToAdd;
        }
        private static TECManufacturer getManufacturerFromRow(DataRow row)
        {
            Guid manufacturerID = new Guid(row[ManufacturerTable.ID.Name].ToString());
            var manufacturer = new TECManufacturer(manufacturerID);
            assignValuePropertiesFromTable(manufacturer, new ManufacturerTable(), row);
            return manufacturer;
        }
        private static TECLabeled getLocationFromRow(DataRow row)
        {
            Guid locationID = new Guid(row[LocationTable.ID.Name].ToString());
            var location = new TECLabeled(locationID);
            assignValuePropertiesFromTable(location, new LocationTable(), row);
            return location;
        }
        private static TECLabeled getTagFromRow(DataRow row)
        {
            var tag = new TECLabeled(new Guid(row[TagTable.ID.Name].ToString()));
            assignValuePropertiesFromTable(tag, new TagTable(), row);
            return tag;
        }
        private static TECPanelType getPanelTypeFromRow(DataRow row)
        {
            Guid guid = new Guid(row[PanelTypeTable.ID.Name].ToString());
            TECManufacturer manufacturer = getPlaceholderManufacturer(guid);
            TECPanelType panelType = new TECPanelType(guid, manufacturer);
            assignValuePropertiesFromTable(panelType, new PanelTypeTable(), row);
            return panelType;
        }
        private static TECIOModule getIOModuleFromRow(DataRow row)
        {
            Guid guid = new Guid(row[IOModuleTable.ID.Name].ToString());
            TECManufacturer manufacturer = getPlaceholderManufacturer(guid);
            TECIOModule module = new TECIOModule(guid, manufacturer);
            assignValuePropertiesFromTable(module, new IOModuleTable(), row);
            return module;
        }
        private static TECControllerType getControllerTypeFromRow(DataRow row)
        {
            Guid guid = new Guid(row[ControllerTypeTable.ID.Name].ToString());
            TECManufacturer manufacturer = getPlaceholderManufacturer(guid);
            TECControllerType controllerType = new TECControllerType(guid, manufacturer);
            controllerType.IO = getIOInControllerType(guid);
            assignValuePropertiesFromTable(controllerType, new ControllerTypeTable(), row);
            return controllerType;
        }
        private static TECValve getValveFromRow(DataRow row)
        {
            Guid id = new Guid(row[DeviceTable.ID.Name].ToString());
            TECManufacturer manufacturer = getPlaceholderManufacturer(id);
            TECDevice actuator = getPlaceholderActuator(id);
            TECValve valve = new TECValve(id, manufacturer, actuator);
            assignValuePropertiesFromTable(valve, new ValveTable(), row);
            getScopeChildren(valve);
            return valve;
        }
        #endregion
        #region Scope Qualifiers
        private static TECScopeBranch getScopeBranchFromRow(DataRow row)
        {
            Guid scopeBranchID = new Guid(row[ScopeBranchTable.ID.Name].ToString());
            TECScopeBranch branch = new TECScopeBranch(scopeBranchID);
            assignValuePropertiesFromTable(branch, new ScopeBranchTable(), row);
            branch.Branches = getChildBranchesInBranch(scopeBranchID);
            return branch;
        }
        private static TECLabeled getNoteFromRow(DataRow row)
        {
            Guid noteID = new Guid(row[NoteTable.ID.Name].ToString());
            var note = new TECLabeled(noteID);
            assignValuePropertiesFromTable(note, new NoteTable(), row);
            return note;
        }
        private static TECLabeled getExclusionFromRow(DataRow row)
        {
            Guid exclusionId = new Guid(row[ExclusionTable.ID.Name].ToString());
            TECLabeled exclusion = new TECLabeled(exclusionId);
            assignValuePropertiesFromTable(exclusion, new ExclusionTable(), row);
            return exclusion;
        }
        #endregion
        
        #region Control Scope
        private static TECPanel getPanelFromRow(DataRow row)
        {
            Guid guid = new Guid(row[PanelTable.ID.Name].ToString());
            TECPanelType type = getPanelTypeInPanel(guid);
            TECPanel panel = new TECPanel(guid, type);

            assignValuePropertiesFromTable(panel, new PanelTable(), row);
            panel.Controllers = getControllersInPanel(guid);
            getScopeChildren(panel);

            return panel;
        }
        private static TECController getControllerFromRow(DataRow row)
        {
            Guid guid = new Guid(row[ControllerTable.ID.Name].ToString());
            TECController controller = new TECController(guid, getTypeInController(guid));

            assignValuePropertiesFromTable(controller, new ControllerTable(), row);
            getScopeChildren(controller);
            controller.ChildrenConnections = getConnectionsInController(controller);
            controller.NetworkType = UtilitiesMethods.StringToEnum<NetworkType>(row[ControllerTable.Type.Name].ToString(), 0);
            return controller;
        }
        private static TECIO getIOFromRow(DataRow row)
        {
            Guid guid = new Guid(row[IOTable.ID.Name].ToString());
            var io = new TECIO(guid);
            assignValuePropertiesFromTable(io, new IOTable(), row);
            io.Type = TECIO.convertStringToType(row[IOTable.IOType.Name].ToString());
            io.IOModule = getModuleInIO(guid);
            return io;
        }
        private static TECSubScopeConnection getSubScopeConnectionFromRow(DataRow row)
        {
            Guid guid = new Guid(row[SubScopeConnectionTable.ID.Name].ToString());
            TECSubScopeConnection connection = new TECSubScopeConnection(guid);
            assignValuePropertiesFromTable(connection, new SubScopeConnectionTable(), row);
            connection.ConduitType = getConduitTypeInConnection(connection.Guid);
            connection.SubScope = getSubScopeInSubScopeConnection(connection.Guid);
            return connection;
        }
        private static TECNetworkConnection getNetworkConnectionFromRow(DataRow row)
        {
            Guid guid = new Guid(row[NetworkConnectionTable.ID.Name].ToString());
            TECNetworkConnection connection = new TECNetworkConnection(guid);
            assignValuePropertiesFromTable(connection, new NetworkConnectionTable(), row);
            connection.IOType = UtilitiesMethods.StringToEnum<IOType>(row[NetworkConnectionTable.IOType.Name].ToString());
            connection.ConduitType = getConduitTypeInConnection(connection.Guid);
            connection.ChildrenControllers = getControllersInNetworkConnection(connection.Guid);
            connection.ConnectionType = getConnectionTypeInNetworkConnection(connection.Guid);
            return connection;
        }
        #endregion

        #region Misc
        private static TECMisc getMiscFromRow(DataRow row)
        {
            Guid guid = new Guid(row[MiscTable.ID.Name].ToString());
            CostType type = UtilitiesMethods.StringToEnum<CostType>(row[MiscTable.Type.Name].ToString());
            TECMisc cost = new TECMisc(guid, type);
            assignValuePropertiesFromTable(cost, new MiscTable(), row);
            getScopeChildren(cost);
            return cost;
        }
        private static TECParameters getBidParametersFromRow(DataRow row)
        {
            Guid guid = new Guid(row[ParametersTable.ID.Name].ToString());
            TECParameters paramters = new TECParameters(guid);
            assignValuePropertiesFromTable(paramters, new ParametersTable(), row);
            return paramters;
        }
        private static TECExtraLabor getExtraLaborFromRow(DataRow row)
        {
            Guid guid = new Guid(row[ExtraLaborTable.ID.Name].ToString());
            TECExtraLabor labor = new TECExtraLabor(guid);
            assignValuePropertiesFromTable(labor, new ExtraLaborTable(), row);
            return labor;
        }
        #endregion

        private static void getScopeChildren(TECScope scope)
        {
            scope.Tags = getTagsInScope(scope.Guid);
            scope.AssociatedCosts = getAssociatedCostsInScope(scope.Guid);
        }

        private static void getLocatedChildren(TECLocated located)
        {
            located.Location = getLocationInLocated(located.Guid);
            getScopeChildren(located);
        }

        #region Placeholder
        private static TECSubScope getSubScopeConnectionChildPlaceholderFromRow(DataRow row)
        {
            Guid subScopeID = new Guid(row[SubScopeConnectionChildrenTable.ChildID.Name].ToString());
            TECSubScope subScopeToAdd = new TECSubScope(subScopeID);
            return subScopeToAdd;
        }
        private static TECController getControllerPlaceholderFromRow(DataRow row)
        {
            Guid guid = new Guid(row[ControllerTable.ID.Name].ToString());
            TECController controller = new TECController(guid, new TECControllerType(new TECManufacturer()));

            controller.Name = row[ControllerTable.Name.Name].ToString();
            controller.Description = row[ControllerTable.Description.Name].ToString();
            return controller;
        }

        private static TECController getPlaceholderPanelControllerFromRow(DataRow row)
        {
            Guid guid = new Guid(row[PanelControllerTable.ControllerID.Name].ToString());
            TECController controller = new TECController(guid, new TECControllerType(new TECManufacturer()));
            return controller;
        }
        private static TECLabeled getPlaceholderTagFromRow(DataRow row)
        {
            Guid guid = new Guid(row[ScopeTagTable.TagID.Name].ToString());
            TECLabeled tag = new TECLabeled(guid);
            return tag;
        }
        private static TECCost getPlaceholderAssociatedCostFromRow(DataRow row)
        {
            Guid guid = new Guid(row[ScopeAssociatedCostTable.AssociatedCostID.Name].ToString());
            TECCost associatedCost = new TECCost(guid, CostType.TEC);
            return associatedCost;
        }
        private static TECCost getPlaceholderRatedCostFromRow(DataRow row)
        {
            Guid guid = new Guid(row[ElectricalMaterialRatedCostTable.CostID.Name].ToString());
            TECCost associatedCost = new TECCost(guid, CostType.TEC);
            return associatedCost;
        }
        private static TECLabeled getPlaceholderLocationFromRow(DataRow row)
        {
            Guid guid = new Guid(row[LoactedLocationTable.LocationID.Name].ToString());
            TECLabeled location = new TECLabeled(guid);
            return location;
        }
        private static TECDevice getPlaceholderSubScopeDeviceFromRow(DataRow row)
        {
            Guid guid = new Guid(row[SubScopeDeviceTable.DeviceID.Name].ToString());
            ObservableCollection<TECElectricalMaterial> connectionTypes = new ObservableCollection<TECElectricalMaterial>();
            TECManufacturer manufacturer = new TECManufacturer();
            TECDevice device = new TECDevice(guid, connectionTypes, manufacturer);
            device.Description = "placeholder";
            return device;
        }
        private static TECManufacturer getPlaceholderManufacturerFromRow(DataRow row)
        {
            Guid guid = new Guid(row[HardwareManufacturerTable.ManufacturerID.Name].ToString());
            TECManufacturer man = new TECManufacturer(guid);
            return man;
        }
        private static TECControllerType getPlaceholderControllerTypeFromRow(DataRow row)
        {
            Guid guid = new Guid(row[ControllerControllerTypeTable.TypeID.Name].ToString());
            TECControllerType type = new TECControllerType(guid, new TECManufacturer());
            return type;
        }
        private static TECElectricalMaterial getPlaceholderDeviceConnectionTypeFromRow(DataRow row)
        {
            Guid guid = new Guid(row[DeviceConnectionTypeTable.TypeID.Name].ToString());
            TECElectricalMaterial connectionType = new TECElectricalMaterial(guid);
            return connectionType;
        }
        private static TECIOModule getPlaceholderIOModuleFromRow(DataRow row)
        {
            Guid guid = new Guid(row[IOIOModuleTable.ModuleID.Name].ToString());
            TECIOModule module = new TECIOModule(guid, new TECManufacturer());
            module.Description = "placeholder";
            return module;
        }
        private static TECDevice getPlaceholderActuatorFromRow(DataRow row)
        {
            Guid guid = new Guid(row[ValveActuatorTable.ActuatorID.Name].ToString());
            ObservableCollection<TECElectricalMaterial> connectionTypes = new ObservableCollection<TECElectricalMaterial>();
            TECManufacturer manufacturer = new TECManufacturer();
            TECDevice device = new TECDevice(guid, connectionTypes, manufacturer);
            device.Description = "placeholder";
            return device;
        }

        private static void addRowToPlaceholderDict(DataRow row, Dictionary<Guid, List<Guid>> dict)
        {
            Guid key = new Guid(row[TypicalInstanceTable.TypicalID.Name].ToString());
            Guid value = new Guid(row[TypicalInstanceTable.InstanceID.Name].ToString());

            if (!dict.ContainsKey(key))
            {
                dict[key] = new List<Guid>();
            }
            dict[key].Add(value);
        }
        #endregion
        #endregion

        static private ObservableCollection<T> getListFromTable<T>(string tableName)
        {
            ObservableCollection<T> list = new ObservableCollection<T>();
            DataTable dt = SQLiteDB.GetDataFromTable(tableName);
            foreach (DataRow row in dt.Rows)
            { list.Add(getDataFromRow<T>(row)); }
            return list;
        }

        private static T getDataFromRow<T>(DataRow row)
        {
            throw new NotImplementedException();
        }

        private static void assignValuePropertiesFromTable(object item, TableBase table, DataRow row)
        {
            foreach(TableField field in table.Fields)
            {
                if (field.Property.DeclaringType.IsInstanceOfType(item) && field.Property.SetMethod != null)
                {
                    if(field.Property.PropertyType == typeof(string))
                    {
                        field.Property.SetValue(item, row[field.Name].ToString());
                    }
                    else if (field.Property.PropertyType == typeof(bool))
                    {
                        field.Property.SetValue(item, row[field.Name].ToString().ToInt(0).ToBool());
                    }
                    else if (field.Property.PropertyType == typeof(int))
                    {
                        field.Property.SetValue(item, row[field.Name].ToString().ToInt());
                    }
                    else if (field.Property.PropertyType == typeof(double))
                    {
                        field.Property.SetValue(item, row[field.Name].ToString().ToDouble(0));
                    }
                }
            }
        }
        
    }
}
