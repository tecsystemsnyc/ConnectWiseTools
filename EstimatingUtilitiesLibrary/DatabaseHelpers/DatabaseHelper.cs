﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EstimatingLibrary;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Drawing;
using System.Reflection;
using System.Collections;
using DebugLibrary;
using EstimatingLibrary.Interfaces;
using EstimatingLibrary.Utilities;

namespace EstimatingUtilitiesLibrary
{
    public static class DatabaseHelper
    {
        public enum DBType { Bid, Templates }

        //FMT is used by DateTime to convert back and forth between the DateTime type and string
        private const string DB_FMT = "O";
        //private const bool DEBUG = true;

        static private SQLiteDatabase SQLiteDB;

        static private bool isCompatbile;
        #region Public Functions
        static public TECScopeManager Load(string path)
        {
            throw new NotImplementedException();
            //var watch = System.Diagnostics.Stopwatch.StartNew();
            //TECScopeManager workingScopeManager = null;
            //SQLiteDB = new SQLiteDatabase(path);
            //SQLiteDB.nonQueryCommand("BEGIN TRANSACTION");

            //var tableNames = getAllTableNames();
            //if (tableNames.Contains("TECBidInfo"))
            //{
            //    workingScopeManager = loadBid();
            //}
            //else if (tableNames.Contains("TECTemplatesInfo"))
            //{
            //    workingScopeManager = loadTemplates();
            //}
            //else
            //{
            //    MessageBox.Show("File is not a compatible database.");
            //    return null;
            //}

            //SQLiteDB.nonQueryCommand("END TRANSACTION");
            //SQLiteDB.Connection.Close();
            //watch.Stop();
            //Console.WriteLine("Load: " + watch.ElapsedMilliseconds);
            //GC.Collect();
            //GC.WaitForPendingFinalizers();

            //return workingScopeManager;

        }

        static public void SaveNew(string path, TECScopeManager scopeManager)
        {
            throw new NotImplementedException();
            //var watch = System.Diagnostics.Stopwatch.StartNew();

            //SQLiteDB = new SQLiteDatabase(path);
            //indexesToUpdate = new Dictionary<TableBase, List<StackItem>>();

            //if (File.Exists(path))
            //{ SQLiteDB.overwriteFile(); }
            //SQLiteDB.nonQueryCommand("BEGIN TRANSACTION");
            //if (scopeManager is TECBid)
            //{
            //    createAllBidTables();
            //    saveCompleteBid(scopeManager as TECBid);
            //}
            //else if (scopeManager is TECTemplates)
            //{
            //    createAllTemplateTables();
            //    saveCompleteTemplate(scopeManager as TECTemplates);
            //}
            //saveIndexRelationships(indexesToUpdate);
            //SQLiteDB.nonQueryCommand("END TRANSACTION");
            //SQLiteDB.Connection.Close();

            //GC.Collect();
            //GC.WaitForPendingFinalizers();
            //watch.Stop();
            //Console.WriteLine("Save New: " + watch.ElapsedMilliseconds);
        }

        static public void CreateDB(string path, DBType type = DBType.Bid)
        {
            throw new NotImplementedException();
            //SQLiteDB = new SQLiteDatabase(path);
            //SQLiteDB.nonQueryCommand("BEGIN TRANSACTION");
            //if (type == DBType.Bid)
            //{
            //    createAllBidTables();
            //}
            //else if (type == DBType.Templates)
            //{
            //    createAllTemplateTables();
            //}
            //else
            //{
            //    throw new NotImplementedException();
            //}
            //SQLiteDB.nonQueryCommand("END TRANSACTION");
            //SQLiteDB.Connection.Close();
        }

        #endregion Public Functions

        //#region Loading from DB Methods
        //static private TECBid loadBid()
        //{
        //    checkAndUpdateDB(typeof(TECBid));
        //    if (isCompatbile)
        //    {
        //        TECBid bid = getBidInfo();
        //        //updateCatalogs(bid, templates);

        //        getScopeManagerProperties(bid);

        //        bid.Parameters = getBidParameters(bid);
        //        bid.ScopeTree = getBidScopeBranches();
        //        bid.Systems = getAllSystemsInBid();
        //        bid.Locations = getAllLocations();
        //        bid.Catalogs.Tags = getAllTags();
        //        bid.Notes = getNotes();
        //        bid.Exclusions = getExclusions();
        //        bid.Drawings = getDrawings();
        //        bid.Controllers = getOrphanControllers();
        //        bid.MiscCosts = getMiscInBid();
        //        bid.Panels = getOrphanPanels();
        //        var placeholderDict = getCharacteristicInstancesList();

        //        ModelLinkingHelper.LinkBid(bid, placeholderDict);
        //        getUserAdjustments(bid);
        //        //Breaks Visual Scope in a page
        //        //populatePageVisualConnections(bid.Drawings, bid.Connections);
        //        bid.Estimate.Refresh();

        //        return bid;
        //    }
        //    else
        //    {
        //        return new TECBid();
        //    }
        //}
        //static private TECTemplates loadTemplates()
        //{
        //    checkAndUpdateDB(typeof(TECTemplates));
        //    if (isCompatbile)
        //    {
        //        TECTemplates templates = new TECTemplates();
        //        //var watch = System.Diagnostics.Stopwatch.StartNew();
        //        templates = getTemplatesInfo();
        //        //watch.Stop();
        //        //Console.WriteLine("getTemplatesInfo: " + watch.ElapsedMilliseconds);
        //        //watch = System.Diagnostics.Stopwatch.StartNew();
        //        getScopeManagerProperties(templates);
        //        //watch.Stop();
        //        //Console.WriteLine("getScopeManagerProperties: " + watch.ElapsedMilliseconds);
        //        //watch = System.Diagnostics.Stopwatch.StartNew();
        //        templates.SystemTemplates = getSystems();
        //        //watch.Stop();
        //        //Console.WriteLine("getSystems: " + watch.ElapsedMilliseconds);
        //        //watch = System.Diagnostics.Stopwatch.StartNew();
        //        templates.EquipmentTemplates = getOrphanEquipment();
        //        //watch.Stop();
        //        //Console.WriteLine("getOrphanEquipment: " + watch.ElapsedMilliseconds);
        //        //watch = System.Diagnostics.Stopwatch.StartNew();
        //        templates.SubScopeTemplates = getOrphanSubScope();
        //        //watch.Stop();
        //        //Console.WriteLine("getOrphanSubScope: " + watch.ElapsedMilliseconds);
        //        //watch = System.Diagnostics.Stopwatch.StartNew();
        //        templates.ControllerTemplates = getOrphanControllers();
        //        //watch.Stop();
        //        //Console.WriteLine("getOrphanControllers: " + watch.ElapsedMilliseconds);
        //        //watch = System.Diagnostics.Stopwatch.StartNew();
        //        templates.MiscCostTemplates = getMisc();
        //        //watch.Stop();
        //        //Console.WriteLine("getMisc: " + watch.ElapsedMilliseconds);
        //        //watch = System.Diagnostics.Stopwatch.StartNew();
        //        templates.PanelTemplates = getOrphanPanels();
        //        //watch.Stop();
        //        //Console.WriteLine("getOrphanPanels: " + watch.ElapsedMilliseconds);
        //        ModelLinkingHelper.LinkTemplates(templates);
        //        return templates;
        //    }
        //    else
        //    {
        //        return new TECTemplates();
        //    }
        //}

        //static private void getScopeManagerProperties(TECScopeManager scopeManager)
        //{
        //    scopeManager.Catalogs = getCatalogs();
        //    scopeManager.Labor = getLaborConsts(scopeManager);
        //}
        //static private void getUserAdjustments(TECBid bid)
        //{
        //    DataTable adjDT = SQLiteDB.getDataFromTable(UserAdjustmentsTable.TableName);

        //    if (adjDT.Rows.Count < 1)
        //    {
        //        DebugHandler.LogError("UserAdjustments not found in database.");
        //        return;
        //    }

        //    DataRow adjRow = adjDT.Rows[0];

        //    bid.Labor.PMExtraHours = adjRow[UserAdjustmentsTable.PMExtraHours.Name].ToString().ToDouble();
        //    bid.Labor.ENGExtraHours = adjRow[UserAdjustmentsTable.ENGExtraHours.Name].ToString().ToDouble();
        //    bid.Labor.CommExtraHours = adjRow[UserAdjustmentsTable.CommExtraHours.Name].ToString().ToDouble();
        //    bid.Labor.SoftExtraHours = adjRow[UserAdjustmentsTable.SoftExtraHours.Name].ToString().ToDouble();
        //    bid.Labor.GraphExtraHours = adjRow[UserAdjustmentsTable.GraphExtraHours.Name].ToString().ToDouble();
        //}

        //static private TECLabor getLaborConsts(TECScopeManager scopeManager)
        //{
        //    DataTable laborDT = null;
        //    DataTable subConstsDT = null;
        //    if (scopeManager is TECBid)
        //    {
        //        string constsCommand = "select * from (" + LaborConstantsTable.TableName + " inner join ";
        //        constsCommand += BidLaborTable.TableName + " on ";
        //        constsCommand += "(TECLaborConst.LaborID = TECBidTECLabor.LaborID";
        //        constsCommand += " and " + BidLaborTable.BidID.Name + " = '";
        //        constsCommand += scopeManager.Guid;
        //        constsCommand += "'))";

        //        laborDT = SQLiteDB.getDataFromCommand(constsCommand);

        //        string subConstsCommand = "select * from (" + SubcontractorConstantsTable.TableName + " inner join ";
        //        subConstsCommand += BidLaborTable.TableName + " on ";
        //        subConstsCommand += "(TECSubcontractorConst.LaborID = TECBidTECLabor.LaborID";
        //        subConstsCommand += " and " + BidLaborTable.BidID.Name + " = '";
        //        subConstsCommand += scopeManager.Guid;
        //        subConstsCommand += "'))";

        //        subConstsDT = SQLiteDB.getDataFromCommand(subConstsCommand);
        //    }
        //    else if (scopeManager is TECTemplates)
        //    {
        //        laborDT = SQLiteDB.getDataFromTable(LaborConstantsTable.TableName);
        //        subConstsDT = SQLiteDB.getDataFromTable(SubcontractorConstantsTable.TableName);
        //    }
        //    else
        //    {
        //        throw new NotImplementedException();
        //    }

        //    if (laborDT.Rows.Count > 1)
        //    {
        //        DebugHandler.LogError("Multiple rows found in labor constants table. Using first found.");
        //    }
        //    else if (laborDT.Rows.Count < 1)
        //    {
        //        DebugHandler.LogError("Labor constants not found in database, using default values. Reload labor constants from loaded templates in the labor tab.");
        //        return new TECLabor();
        //    }

        //    DataRow laborRow = laborDT.Rows[0];
        //    Guid laborID = new Guid(laborRow[LaborConstantsTable.LaborID.Name].ToString());
        //    TECLabor labor = new TECLabor(laborID);

        //    labor.PMCoef = laborRow[LaborConstantsTable.PMCoef.Name].ToString().ToDouble(0);
        //    labor.PMRate = laborRow[LaborConstantsTable.PMRate.Name].ToString().ToDouble(0);

        //    labor.ENGCoef = laborRow[LaborConstantsTable.ENGCoef.Name].ToString().ToDouble(0);
        //    labor.ENGRate = laborRow[LaborConstantsTable.ENGRate.Name].ToString().ToDouble(0);

        //    labor.CommCoef = laborRow[LaborConstantsTable.CommCoef.Name].ToString().ToDouble(0);
        //    labor.CommRate = laborRow[LaborConstantsTable.CommRate.Name].ToString().ToDouble(0);

        //    labor.SoftCoef = laborRow[LaborConstantsTable.SoftCoef.Name].ToString().ToDouble(0);
        //    labor.SoftRate = laborRow[LaborConstantsTable.SoftRate.Name].ToString().ToDouble(0);

        //    labor.GraphCoef = laborRow[LaborConstantsTable.GraphCoef.Name].ToString().ToDouble(0);
        //    labor.GraphRate = laborRow[LaborConstantsTable.GraphRate.Name].ToString().ToDouble(0);



        //    if (subConstsDT.Rows.Count > 1)
        //    {
        //        DebugHandler.LogError("Multiple rows found in subcontractor constants table. Using first found.");
        //    }
        //    else if (subConstsDT.Rows.Count < 1)
        //    {
        //        DebugHandler.LogError("Subcontractor constants not found in database, using default values. Reload labor constants from loaded templates in the labor tab.");
        //        return labor;
        //    }

        //    DataRow subContractRow = subConstsDT.Rows[0];

        //    labor.ElectricalRate = subContractRow[SubcontractorConstantsTable.ElectricalRate.Name].ToString().ToDouble(0);
        //    labor.ElectricalNonUnionRate = subContractRow[SubcontractorConstantsTable.ElectricalNonUnionRate.Name].ToString().ToDouble(0);
        //    labor.ElectricalSuperRate = subContractRow[SubcontractorConstantsTable.ElectricalSuperRate.Name].ToString().ToDouble(0);
        //    labor.ElectricalSuperNonUnionRate = subContractRow[SubcontractorConstantsTable.ElectricalSuperNonUnionRate.Name].ToString().ToDouble(0);
        //    labor.ElectricalSuperRatio = subContractRow[SubcontractorConstantsTable.ElectricalSuperRatio.Name].ToString().ToDouble(0);

        //    labor.ElectricalIsOnOvertime = subContractRow[SubcontractorConstantsTable.ElectricalIsOnOvertime.Name].ToString().ToInt(0).ToBool();
        //    labor.ElectricalIsUnion = subContractRow[SubcontractorConstantsTable.ElectricalIsUnion.Name].ToString().ToInt(0).ToBool();

        //    return labor;
        //}

        //#region Catalogs
        //static private TECCatalogs getCatalogs()
        //{
        //    TECCatalogs catalogs = new TECCatalogs();
        //    catalogs.Devices = getAllDevices();
        //    catalogs.Manufacturers = getAllManufacturers();
        //    catalogs.ConnectionTypes = getConnectionTypes();
        //    catalogs.ConduitTypes = getConduitTypes();
        //    catalogs.AssociatedCosts = getAssociatedCosts();
        //    catalogs.PanelTypes = getPanelTypes();
        //    catalogs.IOModules = getIOModules();
        //    catalogs.Tags = getAllTags();
        //    return catalogs;
        //}
        //static private ObservableCollection<TECDevice> getAllDevices()
        //{
        //    ObservableCollection<TECDevice> devices = new ObservableCollection<TECDevice>();
        //    string command = string.Format("select {0} from {1}", allFieldsInTableString(new DeviceTable()), DeviceTable.TableName);
        //    DataTable devicesDT = SQLiteDB.getDataFromCommand(command);

        //    foreach (DataRow row in devicesDT.Rows)
        //    { devices.Add(getDeviceFromRow(row)); }
        //    return devices;
        //}
        //static private ObservableCollection<TECManufacturer> getAllManufacturers()
        //{
        //    ObservableCollection<TECManufacturer> manufacturers = new ObservableCollection<TECManufacturer>();
        //    string command = string.Format("select {0} from {1}", allFieldsInTableString(new ManufacturerTable()), ManufacturerTable.TableName);
        //    DataTable manufacturersDT = SQLiteDB.getDataFromCommand(command);
        //    foreach (DataRow row in manufacturersDT.Rows)
        //    { manufacturers.Add(getManufacturerFromRow(row)); }
        //    return manufacturers;
        //}
        //static private ObservableCollection<TECElectricalMaterial> getConduitTypes()
        //{
        //    ObservableCollection<TECElectricalMaterial> conduitTypes = new ObservableCollection<TECElectricalMaterial>();
        //    string command = string.Format("select {0} from {1}", allFieldsInTableString(new ConduitTypeTable()), ConduitTypeTable.TableName);
        //    DataTable conduitTypesDT = SQLiteDB.getDataFromCommand(command);
        //    foreach (DataRow row in conduitTypesDT.Rows)
        //    { conduitTypes.Add(getConduitTypeFromRow(row)); }
        //    return conduitTypes;
        //}
        //static private ObservableCollection<TECPanelType> getPanelTypes()
        //{
        //    ObservableCollection<TECPanelType> panelTypes = new ObservableCollection<TECPanelType>();
        //    string command = string.Format("select {0} from {1}", allFieldsInTableString(new PanelTypeTable()), PanelTypeTable.TableName);
        //    DataTable panelTypesDT = SQLiteDB.getDataFromCommand(command);
        //    foreach (DataRow row in panelTypesDT.Rows)
        //    {
        //        panelTypes.Add(getPanelTypeFromRow(row));
        //    }

