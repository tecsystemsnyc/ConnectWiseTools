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

            //try
            //{

            //Update catalogs from templates.
            if (templates.DeviceCatalog.Count > 0)
            {
                foreach (TECDevice device in templates.DeviceCatalog)
                {
                    editObject(device, bid);
                }
            }

            if (templates.ManufacturerCatalog.Count > 0)
            {
                foreach (TECManufacturer manufacturer in templates.ManufacturerCatalog)
                {
                    editObject(manufacturer, bid);
                }
            }

            if (templates.Tags.Count > 0)
            {
                foreach (TECTag tag in templates.Tags)
                {
                    editObject(tag, bid);
                }
            }

            bid = getBidInfo();
            bid.Labor = getLaborConsts();
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
            linkAllVisualScope(bid.Drawings, bid.Systems, bid.Controllers);
            linkAllLocations(bid.Locations, bid.Systems);
            linkAllConnections(bid.Connections, bid.Controllers, bid.Systems);
            linkConnectionTypeWithDevices(bid.ConnectionTypes, bid.DeviceCatalog);
            linkAllDevices(bid.Systems, bid.DeviceCatalog);
            linkManufacturersWithDevices(bid.ManufacturerCatalog, bid.DeviceCatalog);
            linkTagsInBid(bid.Tags, bid);
            getUserAdjustments(bid);
            //Breaks Visual Scope in a page
            //populatePageVisualConnections(bid.Drawings, bid.Connections);
            //}
            //catch (Exception e)
            //{
            //MessageBox.Show("Could not load bid from database. Error: " + e.Message);
            //}

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

            try
            {
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
                linkConnectionTypeWithDevices(templates.ConnectionTypeCatalog, templates.DeviceCatalog);
                linkTagsInTemplates(templates.Tags, templates);
            }
            catch (Exception e)
            {
                MessageBox.Show("Could not load templates from database. Error: " + e.Message);
            }

            SQLiteDB.Connection.Close();

            return templates;
        }

        static public void SaveBidToNewDB(string path, TECBid bid)
        {
            SQLiteDB = new SQLiteDatabase(path);

            if (File.Exists(path))
            { SQLiteDB.overwriteFile(); }

            createAllBidTables();

            try
            { saveCompleteBid(bid); }
            catch (Exception e)
            {
                string message = "Could not save bid to new database. Error: " + e.Message;
                MessageBox.Show(message);
            } 
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

            try
            { saveCompleteTemplate(templates); }
            catch (Exception e)
            {  MessageBox.Show("Could not save templates to new database. Error: " + e.Message); }

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
                    //Console.WriteLine("Add change saving. Target type: " + targetObject.GetType());
                    addUpdate(targetObject, refObject);
                }
                else if (changeType == Change.Edit)
                {
                    //Console.WriteLine("Edit change saving. Target type: " + targetObject.GetType());
                    editUpdate(targetObject, refObject);
                }
                else if (changeType == Change.Remove)
                {
                    //Console.WriteLine("Remove change saving. Target type: " + targetObject.GetType());
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
                    //Console.WriteLine("Edit change saving. Target type: " + targetObject.GetType());
                    editUpdate(targetObject, refObject);
                }
                else if (changeType == Change.Remove)
                {
                    //Console.WriteLine("Remove change saving. Target type: " + targetObject.GetType());
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
        {

            addObject(tarObject, refObject);

        }

        static private void removeUpdate(object tarObject, object refObject)
        {
            removeObject(tarObject, refObject);
        }

        static private void editUpdate(object tarObject, object refObject)
        {
            editObject(tarObject, refObject);
        }

        #region Old Add Update

        //static private void addUpdate(object tarObject, object refObject)
        //{
        //    if (tarObject is TECSystem)
        //    {
        //        if (refObject is TECBid)
        //        {
        //            //Console.WriteLine("Num systems to update: " + (refObject as TECBid).Systems.Count);
        //            addSystem(tarObject as TECSystem);
        //            updateSystemIndexes((refObject as TECBid).Systems);
        //        }
        //        else if (refObject is TECTemplates)
        //        {
        //            addSystem(tarObject as TECSystem);
        //        }
        //    }
        //    else if (tarObject is TECEquipment)
        //    {
        //        addEquipment(tarObject as TECEquipment);
        //        if (refObject is TECSystem)
        //        {
        //            updateSystemEquipmentRelation(refObject as TECSystem);
        //        }
        //    }
        //    else if (tarObject is TECSubScope)
        //    {
        //        addSubScope(tarObject as TECSubScope);
        //        if (refObject is TECEquipment)
        //        {
        //            updateEquipmentSubScopeRelation(refObject as TECEquipment);
        //        }
        //    }
        //    else if (tarObject is TECDevice)
        //    {
        //        if (refObject is TECSubScope)
        //        {
        //            updateSubScopeDeviceRelation(refObject as TECSubScope);
        //        }
        //        else if (refObject is TECBid || refObject is TECTemplates)
        //        {
        //            addDevice(tarObject as TECDevice);
        //        }
        //    }
        //    else if (tarObject is TECPoint)
        //    {
        //        addPoint(tarObject as TECPoint);
        //        updateSubScopePointRelation(refObject as TECSubScope);
        //    }
        //    else if (tarObject is TECDrawing)
        //    {
        //        addDrawing(tarObject as TECDrawing);
        //    }
        //    else if (tarObject is TECPage)
        //    {
        //        addPage(tarObject as TECPage);
        //        addDrawingPageRelation(refObject as TECDrawing, tarObject as TECPage);
        //    }
        //    else if (tarObject is TECVisualScope)
        //    {
        //        addVisualScope(tarObject as TECVisualScope);
        //        addPageVisualScopeRelation(refObject as TECPage, tarObject as TECVisualScope);
        //        addVisualScopeScopeRelation(tarObject as TECVisualScope);
        //    }
        //    else if (tarObject is TECNote)
        //    {
        //        addNote(tarObject as TECNote);
        //    }
        //    else if (tarObject is TECExclusion)
        //    {
        //        addExclusion(tarObject as TECExclusion);
        //    }
        //    else if (tarObject is TECScopeBranch)
        //    {
        //        addScopeBranch(tarObject as TECScopeBranch);
        //        if (refObject is TECScopeBranch)
        //        {
        //            addScopeTreeRelation(refObject as TECScopeBranch, tarObject as TECScopeBranch);
        //        }
        //        else if (refObject is TECProposalScope)
        //        {
        //            addScopeBranchInProposalScope(tarObject as TECScopeBranch, refObject as TECProposalScope);
        //        }
        //        else if (refObject is TECBid)
        //        {
        //            addScopeBranchBidRelation(tarObject as TECScopeBranch, (refObject as TECBid).InfoGuid);
        //        }
        //    }
        //    else if (tarObject is TECManufacturer)
        //    {
        //        if (refObject is TECDevice)
        //        {
        //            addDeviceManufacturerRelation(refObject as TECDevice, tarObject as TECManufacturer);
        //        }
        //        else if (refObject is TECBid)
        //        {
        //            addManufacturer(tarObject as TECManufacturer);
        //        }
        //        else if (refObject is TECTemplates)
        //        {
        //            addManufacturer(tarObject as TECManufacturer);
        //        }
        //    }
        //    else if (tarObject is TECLocation)
        //    {
        //        if (refObject is TECBid)
        //        {
        //            addLocation(tarObject as TECLocation);
        //        }
        //        else
        //        {
        //            addLocationInScope(refObject as TECScope);
        //        }
        //    }
        //    else if (tarObject is TECController)
        //    {
        //        addController(tarObject as TECController);
        //    }
        //    else if (tarObject is TECTag)
        //    {
        //        if (refObject is TECScope)
        //        {
        //            addTagInScope((tarObject as TECTag), (refObject as TECScope).Guid);
        //        }
        //        else if (refObject is TECTemplates)
        //        {
        //            addTag(tarObject as TECTag);
        //        }
        //    }
        //    else if (tarObject is TECProposalScope)
        //    {
        //        addProposalScope(tarObject as TECProposalScope);
        //    }
        //    else if (tarObject is TECIO)
        //    {
        //        if (refObject is TECController)
        //        {
        //            addControllerIORelation(refObject as TECController, tarObject as TECIO);
        //        }
        //        else
        //        {
        //            throw new NotImplementedException();
        //        }

        //    }
        //    else if (tarObject is TECConnectionType)
        //    {
        //        if (refObject is TECTemplates)
        //        {
        //            addConnectionType(tarObject as TECConnectionType);
        //        }
        //        else if(refObject is TECBid)
        //        {
        //            addConnectionType(tarObject as TECConnectionType);
        //        }
        //        else
        //        {
        //            throw new NotImplementedException();
        //        }
        //    }
        //    else if (tarObject is TECConduitType)
        //    {
        //        if (refObject is TECTemplates)
        //        {
        //            addConduitType(tarObject as TECConduitType);
        //        }
        //        else if (refObject is TECBid)
        //        {
        //            addConduitType(tarObject as TECConduitType);
        //        }
        //        else
        //        {
        //            throw new NotImplementedException();
        //        }
        //    }
        //    else
        //    {
        //        Console.WriteLine("Target object type not included in add branch. Target object type: " + tarObject.GetType());
        //        throw new NotImplementedException();
        //    }
        //}
        #endregion

        #region Old Edit Update

        //static private void editUpdate(object tarObject, object refObject)
        //{
        //    if (tarObject is TECBid)
        //    {
        //        editBidInfo(tarObject as TECBid);
        //    }
        //    else if (tarObject is TECLabor)
        //    {
        //        if (refObject is TECBid)
        //        {
        //            editBidInfo(refObject as TECBid);
        //        }
        //    }
        //    else if (tarObject is TECSystem)
        //    {
        //        editSystem(tarObject as TECSystem);
        //    }
        //    else if (tarObject is TECEquipment)
        //    {
        //        editEquipment(tarObject as TECEquipment);
        //    }
        //    else if (tarObject is TECSubScope)
        //    {
        //        editSubScope(tarObject as TECSubScope);
        //    }
        //    else if (tarObject is TECDevice)
        //    {
        //        if (refObject is TECDevice)
        //        {
        //            editDevice(tarObject as TECDevice);
        //        }
        //        else if (refObject is TECSubScope)
        //        {
        //            editDeviceQuantity(refObject as TECSubScope, tarObject as TECDevice);
        //        }
        //    }
        //    else if (tarObject is TECPoint)
        //    {
        //        editPoint(tarObject as TECPoint);
        //    }
        //    else if (tarObject is TECManufacturer)
        //    {
        //        if (refObject is TECDevice)
        //        {
        //            editManufacturerInDevice(refObject as TECDevice);
        //        }
        //        else
        //        {
        //            editManufacturer(tarObject as TECManufacturer);
        //        }
        //    }
        //    else if (tarObject is TECDrawing)
        //    {
        //        editDrawing(tarObject as TECDrawing);
        //    }
        //    else if (tarObject is TECPage)
        //    {
        //        throw new NotImplementedException();
        //    }
        //    else if (tarObject is TECVisualScope)
        //    {
        //        editVisualScope(tarObject as TECVisualScope);
        //    }
        //    else if (tarObject is TECNote)
        //    {
        //        editNote(tarObject as TECNote);
        //    }
        //    else if (tarObject is TECExclusion)
        //    {
        //        editExclusion(tarObject as TECExclusion);
        //    }
        //    else if (tarObject is TECScopeBranch)
        //    {
        //        editScopeBranch(tarObject as TECScopeBranch);
        //    }
        //    else if (tarObject is TECLocation)
        //    {
        //        editLocation(tarObject as TECLocation);
        //    }
        //    else if (tarObject is TECController)
        //    {
        //        editController(tarObject as TECController);
        //    }
        //    else if (tarObject is ObservableCollection<TECSystem>)
        //    {
        //        updateSystemIndexes(tarObject as ObservableCollection<TECSystem>);
        //    }
        //    else if (tarObject is ObservableCollection<TECEquipment>)
        //    {
        //        updateSystemEquipmentRelation(refObject as TECSystem);
        //    }
        //    else if (tarObject is ObservableCollection<TECSubScope>)
        //    {
        //        updateEquipmentSubScopeRelation(refObject as TECEquipment);
        //    }
        //    else if (tarObject is ObservableCollection<TECDevice>)
        //    {
        //        updateSubScopeDeviceRelation(refObject as TECSubScope);
        //    }
        //    else if (tarObject is ObservableCollection<TECPoint>)
        //    {
        //        updateSubScopePointRelation(refObject as TECSubScope);
        //    }
        //    else if (tarObject is TECIO)
        //    {
        //        if (refObject is TECController)
        //        {
        //            editIOInController(tarObject as TECIO, refObject as TECController);
        //        }
        //        else
        //        {
        //            throw new NotImplementedException();
        //        }
        //    }
        //    else if (tarObject is TECProposalScope)
        //    {
        //        editProposalScope(tarObject as TECProposalScope);
        //    }
        //    else if(tarObject is TECConnectionType)
        //    {
        //        editConnectionType(tarObject as TECConnectionType);
        //    }
        //    else if (tarObject is TECConduitType)
        //    {
        //        editConduitType(tarObject as TECConduitType);
        //    }
        //    else
        //    {
        //        Console.WriteLine("Target object type not included in edit branch. Target object type: " + tarObject.GetType());
        //        throw new NotImplementedException();
        //    }
        //}

        #endregion

        #region old remove update

        //static private void removeUpdate(object tarObject, object refObject)
        //{
        //    if (tarObject is TECSystem)
        //    {
        //        removeSystem(tarObject as TECSystem);
        //        removeLocationInScope(tarObject as TECSystem);
        //    }
        //    else if (tarObject is TECEquipment)
        //    {
        //        removeEquipment(tarObject as TECEquipment);
        //        removeLocationInScope(tarObject as TECEquipment);
        //        if (refObject is TECSystem)
        //        {
        //            removeSystemEquipmentRelation(tarObject as TECEquipment);
        //        }
        //    }
        //    else if (tarObject is TECSubScope)
        //    {
        //        removeSubScope(tarObject as TECSubScope);
        //        removeLocationInScope(tarObject as TECSubScope);
        //        if (refObject is TECEquipment)
        //        {
        //            removeEquipmentSubScopeRelation(tarObject as TECSubScope);
        //        }
        //    }
        //    else if (tarObject is TECDevice)
        //    {
        //        if (refObject is TECSubScope)
        //        {
        //            removeSubScopeDeviceRelation(refObject as TECSubScope, tarObject as TECDevice);
        //        }
        //        else if (refObject is TECTemplates)
        //        {
        //            removeDevice(tarObject as TECDevice);
        //        }
        //    }
        //    else if (tarObject is TECPoint)
        //    {
        //        removePoint(tarObject as TECPoint);
        //        if (refObject is TECSubScope)
        //        {
        //            removeSubScopePointRelation(tarObject as TECPoint);
        //        }
        //    }
        //    else if (tarObject is TECDrawing)
        //    {
        //        throw new NotImplementedException();
        //    }
        //    else if (tarObject is TECPage)
        //    {
        //        throw new NotImplementedException();
        //    }
        //    else if (tarObject is TECVisualScope)
        //    {
        //        removeVisualScope(tarObject as TECVisualScope);
        //        removePageVisualScopeRelation(tarObject as TECVisualScope);
        //    }
        //    else if (tarObject is TECNote)
        //    {
        //        removeNote(tarObject as TECNote);
        //    }
        //    else if (tarObject is TECExclusion)
        //    {
        //        removeExclusion(tarObject as TECExclusion);
        //    }
        //    else if (tarObject is TECScopeBranch)
        //    {
        //        removeScopeBranch(tarObject as TECScopeBranch);
        //        if (refObject is TECScopeBranch)
        //        {
        //            removeScopeBranchHierarchyRelation(tarObject as TECScopeBranch);
        //        }
        //        else if (refObject is TECBid)
        //        {
        //            removeScopeBranchBidRelation(tarObject as TECScopeBranch);
        //        }
        //        else if (refObject is TECProposalScope)
        //        {
        //            removeScopeBranchProposalScopeRelation(tarObject as TECScopeBranch);
        //        }
        //        else
        //        {
        //            throw new NotImplementedException();
        //        }
        //    }
        //    else if (tarObject is TECLocation)
        //    {
        //        if (refObject is TECBid)
        //        {
        //            removeLocation(tarObject as TECLocation);
        //        }
        //        else
        //        {
        //            removeLocationInScope(refObject as TECScope);
        //        }
        //    }
        //    else if (tarObject is TECController)
        //    {
        //        removeController(tarObject as TECController);
        //    }
        //    else if (tarObject is TECProposalScope)
        //    {
        //        removeProposalScope(tarObject as TECProposalScope);
        //    }
        //    else if (tarObject is TECIO)
        //    {
        //        if (refObject is TECController)
        //        {
        //            removeControllerIORelation(refObject as TECController, tarObject as TECIO);
        //        }
        //        else
        //        {
        //            throw new NotImplementedException();
        //        }
        //    }
        //    else if (tarObject is TECManufacturer)
        //    {
        //        if (refObject is TECTemplates)
        //        {
        //            removeManufacturer(tarObject as TECManufacturer);
        //        }
        //        else if (refObject is TECDevice)
        //        {
        //            throw new NotImplementedException();
        //        }
        //        else
        //        {
        //            throw new NotImplementedException();
        //        }
        //    }
        //    else if (tarObject is TECTag)
        //    {
        //        if (refObject is TECTemplates)
        //        {
        //            removeTag(tarObject as TECTag);
        //        }
        //        else if (refObject is TECScope)
        //        {
        //            throw new NotImplementedException();
        //        }
        //        else
        //        {
        //            throw new NotImplementedException();
        //        }
        //    }
        //    else if (tarObject is TECConnectionType)
        //    {
        //        if(refObject is TECTemplates)
        //        {
        //            removeConnectionType(tarObject as TECConnectionType);
        //        }
        //        else
        //        {
        //            throw new NotImplementedException();
        //        }
        //    }
        //    else if (tarObject is TECConduitType)
        //    {
        //        if (refObject is TECTemplates)
        //        {
        //            removeConduitType(tarObject as TECConduitType);
        //        }
        //        else
        //        {
        //            throw new NotImplementedException();
        //        }
        //    }
        //    else
        //    {
        //        Console.WriteLine("Target object type not included in remove branch. Target object type: " + tarObject.GetType());
        //        throw new NotImplementedException();
        //    }
        //}

        #endregion


        #endregion Update Functions
            
        #region Loading from DB Methods

        static private TECBid getBidInfo()
        {
            DataTable bidInfoDT = SQLiteDB.getDataFromTable(BidInfoTable.TableName);

            if (bidInfoDT.Rows.Count < 1)
            {
                MessageBox.Show("Bid info not found in database. Bid info and labor will be missing.");
                return new TECBid();
            }
            DataRow bidInfoRow = bidInfoDT.Rows[0];

            Guid infoGuid = new Guid(bidInfoRow[BidInfoTable.BidID.Name].ToString());
            string name = bidInfoRow[BidInfoTable.BidName.Name].ToString();
            string bidNumber = bidInfoRow[BidInfoTable.BidNumber.Name].ToString();

            string dueDateString = bidInfoRow[BidInfoTable.DueDate.Name].ToString();
            DateTime dueDate = DateTime.ParseExact(dueDateString, DB_FMT, CultureInfo.InvariantCulture);

            string salesperson = bidInfoRow[BidInfoTable.Salesperson.Name].ToString();
            string estimator = bidInfoRow[BidInfoTable.Estimator.Name].ToString();

            return new TECBid(name, bidNumber, dueDate, salesperson, estimator, new ObservableCollection<TECScopeBranch>(), new ObservableCollection<TECSystem>(), new ObservableCollection<TECDevice>(), new ObservableCollection<TECManufacturer>(), new ObservableCollection<TECNote>(), new ObservableCollection<TECExclusion>(), new ObservableCollection<TECTag>(), infoGuid);
        }

        static private void getUserAdjustments(TECBid bid)
        {
            DataTable adjDT = SQLiteDB.getDataFromTable(UserAdjustmentsTable.TableName);

            if (adjDT.Rows.Count < 1)
            {
                Console.WriteLine("getUserAdjustments() failed. No datarows exist.");
                return;
            }

            DataRow adjRow = adjDT.Rows[0];

            bid.Labor.PMExtraHours = adjRow[UserAdjustmentsTable.PMExtraHours.Name].ToString().ToDouble();
            bid.Labor.ENGExtraHours = adjRow[UserAdjustmentsTable.ENGExtraHours.Name].ToString().ToDouble();
            bid.Labor.CommExtraHours = adjRow[UserAdjustmentsTable.CommExtraHours.Name].ToString().ToDouble();
            bid.Labor.SoftExtraHours = adjRow[UserAdjustmentsTable.SoftExtraHours.Name].ToString().ToDouble();
            bid.Labor.GraphExtraHours = adjRow[UserAdjustmentsTable.GraphExtraHours.Name].ToString().ToDouble();
        }

        static private TECLabor getLaborConsts()
        {
            TECLabor labor = new TECLabor();

            DataTable laborDT = SQLiteDB.getDataFromTable(LaborConstantsTable.TableName);
            DataTable subContractDT = SQLiteDB.getDataFromTable(SubcontractorConstantsTable.TableName);

            if (laborDT.Rows.Count < 1)
            {
                MessageBox.Show("Labor constants not found in database, using default values. Reload labor constants from loaded templates in the labor tab.");
                return labor;
            }
            else
            {
                DataRow laborRow = laborDT.Rows[0];

                try
                {
                    labor.PMCoef = laborRow[LaborConstantsTable.PMCoef.Name].ToString().ToDouble();
                    labor.PMRate = laborRow[LaborConstantsTable.PMRate.Name].ToString().ToDouble();

                    labor.ENGCoef = laborRow[LaborConstantsTable.ENGCoef.Name].ToString().ToDouble();
                    labor.ENGRate = laborRow[LaborConstantsTable.ENGRate.Name].ToString().ToDouble();

                    labor.CommCoef = laborRow[LaborConstantsTable.CommCoef.Name].ToString().ToDouble();
                    labor.CommRate = laborRow[LaborConstantsTable.CommRate.Name].ToString().ToDouble();

                    labor.SoftCoef = laborRow[LaborConstantsTable.SoftCoef.Name].ToString().ToDouble();
                    labor.SoftRate = laborRow[LaborConstantsTable.SoftRate.Name].ToString().ToDouble();

                    labor.GraphCoef = laborRow[LaborConstantsTable.GraphCoef.Name].ToString().ToDouble();
                    labor.GraphRate = laborRow[LaborConstantsTable.GraphRate.Name].ToString().ToDouble();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e.Message);
                    Console.WriteLine("Reading labor values from database failed. Using default values.");
                    return labor;
                }

                if (subContractDT.Rows.Count < 1)
                {
                    MessageBox.Show("Subcontracter constants not found in database, using default values. Reload labor constants from loaded templates in the labor tab.");

                    labor.ElectricalRate = 115;
                    labor.ElectricalSuperRate = 125;

                    return labor;
                }
                else
                {
                    DataRow subContractRow = subContractDT.Rows[0];

                    try
                    {
                        labor.ElectricalRate = subContractRow[SubcontractorConstantsTable.ElectricalRate.Name].ToString().ToDouble();
                        labor.ElectricalSuperRate = subContractRow[SubcontractorConstantsTable.ElectricalSuperRate.Name].ToString().ToDouble();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error: " + e.Message);
                        Console.WriteLine("Reading subcontractor values from database failed. Using default values.");

                        labor.ElectricalRate = 115;
                        labor.ElectricalSuperRate = 125;
                    }

                    return labor;
                }
            }
            
        }

        static private TECTemplates getTemplatesInfo()
        {
            DataTable templateInfoDT = SQLiteDB.getDataFromTable(TemplatesInfoTable.TableName);

            if (templateInfoDT.Rows.Count < 1)
            {
                Console.WriteLine("Template info not found in database.");
                return new TECTemplates();
            }
            DataRow templateInfoRow = templateInfoDT.Rows[0];

            Guid infoGuid = new Guid(templateInfoRow[TemplatesInfoTable.TemplateID.Name].ToString());

            return new TECTemplates(infoGuid);
        }

        static private ObservableCollection<TECScopeBranch> getBidScopeBranches()
        {
            ObservableCollection<TECScopeBranch> mainBranches = new ObservableCollection<TECScopeBranch>();

            string command = "select * from TECScopeBranch where ScopeBranchID in (select ScopeBranchID from TECBidTECScopeBranch where ScopeBranchID not in ";
            command += "(select ChildID from TECScopeBranchHierarchy))";

            DataTable mainBranchDT = SQLiteDB.getDataFromCommand(command);

            foreach (DataRow row in mainBranchDT.Rows)
            {
                Guid scopeBranchID = new Guid(row[ScopeBranchTable.ScopeBranchID.Name].ToString());
                string name = row["Name"].ToString();
                string description = row["Description"].ToString();

                ObservableCollection<TECScopeBranch> childBranches = getChildBranchesInBranch(scopeBranchID);

                TECScopeBranch branch = new TECScopeBranch(name, description, childBranches, scopeBranchID);

                branch.Tags = getTagsInScope(scopeBranchID);

                mainBranches.Add(branch);
            }

            return mainBranches;
        }

        static private ObservableCollection<TECScopeBranch> getProposalScopeBranches(Guid propScopeID)
        {
            ObservableCollection<TECScopeBranch> scopeBranches = new ObservableCollection<TECScopeBranch>();
            string command = "select * from " + ScopeBranchTable.TableName + " where " + ScopeBranchTable.ScopeBranchID.Name + " in "
                + "(select " + ProposalScopeScopeBranchTable.ScopeBranchID.Name + " from " + ProposalScopeScopeBranchTable.TableName + " where " + ProposalScopeScopeBranchTable.ProposalScopeID.Name + " = '"
                + propScopeID + "')";

            try
            {
                DataTable scopeBranchDT = SQLiteDB.getDataFromCommand(command);

                foreach (DataRow row in scopeBranchDT.Rows)
                {
                    Guid scopeBranchID = new Guid(row[ScopeBranchTable.ScopeBranchID.Name].ToString());
                    string name = row["Name"].ToString();
                    string description = row["Description"].ToString();

                    ObservableCollection<TECScopeBranch> childBranches = getChildBranchesInBranch(scopeBranchID);

                    TECScopeBranch branch = new TECScopeBranch(name, description, childBranches, scopeBranchID);

                    branch.Tags = getTagsInScope(scopeBranchID);

                    scopeBranches.Add(branch);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: getProposalScopeBranches() failed. Code: " + e.Message);
                throw e;
            }

            return scopeBranches;
        }

        static private ObservableCollection<TECScopeBranch> getChildBranchesInBranch(Guid parentID)
        {
            ObservableCollection<TECScopeBranch> childBranches = new ObservableCollection<TECScopeBranch>();

            string command = "select * from TECScopeBranch where ScopeBranchID in ";
            command += "(select ChildID from TECScopeBranchHierarchy where ParentID = '";
            command += parentID;
            command += "')";

            DataTable childBranchDT = SQLiteDB.getDataFromCommand(command);

            foreach (DataRow row in childBranchDT.Rows)
            {
                Guid childBranchID = new Guid(row[ScopeBranchTable.ScopeBranchID.Name].ToString());
                string name = row[ScopeBranchTable.Name.Name].ToString();
                string description = row[ScopeBranchTable.Description.Name].ToString();

                ObservableCollection<TECScopeBranch> grandChildBranches = getChildBranchesInBranch(childBranchID);

                TECScopeBranch branch = new TECScopeBranch(name, description, grandChildBranches, childBranchID);

                branch.Tags = getTagsInScope(childBranchID);

                childBranches.Add(branch);
            }

            return childBranches;
        }

        static private ObservableCollection<TECSystem> getAllSystems()
        {
            ObservableCollection<TECSystem> systems = new ObservableCollection<TECSystem>();

            string command = "select * from TECSystem";

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
            {
                Guid systemID = new Guid(row[SystemTable.SystemID.Name].ToString());
                string name = row[SystemTable.Name.Name].ToString();
                string description = row[SystemTable.Description.Name].ToString();
                string quantityString = row[SystemTable.Quantity.Name].ToString();
                string budgetPriceString = row[SystemTable.BudgetPrice.Name].ToString();

                int quantity;
                if (!int.TryParse(quantityString, out quantity))
                {
                    quantity = 1;
                    Console.WriteLine("Cannot convert quantity to int in system, setting to 1");
                }

                double budgetPrice;
                if (!double.TryParse(budgetPriceString, out budgetPrice))
                {
                    budgetPrice = -1;
                    Console.WriteLine("Cannot convert budgetPrice to double, setting to -1");
                }

                ObservableCollection<TECEquipment> equipmentInSystem = getEquipmentInSystem(systemID);

                TECSystem system = new TECSystem(name, description, budgetPrice, equipmentInSystem, systemID);

                system.Quantity = quantity;
                system.Tags = getTagsInScope(systemID);

                systems.Add(system);
            }
            return systems;
        }

        static private ObservableCollection<TECEquipment> getOrphanEquipment()
        {
            ObservableCollection<TECEquipment> equipment = new ObservableCollection<TECEquipment>();

            string command = "select * from TECEquipment where EquipmentID not in ";
            command += "(select EquipmentID from TECSystemTECEquipment)";

            DataTable equipmentDT = SQLiteDB.getDataFromCommand(command);

            foreach (DataRow row in equipmentDT.Rows)
            {
                var equipmentToAdd = getEquipmentFromRow(row);

                equipment.Add(equipmentToAdd);
            }

            return equipment;
        }

        static private ObservableCollection<TECSubScope> getOrphanSubScope()
        {
            ObservableCollection<TECSubScope> subScope = new ObservableCollection<TECSubScope>();

            string command = "select * from TECSubScope where SubScopeID not in ";
            command += "(select SubScopeID from TECEquipmentTECSubScope)";

            DataTable subScopeDT = SQLiteDB.getDataFromCommand(command);

            foreach (DataRow row in subScopeDT.Rows)
            {
                Guid subScopeID = new Guid(row[SubScopeTable.SubScopeID.Name].ToString());
                string name = row["Name"].ToString();
                string description = row["Description"].ToString();
                string quantityString = row["Quantity"].ToString();

                int quantity;
                if (!int.TryParse(quantityString, out quantity))
                {
                    quantity = 1;
                    Console.WriteLine("Cannot convert quantity to int in subscope, setting to 1");
                }

                ObservableCollection<TECDevice> devicesInSubScope = getDevicesInSubScope(subScopeID);
                ObservableCollection<TECPoint> pointsInSubScope = getPointsInSubScope(subScopeID);

                TECSubScope subScopeToAdd = new TECSubScope(name, description, devicesInSubScope, pointsInSubScope, subScopeID);

                subScopeToAdd.Quantity = quantity;
                subScopeToAdd.Tags = getTagsInScope(subScopeID);

                subScope.Add(subScopeToAdd);
            }

            return subScope;
        }

        static private ObservableCollection<TECDevice> getAllDevices()
        {
            ObservableCollection<TECDevice> devices = new ObservableCollection<TECDevice>();

            DataTable devicesDT = SQLiteDB.getDataFromTable(DeviceTable.TableName);

            foreach (DataRow row in devicesDT.Rows)
            {
                Guid deviceID = new Guid(row[DeviceTable.DeviceID.Name].ToString());
                string name = row[DeviceTable.Name.Name].ToString();
                string description = row[DeviceTable.Description.Name].ToString();
                string costString = row[DeviceTable.Cost.Name].ToString();

                double cost;
                if (!double.TryParse(costString, out cost))
                {
                    cost = 0;
                    Console.WriteLine("Cannot convert cost to double, setting to 0");
                }

                TECManufacturer manufacturer = getManufacturerInDevice(deviceID);
                TECConnectionType connectionType = getConnectionTypeInDevice(deviceID);

                TECDevice deviceToAdd = new TECDevice(name, description, cost, manufacturer, deviceID);
                deviceToAdd.Tags = getTagsInScope(deviceID);
                deviceToAdd.ConnectionType = connectionType;

                devices.Add(deviceToAdd);
            }

            return devices;
        }

        static private ObservableCollection<TECManufacturer> getAllManufacturers()
        {
            ObservableCollection<TECManufacturer> manufacturers = new ObservableCollection<TECManufacturer>();

            DataTable manufacturersDT = SQLiteDB.getDataFromTable("TECManufacturer");

            foreach (DataRow row in manufacturersDT.Rows)
            {
                Guid manufacturerID = new Guid(row["ManufacturerID"].ToString());
                string name = row["Name"].ToString();
                string multiplierString = row["Multiplier"].ToString();

                double multiplier;
                if (!double.TryParse(multiplierString, out multiplier))
                {
                    multiplier = 1;
                    Console.WriteLine("Cannot convert multiplier to double, setting to 1");
                }

                manufacturers.Add(new TECManufacturer(name, multiplier, manufacturerID));
            }

            return manufacturers;
        }

        static private ObservableCollection<TECLocation> getAllLocations()
        {
            ObservableCollection<TECLocation> locations = new ObservableCollection<TECLocation>();

            DataTable locationsDT = SQLiteDB.getDataFromTable("TECLocation");

            foreach (DataRow row in locationsDT.Rows)
            {
                Guid locationID = new Guid(row["LocationID"].ToString());
                string name = row["Name"].ToString();

                locations.Add(new TECLocation(name, locationID));
            }

            return locations;
        }

        static private ObservableCollection<TECEquipment> getEquipmentInSystem(Guid systemID)
        {
            ObservableCollection<TECEquipment> equipment = new ObservableCollection<TECEquipment>();

            string command = "select * from (TECEquipment inner join TECSystemTECEquipment on (TECEquipment.EquipmentID = TECSystemTECEquipment.EquipmentID and SystemID = '";
            command += systemID;
            command += "')) order by ScopeIndex";

            DataTable equipmentDT = SQLiteDB.getDataFromCommand(command);

            foreach (DataRow row in equipmentDT.Rows)
            {
                Guid equipmentID = new Guid(row[EquipmentTable.EquipmentID.Name].ToString());
                string name = row[EquipmentTable.Name.Name].ToString();
                string description = row[EquipmentTable.Description.Name].ToString();
                string quantityString = row[EquipmentTable.Quantity.Name].ToString();
                string budgetPriceString = row[EquipmentTable.BudgetPrice.Name].ToString();

                double budgetPrice;
                if (!double.TryParse(budgetPriceString, out budgetPrice))
                {
                    budgetPrice = -1;
                    Console.WriteLine("Cannot convert budget price to double in equipment, setting to -1");
                }

                int quantity;
                if (!int.TryParse(quantityString, out quantity))
                {
                    quantity = 1;
                    Console.WriteLine("Cannot convert quantity to int in equipment, setting to 1");
                }

                ObservableCollection<TECSubScope> subScopeInEquipment = getSubScopeInEquipment(equipmentID);

                TECEquipment equipmentToAdd = new TECEquipment(name, description, budgetPrice, subScopeInEquipment, equipmentID);

                equipmentToAdd.Quantity = quantity;
                equipmentToAdd.Tags = getTagsInScope(equipmentID);

                equipment.Add(equipmentToAdd);
            }

            return equipment;
        }

        static private ObservableCollection<TECSubScope> getSubScopeInEquipment(Guid equipmentID)
        {
            ObservableCollection<TECSubScope> subScope = new ObservableCollection<TECSubScope>();

            string command = "select * from (TECSubScope inner join TECEquipmentTECSubScope on (TECSubScope.SubScopeID = TECEquipmentTECSubScope.SubScopeID and EquipmentID = '";
            command += equipmentID;
            command += "')) order by ScopeIndex";

            DataTable subScopeDT = SQLiteDB.getDataFromCommand(command);

            foreach (DataRow row in subScopeDT.Rows)
            {
                var subScopeToAdd = getSubScopeFromRow(row);
                subScope.Add(subScopeToAdd);
            }

            return subScope;
        }

        static private ObservableCollection<TECDevice> getDevicesInSubScope(Guid subScopeID)
        {
            //Console.WriteLine("getDevicesInSubScope() called");

            ObservableCollection<TECDevice> devices = new ObservableCollection<TECDevice>();

            string command = "select * from (TECDevice inner join TECSubScopeTECDevice on (TECDevice.DeviceID = TECSubScopeTECDevice.DeviceID and SubScopeID = '";
            command += subScopeID;
            command += "')) order by ScopeIndex";

            DataTable devicesDT = SQLiteDB.getDataFromCommand(command);

            foreach (DataRow row in devicesDT.Rows)
            {
                Guid deviceID = new Guid(row[DeviceTable.DeviceID.Name].ToString());
                string name = row[DeviceTable.Name.Name].ToString();
                string description = row[DeviceTable.Description.Name].ToString();
                string costString = row[DeviceTable.Cost.Name].ToString();

                double cost;
                if (!double.TryParse(costString, out cost))
                {
                    cost = 0;
                    Console.WriteLine("Cannot convert cost to double, setting to 0");
                }

                TECManufacturer manufacturer = getManufacturerInDevice(deviceID);
                TECConnectionType connectionType = getConnectionTypeInDevice(deviceID);

                TECDevice deviceToAdd = new TECDevice(name, description, cost, manufacturer, deviceID);
                deviceToAdd.ConnectionType = connectionType;

                string quantityCommand = "select Quantity from TECSubScopeTECDevice where SubScopeID = '";
                quantityCommand += (subScopeID + "' and DeviceID = '" + deviceID + "'");

                DataTable quantityDT = SQLiteDB.getDataFromCommand(quantityCommand);

                string quantityString = quantityDT.Rows[0][0].ToString();
                //Console.WriteLine("QuantityString: " + quantityString);
                int quantity;
                if (!int.TryParse(quantityString, out quantity))
                {
                    quantity = 1;
                    Console.WriteLine("Cannot convert quantity to int in device, setting to 1");
                }
                deviceToAdd.Tags = getTagsInScope(deviceID);
                //deviceToAdd.Quantity = quantity;
                for (int x = 0; x < quantity; x++)
                {
                    devices.Add(deviceToAdd);
                }

            }

            return devices;
        }

        static private ObservableCollection<TECPoint> getPointsInSubScope(Guid subScopeID)
        {
            ObservableCollection<TECPoint> points = new ObservableCollection<TECPoint>();

            string command = "select * from (TECPoint inner join TECSubScopeTECPoint on (TECPoint.PointID = TECSubScopeTECPoint.PointID and SubScopeID = '";
            command += subScopeID;
            command += "')) order by ScopeIndex";

            DataTable pointsDT = SQLiteDB.getDataFromCommand(command);

            foreach (DataRow row in pointsDT.Rows)
            {
                Guid pointID = new Guid(row[PointTable.PointID.Name].ToString());
                string name = row[PointTable.Name.Name].ToString();
                string description = row[PointTable.Description.Name].ToString();
                string type = row[PointTable.Type.Name].ToString();
                string quantityString = row[PointTable.Quantity.Name].ToString();

                int quantity;
                if (!int.TryParse(quantityString, out quantity))
                {
                    quantity = 1;
                    Console.WriteLine("Cannot convert quantity to int in point, setting to 1");
                }

                TECPoint pointToAdd = new TECPoint(type, name, description, pointID);

                pointToAdd.Quantity = quantity;
                pointToAdd.Tags = getTagsInScope(pointID);

                points.Add(pointToAdd);
            }

            return points;
        }

        static private TECManufacturer getManufacturerInDevice(Guid deviceID)
        {
            string command = "select * from TECManufacturer where ManufacturerID in ";
            command += "(select ManufacturerID from TECDeviceTECManufacturer where DeviceID = '";
            command += deviceID;
            command += "')";

            DataTable manTable = SQLiteDB.getDataFromCommand(command);

            if (manTable.Rows.Count > 0)
            {
                Guid manufacturerID = new Guid(manTable.Rows[0]["ManufacturerID"].ToString());
                string name = manTable.Rows[0]["Name"].ToString();
                string multiplierString = manTable.Rows[0]["Multiplier"].ToString();

                double multiplier;
                if (!double.TryParse(multiplierString, out multiplier))
                {
                    multiplier = 1;
                    Console.WriteLine("Cannot convert multiplier to double, setting to 1");
                }

                return new TECManufacturer(name, multiplier, manufacturerID);
            }
            else
            {
                return new TECManufacturer();
            }
        }

        static private TECConnectionType getConnectionTypeInDevice(Guid deviceID)
        {
            string command = "select * from TECConnectionType where ConnectionTypeID in ";
            command += "(select ConnectionTypeID from TECDeviceTECConnectionType where DeviceID = '";
            command += deviceID;
            command += "')";

            DataTable connectionTypeTable = SQLiteDB.getDataFromCommand(command);

            if (connectionTypeTable.Rows.Count > 0)
            {
                return (getConnectionTypeFromRow(connectionTypeTable.Rows[0]));
            }
            else
            {
                return new TECConnectionType();
            }
        }

        static private ObservableCollection<TECNote> getNotes()
        {
            ObservableCollection<TECNote> notes = new ObservableCollection<TECNote>();
            DataTable notesDT = SQLiteDB.getDataFromTable(NoteTable.TableName);

            foreach (DataRow row in notesDT.Rows)
            {
                Guid noteID = new Guid(row[NoteTable.NoteID.Name].ToString());
                string noteText = row["NoteText"].ToString();

                notes.Add(new TECNote(noteText, noteID));
            }

            return notes;
        }

        static private ObservableCollection<TECExclusion> getExclusions()
        {
            ObservableCollection<TECExclusion> exclusions = new ObservableCollection<TECExclusion>();
            DataTable exclusionsDT = SQLiteDB.getDataFromTable("TECExclusion");

            foreach (DataRow row in exclusionsDT.Rows)
            {
                Guid exclusionId = new Guid(row["ExclusionID"].ToString());
                string exclusionText = row["ExclusionText"].ToString();

                exclusions.Add(new TECExclusion(exclusionText, exclusionId));
            }

            return exclusions;
        }

        static private ObservableCollection<TECTag> getAllTags()
        {
            ObservableCollection<TECTag> tags = new ObservableCollection<TECTag>();

            DataTable tagsDT = SQLiteDB.getDataFromTable("TECTag");

            foreach (DataRow row in tagsDT.Rows)
            {
                tags.Add(new TECTag(row["TagString"].ToString(), new Guid(row["TagID"].ToString())));
            }

            return tags;
        }

        static private ObservableCollection<TECTag> getTagsInScope(Guid scopeID)
        {
            ObservableCollection<TECTag> tags = new ObservableCollection<TECTag>();

            string command = "select * from TECTag where TagID in ";
            command += "(select TagID from TECScopeTECTag where ScopeID = '";
            command += scopeID;
            command += "')";

            DataTable tagsDT = SQLiteDB.getDataFromCommand(command);

            foreach (DataRow row in tagsDT.Rows)
            {
                tags.Add(new TECTag(row["TagString"].ToString(), new Guid(row["TagID"].ToString())));
            }

            return tags;
        }

        static private ObservableCollection<TECDrawing> getDrawings()
        {
            ObservableCollection<TECDrawing> drawings = new ObservableCollection<TECDrawing>();

            try
            {
                DataTable ghostDrawingsDT = SQLiteDB.getDataFromTable(DrawingTable.TableName);

                foreach (DataRow row in ghostDrawingsDT.Rows)
                {
                    string name = row["Name"].ToString();
                    string description = row["Description"].ToString();
                    Guid guid = new Guid(row["DrawingID"].ToString());

                    ObservableCollection<TECPage> pages = getPagesInDrawing(guid);

                    drawings.Add(new TECDrawing(name, description, guid, pages));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: GetDrawings() failed. Code: " + e.Message);
                throw e;
            }

            return drawings;
        }

        static private ObservableCollection<TECPage> getPagesInDrawing(Guid DrawingID)
        {
            ObservableCollection<TECPage> pages = new ObservableCollection<TECPage>();

            string command = "select * from TECPage where PageID in ";
            command += "(select PageID from TECDrawingTECPage where DrawingID = '";
            command += DrawingID;
            command += "') order by PageNum";

            try
            {
                DataTable pagesDT = SQLiteDB.getDataFromCommand(command);

                foreach (DataRow row in pagesDT.Rows)
                {
                    Guid guid = new Guid(row["PageID"].ToString());
                    int pageNum = row["PageNum"].ToString().ToInt();
                    byte[] blob = row["Image"] as byte[];

                    TECPage page = new TECPage(pageNum, guid);

                    page.Path = Path.GetTempFileName();

                    //path = path.Substring(0, path.Length - 3);
                    //path += "png";

                    File.WriteAllBytes(page.Path, blob);

                    page.PageScope = getVisualScopeInPage(guid);

                    pages.Add(page);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: GetPagesInDrawing() failed. Code: " + e.Message);
                throw e;
            }

            return pages;
        }

        static private ObservableCollection<TECVisualScope> getVisualScopeInPage(Guid PageID)
        {
            ObservableCollection<TECVisualScope> vs = new ObservableCollection<TECVisualScope>();

            string command = "select * from TECVisualScope where VisualScopeID in ";
            command += "(select VisualScopeID from TECPageTECVisualScope where PageID = '";
            command += PageID;
            command += "')";

            try
            {
                DataTable vsDT = SQLiteDB.getDataFromCommand(command);

                foreach (DataRow row in vsDT.Rows)
                {
                    Guid guid = new Guid(row["VisualScopeID"].ToString());
                    double xPos = row["XPos"].ToString().ToDouble();
                    double yPos = row["YPos"].ToString().ToDouble();

                    vs.Add(new TECVisualScope(guid, xPos, yPos));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: GetVisualScopeInPage() failed. Code: " + e.Message);
                throw e;
            }

            return vs;
        }

        static private TECLocation getLocationInScope(Guid ScopeID)
        {
            string command = "select * from TECLocation where LocationID in ";
            command += "(select LocationID from TECLocationTECScope where ScopeID = '";
            command += ScopeID;
            command += "')";

            DataTable locationDT = SQLiteDB.getDataFromCommand(command);

            if (locationDT.Rows.Count > 0)
            {
                TECLocation location = new TECLocation(locationDT.Rows[0]["Name"].ToString(), new Guid(locationDT.Rows[0]["LocationID"].ToString()));
                return location;
            }
            else
            {
                return null;
            }
        }

        static private ObservableCollection<TECController> getControllers()
        {
            ObservableCollection<TECController> controllers = new ObservableCollection<TECController>();

            try
            {
                DataTable controllersDT = SQLiteDB.getDataFromTable(ControllerTable.TableName);

                foreach (DataRow row in controllersDT.Rows)
                {
                    Guid guid = new Guid(row[ControllerTable.ControllerID.Name].ToString());
                    string name = row[ControllerTable.Name.Name].ToString();
                    string description = row[ControllerTable.Description.Name].ToString();
                    string costString = row[ControllerTable.Cost.Name].ToString();

                    double cost;
                    if (!double.TryParse(costString, out cost))
                    {
                        cost = 0;
                        Console.WriteLine("Cannot convert cost to double, setting to 0");
                    }

                    TECController controller = new TECController(name, description, guid, cost);

                    controller.IO = getIOInController(guid);
                    controller.Tags = getTagsInScope(guid);

                    controllers.Add(controller);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: getControllers() failed. Code: " + e.Message);
                throw e;
            }

            return controllers;
        }

        static private ObservableCollection<TECConnectionType> getConnectionTypes()
        {
            ObservableCollection<TECConnectionType> connectionTypes = new ObservableCollection<TECConnectionType>();
            try
            {
                DataTable connectionTypesDT = SQLiteDB.getDataFromTable(ConnectionTypeTable.TableName);
                foreach (DataRow row in connectionTypesDT.Rows)
                {
                    connectionTypes.Add(getConnectionTypeFromRow(row));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: getConnectionTypes() failed. Code: " + e.Message);
                throw e;
            }
            return connectionTypes;

        }

        static private ObservableCollection<TECIO> getIOInController(Guid controllerID)
        {
            ObservableCollection<TECIO> outIO = new ObservableCollection<TECIO>();

            string command = "select * from " + ControllerIOTypeTable.TableName + " where " +
                ControllerIOTypeTable.ControllerID.Name + " = '" + controllerID + "'";

            try
            {
                DataTable typeDT = SQLiteDB.getDataFromCommand(command);

                foreach (DataRow row in typeDT.Rows)
                {
                    IOType type = TECIO.convertStringToType(row[ControllerIOTypeTable.IOType.Name].ToString());
                    int qty = row[ControllerIOTypeTable.Quantity.Name].ToString().ToInt();

                    var io = new TECIO(type);
                    io.Quantity = qty;
                    outIO.Add(io);

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("getConnectionTypesInController() failed. Code: " + e.Message);
                throw e;
            }

            return outIO;
        }

        static private ObservableCollection<TECConnection> getConnections()
        {
            ObservableCollection<TECConnection> connections = new ObservableCollection<TECConnection>();

            try
            {
                DataTable connectionDT = SQLiteDB.getDataFromTable(ConnectionTable.TableName);

                foreach (DataRow row in connectionDT.Rows)
                {
                    Guid guid = new Guid(row[ConnectionTable.ConnectionID.Name].ToString());
                    string lengthString = row[ConnectionTable.Length.Name].ToString();

                    double length = lengthString.ToDouble();

                    connections.Add(new TECConnection(length, guid));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: getConnections() failed. Code: " + e.Message);
                throw e;
            }

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


            string command = "select " + ProposalScopeTable.IsProposed.Name + " from " + ProposalScopeTable.TableName + " where " + ProposalScopeTable.ProposalScopeID.Name + " = '" + scope.Guid + "'";

            try
            {
                DataTable isProposedDT = SQLiteDB.getDataFromCommand(command);

                if (isProposedDT.Rows.Count > 0)
                {
                    isProposed = isProposedDT.Rows[0][ProposalScopeTable.IsProposed.Name].ToString().ToInt().ToBool();
                }
                else
                {
                    isProposed = false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: getProposalScopeFromScope() failed. Code: " + e.Message);
                throw e;
            }

            return new TECProposalScope(scope, isProposed, notes);
        }
        #endregion //Loading from DB Methods

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
            var tableInfo =  new TableInfo(table);
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

                        try
                        {
                            DataTable scopeID = SQLiteDB.getDataFromCommand(command);

                            Guid scopeGuid = new Guid(scopeID.Rows[0][0].ToString());

                            scopeToLink.Add(vs, scopeGuid);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Error: Finding TECScopeID from VisualScopeID failed in linkAllVisualScope(). Code: " + e.Message);
                        }
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
                {
                    scopeToLink.Remove(scope);
                }
                if (scopeToLink.Count < 1)
                {
                    return;
                }
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
                {
                    scopeToLink.Remove(scope);
                }
                if (scopeToLink.Count < 1)
                {
                    return;
                }
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
                    {
                        scopeToLink.Remove(scope);
                    }
                    if (scopeToLink.Count < 1)
                    {
                        return;
                    }
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
                        {
                            scopeToLink.Remove(scope);
                        }
                        if (scopeToLink.Count < 1)
                        {
                            return;
                        }
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
                            {
                                scopeToLink.Remove(scope);
                            }
                            if (scopeToLink.Count < 1)
                            {
                                return;
                            }
                            scopeToRemove.Clear();
                        }
                    }
                }
            }

            //if (scopeToLink.Count > 0)
            //{
            //    Exception e = new Exception("Error: " + scopeToLink.Count + " Visual Scope not found. Unable to link.");
            //    throw e;
            //}
        }

        static private void linkAllLocations(ObservableCollection<TECLocation> locations, ObservableCollection<TECSystem> bidSystems)
        {
            Dictionary<Guid, TECLocation> scopeToLink = new Dictionary<Guid, TECLocation>();

            foreach (TECLocation location in locations)
            {
                string command = "select ScopeID from TECLocationTECScope where LocationID = '" + location.Guid + "'";

                try
                {
                    DataTable scopeDT = SQLiteDB.getDataFromCommand(command);

                    foreach (DataRow row in scopeDT.Rows)
                    {
                        scopeToLink.Add(new Guid(row["ScopeID"].ToString()), location);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: Finding scope in location failed in LinkAllLocations. Code: " + e.Message);
                }
            }

            List<Guid> scopeToRemove = new List<Guid>();
            foreach (TECSystem sys in bidSystems)
            {
                foreach (Guid guid in scopeToLink.Keys)
                {
                    if (sys.Guid == guid)
                    {
                        sys.Location = scopeToLink[guid];
                        scopeToRemove.Add(guid);
                    }
                }
                foreach (Guid guid in scopeToRemove)
                {
                    scopeToLink.Remove(guid);
                }
                if (scopeToLink.Count < 1)
                {
                    return;
                }
                scopeToRemove.Clear();

                foreach (TECEquipment equip in sys.Equipment)
                {
                    foreach (Guid guid in scopeToLink.Keys)
                    {
                        if (equip.Guid == guid)
                        {
                            equip.Location = scopeToLink[guid];
                            scopeToRemove.Add(guid);
                        }
                    }
                    foreach (Guid guid in scopeToRemove)
                    {
                        scopeToLink.Remove(guid);
                    }
                    if (scopeToLink.Count < 1)
                    {
                        return;
                    }
                    scopeToRemove.Clear();

                    foreach (TECSubScope ss in equip.SubScope)
                    {
                        foreach (Guid guid in scopeToLink.Keys)
                        {
                            if (ss.Guid == guid)
                            {
                                ss.Location = scopeToLink[guid];
                                scopeToRemove.Add(guid);
                            }
                        }
                        foreach (Guid guid in scopeToRemove)
                        {
                            scopeToLink.Remove(guid);
                        }
                        if (scopeToLink.Count < 1)
                        {
                            return;
                        }
                        scopeToRemove.Clear();
                    }
                }
            }
            //if (scopeToLink.Count > 0)
            //{
            //    Exception e = new Exception("Error: " + scopeToLink.Count + " scope not found in linkAllLocations(). Unable to link.");
            //    throw e;
            //}
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
                {
                    childGuids.Add(new Guid(row[ScopeConnectionTable.ScopeID.Name].ToString()));
                }

                toConnect.Add(new Tuple<TECConnection, Guid, List<Guid>>(conn, parentGuid, childGuids));
            }

            //Construct potential TECScope List
            List<TECScope> scopeToLink = new List<TECScope>();

            foreach (TECController controller in controllers)
            {
                scopeToLink.Add(controller);
            }
            foreach (TECSystem system in bidSystems)
            {
                foreach (TECEquipment equip in system.Equipment)
                {
                    foreach (TECSubScope ss in equip.SubScope)
                    {
                        scopeToLink.Add(ss);
                    }
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
                            {
                                (scope as TECSubScope).Connection = connection;
                            }
                            else if (scope is TECController)
                            {
                                (scope as TECController).Connections.Add(connection);
                            }
                            connection.Scope.Add(scope);
                            scopeToRemove = scope;
                            break;
                        }
                    }
                    if (scopeToRemove != null)
                    {
                        scopeToLink.Remove(scopeToRemove);
                    }
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
                                {
                                    linkedDevices.Add(cDev);
                                }
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
            {
                linkAllDevicesFromEquipment(system.Equipment, deviceCatalog);
            }
        }

        static private void linkAllDevicesFromEquipment(ObservableCollection<TECEquipment> equipment, ObservableCollection<TECDevice> deviceCatalog)
        {
            foreach (TECEquipment equip in equipment)
            {
                linkAllDevicesFromSubScope(equip.SubScope, deviceCatalog);
            }
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
                        {
                            linkedDevices.Add(cDev);
                        }
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
                    {
                        device.Manufacturer = man;
                    }
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
                        {
                            linkTags(tags, device);
                        }
                        foreach (TECPoint point in subScope.Points)
                        {
                            linkTags(tags, point);
                        }
                    }
                }
            }
            foreach (TECController controller in bid.Controllers)
            {
                linkTags(tags, controller);
            }
            foreach (TECDevice device in bid.DeviceCatalog)
            {
                linkTags(tags, device);
            }
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
                        {
                            linkTags(tags, device);
                        }
                        foreach (TECPoint point in subScope.Points)
                        {
                            linkTags(tags, point);
                        }
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
                    {
                        linkTags(tags, device);
                    }
                    foreach (TECPoint point in subScope.Points)
                    {
                        linkTags(tags, point);
                    }
                }
            }
            foreach (TECSubScope subScope in templates.SubScopeTemplates)
            {
                linkTags(tags, subScope);
                foreach (TECDevice device in subScope.Devices)
                {
                    linkTags(tags, device);
                }
                foreach (TECPoint point in subScope.Points)
                {
                    linkTags(tags, point);
                }
            }
            foreach (TECController controller in templates.ControllerTemplates)
            {
                linkTags(tags, controller);
            }
            foreach (TECDevice device in templates.DeviceCatalog)
            {
                linkTags(tags, device);
            }
        }

        static private void linkTags(ObservableCollection<TECTag> tags, TECScope scope)
        {
            ObservableCollection<TECTag> linkedTags = new ObservableCollection<TECTag>();
            foreach (TECTag tag in scope.Tags)
            {
                foreach (TECTag referenceTag in tags)
                {
                    if (tag.Guid == referenceTag.Guid)
                    {
                        linkedTags.Add(referenceTag);
                    }
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
                    {
                        device.ConnectionType = connectionType;
                    }
                }
            }
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
            //try
            //{
                isUpToDate = checkDatabaseVersion(type);
                if (!isUpToDate)
                {
                    updateDatabase(type);
                    updateVersionNumber(type);
                }
            //}
            //catch (Exception e)
            //{
            //    MessageBox.Show("Could not check database version." + e);
            //}
        }

        static private bool checkDatabaseVersion(Type type)
        {
            string currentVersion = Properties.Settings.Default.Version;

            try
            {
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
                        MessageBox.Show("Bid info not found in database. Could not check verison.");
                        throw new Exception("Could not load from TECBidInfo");
                    } else if(type == typeof(TECTemplates))
                    {
                        killTemplatesInfo();
                        return false;
                    }
                    else if (type == typeof(TECTemplates))
                    {
                        MessageBox.Show("Templates info not found in database. Could not check verison.");
                        throw new Exception("Could not load from TECTemplatesInfo");
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
            catch (Exception e)
            {
                MessageBox.Show("Could not load version number from database. Error: " + e.Message);
                throw e;
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
                {
                    updateTableFromType(table);
                }
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
            var date = DateTime.Now;

            Console.WriteLine("Backing up...");
            string APPDATA_FOLDER = @"TECSystems\Backups\";
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string backupFolder = Path.Combine(appData, APPDATA_FOLDER);

            CultureInfo culture = CultureInfo.CreateSpecificCulture("ja-JP");
            DateTimeFormatInfo dtfi = culture.DateTimeFormat;
            dtfi.DateSeparator = "\\";
            backupFolder += date.ToString("d", dtfi);

            if (!Directory.Exists(backupFolder))
            {
                Directory.CreateDirectory(backupFolder);
            }

            string backupFileName = Path.GetFileNameWithoutExtension(originalPath);
            backupFileName += "-";
            culture = CultureInfo.CreateSpecificCulture("hr-HR");
            dtfi = culture.DateTimeFormat;
            dtfi.TimeSeparator = "-";
            backupFileName += date.ToString("T", dtfi);
            var backupPath = Path.Combine(backupFolder, backupFileName);
            Console.WriteLine("Backup path: " + backupPath);
            try
            {
                File.Copy(originalPath, backupPath);
            }
            catch (Exception e)
            {
                Console.WriteLine("Backup Failed: " + e);
            }

        }
        #endregion

        #region Table Row to Object Methods
        private static TECSystem getSystemFromRow(DataRow row)
        {
            Guid systemID = new Guid(row[SystemTable.SystemID.Name].ToString());
            string name = row[SystemTable.Name.Name].ToString();
            string description = row[SystemTable.Description.Name].ToString();
            string quantityString = row[SystemTable.Quantity.Name].ToString();
            string budgetPriceString = row[SystemTable.BudgetPrice.Name].ToString();

            int quantity;
            if (!int.TryParse(quantityString, out quantity))
            {
                quantity = 1;
                Console.WriteLine("Cannot convert quantity to int in system, setting to 1");
            }

            double budgetPrice;
            if (!double.TryParse(budgetPriceString, out budgetPrice))
            {
                budgetPrice = -1;
                Console.WriteLine("Cannot convert budgetPrice to double, setting to -1");
            }

            ObservableCollection<TECEquipment> equipmentInSystem = getEquipmentInSystem(systemID);

            TECSystem system = new TECSystem(name, description, budgetPrice, equipmentInSystem, systemID);

            system.Quantity = quantity;
            system.Tags = getTagsInScope(systemID);

            return system;
        }

        private static TECEquipment getEquipmentFromRow(DataRow row)
        {
            Guid equipmentID = new Guid(row[EquipmentTable.EquipmentID.Name].ToString());
            string name = row[EquipmentTable.Name.Name].ToString();
            string description = row[EquipmentTable.Description.Name].ToString();
            string quantityString = row[EquipmentTable.Quantity.Name].ToString();
            string budgetPriceString = row[EquipmentTable.BudgetPrice.Name].ToString();

            double budgetPrice;
            if (!double.TryParse(budgetPriceString, out budgetPrice))
            {
                budgetPrice = -1;
                Console.WriteLine("Cannot convert budget price to double in equipment, setting to -1");
            }

            int quantity;
            if (!int.TryParse(quantityString, out quantity))
            {
                quantity = 1;
                Console.WriteLine("Cannot convert quantity to int in equipment, setting to 1");
            }

            ObservableCollection<TECSubScope> subScopeInEquipment = getSubScopeInEquipment(equipmentID);

            TECEquipment equipmentToAdd = new TECEquipment(name, description, budgetPrice, subScopeInEquipment, equipmentID);

            equipmentToAdd.Quantity = quantity;
            equipmentToAdd.Tags = getTagsInScope(equipmentID);

            return equipmentToAdd;
        }

        private static TECSubScope getSubScopeFromRow(DataRow row)
        {
            Guid subScopeID = new Guid(row[SubScopeTable.SubScopeID.Name].ToString());
            string name = row[SubScopeTable.Name.Name].ToString();
            string description = row[SubScopeTable.Description.Name].ToString();
            string quantityString = row[SubScopeTable.Quantity.Name].ToString();
            string lengthString = row[SubScopeTable.Length.Name].ToString();

            int quantity;
            if (!int.TryParse(quantityString, out quantity))
            {
                quantity = 1;
                Console.WriteLine("Cannot convert quantity to int in subscope, setting to 1");
            }

            double length;
            if (!double.TryParse(lengthString, out length))
            {
                length = 1;
                Console.WriteLine("Cannot convert length to double in subscope, setting to 1");
            }

            ObservableCollection<TECDevice> devicesInSubScope = getDevicesInSubScope(subScopeID);
            ObservableCollection<TECPoint> pointsInSubScope = getPointsInSubScope(subScopeID);

            TECSubScope subScopeToAdd = new TECSubScope(name, description, devicesInSubScope, pointsInSubScope, subScopeID);

            subScopeToAdd.Length = length;
            subScopeToAdd.Quantity = quantity;
            subScopeToAdd.Tags = getTagsInScope(subScopeID);

            return subScopeToAdd;
        }

        private static TECConnectionType getConnectionTypeFromRow(DataRow row)
        {
            Guid guid = new Guid(row[ConnectionTypeTable.ConnectionTypeID.Name].ToString());
            string name = row[ConnectionTypeTable.Name.Name].ToString();
            string laborString = row[ConnectionTypeTable.Labor.Name].ToString();
            string costString = row[ConnectionTypeTable.Cost.Name].ToString();

            double cost;
            if (!double.TryParse(costString, out cost))
            {
                cost = 0;
                Console.WriteLine("Cannot convert cost to double, setting to 0");
            }
            double labor;
            if (!double.TryParse(laborString, out labor))
            {
                labor = 0;
                Console.WriteLine("Cannot convert labor to double, setting to 0");
            }

            var outConnectionType = new TECConnectionType(guid);
            outConnectionType.Name = name;
            outConnectionType.Cost = cost;
            outConnectionType.Labor = labor;

            return outConnectionType;
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
            foreach(TECAssociatedCost associatedCost in bid.AssociatedCostCatalog)
            { addObject(associatedCost, bid); }
            foreach (TECNote note in bid.Notes)
            { addObject(note, bid); }
            foreach (TECExclusion exclusion in bid.Exclusions)
            { addObject(exclusion, bid); }
            foreach (TECLocation location in bid.Locations)
            { addObject(location, bid); }
            foreach (TECConduitType conduitType in bid.ConduitTypes)
            { addObject(conduitType, bid); }
            foreach (TECConnectionType connectionType in bid.ConnectionTypes)
            { addObject(connectionType, bid); }
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
                saveScopeChildProperties(subScope);
                saveCompleteDevices(subScope);
                saveCompletePoints(subScope);
            }
            foreach (TECDevice device in templates.DeviceCatalog)
            {
                addObject(device, templates);
                saveScopeChildProperties(device);
                saveDeviceChildProperties(device);
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
            { addObject(conduitType, templates); }
            foreach (TECConnectionType connectionType in templates.ConnectionTypeCatalog)
            { addObject(connectionType, templates); }
            foreach (TECAssociatedCost associatedCost in templates.AssociatedCostsCatalog)
            { addObject(associatedCost, templates); }
        }

        private static void saveCompleteDevices(TECSubScope subScope)
        {
            foreach(TECDevice device in subScope.Devices)
            {
                addObject(device, subScope);
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
                saveScopeChildProperties(subScope);
                saveCompleteDevices(subScope);
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
            foreach(TECConnection connection in controller.Connections)
            {
                addObject(connection, controller);
            }
            foreach(TECIO IO in controller.IO)
            {
                addObject(IO, controller);
            }
        }
        private static void saveDeviceChildProperties(TECDevice device)
        {
            addObject(device.Manufacturer, device);
            addObject(device.ConnectionType, device);
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
                    Console.WriteLine("Error: Couldn't add data to " + tableInfo.Name + " table.");
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
                //tableInfo.Item2 = AllTableFields;
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
                        Console.WriteLine("Error: Couldn't add data to " + tableInfo.Name + " table.");
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
                { Console.WriteLine("Error: Couldn't remove data from " + tableInfo.Name + " table."); }
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
                { Console.WriteLine("Error: Couldn't add data to " + tableInfo.Name + " table."); }
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

        #region Generic Load Methods
        //private static ObservableCollection<Object> loadObjects(Type typeToLoad)
        //{
        //    ObservableCollection<Object> loadedObjects = new ObservableCollection<Object>();
        //    List<TableBase> relevantTables = getRelevantTables(new object() as typeToLoad);

        //    DataTable exclusionsDT = SQLiteDB.getDataFromTable("TECExclusion");

        //    foreach (DataRow row in exclusionsDT.Rows)
        //    {
        //        Guid exclusionId = new Guid(row["ExclusionID"].ToString());
        //        string exclusionText = row["ExclusionText"].ToString();

        //        exclusions.Add(new TECExclusion(exclusionText, exclusionId));
        //    }

        //    return loadedObjects;
        //}
        #endregion

        #region Generic Helper Methods
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
                        if (DEBUG_GENERIC) { Console.WriteLine("Adding " + field.Name + " to table " + tableInfo.Name + " with type " + relevantObjects[currentField].GetType()); }
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
                    } else if (field.Property.Name == "Quantity" && field.Property.ReflectedType == typeof(HelperProperties))
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
                    if (DEBUG_GENERIC) { Console.WriteLine("Changing " + field.Name + " in table " + tableInfo.Name + " with type " + item.GetType()); }
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

            try
            {
                child = (childObject as TECScope);
                parent = (parentObject as TECScope);
            }
            catch (Exception e)
            {
                throw new InvalidCastException(e.Message);
            }

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
