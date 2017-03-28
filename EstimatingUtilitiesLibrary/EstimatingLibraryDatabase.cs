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

namespace EstimatingUtilitiesLibrary
{
    public static class EstimatingLibraryDatabase
    {
        //FMT is used by DateTime to convert back and forth between the DateTime type and string
        private const string DB_FMT = "O";
        //private const bool DEBUG = true;

        private const bool DEBUG_GENERIC = false;

        static private SQLiteDatabase SQLiteDB;

        #region Public Functions
        static public TECBid LoadDBToBid(string path, TECTemplates templates)
        {
            SQLiteDB = new SQLiteDatabase(path);
            //var watch = System.Diagnostics.Stopwatch.StartNew();
            checkAndUpdateDB(typeof(TECBid));
            //watch.Stop();
            //Console.WriteLine("checkAndUpdateDB: " + watch.ElapsedMilliseconds);
            TECBid bid = getBidInfo();

            //watch = System.Diagnostics.Stopwatch.StartNew();
            //Update catalogs from templates.
            if (templates.DeviceCatalog.Count > 0)
            {
                foreach (TECDevice device in templates.DeviceCatalog)
                { editObject(new StackItem(Change.Edit, bid, device)); }
            }

            if (templates.ManufacturerCatalog.Count > 0)
            {
                foreach (TECManufacturer manufacturer in templates.ManufacturerCatalog)
                { editObject(new StackItem(Change.Edit, bid, manufacturer)); }
            }

            if (templates.Tags.Count > 0)
            {
                foreach (TECTag tag in templates.Tags)
                { editObject(new StackItem(Change.Edit, bid, tag)); }
            }

            if(templates.ConnectionTypeCatalog.Count > 0)
            {
                foreach(TECConnectionType connectionType in templates.ConnectionTypeCatalog)
                { editObject(new StackItem(Change.Edit, bid, connectionType)); }
            }
            if (templates.ConduitTypeCatalog.Count > 0)
            {
                foreach (TECConduitType conduitType in templates.ConduitTypeCatalog)
                { editObject(new StackItem(Change.Edit, bid, conduitType)); }
            }
            if(templates.AssociatedCostsCatalog.Count > 0)
            {
                foreach(TECAssociatedCost cost in templates.AssociatedCostsCatalog)
                { editObject(new StackItem(Change.Edit, bid, cost)); }
            }
            //watch.Stop();
            //Console.WriteLine("updating from catalog: " + watch.ElapsedMilliseconds);

            //watch = System.Diagnostics.Stopwatch.StartNew();
            
            bid.Parameters = getBidParameters(bid);
            bid.Labor = getLaborConstsInBid(bid);
            bid.ScopeTree = getBidScopeBranches();
            bid.Systems = getAllSystemsInBid(bid);
            bid.ProposalScope = getAllProposalScope(bid.Systems);
            bid.DeviceCatalog = getAllDevices();
            bid.ManufacturerCatalog = getAllManufacturers();
            bid.Locations = getAllLocations();
            bid.Tags = getAllTags();
            bid.Notes = getNotes();
            bid.Exclusions = getExclusions();
            bid.Drawings = getDrawings();
            bid.Controllers = getControllers();
            bid.ConnectionTypes = getConnectionTypes();
            bid.ConduitTypes = getConduitTypes();
            bid.AssociatedCostsCatalog = getAssociatedCosts();
            bid.MiscWiring = getMiscWiring();
            bid.MiscCosts = getMiscCosts();
            bid.Panels = getPanels();
            bid.PanelTypeCatalog = getPanelTypes();
            bid.IOModuleCatalog = getIOModules();

            ModelLinkingHelper.LinkBid(bid);
            getUserAdjustments(bid);
            //watch.Stop();
            //Console.WriteLine("loading data: " + watch.ElapsedMilliseconds);
            //Breaks Visual Scope in a page
            //populatePageVisualConnections(bid.Drawings, bid.Connections);

            SQLiteDB.Connection.Close();

            GC.Collect();
            GC.WaitForPendingFinalizers();

            return bid;
        }
        static public TECTemplates LoadDBToTemplates(string path)
        {
            SQLiteDB = new SQLiteDatabase(path);
            checkAndUpdateDB(typeof(TECTemplates));

            TECTemplates templates = new TECTemplates();
            
            templates = getTemplatesInfo();
            templates.Labor = getLaborConstsInTemplates(templates);
            templates.SystemTemplates = getOrphanSystems();
            templates.EquipmentTemplates = getOrphanEquipment();
            templates.SubScopeTemplates = getOrphanSubScope();
            templates.DeviceCatalog = getAllDevices();
            templates.Tags = getAllTags();
            templates.ManufacturerCatalog = getAllManufacturers();
            templates.ControllerTemplates = getOrphanControllers();
            templates.ConnectionTypeCatalog = getConnectionTypes();
            templates.ConduitTypeCatalog = getConduitTypes();
            templates.AssociatedCostsCatalog = getAssociatedCosts();
            templates.MiscWiringTemplates = getMiscWiring();
            templates.MiscCostTemplates = getMiscCosts();
            templates.PanelTemplates = getOrphanPanels();
            templates.PanelTypeCatalog = getPanelTypes();
            templates.ControlledScopeTemplates = getControlledScope();
            templates.IOModuleCatalog = getIOModules();
            ModelLinkingHelper.LinkTemplates(templates);
            SQLiteDB.Connection.Close();
            return templates;
        }
        static public void SaveBidToNewDB(string path, TECBid bid)
        {
            //var watch = System.Diagnostics.Stopwatch.StartNew();
            SQLiteDB = new SQLiteDatabase(path);
            //watch.Stop();
            //Console.WriteLine("New connection: " + watch.ElapsedMilliseconds);
            //watch = System.Diagnostics.Stopwatch.StartNew();
            if (File.Exists(path))
            { SQLiteDB.overwriteFile(); }
            createAllBidTables();
            //watch.Stop();
            //Console.WriteLine("CreateAllBidTales: " + watch.ElapsedMilliseconds);
            //watch = System.Diagnostics.Stopwatch.StartNew();
            saveCompleteBid(bid);
            //watch.Stop();
            //Console.WriteLine("SaveCompleteBid: " + watch.ElapsedMilliseconds);
            //watch = System.Diagnostics.Stopwatch.StartNew();
            SQLiteDB.Connection.Close();
            //watch.Stop();
            //Console.WriteLine("Close connection: " + watch.ElapsedMilliseconds);
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
        static public void SaveTemplatesToNewDB(string path, TECTemplates templates)
        {
            SQLiteDB = new SQLiteDatabase(path);

            if (File.Exists(path))
            { SQLiteDB.overwriteFile(); }

            createAllTemplateTables();

            saveCompleteTemplate(templates);

            SQLiteDB.Connection.Close();
        }
        static public void UpdateBidToDB(string path, ChangeStack changeStack, bool doBackup = true)
        {
            if (doBackup) { createBackup(path); }

            string tempPath = Path.GetDirectoryName(path) + @"\" + Path.GetFileNameWithoutExtension(path) + String.Format("{0:ffff}", DateTime.Now) + ".tmp";

            File.Copy(path, tempPath);

            SQLiteDB = new SQLiteDatabase(tempPath);

            foreach (StackItem change in changeStack.SaveStack)
            {
                Change changeType = change.Change;
                object targetObject = change.TargetObject;
                object refObject = change.ReferenceObject;

                if (changeType == Change.Add)
                {
                    addObject(change);
                }
                else if (changeType == Change.Edit)
                {
                    editObject(change);
                }
                else if (changeType == Change.Remove)
                {
                    removeObject(change);
                }
                else if(changeType == Change.AddRelationship)
                {
                    addRelationship(change);
                }
                else if(changeType == Change.RemoveRelationship)
                {
                    removeRelationship(change);
                }
            }

            SQLiteDB.Connection.Close();

            GC.Collect();
            GC.WaitForPendingFinalizers();

            File.Copy(tempPath, path, true);

            File.Delete(tempPath);
        }
        static public void UpdateTemplatesToDB(string path, ChangeStack changeStack)
        {
            string tempPath = Path.GetDirectoryName(path) + @"\" + Path.GetFileNameWithoutExtension(path) + String.Format("{0:ffff}", DateTime.Now) + ".tmp";

            File.Copy(path, tempPath);

            SQLiteDB = new SQLiteDatabase(tempPath);

            foreach (StackItem change in changeStack.SaveStack)
            {
                Change changeType = change.Change;
                object targetObject = change.TargetObject;
                object refObject = change.ReferenceObject;

                if (changeType == Change.Add)
                {
                    addObject(change);
                }
                else if (changeType == Change.Edit)
                {
                    editObject(change);
                }
                else if (changeType == Change.Remove)
                {
                    removeObject(change);
                }
                else if (changeType == Change.AddRelationship)
                {
                    addRelationship(change);
                }
                else if (changeType == Change.RemoveRelationship)
                {
                    removeRelationship(change);
                }
            }

            SQLiteDB.Connection.Close();

            GC.Collect();
            GC.WaitForPendingFinalizers();

            File.Copy(tempPath, path, true);

            File.Delete(tempPath);
        }
        #endregion Public Functions
        
        #region Loading from DB Methods

        static private TECBid getBidInfo()
        {
            DataTable bidInfoDT = SQLiteDB.getDataFromTable(BidInfoTable.TableName);
            if (bidInfoDT.Rows.Count < 1)
            {
                DebugHandler.LogError("Bid info not found in database. Bid info and labor will be missing.");
                return new TECBid();
            }

            DataRow bidInfoRow = bidInfoDT.Rows[0];

            TECBid outBid = new TECBid(new Guid(bidInfoRow[BidInfoTable.BidID.Name].ToString()));
            outBid.Name = bidInfoRow[BidInfoTable.BidName.Name].ToString();
            outBid.BidNumber = bidInfoRow[BidInfoTable.BidNumber.Name].ToString();

            string dueDateString = bidInfoRow[BidInfoTable.DueDate.Name].ToString();
            outBid.DueDate = DateTime.ParseExact(dueDateString, DB_FMT, CultureInfo.InvariantCulture);

            outBid.Salesperson = bidInfoRow[BidInfoTable.Salesperson.Name].ToString();
            outBid.Estimator = bidInfoRow[BidInfoTable.Estimator.Name].ToString();

            return outBid;
        }
        static private void getUserAdjustments(TECBid bid)
        {
            DataTable adjDT = SQLiteDB.getDataFromTable(UserAdjustmentsTable.TableName);

            if (adjDT.Rows.Count < 1)
            {
                DebugHandler.LogError("UserAdjustments not found in database.");
                return;
            }

            DataRow adjRow = adjDT.Rows[0];

            bid.Labor.PMExtraHours = adjRow[UserAdjustmentsTable.PMExtraHours.Name].ToString().ToDouble();
            bid.Labor.ENGExtraHours = adjRow[UserAdjustmentsTable.ENGExtraHours.Name].ToString().ToDouble();
            bid.Labor.CommExtraHours = adjRow[UserAdjustmentsTable.CommExtraHours.Name].ToString().ToDouble();
            bid.Labor.SoftExtraHours = adjRow[UserAdjustmentsTable.SoftExtraHours.Name].ToString().ToDouble();
            bid.Labor.GraphExtraHours = adjRow[UserAdjustmentsTable.GraphExtraHours.Name].ToString().ToDouble();
        }
        static private TECLabor getLaborConstsInBid(TECBid bid)
        {
            string constsCommand = "select * from (" + LaborConstantsTable.TableName + " inner join ";
            constsCommand += BidLaborTable.TableName + " on ";
            constsCommand += "(TECLaborConst.LaborID = TECBidTECLabor.LaborID";
            constsCommand += " and " + BidLaborTable.BidID.Name + " = '";
            constsCommand += bid.Guid;
            constsCommand += "'))";

            DataTable laborDT = SQLiteDB.getDataFromCommand(constsCommand);

            if (laborDT.Rows.Count > 1)
            {
                DebugHandler.LogError("Multiple rows found in labor constants table. Using first found.");
            }
            else if (laborDT.Rows.Count < 1)
            {
                DebugHandler.LogError("Labor constants not found in database, using default values. Reload labor constants from loaded templates in the labor tab.");
                return new TECLabor();
            }

            DataRow laborRow = laborDT.Rows[0];
            Guid laborID = new Guid(laborRow[LaborConstantsTable.LaborID.Name].ToString());
            TECLabor labor = new TECLabor(laborID);

            labor.PMCoef = laborRow[LaborConstantsTable.PMCoef.Name].ToString().ToDouble(0);
            labor.PMRate = laborRow[LaborConstantsTable.PMRate.Name].ToString().ToDouble(0);

            labor.ENGCoef = laborRow[LaborConstantsTable.ENGCoef.Name].ToString().ToDouble(0);
            labor.ENGRate = laborRow[LaborConstantsTable.ENGRate.Name].ToString().ToDouble(0);

            labor.CommCoef = laborRow[LaborConstantsTable.CommCoef.Name].ToString().ToDouble(0);
            labor.CommRate = laborRow[LaborConstantsTable.CommRate.Name].ToString().ToDouble(0);

            labor.SoftCoef = laborRow[LaborConstantsTable.SoftCoef.Name].ToString().ToDouble(0);
            labor.SoftRate = laborRow[LaborConstantsTable.SoftRate.Name].ToString().ToDouble(0);

            labor.GraphCoef = laborRow[LaborConstantsTable.GraphCoef.Name].ToString().ToDouble(0);
            labor.GraphRate = laborRow[LaborConstantsTable.GraphRate.Name].ToString().ToDouble(0);


            string subConstsCommand = "select * from (" + SubcontractorConstantsTable.TableName + " inner join ";
            subConstsCommand += BidLaborTable.TableName + " on ";
            subConstsCommand += "(TECSubcontractorConst.LaborID = TECBidTECLabor.LaborID";
            subConstsCommand += " and " + BidLaborTable.BidID.Name + " = '";
            subConstsCommand += bid.Guid;
            subConstsCommand += "'))";

            DataTable subConstsDT = SQLiteDB.getDataFromCommand(subConstsCommand);

            if (subConstsDT.Rows.Count > 1)
            {
                DebugHandler.LogError("Multiple rows found in subcontractor constants table. Using first found.");
            }
            else if (subConstsDT.Rows.Count < 1)
            {
                DebugHandler.LogError("Subcontractor constants not found in database, using default values. Reload labor constants from loaded templates in the labor tab.");
                return labor;
            }

            DataRow subContractRow = subConstsDT.Rows[0];

            labor.ElectricalRate = subContractRow[SubcontractorConstantsTable.ElectricalRate.Name].ToString().ToDouble(0);
            labor.ElectricalNonUnionRate = subContractRow[SubcontractorConstantsTable.ElectricalNonUnionRate.Name].ToString().ToDouble(0);
            labor.ElectricalSuperRate = subContractRow[SubcontractorConstantsTable.ElectricalSuperRate.Name].ToString().ToDouble(0);
            labor.ElectricalSuperNonUnionRate = subContractRow[SubcontractorConstantsTable.ElectricalSuperNonUnionRate.Name].ToString().ToDouble(0);

            labor.ElectricalIsOnOvertime = subContractRow[SubcontractorConstantsTable.ElectricalIsOnOvertime.Name].ToString().ToInt(0).ToBool();
            labor.ElectricalIsUnion = subContractRow[SubcontractorConstantsTable.ElectricalIsUnion.Name].ToString().ToInt(0).ToBool();

            return labor;
        }

        static private TECLabor getLaborConstsInTemplates(TECTemplates templates)
        {
            DataTable laborDT = SQLiteDB.getDataFromTable(LaborConstantsTable.TableName);

            if (laborDT.Rows.Count > 1)
            {
                DebugHandler.LogError("Multiple rows found in labor constants table. Using first found.");
            }
            else if (laborDT.Rows.Count < 1)
            {
                DebugHandler.LogError("Labor constants not found in database, using default values. Reload labor constants from loaded templates in the labor tab.");
                return new TECLabor();
            }

            DataRow laborRow = laborDT.Rows[0];
            Guid laborID = new Guid(laborRow[LaborConstantsTable.LaborID.Name].ToString());
            TECLabor labor = new TECLabor(laborID);

            labor.PMCoef = laborRow[LaborConstantsTable.PMCoef.Name].ToString().ToDouble(0);
            labor.PMRate = laborRow[LaborConstantsTable.PMRate.Name].ToString().ToDouble(0);

            labor.ENGCoef = laborRow[LaborConstantsTable.ENGCoef.Name].ToString().ToDouble(0);
            labor.ENGRate = laborRow[LaborConstantsTable.ENGRate.Name].ToString().ToDouble(0);

            labor.CommCoef = laborRow[LaborConstantsTable.CommCoef.Name].ToString().ToDouble(0);
            labor.CommRate = laborRow[LaborConstantsTable.CommRate.Name].ToString().ToDouble(0);

            labor.SoftCoef = laborRow[LaborConstantsTable.SoftCoef.Name].ToString().ToDouble(0);
            labor.SoftRate = laborRow[LaborConstantsTable.SoftRate.Name].ToString().ToDouble(0);

            labor.GraphCoef = laborRow[LaborConstantsTable.GraphCoef.Name].ToString().ToDouble(0);
            labor.GraphRate = laborRow[LaborConstantsTable.GraphRate.Name].ToString().ToDouble(0);

            DataTable subConstsDT = SQLiteDB.getDataFromTable(SubcontractorConstantsTable.TableName);

            if (subConstsDT.Rows.Count > 1)
            {
                DebugHandler.LogError("Multiple rows found in subcontractor constants table. Using first found.");
            }
            else if (subConstsDT.Rows.Count < 1)
            {
                DebugHandler.LogError("Subcontractor constants not found in database, using default values. Reload labor constants from loaded templates in the labor tab.");
                return labor;
            }

            DataRow subContractRow = subConstsDT.Rows[0];

            labor.ElectricalRate = subContractRow[SubcontractorConstantsTable.ElectricalRate.Name].ToString().ToDouble(0);
            labor.ElectricalNonUnionRate = subContractRow[SubcontractorConstantsTable.ElectricalNonUnionRate.Name].ToString().ToDouble(0);
            labor.ElectricalSuperRate = subContractRow[SubcontractorConstantsTable.ElectricalSuperRate.Name].ToString().ToDouble(0);
            labor.ElectricalSuperNonUnionRate = subContractRow[SubcontractorConstantsTable.ElectricalSuperNonUnionRate.Name].ToString().ToDouble(0);

            labor.ElectricalIsOnOvertime = subContractRow[SubcontractorConstantsTable.ElectricalIsOnOvertime.Name].ToString().ToInt(0).ToBool();
            labor.ElectricalIsUnion = subContractRow[SubcontractorConstantsTable.ElectricalIsUnion.Name].ToString().ToInt(0).ToBool();

            return labor;
        }
        
        static private TECTemplates getTemplatesInfo()
        {
            DataTable templateInfoDT = SQLiteDB.getDataFromTable(TemplatesInfoTable.TableName);

            if (templateInfoDT.Rows.Count < 1)
            {
                DebugHandler.LogError("Template info not found in database.");
                return new TECTemplates();
            }
            DataRow templateInfoRow = templateInfoDT.Rows[0];

            Guid infoGuid = new Guid(templateInfoRow[TemplatesInfoTable.TemplateID.Name].ToString());

            return new TECTemplates(infoGuid);
        }
        static private ObservableCollection<TECScopeBranch> getBidScopeBranches()
        {
            ObservableCollection<TECScopeBranch> mainBranches = new ObservableCollection<TECScopeBranch>();

            string command = "select * from " + ScopeBranchTable.TableName;
            command += " where " + ScopeBranchTable.ScopeBranchID.Name;
            command += " in (select " + ScopeBranchTable.ScopeBranchID.Name;
            command += " from "+BidScopeBranchTable.TableName+ " where " + BidScopeBranchTable.ScopeBranchID.Name + " not in ";
            command += "(select " + ScopeBranchHierarchyTable.ChildID.Name + " from " + ScopeBranchHierarchyTable.TableName + "))";

            DataTable mainBranchDT = SQLiteDB.getDataFromCommand(command);

            foreach (DataRow row in mainBranchDT.Rows)
            {
                mainBranches.Add(getScopeBranchFromRow(row));
            }

            return mainBranches;
        }
        static private ObservableCollection<TECScopeBranch> getProposalScopeBranches(Guid propScopeID)
        {
            ObservableCollection<TECScopeBranch> scopeBranches = new ObservableCollection<TECScopeBranch>();
            string command = "select * from " + ScopeBranchTable.TableName;
            command += " where " + ScopeBranchTable.ScopeBranchID.Name + " in ";
            command += "(select " + ProposalScopeScopeBranchTable.ScopeBranchID.Name;
            command += " from " + ProposalScopeScopeBranchTable.TableName;
            command += " where " + ProposalScopeScopeBranchTable.ProposalScopeID.Name + " = '" + propScopeID + "')";
            
            DataTable scopeBranchDT = SQLiteDB.getDataFromCommand(command);
            foreach (DataRow row in scopeBranchDT.Rows)
            {
                scopeBranches.Add(getScopeBranchFromRow(row));
            }

            return scopeBranches;
        }
        static private ObservableCollection<TECScopeBranch> getChildBranchesInBranch(Guid parentID)
        {
            ObservableCollection<TECScopeBranch> childBranches = new ObservableCollection<TECScopeBranch>();

            string command = "select * from " + ScopeBranchTable.TableName;
            command += " where "+ ScopeBranchTable.ScopeBranchID.Name + " in ";
            command += "(select "+ ScopeBranchHierarchyTable.ChildID.Name + " from " + ScopeBranchHierarchyTable.TableName;
            command += " where " + ScopeBranchHierarchyTable.ParentID.Name + " = '";
            command += parentID;
            command += "')";

            DataTable childBranchDT = SQLiteDB.getDataFromCommand(command);
            foreach (DataRow row in childBranchDT.Rows)
            {
                childBranches.Add(getScopeBranchFromRow(row));
            }

            return childBranches;
        }
        static private ObservableCollection<TECSystem> getAllSystems()
        {
            ObservableCollection<TECSystem> systems = new ObservableCollection<TECSystem>();
            string command = "select * from ";
            command += SystemTable.TableName;
            DataTable systemsDT = SQLiteDB.getDataFromCommand(command);
            foreach (DataRow row in systemsDT.Rows)
            {
                var system = getSystemFromRow(row);
                systems.Add(system);
            }
            return systems;
        }
        static private ObservableCollection<TECSystem> getAllSystemsInBid(TECBid bid)
        {
            ObservableCollection<TECSystem> systems = new ObservableCollection<TECSystem>();

            string command = "select * from ("
                + SystemTable.TableName
                + " inner join "
                + BidSystemTable.TableName
                + " on ("
                + SystemTable.TableName + "." + SystemTable.SystemID.Name 
                + " = " 
                + BidSystemTable.TableName + "." + BidSystemTable.SystemID.Name
                + ")) order by " 
                + BidSystemTable.Index.Name;

            DataTable systemsDT = SQLiteDB.getDataFromCommand(command);
            if (systemsDT.Rows.Count < 1)
            {
                command = "select * from " + SystemTable.TableName;
                systemsDT = SQLiteDB.getDataFromCommand(command);
            }
            foreach (DataRow row in systemsDT.Rows)
            { systems.Add(getSystemFromRow(row)); }
            return systems;
        }
        static private ObservableCollection<TECSystem> getOrphanSystems()
        {
            //Returns the systems that are not in the ControlledScopeSystem table.
            ObservableCollection<TECSystem> systems = new ObservableCollection<TECSystem>();

            string command = "select * from " + SystemTable.TableName;
            command += " where " + SystemTable.SystemID.Name + " not in ";
            command += "(select " + ControlledScopeSystemTable.SystemID.Name;
            command += " from " + ControlledScopeSystemTable.TableName + ")";

            DataTable systemsDT = SQLiteDB.getDataFromCommand(command);
            foreach (DataRow row in systemsDT.Rows)
            {
                systems.Add(getSystemFromRow(row));
            }

            return systems;
        }

        static private ObservableCollection<TECEquipment> getOrphanEquipment()
        {
            ObservableCollection<TECEquipment> equipment = new ObservableCollection<TECEquipment>();

            string command = "select * from " + EquipmentTable.TableName;
            command += " where "+EquipmentTable.EquipmentID.Name+" not in ";
            command += "(select " + SystemEquipmentTable.EquipmentID.Name;
            command += " from "+ SystemEquipmentTable.TableName+ ")";

            DataTable equipmentDT = SQLiteDB.getDataFromCommand(command);
            foreach (DataRow row in equipmentDT.Rows)
            { equipment.Add(getEquipmentFromRow(row)); }

            return equipment;
        }
        static private ObservableCollection<TECSubScope> getOrphanSubScope()
        {
            ObservableCollection<TECSubScope> subScope = new ObservableCollection<TECSubScope>();
            string command = "select * from " + SubScopeTable.TableName ;
            command += " where "+ SubScopeTable.SubScopeID.Name+ " not in ";
            command += "(select " + EquipmentSubScopeTable.SubScopeID.Name + " from " + EquipmentSubScopeTable.TableName + ")";
            DataTable subScopeDT = SQLiteDB.getDataFromCommand(command);
            foreach (DataRow row in subScopeDT.Rows)
            { subScope.Add(getSubScopeFromRow(row)); }
            return subScope;
        }
        static private ObservableCollection<TECDevice> getAllDevices()
        {
            ObservableCollection<TECDevice> devices = new ObservableCollection<TECDevice>();
            DataTable devicesDT = SQLiteDB.getDataFromTable(DeviceTable.TableName);

            foreach (DataRow row in devicesDT.Rows)
            { devices.Add(getDeviceFromRow(row)); }
            return devices;
        }
        static private ObservableCollection<TECManufacturer> getAllManufacturers()
        {
            ObservableCollection<TECManufacturer> manufacturers = new ObservableCollection<TECManufacturer>();
            DataTable manufacturersDT = SQLiteDB.getDataFromTable(ManufacturerTable.TableName);
            foreach (DataRow row in manufacturersDT.Rows)
            { manufacturers.Add(getManufacturerFromRow(row)); }
            return manufacturers;
        }
        static private ObservableCollection<TECLocation> getAllLocations()
        {
            ObservableCollection<TECLocation> locations = new ObservableCollection<TECLocation>();
            DataTable locationsDT = SQLiteDB.getDataFromTable(LocationTable.TableName);
            foreach (DataRow row in locationsDT.Rows)
            { locations.Add(getLocationFromRow(row)); }
            return locations;
        }
        static private ObservableCollection<TECConduitType> getConduitTypes()
        {
            ObservableCollection<TECConduitType> conduitTypes = new ObservableCollection<TECConduitType>();
            DataTable conduitTypesDT = SQLiteDB.getDataFromTable(ConduitTypeTable.TableName);
            foreach(DataRow row in conduitTypesDT.Rows)
            { conduitTypes.Add(getConduitTypeFromRow(row)); }
            return conduitTypes;
        }
        static private ObservableCollection<TECAssociatedCost> getAssociatedCosts()
        {
            ObservableCollection<TECAssociatedCost> associatedCosts = new ObservableCollection<TECAssociatedCost>();
            DataTable associatedCostsDT = SQLiteDB.getDataFromTable(AssociatedCostTable.TableName);
            foreach (DataRow row in associatedCostsDT.Rows)
            {  associatedCosts.Add(getAssociatedCostFromRow(row)); }
            return associatedCosts;
        }
        static private ObservableCollection<TECEquipment> getEquipmentInSystem(Guid systemID)
        {
            ObservableCollection<TECEquipment> equipment = new ObservableCollection<TECEquipment>();

            string command = "select * from ("+EquipmentTable.TableName+" inner join ";
            command += SystemEquipmentTable.TableName + " on ";
            command += "(TECEquipment.EquipmentID = TECSystemTECEquipment.EquipmentID";
            command += " and "+SystemEquipmentTable.SystemID.Name+" = '";
            command += systemID;
            command += "')) order by " + SystemEquipmentTable.ScopeIndex.Name;

            DataTable equipmentDT = SQLiteDB.getDataFromCommand(command);
            foreach (DataRow row in equipmentDT.Rows)
            { equipment.Add(getEquipmentFromRow(row)); }
            return equipment;
        }
        static private ObservableCollection<TECSubScope> getSubScopeInEquipment(Guid equipmentID)
        {
            ObservableCollection<TECSubScope> subScope = new ObservableCollection<TECSubScope>();
            string command = "select * from (TECSubScope inner join " + EquipmentSubScopeTable.TableName + " on ";
            command += "(TECSubScope.SubScopeID = TECEquipmentTECSubScope.SubScopeID and ";
            command += EquipmentSubScopeTable.EquipmentID.Name + "= '" +equipmentID;
            command += "')) order by "+EquipmentSubScopeTable.ScopeIndex.Name+"";

            DataTable subScopeDT = SQLiteDB.getDataFromCommand(command);
            foreach (DataRow row in subScopeDT.Rows)
            { subScope.Add(getSubScopeFromRow(row)); }
            return subScope;
        }
        static private ObservableCollection<TECDevice> getDevicesInSubScope(Guid subScopeID)
        {
            ObservableCollection<TECDevice> devices = new ObservableCollection<TECDevice>();
            string command = "select * from (" + DeviceTable.TableName +" inner join "+ SubScopeDeviceTable.TableName + " on ";
            command += "(TECDevice.DeviceID = TECSubScopeTECDevice.DeviceID and ";
            command += SubScopeDeviceTable.SubScopeID.Name + " = '" + subScopeID;
            command += "')) order by " + SubScopeDeviceTable.ScopeIndex.Name;

            DataTable devicesDT = SQLiteDB.getDataFromCommand(command);
            foreach (DataRow row in devicesDT.Rows)
            {
                var deviceToAdd = getDeviceFromRow(row);
                string quantityCommand = "select "+SubScopeDeviceTable.Quantity.Name+" from "+SubScopeDeviceTable.TableName+" where "+SubScopeDeviceTable.SubScopeID.Name+" = '";
                quantityCommand += (subScopeID + "' and "+SubScopeDeviceTable.DeviceID.Name+" = '" + deviceToAdd.Guid + "'");
                DataTable quantityDT = SQLiteDB.getDataFromCommand(quantityCommand);
                int quantity = quantityDT.Rows[0][0].ToString().ToInt();
                for (int x = 0; x < quantity; x++)
                { devices.Add(deviceToAdd); }
            }

            return devices;
        }
        static private ObservableCollection<TECPoint> getPointsInSubScope(Guid subScopeID)
        {
            ObservableCollection<TECPoint> points = new ObservableCollection<TECPoint>();

            string command = "select * from (" + PointTable.TableName + " inner join "+SubScopePointTable.TableName+" on ";
            command += "(TECPoint.PointID = TECSubScopeTECPoint.PointID and ";
            command += SubScopePointTable.SubScopeID.Name+" = '" +subScopeID;
            command += "')) order by " + SubScopePointTable.ScopeIndex.Name;

            DataTable pointsDT = SQLiteDB.getDataFromCommand(command); 
            foreach (DataRow row in pointsDT.Rows)
            { points.Add(getPointFromRow(row)); }

            return points;
        }
        static private TECManufacturer getManufacturerInDevice(Guid deviceID)
        {
            string command = "select * from "+ManufacturerTable.TableName+ " where " + ManufacturerTable.ManufacturerID.Name + " in ";
            command += "(select " + DeviceManufacturerTable.ManufacturerID.Name + " from " + DeviceManufacturerTable.TableName;
            command += " where " + DeviceManufacturerTable.DeviceID.Name + " = '";
            command += deviceID;
            command += "')";

            DataTable manTable = SQLiteDB.getDataFromCommand(command); 
            if (manTable.Rows.Count > 0)
            { return getManufacturerFromRow(manTable.Rows[0]); }
            else
            { return null; }
        }
        static private TECManufacturer getManufacturerInIOModule(Guid guid)
        {
            string command = "select * from " + ManufacturerTable.TableName + " where " + ManufacturerTable.ManufacturerID.Name + " in ";
            command += "(select " + IOModuleManufacturerTable.ManufacturerID.Name + " from " + IOModuleManufacturerTable.TableName;
            command += " where " + IOModuleManufacturerTable.IOModuleID.Name + " = '";
            command += guid;
            command += "')";

            DataTable manTable = SQLiteDB.getDataFromCommand(command);
            if (manTable.Rows.Count > 0)
            { return getManufacturerFromRow(manTable.Rows[0]); }
            else
            { return null; }
        }
        static private TECConnectionType getConnectionTypeInDevice(Guid deviceID)
        {
            string command = "select * from "+ConnectionTypeTable.TableName+" where "+ ConnectionTypeTable .ConnectionTypeID.Name+ " in ";
            command += "(select "+DeviceConnectionTypeTable.TypeID.Name+" from "+ DeviceConnectionTypeTable.TableName+ " where ";
            command += DeviceConnectionTypeTable.DeviceID.Name + " = '" + deviceID;
            command += "')";

            DataTable connectionTypeTable = SQLiteDB.getDataFromCommand(command);
            if (connectionTypeTable.Rows.Count > 0)
            { return (getConnectionTypeFromRow(connectionTypeTable.Rows[0])); }
            else
            { return null; }
        }
        static private TECConduitType getConduitTypeInConnection(Guid connectionID)
        {
            string command = "select * from " + ConduitTypeTable.TableName + " where " + ConduitTypeTable.ConduitTypeID.Name + " in ";
            command += "(select " + ConnectionConduitTypeTable.TypeID.Name + " from " + ConnectionConduitTypeTable.TableName + " where ";
            command += ConnectionConduitTypeTable.ConnectionID.Name + " = '" + connectionID;
            command += "')";

            DataTable conduitTypeTable = SQLiteDB.getDataFromCommand(command);
            if (conduitTypeTable.Rows.Count > 0)
            { return (getConduitTypeFromRow(conduitTypeTable.Rows[0])); }
            else
            { return null; }
        }
        static private ObservableCollection<TECAssociatedCost> getAssociatedCostsInScope(Guid scopeID)
        {
            string command = "select * from " + AssociatedCostTable.TableName + " where " + AssociatedCostTable.AssociatedCostID.Name + " in ";
            command += "(select " + AssociatedCostTable.AssociatedCostID.Name +" from " + ScopeAssociatedCostTable.TableName+  " where ";
            command += ScopeAssociatedCostTable.ScopeID.Name + " = '" + scopeID;
            command += "')";

            DataTable DT = SQLiteDB.getDataFromCommand(command);
            var associatedCosts = new ObservableCollection<TECAssociatedCost>();
            foreach(DataRow row in DT.Rows)
            { associatedCosts.Add(getAssociatedCostFromRow(row)); }
            return associatedCosts;
        }
        static private ObservableCollection<TECNote> getNotes()
        {
            ObservableCollection<TECNote> notes = new ObservableCollection<TECNote>();
            DataTable notesDT = SQLiteDB.getDataFromTable(NoteTable.TableName);
            foreach (DataRow row in notesDT.Rows)
            { notes.Add(getNoteFromRow(row)); }
            return notes;
        }
        static private ObservableCollection<TECExclusion> getExclusions()
        {
            ObservableCollection<TECExclusion> exclusions = new ObservableCollection<TECExclusion>();
            DataTable exclusionsDT = SQLiteDB.getDataFromTable(ExclusionTable.TableName);
            foreach (DataRow row in exclusionsDT.Rows)
            { exclusions.Add(getExclusionFromRow(row)); }
            return exclusions;
        }
        static private ObservableCollection<TECTag> getAllTags()
        {
            ObservableCollection<TECTag> tags = new ObservableCollection<TECTag>();
            DataTable tagsDT = SQLiteDB.getDataFromTable(TagTable.TableName);
            foreach (DataRow row in tagsDT.Rows)
            { tags.Add(getTagFromRow(row)); }
            return tags;
        }
        static private ObservableCollection<TECTag> getTagsInScope(Guid scopeID)
        {
            ObservableCollection<TECTag> tags = new ObservableCollection<TECTag>();
            string command = "select * from "+TagTable.TableName+" where "+TagTable.TagID.Name+" in ";
            command += "(select "+ScopeTagTable.TagID.Name+" from "+ScopeTagTable.TableName+" where ";
            command += ScopeTagTable.ScopeID.Name + " = '"+scopeID;
            command += "')";
            DataTable tagsDT = SQLiteDB.getDataFromCommand(command);
            foreach (DataRow row in tagsDT.Rows)
            { tags.Add(getTagFromRow(row)); }
            return tags;
        }
        static private ObservableCollection<TECDrawing> getDrawings()
        {
            ObservableCollection<TECDrawing> drawings = new ObservableCollection<TECDrawing>();
            
            DataTable ghostDrawingsDT = SQLiteDB.getDataFromTable(DrawingTable.TableName);
            foreach (DataRow row in ghostDrawingsDT.Rows)
            { drawings.Add(getDrawingFromRow(row)); }
            
            return drawings;
        }
        static private ObservableCollection<TECPage> getPagesInDrawing(Guid DrawingID)
        {
            ObservableCollection<TECPage> pages = new ObservableCollection<TECPage>();
            string command = "select * from "+PageTable.TableName+" where "+PageTable.PageID.Name+" in ";
            command += "(select "+DrawingPageTable.PageID.Name+" from "+DrawingPageTable.TableName+" where ";
            command += DrawingPageTable.DrawingID.Name + " = '"+DrawingID;
            command += "') order by " + PageTable.PageNum.Name;
            
            DataTable pagesDT = SQLiteDB.getDataFromCommand(command);
            foreach (DataRow row in pagesDT.Rows)
            { pages.Add(getPageFromRow(row)); }
            
            return pages;
        }
        static private ObservableCollection<TECVisualScope> getVisualScopeInPage(Guid PageID)
        {
            ObservableCollection<TECVisualScope> vs = new ObservableCollection<TECVisualScope>();
            string command = "select * from "+VisualScopeTable.TableName+" where "+VisualScopeTable.VisualScopeID.Name+" in ";
            command += "(select "+PageVisualScopeTable.VisualScopeID.Name+" from "+PageVisualScopeTable.TableName+" where ";
            command += PageVisualScopeTable.PageID.Name + " = '"+PageID;
            command += "')";
            
            DataTable vsDT = SQLiteDB.getDataFromCommand(command); 
            foreach (DataRow row in vsDT.Rows)
            { vs.Add(getVisualScopeFromRow(row)); }

            return vs;
        }
        static private TECLocation getLocationInScope(Guid ScopeID)
        {
            var tables = getAllTableNames();
            if (tables.Contains(LocationTable.TableName))
            {
                string command = "select * from " + LocationTable.TableName + " where " + LocationTable.LocationID.Name + " in ";
                command += "(select " + LocationScopeTable.LocationID.Name + " from " + LocationScopeTable.TableName + " where ";
                command += LocationScopeTable.ScopeID.Name + " = '" + ScopeID;
                command += "')";
                DataTable locationDT = SQLiteDB.getDataFromCommand(command);
                if (locationDT.Rows.Count > 0)
                { return getLocationFromRow(locationDT.Rows[0]); }
                else
                { return null; }
            } else
            { return null; }
        }
        static private ObservableCollection<TECController> getControllers()
        {
            ObservableCollection<TECController> controllers = new ObservableCollection<TECController>();
            
            DataTable controllersDT = SQLiteDB.getDataFromTable(ControllerTable.TableName);
            foreach (DataRow row in controllersDT.Rows)
            { controllers.Add(getControllerFromRow(row)); }

            return controllers;
        }
        static private ObservableCollection<TECController> getOrphanControllers()
        {
            //Returns the controllers that are not in the ControlledScopeController table.
            ObservableCollection<TECController> controllers = new ObservableCollection<TECController>();

            string command = "select * from " + ControllerTable.TableName;
            command += " where " + ControllerTable.ControllerID.Name + " not in ";
            command += "(select " + ControlledScopeControllerTable.ControllerID.Name;
            command += " from " + ControlledScopeControllerTable.TableName + ")";

            DataTable controllersDT = SQLiteDB.getDataFromCommand(command);
            foreach (DataRow row in controllersDT.Rows)
            {
                controllers.Add(getControllerFromRow(row));
            }

            return controllers;
        }
        static private ObservableCollection<TECConnectionType> getConnectionTypes()
        {
            ObservableCollection<TECConnectionType> connectionTypes = new ObservableCollection<TECConnectionType>();
            
                DataTable connectionTypesDT = SQLiteDB.getDataFromTable(ConnectionTypeTable.TableName);
                foreach (DataRow row in connectionTypesDT.Rows)
                { connectionTypes.Add(getConnectionTypeFromRow(row)); }
            return connectionTypes;
        }
        static private ObservableCollection<TECIO> getIOInController(Guid controllerID)
        {
            ObservableCollection<TECIO> outIO = new ObservableCollection<TECIO>();
            string command = "select * from " + IOTable.TableName + " where " + IOTable.IOID.Name + " in ";
            command += "(select " + ControllerIOTable.IOID.Name + " from " + ControllerIOTable.TableName + " where ";
            command += ControllerIOTable.ControllerID.Name + " = '" + controllerID;
            command += "')";

            DataTable typeDT = SQLiteDB.getDataFromCommand(command);
                foreach (DataRow row in typeDT.Rows)
                {  outIO.Add(getIOFromRow(row)); }
            return outIO;
        }
        static private ObservableCollection<TECConnection> getConnections()
        {
            ObservableCollection<TECConnection> connections = new ObservableCollection<TECConnection>(); 
            DataTable connectionDT = SQLiteDB.getDataFromTable(SubScopeConnectionTable.TableName); 
            foreach (DataRow row in connectionDT.Rows)
            { connections.Add(getSubScopeConnectionFromRow(row)); }
            connectionDT = SQLiteDB.getDataFromTable(NetworkConnectionTable.TableName);
            foreach (DataRow row in connectionDT.Rows)
            { connections.Add(getNetworkConnectionFromRow(row)); }
            return connections;
        }
        static private ObservableCollection<TECProposalScope> getAllProposalScope(ObservableCollection<TECSystem> systems)
        {
            ObservableCollection<TECProposalScope> propScope = new ObservableCollection<TECProposalScope>();
            foreach (TECSystem sys in systems)
            {
                TECProposalScope propSysToAdd = getProposalScopeFromScope(sys);
                foreach (TECEquipment equip in sys.Equipment)
                {
                    TECProposalScope propEquipToAdd = getProposalScopeFromScope(equip);
                    foreach (TECSubScope ss in equip.SubScope)
                    {
                        propEquipToAdd.Children.Add(getProposalScopeFromScope(ss));
                    }
                    propSysToAdd.Children.Add(propEquipToAdd);
                }
                propScope.Add(propSysToAdd);
            }
            return propScope;
        }
        static private TECProposalScope getProposalScopeFromScope(TECScope scope)
        {
            bool isProposed;
            ObservableCollection<TECScopeBranch> notes = getProposalScopeBranches(scope.Guid);
            string command = "select " + ProposalScopeTable.IsProposed.Name + " from " + ProposalScopeTable.TableName;
            command += " where " + ProposalScopeTable.ProposalScopeID.Name + " = '" + scope.Guid + "'";
           
                DataTable isProposedDT = SQLiteDB.getDataFromCommand(command); 
                if (isProposedDT.Rows.Count > 0)
                { isProposed = isProposedDT.Rows[0][ProposalScopeTable.IsProposed.Name].ToString().ToInt().ToBool(); }
                else
                { isProposed = false; }
            return new TECProposalScope(scope, isProposed, notes);
        }
        static private TECScope getScopeGuidInVisualScope(Guid guid)
        {
            string command = "select " + VisualScopeScopeTable.ScopeID.Name + " from " + VisualScopeScopeTable.TableName;
            command += " where " + VisualScopeScopeTable.VisualScopeID.Name + " = '";
            command += guid + "'";
            
            DataTable manTable = SQLiteDB.getDataFromCommand(command);
            if (manTable.Rows.Count > 0)
            {
                var row = manTable.Rows[0];
                return new TECSystem(new Guid(row[VisualScopeScopeTable.ScopeID.Name].ToString()));
            }
            else
            { return null; }
        }
        static private ObservableCollection<TECIOModule> getIOModules()
        {
            ObservableCollection<TECIOModule> ioModules = new ObservableCollection<TECIOModule>();
            DataTable ioModuleDT = SQLiteDB.getDataFromTable(IOModuleTable.TableName);

            foreach (DataRow row in ioModuleDT.Rows)
            { ioModules.Add(getIOModuleFromRow(row)); }
            return ioModules;
        }

        static private TECController getControllerInConnection(Guid connectionID)
        {
            var tables = getAllTableNames();
            if (tables.Contains(SubScopeConnectionTable.TableName) || tables.Contains(NetworkConnectionTable.TableName))
            {
                var outController = new TECController();
                string command = "select * from " + ControllerTable.TableName + " where " + ControllerTable.ControllerID.Name + " in ";
                command += "(select " + ControllerConnectionTable.ControllerID.Name + " from " + ControllerConnectionTable.TableName + " where ";
                command += ControllerConnectionTable.ConnectionID.Name + " = '" + connectionID;
                command += "')";
                DataTable connectionDT = SQLiteDB.getDataFromCommand(command);
                if( connectionDT.Rows.Count > 0){
                    outController = getControllerPlaceholderFromRow(connectionDT.Rows[0]);
                }
                return outController;
            }
            else
            { return  null; }
        }
        static private TECSubScope getSubScopeInSubScopeConnection(Guid connectionID)
        {
            TECSubScope outScope = null;

            string command = "select * from " + SubScopeTable.TableName + " where " + SubScopeTable.SubScopeID.Name + " in ";
            command += "(select " + SubScopeConnectionChildrenTable.ChildID.Name + " from " + SubScopeConnectionChildrenTable.TableName + " where ";
            command += SubScopeConnectionChildrenTable.ConnectionID.Name + " = '" + connectionID;
            command += "')";

            DataTable scopeDT = SQLiteDB.getDataFromCommand(command);
            if (scopeDT.Rows.Count > 0)
            {
                return getSubScopePlaceholderFromRow(scopeDT.Rows[0]);
            }
            
            return outScope;
        }
        static private ObservableCollection<TECController> getChildrenInNetworkConnection(Guid connectionID)
        {
            var outScope = new ObservableCollection<TECController>();
            
            string command = "select * from " + ControllerTable.TableName + " where " + ControllerTable.ControllerID.Name + " in ";
            command += "(select " + NetworkConnectionChildrenTable.ChildID.Name + " from " + NetworkConnectionChildrenTable.TableName + " where ";
            command += NetworkConnectionChildrenTable.ConnectionID.Name + " = '" + connectionID;
            command += "')";

            DataTable scopeDT = SQLiteDB.getDataFromCommand(command);
            foreach (DataRow row in scopeDT.Rows)
            { outScope.Add(getControllerFromRow(row)); }

            return outScope;
        }
        static private IOType getIOTypeInNetworkConnection(Guid connectionID)
        {
            IOType outIO = 0;
            string command = "select * from " + NetworkConnectionIOTypeTable.TableName + " where ";
            command += NetworkConnectionIOTypeTable.ConnectionID.Name + " = '" + connectionID + "'";

            DataTable typeDT = SQLiteDB.getDataFromCommand(command);
            if(typeDT.Rows.Count > 0)
            {
                outIO = TECIO.convertStringToType(typeDT.Rows[0][NetworkConnectionIOTypeTable.IOType.Name].ToString());
            }
            return outIO;
        }

        static private TECManufacturer getManufacturerInController(Guid controllerID)
        {
            string command = "select * from " + ManufacturerTable.TableName + " where " + ManufacturerTable.ManufacturerID.Name + " in ";
            command += "(select " + ControllerManufacturerTable.ManufacturerID.Name + " from " + ControllerManufacturerTable.TableName;
            command += " where " + ControllerManufacturerTable.ControllerID.Name + " = '";
            command += controllerID;
            command += "')";

            DataTable manTable = SQLiteDB.getDataFromCommand(command);
            if (manTable.Rows.Count > 0)
            { return getManufacturerFromRow(manTable.Rows[0]); }
            else
            { return null; }
        }
        static private TECBidParameters getBidParameters(TECBid bid)
        {
            string constsCommand = "select * from (" + BidParametersTable.TableName + " inner join ";
            constsCommand += BidBidParametersTable.TableName + " on ";
            constsCommand += "(TECBidTECParameters.ParametersID = TECBidTECParameters.ParametersID";
            constsCommand += " and " + BidBidParametersTable.BidID.Name + " = '";
            constsCommand += bid.Guid;
            constsCommand += "'))";

            DataTable DT = SQLiteDB.getDataFromCommand(constsCommand);

            if (DT.Rows.Count > 1)
            {
                DebugHandler.LogError("Multiple rows found in bid paramters table. Using first found.");
            }
            else if (DT.Rows.Count < 1)
            {
                DebugHandler.LogError("Bid paramters not found in database, using default values. Reload labor constants from loaded templates in the labor tab.");
                return new TECBidParameters();
            }
            return getBidParametersFromRow(DT.Rows[0]);
        }
        static private ObservableCollection<TECMiscCost> getMiscCosts()
        {
            ObservableCollection<TECMiscCost> costs = new ObservableCollection<TECMiscCost>();

            DataTable costsDT = SQLiteDB.getDataFromTable(MiscCostTable.TableName);
            foreach (DataRow row in costsDT.Rows)
            {
                costs.Add(getMiscCostFromRow(row));
            }

            return costs;
        }
        static private ObservableCollection<TECMiscWiring> getMiscWiring()
        {
            ObservableCollection<TECMiscWiring> wiring = new ObservableCollection<TECMiscWiring>();

            DataTable wiringDT = SQLiteDB.getDataFromTable(MiscWiringTable.TableName);
            foreach (DataRow row in wiringDT.Rows)
            {
                wiring.Add(getMiscWiringFromRow(row));
            }

            return wiring;
        }
        static private ObservableCollection<TECPanelType> getPanelTypes()
        {
            ObservableCollection<TECPanelType> panelTypes = new ObservableCollection<TECPanelType>();

            DataTable panelTypesDT = SQLiteDB.getDataFromTable(PanelTypeTable.TableName);
            foreach (DataRow row in panelTypesDT.Rows)
            {
                panelTypes.Add(getPanelTypeFromRow(row));
            }

            return panelTypes;
        }
        static private ObservableCollection<TECPanel> getPanels()
        {
            ObservableCollection<TECPanel> panels = new ObservableCollection<TECPanel>();

            DataTable panelTypesDT = SQLiteDB.getDataFromTable(PanelTable.TableName);
            foreach (DataRow row in panelTypesDT.Rows)
            {
                panels.Add(getPanelFromRow(row));
            }

            return panels;
        }
        static private ObservableCollection<TECPanel> getOrphanPanels()
        {
            //Returns the panels that are not in the ControlledScopePanel table.
            ObservableCollection<TECPanel> panels = new ObservableCollection<TECPanel>();

            string command = "select * from " + PanelTable.TableName;
            command += " where " + PanelTable.PanelID.Name + " not in ";
            command += "(select " + ControlledScopePanelTable.PanelID.Name;
            command += " from " + ControlledScopePanelTable.TableName + ")";

            DataTable panelsDT = SQLiteDB.getDataFromCommand(command);
            foreach (DataRow row in panelsDT.Rows)
            {
                panels.Add(getPanelFromRow(row));
            }

            return panels;
        }
        static private ObservableCollection<TECControlledScope> getControlledScope()
        {
            ObservableCollection<TECControlledScope> controlledScope = new ObservableCollection<TECControlledScope>();

            DataTable panelTypesDT = SQLiteDB.getDataFromTable(ControlledScopeTable.TableName);
            foreach (DataRow row in panelTypesDT.Rows)
            {
                controlledScope.Add(getControlledScopeFromRow(row));
            }

            return controlledScope;
        }
        
        static private TECPanelType getPanelTypeInPanel(Guid guid)
        {
            string command = "select * from " + PanelTypeTable.TableName + " where " + PanelTypeTable.PanelTypeID.Name + " in ";
            command += "(select " + PanelPanelTypeTable.PanelTypeID.Name + " from " + PanelPanelTypeTable.TableName;
            command += " where " + PanelPanelTypeTable.PanelID.Name + " = '";
            command += guid;
            command += "')";

            DataTable manTable = SQLiteDB.getDataFromCommand(command);
            if (manTable.Rows.Count > 0)
            { return getPanelTypeFromRow(manTable.Rows[0]); }
            else
            { return null; }
        }
        static private ObservableCollection<TECController> getControllersInPanel(Guid guid)
        {
            ObservableCollection<TECController> controllers = new ObservableCollection<TECController>();
            string command = "select * from " + ControllerTable.TableName + " where " + ControllerTable.ControllerID.Name + " in ";
            command += "(select " + PanelControllerTable.ControllerID.Name + " from " + PanelControllerTable.TableName + " where ";
            command += PanelControllerTable.PanelID.Name + " = '" + guid;
            command += "')";

            DataTable controllerDT = SQLiteDB.getDataFromCommand(command);
            foreach (DataRow row in controllerDT.Rows)
            { controllers.Add(getControllerFromRow(row)); }

            return controllers;
        }
        static private ObservableCollection<TECController> getControllersInControlledScope(Guid guid)
        {
            ObservableCollection<TECController> controllers = new ObservableCollection<TECController>();
            string command = "select * from " + ControllerTable.TableName + " where " + ControllerTable.ControllerID.Name + " in ";
            command += "(select " + ControlledScopeControllerTable.ControllerID.Name + " from " + ControlledScopeControllerTable.TableName + " where ";
            command += ControlledScopeControllerTable.ControlledScopeID.Name + " = '" + guid;
            command += "')";

            DataTable controllerDT = SQLiteDB.getDataFromCommand(command);
            foreach (DataRow row in controllerDT.Rows)
            { controllers.Add(getControllerFromRow(row)); }

            return controllers;
        }
        static private ObservableCollection<TECSystem> getSystemsInControlledScope(Guid guid)
        {
            ObservableCollection<TECSystem> systems = new ObservableCollection<TECSystem>();
            string command = "select * from " + SystemTable.TableName + " where " + SystemTable.SystemID.Name + " in ";
            command += "(select " + ControlledScopeSystemTable.SystemID.Name + " from " + ControlledScopeSystemTable.TableName + " where ";
            command += ControlledScopeSystemTable.ControlledScopeID.Name + " = '" + guid;
            command += "')";

            DataTable pagesDT = SQLiteDB.getDataFromCommand(command);
            foreach (DataRow row in pagesDT.Rows)
            { systems.Add(getSystemFromRow(row)); }

            return systems;
        }
        static private ObservableCollection<TECPanel> getPanelsInControlledScope(Guid guid)
        {
            ObservableCollection<TECPanel> panels = new ObservableCollection<TECPanel>();
            string command = "select * from " + PanelTable.TableName + " where " + PanelTable.PanelID.Name + " in ";
            command += "(select " + ControlledScopePanelTable.PanelID.Name + " from " + ControlledScopePanelTable.TableName + " where ";
            command += ControlledScopePanelTable.ControlledScopeID.Name + " = '" + guid;
            command += "')";

            DataTable dt = SQLiteDB.getDataFromCommand(command);
            foreach (DataRow row in dt.Rows)
            { panels.Add(getPanelFromRow(row)); }

            return panels;
        }
        static private ObservableCollection<TECConnection> getConnectionsInController(TECController controller)
        {
            var tables = getAllTableNames();
            ObservableCollection<TECConnection> outScope = new ObservableCollection<TECConnection>();
            string command;
            DataTable scopeDT;

            if (tables.Contains(NetworkConnectionTable.TableName))
            {
                command = "select * from " + NetworkConnectionTable.TableName + " where " + NetworkConnectionTable.ConnectionID.Name + " in ";
                command += "(select " + ControllerConnectionTable.ConnectionID.Name + " from " + ControllerConnectionTable.TableName + " where "; 
                command += ControllerConnectionTable.ControllerID.Name + " = '" + controller.Guid;
                command += "')";

                scopeDT = SQLiteDB.getDataFromCommand(command);
                foreach (DataRow row in scopeDT.Rows)
                {
                    var networkConnection = getNetworkConnectionFromRow(row);
                    networkConnection.ParentController = controller;
                    outScope.Add(networkConnection);
                }
            }

            command = "select * from " + SubScopeConnectionTable.TableName + " where " + SubScopeConnectionTable.ConnectionID.Name + " in ";
            command += "(select " + ControllerConnectionTable.ConnectionID.Name + " from " + ControllerConnectionTable.TableName + " where ";
            command += ControllerConnectionTable.ControllerID.Name + " = '" + controller.Guid;
            command += "')";

            scopeDT = SQLiteDB.getDataFromCommand(command);
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
            string command = "select * from " + IOModuleTable.TableName + " where " + IOModuleTable.IOModuleID.Name + " in ";
            command += "(select " + IOIOModuleTable.ModuleID.Name + " from " + IOIOModuleTable.TableName;
            command += " where " + IOIOModuleTable.IOID.Name + " = '";
            command += ioID;
            command += "')";

            DataTable moduleTable = SQLiteDB.getDataFromCommand(command);
            if (moduleTable.Rows.Count > 0)
            { return getIOModuleFromRow(moduleTable.Rows[0]); }
            else
            { return null; }
        }
        #endregion //Loading from DB Methods

        #region Populate Derived
        static private void populatePageVisualConnections(ObservableCollection<TECDrawing> drawings, ObservableCollection<TECConnection> connections)
        {
            ObservableCollection<TECConnection> connectionsToAdd = connections;
            foreach (TECDrawing drawing in drawings)
            {
                foreach (TECPage page in drawing.Pages)
                {
                    List<Tuple<TECSubScope, TECVisualScope>> vSubScope = GetSubScopeVisual(page.PageScope);
                    List<TECVisualScope> vControllers = new List<TECVisualScope>();

                    ObservableCollection<TECVisualConnection> vConnectionsToAdd = new ObservableCollection<TECVisualConnection>();

                    foreach (TECVisualScope vScope in page.PageScope)
                    {
                        if (vScope.Scope is TECController)
                        { vControllers.Add(vScope); }
                    }

                    foreach (Tuple<TECSubScope, TECVisualScope> item in vSubScope)
                    {
                        foreach (TECSubScopeConnection connection in connectionsToAdd)
                        {
                            foreach (TECVisualScope vController in vControllers)
                            {
                                //if ((connection.ParentController == vController.Scope) && (connection.SubScope == item.Item1))
                                //{
                                //    ObservableCollection<TECSubScopeConnection> childConnections = new ObservableCollection<TECSubScopeConnection>();
                                //    childConnections.Add(connection);
                                //    TECVisualConnection visConnection = new TECVisualConnection(vController, item.Item2);
                                //    vConnectionsToAdd.Add(visConnection);
                                //}
                            }
                        }
                    }
                    page.Connections = vConnectionsToAdd;
                }
            }
        }
        private static List<Tuple<TECSubScope, TECVisualScope>> GetSubScopeVisual(ObservableCollection<TECVisualScope> allVScope)
        {
            List<Tuple<TECSubScope, TECVisualScope>> outList = new List<Tuple<TECSubScope, TECVisualScope>>();
            ObservableCollection<TECVisualScope> vScopeToCheck = allVScope;
            var vScopeToRemove = new ObservableCollection<TECVisualScope>();

            List<TECSubScope> accountedFor = new List<TECSubScope>();
            foreach (TECVisualScope vScope in allVScope)
            {
                if (vScope.Scope is TECSubScope)
                {
                    var sub = vScope.Scope as TECSubScope;
                    accountedFor.Add(sub);
                    outList.Add(Tuple.Create<TECSubScope, TECVisualScope>(sub, vScope));
                    vScopeToRemove.Add(vScope);
                }
            }
            foreach (TECVisualScope item in vScopeToRemove)
            {
                if (vScopeToCheck.Contains(item))
                { vScopeToCheck.Remove(item); }
            }
            vScopeToRemove.Clear();

            foreach (TECVisualScope vScope in vScopeToCheck)
            {
                if (vScope.Scope is TECEquipment)
                {
                    foreach (TECSubScope sub in (vScope.Scope as TECEquipment).SubScope)
                    {
                        if (!accountedFor.Contains(sub))
                        {
                            accountedFor.Add(sub);
                            outList.Add(Tuple.Create<TECSubScope, TECVisualScope>(sub, vScope));
                            vScopeToRemove.Add(vScope);
                        }
                    }
                }
            }
            foreach (TECVisualScope item in vScopeToRemove)
            {
                if (vScopeToCheck.Contains(item))
                { vScopeToCheck.Remove(item); }
            }
            vScopeToRemove.Clear();
            
            foreach (TECVisualScope vScope in vScopeToCheck)
            {
                if (vScope.Scope is TECSystem)
                {
                    foreach (TECEquipment equip in (vScope.Scope as TECSystem).Equipment)
                    {
                        foreach (TECSubScope sub in equip.SubScope)
                        {
                            if (!accountedFor.Contains(sub))
                            {
                                accountedFor.Add(sub);
                                outList.Add(Tuple.Create<TECSubScope, TECVisualScope>(sub, vScope));
                                vScopeToRemove.Add(vScope);
                            }
                        }
                    }
                }
            }
            foreach (TECVisualScope item in vScopeToRemove)
            {
                if (vScopeToCheck.Contains(item))
                { vScopeToCheck.Remove(item); }
            }
            vScopeToRemove.Clear();
            return outList;
        }
        #endregion

        #region Database Version Update Methods
        static private void checkAndUpdateDB(Type type)
        {
            bool isUpToDate;
            isUpToDate = checkDatabaseVersion(type);
            if (!isUpToDate)
            {
                updateDatabase(type);
                updateVersionNumber(type);
            }
        }
        static private bool checkDatabaseVersion(Type type)
        {
            string currentVersion = Properties.Settings.Default.Version;
            DataTable infoDT = new DataTable();
            if (type == typeof(TECBid))
            { infoDT = SQLiteDB.getDataFromTable(BidInfoTable.TableName); }
            else if (type == typeof(TECTemplates))
            {
                try
                {
                    infoDT = SQLiteDB.getDataFromTable(TemplatesInfoTable.TableName);
                }
                catch
                {
                    killTemplatesInfo();
                    return false;
                }
            }
            else
            { throw new ArgumentException("checkDatabaseVersion given invalid type"); }

            if (infoDT.Rows.Count < 1)
            {
                if (type == typeof(TECBid))
                {
                    throw new DataException("Could not load from TECBidInfo");
                }
                else if(type == typeof(TECTemplates))
                {
                    return false;
                }
                else
                { return false; }
            }
            else if ((infoDT.Rows.Count == 1) || (type == typeof(TECBid)))
            {
                DataRow infoRow = infoDT.Rows[0];
                if (infoDT.Columns.Contains(BidInfoTable.DBVersion.Name) || infoDT.Columns.Contains(TemplatesInfoTable.DBVersion.Name))
                {
                    string version = infoRow[BidInfoTable.DBVersion.Name].ToString();
                    return (version == currentVersion);
                }
                else
                { return false; }
            }
            else if ((infoDT.Rows.Count > 1) && (type == typeof(TECTemplates)))
            {
                killTemplatesInfo();
                return false;
            }
            else
            {
                throw new NotImplementedException();
            }
        }
        static private void updateDatabase(Type type)
        {
            List<string> tableNames = getAllTableNames();
            List<object> databaseTableList = new List<object>();
            if (type == typeof(TECBid))
            { databaseTableList = AllBidTables.Tables; }
            else if (type == typeof(TECTemplates))
            { databaseTableList = AllTemplateTables.Tables; }
            else
            { throw new ArgumentException("updateDatabase() given invalid type"); }
            foreach (TableBase table in databaseTableList)
            {
                var tableInfo = new TableInfo(table);
                if (tableNames.Contains(tableInfo.Name))
                { updateTableFromType(table); }
                else
                { createTableFromDefinition(table); }
            }
        }
        static private void updateTableFromType(TableBase table)
        {
            var tableInfo = new TableInfo(table);
            string tableName = tableInfo.Name;
            string tempName = "temp_" + tableName;
            List<TableField> primaryKeys = tableInfo.PrimaryFields;
            List<TableField> fields = tableInfo.Fields;

            List<string> currentFields = getAllTableFields(tableName);
            List<string> currentPrimaryKeys = getPrimaryKeys(tableName);
            List<string> commonFields = new List<string>();
            foreach (TableField field in fields)
            {
                if (currentFields.Contains(field.Name))
                { commonFields.Add(field.Name); }
            }
            List<string> currentFieldNames = new List<string>();
            List<string> newFieldNames = new List<string>();
            foreach (string field in commonFields)
            {
                currentFieldNames.Add(field);
                newFieldNames.Add(field);
            }
           
            if (currentPrimaryKeys.Count == 1 && !commonFields.Contains(currentPrimaryKeys[0]) && (primaryKeys.Count == 1))
            {
                currentFieldNames.Add(currentPrimaryKeys[0]);
                newFieldNames.Add(primaryKeys[0].Name);
            }

            string currentCommonString = UtilitiesMethods.CommaSeparatedString(currentFieldNames);
            string newCommonString = UtilitiesMethods.CommaSeparatedString(newFieldNames);

            createTempTableFromDefinition(table);

            string commandString;
            if (commonFields.Count > 0)
            {
                commandString = "insert or ignore into '" + tempName + "' (" + newCommonString + ") select " + currentCommonString + " from '" + tableName + "'";
                SQLiteDB.nonQueryCommand(commandString);
            }
            
            commandString = "drop table '" + tableName + "'";
            SQLiteDB.nonQueryCommand(commandString);
            createTableFromDefinition(table);

            commandString = "insert into '" + tableName + "' select * from '" + tempName + "'";
            SQLiteDB.nonQueryCommand(commandString);
            commandString = "drop table '" + tempName + "'";
            SQLiteDB.nonQueryCommand(commandString);
            
        }
        private static void updateVersionNumber(Type type)
        {
            if (type == typeof(TECBid) || type == typeof(TECTemplates))
            {
                Dictionary<string, string> Data = new Dictionary<string, string>();
                if (type == typeof(TECBid))
                {
                    var infoBid = getBidInfo();
                    string commandString = "update " + BidInfoTable.TableName + " set " + BidInfoTable.DBVersion.Name + " = '" + Properties.Settings.Default.Version + "' ";
                    commandString += "where " + BidInfoTable.BidID.Name + " = '" + infoBid.Guid.ToString() + "'";
                    SQLiteDB.nonQueryCommand(commandString);
                }
                else if(type == typeof(TECTemplates))
                {
                    var templateGuid = getTemplatesInfo().Guid;

                    Dictionary<string, string> data = new Dictionary<string, string>();

                    data.Add(TemplatesInfoTable.DBVersion.Name, Properties.Settings.Default.Version);
                    data.Add(TemplatesInfoTable.TemplateID.Name, templateGuid.ToString());

                    SQLiteDB.Replace(TemplatesInfoTable.TableName, data);
                }
            }
        }
        private static void killTemplatesInfo()
        {
            string commandString = commandString = "drop table '" + TemplatesInfoTable.TableName + "'";
            SQLiteDB.nonQueryCommand(commandString);
        }
        #endregion

        #region Backup Methods
        private static void createBackup(string originalPath)
        {
            DebugHandler.LogDebugMessage("Backing up...");

            var date = DateTime.Now;
            
            string APPDATA_FOLDER = @"TECSystems\Backups\";
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string backupFolder = Path.Combine(appData, APPDATA_FOLDER);

            CultureInfo culture = CultureInfo.CreateSpecificCulture("ja-JP");
            DateTimeFormatInfo dtfi = culture.DateTimeFormat;
            dtfi.DateSeparator = "\\";
            backupFolder += date.ToString("d", dtfi);

            if (!Directory.Exists(backupFolder))
            { Directory.CreateDirectory(backupFolder); }

            string backupFileName = Path.GetFileNameWithoutExtension(originalPath);
            backupFileName += "-";
            culture = CultureInfo.CreateSpecificCulture("hr-HR");
            dtfi = culture.DateTimeFormat;
            dtfi.TimeSeparator = "-";
            backupFileName += date.ToString("T", dtfi);
            var backupPath = Path.Combine(backupFolder, backupFileName);
            
            File.Copy(originalPath, backupPath);

            DebugHandler.LogDebugMessage("Finished backup. Backup path: " + backupPath);
        }
        #endregion

        #region Table Row to Object Methods
        #region Base Scope
        private static TECSystem getSystemFromRow(DataRow row)
        {
            Guid systemID = new Guid(row[SystemTable.SystemID.Name].ToString());
            TECSystem system = new TECSystem(systemID);
            system.Name = row[SystemTable.Name.Name].ToString();
            system.Description = row[SystemTable.Description.Name].ToString();
            system.Quantity = row[SystemTable.Quantity.Name].ToString().ToInt();
            system.BudgetPriceModifier = row[SystemTable.BudgetPrice.Name].ToString().ToDouble();
            system.Equipment = getEquipmentInSystem(systemID);
            system.Tags = getTagsInScope(systemID);
            system.Location = getLocationInScope(systemID);
            system.AssociatedCosts = getAssociatedCostsInScope(systemID);
            return system;
        }
        private static TECEquipment getEquipmentFromRow(DataRow row)
        {
            Guid equipmentID = new Guid(row[EquipmentTable.EquipmentID.Name].ToString());
            TECEquipment equipmentToAdd = new TECEquipment(equipmentID);
            equipmentToAdd.Name = row[EquipmentTable.Name.Name].ToString();
            equipmentToAdd.Description = row[EquipmentTable.Description.Name].ToString();
            equipmentToAdd.Quantity = row[EquipmentTable.Quantity.Name].ToString().ToInt();
            equipmentToAdd.BudgetUnitPrice = row[EquipmentTable.BudgetPrice.Name].ToString().ToDouble();
            equipmentToAdd.SubScope = getSubScopeInEquipment(equipmentID);
            equipmentToAdd.Tags = getTagsInScope(equipmentID);
            equipmentToAdd.Location = getLocationInScope(equipmentID);
            equipmentToAdd.AssociatedCosts = getAssociatedCostsInScope(equipmentID);
            return equipmentToAdd;
        }
        private static TECSubScope getSubScopeFromRow(DataRow row)
        {
            Guid subScopeID = new Guid(row[SubScopeTable.SubScopeID.Name].ToString());
            TECSubScope subScopeToAdd = new TECSubScope(subScopeID);
            subScopeToAdd.Name = row[SubScopeTable.Name.Name].ToString();
            subScopeToAdd.Description = row[SubScopeTable.Description.Name].ToString();
            subScopeToAdd.Quantity = row[SubScopeTable.Quantity.Name].ToString().ToInt(1);
            subScopeToAdd.Devices = getDevicesInSubScope(subScopeID);
            subScopeToAdd.Points = getPointsInSubScope(subScopeID);
            subScopeToAdd.Location = getLocationInScope(subScopeID);
            subScopeToAdd.Tags = getTagsInScope(subScopeID);
            subScopeToAdd.AssociatedCosts = getAssociatedCostsInScope(subScopeID);
            return subScopeToAdd;
        }
        private static TECPoint getPointFromRow(DataRow row)
        {
            Guid pointID = new Guid(row[PointTable.PointID.Name].ToString());
            TECPoint pointToAdd = new TECPoint(pointID);
            pointToAdd.Name = row[PointTable.Name.Name].ToString();
            pointToAdd.Description = row[PointTable.Description.Name].ToString();
            pointToAdd.Type = TECPoint.convertStringToType(row[PointTable.Type.Name].ToString());
            pointToAdd.Quantity = row[PointTable.Quantity.Name].ToString().ToInt();
            pointToAdd.Tags = getTagsInScope(pointID);
            pointToAdd.AssociatedCosts = getAssociatedCostsInScope(pointID);
            return pointToAdd;
        }
        #endregion
        #region Catalogs
        private static TECConnectionType getConnectionTypeFromRow(DataRow row)
        {
            Guid guid = new Guid(row[ConnectionTypeTable.ConnectionTypeID.Name].ToString());
        string name = row[ConnectionTypeTable.Name.Name].ToString();
        string laborString = row[ConnectionTypeTable.Labor.Name].ToString();
        string costString = row[ConnectionTypeTable.Cost.Name].ToString();

        double cost = costString.ToDouble(0);
        double labor = laborString.ToDouble(0);

        var outConnectionType = new TECConnectionType(guid);
        outConnectionType.Name = name;
            outConnectionType.Cost = cost;
            outConnectionType.Labor = labor;
            outConnectionType.Tags = getTagsInScope(guid);
        outConnectionType.AssociatedCosts = getAssociatedCostsInScope(guid);
            return outConnectionType;
        }
        private static TECAssociatedCost getAssociatedCostFromRow(DataRow row)
        {
            Guid guid = new Guid(row[AssociatedCostTable.AssociatedCostID.Name].ToString());
            string name = row[AssociatedCostTable.Name.Name].ToString();
            double cost = row[AssociatedCostTable.Cost.Name].ToString().ToDouble(0);
            double labor = row[AssociatedCostTable.Labor.Name].ToString().ToDouble(0);

            var associatedCost = new TECAssociatedCost(guid);
            associatedCost.Name = name;
            associatedCost.Cost = cost;
            associatedCost.Labor = labor;

            return associatedCost;
        }
        private static TECDevice getDeviceFromRow(DataRow row)
        {
            Guid deviceID = new Guid(row[DeviceTable.DeviceID.Name].ToString());
            TECDevice deviceToAdd = new TECDevice(deviceID);
            deviceToAdd.Name = row[DeviceTable.Name.Name].ToString();
            deviceToAdd.Description = row[DeviceTable.Description.Name].ToString();
            deviceToAdd.Cost = row[DeviceTable.Cost.Name].ToString().ToDouble();
            deviceToAdd.Manufacturer = getManufacturerInDevice(deviceID);
            deviceToAdd.ConnectionType = getConnectionTypeInDevice(deviceID);
            deviceToAdd.Tags = getTagsInScope(deviceToAdd.Guid);
            deviceToAdd.AssociatedCosts = getAssociatedCostsInScope(deviceToAdd.Guid);
            return deviceToAdd;
        }
        private static TECManufacturer getManufacturerFromRow(DataRow row)
        {
            Guid manufacturerID = new Guid(row[ManufacturerTable.ManufacturerID.Name].ToString());
            var manufacturer = new TECManufacturer(manufacturerID);
            manufacturer.Name = row[ManufacturerTable.Name.Name].ToString();
            manufacturer.Multiplier = row[ManufacturerTable.Multiplier.Name].ToString().ToDouble(1);
            return manufacturer;
        }
        private static TECLocation getLocationFromRow(DataRow row)
        {
            Guid locationID = new Guid(row[LocationTable.LocationID.Name].ToString());
            var location = new TECLocation(locationID);
            location.Name = row[LocationTable.Name.Name].ToString();
            return location;
        }
        private static TECConduitType getConduitTypeFromRow(DataRow row)
        {
            Guid conduitGuid = new Guid(row[ConduitTypeTable.ConduitTypeID.Name].ToString());
            string name = row[ConduitTypeTable.Name.Name].ToString();
            double cost = row[ConduitTypeTable.Cost.Name].ToString().ToDouble(0);
            double labor = row[ConduitTypeTable.Labor.Name].ToString().ToDouble(0);
            var conduitType = new TECConduitType(conduitGuid);
            conduitType.Name = name;
            conduitType.Cost = cost;
            conduitType.Labor = labor;
            conduitType.Tags = getTagsInScope(conduitGuid);
            conduitType.AssociatedCosts = getAssociatedCostsInScope(conduitGuid);
            return conduitType;
        }
        private static TECTag getTagFromRow(DataRow row)
        {
            var tag = new TECTag(new Guid(row["TagID"].ToString()));
            tag.Text = row["TagString"].ToString();
            return tag;
        }
        private static TECPanelType getPanelTypeFromRow(DataRow row)
        {
            Guid guid = new Guid(row[PanelTypeTable.PanelTypeID.Name].ToString());
            TECPanelType panelType = new TECPanelType(guid);

            panelType.Name = row[PanelTypeTable.Name.Name].ToString();
            panelType.Cost = row[PanelTypeTable.Cost.Name].ToString().ToDouble(0);

            return panelType;
        }
        private static TECIOModule getIOModuleFromRow(DataRow row)
        {
            Guid guid = new Guid(row[IOModuleTable.IOModuleID.Name].ToString());
            TECIOModule module = new TECIOModule(guid);

            module.Name = row[IOModuleTable.Name.Name].ToString();
            module.Description = row[IOModuleTable.Description.Name].ToString();
            module.Cost = row[IOModuleTable.Cost.Name].ToString().ToDouble(0);
            module.IOPerModule = row[IOModuleTable.IOPerModule.Name].ToString().ToInt(1);
            module.Manufacturer = getManufacturerInIOModule(guid);
            return module;
        }

        #endregion
        #region Scope Qualifiers
        private static TECScopeBranch getScopeBranchFromRow(DataRow row)
        {
            Guid scopeBranchID = new Guid(row[ScopeBranchTable.ScopeBranchID.Name].ToString());
            TECScopeBranch branch = new TECScopeBranch(scopeBranchID);
            branch.Name = row[ScopeBranchTable.Name.Name].ToString();
            branch.Description = row[ScopeBranchTable.Description.Name].ToString();
            branch.Branches = getChildBranchesInBranch(scopeBranchID);
            branch.Tags = getTagsInScope(scopeBranchID);
            return branch;
        }
        private static TECNote getNoteFromRow(DataRow row)
        {
            Guid noteID = new Guid(row[NoteTable.NoteID.Name].ToString());
            var note = new TECNote(noteID);
            note.Text = row["NoteText"].ToString();
            return note;
        }
        private static TECExclusion getExclusionFromRow(DataRow row)
        {
            Guid exclusionId = new Guid(row["ExclusionID"].ToString());
            TECExclusion exclusion = new TECExclusion(exclusionId);
            exclusion.Text = row["ExclusionText"].ToString();
            return exclusion;
        }
        #endregion
        #region Drawing Objects
        private static TECDrawing getDrawingFromRow(DataRow row)
        {
            string name = row[DrawingTable.Name.Name].ToString();
            string description = row[DrawingTable.Description.Name].ToString();
            Guid guid = new Guid(row[DrawingTable.DrawingID.Name].ToString());
            ObservableCollection<TECPage> pages = getPagesInDrawing(guid);
            return new TECDrawing(name, description, guid, pages);
        }
        private static TECPage getPageFromRow(DataRow row)
        {
            Guid guid = new Guid(row[PageTable.PageID.Name].ToString());
            TECPage page = new TECPage(guid);
            page.PageNum = row[PageTable.PageNum.Name].ToString().ToInt();
            byte[] blob = row[PageTable.Image.Name] as byte[];
            page.Path = Path.GetTempFileName();
            File.WriteAllBytes(page.Path, blob);
            page.PageScope = getVisualScopeInPage(guid);
            return page;
        }
        private static TECVisualScope getVisualScopeFromRow(DataRow row)
        {
            Guid guid = new Guid(row[VisualScopeTable.VisualScopeID.Name].ToString());
            var visualScope = new TECVisualScope(guid);
            visualScope.X = row[VisualScopeTable.XPos.Name].ToString().ToDouble();
            visualScope.Y = row[VisualScopeTable.YPos.Name].ToString().ToDouble();
            visualScope.Scope = getScopeGuidInVisualScope(guid);
            return visualScope;
        }
        #endregion
        #region Control Scope
        private static TECPanel getPanelFromRow(DataRow row)
        {
            Guid guid = new Guid(row[PanelTable.PanelID.Name].ToString());
            TECPanel panel = new TECPanel(guid);

            panel.Name = row[PanelTable.Name.Name].ToString();
            panel.Description = row[PanelTable.Description.Name].ToString();
            panel.Quantity = row[PanelTable.Quantity.Name].ToString().ToInt(1);
            panel.Type = getPanelTypeInPanel(guid);
            panel.Controllers = getControllersInPanel(guid);

            return panel;
        }
        private static TECController getControllerFromRow(DataRow row)
        {
            Guid guid = new Guid(row[ControllerTable.ControllerID.Name].ToString());
            TECController controller = new TECController(guid);

            controller.Name = row[ControllerTable.Name.Name].ToString();
            controller.Description = row[ControllerTable.Description.Name].ToString();
            controller.Cost = row[ControllerTable.Cost.Name].ToString().ToDouble(0);
            controller.IO = getIOInController(guid);
            controller.Tags = getTagsInScope(guid);
            controller.Manufacturer = getManufacturerInController(guid);
            controller.AssociatedCosts = getAssociatedCostsInScope(guid);
            controller.ChildrenConnections = getConnectionsInController(controller);
            return controller;
        }
        private static TECIO getIOFromRow(DataRow row)
        {
            Guid guid = new Guid(row[IOTable.IOID.Name].ToString());
            var io = new TECIO(guid);
            io.Type = TECIO.convertStringToType(row[IOTable.IOType.Name].ToString());
            io.Quantity = row[IOTable.Quantity.Name].ToString().ToInt();
            io.IOModule = getModuleInIO(guid);
            return io;
        }
        private static TECSubScopeConnection getSubScopeConnectionFromRow(DataRow row)
        {
            Guid guid = new Guid(row[SubScopeConnectionTable.ConnectionID.Name].ToString());
            TECSubScopeConnection connection = new TECSubScopeConnection(guid);
            connection.Length = row[SubScopeConnectionTable.Length.Name].ToString().ToDouble();
            connection.ConduitType = getConduitTypeInConnection(connection.Guid);
            connection.SubScope = getSubScopeInSubScopeConnection(connection.Guid);
            return connection;
        }
        private static TECNetworkConnection getNetworkConnectionFromRow(DataRow row)
        {
            Guid guid = new Guid(row[NetworkConnectionTable.ConnectionID.Name].ToString());
            TECNetworkConnection connection = new TECNetworkConnection(guid);
            connection.Length = row[NetworkConnectionTable.Length.Name].ToString().ToDouble();
            connection.ConduitType = getConduitTypeInConnection(connection.Guid);
            connection.IOType = getIOTypeInNetworkConnection(connection.Guid);
            connection.ChildrenControllers = getChildrenInNetworkConnection(connection.Guid);
            return connection;
        }
        #endregion

        #region Misc
        private static TECMiscCost getMiscCostFromRow(DataRow row)
        {
            Guid guid = new Guid(row[MiscCostTable.MiscCostID.Name].ToString());
            TECMiscCost cost = new TECMiscCost(guid);

            cost.Name = row[MiscCostTable.Name.Name].ToString();
            cost.Cost = row[MiscCostTable.Cost.Name].ToString().ToDouble(0);
            cost.Quantity = row[MiscCostTable.Quantity.Name].ToString().ToInt(1);

            return cost;
        }
        private static TECMiscWiring getMiscWiringFromRow(DataRow row)
        {
            Guid guid = new Guid(row[MiscWiringTable.MiscWiringID.Name].ToString());
            TECMiscWiring wiring = new TECMiscWiring(guid);

            wiring.Name = row[MiscWiringTable.Name.Name].ToString();
            wiring.Cost = row[MiscWiringTable.Cost.Name].ToString().ToDouble(0);
            wiring.Quantity = row[MiscWiringTable.Quantity.Name].ToString().ToInt(1);

            return wiring;
        }
        private static TECBidParameters getBidParametersFromRow(DataRow row)
        {
            Guid guid = new Guid(row[BidParametersTable.ParamtersID.Name].ToString());
            TECBidParameters paramters = new TECBidParameters(guid);

            paramters.Escalation = row[BidParametersTable.Escalation.Name].ToString().ToDouble(0);
            paramters.Overhead = row[BidParametersTable.Overhead.Name].ToString().ToDouble(0);
            paramters.Profit = row[BidParametersTable.Profit.Name].ToString().ToDouble(0);
            paramters.SubcontractorMarkup = row[BidParametersTable.SubcontractorMarkup.Name].ToString().ToDouble(0);
            paramters.SubcontractorEscalation = row[BidParametersTable.SubcontractorEscalation.Name].ToString().ToDouble(0);

            paramters.IsTaxExempt = row[BidParametersTable.IsTaxExempt.Name].ToString().ToInt(0).ToBool();
            paramters.RequiresBond = row[BidParametersTable.RequiresBond.Name].ToString().ToInt(0).ToBool();
            paramters.RequiresWrapUp = row[BidParametersTable.RequiresWrapUp.Name].ToString().ToInt(0).ToBool();

            return paramters;
        }

        #endregion

        private static TECControlledScope getControlledScopeFromRow(DataRow row)
        {
            Guid guid = new Guid(row[ControlledScopeTable.ControlledScopeID.Name].ToString());
            TECControlledScope controlledScope = new TECControlledScope(guid);

            controlledScope.Name = row[ControlledScopeTable.Name.Name].ToString();
            controlledScope.Description = row[ControlledScopeTable.Description.Name].ToString();
            controlledScope.Controllers = getControllersInControlledScope(guid);
            controlledScope.Systems = getSystemsInControlledScope(guid);
            controlledScope.Panels = getPanelsInControlledScope(guid);

            return controlledScope;
        }
        private static TECSubScope getSubScopePlaceholderFromRow(DataRow row)
        {
            Guid subScopeID = new Guid(row[SubScopeTable.SubScopeID.Name].ToString());
            TECSubScope subScopeToAdd = new TECSubScope(subScopeID);
            subScopeToAdd.Name = row[SubScopeTable.Name.Name].ToString();
            subScopeToAdd.Description = row[SubScopeTable.Description.Name].ToString();
            subScopeToAdd.Tags = getTagsInScope(subScopeID);
            return subScopeToAdd;
        }
        private static TECController getControllerPlaceholderFromRow(DataRow row)
        {
            Guid guid = new Guid(row[ControllerTable.ControllerID.Name].ToString());
            TECController controller = new TECController(guid);

            controller.Name = row[ControllerTable.Name.Name].ToString();
            controller.Description = row[ControllerTable.Description.Name].ToString();
            controller.Cost = row[ControllerTable.Cost.Name].ToString().ToDouble(0);
            return controller;
        }
        #endregion

        #region Generic Create Methods
        static private void createTableFromDefinition(TableBase table)
        {
            var tableInfo = new TableInfo(table);
            string tableName = tableInfo.Name;
            List<TableField> primaryKey = tableInfo.PrimaryFields;
            List<TableField> fields = tableInfo.Fields;

            string createString = "CREATE TABLE '" + tableName + "' (";
            foreach (TableField field in fields)
            {
                createString += "'" + field.Name + "' " + field.FieldType;
                if (fields.IndexOf(field) < (fields.Count - 1))
                { createString += ", "; }
            }
            if (primaryKey.Count != 0)
            { createString += ", PRIMARY KEY("; }
            foreach (TableField pk in primaryKey)
            {
                createString += "'" + pk.Name + "' ";
                if (primaryKey.IndexOf(pk) < (primaryKey.Count - 1))
                { createString += ", "; }
                else
                { createString += ")"; }
            }
            createString += ")";
            SQLiteDB.nonQueryCommand(createString);
        }
        static private void createTempTableFromDefinition(TableBase table)
        {
            var tableInfo = new TableInfo(table);
            string tableName = "temp_" + tableInfo.Name;
            List<TableField> primaryKey = tableInfo.PrimaryFields;
            List<TableField> fields = tableInfo.Fields;

            string createString = "CREATE TEMPORARY TABLE '" + tableName + "' (";
            foreach (TableField field in fields)
            {
                createString += "'" + field.Name + "' " + field.FieldType;
                if (fields.IndexOf(field) < (fields.Count - 1))
                { createString += ", "; }
            }
            if (primaryKey.Count != 0)
            { createString += ", PRIMARY KEY("; }
            foreach (TableField pk in primaryKey)
            {
                createString += "'" + pk.Name + "' ";
                if (primaryKey.IndexOf(pk) < (primaryKey.Count - 1))
                { createString += ", "; }
                else
                { createString += ")"; }
            }
            createString += ")";
            SQLiteDB.nonQueryCommand(createString);
        }
        static private void createAllBidTables()
        {
            foreach (TableBase table in AllBidTables.Tables)
            {
                createTableFromDefinition(table);
            }
        }
        static private void createAllTemplateTables()
        {
            foreach (TableBase table in AllTemplateTables.Tables)
            {
                createTableFromDefinition(table);
            }
        }
        #endregion

        #region Generic Complete Save Methods
        private static void saveCompleteBid(TECBid bid)
        {
            addObject(new StackItem(Change.Add, bid, bid));
            addObject(new StackItem(Change.Add, bid, bid.Labor));
            addObject(new StackItem(Change.Add, bid, bid.Parameters));
            foreach(TECSystem system in bid.Systems)
            {
                addObject(new StackItem(Change.Add, bid, system));
                saveScopeChildProperties(system);
                saveCompleteEquipment(system);
            }
            foreach(TECManufacturer manufacturer in bid.ManufacturerCatalog)
            { addObject(new StackItem(Change.Add, bid, manufacturer)); }
            foreach(TECController controller in bid.Controllers)
            {
                addObject(new StackItem(Change.Add, bid, controller));
                saveScopeChildProperties(controller);
                saveControllerChildProperties(controller);
            }
            foreach (TECAssociatedCost associatedCost in bid.AssociatedCostsCatalog)
            { addObject(new StackItem(Change.Add, bid, associatedCost)); }
            foreach (TECNote note in bid.Notes)
            { addObject(new StackItem(Change.Add, bid, note)); }
            foreach (TECExclusion exclusion in bid.Exclusions)
            { addObject(new StackItem(Change.Add, bid, exclusion)); }
            foreach (TECLocation location in bid.Locations)
            { addObject(new StackItem(Change.Add, bid, location)); }
            foreach (TECConduitType conduitType in bid.ConduitTypes)
            {
                addObject(new StackItem(Change.Add, bid, conduitType));
                saveScopeChildProperties(conduitType);
            }
            foreach (TECConnectionType connectionType in bid.ConnectionTypes)
            {
                addObject(new StackItem(Change.Add, bid, connectionType));
                saveScopeChildProperties(connectionType);
            }
            foreach (TECTag tag in bid.Tags)
            { addObject(new StackItem(Change.Add, bid, tag)); }
            foreach (TECScopeBranch branch in bid.ScopeTree)
            {
                addObject(new StackItem(Change.Add, bid, branch));
                saveCompleteScopeBranch(branch);
            }
            foreach(TECDrawing drawing in bid.Drawings)
            {
                addObject(new StackItem(Change.Add, bid, drawing));
                saveCompletePage(drawing);
            }
            foreach(TECProposalScope proposalScope in bid.ProposalScope)
            {
                addObject(new StackItem(Change.Add, bid, proposalScope));
                saveCompleteProposalScope(proposalScope);
            }
            foreach(TECDevice device in bid.DeviceCatalog)
            {
                addObject(new StackItem(Change.Add, bid, device));
                saveScopeChildProperties(device);
                saveDeviceChildProperties(device);
            }
            foreach(TECMiscCost cost in bid.MiscCosts)
            {
                addObject(new StackItem(Change.Add, bid, cost));
            }
            foreach (TECMiscWiring wiring in bid.MiscWiring)
            {
                addObject(new StackItem(Change.Add, bid, wiring));
            }
            foreach (TECPanelType panelType in bid.PanelTypeCatalog)
            {
                addObject(new StackItem(Change.Add, bid, panelType));
            }
            foreach (TECPanel panel in bid.Panels)
            {
                savePanel(panel, bid);
            }
            foreach (TECIOModule ioModule in bid.IOModuleCatalog)
            {
                saveCompleteIOModule(ioModule, bid);
            }
        }
        private static void saveCompleteTemplate(TECTemplates templates)
        {
            addObject(new StackItem(Change.Add, templates, templates));
            addObject(new StackItem(Change.Add, templates, templates.Labor));
            foreach(TECTag tag in templates.Tags)
            { addObject(new StackItem(Change.Add, templates, tag)); }
            foreach (TECSystem system in templates.SystemTemplates)
            {
                addObject(new StackItem(Change.Add, templates, system));
                saveScopeChildProperties(system);
                saveCompleteEquipment(system);
            }
            foreach (TECEquipment equipment in templates.EquipmentTemplates)
            {
                addObject(new StackItem(Change.Add, templates, equipment));
                saveScopeChildProperties(equipment);
                saveCompleteSubScope(equipment);
            }
            foreach (TECSubScope subScope in templates.SubScopeTemplates)
            {
                addObject(new StackItem(Change.Add, templates, subScope));
                saveScopeChildProperties(subScope);
                saveDevicesInSubScope(subScope);
                saveCompletePoints(subScope);
            }
            foreach (TECDevice device in templates.DeviceCatalog)
            {
                saveDeviceInCatalog(device, templates);
            }
            foreach (TECManufacturer manufacturer in templates.ManufacturerCatalog)
            { addObject(new StackItem(Change.Add, templates, manufacturer)); }
            foreach (TECController controller in templates.ControllerTemplates)
            {
                addObject(new StackItem(Change.Add, templates, controller));
                saveScopeChildProperties(controller);
                saveControllerChildProperties(controller);
            }
            foreach (TECConduitType conduitType in templates.ConduitTypeCatalog)
            {
                addObject(new StackItem(Change.Add, templates, conduitType));
                saveAssociatedCosts(conduitType);
            }
            foreach (TECConnectionType connectionType in templates.ConnectionTypeCatalog)
            {
                addObject(new StackItem(Change.Add, templates, connectionType));
                saveAssociatedCosts(connectionType);
            }
            foreach (TECAssociatedCost associatedCost in templates.AssociatedCostsCatalog)
            { addObject(new StackItem(Change.Add, templates, associatedCost)); }
            foreach (TECMiscCost cost in templates.MiscCostTemplates)
            {
                addObject(new StackItem(Change.Add, templates, cost));
            }
            foreach (TECMiscWiring wiring in templates.MiscWiringTemplates)
            {
                addObject(new StackItem(Change.Add, templates, wiring));
            }
            foreach (TECPanelType panelType in templates.PanelTypeCatalog)
            {
                addObject(new StackItem(Change.Add, templates, panelType));
            }
            foreach (TECPanel panel in templates.PanelTemplates)
            {
                savePanel(panel, templates);
            }
            foreach(TECControlledScope conScope in templates.ControlledScopeTemplates)
            {
                saveFullControlledScope(conScope, templates);
            }
            foreach(TECIOModule ioModule in templates.IOModuleCatalog)
            {
                saveCompleteIOModule(ioModule, templates);
            }
        }

        private static void saveFullControlledScope(TECControlledScope conScope, TECTemplates templates)
        {
            addObject(new StackItem(Change.Add, templates, conScope));
            saveScopeChildProperties(conScope);
            foreach (TECSystem system in conScope.Systems)
            {
                addObject(new StackItem(Change.Add, conScope, system));
                saveScopeChildProperties(system);
                saveCompleteEquipment(system);
            }
            foreach (TECPanel panel in conScope.Panels)
            {
                savePanel(panel, conScope);
            }
            foreach(TECController controller in conScope.Controllers)
            {
                addObject(new StackItem(Change.Add, conScope, controller));
                saveScopeChildProperties(controller);
                saveControllerChildProperties(controller);
            }
        }

        private static void saveDevicesInSubScope(TECSubScope subscope)
        {
            foreach(TECDevice device in subscope.Devices)
            { addObject(new StackItem(Change.Add, subscope, device)); }
        }
        private static void saveDeviceInCatalog(TECDevice device, object bidOrTemplates)
        {
            if(bidOrTemplates is TECBid || bidOrTemplates is TECTemplates)
            {
                addObject(new StackItem(Change.Add, bidOrTemplates, device));
                saveScopeChildProperties(device);
                saveDeviceChildProperties(device);
            }
        }
        private static void saveCompletePoints(TECSubScope subScope)
        {
            foreach(TECPoint point in subScope.Points)
            {
                addObject(new StackItem(Change.Add, subScope, point));
                saveScopeChildProperties(point);
            }
        }
        private static void saveCompleteSubScope(TECEquipment equipment)
        {
            foreach(TECSubScope subScope in equipment.SubScope)
            {
                addObject(new StackItem(Change.Add, equipment, subScope));
                saveScopeChildProperties(subScope);
                saveDevicesInSubScope(subScope);
                saveCompletePoints(subScope);
            }
        }
        private static void saveCompleteEquipment(TECSystem system)
        {
            foreach(TECEquipment equipment in system.Equipment)
            {
                addObject(new StackItem(Change.Add, system, equipment));
                saveScopeChildProperties(equipment);
                saveCompleteSubScope(equipment);
            }
        }
        private static void saveCompleteScopeBranch(TECScopeBranch branch)
        {
            foreach(TECScopeBranch subBranch in branch.Branches)
            {
                addObject(new StackItem(Change.Add, branch, subBranch));
                saveCompleteScopeBranch(subBranch);
            }
        }
        private static void saveCompleteVisualScope(TECPage page)
        {
            foreach(TECVisualScope visualScope in page.PageScope)
            { addObject(new StackItem(Change.Add, page, visualScope)); }
        }
        private static void saveCompleteVisualConnections(TECPage page)
        {
            foreach (TECVisualConnection visualConnection in page.Connections)
            { addObject(new StackItem(Change.Add, page, visualConnection)); }
        }
        private static void saveCompletePage(TECDrawing drawing)
        {
            foreach(TECPage page in drawing.Pages)
            {
                addObject(new StackItem(Change.Add, drawing, page));
                saveCompleteVisualScope(page);
                saveCompleteVisualConnections(page);
            }
        }
        private static void saveCompleteProposalScope(TECProposalScope proposalScope)
        {
            
            foreach(TECProposalScope subProposalScope in proposalScope.Children)
            {
                addObject(new StackItem(Change.Add, proposalScope, subProposalScope));
                saveCompleteProposalScope(subProposalScope);
            }
            foreach(TECScopeBranch branch in proposalScope.Notes)
            {
                addObject(new StackItem(Change.Add, proposalScope, branch));
                saveCompleteScopeBranch(branch);
            }
        }
        private static void saveScopeChildProperties(TECScope scope)
        {
            saveLocation(scope);
            saveTags(scope);
            saveAssociatedCosts(scope);
        }
        private static void saveLocation(TECScope scope)
        {
            if(scope.Location != null)
            {
                addObject(new StackItem(Change.Add, scope, scope.Location, typeof(TECScope), typeof(TECLocation)));
            }
        }
        private static void saveTags(TECScope scope)
        {
            foreach(TECTag tag in scope.Tags)
            {
                addObject(new StackItem(Change.Add, scope, tag, typeof(TECScope), typeof(TECTag)));
            }
        }
        private static void saveAssociatedCosts(TECScope scope)
        {
            foreach(TECAssociatedCost cost in scope.AssociatedCosts)
            {
                addObject(new StackItem(Change.Add, scope, cost, typeof(TECScope), typeof(TECAssociatedCost)));
            }
        }
        private static void saveControllerChildProperties(TECController controller)
        {
            if(controller.Manufacturer != null) { addObject(new StackItem(Change.AddRelationship, controller, controller.Manufacturer)); }
            foreach(TECIO IO in controller.IO)
            {
                addObject(new StackItem(Change.Add, controller, IO));
                if(IO.IOModule != null)
                {
                    addObject(new StackItem(Change.Add, IO, IO.IOModule));

                }
            }
            foreach(TECConnection connection in controller.ChildrenConnections)
            {
                addObject(new StackItem(Change.Add, controller, connection, typeof(TECController), typeof(TECConnection)));
                saveConnectionChildren(connection);
            }
        }
        private static void saveDeviceChildProperties(TECDevice device)
        {
            if(device.Manufacturer != null) { addObject(new StackItem(Change.Add, device, device.Manufacturer)); }
            if(device.ConnectionType != null) { addObject(new StackItem(Change.Add, device, device.ConnectionType)); }
        }
        private static void savePanel(TECPanel panel, object parent)
        {
            addObject(new StackItem(Change.Add, parent, panel));
            addObject(new StackItem(Change.Add, panel, panel.Type));
            foreach(TECController controller in panel.Controllers)
            {
                addRelationship(new StackItem(Change.AddRelationship, panel, controller));
            }
            saveScopeChildProperties(panel);

        }
        private static void saveConnectionChildren(TECConnection connection)
        {
            if(connection is TECNetworkConnection)
            {
                foreach (TECController scope in (connection as TECNetworkConnection).ChildrenControllers)
                {
                    addRelationship(new StackItem(Change.AddRelationship, connection, scope, typeof(TECNetworkConnection), typeof(TECController)));
                }
            }
            else if(connection is TECSubScopeConnection)
            {
                if ((connection as TECSubScopeConnection).SubScope != null) { addRelationship(new StackItem(Change.AddRelationship, connection, (connection as TECSubScopeConnection).SubScope, typeof(TECSubScopeConnection), typeof(TECSubScope))); }
            }

            if (connection.ConduitType != null) { addObject(new StackItem(Change.AddRelationship,  connection, connection.ConduitType, typeof(TECConnection), typeof(TECConduitType))); }
        }
        private static void saveCompleteIOModule(TECIOModule ioModule, object parent)
        {
            addObject(new StackItem(Change.Add, ioModule, parent));
            addObject(new StackItem(Change.Add, ioModule.Manufacturer, ioModule));
        }
        #endregion

        #region Generic Add Methods
        private static void addObject(StackItem item)
        {
            //ObjectsToAdd = [targetObject, referenceObject];
            var relevantTables = getRelevantTablesForAddRemove(item);
            foreach (TableBase table in relevantTables)
            {
                var tableInfo = new TableInfo(table);
                if (tableInfo.IsRelationTable)
                { updateIndexedRelation(table, item); }
                else
                { addObjectToTable(table, item); } 
            } 
        }
        private static void addObjectToTable(TableBase table, StackItem item)
        {
            var tableInfo = new TableInfo(table);
            
            Dictionary<string, string> data = assembleDataToAddRemove(table, item);

            if (data.Count > 0)
            {
                if (!SQLiteDB.Insert(tableInfo.Name, data))
                {
                    DebugHandler.LogError("Error: Couldn't add data to " + tableInfo.Name + " table.");
                }
            }
        }
        private static void updateIndexedRelation(TableBase table, StackItem item)
        {
            var tableInfo = new TableInfo(table);

            var childrenCollection = UtilitiesMethods.GetChildCollection(item.TargetObject, item.ReferenceObject);
            
            foreach(TECObject child in (IList)childrenCollection)
            {
                Dictionary<string, string> data = new Dictionary<string, string>();
                foreach (TableField field in tableInfo.Fields)
                {
                    if (field.Property.Name == "Index" && field.Property.ReflectedType == typeof(HelperProperties))
                    {
                        var dataString = objectToDBString(((IList)childrenCollection).IndexOf(child));
                        data.Add(field.Name, dataString);
                    }
                    else if (field.Property.Name == "Quantity" && field.Property.ReflectedType == typeof(HelperProperties))
                    {
                        var dataString = objectToDBString(getQuantityInParentCollection(item.TargetObject, item.ReferenceObject));
                        data.Add(field.Name, dataString);
                    }
                    assembleDataWithObjects(data, item.Objects(), tableInfo, field);
                }
                if (data.Count > 0)
                {
                    if (!SQLiteDB.Replace(tableInfo.Name, data))
                    {
                        DebugHandler.LogError("Couldn't add data to " + tableInfo.Name + " table.");
                    }
                }
            }
            
        }

        private static void addRelationship(StackItem item)
        {
            //ObjectsToAdd = [targetObject, referenceObject];
            var relevantTables = getRelevantTablesForAddRemoveRelationship(item);
            foreach (TableBase table in relevantTables)
            {
                var tableInfo = new TableInfo(table);
                if (tableInfo.IsRelationTable)
                { updateIndexedRelation(table, item); }
                else
                { addObjectToTable(table, item); }
            }
        }
        #endregion

        #region Generic Remove Methods
        private static void removeObject(StackItem item)
        {
            var relevantTables = getRelevantTablesForAddRemove(item);
            foreach (TableBase table in relevantTables)
            {
                var tableInfo = new TableInfo(table);
                removeObjectFromTable(table, item);
            }

        }
        private static void removeObjectFromTable(TableBase table, StackItem item)
        {
            var tableInfo = new TableInfo(table);
            if (fieldsIncludeQuantity(tableInfo.Fields))
            {
                var qty = getQuantityInParentCollection(item.TargetObject, item.ReferenceObject);
                if (qty > 1)
                {
                    editObjectInTable(table, item);
                    return;
                }
            }

            Dictionary<string, string> data = assembleDataToAddRemove(table, item);
            if(data.ContainsKey("Quantity"))
            {
                data.Remove("Quantity");
            }

            if (data.Count > 0)
            {
                if (!SQLiteDB.Delete(tableInfo.Name, data))
                { DebugHandler.LogError("Couldn't remove data from " + tableInfo.Name + " table."); }
            }
        }

        private static void removeRelationship(StackItem item)
        {
            var relevantTables = getRelevantTablesForAddRemoveRelationship(item);
            foreach (TableBase table in relevantTables)
            {
                var tableInfo = new TableInfo(table);
                removeObjectFromTable(table, item);
            }

        }
        #endregion

        #region Generic Edit Methods
        private static void editObject(StackItem item)
        {
            var relevantTables = getRelevantTablesToEdit(item);
            foreach (TableBase table in relevantTables)
            {
                var tableInfo = new TableInfo(table);
                editObjectInTable(table, item);
            }
        }
        private static void editObjectInTable(TableBase table, StackItem item)
        {
            var tableInfo = new TableInfo(table);
            var relevantObjects = new Object[]
                { item.TargetObject,
                item.ReferenceObject}; 
            Dictionary<string, string> data = new Dictionary<string, string>();
            
            if(item.TargetType == item.ReferenceType)
            {
                relevantObjects = new Object[]
                { item.TargetObject };
            }
            foreach (TableField field in tableInfo.Fields)
            {
                if ((field.Property != null) && (field.Property.Name == "Quantity" && field.Property.ReflectedType == typeof(HelperProperties)))
                {
                    var dataString = objectToDBString(getQuantityInParentCollection(item.TargetObject, item.ReferenceObject));
                    data.Add(field.Name, dataString);
                }
                assembleDataWithObjects(data, relevantObjects, tableInfo, field);
            }

            if (data.Count > 0)
            {
                if (!SQLiteDB.Replace(tableInfo.Name, data))
                { DebugHandler.LogError("Couldn't edit data in " + tableInfo.Name + " table."); }
            }
        }
        private static List<TableBase> getRelevantTablesToEdit(StackItem item)
        {
            var relevantTables = new List<TableBase>();
            var objectTypes = new List<Type> {item.TargetType, item.ReferenceType };

            foreach (TableBase table in AllTables.Tables)
            {
                var tableInfo = new TableInfo(table);
                bool allTypesMatch = sharesAllTypesForEdit(objectTypes, tableInfo.Types);
                bool tableHasOnlyType = hasOnlyType(objectTypes[0], tableInfo.Types);
                bool shouldIncludeCatalog = isCatalogEdit(objectTypes, tableInfo.IsCatalogTable);

                if ((allTypesMatch || tableHasOnlyType) && (shouldIncludeCatalog))
                { relevantTables.Add(table);}
            }
            return relevantTables;
        }
        #endregion
        
        #region Helper Methods
        static private List<string> getAllTableNames()
        {
            string command = "select name from sqlite_master where type = 'table' order by 1";
            DataTable tables = SQLiteDB.getDataFromCommand(command);
            List<string> tableNames = new List<string>();
            foreach (DataRow row in tables.Rows)
            {
                tableNames.Add(row["Name"].ToString());
            }
            return tableNames;
        }
        static private List<string> getAllTableFields(string tableName)
        {
            string command = "select * from " + tableName + " limit 1";
            DataTable data = SQLiteDB.getDataFromCommand(command);
            List<string> tableFields = new List<string>();
            foreach (DataColumn col in data.Columns)
            {
                tableFields.Add(col.ColumnName);
            }
            return tableFields;
        }
        static private List<string> getPrimaryKeys(string tableName)
        {
            string command = "PRAGMA table_info(" + tableName + ")";
            DataTable data = SQLiteDB.getDataFromCommand(command);
            List<string> primaryKeys = new List<string>();
            foreach (DataRow row in data.Rows)
            {
                if(row["pk"].ToString() != "0")
                {
                    primaryKeys.Add(row["name"].ToString());
                }
                
            }
            return primaryKeys;
        }
        private static Dictionary<string, string> assembleDataToAddRemove(TableBase table, StackItem item)
        {
            var tableInfo = new TableInfo(table);
            var relevantObjects = new object[]
            {
                item.TargetObject,
                item.ReferenceObject
            };
            Dictionary<string, string> data = new Dictionary<string, string>();

            var isHierarchial = false;
            if (tableInfo.Types.Count == 2 && tableInfo.Types[0] == tableInfo.Types[1])
            {
                isHierarchial = true;
                relevantObjects = new object[]
                { 
                    item.ReferenceObject,
                    item.TargetObject
                };
            }
            else if (tableInfo.Types.Count == 1)
            {
                relevantObjects = new object[]
                { item.TargetObject };
            }

            int currentField = 0;
            foreach (TableField field in tableInfo.Fields)
            //tableInfo.Item2 = AllTableFields;
            {
                if (isHierarchial)
                {
                    if (isFieldType(tableInfo, field, relevantObjects[currentField]))
                    {
                        DebugHandler.LogDebugMessage("Adding " + field.Name + " to table " + tableInfo.Name + " with type " + relevantObjects[currentField].GetType(), DEBUG_GENERIC);
                        
                        var dataString = objectToDBString(field.Property.GetValue(relevantObjects[currentField], null));
                        data.Add(field.Name, dataString);
                    }
                }
                else
                {
                    if (field.Property.Name == "Quantity" && field.Property.ReflectedType == typeof(HelperProperties))
                    {
                        var dataString = objectToDBString(getQuantityInParentCollection(item.TargetObject, item.ReferenceObject));
                        data.Add(field.Name, dataString);
                    } else if (field.Property.Name == "DBVersion" && field.Property.ReflectedType == typeof(HelperProperties))
                    {
                        var dataString = objectToDBString(Properties.Settings.Default.Version);
                        data.Add(field.Name, dataString);
                    }
                    assembleDataWithObjects(data, relevantObjects, tableInfo, field);
                }
                currentField++;
            }

            return data;
        } 
        private static Dictionary<string, string> assembleDataWithObjects(Dictionary<string, string> data, Object[] relevantObjects, TableInfo tableInfo, TableField field)
        {
            foreach (Object item in relevantObjects)
            {
                if (isFieldType(tableInfo, field, item))
                {
                    DebugHandler.LogDebugMessage("Changing " + field.Name + " in table " + tableInfo.Name + " with type " + item.GetType(), DEBUG_GENERIC);
                    
                    var dataString = objectToDBString(field.Property.GetValue(item, null));
                    data.Add(field.Name, dataString);
                }
            }
            return data;
        }
        private static List<TableBase> getRelevantTablesForAddRemove(StackItem item)
        {
            var relevantTables = new List<TableBase>();
            List<Type> objectTypes = new List<Type> { item.TargetType, item.ReferenceType };
            List<Type> objectTypesWithTarget = new List<Type> { item.TargetType, item.ReferenceType, item.TargetObject.GetType() };
            foreach (TableBase table in AllTables.Tables)
            {
                var tableInfo = new TableInfo(table);

                //TableInfo.Item4 = List<TableType>
                bool allTypesMatch = sharesAllTypes(objectTypes, tableInfo.Types);
                bool allTypesAndTargetObjectTypeMatch = sharesAllTypes(objectTypesWithTarget, tableInfo.Types);
                bool tableHasOnlyType = hasOnlyType(item.TargetObject.GetType(), tableInfo.Types);
                //bool baseAndObjectMatch = hasBaseTypeAndType(objectTypes, tableInfo.Types);
                bool baseAndObjectMatch = false;
                bool shouldIncludeCatalog = isCatalogEdit(objectTypes, tableInfo.IsCatalogTable);

                if ((allTypesMatch || tableHasOnlyType || baseAndObjectMatch || allTypesAndTargetObjectTypeMatch) && (shouldIncludeCatalog))
                {
                    relevantTables.Add(table);
                }
            }

            return relevantTables;

        }
        private static List<TableBase> getRelevantTablesForAddRemoveRelationship(StackItem item)
        {
            var relevantTables = new List<TableBase>();
            List<Type> objectTypes = new List<Type> {item.TargetType, item.ReferenceType };

            foreach (TableBase table in AllTables.Tables)
            {
                var tableInfo = new TableInfo(table);

                //TableInfo.Item4 = List<TableType>
                bool allTypesMatch = sharesAllTypes(objectTypes, tableInfo.Types);
                bool shouldIncludeCatalog = isCatalogEdit(objectTypes, tableInfo.IsCatalogTable);
                if(allTypesMatch && shouldIncludeCatalog)
                {
                    relevantTables = new List<TableBase>();
                    relevantTables.Add(table);
                    return relevantTables;
                }
            }

            return relevantTables;

        }

        private static string objectToDBString(Object inObject)
        {
            string outstring = "";
            if(inObject is bool)
            {
                outstring = ((bool)inObject).ToInt().ToString();
            }
            else
            {
                outstring = inObject.ToString();
            }
            
            return outstring;
        }
        private static List<Type> getObjectTypes(params Object[] objectsToAdd)
        {
            var outList = new List<Type>();

            foreach (Object item in objectsToAdd)
            {
                outList.Add(item.GetType());
            }

            return outList;
        }
        private static bool sharesTypes(List<Type> list1, List<Type> list2)
        {
            bool doesShare = false;
            foreach (Type type in list1)
            {
                foreach (Type otherType in list2)
                {
                    if (type == otherType)
                    {
                        doesShare = true;
                    }
                }
            }
            return doesShare;
        }
        private static bool sharesAllTypes(List<Type> list1, List<Type> list2)
        {
            var numMatch = 0;
            var uniqueList1 = getUniqueTypes(list1);
            var uniqueList2 = getUniqueTypes(list2);

            if((list1.Count == 2 && list2.Count == 2) && (uniqueList1.Count == uniqueList2.Count))
            {
                if (list1[0] == list2[0] && list1[1] == list2[1])
                {
                    return true;
                }
            }
            

            foreach (Type type in uniqueList1)
            {
                foreach (Type otherType in uniqueList2)
                {
                    if (type == otherType)
                    {
                        numMatch++;
                    }
                }
            }
            return ((numMatch == list1.Count) && (numMatch == list2.Count));
        }
        private static bool sharesAllTypesForEdit(List<Type> list1, List<Type> list2)
        {
            var numMatch = 0;
            var uniqueList1 = getUniqueTypes(list1);
            var uniqueList2 = getUniqueTypes(list2);

            foreach (Type type in uniqueList1)
            {
                foreach (Type otherType in uniqueList2)
                {
                    if (type == otherType)
                    {
                        numMatch++;
                    }
                }
            }
            return ((numMatch == list1.Count) && (numMatch == list2.Count));

        }
        private static bool hasOnlyType(Type primaryType, List<Type> list2)
        {
            bool doesShare = false;
            if(list2.Count == 1)
            {
                if(primaryType == list2[0])
                {
                    doesShare = true;
                }
            }
            return doesShare;
        }
        private static bool hasBaseTypeAndType(List<Type> list1, List<Type> list2)
        {
            var uniqueList1 = getUniqueTypes(list1);
            var uniqueList2 = getUniqueTypes(list2);
            Type list1Type1 = null;
            Type list1Type2 = null;
            Type list2Type1 = null;
            Type list2Type2 = null;

            if (uniqueList1.Count == 2)
            {
                list1Type1 = uniqueList1[0];
                list1Type2 = uniqueList1[1];
            } else
            {
                return false;
            }

            if (uniqueList2.Count == 2)
            {
                list2Type1 = uniqueList2[0];
                list2Type2 = uniqueList2[1];
            }
            else
            {
                return false;
            }


            if(((list1Type1.BaseType == list2Type1 && list1Type2 == list2Type2) || (list1Type1.BaseType == list2Type2 && list1Type2 == list2Type1)) ||
                ((list1Type2.BaseType == list2Type1 && list1Type1 == list2Type2) || (list1Type2.BaseType == list2Type2 && list1Type1 == list2Type1)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private static bool isCatalogEdit(List<Type> list1, bool isCatalogTable)
        {
            bool isEdit = false;
            bool isBidOrTemplates = ((list1.Contains(typeof(TECBid)) || list1.Contains(typeof(TECTemplates))));
            bool isEditingObject = (list1.Count == 2) && (list1[0] == list1[1]);
            if ((isBidOrTemplates && (isCatalogTable)) || (!isCatalogTable) || isEditingObject)
            {
                isEdit = true;
            }
            return isEdit;
        }
        private static bool isFieldType(TableInfo table, TableField field, Object consideredObject)
        {
            var type = consideredObject.GetType();
            if(field.Property == null)
                return false;
            else if(field.Property.ReflectedType == type)
                return true;
            else if (field.Property.ReflectedType == type.BaseType && !table.Types.Contains(type))
                return true;
            else
                return false;
        }
        private static bool fieldsIncludeQuantity(List<TableField> fields)
        {
            var includes = false;
            foreach(TableField field in fields)
            {
                if (field.Property.Name == "Quantity" && field.Property.ReflectedType == typeof(HelperProperties))
                {
                    includes = true;
                }
            }

            return includes;
        }
        private static List<Type> getUniqueTypes(List<Type> types)
        {
            var outList = new List<Type>();
            foreach(Type type in types)
            {
                if (!outList.Contains(type))
                {
                    outList.Add(type);
                }
            }
            return outList;
        }
        private static int getQuantityInParentCollection(object childObject, object parentObject)
        {
            TECScope child;
            TECScope parent;
            
            child = (childObject as TECScope);
            parent = (parentObject as TECScope);

            int quantity = 0;
            var childCollection = UtilitiesMethods.GetChildCollection(childObject, parentObject);

            foreach(TECScope item in (IList)childCollection)
            {
                if(item.Guid == child.Guid)
                {
                    quantity++;
                }
            }
            return quantity;
        }
        #endregion
    }
}