        //    return panelTypes;
        //}
        //static private ObservableCollection<TECElectricalMaterial> getConnectionTypes()
        //{
        //    ObservableCollection<TECElectricalMaterial> connectionTypes = new ObservableCollection<TECElectricalMaterial>();
        //    string command = string.Format("select {0} from {1}", allFieldsInTableString(new ConnectionTypeTable()), ConnectionTypeTable.TableName);
        //    DataTable connectionTypesDT = SQLiteDB.getDataFromCommand(command);
        //    foreach (DataRow row in connectionTypesDT.Rows)
        //    { connectionTypes.Add(getConnectionTypeFromRow(row)); }
        //    return connectionTypes;
        //}
        //static private ObservableCollection<TECCost> getRatedCostsInComponent(Guid componentID)
        //{

        //    string command = "select " + ElectricalMaterialRatedCostTable.CostID.Name + ", " + ElectricalMaterialRatedCostTable.Quantity.Name + " from " + ElectricalMaterialRatedCostTable.TableName + " where ";
        //    command += ElectricalMaterialRatedCostTable.ComponentID.Name + " = '" + componentID;
        //    command += "'";
        //    DataTable DT = SQLiteDB.getDataFromCommand(command);
        //    var costs = new ObservableCollection<TECCost>();
        //    foreach (DataRow row in DT.Rows)
        //    {
        //        TECCost costToAdd = getPlaceholderRatedCostFromRow(row);
        //        int quantity = row[ElectricalMaterialRatedCostTable.Quantity.Name].ToString().ToInt();
        //        for (int x = 0; x < quantity; x++) { costs.Add(costToAdd); }
        //    }
        //    return costs;
        //}
        //#endregion
        //#region System Components
        //static private ObservableCollection<TECPanel> getPanelsInSystem(Guid guid)
        //{
        //    ObservableCollection<TECPanel> panels = new ObservableCollection<TECPanel>();
        //    string command = "select " + allFieldsInTableString(new PanelTable()) + " from " + PanelTable.TableName + " where " + PanelTable.PanelID.Name + " in ";
        //    command += "(select " + SystemPanelTable.PanelID.Name + " from " + SystemPanelTable.TableName + " where ";
        //    command += SystemPanelTable.SystemID.Name + " = '" + guid;
        //    command += "')";
        //    explain(command);
        //    DataTable dt = SQLiteDB.getDataFromCommand(command);
        //    foreach (DataRow row in dt.Rows)
        //    { panels.Add(getPanelFromRow(row)); }

        //    return panels;
        //}
        //static private ObservableCollection<TECEquipment> getEquipmentInSystem(Guid systemID)
        //{
        //    ObservableCollection<TECEquipment> equipment = new ObservableCollection<TECEquipment>();

        //    string command = "select " +
        //        allFieldsInTableString(new EquipmentTable());
        //    command += " from (" + EquipmentTable.TableName + " inner join ";
        //    command += SystemEquipmentTable.TableName + " on ";
        //    command += "(TECEquipment.EquipmentID = TECSystemTECEquipment.EquipmentID";
        //    command += " and " + SystemEquipmentTable.SystemID.Name + " = '";
        //    command += systemID;
        //    command += "')) order by " + SystemEquipmentTable.ScopeIndex.Name;
        //    explain(command);
        //    //string command = string.Format("select * from {0} where {1} in (select {2} from {3} indexed by {4} where {5} = '{6}')",
        //    //    EquipmentTable.TableName, EquipmentTable.EquipmentID.Name, SystemEquipmentTable.EquipmentID.Name, SystemEquipmentTable.TableName,
        //    //    systemEquipmentIndex, SystemEquipmentTable.SystemID.Name, systemID);

        //    DataTable equipmentDT = SQLiteDB.getDataFromCommand(command);
        //    foreach (DataRow row in equipmentDT.Rows)
        //    { equipment.Add(getEquipmentFromRow(row)); }
        //    return equipment;
        //}
        //static private ObservableCollection<TECSystem> getChildrenSystems(Guid parentID)
        //{
        //    ObservableCollection<TECSystem> children = new ObservableCollection<TECSystem>();

        //    string command = "select " + allFieldsInTableString(new SystemTable()) + " from " + SystemTable.TableName;
        //    command += " where " + SystemTable.SystemID.Name + " in ";
        //    command += "(select " + SystemHierarchyTable.ChildID.Name + " from " + SystemHierarchyTable.TableName;
        //    command += " where " + SystemHierarchyTable.ParentID.Name + " = '";
        //    command += parentID;
        //    command += "')";
        //    explain(command);
        //    DataTable childDT = SQLiteDB.getDataFromCommand(command);
        //    foreach (DataRow row in childDT.Rows)
        //    {
        //        children.Add(getSystemFromRow(row));
        //    }

        //    return children;
        //}
        //static private ObservableCollection<TECController> getControllersInSystem(Guid guid)
        //{
        //    ObservableCollection<TECController> controllers = new ObservableCollection<TECController>();
        //    string command = "select " + allFieldsInTableString(new ControllerTable()) + " from " + ControllerTable.TableName + " where " + ControllerTable.ControllerID.Name + " in ";
        //    command += "(select " + SystemControllerTable.ControllerID.Name + " from " + SystemControllerTable.TableName + " where ";
        //    command += SystemControllerTable.SystemID.Name + " = '" + guid;
        //    command += "')";
        //    explain(command);
        //    DataTable controllerDT = SQLiteDB.getDataFromCommand(command);
        //    foreach (DataRow row in controllerDT.Rows)
        //    {
        //        var controller = getControllerFromRow(row);
        //        controller.IsGlobal = false;
        //        controllers.Add(controller);
        //    }

        //    return controllers;
        //}
        //static private ObservableCollection<TECScopeBranch> getScopeBranchesInSystem(Guid guid)
        //{
        //    ObservableCollection<TECScopeBranch> branches = new ObservableCollection<TECScopeBranch>();
        //    string command = "select " + allFieldsInTableString(new ScopeBranchTable()) + " from " + ScopeBranchTable.TableName + " where " + ScopeBranchTable.ScopeBranchID.Name + " in ";
        //    command += "(select " + SystemScopeBranchTable.BranchID.Name + " from " + SystemScopeBranchTable.TableName + " where ";
        //    command += SystemScopeBranchTable.SystemID.Name + " = '" + guid;
        //    command += "')";
        //    explain(command);
        //    DataTable branchDT = SQLiteDB.getDataFromCommand(command);
        //    foreach (DataRow row in branchDT.Rows)
        //    { branches.Add(getScopeBranchFromRow(row)); }

        //    return branches;
        //}
        //static private Dictionary<Guid, List<Guid>> getCharacteristicInstancesList()
        //{
        //    Dictionary<Guid, List<Guid>> outDict = new Dictionary<Guid, List<Guid>>();
        //    DataTable dictDT = SQLiteDB.getDataFromTable(CharacteristicScopeInstanceScopeTable.TableName);
        //    foreach (DataRow row in dictDT.Rows)
        //    {
        //        addRowToPlaceholderDict(row, outDict);
        //    }
        //    return outDict;
        //}
        //static private ObservableCollection<TECMisc> getMiscInSystem(Guid guid)
        //{
        //    ObservableCollection<TECMisc> misc = new ObservableCollection<TECMisc>();
        //    string command = "select " + allFieldsInTableString(new MiscTable()) + " from " + MiscTable.TableName + " where " + MiscTable.MiscID.Name + " in ";
        //    command += "(select " + SystemMiscTable.MiscID.Name + " from " + SystemMiscTable.TableName + " where ";
        //    command += SystemMiscTable.SystemID.Name + " = '" + guid;
        //    command += "')";
        //    explain(command);
        //    DataTable miscDT = SQLiteDB.getDataFromCommand(command);
        //    foreach (DataRow row in miscDT.Rows)
        //    {
        //        misc.Add(getMiscFromRow(row));
        //    }

        //    return misc;
        //}

        //#endregion
        //#region Scope Children
        //static private ObservableCollection<TECLabeled> getTagsInScope(Guid scopeID)
        //{
        //    ObservableCollection<TECLabeled> tags = new ObservableCollection<TECLabeled>();
        //    //string command = "select * from "+TagTable.TableName+" where "+TagTable.TagID.Name+" in ";
        //    //command += "(select "+ScopeTagTable.TagID.Name+" from "+ScopeTagTable.TableName+" where ";
        //    //command += ScopeTagTable.ScopeID.Name + " = '"+scopeID;
        //    //command += "')";
        //    string command = "select " + ScopeTagTable.TagID.Name + " from " + ScopeTagTable.TableName + " where ";
        //    command += ScopeTagTable.ScopeID.Name + " = '" + scopeID;
        //    command += "'";
        //    DataTable tagsDT = SQLiteDB.getDataFromCommand(command);
        //    foreach (DataRow row in tagsDT.Rows)
        //    { tags.Add(getPlaceholderTagFromRow(row)); }
        //    return tags;
        //}
        //static private ObservableCollection<TECCost> getAssociatedCostsInScope(Guid scopeID)
        //{
        //    //string command = "select * from " + AssociatedCostTable.TableName + " where " + AssociatedCostTable.AssociatedCostID.Name + " in ";
        //    //command += "(select " + AssociatedCostTable.AssociatedCostID.Name + " from " + ScopeAssociatedCostTable.TableName + " where ";
        //    //command += ScopeAssociatedCostTable.ScopeID.Name + " = '" + scopeID;
        //    //command += "')";
        //    string command = "select " + ScopeAssociatedCostTable.AssociatedCostID.Name + ", " + ScopeAssociatedCostTable.Quantity.Name + " from " + ScopeAssociatedCostTable.TableName + " where ";
        //    command += ScopeAssociatedCostTable.ScopeID.Name + " = '" + scopeID;
        //    command += "'";
        //    DataTable DT = SQLiteDB.getDataFromCommand(command);
        //    var associatedCosts = new ObservableCollection<TECCost>();
        //    foreach (DataRow row in DT.Rows)
        //    {
        //        TECCost costToAdd = getPlaceholderAssociatedCostFromRow(row);
        //        int quantity = row[ScopeAssociatedCostTable.Quantity.Name].ToString().ToInt();
        //        for (int x = 0; x < quantity; x++) { associatedCosts.Add(costToAdd); }
        //    }
        //    return associatedCosts;
        //}
        //static private TECLabeled getLocationInScope(Guid ScopeID)
        //{
        //    var tables = getAllTableNames();
        //    if (tables.Contains(LocationTable.TableName))
        //    {
        //        //string command = "select * from " + LocationTable.TableName + " where " + LocationTable.LocationID.Name + " in ";
        //        //command += "(select " + LocationScopeTable.LocationID.Name + " from " + LocationScopeTable.TableName + " where ";
        //        //command += LocationScopeTable.ScopeID.Name + " = '" + ScopeID;
        //        //command += "')";
        //        string command = "select " + LocationScopeTable.LocationID.Name + " from " + LocationScopeTable.TableName + " where ";
        //        command += LocationScopeTable.ScopeID.Name + " = '" + ScopeID;
        //        command += "'";
        //        DataTable locationDT = SQLiteDB.getDataFromCommand(command);
        //        if (locationDT.Rows.Count > 0)
        //        { return getPlaceholderLocationFromRow(locationDT.Rows[0]); }
        //        else
        //        { return null; }
        //    }
        //    else
        //    { return null; }
        //}
        //#endregion

        //static private TECBid getBidInfo()
        //{
        //    DataTable bidInfoDT = SQLiteDB.getDataFromTable(BidInfoTable.TableName);
        //    if (bidInfoDT.Rows.Count < 1)
        //    {
        //        DebugHandler.LogError("Bid info not found in database. Bid info and labor will be missing.");
        //        return new TECBid();
        //    }

        //    DataRow bidInfoRow = bidInfoDT.Rows[0];

        //    TECBid outBid = new TECBid(new Guid(bidInfoRow[BidInfoTable.BidID.Name].ToString()));
        //    outBid.Name = bidInfoRow[BidInfoTable.BidName.Name].ToString();
        //    outBid.BidNumber = bidInfoRow[BidInfoTable.BidNumber.Name].ToString();

        //    string dueDateString = bidInfoRow[BidInfoTable.DueDate.Name].ToString();
        //    outBid.DueDate = DateTime.ParseExact(dueDateString, DB_FMT, CultureInfo.InvariantCulture);

        //    outBid.Salesperson = bidInfoRow[BidInfoTable.Salesperson.Name].ToString();
        //    outBid.Estimator = bidInfoRow[BidInfoTable.Estimator.Name].ToString();

        //    return outBid;
        //}
        //static private TECTemplates getTemplatesInfo()
        //{
        //    DataTable templateInfoDT = SQLiteDB.getDataFromTable(TemplatesInfoTable.TableName);

        //    if (templateInfoDT.Rows.Count < 1)
        //    {
        //        DebugHandler.LogError("Template info not found in database.");
        //        return new TECTemplates();
        //    }
        //    DataRow templateInfoRow = templateInfoDT.Rows[0];

        //    Guid infoGuid = new Guid(templateInfoRow[TemplatesInfoTable.TemplateID.Name].ToString());

        //    return new TECTemplates(infoGuid);
        //}
        //static private ObservableCollection<TECScopeBranch> getBidScopeBranches()
        //{
        //    ObservableCollection<TECScopeBranch> mainBranches = new ObservableCollection<TECScopeBranch>();

        //    string command = "select " + allFieldsInTableString(new ScopeBranchTable()) + " from " + ScopeBranchTable.TableName;
        //    command += " where " + ScopeBranchTable.ScopeBranchID.Name;
        //    command += " in (select " + ScopeBranchTable.ScopeBranchID.Name;
        //    command += " from " + BidScopeBranchTable.TableName + " where " + BidScopeBranchTable.ScopeBranchID.Name + " not in ";
        //    command += "(select " + ScopeBranchHierarchyTable.ChildID.Name + " from " + ScopeBranchHierarchyTable.TableName + "))";

        //    DataTable mainBranchDT = SQLiteDB.getDataFromCommand(command);

        //    foreach (DataRow row in mainBranchDT.Rows)
        //    {
        //        mainBranches.Add(getScopeBranchFromRow(row));
        //    }

        //    return mainBranches;
        //}
        //static private ObservableCollection<TECScopeBranch> getChildBranchesInBranch(Guid parentID)
        //{
        //    ObservableCollection<TECScopeBranch> childBranches = new ObservableCollection<TECScopeBranch>();

        //    string command = "select " + allFieldsInTableString(new ScopeBranchTable()) + " from " + ScopeBranchTable.TableName;
        //    command += " where " + ScopeBranchTable.ScopeBranchID.Name + " in ";
        //    command += "(select " + ScopeBranchHierarchyTable.ChildID.Name + " from " + ScopeBranchHierarchyTable.TableName;
        //    command += " where " + ScopeBranchHierarchyTable.ParentID.Name + " = '";
        //    command += parentID;
        //    command += "')";

        //    DataTable childBranchDT = SQLiteDB.getDataFromCommand(command);
        //    foreach (DataRow row in childBranchDT.Rows)
        //    {
        //        childBranches.Add(getScopeBranchFromRow(row));
        //    }

        //    return childBranches;
        //}
        //static private ObservableCollection<TECSystem> getAllSystemsInBid()
        //{
        //    ObservableCollection<TECSystem> systems = new ObservableCollection<TECSystem>();

