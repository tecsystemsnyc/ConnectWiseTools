using System;
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

            checkAndUpdateDB(typeof(TECBid));
            TECBid bid = new TECBid();

            //Update catalogs from templates.
            if (templates.DeviceCatalog.Count > 0)
            {
                foreach (TECDevice device in templates.DeviceCatalog)
                { editObject(device, bid); }
            }

            if (templates.ManufacturerCatalog.Count > 0)
            {
                foreach (TECManufacturer manufacturer in templates.ManufacturerCatalog)
                { editObject(manufacturer, bid); }
            }

            if (templates.Tags.Count > 0)
            {
                foreach (TECTag tag in templates.Tags)
                { editObject(tag, bid); }
            }

            if(templates.ConnectionTypeCatalog.Count > 0)
            {
                foreach(TECConnectionType connectionType in templates.ConnectionTypeCatalog)
                { editObject(connectionType, bid); }
            }
            if (templates.ConduitTypeCatalog.Count > 0)
            {
                foreach (TECConduitType conduitType in templates.ConduitTypeCatalog)
                { editObject(conduitType, bid); }
            }
            if(templates.AssociatedCostsCatalog.Count > 0)
            {
                foreach(TECAssociatedCost cost in templates.AssociatedCostsCatalog)
                { editObject(cost, bid); }
            }

            bid = getBidInfo();
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
            bid.Connections = getConnections();
            bid.Controllers = getControllers();
            bid.ConnectionTypes = getConnectionTypes();
            bid.ConduitTypes = getConduitTypes();
            bid.AssociatedCostsCatalog = getAssociatedCosts();
            linkAllVisualScope(bid.Drawings, bid.Systems, bid.Controllers);
            linkAllLocations(bid.Locations, bid.Systems);
            linkAllConnections(bid.Connections, bid.Controllers, bid.Systems);
            linkConnectionTypeWithDevices(bid.ConnectionTypes, bid.DeviceCatalog);
            linkAllDevices(bid.Systems, bid.DeviceCatalog);
            linkManufacturersWithDevices(bid.ManufacturerCatalog, bid.DeviceCatalog);
            linkTagsInBid(bid.Tags, bid);
            linkManufacturersWithControllers(bid.ManufacturerCatalog, bid.Controllers);
            linkAssociatedCostsWithScope(bid);
            getUserAdjustments(bid);
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
            templates.SystemTemplates = getAllSystems();
            templates.EquipmentTemplates = getOrphanEquipment();
            templates.SubScopeTemplates = getOrphanSubScope();
            templates.DeviceCatalog = getAllDevices();
            templates.Tags = getAllTags();
            templates.ManufacturerCatalog = getAllManufacturers();
            templates.ControllerTemplates = getControllers();
            linkAllDevicesFromSystems(templates.SystemTemplates, templates.DeviceCatalog);
            linkAllDevicesFromEquipment(templates.EquipmentTemplates, templates.DeviceCatalog);
            linkAllDevicesFromSubScope(templates.SubScopeTemplates, templates.DeviceCatalog);
            linkManufacturersWithDevices(templates.ManufacturerCatalog, templates.DeviceCatalog);
            templates.ConnectionTypeCatalog = getConnectionTypes();
            templates.ConduitTypeCatalog = getConduitTypes();
            templates.AssociatedCostsCatalog = getAssociatedCosts();
            linkConnectionTypeWithDevices(templates.ConnectionTypeCatalog, templates.DeviceCatalog);
            linkTagsInTemplates(templates.Tags, templates);
            linkManufacturersWithControllers(templates.ManufacturerCatalog, templates.ControllerTemplates);
            linkAssociatedCostsWithScope(templates);

            SQLiteDB.Connection.Close();
            return templates;
        }
        static public void SaveBidToNewDB(string path, TECBid bid)
        {
            SQLiteDB = new SQLiteDatabase(path);

            if (File.Exists(path))
            { SQLiteDB.overwriteFile(); }
            createAllBidTables();

            saveCompleteBid(bid);
            SQLiteDB.Connection.Close();

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
        static public void UpdateBidToDB(string path, ChangeStack changeStack)
        {
            createBackup(path);
            string tempPath = Path.GetDirectoryName(path) + @"\" + Path.GetFileNameWithoutExtension(path) + ".tmp";

            File.Copy(path, tempPath);

            SQLiteDB = new SQLiteDatabase(tempPath);

            foreach (Tuple<Change, object, object> change in changeStack.SaveStack)
            {
                Change changeType = change.Item1;
                object targetObject = change.Item3;
                object refObject = change.Item2;

                if (changeType == Change.Add)
                {
                    addUpdate(targetObject, refObject);
                }
                else if (changeType == Change.Edit)
                {
                    editUpdate(targetObject, refObject);
                }
                else if (changeType == Change.Remove)
                {
                    removeUpdate(targetObject, refObject);
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

            foreach (Tuple<Change, object, object> change in changeStack.SaveStack)
            {
                Change changeType = change.Item1;
                object targetObject = change.Item3;
                object refObject = change.Item2;

                if (changeType == Change.Add)
                {
                    addUpdate(targetObject, refObject);
                }
                else if (changeType == Change.Edit)
                {
                    editUpdate(targetObject, refObject);
                }
                else if (changeType == Change.Remove)
                {
                    removeUpdate(targetObject, refObject);
                }
            }

            SQLiteDB.Connection.Close();

            GC.Collect();
            GC.WaitForPendingFinalizers();

            File.Copy(tempPath, path, true);

            File.Delete(tempPath);
        }
        #endregion Public Functions

        #region Update Functions
        static private void addUpdate(object tarObject, object refObject)
        { addObject(tarObject, refObject); }
        static private void removeUpdate(object tarObject, object refObject)
        { removeObject(tarObject, refObject); }
        static private void editUpdate(object tarObject, object refObject)
        { editObject(tarObject, refObject); }
        #endregion Update Functions
            
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
            labor.ElectricalSuperRate = subContractRow[SubcontractorConstantsTable.ElectricalSuperRate.Name].ToString().ToDouble(0);

            return labor;
        }

        static private TECLabor getLaborConstsInTemplates(TECTemplates templates)
        {
            TECLabor labor = new TECLabor();

            string constsCommand = "select * from (" + LaborConstantsTable.TableName + " inner join ";
            constsCommand += TemplatesLaborTable.TableName + " on ";
            constsCommand += "(TECLaborConst.LaborID = TECTemplatesTECLabor.LaborID";
            constsCommand += " and " + TemplatesLaborTable.TemplatesID.Name + " = '";
            constsCommand += templates.Guid;
            constsCommand += "'))";

            DataTable laborDT = SQLiteDB.getDataFromCommand(constsCommand);

            if (laborDT.Rows.Count > 1)
            {
                DebugHandler.LogError("Multiple rows found in labor constants table. Using first found.");
            }
            else if (laborDT.Rows.Count < 1)
            {
                DebugHandler.LogError("Labor constants not found in database, using default values. Reload labor constants from loaded templates in the labor tab.");
                return labor;
            }

            DataRow laborRow = laborDT.Rows[0];

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
            subConstsCommand += TemplatesLaborTable.TableName + " on ";
            subConstsCommand += "(TECSubcontractorConst.LaborID = TECTemplatesTECLabor.LaborID";
            subConstsCommand += " and " + TemplatesLaborTable.TemplatesID.Name + " = '";
            subConstsCommand += templates.Guid;
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
            labor.ElectricalSuperRate = subContractRow[SubcontractorConstantsTable.ElectricalSuperRate.Name].ToString().ToDouble(0);

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
            { return new TECManufacturer(); }
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
            { return new TECConnectionType(); }
        }
        static private TECConduitType getConduitTypeInSubScope(Guid subScopeID)
        {
            string command = "select * from " + ConduitTypeTable.TableName + " where " + ConduitTypeTable.ConduitTypeID.Name + " in ";
            command += "(select " + SubScopeConduitTypeTable.TypeID.Name + " from " + SubScopeConduitTypeTable.TableName + " where ";
            command += SubScopeConduitTypeTable.SubScopeID.Name + " = '" + subScopeID;
            command += "')";

            DataTable conduitTypeTable = SQLiteDB.getDataFromCommand(command);
            if (conduitTypeTable.Rows.Count > 0)
            { return (getConduitTypeFromRow(conduitTypeTable.Rows[0])); }
            else
            { return new TECConduitType(); }
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
            string command = "select * from " + ControllerIOTypeTable.TableName + " where ";
            command += ControllerIOTypeTable.ControllerID.Name + " = '" + controllerID + "'";
            
                DataTable typeDT = SQLiteDB.getDataFromCommand(command);
                foreach (DataRow row in typeDT.Rows)
                {  outIO.Add(getIOFromRow(row)); }
            return outIO;
        }
        static private ObservableCollection<TECConnection> getConnections()
        {
            ObservableCollection<TECConnection> connections = new ObservableCollection<TECConnection>(); 
            
                DataTable connectionDT = SQLiteDB.getDataFromTable(ConnectionTable.TableName); 
                foreach (DataRow row in connectionDT.Rows)
                { connections.Add(getConnectionFromRow(row)); }
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
            { return new TECManufacturer(); }
        }
        #endregion //Loading from DB Methods

        #region Link Methods
        static private void linkAllVisualScope(ObservableCollection<TECDrawing> bidDrawings, ObservableCollection<TECSystem> bidSystems, ObservableCollection<TECController> bidControllers)
        {
            //This function links visual scope with scope in Systems, Equipment, SubScope and Devices if they have the same GUID.

            Dictionary<TECVisualScope, Guid> scopeToLink = new Dictionary<TECVisualScope, Guid>();

            foreach (TECDrawing drawing in bidDrawings)
            {
                foreach (TECPage page in drawing.Pages)
                {
                    foreach (TECVisualScope vs in page.PageScope)
                    {
                        string command = "select ScopeID from TECVisualScopeTECScope where VisualScopeID = '" + vs.Guid + "'";
                        
                            DataTable scopeID = SQLiteDB.getDataFromCommand(command);
                            Guid scopeGuid = new Guid(scopeID.Rows[0][0].ToString());
                            scopeToLink.Add(vs, scopeGuid);
                        
                    }
                }
            }

            foreach (TECController controller in bidControllers)
            {
                //Check scope in systems.
                List<TECVisualScope> scopeToRemove = new List<TECVisualScope>();
                foreach (KeyValuePair<TECVisualScope, Guid> vs in scopeToLink)
                {
                    if (vs.Value == controller.Guid)
                    {
                        vs.Key.Scope = controller;
                        scopeToRemove.Add(vs.Key);
                    }
                }
                foreach (TECVisualScope scope in scopeToRemove)
                { scopeToLink.Remove(scope);  }
                if (scopeToLink.Count < 1)
                { return; }
                scopeToRemove.Clear();
            }

            foreach (TECSystem system in bidSystems)
            {
                //Check scope in systems.
                List<TECVisualScope> scopeToRemove = new List<TECVisualScope>();
                foreach (KeyValuePair<TECVisualScope, Guid> vs in scopeToLink)
                {
                    if (vs.Value == system.Guid)
                    {
                        vs.Key.Scope = system;
                        scopeToRemove.Add(vs.Key);
                    }
                }
                foreach (TECVisualScope scope in scopeToRemove)
                { scopeToLink.Remove(scope); }
                if (scopeToLink.Count < 1)
                { return; }
                scopeToRemove.Clear();
                
                foreach (TECEquipment equip in system.Equipment)
                {
                    //Check scope in equipment.
                    foreach (KeyValuePair<TECVisualScope, Guid> vs in scopeToLink)
                    {
                        if (vs.Value == equip.Guid)
                        {
                            vs.Key.Scope = equip;
                            scopeToRemove.Add(vs.Key);
                        }
                    }
                    foreach (TECVisualScope scope in scopeToRemove)
                    { scopeToLink.Remove(scope); }
                    if (scopeToLink.Count < 1)
                    { return; }
                    scopeToRemove.Clear();

                    foreach (TECSubScope ss in equip.SubScope)
                    {
                        //Check scope in subScope.
                        foreach (KeyValuePair<TECVisualScope, Guid> vs in scopeToLink)
                        {
                            if (vs.Value == ss.Guid)
                            {
                                vs.Key.Scope = ss;
                                scopeToRemove.Add(vs.Key);
                            }
                        }
                        foreach (TECVisualScope scope in scopeToRemove)
                        {  scopeToLink.Remove(scope); }
                        if (scopeToLink.Count < 1)
                        { return; }
                        scopeToRemove.Clear();

                        foreach (TECDevice dev in ss.Devices)
                        {
                            //Check scope in devices.
                            foreach (KeyValuePair<TECVisualScope, Guid> vs in scopeToLink)
                            {
                                if (vs.Value == dev.Guid)
                                {
                                    vs.Key.Scope = dev;
                                    scopeToRemove.Add(vs.Key);
                                }
                            }
                            foreach (TECVisualScope scope in scopeToRemove)
                            { scopeToLink.Remove(scope); }
                            if (scopeToLink.Count < 1)
                            { return; }
                            scopeToRemove.Clear();
                        }
                    }
                }
            }
        }
        static private void linkAllLocations(ObservableCollection<TECLocation> locations, ObservableCollection<TECSystem> bidSystems)
        {
            foreach(TECLocation location in locations)
            {
                foreach (TECSystem system in bidSystems)
                {
                    if (system.Location != null && system.Location.Guid == location.Guid)
                    { system.Location = location; }
                    foreach(TECEquipment equipment in system.Equipment)
                    {
                        if (equipment.Location != null && equipment.Location.Guid == location.Guid)
                        { equipment.Location = location; }
                        foreach(TECSubScope subScope in equipment.SubScope)
                        {
                            if(subScope.Location != null && subScope.Location.Guid == location.Guid)
                            { subScope.Location = location; }
                        }
                   }
                }
            }
        }
        static private void linkAllConnections(ObservableCollection<TECConnection> connections, ObservableCollection<TECController> controllers, ObservableCollection<TECSystem> bidSystems)
        {
            //Construct guid relations
            List<Tuple<TECConnection, Guid, List<Guid>>> toConnect = new List<Tuple<TECConnection, Guid, List<Guid>>>();
            foreach (TECConnection conn in connections)
            {
                string command = "select * from " + ControllerConnectionTable.TableName + " where " + ControllerConnectionTable.ConnectionID.Name + " = '" + conn.Guid.ToString() + "'";
                DataTable guidDT = SQLiteDB.getDataFromCommand(command);
                Guid parentGuid = new Guid(guidDT.Rows[0][ControllerConnectionTable.ControllerID.Name].ToString());

                command = "select * from " + ScopeConnectionTable.TableName + " where " + ScopeConnectionTable.ConnectionID.Name + " = '" + conn.Guid.ToString() + "'";
                guidDT = SQLiteDB.getDataFromCommand(command);
                List<Guid> childGuids = new List<Guid>();
                foreach (DataRow row in guidDT.Rows)
                { childGuids.Add(new Guid(row[ScopeConnectionTable.ScopeID.Name].ToString())); }

                toConnect.Add(new Tuple<TECConnection, Guid, List<Guid>>(conn, parentGuid, childGuids));
            }

            //Construct potential TECScope List
            List<TECScope> scopeToLink = new List<TECScope>();
            foreach (TECController controller in controllers)
            { scopeToLink.Add(controller); }
            foreach (TECSystem system in bidSystems)
            {
                foreach (TECEquipment equip in system.Equipment)
                {
                    foreach (TECSubScope ss in equip.SubScope)
                    { scopeToLink.Add(ss); }
                }
            }

            //Link
            foreach (Tuple<TECConnection, Guid, List<Guid>> item in toConnect)
            {
                TECConnection connection = item.Item1;
                Guid parentGuid = item.Item2;
                List<Guid> childGuids = item.Item3;

                //Link Parent Controller
                foreach (TECController controller in controllers)
                {
                    if (controller.Guid == parentGuid)
                    {
                        connection.Controller = controller;
                        controller.Connections.Add(connection);
                        break;
                    }
                }

                //Link Children Scope
                foreach (Guid child in childGuids)
                {
                    TECScope scopeToRemove = null;
                    foreach (TECScope scope in scopeToLink)
                    {
                        if (scope.Guid == child)
                        {
                            if (scope is TECSubScope)
                            { (scope as TECSubScope).Connection = connection; }
                            else if (scope is TECController)
                            { (scope as TECController).Connections.Add(connection);  }
                            connection.Scope.Add(scope);
                            scopeToRemove = scope;
                            break;
                        }
                    }
                    if (scopeToRemove != null)
                    { scopeToLink.Remove(scopeToRemove); }
                }
            }
        }
        static private void linkAllDevices(ObservableCollection<TECSystem> bidSystems, ObservableCollection<TECDevice> deviceCatalog)
        {
            foreach (TECSystem system in bidSystems)
            {
                foreach (TECEquipment equipment in system.Equipment)
                {
                    foreach (TECSubScope sub in equipment.SubScope)
                    {
                        var linkedDevices = new ObservableCollection<TECDevice>();
                        foreach (TECDevice device in sub.Devices)
                        {
                            foreach (TECDevice cDev in deviceCatalog)
                            {
                                if (cDev.Guid == device.Guid)
                                { linkedDevices.Add(cDev); }
                            }
                        }
                        sub.Devices = linkedDevices;
                    }
                }
            }
        }
        static private void linkAllDevicesFromSystems(ObservableCollection<TECSystem> systems, ObservableCollection<TECDevice> deviceCatalog)
        {
            foreach (TECSystem system in systems)
            { linkAllDevicesFromEquipment(system.Equipment, deviceCatalog); }
        }
        static private void linkAllDevicesFromEquipment(ObservableCollection<TECEquipment> equipment, ObservableCollection<TECDevice> deviceCatalog)
        {
            foreach (TECEquipment equip in equipment)
            { linkAllDevicesFromSubScope(equip.SubScope, deviceCatalog); }
        }
        static private void linkAllDevicesFromSubScope(ObservableCollection<TECSubScope> subScope, ObservableCollection<TECDevice> deviceCatalog)
        {
            foreach (TECSubScope sub in subScope)
            {
                var linkedDevices = new ObservableCollection<TECDevice>();
                foreach (TECDevice device in sub.Devices)
                {
                    foreach (TECDevice cDev in deviceCatalog)
                    {
                        if (cDev.Guid == device.Guid)
                        { linkedDevices.Add(cDev); }
                    }
                }
                sub.Devices = linkedDevices;
            }
        }
        static private void linkManufacturersWithDevices(ObservableCollection<TECManufacturer> mans, ObservableCollection<TECDevice> devices)
        {
            foreach (TECDevice device in devices)
            {
                foreach (TECManufacturer man in mans)
                {
                    if (device.Manufacturer.Guid == man.Guid)
                    { device.Manufacturer = man; }
                }

            }
        }
        static private void linkTagsInBid(ObservableCollection<TECTag> tags, TECBid bid)
        {
            foreach (TECSystem system in bid.Systems)
            {
                linkTags(tags, system);
                foreach (TECEquipment equipment in system.Equipment)
                {
                    linkTags(tags, equipment);
                    foreach (TECSubScope subScope in equipment.SubScope)
                    {
                        linkTags(tags, subScope);
                        foreach (TECDevice device in subScope.Devices)
                        { linkTags(tags, device); }
                        foreach (TECPoint point in subScope.Points)
                        { linkTags(tags, point); }
                    }
                }
            }
            foreach (TECController controller in bid.Controllers)
            { linkTags(tags, controller);}
            foreach (TECDevice device in bid.DeviceCatalog)
            { linkTags(tags, device); }
        }
        static private void linkTagsInTemplates(ObservableCollection<TECTag> tags, TECTemplates templates)
        {
            foreach (TECSystem system in templates.SystemTemplates)
            {
                linkTags(tags, system);
                foreach (TECEquipment equipment in system.Equipment)
                {
                    linkTags(tags, equipment);
                    foreach (TECSubScope subScope in equipment.SubScope)
                    {
                        linkTags(tags, subScope);
                        foreach (TECDevice device in subScope.Devices)
                        { linkTags(tags, device); }
                        foreach (TECPoint point in subScope.Points)
                        { linkTags(tags, point); }
                    }
                }
            }
            foreach (TECEquipment equipment in templates.EquipmentTemplates)
            {
                linkTags(tags, equipment);
                foreach (TECSubScope subScope in equipment.SubScope)
                {
                    linkTags(tags, subScope);
                    foreach (TECDevice device in subScope.Devices)
                    { linkTags(tags, device); }
                    foreach (TECPoint point in subScope.Points)
                    { linkTags(tags, point); }
                }
            }
            foreach (TECSubScope subScope in templates.SubScopeTemplates)
            {
                linkTags(tags, subScope);
                foreach (TECDevice device in subScope.Devices)
                { linkTags(tags, device); }
                foreach (TECPoint point in subScope.Points)
                { linkTags(tags, point); }
            }
            foreach (TECController controller in templates.ControllerTemplates)
            { linkTags(tags, controller); }
            foreach (TECDevice device in templates.DeviceCatalog)
            { linkTags(tags, device); }
        }
        static private void linkTags(ObservableCollection<TECTag> tags, TECScope scope)
        {
            ObservableCollection<TECTag> linkedTags = new ObservableCollection<TECTag>();
            foreach (TECTag tag in scope.Tags)
            {
                foreach (TECTag referenceTag in tags)
                {
                    if (tag.Guid == referenceTag.Guid)
                    { linkedTags.Add(referenceTag); }
                }
            }
            scope.Tags = linkedTags;
        }
        static private void linkConnectionTypeWithDevices(ObservableCollection<TECConnectionType> connectionTypes, ObservableCollection<TECDevice> devices)
        {
            foreach (TECDevice device in devices)
            {
                foreach (TECConnectionType connectionType in connectionTypes)
                {
                    if (device.ConnectionType.Guid == connectionType.Guid)
                    { device.ConnectionType = connectionType; }
                }
            }
        }
        static private void linkManufacturersWithControllers(ObservableCollection<TECManufacturer> mans, ObservableCollection<TECController> controllers)
        {
            foreach(TECManufacturer manufacturer in mans)
            {
                foreach(TECController controller in controllers)
                {
                    if(controller.Manufacturer.Guid == manufacturer.Guid)
                    { controller.Manufacturer = manufacturer; }
                }
            }
        }
        static private void linkAssociatedCostsWithScope(Object bidOrTemp)
        {
            if(bidOrTemp is TECBid)
            {
                TECBid bid = bidOrTemp as TECBid;
                foreach(TECSystem system in bid.Systems)
                { linkAssociatedCostsInSystem(bid.AssociatedCostsCatalog, system); }
            } else if (bidOrTemp is TECTemplates)
            {
                TECTemplates templates = bidOrTemp as TECTemplates;
                foreach (TECSystem system in templates.SystemTemplates)
                { linkAssociatedCostsInSystem(templates.AssociatedCostsCatalog, system); }
                foreach (TECEquipment equipment in templates.EquipmentTemplates)
                { linkAssociatedCostsInEquipment(templates.AssociatedCostsCatalog, equipment); }
                foreach (TECSubScope subScope in templates.SubScopeTemplates)
                { linkAssociatedCostsInSubScope(templates.AssociatedCostsCatalog, subScope); }
                foreach (TECDevice device in templates.DeviceCatalog)
                { linkAssociatedCostsInDevice(templates.AssociatedCostsCatalog, device); }
            }

        }
        static private void linkAssociatedCostsInDevice(ObservableCollection<TECAssociatedCost> costs, TECDevice device)
        {
            ObservableCollection<TECAssociatedCost> costsToAssign = new ObservableCollection<TECAssociatedCost>();
            foreach (TECAssociatedCost cost in costs)
            {
                foreach(TECAssociatedCost devCost in device.AssociatedCosts)
                {
                    if(devCost.Guid == cost.Guid)
                    { costsToAssign.Add(cost); }
                }
            }
            device.AssociatedCosts = costsToAssign;
        }
        static private void linkAssociatedCostsInSubScope(ObservableCollection<TECAssociatedCost> costs, TECSubScope subScope)
        {
            ObservableCollection<TECAssociatedCost> costsToAssign = new ObservableCollection<TECAssociatedCost>();
            foreach (TECAssociatedCost cost in costs)
            {
                foreach (TECAssociatedCost childCost in subScope.AssociatedCosts)
                {
                    if (childCost.Guid == cost.Guid)
                    { costsToAssign.Add(cost); }
                }
            }
            subScope.AssociatedCosts = costsToAssign;
            foreach(TECDevice device in subScope.Devices)
            { linkAssociatedCostsInDevice(costs, device); }
        }
        static private void linkAssociatedCostsInEquipment(ObservableCollection<TECAssociatedCost> costs, TECEquipment equipment)
        {
            ObservableCollection<TECAssociatedCost> costsToAssign = new ObservableCollection<TECAssociatedCost>();
            foreach (TECAssociatedCost cost in costs)
            {
                foreach (TECAssociatedCost childCost in equipment.AssociatedCosts)
                {
                    if (childCost.Guid == cost.Guid)
                    { costsToAssign.Add(cost); }
                }
            }
            equipment.AssociatedCosts = costsToAssign;
            foreach (TECSubScope subScope in equipment.SubScope)
            { linkAssociatedCostsInSubScope(costs, subScope); }
        }
        static private void linkAssociatedCostsInSystem(ObservableCollection<TECAssociatedCost> costs, TECSystem system)
        {
            ObservableCollection<TECAssociatedCost> costsToAssign = new ObservableCollection<TECAssociatedCost>();
            foreach (TECAssociatedCost cost in costs)
            {
                foreach (TECAssociatedCost childCost in system.AssociatedCosts)
                {
                    if (childCost.Guid == cost.Guid)
                    { costsToAssign.Add(cost); }
                }
            }
            system.AssociatedCosts = costsToAssign;
            foreach (TECEquipment equipment in system.Equipment)
            { linkAssociatedCostsInEquipment(costs, equipment); }
        }
        #endregion Link Methods

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
                        foreach (TECConnection connection in connectionsToAdd)
                        {
                            foreach (TECVisualScope vController in vControllers)
                            {
                                if ((connection.Controller == vController.Scope) && (connection.Scope.Contains(item.Item1)))
                                {
                                    ObservableCollection<TECConnection> childConnections = new ObservableCollection<TECConnection>();
                                    childConnections.Add(connection);
                                    TECVisualConnection visConnection = new TECVisualConnection(vController, item.Item2);
                                    vConnectionsToAdd.Add(visConnection);
                                }
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
            { infoDT = SQLiteDB.getDataFromTable(TemplatesInfoTable.TableName); }
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
                    killTemplatesInfo();
                    return false;
                }
                else
                { return false; }
            }
            else
            {
                DataRow infoRow = infoDT.Rows[0];
                if (infoDT.Columns.Contains(BidInfoTable.DBVersion.Name) || infoDT.Columns.Contains(TemplatesInfoTable.DBVersion.Name))
                {
                    string version = infoRow["DBVersion"].ToString();
                    return (version == currentVersion);
                }
                else
                { return false; }
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
            List<TableField> primaryKey = tableInfo.PrimaryFields;
            List<TableField> fields = tableInfo.Fields;

            List<string> currentFields = getAllTableFields(tableName);
            List<string> commonFields = new List<string>();
            foreach (TableField field in fields)
            {
                if (currentFields.Contains(field.Name))
                { commonFields.Add(field.Name); }
            }

            string commonString = UtilitiesMethods.CommaSeparatedString(commonFields);
            createTempTableFromDefinition(table);

            string commandString;
            if (commonFields.Count > 0)
            {
                commandString = "insert or ignore into '" + tempName + "' (" + commonString + ") select " + commonString + " from '" + tableName + "'";
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
            string commandString;
            if (type == typeof(TECBid) || type == typeof(TECTemplates))
            {
                Dictionary<string, string> Data = new Dictionary<string, string>();
                if (type == typeof(TECBid))
                {
                    var infoBid = getBidInfo();
                    commandString = "update " + BidInfoTable.TableName + " set " + BidInfoTable.DBVersion.Name + " = '" + Properties.Settings.Default.Version + "' ";
                    commandString += "where " + BidInfoTable.BidID.Name + " = '" + infoBid.Guid.ToString() + "'";
                    SQLiteDB.nonQueryCommand(commandString);
                }
                else if(type == typeof(TECTemplates))
                {
                    var templateGuid = getTemplatesInfo().Guid;
                    commandString = "update " + TemplatesInfoTable.TableName + " set " + TemplatesInfoTable.DBVersion.Name + " = '" + Properties.Settings.Default.Version + "' ";
                    commandString += "where " + TemplatesInfoTable.TemplateID.Name + " = '" + templateGuid.ToString() + "'";
                    SQLiteDB.nonQueryCommand(commandString);
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
        private static TECSystem getSystemFromRow(DataRow row)
        {
            Guid systemID = new Guid(row[SystemTable.SystemID.Name].ToString());
            TECSystem system = new TECSystem(systemID);
            system.Name = row[SystemTable.Name.Name].ToString();
            system.Description = row[SystemTable.Description.Name].ToString();
            system.Quantity = row[SystemTable.Quantity.Name].ToString().ToInt();
            system.BudgetPrice = row[SystemTable.BudgetPrice.Name].ToString().ToDouble();
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
            equipmentToAdd.BudgetPrice = row[EquipmentTable.BudgetPrice.Name].ToString().ToDouble();
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
            subScopeToAdd.Length = row[SubScopeTable.Length.Name].ToString().ToDouble(0);
            subScopeToAdd.Devices = getDevicesInSubScope(subScopeID);
            subScopeToAdd.Points = getPointsInSubScope(subScopeID);
            subScopeToAdd.Location = getLocationInScope(subScopeID);
            subScopeToAdd.Tags = getTagsInScope(subScopeID);
            subScopeToAdd.ConduitType = getConduitTypeInSubScope(subScopeID);
            subScopeToAdd.AssociatedCosts = getAssociatedCostsInScope(subScopeID);
            return subScopeToAdd;
        }
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

            var associatedCost = new TECAssociatedCost(guid);
            associatedCost.Name = name;
            associatedCost.Cost = cost;

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
        private static TECTag getTagFromRow(DataRow row)
        {
            var tag = new TECTag(new Guid(row["TagID"].ToString()));
            tag.Text = row["TagString"].ToString();
            return tag;
        }
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
            return visualScope;
        }
        private static TECController getControllerFromRow(DataRow row)
        {
            Guid guid = new Guid(row[ControllerTable.ControllerID.Name].ToString());
            TECController controller = new TECController(guid);

            controller.Name = row[ControllerTable.Name.Name].ToString();
            controller.Description = row[ControllerTable.Description.Name].ToString();
            controller.Cost = row[ControllerTable.Cost.Name].ToString().ToDouble();
            controller.IO = getIOInController(guid);
            controller.Tags = getTagsInScope(guid);
            controller.Manufacturer = getManufacturerInController(guid);
            controller.AssociatedCosts = getAssociatedCostsInScope(guid);
            return controller;
        }
        private static TECIO getIOFromRow(DataRow row)
        {
            IOType type = TECIO.convertStringToType(row[ControllerIOTypeTable.IOType.Name].ToString());
            var io = new TECIO();
            io.Type = type;
            io.Quantity = row[ControllerIOTypeTable.Quantity.Name].ToString().ToInt();
            return io;
        }
        private static TECConnection getConnectionFromRow(DataRow row)
        {
            Guid guid = new Guid(row[ConnectionTable.ConnectionID.Name].ToString());
            TECConnection connection = new TECConnection(guid);

            connection.Length = row[ConnectionTable.Length.Name].ToString().ToDouble();
            return connection;
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
            addObject(bid, bid);
            addObject(bid.Labor, bid);
            addObject(bid.Parameters, bid);
            foreach(TECSystem system in bid.Systems)
            {
                addObject(system, bid);
                saveScopeChildProperties(system);
                saveCompleteEquipment(system);
            }
            foreach(TECManufacturer manufacturer in bid.ManufacturerCatalog)
            { addObject(manufacturer, bid); }
            foreach(TECController controller in bid.Controllers)
            {
                addObject(controller, bid);
                saveScopeChildProperties(controller);
                saveControllerChildProperties(controller);
            }
            foreach(TECConnection connection in bid.Connections)
            { addObject(connection, bid); }
            foreach(TECAssociatedCost associatedCost in bid.AssociatedCostsCatalog)
            { addObject(associatedCost, bid); }
            foreach (TECNote note in bid.Notes)
            { addObject(note, bid); }
            foreach (TECExclusion exclusion in bid.Exclusions)
            { addObject(exclusion, bid); }
            foreach (TECLocation location in bid.Locations)
            { addObject(location, bid); }
            foreach (TECConduitType conduitType in bid.ConduitTypes)
            {
                addObject(conduitType, bid);
                saveScopeChildProperties(conduitType);
            }
            foreach (TECConnectionType connectionType in bid.ConnectionTypes)
            {
                addObject(connectionType, bid);
                saveScopeChildProperties(connectionType);
            }
            foreach (TECTag tag in bid.Tags)
            { addObject(tag, bid); }
            foreach (TECScopeBranch branch in bid.ScopeTree)
            {
                addObject(branch, bid);
                saveCompleteScopeBranch(branch);
            }
            foreach(TECDrawing drawing in bid.Drawings)
            {
                addObject(drawing, bid);
                saveCompletePage(drawing);
            }
            foreach(TECProposalScope proposalScope in bid.ProposalScope)
            {
                addObject(proposalScope, bid);
                saveCompleteProposalScope(proposalScope);
            }
            foreach(TECDevice device in bid.DeviceCatalog)
            {
                addObject(device, bid);
                saveScopeChildProperties(device);
                saveDeviceChildProperties(device);
            }
        }
        private static void saveCompleteTemplate(TECTemplates templates)
        {
            addObject(templates, templates);
            foreach(TECTag tag in templates.Tags)
            { addObject(tag, templates); }
            foreach (TECSystem system in templates.SystemTemplates)
            {
                addObject(system, templates);
                saveScopeChildProperties(system);
                saveCompleteEquipment(system);
            }
            foreach (TECEquipment equipment in templates.EquipmentTemplates)
            {
                addObject(equipment, templates);
                saveScopeChildProperties(equipment);
                saveCompleteSubScope(equipment);
            }
            foreach (TECSubScope subScope in templates.SubScopeTemplates)
            {
                addObject(subScope, templates);
                if (subScope.ConduitType != null) { addObject(subScope.ConduitType, subScope); }
                saveScopeChildProperties(subScope);
                saveDevicesInSubScope(subScope);
                saveCompletePoints(subScope);
            }
            foreach (TECDevice device in templates.DeviceCatalog)
            {
                saveDeviceInCatalog(device, templates);
            }
            foreach (TECManufacturer manufacturer in templates.ManufacturerCatalog)
            { addObject(manufacturer, templates); }
            foreach (TECController controller in templates.ControllerTemplates)
            {
                addObject(controller, templates);
                saveScopeChildProperties(controller);
                saveControllerChildProperties(controller);
            }
            foreach (TECConduitType conduitType in templates.ConduitTypeCatalog)
            {
                addObject(conduitType, templates);
                saveAssociatedCosts(conduitType);
            }
            foreach (TECConnectionType connectionType in templates.ConnectionTypeCatalog)
            {
                addObject(connectionType, templates);
                saveAssociatedCosts(connectionType);
            }
            foreach (TECAssociatedCost associatedCost in templates.AssociatedCostsCatalog)
            { addObject(associatedCost, templates); }
        }

        private static void saveDevicesInSubScope(TECSubScope subscope)
        {
            foreach(TECDevice device in subscope.Devices)
            { addObject(device, subscope); }
        }
        private static void saveDeviceInCatalog(TECDevice device, object bidOrTemplates)
        {
            if(bidOrTemplates is TECBid || bidOrTemplates is TECTemplates)
            {
                addObject(device, bidOrTemplates);
                saveScopeChildProperties(device);
                saveDeviceChildProperties(device);
            }
        }
        private static void saveCompletePoints(TECSubScope subScope)
        {
            foreach(TECPoint point in subScope.Points)
            {
                addObject(point, subScope);
                saveScopeChildProperties(point);
            }
        }
        private static void saveCompleteSubScope(TECEquipment equipment)
        {
            foreach(TECSubScope subScope in equipment.SubScope)
            {
                addObject(subScope, equipment);
                if(subScope.ConduitType != null) { addObject(subScope.ConduitType, subScope); }
                saveScopeChildProperties(subScope);
                saveDevicesInSubScope(subScope);
                saveCompletePoints(subScope);
            }
        }
        private static void saveCompleteEquipment(TECSystem system)
        {
            foreach(TECEquipment equipment in system.Equipment)
            {
                addObject(equipment, system);
                saveScopeChildProperties(equipment);
                saveCompleteSubScope(equipment);
            }
        }
        private static void saveCompleteScopeBranch(TECScopeBranch branch)
        {
            foreach(TECScopeBranch subBranch in branch.Branches)
            {
                addObject(subBranch, branch);
                saveCompleteScopeBranch(subBranch);
            }
        }
        private static void saveCompleteVisualScope(TECPage page)
        {
            foreach(TECVisualScope visualScope in page.PageScope)
            { addObject(visualScope, page); }
        }
        private static void saveCompleteVisualConnections(TECPage page)
        {
            foreach (TECVisualConnection visualConnection in page.Connections)
            { addObject(visualConnection, page); }
        }
        private static void saveCompletePage(TECDrawing drawing)
        {
            foreach(TECPage page in drawing.Pages)
            {
                addObject(page, drawing);
                saveCompleteVisualScope(page);
                saveCompleteVisualConnections(page);
            }
        }
        private static void saveCompleteProposalScope(TECProposalScope proposalScope)
        {
            
            foreach(TECProposalScope subProposalScope in proposalScope.Children)
            {
                addObject(subProposalScope, proposalScope);
                saveCompleteProposalScope(subProposalScope);
            }
            foreach(TECScopeBranch branch in proposalScope.Notes)
            {
                addObject(branch, proposalScope);
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
                addObject(scope.Location, scope);
            }
        }
        private static void saveTags(TECScope scope)
        {
            foreach(TECTag tag in scope.Tags)
            {
                addObject(tag, scope);
            }
        }
        private static void saveAssociatedCosts(TECScope scope)
        {
            foreach(TECAssociatedCost cost in scope.AssociatedCosts)
            {
                addObject(cost, scope);
            }
        }
        private static void saveControllerChildProperties(TECController controller)
        {
            if(controller.Manufacturer != null) { addObject(controller.Manufacturer, controller); }
            foreach(TECConnection connection in controller.Connections)
            {  addObject(connection, controller); }
            foreach(TECIO IO in controller.IO)
            { addObject(IO, controller); }
        }
        private static void saveDeviceChildProperties(TECDevice device)
        {
            if(device.Manufacturer != null) { addObject(device.Manufacturer, device); }
            if(device.ConnectionType != null) { addObject(device.ConnectionType, device); }
        }
        #endregion

        #region Generic Add Methods
        private static void addObject(params Object[] objectsToAdd)
        {
            //ObjectsToAdd = [targetObject, referenceObject];
            var relevantTables = getRelevantTablesForAddRemove(objectsToAdd);
            foreach (TableBase table in relevantTables)
            {
                var tableInfo = new TableInfo(table);
                if (tableInfo.IsRelationTable)
                { updateIndexedRelation(table, objectsToAdd); }
                else
                { addObjectToTable(table, objectsToAdd); } 
            } 
        }
        private static void addObjectToTable(TableBase table, params Object[] objectsToAdd)
        {
            //ObjectsToAdd = [targetObject, referenceObject];
            var tableInfo = new TableInfo(table);
            var relevantObjects = objectsToAdd;
            
            Dictionary<string, string> data = assembleDataToAddRemove(table, objectsToAdd);

            if (data.Count > 0)
            {
                if (!SQLiteDB.Insert(tableInfo.Name, data))
                {
                    DebugHandler.LogError("Error: Couldn't add data to " + tableInfo.Name + " table.");
                }
            }
        }
        private static void updateIndexedRelation(TableBase table, params Object[] objectsToAdd)
        {
            var tableInfo = new TableInfo(table);

            var childrenCollection = getChildCollection(objectsToAdd[0], objectsToAdd[1]);
            
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
                        var dataString = objectToDBString(getQuantityInParentCollection(objectsToAdd[0], objectsToAdd[1]));
                        data.Add(field.Name, dataString);
                    }
                    assembleDataWithObjects(data, objectsToAdd, tableInfo, field);
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
        #endregion

        #region Generic Remove Methods
        private static void removeObject(params Object[] objectsToRemove)
        {
            var relevantTables = getRelevantTablesForAddRemove(objectsToRemove);
            foreach (TableBase table in relevantTables)
            {
                var tableInfo = new TableInfo(table);
                removeObjectFromTable(table, objectsToRemove);
            }

        }
        private static void removeObjectFromTable(TableBase table, params Object[] objectsToRemove)
        {
            var tableInfo = new TableInfo(table);
            if (fieldsIncludeQuantity(tableInfo.Fields))
            {
                var qty = getQuantityInParentCollection(objectsToRemove[0], objectsToRemove[1]);
                if (qty > 1)
                {
                    editObjectInTable(table, objectsToRemove);
                    return;
                }
            }

            Dictionary<string, string> data = assembleDataToAddRemove(table, objectsToRemove);
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
        #endregion

        #region Generic Edit Methods
        private static void editObject(params Object[] objectsToEdit)
        {
            var relevantTables = getRelevantTablesToEdit(objectsToEdit);
            foreach (TableBase table in relevantTables)
            {
                var tableInfo = new TableInfo(table);
                editObjectInTable(table, objectsToEdit);
            }
        }
        private static void editObjectInTable(TableBase table, params Object[] objectsToEdit)
        {
            var tableInfo = new TableInfo(table);
            var relevantObjects = objectsToEdit;
            Dictionary<string, string> data = new Dictionary<string, string>();
            if(objectsToEdit.Length == 2)
            {
                if(objectsToEdit[0].GetType() == objectsToEdit[1].GetType())
                {
                    relevantObjects = new Object[]
                    { objectsToEdit[0] };
                }
            }
            foreach (TableField field in tableInfo.Fields)
            {
                if ((field.Property != null) && (field.Property.Name == "Quantity" && field.Property.ReflectedType == typeof(HelperProperties)))
                {
                    var dataString = objectToDBString(getQuantityInParentCollection(objectsToEdit[0], objectsToEdit[1]));
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
        private static List<TableBase> getRelevantTablesToEdit(params Object[] objectsToEdit)
        {
            var relevantTables = new List<TableBase>();
            var objectTypes = getObjectTypes(objectsToEdit);

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
        private static Dictionary<string, string> assembleDataToAddRemove(TableBase table, params Object[] inputObjects)
        {
            var tableInfo = new TableInfo(table);
            var relevantObjects = inputObjects;
            Dictionary<string, string> data = new Dictionary<string, string>();

            var isHierarchial = false;
            if (tableInfo.Types.Count == 2 && tableInfo.Types[0] == tableInfo.Types[1])
            {
                isHierarchial = true;
                relevantObjects = new object[]
                { inputObjects[1],
                    inputObjects[0] };
            }
            else if (tableInfo.Types.Count == 1)
            {
                relevantObjects = new object[]
                { inputObjects[0] };
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
                        var dataString = objectToDBString(getQuantityInParentCollection(inputObjects[0], inputObjects[1]));
                        data.Add(field.Name, dataString);
                    } else if (field.Property.Name == "Version" && field.Property.ReflectedType == typeof(HelperProperties))
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
        private static List<TableBase> getRelevantTablesForAddRemove(params Object[] relevantObjects)
        {
            var relevantTables = new List<TableBase>();
            var objectTypes = getObjectTypes(relevantObjects);

            foreach (TableBase table in AllTables.Tables)
            {
                var tableInfo = new TableInfo(table);

                //TableInfo.Item4 = List<TableType>
                bool allTypesMatch = sharesAllTypes(objectTypes, tableInfo.Types);
                bool tableHasOnlyType = hasOnlyType(objectTypes[0], tableInfo.Types);
                bool baseAndObjectMatch = hasBaseTypeAndType(objectTypes, tableInfo.Types);
                bool shouldIncludeCatalog = isCatalogEdit(objectTypes, tableInfo.IsCatalogTable);

                if ((allTypesMatch || tableHasOnlyType || baseAndObjectMatch) && (shouldIncludeCatalog))
                {
                    relevantTables.Add(table);
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
        private static object getChildCollection(object childObject, object parentObject)
        {
            Type childType = childObject.GetType();

            foreach (PropertyInfo info in parentObject.GetType().GetProperties())
            {
                if (info.GetGetMethod() != null && info.PropertyType == typeof(ObservableCollection<>).MakeGenericType(new[] { childType }))
                    return parentObject.GetType().GetProperty(info.Name).GetValue(parentObject, null);
            }
            return null;
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
            var childCollection = getChildCollection(childObject, parentObject);

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
