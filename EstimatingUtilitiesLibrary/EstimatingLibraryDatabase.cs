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

namespace EstimatingUtilitiesLibrary
{
    public static class EstimatingLibraryDatabase
    {
        //FMT is used by DateTime to convert back and forth between the DateTime type and string
        private const string DB_FMT = "O";
        //private const bool DEBUG = true;

        static private SQLiteDatabase SQLiteDB;

        #region Public Functions
        static public TECBid LoadDBToBid(string path, TECTemplates templates)
        {
            SQLiteDB = new SQLiteDatabase(path);

            checkAndUpdateDB(typeof(TECBid));

            TECBid bid = new TECBid();

            try
            {

                //Update catalogs from templates.
                if (templates.DeviceCatalog.Count > 0)
                {
                    foreach (TECDevice device in templates.DeviceCatalog)
                    {
                        editDevice(device);
                    }
                }

                if (templates.ManufacturerCatalog.Count > 0)
                {
                    foreach (TECManufacturer manufacturer in templates.ManufacturerCatalog)
                    {
                        editManufacturer(manufacturer);
                    }
                }

                if (templates.Tags.Count > 0)
                {
                    foreach (TECTag tag in templates.Tags)
                    {
                        editTag(tag);
                    }
                }

                bid = getBidInfo();
                bid.ScopeTree = getBidScopeBranches();
                bid.Systems = getAllSystemsInBid();
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
                linkAllVisualScope(bid.Drawings, bid.Systems, bid.Controllers);
                linkAllLocations(bid.Locations, bid.Systems);
                linkAllConnections(bid.Connections, bid.Controllers, bid.Systems);
            //Breaks Visual Scope in a page
            //populatePageVisualConnections(bid.Drawings, bid.Connections);
            }
            catch (Exception e)
            {
                MessageBox.Show("Could not load bid from database. Error: " + e.Message);
            }

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
            {
                SQLiteDB.overwriteFile();
            }

            createAllBidTables();
            
            try
            {
            
                foreach (TECDevice device in bid.DeviceCatalog)
                {
                    addDevice(device);
                    addDeviceManufacturerRelation(device, device.Manufacturer);
                    addTagsInScope(device.Tags, device.Guid);
                }

                foreach (TECManufacturer manufacturer in bid.ManufacturerCatalog)
                {
                    addManufacturer(manufacturer);
                }

                addTags(bid.Tags);

                addBidInfo(bid);

                addFullSystems(bid.Systems);

                foreach (TECScopeBranch branch in bid.ScopeTree)
                {
                    addScopeTree(branch);
                }
                
                foreach (TECNote note in bid.Notes)
                {
                    addNote(note);
                }

                foreach (TECExclusion exclusion in bid.Exclusions)
                {
                    addExclusion(exclusion);
                }

                foreach (TECDrawing drawing in bid.Drawings)
                {
                    addDrawing(drawing);
                    foreach (TECPage page in drawing.Pages)
                    {
                        addPage(page);
                        addDrawingPageRelation(drawing, page);
                        foreach(TECVisualScope vs in page.PageScope)
                        {
                            addVisualScope(vs);
                            addPageVisualScopeRelation(page, vs);
                            addVisualScopeScopeRelation(vs);
                        }
                    }
                }
                foreach (TECLocation location in bid.Locations)
                {
                    addLocation(location);
                }

                foreach (TECConnection connection in bid.Connections)
                {
                    addConnection(connection);
                    foreach(TECScope scope in connection.Scope)
                    {
                        addScopeConnectionRelation(scope, connection);
                    }
                    addControllerConnectionRelation(connection.Controller, connection);
                }
                foreach(TECController controller in bid.Controllers)
                {
                    addController(controller);
                }

                foreach(TECProposalScope propScope in bid.ProposalScope)
                {
                    addProposalScope(propScope);
                }
                
            }
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
            {
                SQLiteDB.overwriteFile();
            }
            
            createAllTemplateTables();

            try
            {
                addTemplatesInfo(templates);
                addTags(templates.Tags);
                addFullSystems(templates.SystemTemplates);
                foreach (TECEquipment equipment in templates.EquipmentTemplates)
                {
                    addFullEquipment(equipment);
                }
                foreach (TECSubScope subScope in templates.SubScopeTemplates)
                {
                    addFullSubScope(subScope);
                }
                foreach (TECDevice device in templates.DeviceCatalog)
                {
                    addDevice(device);
                    addDeviceManufacturerRelation(device, device.Manufacturer);
                    addTagsInScope(device.Tags, device.Guid);
                }
                foreach (TECManufacturer manufacturer in templates.ManufacturerCatalog)
                {
                    addManufacturer(manufacturer);
                }
                foreach (TECController controller in templates.ControllerTemplates)
                {
                    addController(controller);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Could not save templates to new database. Error: " + e.Message);
            }

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
            if (tarObject is TECSystem)
            {
                if (refObject is TECBid)
                {
                    //Console.WriteLine("Num systems to update: " + (refObject as TECBid).Systems.Count);
                    addSystem(tarObject as TECSystem);
                    updateSystemIndexes((refObject as TECBid).Systems);
                }
                else if (refObject is TECTemplates)
                {
                    addSystem(tarObject as TECSystem);
                }
            }
            else if (tarObject is TECEquipment)
            {
                addEquipment(tarObject as TECEquipment);
                if (refObject is TECSystem)
                {
                    updateSystemEquipmentRelation(refObject as TECSystem);
                }
            }
            else if (tarObject is TECSubScope)
            {
                addSubScope(tarObject as TECSubScope);
                if (refObject is TECEquipment)
                {
                    updateEquipmentSubScopeRelation(refObject as TECEquipment);
                }
            }
            else if (tarObject is TECDevice)
            {
                if (refObject is TECSubScope)
                {
                    updateSubScopeDeviceRelation(refObject as TECSubScope);
                }
                else if (refObject is TECBid)
                {
                    addDevice(tarObject as TECDevice);
                }
                else if (refObject is TECTemplates)
                {
                    addDevice(tarObject as TECDevice);
                }
            }
            else if (tarObject is TECPoint)
            {
                addPoint(tarObject as TECPoint);
                updateSubScopePointRelation(refObject as TECSubScope);
            }
            else if (tarObject is TECDrawing)
            {
                addDrawing(tarObject as TECDrawing);
            }
            else if (tarObject is TECPage)
            {
                addPage(tarObject as TECPage);
                addDrawingPageRelation(refObject as TECDrawing, tarObject as TECPage);
            }
            else if (tarObject is TECVisualScope)
            {
                addVisualScope(tarObject as TECVisualScope);
                addPageVisualScopeRelation(refObject as TECPage, tarObject as TECVisualScope);
                addVisualScopeScopeRelation(tarObject as TECVisualScope);
            }
            else if (tarObject is TECNote)
            {
                addNote(tarObject as TECNote);
            }
            else if (tarObject is TECExclusion)
            {
                addExclusion(tarObject as TECExclusion);
            }
            else if (tarObject is TECScopeBranch)
            {
                addScopeBranch(tarObject as TECScopeBranch);
                if (refObject is TECScopeBranch)
                {
                    addScopeTreeRelation(refObject as TECScopeBranch, tarObject as TECScopeBranch);
                }
            }
            else if (tarObject is TECManufacturer)
            {
                if (refObject is TECDevice)
                {
                    addDeviceManufacturerRelation(refObject as TECDevice, tarObject as TECManufacturer);
                }
                else if (refObject is TECBid)
                {
                    addManufacturer(tarObject as TECManufacturer);
                }
                else if (refObject is TECTemplates)
                {
                    addManufacturer(tarObject as TECManufacturer);
                }
            }
            else if (tarObject is TECLocation)
            {
                if (refObject is TECBid)
                {
                    addLocation(tarObject as TECLocation);
                }
                else
                {
                    addLocationInScope(refObject as TECScope);
                }
            }
            else if (tarObject is TECController)
            {
                addController(tarObject as TECController);
            }
            else if (tarObject is TECTag)
            {
                if (refObject is TECScope)
                {
                    addTagInScope((tarObject as TECTag), (refObject as TECScope).Guid);
                }
                else if (refObject is TECTemplates)
                {
                    addTag(tarObject as TECTag);
                }
            }
            else if (tarObject is TECProposalScope)
            {
                addProposalScope(tarObject as TECProposalScope);
            }
            else
            {
                Console.WriteLine("Target object type not included in add branch. Target object type: " + tarObject.GetType());
                throw new NotImplementedException();
            }
        }

        static private void editUpdate(object tarObject, object refObject)
        {
            if (tarObject is TECBid)
            {
                editBidInfo(tarObject as TECBid);
            }
            else if (tarObject is TECLabor)
            {
                if (refObject is TECBid)
                {
                    editBidInfo(refObject as TECBid);
                }
            }
            else if (tarObject is TECSystem)
            {
                editSystem(tarObject as TECSystem);
            }
            else if (tarObject is TECEquipment)
            {
                editEquipment(tarObject as TECEquipment);
            }
            else if (tarObject is TECSubScope)
            {
                editSubScope(tarObject as TECSubScope);
            }
            else if (tarObject is TECDevice)
            {
                if (refObject is TECDevice)
                {
                    editDevice(tarObject as TECDevice);
                }
                else if (refObject is TECSubScope)
                {
                    editDeviceQuantity(refObject as TECSubScope, tarObject as TECDevice);
                }
            }
            else if (tarObject is TECPoint)
            {
                editPoint(tarObject as TECPoint);
            }
            else if (tarObject is TECManufacturer)
            {
                if (refObject is TECDevice)
                {
                    editManufacturerInDevice(refObject as TECDevice);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else if (tarObject is TECDrawing)
            {
                editDrawing(tarObject as TECDrawing);
            }
            else if (tarObject is TECPage)
            {
                throw new NotImplementedException();
            }
            else if (tarObject is TECVisualScope)
            {
                editVisualScope(tarObject as TECVisualScope);
            }
            else if (tarObject is TECNote)
            {
                editNote(tarObject as TECNote);
            }
            else if (tarObject is TECExclusion)
            {
                editExclusion(tarObject as TECExclusion);
            }
            else if (tarObject is TECScopeBranch)
            {
                editScopeBranch(tarObject as TECScopeBranch);
            }
            else if (tarObject is TECLocation)
            {
                editLocation(tarObject as TECLocation);
            }
            else if (tarObject is TECController)
            {
                editController(tarObject as TECController);
            }
            else if (tarObject is ObservableCollection<TECSystem>)
            {
                updateSystemIndexes(tarObject as ObservableCollection<TECSystem>);
            }
            else if (tarObject is ObservableCollection<TECEquipment>)
            {
                updateSystemEquipmentRelation(refObject as TECSystem);
            }
            else if (tarObject is ObservableCollection<TECSubScope>)
            {
                updateEquipmentSubScopeRelation(refObject as TECEquipment);
            }
            else if (tarObject is ObservableCollection<TECDevice>)
            {
                updateSubScopeDeviceRelation(refObject as TECSubScope);
            }
            else if (tarObject is ObservableCollection<TECPoint>)
            {
                updateSubScopePointRelation(refObject as TECSubScope);
            }
            else
            {
                Console.WriteLine("Target object type not included in edit branch. Target object type: " + tarObject.GetType());
                throw new NotImplementedException();
            }
        }

        static private void removeUpdate(object tarObject, object refObject)
        {
            if (tarObject is TECSystem)
            {
                removeSystem(tarObject as TECSystem);
                removeLocationInScope(tarObject as TECSystem);
            }
            else if (tarObject is TECEquipment)
            {
                removeEquipment(tarObject as TECEquipment);
                removeLocationInScope(tarObject as TECEquipment);
                if (refObject is TECSystem)
                {
                    removeSystemEquipmentRelation(tarObject as TECEquipment);
                }
            }
            else if (tarObject is TECSubScope)
            {
                removeSubScope(tarObject as TECSubScope);
                removeLocationInScope(tarObject as TECSubScope);
                if (refObject is TECEquipment)
                {
                    removeEquipmentSubScopeRelation(tarObject as TECSubScope);
                }
            }
            else if (tarObject is TECDevice)
            {
                if (refObject is TECSubScope)
                {
                    removeSubScopeDeviceRelation(tarObject as TECDevice);
                }
                else if (refObject is TECTemplates)
                {
                    removeDevice(tarObject as TECDevice);
                }
            }
            else if (tarObject is TECPoint)
            {
                removePoint(tarObject as TECPoint);
                if (refObject is TECSubScope)
                {
                    removeSubScopePointRelation(tarObject as TECPoint);
                }
            }
            else if (tarObject is TECDrawing)
            {
                throw new NotImplementedException();
            }
            else if (tarObject is TECPage)
            {
                throw new NotImplementedException();
            }
            else if (tarObject is TECVisualScope)
            {
                removeVisualScope(tarObject as TECVisualScope);
                removePageVisualScopeRelation(tarObject as TECVisualScope);
            }
            else if (tarObject is TECNote)
            {
                removeNote(tarObject as TECNote);
            }
            else if (tarObject is TECExclusion)
            {
                removeExclusion(tarObject as TECExclusion);
            }
            else if (tarObject is TECScopeBranch)
            {
                removeScopeBranch(tarObject as TECScopeBranch);
                if (refObject is TECScopeBranch)
                {
                    removeScopeBranchHierarchyRelation(tarObject as TECScopeBranch);
                }
                else if (refObject is TECBid)
                {
                    removeScopeBranchBidRelation(tarObject as TECScopeBranch);
                }
                else if (refObject is TECProposalScope)
                {
                    removeScopeBranchProposalScope(tarObject as TECScopeBranch, refObject as TECProposalScope);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else if (tarObject is TECLocation)
            {
                if (refObject is TECBid)
                {
                    removeLocation(tarObject as TECLocation);
                }
                else
                {
                    removeLocationInScope(refObject as TECScope);
                }
            }
            else if (tarObject is TECController)
            {
                removeController(tarObject as TECController);
            }
            else if(tarObject is TECProposalScope)
            {
                removeProposalScope(tarObject as TECProposalScope);
            }
            else
            {
                Console.WriteLine("Target object type not included in remove branch. Target object type: " + tarObject.GetType());
                throw new NotImplementedException();
            }
        }

        #endregion Update Functions

        #region Saving to DB Methods

        #region Add Methods
        static private void addBidInfo(TECBid bid)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add(BidInfoTable.DBVersion.Name, Properties.Settings.Default.Version);
            data.Add(BidInfoTable.BidInfoID.Name, bid.InfoGuid.ToString());
            data.Add(BidInfoTable.BidName.Name, bid.Name);
            data.Add(BidInfoTable.BidNumber.Name, bid.BidNumber);
            data.Add(BidInfoTable.DueDate.Name, bid.DueDate.ToString(DB_FMT));
            data.Add(BidInfoTable.Salesperson.Name, bid.Salesperson);
            data.Add(BidInfoTable.Estimator.Name, bid.Estimator);

            data.Add(BidInfoTable.PMCoef.Name, bid.Labor.PMCoef.ToString());
            data.Add(BidInfoTable.ENGCoef.Name, bid.Labor.ENGCoef.ToString());
            data.Add(BidInfoTable.CommCoef.Name, bid.Labor.CommCoef.ToString());
            data.Add(BidInfoTable.SoftCoef.Name, bid.Labor.SoftCoef.ToString());
            data.Add(BidInfoTable.GraphCoef.Name, bid.Labor.GraphCoef.ToString());
            data.Add(BidInfoTable.ElectricalRate.Name, bid.Labor.ElectricalRate.ToString());

            if (!SQLiteDB.Insert(BidInfoTable.TableName, data))
            {
                Console.WriteLine("Error: Couldn't add item to TECBidInfo table.");
            }
        }

        static private void addTemplatesInfo(TECTemplates templates)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add(TemplatesInfoTable.TemplatesInfoID.Name, templates.InfoGuid.ToString());
            data.Add(TemplatesInfoTable.DBVersion.Name, Properties.Settings.Default.Version);

            if (!SQLiteDB.Insert(TemplatesInfoTable.TableName, data))
            {
                Console.WriteLine("Error: Couldn't add item to TECTemplatesInfo table.");
            }
        }

        static private void addFullSystems(ObservableCollection<TECSystem> systems)
        {
            updateSystemIndexes(systems);
            foreach (TECSystem system in systems)
            {
                addTagsInScope(system.Tags, system.Guid);
                addLocationInScope(system);
                addSystem(system);
                foreach (TECEquipment equip in system.Equipment)
                {
                    addFullEquipment(equip);
                }
                updateSystemEquipmentRelation(system);
            }
        }

        static private void addFullEquipment(TECEquipment equipment)
        {
            addEquipment(equipment);
            addTagsInScope(equipment.Tags, equipment.Guid);
            addLocationInScope(equipment);
            foreach (TECSubScope ss in equipment.SubScope)
            {
                addFullSubScope(ss);
            }
            updateEquipmentSubScopeRelation(equipment);
        }

        static private void addFullSubScope(TECSubScope subScope)
        {
            addSubScope(subScope);
            addTagsInScope(subScope.Tags, subScope.Guid);
            addLocationInScope(subScope);
            updateSubScopeDeviceRelation(subScope);
            foreach (TECPoint point in subScope.Points)
            {
                addPoint(point);
                addTagsInScope(point.Tags, point.Guid);
            }
            updateSubScopePointRelation(subScope);
        }

        static private void addScopeTree(TECScopeBranch trunk)
        {
            addScopeBranch(trunk);
            foreach (TECScopeBranch branch in trunk.Branches)
            {
                addScopeTreeRelation(trunk, branch);
                addScopeTree(branch);
            }
        }

        #region Add Object Functions

        static private void addSystem(TECSystem system)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add(SystemTable.SystemID.Name, system.Guid.ToString());
            data.Add(SystemTable.Name.Name, system.Name);
            data.Add(SystemTable.Description.Name, system.Description);
            data.Add(SystemTable.Quantity.Name, system.Quantity.ToString());
            data.Add(SystemTable.BudgetPrice.Name, system.BudgetPrice.ToString());

            if (!SQLiteDB.Insert(SystemTable.TableName, data))
            {
                Console.WriteLine("Error: Couldn't add system to TECSystem table.");
            }
        }
        static private void addEquipment(TECEquipment equipment)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add(EquipmentTable.EquipmentID.Name, equipment.Guid.ToString());
            data.Add(EquipmentTable.Name.Name, equipment.Name);
            data.Add(EquipmentTable.Description.Name, equipment.Description);
            data.Add(EquipmentTable.Quantity.Name, equipment.Quantity.ToString());
            data.Add(EquipmentTable.BudgetPrice.Name, equipment.BudgetPrice.ToString());

            if (!SQLiteDB.Insert(EquipmentTable.TableName, data))
            {
                Console.WriteLine("Error: Couldn't add equipment to TECEquipment table.");
            }
        }
        static private void addSubScope(TECSubScope subScope)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add(SubScopeTable.SubScopeID.Name, subScope.Guid.ToString());
            data.Add(SubScopeTable.Name.Name, subScope.Name);
            data.Add(SubScopeTable.Description.Name, subScope.Description);
            data.Add(SubScopeTable.Quantity.Name, subScope.Quantity.ToString());

            if (!SQLiteDB.Insert(SubScopeTable.TableName, data))
            {
                Console.WriteLine("Error: Couldn't add subScope to TECSubScope table.");
            }
        }
        static private void addDevice(TECDevice device)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add(DeviceTable.DeviceID.Name, device.Guid.ToString());
            data.Add(DeviceTable.Name.Name, device.Name);
            data.Add(DeviceTable.Description.Name, device.Description);
            data.Add(DeviceTable.Cost.Name, device.Cost.ToString());
            data.Add(DeviceTable.ConnectionType.Name, TECConnectionType.convertTypeToString(device.ConnectionType));