        //    string command = "select " + allFieldsInTableString(new SystemTable()) + " from ("
        //        + SystemTable.TableName
        //        + " inner join "
        //        + BidSystemTable.TableName
        //        + " on ("
        //        + SystemTable.TableName + "." + SystemTable.SystemID.Name
        //        + " = "
        //        + BidSystemTable.TableName + "." + BidSystemTable.SystemID.Name
        //        + ")) order by "
        //        + BidSystemTable.Index.Name;

        //    DataTable systemsDT = SQLiteDB.getDataFromCommand(command);
        //    if (systemsDT.Rows.Count < 1)
        //    {
        //        command = "select " + allFieldsInTableString(new SystemTable()) + " from " + SystemTable.TableName;
        //        systemsDT = SQLiteDB.getDataFromCommand(command);
        //    }
        //    foreach (DataRow row in systemsDT.Rows)
        //    { systems.Add(getSystemFromRow(row)); }
        //    return systems;
        //}

        //static private ObservableCollection<TECEquipment> getOrphanEquipment()
        //{
        //    ObservableCollection<TECEquipment> equipment = new ObservableCollection<TECEquipment>();

        //    string command = "select " + allFieldsInTableString(new EquipmentTable()) + " from " + EquipmentTable.TableName;
        //    command += " where " + EquipmentTable.EquipmentID.Name + " not in ";
        //    command += "(select " + SystemEquipmentTable.EquipmentID.Name;
        //    command += " from " + SystemEquipmentTable.TableName + ")";

        //    DataTable equipmentDT = SQLiteDB.getDataFromCommand(command);
        //    foreach (DataRow row in equipmentDT.Rows)
        //    { equipment.Add(getEquipmentFromRow(row)); }

        //    return equipment;
        //}
        //static private ObservableCollection<TECSubScope> getOrphanSubScope()
        //{
        //    ObservableCollection<TECSubScope> subScope = new ObservableCollection<TECSubScope>();
        //    string command = "select " + allFieldsInTableString(new SubScopeTable()) + " from " + SubScopeTable.TableName;
        //    command += " where " + SubScopeTable.SubScopeID.Name + " not in ";
        //    command += "(select " + EquipmentSubScopeTable.SubScopeID.Name + " from " + EquipmentSubScopeTable.TableName + ")";
        //    DataTable subScopeDT = SQLiteDB.getDataFromCommand(command);
        //    foreach (DataRow row in subScopeDT.Rows)
        //    { subScope.Add(getSubScopeFromRow(row)); }
        //    return subScope;
        //}
        //static private ObservableCollection<TECLabeled> getAllLocations()
        //{
        //    ObservableCollection<TECLabeled> locations = new ObservableCollection<TECLabeled>();
        //    DataTable locationsDT = SQLiteDB.getDataFromTable(LocationTable.TableName);
        //    foreach (DataRow row in locationsDT.Rows)
        //    { locations.Add(getLocationFromRow(row)); }
        //    return locations;
        //}
        //static private ObservableCollection<TECCost> getAssociatedCosts()
        //{
        //    ObservableCollection<TECCost> associatedCosts = new ObservableCollection<TECCost>();
        //    DataTable associatedCostsDT = SQLiteDB.getDataFromTable(AssociatedCostTable.TableName);
        //    foreach (DataRow row in associatedCostsDT.Rows)
        //    { associatedCosts.Add(getAssociatedCostFromRow(row)); }
        //    return associatedCosts;
        //}
        //static private ObservableCollection<TECSubScope> getSubScopeInEquipment(Guid equipmentID)
        //{
        //    ObservableCollection<TECSubScope> subScope = new ObservableCollection<TECSubScope>();
        //    string command = "select " + allFieldsInTableString(new SubScopeTable()) + " from (TECSubScope inner join " + EquipmentSubScopeTable.TableName + " on ";
        //    command += "(TECSubScope.SubScopeID = TECEquipmentTECSubScope.SubScopeID and ";
        //    command += EquipmentSubScopeTable.EquipmentID.Name + "= '" + equipmentID;
        //    command += "')) order by " + EquipmentSubScopeTable.ScopeIndex.Name + "";
        //    explain(command);
        //    DataTable subScopeDT = SQLiteDB.getDataFromCommand(command);
        //    foreach (DataRow row in subScopeDT.Rows)
        //    { subScope.Add(getSubScopeFromRow(row)); }
        //    return subScope;
        //}
        //static private ObservableCollection<TECDevice> getDevicesInSubScope(Guid subScopeID)
        //{
        //    ObservableCollection<TECDevice> devices = new ObservableCollection<TECDevice>();

        //    string command = string.Format("select {0}, {4} from {1} where {2} = '{3}'",
        //        SubScopeDeviceTable.DeviceID.Name, SubScopeDeviceTable.TableName,
        //        SubScopeDeviceTable.SubScopeID.Name, subScopeID, SubScopeDeviceTable.Quantity.Name);
        //    explain(command);
        //    DataTable devicesDT = SQLiteDB.getDataFromCommand(command);
        //    foreach (DataRow row in devicesDT.Rows)
        //    {
        //        var deviceToAdd = getPlaceholderSubScopeDeviceFromRow(row);
        //        int quantity = row[SubScopeDeviceTable.Quantity.Name].ToString().ToInt();
        //        for (int x = 0; x < quantity; x++)
        //        { devices.Add(deviceToAdd); }
        //    }

        //    return devices;
        //}
        //static private ObservableCollection<TECPoint> getPointsInSubScope(Guid subScopeID)
        //{
        //    ObservableCollection<TECPoint> points = new ObservableCollection<TECPoint>();

        //    string command = "select " + allFieldsInTableString(new PointTable()) + " from (" + PointTable.TableName + " inner join " + SubScopePointTable.TableName + " on ";
        //    command += "(TECPoint.PointID = TECSubScopeTECPoint.PointID and ";
        //    command += SubScopePointTable.SubScopeID.Name + " = '" + subScopeID;
        //    command += "'))";
        //    explain(command);
        //    DataTable pointsDT = SQLiteDB.getDataFromCommand(command);
        //    foreach (DataRow row in pointsDT.Rows)
        //    { points.Add(getPointFromRow(row)); }

        //    return points;
        //}
        //static private TECManufacturer getManufacturerInDevice(Guid deviceID)
        //{
        //    string command = "select " + DeviceManufacturerTable.ManufacturerID.Name + " from " + DeviceManufacturerTable.TableName;
        //    command += " where " + DeviceManufacturerTable.DeviceID.Name + " = '";
        //    command += deviceID;
        //    command += "'";
        //    DataTable manTable = SQLiteDB.getDataFromCommand(command);
        //    if (manTable.Rows.Count > 0)
        //    { return getPlaceholderDeviceManufacturerFromRow(manTable.Rows[0]); }
        //    else
        //    { return null; }
        //}
        //static private TECManufacturer getManufacturerInIOModule(Guid guid)
        //{
        //    string command = "select " + allFieldsInTableString(new ManufacturerTable()) + " from " + ManufacturerTable.TableName + " where " + ManufacturerTable.ManufacturerID.Name + " in ";
        //    command += "(select " + IOModuleManufacturerTable.ManufacturerID.Name + " from " + IOModuleManufacturerTable.TableName;
        //    command += " where " + IOModuleManufacturerTable.IOModuleID.Name + " = '";
        //    command += guid;
        //    command += "')";

        //    DataTable manTable = SQLiteDB.getDataFromCommand(command);
        //    if (manTable.Rows.Count > 0)
        //    { return getManufacturerFromRow(manTable.Rows[0]); }
        //    else
        //    { return null; }
        //}
        //static private ObservableCollection<TECElectricalMaterial> getConnectionTypesInDevice(Guid deviceID)
        //{
        //    ObservableCollection<TECElectricalMaterial> connectionTypes = new ObservableCollection<TECElectricalMaterial>();
        //    string command = string.Format("select {0}, {1} from {2} where {3} = '{4}'",
        //        DeviceConnectionTypeTable.TypeID.Name, DeviceConnectionTypeTable.Quantity.Name, DeviceConnectionTypeTable.TableName,
        //        DeviceConnectionTypeTable.DeviceID.Name, deviceID);

        //    DataTable connectionTypeTable = SQLiteDB.getDataFromCommand(command);
        //    foreach (DataRow row in connectionTypeTable.Rows)
        //    {
        //        var connectionTypeToAdd = new TECElectricalMaterial(new Guid(row[DeviceConnectionTypeTable.TypeID.Name].ToString()));
        //        int quantity = row[DeviceConnectionTypeTable.Quantity.Name].ToString().ToInt(1);
        //        for (int x = 0; x < quantity; x++)
        //        { connectionTypes.Add(connectionTypeToAdd); }
        //    }
        //    return connectionTypes;
        //}
        //static private TECElectricalMaterial getConduitTypeInConnection(Guid connectionID)
        //{
        //    string command = "select " + allFieldsInTableString(new ConduitTypeTable()) + " from " + ConduitTypeTable.TableName + " where " + ConduitTypeTable.ConduitTypeID.Name + " in ";
        //    command += "(select " + ConnectionConduitTypeTable.TypeID.Name + " from " + ConnectionConduitTypeTable.TableName + " where ";
        //    command += ConnectionConduitTypeTable.ConnectionID.Name + " = '" + connectionID;
        //    command += "')";

        //    DataTable conduitTypeTable = SQLiteDB.getDataFromCommand(command);
        //    if (conduitTypeTable.Rows.Count > 0)
        //    { return (getConduitTypeFromRow(conduitTypeTable.Rows[0])); }
        //    else
        //    { return null; }
        //}
        //static private TECElectricalMaterial getConnectionTypeInNetworkConnection(Guid netConnectionID)
        //{
        //    string command = "select " + allFieldsInTableString(new ConnectionTypeTable()) + " from " + ConnectionTypeTable.TableName + " where " + ConnectionTypeTable.ConnectionTypeID.Name + " in ";
        //    command += "(select " + NetworkConnectionConnectionTypeTable.TypeID.Name + " from " + NetworkConnectionConnectionTypeTable.TableName + " where ";
        //    command += NetworkConnectionConnectionTypeTable.ConnectionID.Name + " = '" + netConnectionID;
        //    command += "')";

        //    DataTable connectionTypeDT = SQLiteDB.getDataFromCommand(command);
        //    if (connectionTypeDT.Rows.Count > 0)
        //    {
        //        return (getConnectionTypeFromRow(connectionTypeDT.Rows[0]));
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
        //static private ObservableCollection<TECLabeled> getNotes()
        //{
        //    ObservableCollection<TECLabeled> notes = new ObservableCollection<TECLabeled>();
        //    DataTable notesDT = SQLiteDB.getDataFromTable(NoteTable.TableName);
        //    foreach (DataRow row in notesDT.Rows)
        //    { notes.Add(getNoteFromRow(row)); }
        //    return notes;
        //}
        //static private ObservableCollection<TECLabeled> getExclusions()
        //{
        //    ObservableCollection<TECLabeled> exclusions = new ObservableCollection<TECLabeled>();
        //    DataTable exclusionsDT = SQLiteDB.getDataFromTable(ExclusionTable.TableName);
        //    foreach (DataRow row in exclusionsDT.Rows)
        //    { exclusions.Add(getExclusionFromRow(row)); }
        //    return exclusions;
        //}
        //static private ObservableCollection<TECLabeled> getAllTags()
        //{
        //    ObservableCollection<TECLabeled> tags = new ObservableCollection<TECLabeled>();
        //    DataTable tagsDT = SQLiteDB.getDataFromTable(TagTable.TableName);
        //    foreach (DataRow row in tagsDT.Rows)
        //    { tags.Add(getTagFromRow(row)); }
        //    return tags;
        //}
        //static private ObservableCollection<TECController> getOrphanControllers()
        //{
        //    //Returns the controllers that are not in the ControlledScopeController table.
        //    ObservableCollection<TECController> controllers = new ObservableCollection<TECController>();

        //    string command = "select " + allFieldsInTableString(new ControllerTable()) + " from " + ControllerTable.TableName;
        //    command += " where " + ControllerTable.ControllerID.Name + " not in ";
        //    command += "(select " + SystemControllerTable.ControllerID.Name;
        //    command += " from " + SystemControllerTable.TableName + ")";

        //    DataTable controllersDT = SQLiteDB.getDataFromCommand(command);
        //    foreach (DataRow row in controllersDT.Rows)
        //    {
        //        var controller = getControllerFromRow(row);
        //        controller.IsGlobal = true;
        //        controllers.Add(controller);
        //    }

        //    return controllers;
        //}
        //static private ObservableCollection<TECIO> getIOInController(Guid controllerID)
        //{
        //    ObservableCollection<TECIO> outIO = new ObservableCollection<TECIO>();
        //    string command = "select " + allFieldsInTableString(new IOTable()) + " from " + IOTable.TableName + " where " + IOTable.IOID.Name + " in ";
        //    command += "(select " + ControllerIOTable.IOID.Name + " from " + ControllerIOTable.TableName + " where ";
        //    command += ControllerIOTable.ControllerID.Name + " = '" + controllerID;
        //    command += "')";

        //    DataTable typeDT = SQLiteDB.getDataFromCommand(command);
        //    foreach (DataRow row in typeDT.Rows)
        //    { outIO.Add(getIOFromRow(row)); }
        //    return outIO;
        //}
        //static private TECScope getScopeGuidInVisualScope(Guid guid)
        //{
        //    string command = "select " + VisualScopeScopeTable.ScopeID.Name + " from " + VisualScopeScopeTable.TableName;
        //    command += " where " + VisualScopeScopeTable.VisualScopeID.Name + " = '";
        //    command += guid + "'";

        //    DataTable manTable = SQLiteDB.getDataFromCommand(command);
        //    if (manTable.Rows.Count > 0)
        //    {
        //        var row = manTable.Rows[0];
        //        return new TECSystem(new Guid(row[VisualScopeScopeTable.ScopeID.Name].ToString()));
        //    }
        //    else
        //    { return null; }
        //}
        //static private ObservableCollection<TECIOModule> getIOModules()
        //{
        //    ObservableCollection<TECIOModule> ioModules = new ObservableCollection<TECIOModule>();
        //    DataTable ioModuleDT = SQLiteDB.getDataFromTable(IOModuleTable.TableName);

        //    foreach (DataRow row in ioModuleDT.Rows)
        //    { ioModules.Add(getIOModuleFromRow(row)); }
        //    return ioModules;
        //}

        //static private TECSubScope getSubScopeInSubScopeConnection(Guid connectionID)
        //{
        //    TECSubScope outScope = null;

        //    //string command = "select * from " + SubScopeTable.TableName + " where " + SubScopeTable.SubScopeID.Name + " in ";
        //    //command += "(select " + SubScopeConnectionChildrenTable.ChildID.Name + " from " + SubScopeConnectionChildrenTable.TableName + " where ";
        //    //command += SubScopeConnectionChildrenTable.ConnectionID.Name + " = '" + connectionID;
        //    //command += "')";
        //    string command = "select " + SubScopeConnectionChildrenTable.ChildID.Name + " from " + SubScopeConnectionChildrenTable.TableName + " where ";
        //    command += SubScopeConnectionChildrenTable.ConnectionID.Name + " = '" + connectionID;
        //    command += "'";

        //    DataTable scopeDT = SQLiteDB.getDataFromCommand(command);
        //    if (scopeDT.Rows.Count > 0)
        //    {
        //        return getSubScopeConnectionChildPlaceholderFromRow(scopeDT.Rows[0]);
        //    }

        //    return outScope;
        //}
        //static private ObservableCollection<TECController> getControllersInNetworkConnection(Guid connectionID)
        //{
        //    var outScope = new ObservableCollection<TECController>();

        //    string command = "select " + allFieldsInTableString(new ControllerTable()) + " from " + ControllerTable.TableName + " where " + ControllerTable.ControllerID.Name + " in ";
        //    command += "(select " + NetworkConnectionControllerTable.ControllerID.Name + " from " + NetworkConnectionControllerTable.TableName + " where ";
        //    command += NetworkConnectionControllerTable.ConnectionID.Name + " = '" + connectionID;
        //    command += "')";

        //    DataTable scopeDT = SQLiteDB.getDataFromCommand(command);
        //    foreach (DataRow row in scopeDT.Rows)
        //    { outScope.Add(getControllerPlaceholderFromRow(row)); }

        //    return outScope;
        //}

        //static private TECControllerType getTypeInController(Guid controllerID)
        //{

        //    string command = "select " + ControllerManufacturerTable.ManufacturerID.Name + " from " + ControllerManufacturerTable.TableName;
        //    command += " where " + ControllerManufacturerTable.ControllerID.Name + " = '";
        //    command += controllerID;
        //    command += "'";

        //    DataTable manTable = SQLiteDB.getDataFromCommand(command);
        //    if (manTable.Rows.Count > 0)
        //    { return getPlaceholderControllerTypeFromRow(manTable.Rows[0]); }
        //    else
        //    { return null; }
        //}
        //static private TECBidParameters getBidParameters(TECBid bid)
        //{
        //    string constsCommand = "select " + allFieldsInTableString(new BidParametersTable()) + " from (" + BidParametersTable.TableName + " inner join ";
        //    constsCommand += BidBidParametersTable.TableName + " on ";
        //    constsCommand += "(TECBidTECParameters.ParametersID = TECBidTECParameters.ParametersID";
        //    constsCommand += " and " + BidBidParametersTable.BidID.Name + " = '";
        //    constsCommand += bid.Guid;
        //    constsCommand += "'))";

        //    DataTable DT = SQLiteDB.getDataFromCommand(constsCommand);

        //    if (DT.Rows.Count > 1)
        //    {
        //        DebugHandler.LogError("Multiple rows found in bid paramters table. Using first found.");
        //    }
        //    else if (DT.Rows.Count < 1)
        //    {
        //        DebugHandler.LogError("Bid paramters not found in database, using default values. Reload labor constants from loaded templates in the labor tab.");
        //        return new TECBidParameters();
        //    }
        //    return getBidParametersFromRow(DT.Rows[0]);
        //}
        //static private ObservableCollection<TECMisc> getMisc()
        //{
        //    ObservableCollection<TECMisc> misc = new ObservableCollection<TECMisc>();

        //    DataTable miscDT = SQLiteDB.getDataFromTable(MiscTable.TableName);
        //    foreach (DataRow row in miscDT.Rows)
        //    {
        //        misc.Add(getMiscFromRow(row));
        //    }

        //    return misc;
        //}
        //static private ObservableCollection<TECPanel> getOrphanPanels()
        //{
        //    //Returns the panels that are not in the ControlledScopePanel table.
        //    ObservableCollection<TECPanel> panels = new ObservableCollection<TECPanel>();

        //    string command = "select " + allFieldsInTableString(new PanelTable()) + " from " + PanelTable.TableName;
        //    command += " where " + PanelTable.PanelID.Name + " not in ";
        //    command += "(select " + SystemPanelTable.PanelID.Name;
        //    command += " from " + SystemPanelTable.TableName + ")";

        //    DataTable panelsDT = SQLiteDB.getDataFromCommand(command);
        //    foreach (DataRow row in panelsDT.Rows)
        //    {
        //        panels.Add(getPanelFromRow(row));
        //    }

        //    return panels;
        //}
        //static private ObservableCollection<TECSystem> getSystems()
        //{
        //    ObservableCollection<TECSystem> systems = new ObservableCollection<TECSystem>();

        //    string command = "select " + allFieldsInTableString(new SystemTable()) + " from " + SystemTable.TableName;
        //    command += " where " + SystemTable.SystemID.Name;
        //    command += " in (select " + SystemTable.SystemID.Name;
        //    command += " from " + SystemTable.TableName + " where " + SystemTable.SystemID.Name + " not in ";
        //    command += "(select " + SystemHierarchyTable.ChildID.Name + " from " + SystemHierarchyTable.TableName + "))";

        //    explain(command);
        //    DataTable systemsDT = SQLiteDB.getDataFromCommand(command);

        //    foreach (DataRow row in systemsDT.Rows)
        //    {
        //        systems.Add(getSystemFromRow(row));
        //    }
        //    return systems;
        //}
        //static private TECPanelType getPanelTypeInPanel(Guid guid)
        //{
        //    string command = "select " + allFieldsInTableString(new PanelTypeTable()) + " from " + PanelTypeTable.TableName + " where " + PanelTypeTable.PanelTypeID.Name + " in ";
        //    command += "(select " + PanelPanelTypeTable.PanelTypeID.Name + " from " + PanelPanelTypeTable.TableName;
        //    command += " where " + PanelPanelTypeTable.PanelID.Name + " = '";
        //    command += guid;
        //    command += "')";

        //    DataTable manTable = SQLiteDB.getDataFromCommand(command);
        //    if (manTable.Rows.Count > 0)
        //    { return getPanelTypeFromRow(manTable.Rows[0]); }
        //    else
        //    { return null; }
        //}
        //static private ObservableCollection<TECController> getControllersInPanel(Guid guid)
        //{
        //    ObservableCollection<TECController> controllers = new ObservableCollection<TECController>();
        //    string command = "select " + allFieldsInTableString(new ControllerTable()) + " from " + ControllerTable.TableName + " where " + ControllerTable.ControllerID.Name + " in ";
        //    command += "(select " + PanelControllerTable.ControllerID.Name + " from " + PanelControllerTable.TableName + " where ";
        //    command += PanelControllerTable.PanelID.Name + " = '" + guid;
        //    command += "')";

        //    DataTable controllerDT = SQLiteDB.getDataFromCommand(command);
        //    foreach (DataRow row in controllerDT.Rows)
        //    { controllers.Add(getControllerFromRow(row)); }

        //    return controllers;
        //}

        //static private ObservableCollection<TECConnection> getConnectionsInController(TECController controller)
        //{
        //    var tables = getAllTableNames();
        //    ObservableCollection<TECConnection> outScope = new ObservableCollection<TECConnection>();
        //    string command;
        //    DataTable scopeDT;

        //    if (tables.Contains(NetworkConnectionTable.TableName))
        //    {
        //        command = "select " + allFieldsInTableString(new NetworkConnectionTable()) + " from " + NetworkConnectionTable.TableName + " where " + NetworkConnectionTable.ConnectionID.Name + " in ";
        //        command += "(select " + ControllerConnectionTable.ConnectionID.Name + " from " + ControllerConnectionTable.TableName + " where ";
        //        command += ControllerConnectionTable.ControllerID.Name + " = '" + controller.Guid;
        //        command += "')";

        //        scopeDT = SQLiteDB.getDataFromCommand(command);
        //        foreach (DataRow row in scopeDT.Rows)
        //        {
        //            var networkConnection = getNetworkConnectionFromRow(row);
        //            networkConnection.ParentController = controller;

        //            outScope.Add(networkConnection);
        //        }
        //    }

        //    command = "select " + allFieldsInTableString(new SubScopeConnectionTable()) + " from " + SubScopeConnectionTable.TableName + " where " + SubScopeConnectionTable.ConnectionID.Name + " in ";
        //    command += "(select " + ControllerConnectionTable.ConnectionID.Name + " from " + ControllerConnectionTable.TableName + " where ";
        //    command += ControllerConnectionTable.ControllerID.Name + " = '" + controller.Guid;
        //    command += "')";

        //    scopeDT = SQLiteDB.getDataFromCommand(command);
        //    foreach (DataRow row in scopeDT.Rows)
        //    {
        //        var subScopeConnection = getSubScopeConnectionFromRow(row);
        //        subScopeConnection.ParentController = controller;
        //        outScope.Add(subScopeConnection);
        //    }

        //    return outScope;
        //}
        //static private TECIOModule getModuleInIO(Guid ioID)
        //{
        //    //string command = "select * from " + IOModuleTable.TableName + " where " + IOModuleTable.IOModuleID.Name + " in ";
        //    //command += "(select " + IOIOModuleTable.ModuleID.Name + " from " + IOIOModuleTable.TableName;
        //    //command += " where " + IOIOModuleTable.IOID.Name + " = '";
        //    //command += ioID;
        //    //command += "')";
        //    string command = "select " + IOIOModuleTable.ModuleID.Name + " from " + IOIOModuleTable.TableName;
        //    command += " where " + IOIOModuleTable.IOID.Name + " = '";
        //    command += ioID;
        //    command += "'";

        //    DataTable moduleTable = SQLiteDB.getDataFromCommand(command);
        //    if (moduleTable.Rows.Count > 0)
        //    { return getPlaceholderIOModuleFromRow(moduleTable.Rows[0]); }
        //    else
        //    { return null; }
        //}

        //static private ObservableCollection<TECMisc> getMiscInBid()
        //{
        //    ObservableCollection<TECMisc> misc = new ObservableCollection<TECMisc>();
        //    string command = "select " + allFieldsInTableString(new MiscTable()) + " from " + MiscTable.TableName + " where " + MiscTable.MiscID.Name + " in ";
        //    command += "(select " + BidMiscTable.MiscID.Name + " from " + BidMiscTable.TableName;
        //    command += ")";
        //    DataTable miscDT = SQLiteDB.getDataFromCommand(command);
        //    foreach (DataRow row in miscDT.Rows)
        //    {
        //        misc.Add(getMiscFromRow(row));
        //    }

        //    return misc;
        //}

        //#endregion //Loading from DB Methods

        //#region Database Version Update Methods
        //static private void checkAndUpdateDB(Type type)
        //{
        //    bool isUpToDate;
        //    isUpToDate = checkDatabaseVersion(type);
        //    if (!isCompatbile)
        //    {
        //        MessageBox.Show("This database is not compatible with this version of the program.");
        //        return;
        //    }
        //    else if (!isUpToDate)
        //    {
        //        updateDatabase(type);
        //        updateVersionNumber(type);
        //    }
        //}
        //static private bool checkDatabaseVersion(Type type)
        //{
        //    string currentVersion = Properties.Settings.Default.Version;
        //    string lowestCompatible = "1.6.0.11";
        //    DataTable infoDT = new DataTable();
        //    if (type == typeof(TECBid))
        //    { infoDT = SQLiteDB.getDataFromTable(BidInfoTable.TableName); }
        //    else if (type == typeof(TECTemplates))
        //    {
        //        try
        //        {
        //            infoDT = SQLiteDB.getDataFromTable(TemplatesInfoTable.TableName);
        //        }
        //        catch
        //        {
        //            killTemplatesInfo();
        //            return false;
        //        }
        //    }
        //    else
        //    { throw new ArgumentException("checkDatabaseVersion given invalid type"); }

        //    if (infoDT.Rows.Count < 1)
        //    {
        //        if (type == typeof(TECBid))
        //        {
        //            isCompatbile = false;
        //            throw new DataException("Could not load from TECBidInfo");
        //        }
        //        else if (type == typeof(TECTemplates))
        //        {
        //            isCompatbile = false;
        //            return false;
        //        }
        //        else
        //        { return false; }
        //    }
        //    else if ((infoDT.Rows.Count == 1) || (type == typeof(TECBid)))
        //    {
        //        DataRow infoRow = infoDT.Rows[0];
        //        if (infoDT.Columns.Contains(BidInfoTable.DBVersion.Name) || infoDT.Columns.Contains(TemplatesInfoTable.DBVersion.Name))
        //        {
        //            string version = infoRow[BidInfoTable.DBVersion.Name].ToString();
        //            if (UtilitiesMethods.IsLowerVersion(lowestCompatible, version))
        //            {
        //                isCompatbile = false;
        //                return false;
        //            }
        //            isCompatbile = true;
        //            return (!UtilitiesMethods.IsLowerVersion(currentVersion, version));
        //        }
        //        else
        //        { return false; }
        //    }
        //    else if ((infoDT.Rows.Count > 1) && (type == typeof(TECTemplates)))
        //    {
        //        killTemplatesInfo();
        //        return false;
        //    }
        //    else
        //    {
        //        throw new NotImplementedException();
        //    }
        //}
        //static private void updateDatabase(Type type)
        //{
        //    List<string> tableNames = getAllTableNames();
        //    List<object> databaseTableList = new List<object>();
        //    if (type == typeof(TECBid))
        //    { databaseTableList = AllBidTables.Tables; }
        //    else if (type == typeof(TECTemplates))
        //    { databaseTableList = AllTemplateTables.Tables; }
        //    else
        //    { throw new ArgumentException("updateDatabase() given invalid type"); }
        //    foreach (TableBase table in databaseTableList)
        //    {
        //        var tableInfo = new TableInfo(table);
        //        if (tableNames.Contains(tableInfo.Name))
        //        { updateTableFromType(table); }
        //        else
        //        { createTableFromDefinition(table); }
        //    }
        //}
        //static private void updateTableFromType(TableBase table)
        //{
        //    var tableInfo = new TableInfo(table);
        //    string tableName = tableInfo.Name;
        //    string tempName = "temp_" + tableName;
        //    List<TableField> primaryKeys = tableInfo.PrimaryFields;
        //    List<TableField> fields = tableInfo.Fields;

        //    List<string> currentFields = getAllTableFields(tableName);
        //    List<string> currentPrimaryKeys = getPrimaryKeys(tableName);
        //    List<string> commonFields = new List<string>();
        //    foreach (TableField field in fields)
        //    {
        //        if (currentFields.Contains(field.Name))
        //        { commonFields.Add(field.Name); }
        //    }
        //    List<string> currentFieldNames = new List<string>();
        //    List<string> newFieldNames = new List<string>();
        //    foreach (string field in commonFields)
        //    {
        //        currentFieldNames.Add(field);
        //        newFieldNames.Add(field);
        //    }

        //    if (currentPrimaryKeys.Count == 1 && !commonFields.Contains(currentPrimaryKeys[0]) && (primaryKeys.Count == 1))
        //    {
        //        currentFieldNames.Add(currentPrimaryKeys[0]);
        //        newFieldNames.Add(primaryKeys[0].Name);
        //    }

        //    string currentCommonString = UtilitiesMethods.CommaSeparatedString(currentFieldNames);
        //    string newCommonString = UtilitiesMethods.CommaSeparatedString(newFieldNames);

        //    createTempTableFromDefinition(table);

        //    string commandString;
        //    if (commonFields.Count > 0)
        //    {
        //        commandString = "insert or ignore into '" + tempName + "' (" + newCommonString + ") select " + currentCommonString + " from '" + tableName + "'";
        //        SQLiteDB.nonQueryCommand(commandString);
        //    }

        //    commandString = "drop table '" + tableName + "'";
        //    SQLiteDB.nonQueryCommand(commandString);
        //    createTableFromDefinition(table);

        //    commandString = "insert into '" + tableName + "' select * from '" + tempName + "'";
        //    SQLiteDB.nonQueryCommand(commandString);
        //    commandString = "drop table '" + tempName + "'";
        //    SQLiteDB.nonQueryCommand(commandString);

        //}
        //private static void updateVersionNumber(Type type)
        //{
        //    if (type == typeof(TECBid) || type == typeof(TECTemplates))
        //    {
        //        Dictionary<string, string> Data = new Dictionary<string, string>();
        //        if (type == typeof(TECBid))
        //        {
        //            var infoBid = getBidInfo();
        //            string commandString = "update " + BidInfoTable.TableName + " set " + BidInfoTable.DBVersion.Name + " = '" + Properties.Settings.Default.Version + "' ";
        //            commandString += "where " + BidInfoTable.BidID.Name + " = '" + infoBid.Guid.ToString() + "'";
        //            SQLiteDB.nonQueryCommand(commandString);
        //        }
        //        else if (type == typeof(TECTemplates))
        //        {
        //            var templateGuid = getTemplatesInfo().Guid;

        //            Dictionary<string, string> data = new Dictionary<string, string>();

        //            data.Add(TemplatesInfoTable.DBVersion.Name, Properties.Settings.Default.Version);
        //            data.Add(TemplatesInfoTable.TemplateID.Name, templateGuid.ToString());

        //            SQLiteDB.Replace(TemplatesInfoTable.TableName, data);
        //        }
        //    }
        //}
        //private static void killTemplatesInfo()
        //{
        //    string commandString = commandString = "drop table '" + TemplatesInfoTable.TableName + "'";
        //    SQLiteDB.nonQueryCommand(commandString);
        //}
        //#endregion