            if (!SQLiteDB.Insert(DeviceTable.TableName, data))
            {
                Console.WriteLine("Error: Couldn't add device to TECDevice table.");
            }
        }
        static private void addManufacturer(TECManufacturer manufacturer)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add(ManufacturerTable.ManufacturerID.Name, manufacturer.Guid.ToString());
            data.Add(ManufacturerTable.Name.Name, manufacturer.Name);
            data.Add(ManufacturerTable.Multiplier.Name, manufacturer.Multiplier.ToString());

            if (!SQLiteDB.Insert(ManufacturerTable.TableName, data))
            {
                Console.WriteLine("Error: Couldn't add manufacturer to TECManufacturer table.");
            }
        }
        static private void addPoint(TECPoint point)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add(PointTable.PointID.Name, point.Guid.ToString());
            data.Add(PointTable.Name.Name, point.Name);
            data.Add(PointTable.Description.Name, point.Description);
            data.Add(PointTable.Type.Name, point.Type.ToString());
            data.Add(PointTable.Quantity.Name, point.Quantity.ToString());

            if (!SQLiteDB.Insert(PointTable.TableName, data))
            {
                Console.WriteLine("Error: Couldn't add point to TECPoint table.");
            }
        }
        static private void addScopeBranch(TECScopeBranch branch)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add(ScopeBranchTable.ScopeBranchID.Name, branch.Guid.ToString());
            data.Add(ScopeBranchTable.Name.Name, branch.Name);
            data.Add(ScopeBranchTable.Description.Name, branch.Description);

            if (!SQLiteDB.Insert(ScopeBranchTable.TableName, data))
            {
                Console.WriteLine("Error: Couldn't add item to TECScopeBranch table");
            }
        }
        static private void addTags(ObservableCollection<TECTag> tags)
        {
            foreach (TECTag tag in tags)
            {
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add(TagTable.TagID.Name, tag.Guid.ToString());
                data.Add(TagTable.TagString.Name, tag.Text);

                if (!SQLiteDB.Insert(TagTable.TableName, data))
                {
                    Console.WriteLine("Error: Couldn't add tag to TECTag table.");
                }
            }
        }
        static private void addTag(TECTag tag)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add(TagTable.TagID.Name, tag.Guid.ToString());
            data.Add(TagTable.TagString.Name, tag.Text);

            if (!SQLiteDB.Insert(TagTable.TableName, data))
            {
                Console.WriteLine("Error: Couldn't add item to TECNote table");
            }
        }
        static private void addNote(TECNote note)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add(NoteTable.NoteID.Name, note.Guid.ToString());
            data.Add(NoteTable.NoteText.Name, note.Text);

            if (!SQLiteDB.Insert(NoteTable.TableName, data))
            {
                Console.WriteLine("Error: Couldn't add item to TECNote table");
            }
        }
        static private void addExclusion(TECExclusion exclusion)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add(ExclusionTable.ExclusionID.Name, exclusion.Guid.ToString());
            data.Add(ExclusionTable.ExclusionText.Name, exclusion.Text);

            if (!SQLiteDB.Insert(ExclusionTable.TableName, data))
            {
                Console.WriteLine("Error: Couldn't add item to TECExclusion table.");
            }
        }
        static private void addDrawing(TECDrawing drawing)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add(DrawingTable.DrawingID.Name, drawing.Guid.ToString());
            data.Add(DrawingTable.Name.Name, drawing.Name);
            data.Add(DrawingTable.Description.Name, drawing.Description);
            if (!SQLiteDB.Insert(DrawingTable.TableName, data))
            {
                Console.WriteLine("Error: Couldn't add item to TECDrawing table.");
            }
        }
        static private void addPage(TECPage page)
        {
            byte[] imageBytes = File.ReadAllBytes(page.Path);

            Dictionary<string, string> stringData = new Dictionary<string, string>();
            stringData.Add(PageTable.PageID.Name, page.Guid.ToString());
            stringData.Add(PageTable.PageNum.Name, page.PageNum.ToString());

            Dictionary<string, byte[]> byteData = new Dictionary<string, byte[]>();
            byteData.Add(PageTable.Image.Name, imageBytes);

            if (!SQLiteDB.Insert(PageTable.TableName, stringData, byteData))
            {
                Console.WriteLine("Error: Couldn't add item to TECPageTable.");
            }
        }
        static private void addVisualScope(TECVisualScope visualScope)
        {
            Dictionary<string, string> vsData = new Dictionary<string, string>();
            vsData.Add(VisualScopeTable.VisualScopeID.Name, visualScope.Guid.ToString());
            vsData.Add(VisualScopeTable.XPos.Name, visualScope.X.ToString());
            vsData.Add(VisualScopeTable.YPos.Name, visualScope.Y.ToString());

            if (!SQLiteDB.Insert(VisualScopeTable.TableName, vsData))
            {
                Console.WriteLine("Error: Couldn't add item to TECVisualScopeTable.");
            }
        }
        static private void addProposalScope(TECProposalScope proposalScope)
        {
            Dictionary<string, string> psData = new Dictionary<string, string>();
            psData.Add(ProposalScopeTable.ProposalScopeID.Name, proposalScope.Scope.Guid.ToString());
            psData.Add(ProposalScopeTable.IsProposed.Name, proposalScope.IsProposed.ToInt().ToString());

            if (!SQLiteDB.Insert(ProposalScopeTable.TableName, psData))
            {
                Console.WriteLine("Error: Couldn't add item to TECProposalScopeTable.");
            }
            else
            {
                foreach(TECScopeBranch branch in proposalScope.Notes)
                {
                    addScopeTree(branch);
                    addScopeBranchInProposalScope(branch.Guid, proposalScope.Scope.Guid);
                }
                foreach(TECProposalScope child in proposalScope.Children)
                {
                    addProposalScope(child);
                }
            }
        }
        static private void addLocation(TECLocation location)
        {
            Dictionary<string, string> locData = new Dictionary<string, string>();
            locData.Add(LocationTable.LocationID.Name, location.Guid.ToString());
            locData.Add(LocationTable.Name.Name, location.Name.ToString());

            if (!SQLiteDB.Insert(LocationTable.TableName, locData))
            {
                Console.WriteLine("Error: Couldn't add item to TECLocationTable.");
            }
        }
        static private void addController(TECController controller)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add(ControllerTable.ControllerID.Name, controller.Guid.ToString());
            data.Add(ControllerTable.Name.Name, controller.Name);
            data.Add(ControllerTable.Description.Name, controller.Description);
            data.Add(ControllerTable.Cost.Name, controller.Cost.ToString());
            
            foreach(TECIO io in controller.IO)
            {
                
               addControllerIORelation(controller, io.Type.ToString(), io.Quantity);
               
            }


            if (!SQLiteDB.Insert(ControllerTable.TableName, data))
            {
                Console.WriteLine("Error: Couldn't add item to TECController table.");
            }
            

        }

        static private void addConnection(TECConnection connection)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add(ConnectionTable.ConnectionID.Name, connection.Guid.ToString());
            data.Add(ConnectionTable.Length.Name, connection.Length.ToString());
            //data.Add(ConnectionTable.Type.Name, connection.Type.ToString());

            if (!SQLiteDB.Insert(ConnectionTable.TableName, data))
            {
                Console.WriteLine("Error: Couldn't add item to TECConnection table.");
            }
        }

        #endregion Add Object Functions

        #region Add Relation Functions
        
        static private void addDeviceManufacturerRelation(TECDevice device, TECManufacturer manufacturer)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add(DeviceManufacturerTable.DeviceID.Name, device.Guid.ToString());
            data.Add(DeviceManufacturerTable.ManufacturerID.Name, manufacturer.Guid.ToString());

            if (!SQLiteDB.Insert(DeviceManufacturerTable.TableName, data))
            {
                Console.WriteLine("Error: Couldn't add relation to TECDeviceTECManufacturer table.");
            }
        }
        static private void addScopeTreeRelation(TECScopeBranch parentBranch, TECScopeBranch childBranch)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add(ScopeBranchHierarchyTable.ParentID.Name, parentBranch.Guid.ToString());
            data.Add(ScopeBranchHierarchyTable.ChildID.Name, childBranch.Guid.ToString());

            if (!SQLiteDB.Insert("TECScopeBranchHierarchy", data))
            {
                Console.WriteLine("Error: Couldn't add relation to TECScopeBranchHierarhy table.");
            }
        }
        static private void addTagsInScope(ObservableCollection<TECTag> tags, Guid scopeID)
        {
            foreach (TECTag tag in tags)
            {
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add(ScopeTagTable.ScopeID.Name, scopeID.ToString());
                data.Add(ScopeTagTable.TagID.Name, tag.Guid.ToString());

                if (!SQLiteDB.Insert(ScopeTagTable.TableName, data))
                {
                    Console.WriteLine("Error: Couldn't add relation to TECScopeTECTag table.");
                }
            }
        }
        static private void addTagInScope(TECTag tag, Guid scopeID)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add(ScopeTagTable.ScopeID.Name, scopeID.ToString());
            data.Add(ScopeTagTable.TagID.Name, tag.Guid.ToString());

            if (!SQLiteDB.Insert(ScopeTagTable.TableName, data))
            {
                Console.WriteLine("Error: Couldn't add relation to TECScopeTECTag table.");
            }
        }
        static private void addDrawingPageRelation(TECDrawing drawing, TECPage page)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add(DrawingPageTable.DrawingID.Name, drawing.Guid.ToString());
            data.Add(DrawingPageTable.PageID.Name, page.Guid.ToString());

            if (!SQLiteDB.Insert(DrawingPageTable.TableName, data))
            {
                Console.WriteLine("Error: Couldn't add relation to TECDrawingTECPage table.");
            }
        }
        static private void addPageVisualScopeRelation(TECPage page, TECVisualScope vs)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add(PageVisualScopeTable.PageID.Name, page.Guid.ToString());
            data.Add(PageVisualScopeTable.VisualScopeID.Name, vs.Guid.ToString());

            if (!SQLiteDB.Insert(PageVisualScopeTable.TableName, data))
            {
                Console.WriteLine("Error: Couldn't add relation to TECPageTECScope table.");
            }
        }
        static private void addVisualScopeScopeRelation(TECVisualScope vs)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add(VisualScopeScopeTable.VisualScopeID.Name, vs.Guid.ToString());
            data.Add(VisualScopeScopeTable.ScopeID.Name, vs.Scope.Guid.ToString());

            if (!SQLiteDB.Insert(VisualScopeScopeTable.TableName, data))
            {
                Console.WriteLine("Could not add relation to TECVisualScopeTECScope table.");
            }
        }

        static private void addLocationInScope(TECScope scope)
        {
            if (scope.Location != null)
            {
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add(LocationScopeTable.ScopeID.Name, scope.Guid.ToString());
                data.Add(LocationScopeTable.LocationID.Name, scope.Location.Guid.ToString());

                if (!SQLiteDB.Insert(LocationScopeTable.TableName, data))
                {
                    Console.WriteLine("Error: Couldn't add relation to TECLocationTECScope table.");
                }
            }
        }

        static private void addControllerConnectionRelation(TECController controller, TECConnection connection)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add(ControllerConnectionTable.ControllerID.Name, controller.Guid.ToString());
            data.Add(ControllerConnectionTable.ConnectionID.Name, connection.Guid.ToString());

            if (!SQLiteDB.Insert(ControllerConnectionTable.TableName, data))
            {
                Console.WriteLine("Error: Couldn't add relation to TECControllerTECConnection table.");
            }
        }

        static private void addScopeConnectionRelation(TECScope scope, TECConnection connection)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add(ScopeConnectionTable.ScopeID.Name, scope.Guid.ToString());
            data.Add(ScopeConnectionTable.ConnectionID.Name, connection.Guid.ToString());

            if (!SQLiteDB.Insert(ScopeConnectionTable.TableName, data))
            {
                Console.WriteLine("Error: Couldn't add relation to TECScopeTECConnection table.");
            }
        }

        static private void addControllerIORelation(TECController controller, string typeString, int qty)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add(ControllerIOTypeTable.ControllerID.Name, controller.Guid.ToString());
            data.Add(ControllerIOTypeTable.IOType.Name, typeString);
            data.Add(ControllerIOTypeTable.Quantity.Name, qty.ToString());

            if (!SQLiteDB.Insert(ControllerIOTypeTable.TableName, data))
            {
                Console.WriteLine("Error: Couldn't add relation to TECControllerTECConnectionType table.");
            }
        }
        
        static private void addScopeBranchInProposalScope(Guid branchID, Guid propScopeID)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add(ProposalScopeScopeBranchTable.ScopeBranchID.Name, branchID.ToString());
            data.Add(ProposalScopeScopeBranchTable.ProposalScopeID.Name, propScopeID.ToString());

            if (!SQLiteDB.Insert(ProposalScopeScopeBranchTable.TableName, data))
            {
                Console.WriteLine("Error: Couldn't add relation to TECProposalScopeTECScopeBranch table.");
            }
        }

        #endregion Add Relation Functions
        #endregion Add Methods

        #region Edit Methods
        static private void editBidInfo(TECBid bid)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add(BidInfoTable.BidInfoID.Name, bid.InfoGuid.ToString());
            data.Add(BidInfoTable.BidName.Name, bid.Name);
            data.Add(BidInfoTable.BidNumber.Name, bid.BidNumber);
            data.Add(BidInfoTable.DueDate.Name, bid.DueDate.ToString(DB_FMT));
            data.Add(BidInfoTable.Salesperson.Name, bid.Salesperson);
            data.Add(BidInfoTable.Estimator.Name, bid.Estimator);
            data.Add(BidInfoTable.PMCoef.Name, bid.Labor.PMCoef.ToString());
            data.Add(BidInfoTable.ENGCoef.Name, bid.Labor.ENGCoef.ToString());
            data.Add(BidInfoTable.CommCoef.Name, bid.Labor.CommCoef.ToString());
            data.Add(BidInfoTable.SoftCoef.Name, bid.Labor.SoftCoef.ToString());
            data.Add(BidInfoTable.GraphCoef.Name, bid.Labor.GraphCoef.ToString());
            data.Add(BidInfoTable.ElectricalRate.Name, bid.Labor.ElectricalRate.ToString());

            if (!SQLiteDB.Replace(BidInfoTable.TableName, data))
            {
                Console.WriteLine("Error: Couldn't update bid info in TECBidInfo table.");
            }
        }

        static private void editSystem(TECSystem system)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add(SystemTable.SystemID.Name, system.Guid.ToString());
            data.Add(SystemTable.Name.Name, system.Name);
            data.Add(SystemTable.Description.Name, system.Description);
            data.Add(SystemTable.Quantity.Name, system.Quantity.ToString());
            data.Add(SystemTable.BudgetPrice.Name, system.BudgetPrice.ToString());

            if (!SQLiteDB.Replace(SystemTable.TableName, data))
            {
                Console.WriteLine("Error: Couldn't update system in TECSystem table");
            }
        }

        static private void editEquipment(TECEquipment equipment)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add(EquipmentTable.EquipmentID.Name, equipment.Guid.ToString());
            data.Add(EquipmentTable.Name.Name, equipment.Name);
            data.Add(EquipmentTable.Description.Name, equipment.Description);
            data.Add(EquipmentTable.Quantity.Name, equipment.Quantity.ToString());
            data.Add(EquipmentTable.BudgetPrice.Name, equipment.BudgetPrice.ToString());

            if (!SQLiteDB.Replace(EquipmentTable.TableName, data))
            {
                Console.WriteLine("Error: Couldn't update equipment in TECEquipment table.");
            }
        }
        
        static private void editSubScope(TECSubScope subScope)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add(SubScopeTable.SubScopeID.Name, subScope.Guid.ToString());
            data.Add(SubScopeTable.Name.Name, subScope.Name);
            data.Add(SubScopeTable.Description.Name, subScope.Description);
            data.Add(SubScopeTable.Quantity.Name, subScope.Quantity.ToString());

            if (!SQLiteDB.Replace(SubScopeTable.TableName, data))
            {
                Console.WriteLine("Error: Couldn't update subScope in TECSubScope table.");
            }
        }

        static private void editDevice(TECDevice device)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add(DeviceTable.DeviceID.Name, device.Guid.ToString());
            data.Add(DeviceTable.Name.Name, device.Name);
            data.Add(DeviceTable.Description.Name, device.Description);
            data.Add(DeviceTable.Cost.Name, device.Cost.ToString());
            data.Add(DeviceTable.ConnectionType.Name, TECConnectionType.convertTypeToString(device.ConnectionType));

            if (!SQLiteDB.Replace(DeviceTable.TableName, data))
            {
                Console.WriteLine("Error: Couldn't update device in TECDevice table.");
            }
        }

        static private void editDeviceQuantity(TECSubScope subScope, TECDevice device)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add(SubScopeDeviceTable.SubScopeID.Name, subScope.Guid.ToString());
            data.Add(SubScopeDeviceTable.DeviceID.Name, device.Guid.ToString());
            data.Add(SubScopeDeviceTable.Quantity.Name, device.Quantity.ToString());

            if (!SQLiteDB.Replace(SubScopeDeviceTable.TableName, data))
            {
                Console.WriteLine("Error: Couldn't update SubScopeDevice relation in TECSubScopeTECDevice.");
            }
        }

        static private void editManufacturer(TECManufacturer manufacturer)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add(ManufacturerTable.ManufacturerID.Name, manufacturer.Guid.ToString());
            data.Add(ManufacturerTable.Name.Name, manufacturer.Name);
            data.Add(ManufacturerTable.Multiplier.Name, manufacturer.Multiplier.ToString());

            if (!SQLiteDB.Replace(ManufacturerTable.TableName, data))
            {
                Console.WriteLine("Error: Couldn't update manufacturer in TECManufacturer table.");
            }
        }

        static private void editManufacturerInDevice(TECDevice device)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add(DeviceManufacturerTable.DeviceID.Name, device.Guid.ToString());
            data.Add(DeviceManufacturerTable.ManufacturerID.Name, device.Manufacturer.Guid.ToString());

            if (!SQLiteDB.Replace(DeviceManufacturerTable.TableName, data))
            {
                Console.WriteLine("Error: Couldn't update manufacturer in TECDeviceTECManufacturer table.");
            }
        }

        static private void editPoint(TECPoint point)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add(PointTable.PointID.Name, point.Guid.ToString());
            data.Add(PointTable.Name.Name, point.Name);
            data.Add(PointTable.Description.Name, point.Description);
            data.Add(PointTable.Type.Name, point.Type.ToString());
            data.Add(PointTable.Quantity.Name, point.Quantity.ToString());

            if (!SQLiteDB.Replace(PointTable.TableName, data))
            {
                Console.WriteLine("Error: Couldn't update point in TECPoint table.");
            }
        }

        static private void editNote(TECNote note)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add(NoteTable.NoteID.Name, note.Guid.ToString());
            data.Add(NoteTable.NoteText.Name, note.Text);

            if (!SQLiteDB.Replace(NoteTable.TableName, data))
            {
                Console.WriteLine("Error: Couldn't update item in TECNote table");
            }
        }

        static private void editExclusion(TECExclusion exclusion)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add(ExclusionTable.ExclusionID.Name, exclusion.Guid.ToString());
            data.Add(ExclusionTable.ExclusionText.Name, exclusion.Text);

            if (!SQLiteDB.Replace(ExclusionTable.TableName, data))
            {
                Console.WriteLine("Error: Couldn't update item in TECExclusion table.");
            }
        }

        static private void editScopeBranch(TECScopeBranch branch)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add(ScopeBranchTable.ScopeBranchID.Name, branch.Guid.ToString());
            data.Add(ScopeBranchTable.Name.Name, branch.Name);
            data.Add(ScopeBranchTable.Description.Name, branch.Description);

            if (!SQLiteDB.Replace(ScopeBranchTable.TableName, data))
            {
                Console.WriteLine("Error: Couldn't update item in TECScopeBranch table.");
            }
        }

        static private void editDrawing(TECDrawing drawing)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add(DrawingTable.DrawingID.Name, drawing.Guid.ToString());
            data.Add(DrawingTable.Name.Name, drawing.Name);
            data.Add(DrawingTable.Description.Name, drawing.Description);
            
            if (!SQLiteDB.Replace(DrawingTable.TableName, data))
            {
                Console.WriteLine("Error: Couldn't update item in TECDrawing table.");
            }
        }

        static private void editVisualScope(TECVisualScope vs)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add(VisualScopeTable.VisualScopeID.Name, vs.Guid.ToString());
            data.Add(VisualScopeTable.XPos.Name, vs.X.ToString());
            data.Add(VisualScopeTable.YPos.Name, vs.Y.ToString());

            if (!SQLiteDB.Replace(VisualScopeTable.TableName, data))
            {
                Console.WriteLine("Error: Couldn't update item in TECVisualScope table.");
            }
        }

        static private void editLocation(TECLocation location)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add(LocationTable.LocationID.Name, location.Guid.ToString());
            data.Add(LocationTable.Name.Name, location.Name);

            if (!SQLiteDB.Replace(LocationTable.TableName, data))
            {
                Console.WriteLine("Error: Couldn't update item in TECLocation table.");
            }
        }

        static private void editTag(TECTag tag)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add(TagTable.TagID.Name, tag.Guid.ToString());
            data.Add(TagTable.TagString.Name, tag.Text);

            if (!SQLiteDB.Replace(TagTable.TableName, data))
            {
                Console.WriteLine("Error: Couldn't update item in TECTag table");
            }
        }

        static private void editController(TECController controller)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add(ControllerTable.ControllerID.Name, controller.Guid.ToString());
            data.Add(ControllerTable.Description.Name, controller.Description);
            data.Add(ControllerTable.Cost.Name, controller.Cost.ToString());
            data.Add(ControllerTable.Description.Name, controller.Description);

            if (!SQLiteDB.Replace(TagTable.TableName, data))
            {
                Console.WriteLine("Error: Couldn't update item in TECController table");
            }
        }
        #endregion Edit Methods

        #region Remove Methods
        #region Remove Objects
        static private void removeSystem(TECSystem system)
        {
            SQLiteDB.Delete(SystemTable.TableName, "SystemID", system.Guid);
        }

        static private void removeEquipment(TECEquipment equipment)
        {
            SQLiteDB.Delete(EquipmentTable.TableName, EquipmentTable.EquipmentID.Name, equipment.Guid);
        }

        static private void removeSubScope(TECSubScope subScope)
        {
            SQLiteDB.Delete(SubScopeTable.TableName, SubScopeTable.SubScopeID.Name, subScope.Guid);
        }

        static private void removeDevice(TECDevice device)
        {
            SQLiteDB.Delete(DeviceTable.TableName, DeviceTable.DeviceID.Name, device.Guid);
        }

        static private void removePoint(TECPoint point)
        {
            SQLiteDB.Delete(PointTable.TableName, PointTable.PointID.Name, point.Guid);
        }

        static private void removeScopeBranch(TECScopeBranch branch)
        {
            SQLiteDB.Delete(ScopeBranchTable.TableName, ScopeBranchTable.ScopeBranchID.Name, branch.Guid);
            foreach(TECScopeBranch childBranch in branch.Branches)
            {
                removeScopeBranch(childBranch);
                removeScopeBranchHierarchyRelation(childBranch);
            }
        }

        static private void removeNote(TECNote note)
        {
            SQLiteDB.Delete(NoteTable.TableName, NoteTable.NoteID.Name, note.Guid);
        }

        static private void removeExclusion(TECExclusion exclusion)
        {
            SQLiteDB.Delete("TECExclusion", "ExclusionID", exclusion.Guid);
        }

        static private void removeLocation(TECLocation location)
        {
            SQLiteDB.Delete("TECLocation", "LocationID", location.Guid);
        }

        static private void removeVisualScope(TECVisualScope vs)
        {
            SQLiteDB.Delete(VisualScopeTable.TableName, VisualScopeTable.VisualScopeID.Name, vs.Guid);
        }

        static private void removeController(TECController controller)
        {
            SQLiteDB.Delete(ControllerTable.TableName, ControllerTable.ControllerID.Name, controller.Guid);
        }

        static private void removeProposalScope(TECProposalScope scope)
        {
            SQLiteDB.Delete(ProposalScopeTable.TableName, ProposalScopeTable.ProposalScopeID.Name, scope.Scope.Guid);
            foreach(TECScopeBranch branch in scope.Notes)
            {
                removeScopeBranch(branch);
            }
            foreach(TECProposalScope children in scope.Children)
            {
                removeProposalScope(children);
            }
        }
        #endregion Remove Objects

        #region Remove Relations
        private static void removeSystemEquipmentRelation(TECEquipment equipment)
        {
            SQLiteDB.Delete("TECSystemTECEquipment", EquipmentTable.EquipmentID.Name, equipment.Guid);
        }

        private static void removeEquipmentSubScopeRelation(TECSubScope subScope)
        {
            SQLiteDB.Delete("TECEquipmentTECSubScope", SubScopeTable.SubScopeID.Name, subScope.Guid);
        }

        private static void removeSubScopeDeviceRelation(TECDevice device)
        {
            SQLiteDB.Delete("TECSubScopeTECDevice", DeviceTable.DeviceID.Name, device.Guid);
        }

        private static void removeSubScopePointRelation(TECPoint point)
        {
            SQLiteDB.Delete("TECSubScopeTECPoint", PointTable.PointID.Name, point.Guid);
        }

        private static void removeDeviceManufacturerRelation(TECDevice device)
        {
            SQLiteDB.Delete(DeviceManufacturerTable.TableName, DeviceManufacturerTable.DeviceID.Name, device.Guid);
        }

        private static void removeScopeBranchHierarchyRelation(TECScopeBranch branch)
        {
            SQLiteDB.Delete("TECScopeBranchHierarchy", "BranchID", branch.Guid);

        }

        private static void removeScopeBranchBidRelation(TECScopeBranch branch, Guid bidID)
        {

        }

        private static void removeScopeBranchProposalScopeRelation(TECScopeBranch branch, Guid scopeID)
        {

        }

        private static void removeLocationInScope(TECScope scope)
        {
            SQLiteDB.Delete("TECLocationTECScope", "ScopeID", scope.Guid);
        }

        private static void removePageVisualScopeRelation(TECVisualScope vs)
        {
            SQLiteDB.Delete(PageVisualScopeTable.TableName, PageVisualScopeTable.VisualScopeID.Name, vs.Guid);
        }

        private static void removeControllerConnectionTypeRelation(TECController controller)
        {
            SQLiteDB.Delete(ControllerIOTypeTable.TableName, ControllerIOTypeTable.ControllerID.Name, controller.Guid);
        }
        #endregion
        #endregion Remove Methods

        #region Update Relations/Order

        static private void updateSystemIndexes(ObservableCollection<TECSystem> systems)
        {
            int i = 0;
            foreach (TECSystem system in systems)
            {
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add(SystemIndexTable.SystemID.Name, system.Guid.ToString());
                data.Add(SystemIndexTable.Index.Name, i.ToString());

                if (!SQLiteDB.Replace(SystemIndexTable.TableName, data))
                {
                    Console.WriteLine("Error: Couldn't add system to TECSystemIndex table");
                }
                i++;
            }
        }
        static private void updateSystemEquipmentRelation(TECSystem system)
        {
            int i = 0;
            foreach (TECEquipment equipment in system.Equipment)
            {
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("SystemID", system.Guid.ToString());
                data.Add(EquipmentTable.EquipmentID.Name, equipment.Guid.ToString());
                data.Add("ScopeIndex", i.ToString());

                if (!SQLiteDB.Replace("TECSystemTECEquipment", data))
                {
                    Console.WriteLine("Error: Couldn't add relation to TECSystemTECEquipment table.");
                }
                i++;
            }
        }
        static private void updateEquipmentSubScopeRelation(TECEquipment equipment)
        {
            int i = 0;
            foreach (TECSubScope subScope in equipment.SubScope)
            {
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add(EquipmentTable.EquipmentID.Name, equipment.Guid.ToString());
                data.Add(SubScopeTable.SubScopeID.Name, subScope.Guid.ToString());
                data.Add("ScopeIndex", i.ToString());

                if (!SQLiteDB.Replace("TECEquipmentTECSubScope", data))
                {
                    Console.WriteLine("Error: Couldn't add relation to TECEquipmentTECSubScope table.");
                }
                i++;
            }
        }
        static private void updateSubScopeDeviceRelation(TECSubScope subScope)
        {
            int i = 0;
            foreach (TECDevice device in subScope.Devices)
            {
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add(SubScopeTable.SubScopeID.Name, subScope.Guid.ToString());
                data.Add(DeviceTable.DeviceID.Name, device.Guid.ToString());
                data.Add("Quantity", device.Quantity.ToString());
                data.Add("ScopeIndex", i.ToString());

                if (!SQLiteDB.Replace("TECSubScopeTECDevice", data))
                {
                    Console.WriteLine("Error: Couldn't add relation to TECSubScopeTECDevice table.");
                }
                i++;
            }
        }
        static private void updateSubScopePointRelation(TECSubScope subScope)
        {
            int i = 0;
            foreach (TECPoint point in subScope.Points)
            {
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add(SubScopeTable.SubScopeID.Name, subScope.Guid.ToString());
                data.Add(PointTable.PointID.Name, point.Guid.ToString());
                data.Add("ScopeIndex", i.ToString());

                if (!SQLiteDB.Replace("TECSubScopeTECPoint", data))
                {
                    Console.WriteLine("Error: Couldn't add relation to TECSubScopeTECPoint table.");
                }
                i++;
            }
        }

        #endregion Update Relations/Order

        #endregion //Saving to DB Methods

        #region Loading from DB Methods

        static private TECBid getBidInfo()
        {
            DataTable bidInfoDT = SQLiteDB.getDataFromTable("TECBidInfo");
            
            if (bidInfoDT.Rows.Count < 1)
            {
                MessageBox.Show("Bid info not found in database. Bid info and labor will be missing.");
                return new TECBid();
            }
            DataRow bidInfoRow = bidInfoDT.Rows[0];

            Guid infoGuid = new Guid(bidInfoRow["BidInfoID"].ToString());
            string name = bidInfoRow["BidName"].ToString();
            string bidNumber = bidInfoRow["BidNumber"].ToString();

            string dueDateString = bidInfoRow["DueDate"].ToString();
            DateTime dueDate = DateTime.ParseExact(dueDateString, DB_FMT, CultureInfo.InvariantCulture);

            string salesperson = bidInfoRow["Salesperson"].ToString();
            string estimator = bidInfoRow["Estimator"].ToString();

            TECBid bid = new TECBid(name, bidNumber, dueDate, salesperson, estimator, new ObservableCollection<TECScopeBranch>(), new ObservableCollection<TECSystem>(), new ObservableCollection<TECDevice>(), new ObservableCollection<TECManufacturer>(), new ObservableCollection<TECNote>(), new ObservableCollection<TECExclusion>(), new ObservableCollection<TECTag>(), infoGuid);

            try
            {
                bid.Labor.PMCoef = bidInfoRow["PMCoef"].ToString().ToDouble();
                bid.Labor.ENGCoef = bidInfoRow["ENGCoef"].ToString().ToDouble();
                bid.Labor.CommCoef = bidInfoRow["CommCoef"].ToString().ToDouble();
                bid.Labor.SoftCoef = bidInfoRow["SoftCoef"].ToString().ToDouble();
                bid.Labor.GraphCoef = bidInfoRow["GraphCoef"].ToString().ToDouble();
                bid.Labor.ElectricalRate = bidInfoRow["ElectricalRate"].ToString().ToDouble();
            }
            catch
            {
                Console.WriteLine("Reading labor values from database failed. Using default values.");
                bid.Labor = new TECLabor();
            }

            return bid;
        }

        static private TECTemplates getTemplatesInfo()
        {
            DataTable bidInfoDT = SQLiteDB.getDataFromTable("TECTemplatesInfo");

            if (bidInfoDT.Rows.Count < 1)
            {
                MessageBox.Show("Bid info not found in database. Bid info and labor will be missing.");
                return new TECTemplates();
            }
            DataRow bidInfoRow = bidInfoDT.Rows[0];

            Guid infoGuid = new Guid(bidInfoRow["TemplatesInfoID"].ToString());

            return new TECTemplates(infoGuid);
        }

        static private ObservableCollection<TECScopeBranch> getBidScopeBranches()
        {
            ObservableCollection<TECScopeBranch> mainBranches = new ObservableCollection<TECScopeBranch>();

            string command =    "select * from TECScopeBranch where ScopeBranchID not in ";
            command +=          "(select ChildID from TECScopeBranchHierarchy)";

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

            string command =    "select * from TECScopeBranch where ScopeBranchID in ";
            command +=          "(select ChildID from TECScopeBranchHierarchy where ParentID = '";
            command +=          parentID;
            command +=          "')";

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

        static private ObservableCollection<TECSystem> getAllSystemsInBid()
        {
            ObservableCollection<TECSystem> systems = new ObservableCollection<TECSystem>();

            string command = "select * from (TECSystem inner join TECSystemIndex on (TECSystem.SystemID = TECSystemIndex.SystemID)) order by ScopeIndex";

            DataTable systemsDT = SQLiteDB.getDataFromCommand(command);

            if (systemsDT.Rows.Count < 1)
            {
                command = "select * from TECSystem";
                systemsDT = SQLiteDB.getDataFromCommand(command);
            }

            foreach (DataRow row in systemsDT.Rows)
            {
                Guid systemID = new Guid(row["SystemID"].ToString());
                string name = row["Name"].ToString();
                string description = row["Description"].ToString();
                string quantityString = row["Quantity"].ToString();
                string budgetPriceString = row["BudgetPrice"].ToString();

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

            string command =    "select * from TECEquipment where EquipmentID not in ";
            command +=          "(select EquipmentID from TECSystemTECEquipment)";

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
                ConnectionType connectionType = TECConnectionType.convertStringToType(row[DeviceTable.ConnectionType.Name].ToString());

                double cost;
                if (!double.TryParse(costString, out cost))
                {
                    cost = 0;
                    Console.WriteLine("Cannot convert cost to double, setting to 0");
                }

                TECManufacturer manufacturer = getManufacturerInDevice(deviceID);

                TECDevice deviceToAdd = new TECDevice(name, description, cost, connectionType, manufacturer, deviceID);
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

            string command =    "select * from (TECEquipment inner join TECSystemTECEquipment on (TECEquipment.EquipmentID = TECSystemTECEquipment.EquipmentID and SystemID = '";
            command +=          systemID;
            command +=          "')) order by ScopeIndex";

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
                ConnectionType connectionType = TECConnectionType.convertStringToType(row[DeviceTable.ConnectionType.Name].ToString());

                double cost;
                if (!double.TryParse(costString, out cost))
                {
                    cost = 0;
                    Console.WriteLine("Cannot convert cost to double, setting to 0");
                }

                TECManufacturer manufacturer = getManufacturerInDevice(deviceID);

                TECDevice deviceToAdd = new TECDevice(name, description, cost, connectionType, manufacturer, deviceID);

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

                deviceToAdd.Quantity = quantity;
                deviceToAdd.Tags = getTagsInScope(deviceID);

                devices.Add(deviceToAdd);
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
                    //string typeString = row[ConnectionTable.Type.Name].ToString();

                    double length = lengthString.ToDouble();
                    //ConnectionType type = TECConnectionType.convertStringToType(typeString);

                    ObservableCollection<ConnectionType> connectionTypes = getConnectionTypesInConnection(guid);

                    connections.Add(new TECConnection(length, connectionTypes, guid));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: getConnections() failed. Code: " + e.Message);
                throw e;
            }

            return connections;
        }

        static private ObservableCollection<ConnectionType> getConnectionTypesInConnection(Guid connectionID)
        {
            ObservableCollection<ConnectionType> types = new ObservableCollection<ConnectionType>();

            string command = "select " + ConnectionConnectionTypeTable.Type.Name + " from " + ConnectionConnectionTypeTable.TableName + " where " +
                ConnectionConnectionTypeTable.ConnectionID.Name + " = '" + connectionID + "'";

            try
            {
                DataTable typeDT = SQLiteDB.getDataFromCommand(command);

                foreach (DataRow row in typeDT.Rows)
                {
                    ConnectionType type = TECConnectionType.convertStringToType(row[ConnectionConnectionTypeTable.Type.Name].ToString());
                    types.Add(type);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("getConnectionTypesInController() failed. Code: " + e.Message);
                throw e;
            }

            return types;
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
            var tableInfo = getTableInfo(table);
            string tableName = tableInfo.Item1;
            List<TableField> primaryKey = tableInfo.Item3;
            List<TableField> fields = tableInfo.Item2;

            string createString = "CREATE TABLE '" + tableName + "' (";
            foreach (TableField field in fields)
            {
                createString += "'" + field.Name + "' " + field.FieldType;
                if (fields.IndexOf(field) < (fields.Count - 1))
                { createString += ", "; }
            }
            if (primaryKey.Count != 0)
            {  createString += ", PRIMARY KEY("; }
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

            if(table is TemplatesInfoTable)
            {
                populateTemplatesInfo();
            }
        }
        static private void createTempTableFromDefinition(TableBase table)
        {
            var tableInfo = getTableInfo(table);
            string tableName = "temp_" + tableInfo.Item1;
            List<TableField> primaryKey = tableInfo.Item3;
            List<TableField> fields = tableInfo.Item2;

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
            foreach(TableBase table in AllBidTables.Tables)
            {
                createTableFromDefinition(table);
            }
        }
        static private void createAllTemplateTables()
        {
            foreach(TableBase table in AllTemplateTables.Tables)
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

            foreach(TECController controller in bidControllers)
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
                foreach(KeyValuePair<TECVisualScope, Guid> vs in scopeToLink)
                {
                    if (vs.Value == system.Guid)
                    {
                        vs.Key.Scope = system;
                        scopeToRemove.Add(vs.Key);
                    }
                }
                foreach(TECVisualScope scope in scopeToRemove)
                {
                    scopeToLink.Remove(scope);
                }
                if (scopeToLink.Count < 1)
                {
                    return;
                }
                scopeToRemove.Clear();


                foreach(TECEquipment equip in system.Equipment)
                {
                    //Check scope in equipment.
                    foreach(KeyValuePair<TECVisualScope, Guid> vs in scopeToLink)
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
                        foreach(KeyValuePair<TECVisualScope, Guid> vs in scopeToLink)
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
                            foreach(KeyValuePair<TECVisualScope, Guid> vs in scopeToLink)
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

            if (scopeToLink.Count > 0)
            {
                Exception e = new Exception("Error: " + scopeToLink.Count + " Visual Scope not found. Unable to link.");
                throw e;
            }
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
            if (scopeToLink.Count > 0)
            {
                Exception e = new Exception("Error: " + scopeToLink.Count + " scope not found in linkAllLocations(). Unable to link.");
                throw e;
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

        #endregion Link Methods

        #region Populate Derived

        static private void populatePageVisualConnections(ObservableCollection<TECDrawing> drawings, ObservableCollection<TECConnection> connections)
        {
            ObservableCollection<TECConnection> connectionsToAdd = connections;
            foreach (TECDrawing drawing in drawings)
            {
                foreach(TECPage page in drawing.Pages)
                {
                    List<Tuple<TECSubScope, TECVisualScope>> vSubScope = GetSubScopeVisual(page.PageScope);
                    List<TECVisualScope> vControllers = new List<TECVisualScope>();

                    ObservableCollection<TECVisualConnection> vConnectionsToAdd = new ObservableCollection<TECVisualConnection>();

                    foreach(TECVisualScope vScope in page.PageScope)
                    {
                        if (vScope.Scope is TECController)
                        { vControllers.Add(vScope); }
                    }

                    foreach(Tuple<TECSubScope, TECVisualScope> item in vSubScope)
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
        static private void populateTemplatesInfo()
        {
            Dictionary<string, string> Data = new Dictionary<string, string>();
            Data.Add(TemplatesInfoTable.DBVersion.Name, Properties.Settings.Default.Version);
            Data.Add(TemplatesInfoTable.TemplatesInfoID.Name, Guid.NewGuid().ToString());
            SQLiteDB.Insert(TemplatesInfoTable.TableName, Data);
        }

        static private void checkAndUpdateDB(Type type)
        {
            bool isUpToDate;
            try
            {
                isUpToDate = checkDatabaseVersion(type);
                if (!isUpToDate)
                {
                    updateDatabase(type);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Could not check database version." + e);
            }
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
                { throw new ArgumentException("checkDatabaseVersion given invalid type");  }
                
                if (infoDT.Rows.Count < 1)
                {
                    if (type == typeof(TECBid))
                    {
                        MessageBox.Show("Bid info not found in database. Could not check verison.");
                        throw new Exception("Could not load from TECBidInfo");
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
                    } else
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
            { databaseTableList = AllBidTables.Tables;  }
            else if (type == typeof(TECTemplates))
            { databaseTableList = AllTemplateTables.Tables; }
            else
            { throw new ArgumentException("updateDatabase() given invalid type"); }
            foreach(TableBase table in databaseTableList)
            {
                var tableInfo = getTableInfo(table);
                if (tableNames.Contains(tableInfo.Item1))
                { updateTableFromType(table);
                } else
                { createTableFromDefinition(table); }
            }
        }
        static private List<string> getAllTableNames()
        {
            string command = "select name from sqlite_master where type = 'table' order by 1";
            DataTable tables = SQLiteDB.getDataFromCommand(command);
            List<string> tableNames = new List<string>();
            foreach(DataRow row in tables.Rows)
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
        
        static private void updateTableFromType(TableBase table)
        {
            var tableInfo = getTableInfo(table);
            string tableName = tableInfo.Item1;
            string tempName = "temp_" + tableName;
            List<TableField> primaryKey = tableInfo.Item3;
            List<TableField> fields = tableInfo.Item2;

            List<string> currentFields = getAllTableFields(tableName);
            List<string> commonFields = new List<string>();
            foreach(TableField field in fields)
            {
                if (currentFields.Contains(field.Name))
                {  commonFields.Add(field.Name); }
            }

            string commonString = UtilitiesMethods.CommaSeparatedString(commonFields);

            createTempTableFromDefinition(table);

            string commandString = "insert or ignore into '" + tempName + "' (" + commonString + ") select " + commonString + " from '" + tableName + "'";
            SQLiteDB.nonQueryCommand(commandString);
            commandString = "drop table '" + tableName + "'";
            SQLiteDB.nonQueryCommand(commandString);

            createTableFromDefinition(table);

            commandString = "insert into '" + tableName + "' select * from '" + tempName + "'";
            SQLiteDB.nonQueryCommand(commandString);
            commandString = "drop table '" + tempName + "'";
            SQLiteDB.nonQueryCommand(commandString);

            if ((table is BidInfoTable) || (table is TemplatesInfoTable))
            {
                Dictionary<string, string> Data = new Dictionary<string, string>();
                if(table is BidInfoTable)
                {
                    var infoBid = getBidInfo();
                    commandString = "update " + BidInfoTable.TableName + " set " + BidInfoTable.DBVersion.Name + " = '" + Properties.Settings.Default.Version + "' ";
                    commandString += "where " + BidInfoTable.BidInfoID.Name + " = '" + infoBid.InfoGuid.ToString() + "'";
                    SQLiteDB.nonQueryCommand(commandString);
                } else
                {
                    var infoTemplates = getTemplatesInfo();
                    commandString = "update " + TemplatesInfoTable.TableName + " set " + TemplatesInfoTable.DBVersion.Name + " = '" + Properties.Settings.Default.Version + "' ";
                    commandString += "where " + TemplatesInfoTable.TemplatesInfoID.Name + " = '" + infoTemplates.InfoGuid.ToString() + "'";
                    SQLiteDB.nonQueryCommand(commandString);
                }
            }
        }
        
        static private Tuple<string, List<TableField>, List<TableField>> getTableInfo(TableBase table)
        {
            string tableName = "";
            List<TableField> primaryKey = new List<TableField>();
            List<TableField> fields = new List<TableField>();
            var type = table.GetType();

            foreach (var p in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
            {
                if (p.Name == "TableName")
                {
                    var v = p.GetValue(null);
                    tableName += (string)v;
                }
                else if (p.Name == "PrimaryKey")
                {
                    var v = p.GetValue(null) as List<TableField>;
                    foreach (TableField field in v)
                    { primaryKey.Add(field); }
                }
                else
                {
                    var v = p.GetValue(null) as TableField;
                    fields.Add(v);
                }
            }

            return Tuple.Create<string, List<TableField>, List<TableField>>(tableName, fields, primaryKey);
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
            } catch (Exception e)
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

            return subScopeToAdd;
        }

        #endregion
    }

}