        //#region Backup Methods
        //private static void createBackup(string originalPath)
        //{
        //    DebugHandler.LogDebugMessage("Backing up...");

        //    var date = DateTime.Now;

        //    string APPDATA_FOLDER = @"TECSystems\Backups\";
        //    string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        //    string backupFolder = Path.Combine(appData, APPDATA_FOLDER);

        //    CultureInfo culture = CultureInfo.CreateSpecificCulture("ja-JP");
        //    DateTimeFormatInfo dtfi = culture.DateTimeFormat;
        //    dtfi.DateSeparator = "\\";
        //    backupFolder += date.ToString("d", dtfi);

        //    if (!Directory.Exists(backupFolder))
        //    { Directory.CreateDirectory(backupFolder); }

        //    string backupFileName = Path.GetFileNameWithoutExtension(originalPath);
        //    backupFileName += "-";
        //    culture = CultureInfo.CreateSpecificCulture("hr-HR");
        //    dtfi = culture.DateTimeFormat;
        //    dtfi.TimeSeparator = "-";
        //    backupFileName += date.ToString("T", dtfi);
        //    var backupPath = Path.Combine(backupFolder, backupFileName);

        //    File.Copy(originalPath, backupPath);

        //    DebugHandler.LogDebugMessage("Finished backup. Backup path: " + backupPath);
        //}
        //#endregion

        //#region Row to Object Methods
        //#region Base Scope
        //private static TECSystem getSystemFromRow(DataRow row)
        //{
        //    Guid guid = new Guid(row[SystemTable.SystemID.Name].ToString());
        //    TECSystem system = new TECSystem(guid);

        //    system.Name = row[SystemTable.Name.Name].ToString();
        //    system.Description = row[SystemTable.Description.Name].ToString();
        //    system.ProposeEquipment = row[SystemTable.ProposeEquipment.Name].ToString().ToInt(0).ToBool();
        //    //var watch = System.Diagnostics.Stopwatch.StartNew();
        //    system.Controllers = getControllersInSystem(guid);
        //    //watch.Stop();
        //    //Console.WriteLine("getControllersInSystem: " + watch.ElapsedMilliseconds);
        //    //watch = System.Diagnostics.Stopwatch.StartNew();
        //    system.Equipment = getEquipmentInSystem(guid);
        //    //watch.Stop();
        //    //Console.WriteLine("getEquipmentInSystem: " + watch.ElapsedMilliseconds);
        //    //watch = System.Diagnostics.Stopwatch.StartNew();
        //    system.Panels = getPanelsInSystem(guid);
        //    //watch.Stop();
        //    //Console.WriteLine("getPanelsInSystem: " + watch.ElapsedMilliseconds);
        //    //watch = System.Diagnostics.Stopwatch.StartNew();
        //    system.SystemInstances = getChildrenSystems(guid);
        //    //watch.Stop();
        //    //Console.WriteLine("getChildrenSystems: " + watch.ElapsedMilliseconds);
        //    //watch = System.Diagnostics.Stopwatch.StartNew();
        //    system.MiscCosts = getMiscInSystem(guid);
        //    //watch.Stop();
        //    //Console.WriteLine("getMiscInSystem: " + watch.ElapsedMilliseconds);
        //    //watch = System.Diagnostics.Stopwatch.StartNew();
        //    system.ScopeBranches = getScopeBranchesInSystem(guid);
        //    // watch.Stop();
        //    // Console.WriteLine("getScopeBranchesInSystem: " + watch.ElapsedMilliseconds);
        //    // watch = System.Diagnostics.Stopwatch.StartNew();
        //    getScopeChildren(system);
        //    // watch.Stop();
        //    // Console.WriteLine("getScopeChildren: " + watch.ElapsedMilliseconds);
        //    system.RefreshReferences();

        //    return system;
        //}

        //private static TECEquipment getEquipmentFromRow(DataRow row)
        //{
        //    Guid equipmentID = new Guid(row[EquipmentTable.EquipmentID.Name].ToString());
        //    TECEquipment equipmentToAdd = new TECEquipment(equipmentID);
        //    equipmentToAdd.Name = row[EquipmentTable.Name.Name].ToString();
        //    equipmentToAdd.Description = row[EquipmentTable.Description.Name].ToString();
        //    getScopeChildren(equipmentToAdd);
        //    equipmentToAdd.SubScope = getSubScopeInEquipment(equipmentID);
        //    return equipmentToAdd;
        //}
        //private static TECSubScope getSubScopeFromRow(DataRow row)
        //{
        //    Guid subScopeID = new Guid(row[SubScopeTable.SubScopeID.Name].ToString());
        //    TECSubScope subScopeToAdd = new TECSubScope(subScopeID);
        //    subScopeToAdd.Name = row[SubScopeTable.Name.Name].ToString();
        //    subScopeToAdd.Description = row[SubScopeTable.Description.Name].ToString();
        //    subScopeToAdd.Devices = getDevicesInSubScope(subScopeID);
        //    subScopeToAdd.Points = getPointsInSubScope(subScopeID);
        //    getScopeChildren(subScopeToAdd);
        //    return subScopeToAdd;
        //}
        //private static TECPoint getPointFromRow(DataRow row)
        //{
        //    Guid pointID = new Guid(row[PointTable.PointID.Name].ToString());
        //    TECPoint pointToAdd = new TECPoint(pointID);
        //    pointToAdd.Label = row[PointTable.Name.Name].ToString();
        //    pointToAdd.Type = TECPoint.convertStringToType(row[PointTable.Type.Name].ToString());
        //    pointToAdd.Quantity = row[PointTable.Quantity.Name].ToString().ToInt();
        //    return pointToAdd;
        //}
        //#endregion
        //#region Catalogs
        //private static TECElectricalMaterial getConnectionTypeFromRow(DataRow row)
        //{
        //    Guid guid = new Guid(row[ConnectionTypeTable.ConnectionTypeID.Name].ToString());
        //    string name = row[ConnectionTypeTable.Name.Name].ToString();
        //    string laborString = row[ConnectionTypeTable.Labor.Name].ToString();
        //    string costString = row[ConnectionTypeTable.Cost.Name].ToString();

        //    double cost = costString.ToDouble(0);
        //    double labor = laborString.ToDouble(0);

        //    var outConnectionType = new TECElectricalMaterial(guid);
        //    outConnectionType.Name = name;
        //    outConnectionType.Cost = cost;
        //    outConnectionType.Labor = labor;
        //    getScopeChildren(outConnectionType);
        //    outConnectionType.RatedCosts = getRatedCostsInComponent(outConnectionType.Guid);
        //    return outConnectionType;
        //}
        //private static TECElectricalMaterial getConduitTypeFromRow(DataRow row)
        //{
        //    Guid conduitGuid = new Guid(row[ConduitTypeTable.ConduitTypeID.Name].ToString());
        //    string name = row[ConduitTypeTable.Name.Name].ToString();
        //    double cost = row[ConduitTypeTable.Cost.Name].ToString().ToDouble(0);
        //    double labor = row[ConduitTypeTable.Labor.Name].ToString().ToDouble(0);
        //    var conduitType = new TECElectricalMaterial(conduitGuid);
        //    conduitType.Name = name;
        //    conduitType.Cost = cost;
        //    conduitType.Labor = labor;
        //    getScopeChildren(conduitType);
        //    conduitType.RatedCosts = getRatedCostsInComponent(conduitType.Guid);
        //    return conduitType;
        //}

        //private static TECCost getAssociatedCostFromRow(DataRow row)
        //{
        //    Guid guid = new Guid(row[AssociatedCostTable.AssociatedCostID.Name].ToString());
        //    string name = row[AssociatedCostTable.Name.Name].ToString();
        //    double cost = row[AssociatedCostTable.Cost.Name].ToString().ToDouble(0);
        //    double labor = row[AssociatedCostTable.Labor.Name].ToString().ToDouble(0);
        //    string costTypeString = row[AssociatedCostTable.Type.Name].ToString();

        //    var associatedCost = new TECCost(guid);

        //    associatedCost.Name = name;
        //    associatedCost.Cost = cost;
        //    associatedCost.Labor = labor;
        //    associatedCost.Type = UtilitiesMethods.StringToEnum(costTypeString, CostType.None);

        //    return associatedCost;
        //}
        //private static TECDevice getDeviceFromRow(DataRow row)
        //{
        //    Guid deviceID = new Guid(row[DeviceTable.DeviceID.Name].ToString());
        //    ObservableCollection<TECElectricalMaterial> connectionType = getConnectionTypesInDevice(deviceID);
        //    TECManufacturer manufacturer = getManufacturerInDevice(deviceID);
        //    TECDevice deviceToAdd = new TECDevice(deviceID, connectionType, manufacturer);
        //    deviceToAdd.Name = row[DeviceTable.Name.Name].ToString();
        //    deviceToAdd.Description = row[DeviceTable.Description.Name].ToString();
        //    deviceToAdd.Cost = row[DeviceTable.Cost.Name].ToString().ToDouble();
        //    getScopeChildren(deviceToAdd);
        //    return deviceToAdd;
        //}
        //private static TECManufacturer getManufacturerFromRow(DataRow row)
        //{
        //    Guid manufacturerID = new Guid(row[ManufacturerTable.ManufacturerID.Name].ToString());
        //    var manufacturer = new TECManufacturer(manufacturerID);
        //    manufacturer.Label = row[ManufacturerTable.Name.Name].ToString();
        //    manufacturer.Multiplier = row[ManufacturerTable.Multiplier.Name].ToString().ToDouble(1);
        //    return manufacturer;
        //}
        //private static TECLabeled getLocationFromRow(DataRow row)
        //{
        //    Guid locationID = new Guid(row[LocationTable.LocationID.Name].ToString());
        //    var location = new TECLabeled(locationID);
        //    location.Label = row[LocationTable.Name.Name].ToString();
        //    return location;
        //}
        //private static TECLabeled getTagFromRow(DataRow row)
        //{
        //    var tag = new TECLabeled(new Guid(row["TagID"].ToString()));
        //    tag.Label = row["TagString"].ToString();
        //    tag.Flavor = Flavor.Tag;
        //    return tag;
        //}
        ///// <summary>
        ///// MUST BE FIXED
        ///// </summary>
        ///// <param name="row"></param>
        ///// <returns></returns>
        //private static TECPanelType getPanelTypeFromRow(DataRow row)
        //{
        //    Guid guid = new Guid(row[PanelTypeTable.PanelTypeID.Name].ToString());
        //    TECPanelType panelType = new TECPanelType(guid, new TECManufacturer());

        //    panelType.Name = row[PanelTypeTable.Name.Name].ToString();
        //    panelType.Cost = row[PanelTypeTable.Cost.Name].ToString().ToDouble(0);
        //    panelType.Labor = row[PanelTypeTable.Labor.Name].ToString().ToDouble(0);

        //    return panelType;
        //}
        //private static TECIOModule getIOModuleFromRow(DataRow row)
        //{
        //    Guid guid = new Guid(row[IOModuleTable.IOModuleID.Name].ToString());
        //    TECManufacturer manufacturer = getManufacturerInIOModule(guid);

        //    TECIOModule module = new TECIOModule(guid, manufacturer);

        //    module.Name = row[IOModuleTable.Name.Name].ToString();
        //    module.Description = row[IOModuleTable.Description.Name].ToString();
        //    module.Cost = row[IOModuleTable.Cost.Name].ToString().ToDouble(0);
        //    module.IOPerModule = row[IOModuleTable.IOPerModule.Name].ToString().ToInt(1);
        //    return module;
        //}

        //#endregion
        //#region Scope Qualifiers
        //private static TECScopeBranch getScopeBranchFromRow(DataRow row)
        //{
        //    Guid scopeBranchID = new Guid(row[ScopeBranchTable.ScopeBranchID.Name].ToString());
        //    TECScopeBranch branch = new TECScopeBranch(scopeBranchID);
        //    branch.Label = row[ScopeBranchTable.Name.Name].ToString();
        //    branch.Branches = getChildBranchesInBranch(scopeBranchID);
        //    return branch;
        //}
        //private static TECLabeled getNoteFromRow(DataRow row)
        //{
        //    Guid noteID = new Guid(row[NoteTable.NoteID.Name].ToString());
        //    var note = new TECLabeled(noteID);
        //    note.Label = row["NoteText"].ToString();
        //    note.Flavor = Flavor.Note;
        //    return note;
        //}
        //private static TECLabeled getExclusionFromRow(DataRow row)
        //{
        //    Guid exclusionId = new Guid(row["ExclusionID"].ToString());
        //    TECLabeled exclusion = new TECLabeled(exclusionId);
        //    exclusion.Label = row["ExclusionText"].ToString();
        //    exclusion.Flavor = Flavor.Exclusion;
        //    return exclusion;
        //}
        //#endregion
        //#region Drawing Objects
        //private static TECDrawing getDrawingFromRow(DataRow row)
        //{
        //    string name = row[DrawingTable.Name.Name].ToString();
        //    string description = row[DrawingTable.Description.Name].ToString();
        //    Guid guid = new Guid(row[DrawingTable.DrawingID.Name].ToString());
        //    ObservableCollection<TECPage> pages = getPagesInDrawing(guid);
        //    return new TECDrawing(name, description, guid, pages);
        //}
        //private static TECPage getPageFromRow(DataRow row)
        //{
        //    Guid guid = new Guid(row[PageTable.PageID.Name].ToString());
        //    TECPage page = new TECPage(guid);
        //    page.PageNum = row[PageTable.PageNum.Name].ToString().ToInt();
        //    byte[] blob = row[PageTable.Image.Name] as byte[];
        //    page.Path = Path.GetTempFileName();
        //    File.WriteAllBytes(page.Path, blob);
        //    page.PageScope = getVisualScopeInPage(guid);
        //    return page;
        //}
        //private static TECVisualScope getVisualScopeFromRow(DataRow row)
        //{
        //    Guid guid = new Guid(row[VisualScopeTable.VisualScopeID.Name].ToString());
        //    var visualScope = new TECVisualScope(guid);
        //    visualScope.X = row[VisualScopeTable.XPos.Name].ToString().ToDouble();
        //    visualScope.Y = row[VisualScopeTable.YPos.Name].ToString().ToDouble();
        //    visualScope.Scope = getScopeGuidInVisualScope(guid);
        //    return visualScope;
        //}
        //#endregion
        //#region Control Scope
        //private static TECPanel getPanelFromRow(DataRow row)
        //{
        //    Guid guid = new Guid(row[PanelTable.PanelID.Name].ToString());
        //    TECPanelType type = getPanelTypeInPanel(guid);
        //    TECPanel panel = new TECPanel(guid, type);

        //    panel.Name = row[PanelTable.Name.Name].ToString();
        //    panel.Description = row[PanelTable.Description.Name].ToString();
        //    panel.Controllers = getControllersInPanel(guid);
        //    getScopeChildren(panel);

        //    return panel;
        //}
        //private static TECController getControllerFromRow(DataRow row)
        //{
        //    Guid guid = new Guid(row[ControllerTable.ControllerID.Name].ToString());
        //    TECController controller = new TECController(guid, getTypeInController(guid));

        //    controller.Name = row[ControllerTable.Name.Name].ToString();
        //    controller.Description = row[ControllerTable.Description.Name].ToString();
        //    controller.NetworkType = UtilitiesMethods.StringToEnum<NetworkType>(row[ControllerTable.Type.Name].ToString(), 0);
        //    getScopeChildren(controller);
        //    controller.ChildrenConnections = getConnectionsInController(controller);
        //    return controller;
        //}
        //private static TECIO getIOFromRow(DataRow row)
        //{
        //    Guid guid = new Guid(row[IOTable.IOID.Name].ToString());
        //    var io = new TECIO(guid);
        //    io.Type = TECIO.convertStringToType(row[IOTable.IOType.Name].ToString());
        //    io.Quantity = row[IOTable.Quantity.Name].ToString().ToInt();
        //    io.IOModule = getModuleInIO(guid);
        //    return io;
        //}
        //private static TECSubScopeConnection getSubScopeConnectionFromRow(DataRow row)
        //{
        //    Guid guid = new Guid(row[SubScopeConnectionTable.ConnectionID.Name].ToString());
        //    TECSubScopeConnection connection = new TECSubScopeConnection(guid);
        //    connection.Length = row[SubScopeConnectionTable.Length.Name].ToString().ToDouble();
        //    connection.ConduitLength = row[SubScopeConnectionTable.ConduitLength.Name].ToString().ToDouble(0);
        //    connection.ConduitType = getConduitTypeInConnection(connection.Guid);
        //    connection.SubScope = getSubScopeInSubScopeConnection(connection.Guid);
        //    return connection;
        //}
        //private static TECNetworkConnection getNetworkConnectionFromRow(DataRow row)
        //{
        //    Guid guid = new Guid(row[NetworkConnectionTable.ConnectionID.Name].ToString());
        //    TECNetworkConnection connection = new TECNetworkConnection(guid);
        //    connection.Length = row[NetworkConnectionTable.Length.Name].ToString().ToDouble();
        //    connection.ConduitLength = row[NetworkConnectionTable.ConduitLength.Name].ToString().ToDouble(0);
        //    connection.IOType = UtilitiesMethods.StringToEnum<IOType>(row[NetworkConnectionTable.IOType.Name].ToString());
        //    connection.ConduitType = getConduitTypeInConnection(connection.Guid);
        //    connection.ChildrenControllers = getControllersInNetworkConnection(connection.Guid);
        //    connection.ConnectionType = getConnectionTypeInNetworkConnection(connection.Guid);
        //    return connection;
        //}
        //#endregion

        //#region Misc
        //private static TECMisc getMiscFromRow(DataRow row)
        //{
        //    Guid guid = new Guid(row[MiscTable.MiscID.Name].ToString());
        //    TECMisc cost = new TECMisc(guid);

        //    cost.Name = row[MiscTable.Name.Name].ToString();
        //    cost.Cost = row[MiscTable.Cost.Name].ToString().ToDouble(0);
        //    cost.Labor = row[MiscTable.Labor.Name].ToString().ToDouble(0);
        //    cost.Quantity = row[MiscTable.Quantity.Name].ToString().ToInt(1);
        //    string costTypeString = row[AssociatedCostTable.Type.Name].ToString();
        //    cost.Type = UtilitiesMethods.StringToEnum(costTypeString, CostType.None);
        //    getScopeChildren(cost);
        //    return cost;
        //}
        //private static TECBidParameters getBidParametersFromRow(DataRow row)
        //{
        //    Guid guid = new Guid(row[BidParametersTable.ParametersID.Name].ToString());
        //    TECBidParameters paramters = new TECBidParameters(guid);

        //    paramters.Escalation = row[BidParametersTable.Escalation.Name].ToString().ToDouble(0);
        //    paramters.Overhead = row[BidParametersTable.Overhead.Name].ToString().ToDouble(0);
        //    paramters.Profit = row[BidParametersTable.Profit.Name].ToString().ToDouble(0);
        //    paramters.SubcontractorMarkup = row[BidParametersTable.SubcontractorMarkup.Name].ToString().ToDouble(0);
        //    paramters.SubcontractorEscalation = row[BidParametersTable.SubcontractorEscalation.Name].ToString().ToDouble(0);

        //    paramters.IsTaxExempt = row[BidParametersTable.IsTaxExempt.Name].ToString().ToInt(0).ToBool();
        //    paramters.RequiresBond = row[BidParametersTable.RequiresBond.Name].ToString().ToInt(0).ToBool();
        //    paramters.RequiresWrapUp = row[BidParametersTable.RequiresWrapUp.Name].ToString().ToInt(0).ToBool();

        //    return paramters;
        //}

        //#endregion

        //private static void getScopeChildren(TECScope scope)
        //{
        //    scope.Tags = getTagsInScope(scope.Guid);
        //    scope.AssociatedCosts = getAssociatedCostsInScope(scope.Guid);
        //}

        //private static void getLocatedChildren(TECLocated located)
        //{
        //    located.Location = getLocationInScope(located.Guid);
        //}

        //#region Placeholder
        //private static TECSubScope getSubScopeConnectionChildPlaceholderFromRow(DataRow row)
        //{
        //    Guid subScopeID = new Guid(row[SubScopeConnectionChildrenTable.ChildID.Name].ToString());
        //    TECSubScope subScopeToAdd = new TECSubScope(subScopeID);
        //    return subScopeToAdd;
        //}
        //private static TECController getControllerPlaceholderFromRow(DataRow row)
        //{
        //    Guid guid = new Guid(row[ControllerTable.ControllerID.Name].ToString());
        //    TECController controller = new TECController(guid, new TECControllerType(new TECManufacturer()));

        //    controller.Name = row[ControllerTable.Name.Name].ToString();
        //    controller.Description = row[ControllerTable.Description.Name].ToString();
        //    return controller;
        //}
        //private static TECLabeled getPlaceholderTagFromRow(DataRow row)
        //{
        //    Guid guid = new Guid(row[ScopeTagTable.TagID.Name].ToString());
        //    TECLabeled tag = new TECLabeled(guid);
        //    return tag;
        //}
        //private static TECCost getPlaceholderAssociatedCostFromRow(DataRow row)
        //{
        //    Guid guid = new Guid(row[ScopeAssociatedCostTable.AssociatedCostID.Name].ToString());
        //    TECCost associatedCost = new TECCost(guid);
        //    return associatedCost;
        //}
        //private static TECCost getPlaceholderRatedCostFromRow(DataRow row)
        //{
        //    Guid guid = new Guid(row[ElectricalMaterialRatedCostTable.CostID.Name].ToString());
        //    TECCost associatedCost = new TECCost(guid);
        //    return associatedCost;
        //}
        //private static TECLabeled getPlaceholderLocationFromRow(DataRow row)
        //{
        //    Guid guid = new Guid(row[LocationScopeTable.LocationID.Name].ToString());
        //    TECLabeled location = new TECLabeled(guid);
        //    return location;
        //}
        //private static TECDevice getPlaceholderSubScopeDeviceFromRow(DataRow row)
        //{
        //    Guid guid = new Guid(row[SubScopeDeviceTable.DeviceID.Name].ToString());
        //    ObservableCollection<TECElectricalMaterial> connectionTypes = new ObservableCollection<TECElectricalMaterial>();
        //    TECManufacturer manufacturer = new TECManufacturer();
        //    TECDevice device = new TECDevice(guid, connectionTypes, manufacturer);
        //    device.Description = "placeholder";
        //    return device;
        //}
        //private static TECManufacturer getPlaceholderDeviceManufacturerFromRow(DataRow row)
        //{
        //    Guid guid = new Guid(row[DeviceManufacturerTable.ManufacturerID.Name].ToString());
        //    TECManufacturer man = new TECManufacturer(guid);
        //    return man;
        //}
        //private static TECControllerType getPlaceholderControllerTypeFromRow(DataRow row)
        //{
        //    Guid guid = new Guid(row[ControllerManufacturerTable.ManufacturerID.Name].ToString());
        //    TECControllerType type = new TECControllerType(guid, new TECManufacturer());
        //    return type;
        //}
        //private static TECElectricalMaterial getPlaceholderDeviceConnectionTypeFromRow(DataRow row)
        //{
        //    Guid guid = new Guid(row[DeviceConnectionTypeTable.TypeID.Name].ToString());
        //    TECElectricalMaterial connectionType = new TECElectricalMaterial(guid);
        //    return connectionType;
        //}
        //private static TECIOModule getPlaceholderIOModuleFromRow(DataRow row)
        //{
        //    Guid guid = new Guid(row[IOIOModuleTable.ModuleID.Name].ToString());
        //    TECIOModule module = new TECIOModule(guid, new TECManufacturer());
        //    module.Description = "placeholder";
        //    return module;
        //}
        //private static void addRowToPlaceholderDict(DataRow row, Dictionary<Guid, List<Guid>> dict)
        //{
        //    Guid key = new Guid(row[CharacteristicScopeInstanceScopeTable.CharacteristicID.Name].ToString());
        //    Guid value = new Guid(row[CharacteristicScopeInstanceScopeTable.InstanceID.Name].ToString());

        //    if (!dict.ContainsKey(key))
        //    {
        //        dict[key] = new List<Guid>();
        //    }
        //    dict[key].Add(value);
        //}
        //#endregion
        //#endregion

        //#region Generic Create Methods
        //static private void createTableFromDefinition(TableBase table)
        //{
        //    var tableInfo = new TableInfo(table);
        //    string tableName = tableInfo.Name;
        //    List<TableField> primaryKey = tableInfo.PrimaryFields;
        //    List<TableField> fields = tableInfo.Fields;

        //    string createString = "CREATE TABLE '" + tableName + "' (";
        //    foreach (TableField field in fields)
        //    {
        //        createString += "'" + field.Name + "' " + field.FieldType;
        //        if (fields.IndexOf(field) < (fields.Count - 1))
        //        { createString += ", "; }
        //    }
        //    if (primaryKey.Count != 0)
        //    { createString += ", PRIMARY KEY("; }
        //    foreach (TableField pk in primaryKey)
        //    {
        //        createString += "'" + pk.Name + "' ";
        //        if (primaryKey.IndexOf(pk) < (primaryKey.Count - 1))
        //        { createString += ", "; }
        //        else
        //        { createString += ")"; }
        //    }
        //    createString += ")";
        //    SQLiteDB.nonQueryCommand(createString);
        //}
        //static private void createTempTableFromDefinition(TableBase table)
        //{
        //    var tableInfo = new TableInfo(table);
        //    string tableName = "temp_" + tableInfo.Name;
        //    List<TableField> primaryKey = tableInfo.PrimaryFields;
        //    List<TableField> fields = tableInfo.Fields;

        //    string createString = "CREATE TEMPORARY TABLE '" + tableName + "' (";
        //    foreach (TableField field in fields)
        //    {
        //        createString += "'" + field.Name + "' " + field.FieldType;
        //        if (fields.IndexOf(field) < (fields.Count - 1))
        //        { createString += ", "; }
        //    }
        //    if (primaryKey.Count != 0)
        //    { createString += ", PRIMARY KEY("; }
        //    foreach (TableField pk in primaryKey)
        //    {
        //        createString += "'" + pk.Name + "' ";
        //        if (primaryKey.IndexOf(pk) < (primaryKey.Count - 1))
        //        { createString += ", "; }
        //        else
        //        { createString += ")"; }
        //    }
        //    createString += ")";
        //    SQLiteDB.nonQueryCommand(createString);
        //}
        //static private void createAllBidTables()
        //{
        //    foreach (TableBase table in AllBidTables.Tables)
        //    {
        //        createTableFromDefinition(table);
        //    }
        //}
        //static private void createAllTemplateTables()
        //{
        //    foreach (TableBase table in AllTemplateTables.Tables)
        //    {
        //        createTableFromDefinition(table);
        //    }
        //}
        //#endregion

        //#region Complete Save Methods
        //private static void saveScopeManagerProperties(TECScopeManager scopeManager)
        //{
        //    addObject(new StackItem(StackChange.Add, scopeManager, scopeManager.Labor));
        //    saveCompleteCatalogs(scopeManager.Catalogs);
        //}
        //private static void saveCompleteBid(TECBid bid)
        //{
        //    indexesToUpdate = new Dictionary<TableBase, List<StackItem>>();
        //    addObject(new StackItem(StackChange.Add, bid, bid));
        //    saveScopeManagerProperties(bid);
        //    addObject(new StackItem(StackChange.Add, bid, bid.Parameters));
        //    foreach (TECSystem system in bid.Systems)
        //    {
        //        addObject(new StackItem(StackChange.Add, bid, system));
        //        saveFullSystem(system, bid);
        //    }
        //    foreach (TECController controller in bid.Controllers)
        //    {
        //        addObject(new StackItem(StackChange.Add, bid, controller));
        //        saveScopeChildProperties(controller);
        //        saveControllerChildProperties(controller);
        //    }
        //    foreach (TECLabeled note in bid.Notes)
        //    { addObject(new StackItem(StackChange.Add, bid, note)); }
        //    foreach (TECLabeled exclusion in bid.Exclusions)
        //    { addObject(new StackItem(StackChange.Add, bid, exclusion)); }
        //    foreach (TECLabeled location in bid.Locations)
        //    { addObject(new StackItem(StackChange.Add, bid, location)); }
        //    foreach (TECScopeBranch branch in bid.ScopeTree)
        //    {
        //        addObject(new StackItem(StackChange.Add, bid, branch));
        //        saveCompleteScopeBranch(branch);
        //    }
        //    foreach (TECDrawing drawing in bid.Drawings)
        //    {
        //        addObject(new StackItem(StackChange.Add, bid, drawing));
        //        saveCompletePage(drawing);
        //    }
        //    foreach (TECMisc cost in bid.MiscCosts)
        //    {
        //        addObject(new StackItem(StackChange.Add, bid, cost, typeof(TECBid), typeof(TECMisc)));
        //    }
        //    foreach (TECPanel panel in bid.Panels)
        //    {
        //        savePanel(panel, bid);
        //    }
        //    saveIndexRelationships(indexesToUpdate);
        //}
        //private static void saveCompleteTemplate(TECTemplates templates)
        //{
        //    indexesToUpdate = new Dictionary<TableBase, List<StackItem>>();
        //    addObject(new StackItem(StackChange.Add, templates, templates));
        //    saveScopeManagerProperties(templates);
        //    foreach (TECSystem system in templates.SystemTemplates)
        //    {
        //        addObject(new StackItem(StackChange.Add, templates, system));
        //        saveFullSystem(system, templates);
        //    }
        //    foreach (TECEquipment equipment in templates.EquipmentTemplates)
        //    {
        //        addObject(new StackItem(StackChange.Add, templates, equipment));
        //        saveScopeChildProperties(equipment);
        //        saveCompleteSubScope(equipment);
        //    }
        //    foreach (TECSubScope subScope in templates.SubScopeTemplates)
        //    {
        //        addObject(new StackItem(StackChange.Add, templates, subScope));
        //        saveScopeChildProperties(subScope);
        //        saveDevicesInSubScope(subScope);
        //        saveCompletePoints(subScope);
        //    }
        //    foreach (TECController controller in templates.ControllerTemplates)
        //    {
        //        addObject(new StackItem(StackChange.Add, templates, controller));
        //        saveScopeChildProperties(controller);
        //        saveControllerChildProperties(controller);
        //    }
        //    foreach (TECMisc cost in templates.MiscCostTemplates)
        //    {
        //        addObject(new StackItem(StackChange.Add, templates, cost));
        //    }
        //    foreach (TECPanel panel in templates.PanelTemplates)
        //    {
        //        savePanel(panel, templates);
        //    }
        //    saveIndexRelationships(indexesToUpdate);
        //}
        //private static void saveCompleteCatalogs(TECCatalogs catalogs)
        //{
        //    foreach (TECManufacturer manufacturer in catalogs.Manufacturers)
        //    { addObject(new StackItem(StackChange.Add, catalogs, manufacturer)); }
        //    foreach (TECIOModule ioModule in catalogs.IOModules)
        //    {
        //        saveCompleteIOModule(ioModule, catalogs);
        //    }
        //    foreach (TECPanelType panelType in catalogs.PanelTypes)
        //    {
        //        addObject(new StackItem(StackChange.Add, catalogs, panelType));
        //    }
        //    foreach (TECDevice device in catalogs.Devices)
        //    {
        //        addObject(new StackItem(StackChange.Add, catalogs, device));
        //        saveScopeChildProperties(device);
        //        saveDeviceChildProperties(device);
        //    }
        //    foreach (TECElectricalMaterial conduitType in catalogs.ConduitTypes)
        //    {
        //        addObject(new StackItem(StackChange.Add, catalogs, conduitType));
        //        saveScopeChildProperties(conduitType);
        //        saveRatedCosts(conduitType);
        //    }
        //    foreach (TECElectricalMaterial connectionType in catalogs.ConnectionTypes)
        //    {
        //        addObject(new StackItem(StackChange.Add, catalogs, connectionType));
        //        saveScopeChildProperties(connectionType);
        //        saveRatedCosts(connectionType);
        //    }
        //    foreach (TECLabeled tag in catalogs.Tags)
        //    { addObject(new StackItem(StackChange.Add, catalogs, tag)); }
        //    foreach (TECCost associatedCost in catalogs.AssociatedCosts)
        //    { addObject(new StackItem(StackChange.Add, catalogs, associatedCost)); }
        //}

        //private static void saveFullSystem(TECSystem system, TECScopeManager scopeManager)
        //{
        //    var change = StackChange.Add;
        //    saveScopeChildProperties(system);
        //    saveCompleteEquipment(system);
        //    foreach (TECPanel panel in system.Panels)
        //    {
        //        savePanel(panel, system);
        //    }
        //    foreach (TECController controller in system.Controllers)
        //    {
        //        addObject(new StackItem(change, system, controller));
        //        saveScopeChildProperties(controller);
        //        saveControllerChildProperties(controller);
        //    }
        //    foreach (TECMisc misc in system.MiscCosts)
        //    {
        //        addObject(new StackItem(change, system, misc, typeof(TECSystem), typeof(TECMisc)));
        //    }
        //    foreach (TECScopeBranch branch in system.ScopeBranches)
        //    {
        //        addObject(new StackItem(change, system, branch));
        //    }
        //    foreach (TECSystem childScope in system.SystemInstances)
        //    {
        //        addObject(new StackItem(change, system, childScope));
        //        saveFullSystem(childScope, scopeManager);
        //    }
        //    foreach (KeyValuePair<TECObject, List<TECObject>> item in system.CharactersticInstances.GetFullDictionary())
        //    {
        //        foreach (TECScope value in item.Value)
        //        {
        //            addRelationship(new StackItem(StackChange.AddRelationship, item.Key, value, typeof(TECObject), typeof(TECObject)));
        //        }
        //    }
        //}

        //private static void saveDevicesInSubScope(TECSubScope subscope)
        //{
        //    foreach (TECDevice device in subscope.Devices)
        //    { addObject(new StackItem(StackChange.Add, subscope, device)); }
        //}
        //private static void saveDeviceInCatalog(TECDevice device, object bidOrTemplates)
        //{
        //    if (bidOrTemplates is TECBid || bidOrTemplates is TECTemplates)
        //    {
        //        addObject(new StackItem(StackChange.Add, bidOrTemplates, device));
        //        saveScopeChildProperties(device);
        //        saveDeviceChildProperties(device);
        //    }
        //}
        //private static void saveCompletePoints(TECSubScope subScope)
        //{
        //    foreach (TECPoint point in subScope.Points)
        //    {
        //        addObject(new StackItem(StackChange.Add, subScope, point));
        //    }
        //}
        //private static void saveCompleteSubScope(TECEquipment equipment)
        //{
        //    foreach (TECSubScope subScope in equipment.SubScope)
        //    {
        //        addObject(new StackItem(StackChange.Add, equipment, subScope));
        //        saveScopeChildProperties(subScope);
        //        saveDevicesInSubScope(subScope);
        //        saveCompletePoints(subScope);
        //    }
        //}
        //private static void saveCompleteEquipment(TECSystem system)
        //{
        //    foreach (TECEquipment equipment in system.Equipment)
        //    {
        //        addObject(new StackItem(StackChange.Add, system, equipment));
        //        saveScopeChildProperties(equipment);
        //        saveCompleteSubScope(equipment);
        //    }
        //}
        //private static void saveCompleteScopeBranch(TECScopeBranch branch)
        //{
        //    foreach (TECScopeBranch subBranch in branch.Branches)
        //    {
        //        addObject(new StackItem(StackChange.Add, branch, subBranch));
        //        saveCompleteScopeBranch(subBranch);
        //    }
        //}
        //private static void saveCompleteVisualScope(TECPage page)
        //{
        //    foreach (TECVisualScope visualScope in page.PageScope)
        //    { addObject(new StackItem(StackChange.Add, page, visualScope)); }
        //}
        //private static void saveCompleteVisualConnections(TECPage page)
        //{
        //    foreach (TECVisualConnection visualConnection in page.Connections)
        //    { addObject(new StackItem(StackChange.Add, page, visualConnection)); }
        //}
        //private static void saveCompletePage(TECDrawing drawing)
        //{
        //    foreach (TECPage page in drawing.Pages)
        //    {
        //        addObject(new StackItem(StackChange.Add, drawing, page));
        //        saveCompleteVisualScope(page);
        //        saveCompleteVisualConnections(page);
        //    }
        //}
        //private static void saveScopeChildProperties(TECScope scope)
        //{
        //    saveTags(scope);
        //    saveAssociatedCosts(scope);
        //}
        //private static void saveLocatedChildProperties(TECLocated located)
        //{
        //    saveLocation(located);
        //    saveScopeChildProperties(located);
        //}
        //private static void saveLocation(TECLocated located)
        //{
        //    if (located.Location != null)
        //    {
        //        addObject(new StackItem(StackChange.Add, located, located.Location, typeof(TECScope), typeof(TECLabeled)));
        //    }
        //}
        //private static void saveTags(TECScope scope)
        //{
        //    foreach (TECLabeled tag in scope.Tags)
        //    {
        //        addObject(new StackItem(StackChange.Add, scope, tag, typeof(TECScope), typeof(TECLabeled)));
        //    }
        //}
        //private static void saveAssociatedCosts(TECScope scope)
        //{
        //    foreach (TECCost cost in scope.AssociatedCosts)
        //    {
        //        addObject(new StackItem(StackChange.Add, scope, cost, typeof(TECScope), typeof(TECCost)));
        //    }
        //}
        //private static void saveRatedCosts(TECElectricalMaterial component)
        //{
        //    foreach (TECCost cost in component.RatedCosts)
        //    {
        //        addObject(new StackItem(StackChange.Add, component, cost, typeof(ElectricalMaterialComponent), typeof(TECCost)));
        //    }
        //}
        //private static void saveControllerChildProperties(TECController controller)
        //{
        //    if (controller.Type != null) { addObject(new StackItem(StackChange.AddRelationship, controller, controller.Type)); }
        //    foreach (TECIO IO in controller.Type.IO)
        //    {
        //        addObject(new StackItem(StackChange.Add, controller, IO));
        //        if (IO.IOModule != null)
        //        {
        //            addObject(new StackItem(StackChange.Add, IO, IO.IOModule));
        //        }
        //    }
        //    foreach (TECConnection connection in controller.ChildrenConnections)
        //    {
        //        addObject(new StackItem(StackChange.Add, controller, connection, typeof(TECController), typeof(TECConnection)));
        //        saveConnectionChildren(connection);
        //    }
        //}
        //private static void saveDeviceChildProperties(TECDevice device)
        //{
        //    if (device.Manufacturer != null) { addObject(new StackItem(StackChange.Add, device, device.Manufacturer)); }
        //    foreach (TECElectricalMaterial type in device.ConnectionTypes)
        //    {
        //        addObject(new StackItem(StackChange.Add, device, type));
        //    }
        //}
        //private static void savePanel(TECPanel panel, object parent)
        //{
        //    addObject(new StackItem(StackChange.Add, parent, panel));
        //    addObject(new StackItem(StackChange.Add, panel, panel.Type));
        //    foreach (TECController controller in panel.Controllers)
        //    {
        //        addRelationship(new StackItem(StackChange.AddRelationship, panel, controller));
        //    }
        //    saveScopeChildProperties(panel);

        //}
        //private static void saveConnectionChildren(TECConnection connection)
        //{
        //    if (connection is TECNetworkConnection)
        //    {
        //        saveNetworkConnectionChildProperties(connection as TECNetworkConnection);
        //    }
        //    else if (connection is TECSubScopeConnection)
        //    {
        //        if ((connection as TECSubScopeConnection).SubScope != null) { addRelationship(new StackItem(StackChange.AddRelationship, connection, (connection as TECSubScopeConnection).SubScope, typeof(TECSubScopeConnection), typeof(TECSubScope))); }
        //    }

        //    if (connection.ConduitType != null) { addObject(new StackItem(StackChange.AddRelationship, connection, connection.ConduitType, typeof(TECConnection), typeof(TECElectricalMaterial))); }
        //}
        //private static void saveCompleteIOModule(TECIOModule ioModule, object parent)
        //{
        //    addObject(new StackItem(StackChange.Add, parent, ioModule));
        //    addObject(new StackItem(StackChange.Add, ioModule.Manufacturer, ioModule));
        //}
        //private static void saveNetworkConnectionChildProperties(TECNetworkConnection netConnect)
        //{
        //    addObject(new StackItem(StackChange.Add, netConnect, netConnect.ConnectionType));
        //    foreach (TECController controller in netConnect.ChildrenControllers)
        //    {
        //        addRelationship(new StackItem(StackChange.Add, netConnect, controller, typeof(TECNetworkConnection), typeof(TECController)));
        //    }
        //}

        //#endregion

        //#region Generic Add Methods
        //private static void addObject(StackItem item)
        //{
        //    //ObjectsToAdd = [targetObject, referenceObject];
        //    var relevantTables = getRelevantTablesForAddRemove(item);
        //    addToTables(item, relevantTables);
        //}
        //private static void addToTables(StackItem item, List<TableBase> tables)
        //{
        //    foreach (TableBase table in tables)
        //    {
        //        var tableInfo = new TableInfo(table);
        //        if (tableInfo.IsRelationTable)
        //        { addToIndexUpdates(indexesToUpdate, item, table); }
        //        //{ updateIndexedRelation(table, item); }
        //        else
        //        { addObjectToTable(table, item); }
        //    }
        //}
        //private static void addObjectToTable(TableBase table, StackItem item)
        //{
        //    var tableInfo = new TableInfo(table);

        //    Dictionary<string, string> data = assembleDataToAddRemove(table, item);

        //    if (data.Count > 0)
        //    {
        //        if (!SQLiteDB.Insert(tableInfo.Name, data))
        //        {
        //            DebugHandler.LogError("Error: Couldn't add data to " + tableInfo.Name + " table.");
        //        }
        //    }
        //}
        //private static void addToIndexUpdates(Dictionary<TableBase, List<StackItem>> updates, StackItem item, TableBase table)
        //{
        //    if (!updates.ContainsKey(table))
        //    {
        //        var stackForTable = new List<StackItem>();
        //        stackForTable.Add(item);
        //        updates[table] = stackForTable;
        //    }
        //    else
        //    {
        //        bool alreadyRepresented = false;
        //        foreach (StackItem updateItem in updates[table])
        //        {
        //            if (updateItem.ReferenceObject == item.ReferenceObject)
        //            {
        //                alreadyRepresented = true;
        //            }
        //        }
        //        if (!alreadyRepresented)
        //        {
        //            updates[table].Add(item);
        //        }
        //    }
        //}

        //private static void updateIndexedRelation(TableBase table, StackItem item)
        //{
        //    var tableInfo = new TableInfo(table);
        //    var referenceCopy = (item.ReferenceObject as TECObject).Copy();

        //    var childrenCollection = UtilitiesMethods.GetChildCollection(item.TargetType, referenceCopy, item.ReferenceType);

        //    foreach (TECObject child in (IList)childrenCollection)
        //    {
        //        Dictionary<string, string> data = new Dictionary<string, string>();
        //        foreach (TableField field in tableInfo.Fields)
        //        {
        //            if (field.Property.Name == "Index" && field.Property.ReflectedType == typeof(HelperProperties))
        //            {
        //                var dataString = objectToDBString(((IList)childrenCollection).IndexOf(child));
        //                data.Add(field.Name, dataString);
        //            }
        //            else if (field.Property.Name == "Quantity" && field.Property.ReflectedType == typeof(HelperProperties))
        //            {
        //                var dataString = objectToDBString(getQuantityInParentCollection(child, item.ReferenceObject, item.ReferenceType));
        //                data.Add(field.Name, dataString);
        //            }
        //            var assemblyItem = new StackItem(StackChange.Add, item.ReferenceObject, child, item.ReferenceType, item.TargetType);
        //            assembleDataWithItem(data, assemblyItem, field);
        //        }
        //        if (data.Count > 0)
        //        {
        //            if (!SQLiteDB.Replace(tableInfo.Name, data))
        //            {
        //                DebugHandler.LogError("Couldn't add data to " + tableInfo.Name + " table.");
        //            }
        //        }
        //    }

        //}

        //private static void addRelationship(StackItem item)
        //{
        //    //ObjectsToAdd = [targetObject, referenceObject];
        //    var relevantTables = getRelevantTablesForAddRemoveRelationship(item);
        //    addToTables(item, relevantTables);
        //}
        //private static void saveIndexRelationships(Dictionary<TableBase, List<StackItem>> updates)
        //{
        //    foreach (KeyValuePair<TableBase, List<StackItem>> item in updates)
        //    {
        //        foreach (StackItem stackItem in item.Value)
        //        {
        //            updateIndexedRelation(item.Key, stackItem);
        //        }
        //    }
        //}

        //#endregion

        //#region Generic Remove Methods
        //private static void removeObject(StackItem item)
        //{
        //    var relevantTables = getRelevantTablesForAddRemove(item);
        //    foreach (TableBase table in relevantTables)
        //    {
        //        var tableInfo = new TableInfo(table);
        //        removeObjectFromTable(table, item);
        //    }

        //}
        //private static void removeObjectFromTable(TableBase table, StackItem item)
        //{
        //    var tableInfo = new TableInfo(table);

        //    if (fieldsIncludeQuantity(tableInfo.Fields))
        //    {
        //        var qty = getQuantityInParentCollection(item.TargetObject, item.ReferenceObject);
        //        if (qty > 1)
        //        {
        //            editObjectInTable(table, item);
        //            return;
        //        }
        //    }

        //    Dictionary<string, string> data = assembleDataToAddRemove(table, item);
        //    if (data.ContainsKey("Quantity"))
        //    {
        //        data.Remove("Quantity");
        //    }

        //    if (data.Count > 0)
        //    {
        //        if (!SQLiteDB.Delete(tableInfo.Name, data))
        //        { DebugHandler.LogError("Couldn't remove data from " + tableInfo.Name + " table."); }
        //    }
        //}

        //private static void removeRelationship(StackItem item)
        //{
        //    var relevantTables = getRelevantTablesForAddRemoveRelationship(item);
        //    foreach (TableBase table in relevantTables)
        //    {
        //        var tableInfo = new TableInfo(table);
        //        removeObjectFromTable(table, item);
        //    }

        //}
        //#endregion

        //#region Generic Edit Methods
        //private static void editObject(StackItem item)
        //{
        //    var relevantTables = getRelevantTablesToEdit(item);
        //    foreach (TableBase table in relevantTables)
        //    {
        //        var tableInfo = new TableInfo(table);
        //        if (tableInfo.IsRelationTable)
        //        { addToIndexUpdates(indexesToUpdate, item, table); }
        //        else
        //        { editObjectInTable(table, item); }


        //    }
        //}
        //private static void editObjectInTable(TableBase table, StackItem item)
        //{
        //    var tableInfo = new TableInfo(table);
        //    var relevantObjects = new Object[]
        //        { item.TargetObject,
        //        item.ReferenceObject};
        //    Dictionary<string, string> data = new Dictionary<string, string>();

        //    if (item.TargetType == item.ReferenceType)
        //    {
        //        relevantObjects = new Object[]
        //        { item.TargetObject };
        //    }
        //    foreach (TableField field in tableInfo.Fields)
        //    {
        //        if ((field.Property != null) && (field.Property.Name == "Quantity" && field.Property.ReflectedType == typeof(HelperProperties)))
        //        {
        //            var dataString = objectToDBString(getQuantityInParentCollection(item.TargetObject, item.ReferenceObject));
        //            data.Add(field.Name, dataString);
        //        }
        //        else if (field.Property.Name == "DBVersion" && field.Property.ReflectedType == typeof(HelperProperties))
        //        {
        //            var dataString = objectToDBString(Properties.Settings.Default.Version);
        //            data.Add(field.Name, dataString);
        //        }
        //        assembleDataWithItem(data, item, field);
        //    }

        //    if (data.Count > 0)
        //    {
        //        if (!SQLiteDB.Replace(tableInfo.Name, data))
        //        { DebugHandler.LogError("Couldn't edit data in " + tableInfo.Name + " table."); }
        //    }
        //}
        //#endregion

        //#region Helper Methods
        //private static Dictionary<string, string> assembleDataToAddRemove(TableBase table, StackItem item)
        //{
        //    var tableInfo = new TableInfo(table);
        //    var relevantObjects = new object[]
        //    {
        //        item.TargetObject,
        //        item.ReferenceObject
        //    };
        //    var relevantTypes = new Type[]
        //    {
        //        item.TargetType,
        //        item.ReferenceType
        //    };
        //    Dictionary<string, string> data = new Dictionary<string, string>();

        //    var isHierarchial = false;
        //    if (tableInfo.Types.Count == 2 && tableInfo.Types[0] == tableInfo.Types[1])
        //    {
        //        isHierarchial = true;
        //        relevantObjects = new object[]
        //        {
        //            item.ReferenceObject,
        //            item.TargetObject
        //        };
        //        relevantTypes = new Type[]
        //        {
        //            item.ReferenceType,
        //            item.TargetType
        //        };
        //    }
        //    else if (tableInfo.Types.Count == 1)
        //    {
        //        relevantObjects = new object[]
        //        { item.TargetObject };
        //    }
        //    if (tableInfo.PrimaryFields.Count == 1 && item.Change == StackChange.Remove)
        //    {
        //        return assembleDataWithItem(data, item, tableInfo.PrimaryFields[0]);
        //    }

        //    int currentField = 0;
        //    foreach (TableField field in tableInfo.Fields)
        //    {
        //        if (isHierarchial)
        //        {
        //            if (isFieldType(field, relevantObjects[currentField], relevantTypes[currentField]))
        //            {
        //                DebugHandler.LogDebugMessage("Adding " + field.Name + " to table " + tableInfo.Name + " with type " + relevantObjects[currentField].GetType(), DebugBooleans.Generic);

        //                var dataString = objectToDBString(field.Property.GetValue(relevantObjects[currentField], null));
        //                data.Add(field.Name, dataString);
        //            }
        //        }
        //        else
        //        {
        //            if (field.Property.Name == "Quantity" && field.Property.ReflectedType == typeof(HelperProperties))
        //            {
        //                var dataString = objectToDBString(getQuantityInParentCollection(item.TargetObject, item.ReferenceObject));
        //                data.Add(field.Name, dataString);
        //            }
        //            else if (field.Property.Name == "DBVersion" && field.Property.ReflectedType == typeof(HelperProperties))
        //            {
        //                var dataString = objectToDBString(Properties.Settings.Default.Version);
        //                data.Add(field.Name, dataString);
        //            }
        //            assembleDataWithItem(data, item, field);
        //        }
        //        currentField++;
        //    }

        //    return data;
        //}
        //private static Dictionary<string, string> assembleDataWithItem(Dictionary<string, string> data, StackItem item, TableField field)
        //{
        //    if (isFieldType(field, item.TargetObject, item.TargetType))
        //    {
        //        DebugHandler.LogDebugMessage("Changing " + field.Name + " with type " + item.TargetType, DebugBooleans.Generic);
        //        addFieldPropertyToData(field, item.TargetObject, data);
        //    }
        //    else if (isFieldType(field, item.ReferenceObject, item.ReferenceType))
        //    {
        //        DebugHandler.LogDebugMessage("Changing " + field.Name + " with type " + item.ReferenceType, DebugBooleans.Generic);
        //        addFieldPropertyToData(field, item.ReferenceObject, data);
        //    }

        //    return data;
        //}
        //private static List<TableBase> getRelevantTablesForAddRemove(StackItem item)
        //{
        //    var relevantTables = new List<TableBase>();
        //    List<Type> objectTypes = new List<Type> { item.TargetType, item.ReferenceType };
        //    List<Type> objectTypesWithTarget = new List<Type> { item.TargetType, item.ReferenceType, item.TargetObject.GetType() };
        //    foreach (TableBase table in AllTables.Tables)
        //    {
        //        var tableInfo = new TableInfo(table);

        //        bool allTypesMatch = sharesAllTypes(objectTypes, tableInfo.Types);
        //        bool allTypesAndTargetObjectTypeMatch = sharesAllTypes(objectTypesWithTarget, tableInfo.Types);
        //        bool tableHasOnlyType = hasOnlyType(item.TargetObject.GetType(), tableInfo.Types);
        //        bool baseAndObjectMatch = false;
        //        bool shouldIncludeCatalog = isCatalogEdit(objectTypes, tableInfo.IsCatalogTable);

        //        if ((allTypesMatch || tableHasOnlyType || baseAndObjectMatch || allTypesAndTargetObjectTypeMatch) && (shouldIncludeCatalog))
        //        {
        //            relevantTables.Add(table);
        //        }
        //    }

        //    return relevantTables;

        //}
        //private static List<TableBase> getRelevantTablesForAddRemoveRelationship(StackItem item)
        //{
        //    var relevantTables = new List<TableBase>();
        //    List<Type> objectTypes = new List<Type> { item.TargetType, item.ReferenceType };

        //    foreach (TableBase table in AllTables.Tables)
        //    {
        //        var tableInfo = new TableInfo(table);

        //        bool allTypesMatch = sharesAllTypes(objectTypes, tableInfo.Types);
        //        bool shouldIncludeCatalog = isCatalogEdit(objectTypes, tableInfo.IsCatalogTable);
        //        if (allTypesMatch && shouldIncludeCatalog)
        //        {
        //            relevantTables = new List<TableBase>();
        //            relevantTables.Add(table);
        //            return relevantTables;
        //        }
        //    }
        //    return relevantTables;
        //}
        //private static List<TableBase> getRelevantTablesToEdit(StackItem item)
        //{
        //    var relevantTables = new List<TableBase>();
        //    var objectTypes = new List<Type> { item.TargetType, item.ReferenceType };

        //    foreach (TableBase table in AllTables.Tables)
        //    {
        //        var tableInfo = new TableInfo(table);
        //        bool allTypesMatch = sharesAllTypesForEdit(objectTypes, tableInfo.Types);
        //        bool tableHasOnlyType = false;
        //        if (item.TargetObject != null)
        //        {
        //            tableHasOnlyType = hasOnlyType(objectTypes[0], tableInfo.Types);
        //        }

        //        bool shouldIncludeCatalog = isCatalogEdit(objectTypes, tableInfo.IsCatalogTable);

        //        if ((allTypesMatch || tableHasOnlyType) && (shouldIncludeCatalog))
        //        { relevantTables.Add(table); }
        //    }
        //    return relevantTables;
        //}

        //private static void addFieldPropertyToData(TableField field, object obj, Dictionary<string, string> data)
        //{
        //    var dataString = objectToDBString(field.Property.GetValue(obj, null));
        //    data.Add(field.Name, dataString);
        //}
        //private static string objectToDBString(Object inObject)
        //{
        //    string outstring = "";
        //    if (inObject is bool)
        //    {
        //        outstring = ((bool)inObject).ToInt().ToString();
        //    }
        //    else
        //    {
        //        outstring = inObject.ToString();
        //    }

        //    return outstring;
        //}
        //private static bool sharesAllTypes(List<Type> list1, List<Type> list2)
        //{
        //    var numMatch = 0;
        //    var uniqueList1 = getUniqueTypes(list1);
        //    var uniqueList2 = getUniqueTypes(list2);

        //    if ((list1.Count == 2 && list2.Count == 2) && (uniqueList1.Count == uniqueList2.Count))
        //    {
        //        if (list1[0] == list2[0] && list1[1] == list2[1])
        //        {
        //            return true;
        //        }
        //    }


        //    foreach (Type type in uniqueList1)
        //    {
        //        foreach (Type otherType in uniqueList2)
        //        {
        //            if (type == otherType)
        //            {
        //                numMatch++;
        //            }
        //        }
        //    }
        //    return ((numMatch == list1.Count) && (numMatch == list2.Count));
        //}
        //private static bool sharesAllTypesForEdit(List<Type> list1, List<Type> list2)
        //{
        //    var numMatch = 0;
        //    var uniqueList1 = getUniqueTypes(list1);
        //    var uniqueList2 = getUniqueTypes(list2);

        //    foreach (Type type in uniqueList1)
        //    {
        //        foreach (Type otherType in uniqueList2)
        //        {
        //            if (type == otherType)
        //            {
        //                numMatch++;
        //            }
        //        }
        //    }
        //    return ((numMatch == list1.Count) && (numMatch == list2.Count));

        //}
        //private static bool hasOnlyType(Type primaryType, List<Type> list2)
        //{
        //    bool doesShare = false;
        //    if (list2.Count == 1)
        //    {
        //        if (primaryType == list2[0])
        //        {
        //            doesShare = true;
        //        }
        //    }
        //    return doesShare;
        //}
        //private static bool hasBaseTypeAndType(List<Type> list1, List<Type> list2)
        //{
        //    var uniqueList1 = getUniqueTypes(list1);
        //    var uniqueList2 = getUniqueTypes(list2);
        //    Type list1Type1 = null;
        //    Type list1Type2 = null;
        //    Type list2Type1 = null;
        //    Type list2Type2 = null;

        //    if (uniqueList1.Count == 2)
        //    {
        //        list1Type1 = uniqueList1[0];
        //        list1Type2 = uniqueList1[1];
        //    }
        //    else
        //    {
        //        return false;
        //    }

        //    if (uniqueList2.Count == 2)
        //    {
        //        list2Type1 = uniqueList2[0];
        //        list2Type2 = uniqueList2[1];
        //    }
        //    else
        //    {
        //        return false;
        //    }


        //    if (((list1Type1.BaseType == list2Type1 && list1Type2 == list2Type2) || (list1Type1.BaseType == list2Type2 && list1Type2 == list2Type1)) ||
        //        ((list1Type2.BaseType == list2Type1 && list1Type1 == list2Type2) || (list1Type2.BaseType == list2Type2 && list1Type1 == list2Type1)))
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
        //private static bool isCatalogEdit(List<Type> list1, bool isCatalogTable)
        //{
        //    bool isEdit = false;
        //    bool isFromCatalogOrBid = (list1.Contains(typeof(TECCatalogs)) || list1.Contains(typeof(TECBid)));
        //    bool isEditingObject = (list1.Count == 2) && (list1[0] == list1[1]);
        //    if ((isFromCatalogOrBid && (isCatalogTable)) || (!isCatalogTable) || isEditingObject)
        //    {
        //        isEdit = true;
        //    }
        //    return isEdit;
        //}
        //private static bool isFieldType(TableField field, Object consideredObject, Type consideredType)
        //{
        //    if (field.Property == null)
        //        return false;
        //    else if (field.Property.ReflectedType == consideredType)
        //        return true;
        //    else if (field.Property.ReflectedType == consideredObject.GetType())
        //        return true;
        //    else
        //        return false;
        //}
        //private static bool fieldsIncludeQuantity(List<TableField> fields)
        //{
        //    var includes = false;
        //    foreach (TableField field in fields)
        //    {
        //        if (field.Property.Name == "Quantity" && field.Property.ReflectedType == typeof(HelperProperties))
        //        {
        //            includes = true;
        //        }
        //    }

        //    return includes;
        //}
        //private static List<Type> getUniqueTypes(List<Type> types)
        //{
        //    var outList = new List<Type>();
        //    foreach (Type type in types)
        //    {
        //        if (!outList.Contains(type))
        //        {
        //            outList.Add(type);
        //        }
        //    }
        //    return outList;
        //}
        //private static int getQuantityInParentCollection(object childObject, object parentObject, Type parentType = null)
        //{
        //    TECScope child;
        //    TECScope parent;

        //    child = (childObject as TECScope);
        //    parent = (parentObject as TECScope);

        //    int quantity = 0;
        //    var childCollection = UtilitiesMethods.GetChildCollection(childObject.GetType(), parentObject, parentType);

        //    foreach (TECScope item in (IList)childCollection)
        //    {
        //        if (item.Guid == child.Guid)
        //        {
        //            quantity++;
        //        }
        //    }
        //    return quantity;
        //}
        //private static bool isScope(Type type)
        //{
        //    if (type == typeof(Object))
        //    {
        //        return false;
        //    }
        //    else if (type == typeof(TECScope))
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return (isScope(type.BaseType));
        //    }
        //}
        
        //static private List<string> getAllTableFields(string tableName)
        //{
        //    string command = "select * from " + tableName + " limit 1";
        //    DataTable data = SQLiteDB.getDataFromCommand(command);
        //    List<string> tableFields = new List<string>();
        //    foreach (DataColumn col in data.Columns)
        //    {
        //        tableFields.Add(col.ColumnName);
        //    }
        //    return tableFields;
        //}
        //static private List<string> getPrimaryKeys(string tableName)
        //{
        //    string command = "PRAGMA table_info(" + tableName + ")";
        //    DataTable data = SQLiteDB.getDataFromCommand(command);
        //    List<string> primaryKeys = new List<string>();
        //    foreach (DataRow row in data.Rows)
        //    {
        //        if (row["pk"].ToString() != "0")
        //        {
        //            primaryKeys.Add(row["name"].ToString());
        //        }

        //    }
        //    return primaryKeys;
        //}

        //static private List<StackItem> cleanseStack(ObservableCollection<StackItem> saveStack)
        //{
        //    List<StackItem> outStack = new List<StackItem>();
        //    foreach (StackItem item in saveStack)
        //    {
        //        bool alreadyEdited = false;
        //        if (item.StackChange == StackChange.Edit)
        //        {
        //            foreach (StackItem outItem in outStack)
        //            {
        //                if (outItem.StackChange == StackChange.Edit)
        //                {
        //                    if (item.TargetType == item.ReferenceType &&
        //                        outItem.TargetType == outItem.ReferenceType &&
        //                        item.TargetObject == outItem.TargetObject)
        //                    {
        //                        alreadyEdited = true;
        //                    }
        //                }
        //            }
        //        }

        //        if (!alreadyEdited)
        //        {
        //            outStack.Add(item);
        //        }
        //    }

        //    return outStack;
        //}

        //static private void explain(string command)
        //{
        //    if (DebugBooleans.VerboseSQLite)
        //    {
        //        Console.WriteLine("");
        //        var explainer = "explain query plan " + command;
        //        var explainDT = SQLiteDB.getDataFromCommand(explainer);
        //        foreach (DataRow row in explainDT.Rows)
        //        {
        //            Console.WriteLine(row["detail"]);
        //        }
        //    }
        //}
        //static private string allFieldsInTableString(TableBase table)
        //{
        //    var tableInfo = new TableInfo(table);
        //    string command = "";
        //    for (int x = 0; x < tableInfo.Fields.Count; x++)
        //    {
        //        if (x != tableInfo.Fields.Count - 1)
        //        {
        //            command += tableInfo.Name + "." + tableInfo.Fields[x].Name + ", ";
        //        }
        //        else
        //        {
        //            command += tableInfo.Name + "." + tableInfo.Fields[x].Name;
        //        }
        //    }
        //    return command;
        //}


        /////<summary>
        /////Used to merge catalogs more completely. Pass a scope and all relations will be updated.
        /////</summary>
        //private static void editScopeChildrenRelations(TECScope scope)
        //{
        //    foreach (TECLabeled tag in scope.Tags)
        //    {
        //        editObject(new StackItem(StackChange.Edit, scope, tag, typeof(TECScope), typeof(TECLabeled)));
        //    }
        //    foreach (TECCost assCost in scope.AssociatedCosts)
        //    {
        //        editObject(new StackItem(StackChange.Edit, scope, assCost, typeof(TECScope), typeof(TECCost)));
        //    }
        //}
        //#endregion



        #region Remaining in this helper class
        public static List<string> TableNames(SQLiteDatabase db)
        {
            string command = "select name from sqlite_master where type = 'table' order by 1";
            DataTable tables = db.getDataFromCommand(command);
            List<string> tableNames = new List<string>();
            foreach (DataRow row in tables.Rows)
            {
                tableNames.Add(row["Name"].ToString());
            }
            return tableNames;
        }

        #endregion


    }
}
