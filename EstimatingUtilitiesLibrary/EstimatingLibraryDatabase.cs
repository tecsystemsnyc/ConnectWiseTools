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

namespace EstimatingUtilitiesLibrary
{
    public static class EstimatingLibraryDatabase
    {
        static private SQLiteDatabase SQLiteDB;

        //FMT is used by DateTime to convert back and forth between the DateTime type and string
        private const string FMT = "O";

        #region Public Functions
        static public TECBid LoadDBToBid(string path, TECTemplates templates)
        {
            SQLiteDB = new SQLiteDatabase(path);

            TECBid bid = new TECBid();

            try
            {
                if (templates.DeviceCatalog != null)
                {
                    foreach (TECDevice device in templates.DeviceCatalog)
                    {
                        editDevice(device);
                    }
                }

                if (templates.ManufacturerCatalog != null)
                {
                    foreach (TECManufacturer manufacturer in templates.ManufacturerCatalog)
                    {
                        editManufacturer(manufacturer);
                    }
                }

                bid = getBidInfo();
                if (templates.Tags != null)
                {
                    bid.Tags = templates.Tags;
                }
                else
                {
                    bid.Tags = getAllTags();
                }
                bid.ScopeTree = getMainScopeBranches();
                bid.Systems = getAllSystems();
                bid.DeviceCatalog = getAllDevices();
                bid.ManufacturerCatalog = getAllManufacturers();
                bid.Locations = getAllLocations();
                bid.Notes = getNotes();
                bid.Exclusions = getExclusions();
                bid.Drawings = getDrawings();
                linkAllVisualScope(bid.Drawings, bid.Systems);
                linkAllLocations(bid.Locations, bid.Systems);
            }
            catch (Exception e)
            {
                MessageBox.Show("Could not load bid from database. Error: " + e.Message);
            }

            SQLiteDB.Connection.Close();

            return bid;
        }

        static public TECTemplates LoadDBToTemplates(string path)
        {
            SQLiteDB = new SQLiteDatabase(path);

            TECTemplates templates = new TECTemplates();

            try
            {
                templates.SystemTemplates = getAllSystems();
                templates.EquipmentTemplates = getOrphanEquipment();
                templates.SubScopeTemplates = getOrphanSubScope();
                templates.DeviceCatalog = getAllDevices();
                templates.Tags = getAllTags();
                templates.ManufacturerCatalog = getAllManufacturers();
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

            createBidObjectTables();
            createBidRelationTables();

            try
            {
                foreach (TECDevice device in bid.DeviceCatalog)
                {
                    addDevice(device);
                }

                foreach (TECManufacturer manufacturer in bid.ManufacturerCatalog)
                {
                    addManufacturer(manufacturer);
                }

                addTags(bid.Tags);

                addBidInfo(bid);

                foreach (TECSystem system in bid.Systems)
                {
                    addFullSystem(system);
                }

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
            }
            catch (Exception e)
            {
                string message = "Could not save bid to new database. Error: " + e.Message;
                MessageBox.Show(message);
            }

            SQLiteDB.Connection.Close();
        }

        static public void SaveTemplatesToNewDB(string path, TECTemplates templates)
        {
            SQLiteDB = new SQLiteDatabase(path);

            if (File.Exists(path))
            {
                SQLiteDB.overwriteFile();
            }

            createTemplateObjectTables();
            createTemplateRelationTables();

            try
            {
                addTags(templates.Tags);
                foreach (TECSystem system in templates.SystemTemplates)
                {
                    addFullSystem(system);
                }
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
                }
                foreach (TECManufacturer manufacturer in templates.ManufacturerCatalog)
                {
                    addManufacturer(manufacturer);
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
            string tempPath = Path.GetDirectoryName(path) + Path.GetFileNameWithoutExtension(path) + ".tmp";

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
                addSystem(tarObject as TECSystem);
            }
            else if (tarObject is TECEquipment)
            {
                addEquipment(tarObject as TECEquipment);
                addSystemEquipmentRelation(refObject as TECSystem, tarObject as TECEquipment);
            }
            else if (tarObject is TECSubScope)
            {
                addSubScope(tarObject as TECSubScope);
                addEquipmentSubScopeRelation(refObject as TECEquipment, tarObject as TECSubScope);
            }
            else if (tarObject is TECDevice)
            {
                if (refObject is TECSubScope)
                {
                    addSubScopeDeviceRelation(refObject as TECSubScope, tarObject as TECDevice);
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
                addSubScopePointRelation(refObject as TECSubScope, tarObject as TECPoint);
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
                addLocation(tarObject as TECLocation);
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
                editBidInfo(refObject as TECBid);
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
                editDevice(tarObject as TECDevice);
            }
            else if (tarObject is TECPoint)
            {
                editPoint(tarObject as TECPoint);
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
            }
            else if (tarObject is TECEquipment)
            {
                removeEquipment(tarObject as TECEquipment);
                if (refObject is TECSystem)
                {
                    removeSystemEquipmentRelation(tarObject as TECEquipment);
                }
            }
            else if (tarObject is TECSubScope)
            {
                removeSubScope(tarObject as TECSubScope);
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
                else if (refObject is TECBid)
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
                throw new NotImplementedException();
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
            }
            else if (tarObject is TECLocation)
            {
                removeLocation(tarObject as TECLocation);
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
            data.Add("BidInfoID", bid.InfoGuid.ToString());
            data.Add("BidName", bid.Name);
            data.Add("BidNumber", bid.BidNumber);
            data.Add("DueDate", bid.DueDate.ToString(FMT));
            data.Add("Salesperson", bid.Salesperson);
            data.Add("Estimator", bid.Estimator);

            data.Add("PMCoef", bid.Labor.PMCoef.ToString());
            data.Add("ENGCoef", bid.Labor.ENGCoef.ToString());
            data.Add("CommCoef", bid.Labor.CommCoef.ToString());
            data.Add("SoftCoef", bid.Labor.SoftCoef.ToString());
            data.Add("GraphCoef", bid.Labor.GraphCoef.ToString());
            data.Add("ElectricalRate", bid.Labor.ElectricalRate.ToString());

            if (!SQLiteDB.Insert("TECBidInfo", data))
            {
                Console.WriteLine("Error: Couldn't add item to TECBidInfo table");
            }
        }

        static private void addFullSystem(TECSystem system)
        {
            addSystem(system);
            addTagsInScope(system.Tags, system.Guid);
            addLocationInScope(system.Location, system.Guid);
            foreach (TECEquipment equip in system.Equipment)
            {
                addFullEquipment(equip);
                addSystemEquipmentRelation(system, equip);
            }
        }

        static private void addFullEquipment(TECEquipment equipment)
        {
            addEquipment(equipment);
            addTagsInScope(equipment.Tags, equipment.Guid);
            addLocationInScope(equipment.Location, equipment.Guid);
            foreach (TECSubScope ss in equipment.SubScope)
            {
                addEquipmentSubScopeRelation(equipment, ss);
                addFullSubScope(ss);
            }
        }

        static private void addFullSubScope(TECSubScope subScope)
        {
            addSubScope(subScope);
            addTagsInScope(subScope.Tags, subScope.Guid);
            addLocationInScope(subScope.Location, subScope.Guid);
            foreach (TECDevice dev in subScope.Devices)
            {
                addSubScopeDeviceRelation(subScope, dev);
            }

            foreach (TECPoint point in subScope.Points)
            {
                addSubScopePointRelation(subScope, point);
                addPoint(point);
                addTagsInScope(point.Tags, point.Guid);
            }
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
            data.Add("SystemID", system.Guid.ToString());
            data.Add("Name", system.Name);
            data.Add("Description", system.Description);
            data.Add("Quantity", system.Quantity.ToString());
            data.Add("BudgetPrice", system.BudgetPrice.ToString());

            if (!SQLiteDB.Insert("TECSystem", data))
            {
                Console.WriteLine("Error: Couldn't add system to TECSystem table");
            }
        }
        static private void addEquipment(TECEquipment equipment)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("EquipmentID", equipment.Guid.ToString());
            data.Add("Name", equipment.Name);
            data.Add("Description", equipment.Description);
            data.Add("Quantity", equipment.Quantity.ToString());
            data.Add("BudgetPrice", equipment.BudgetPrice.ToString());

            if (!SQLiteDB.Insert("TECEquipment", data))
            {
                Console.WriteLine("Error: Couldn't add equipment to TECEquipment table.");
            }
        }
        static private void addSubScope(TECSubScope subScope)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("SubScopeID", subScope.Guid.ToString());
            data.Add("Name", subScope.Name);
            data.Add("Description", subScope.Description);
            data.Add("Quantity", subScope.Quantity.ToString());

            if (!SQLiteDB.Insert("TECSubScope", data))
            {
                Console.WriteLine("Error: Couldn't add subScope to TECSubScope table.");
            }
        }
        static private void addDevice(TECDevice device)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("DeviceID", device.Guid.ToString());
            data.Add("Name", device.Name);
            data.Add("Description", device.Description);
            data.Add("Quantity", device.Quantity.ToString());
            data.Add("Cost", device.Cost.ToString());
            data.Add("Wire", device.Wire);

            if (!SQLiteDB.Insert("TECDevice", data))
            {
                Console.WriteLine("Error: Couldn't add device to TECDevice table.");
            }
        }
        static private void addManufacturer(TECManufacturer manufacturer)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("ManufacturerID", manufacturer.Guid.ToString());
            data.Add("Name", manufacturer.Name);
            data.Add("Multiplier", manufacturer.Multiplier.ToString());

            if (!SQLiteDB.Insert("TECManufacturer", data))
            {
                Console.WriteLine("Error: Couldn't add manufacturer to TECManufacturer table.");
            }
        }
        static private void addPoint(TECPoint point)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("PointID", point.Guid.ToString());
            data.Add("Name", point.Name);
            data.Add("Description", point.Description);
            data.Add("Type", point.Type.ToString());
            data.Add("Quantity", point.Quantity.ToString());

            if (!SQLiteDB.Insert("TECPoint", data))
            {
                Console.WriteLine("Error: Couldn't add point to TECPoint table.");
            }
        }
        static private void addScopeBranch(TECScopeBranch branch)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("ScopeBranchID", branch.Guid.ToString());
            data.Add("Name", branch.Name);
            data.Add("Description", branch.Description);

            if (!SQLiteDB.Insert("TECScopeBranch", data))
            {
                Console.WriteLine("Error: Couldn't add item to TECScopeBranch table");
            }
        }
        static private void addTags(ObservableCollection<TECTag> tags)
        {
            foreach (TECTag tag in tags)
            {
                //Console.WriteLine("Adding a tag");

                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("TagID", Guid.NewGuid().ToString());
                data.Add("TagString", tag.Text);

                if (!SQLiteDB.Insert("TECTag", data))
                {
                    Console.WriteLine("Error: Couldn't add tag to TECTag table.");
                }
            }
        }
        static private void addNote(TECNote note)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("NoteID", note.Guid.ToString());
            data.Add("NoteText", note.Text);

            if (!SQLiteDB.Insert("TECNote", data))
            {
                Console.WriteLine("Error: Couldn't add item to TECNote table");
            }
        }
        static private void addExclusion(TECExclusion exclusion)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("ExclusionID", exclusion.Guid.ToString());
            data.Add("ExclusionText", exclusion.Text);

            if (!SQLiteDB.Insert("TECExclusion", data))
            {
                Console.WriteLine("Error: Couldn't add item to TECExclusion table.");
            }
        }
        static private void addDrawing(TECDrawing drawing)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("DrawingID", drawing.Guid.ToString());
            data.Add("Name", drawing.Name);
            data.Add("Description", drawing.Description);
            if (!SQLiteDB.Insert("TECDrawing", data))
            {
                Console.WriteLine("Error: Couldn't add item to TECDrawing table.");
            }
        }
        static private void addPage(TECPage page)
        {
            byte[] imageBytes = File.ReadAllBytes(page.Path);

            Dictionary<string, string> stringData = new Dictionary<string, string>();
            stringData.Add("PageID", page.Guid.ToString());
            stringData.Add("PageNum", page.PageNum.ToString());

            Dictionary<string, byte[]> byteData = new Dictionary<string, byte[]>();
            byteData.Add("Image", imageBytes);

            if (!SQLiteDB.Insert("TECPage", stringData, byteData))
            {
                Console.WriteLine("Error: Couldn't add item to TECPageTable.");
            }
        }
        static private void addVisualScope(TECVisualScope visualScope)
        {
            Dictionary<string, string> vsData = new Dictionary<string, string>();
            vsData.Add("VisualScopeID", visualScope.Guid.ToString());
            vsData.Add("XPos", visualScope.X.ToString());
            vsData.Add("YPos", visualScope.Y.ToString());

            if (!SQLiteDB.Insert("TECVisualScope", vsData))
            {
                Console.WriteLine("Error: Couldn't add item to TECVisualScopeTable.");
            }
        }
        static private void addLocation(TECLocation location)
        {
            Dictionary<string, string> locData = new Dictionary<string, string>();
            locData.Add("LocationID", location.Guid.ToString());
            locData.Add("Name", location.Name.ToString());

            if (!SQLiteDB.Insert("TECLocation", locData))
            {
                Console.WriteLine("Error: Couldn't add item to TECLocationTable.");
            }
        }

        #endregion Add Object Functions

        #region Add Relation Functions

        static private void addSystemEquipmentRelation(TECSystem system, TECEquipment equipment)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("SystemID", system.Guid.ToString());
            data.Add("EquipmentID", equipment.Guid.ToString());

            if (!SQLiteDB.Insert("TECSystemTECEquipment", data))
            {
                Console.WriteLine("Error: Couldn't add relation to TECSystemTECEquipment table.");
            }
        }
        static private void addEquipmentSubScopeRelation(TECEquipment equipment, TECSubScope subScope)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("EquipmentID", equipment.Guid.ToString());
            data.Add("SubScopeID", subScope.Guid.ToString());

            if (!SQLiteDB.Insert("TECEquipmentTECSubScope", data))
            {
                Console.WriteLine("Error: Couldn't add relation to TECEquipmentTECSubScope table.");
            }
        }
        static private void addSubScopeDeviceRelation(TECSubScope subScope, TECDevice device)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("SubScopeID", subScope.Guid.ToString());
            data.Add("DeviceID", device.Guid.ToString());

            if (!SQLiteDB.Insert("TECSubScopeTECDevice", data))
            {
                Console.WriteLine("Error: Couldn't add relation to TECSubScopeTECDevice table.");
            }
        }
        static private void addSubScopePointRelation(TECSubScope subScope, TECPoint point)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("SubScopeID", subScope.Guid.ToString());
            data.Add("PointID", point.Guid.ToString());

            if (!SQLiteDB.Insert("TECSubScopeTECPoint", data))
            {
                Console.WriteLine("Error: Couldn't add relation to TECSubScopeTECPoint table.");
            }
        }
        static private void addDeviceManufacturerRelation(TECDevice device, TECManufacturer manufacturer)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("DeviceID", device.Guid.ToString());
            data.Add("ManufacturerID", manufacturer.Guid.ToString());

            if (!SQLiteDB.Insert("TECDeviceTECManufacturer", data))
            {
                Console.WriteLine("Error: Couldn't add relation to TECDeviceTECManufacturer table.");
            }
        }
        static private void addScopeTreeRelation(TECScopeBranch parentBranch, TECScopeBranch childBranch)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("ParentID", parentBranch.Guid.ToString());
            data.Add("ChildID", childBranch.Guid.ToString());

            if (!SQLiteDB.Insert("TECScopeBranchHierarchy", data))
            {
                Console.WriteLine("Error: Couldn't add relation to TECScopeBranchHierarhy table.");
            }
        }
        static private void addTagsInScope(ObservableCollection<TECTag> tags, Guid scopeID)
        {
            foreach (TECTag tag in tags)
            {
                string command = "select TagID from TECTag where TagString = '";
                command += tag;
                command += "'";

                DataTable tagIDDT = SQLiteDB.getDataFromCommand(command);

                try
                {
                    string tagID = tagIDDT.Rows[0]["TagID"].ToString();

                    Dictionary<string, string> data = new Dictionary<string, string>();
                    data.Add("ScopeID", scopeID.ToString());
                    data.Add("TagID", tagID);

                    if (!SQLiteDB.Insert("TECScopeTECTag", data))
                    {
                        Console.WriteLine("Error: Couldn't add relation to TECScopeTECTag table.");
                    }
                }
                catch
                {
                    Console.WriteLine("Error: Could not find tag, '" + tag + "'.");
                }
            }
        }
        static private void addDrawingPageRelation(TECDrawing drawing, TECPage page)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("DrawingID", drawing.Guid.ToString());
            data.Add("PageID", page.Guid.ToString());

            if (!SQLiteDB.Insert("TECDrawingTECPage", data))
            {
                Console.WriteLine("Error: Couldn't add relation to TECDrawingTECPage table.");
            }
        }
        static private void addPageVisualScopeRelation(TECPage page, TECVisualScope vs)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("PageID", page.Guid.ToString());
            data.Add("VisualScopeID", vs.Guid.ToString());

            if (!SQLiteDB.Insert("TECPageTECVisualScope", data))
            {
                Console.WriteLine("Error: Couldn't add relation to TECPageTECScope table.");
            }
        }
        static private void addVisualScopeScopeRelation(TECVisualScope vs)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("VisualScopeID", vs.Guid.ToString());
            data.Add("ScopeID", vs.Scope.Guid.ToString());

            if (!SQLiteDB.Insert("TECVisualScopeTECScope", data))
            {
                Console.WriteLine("Could not add relation to TECVisualScopeTECScope table.");
            }
        }

        static private void addLocationInScope(TECLocation location, Guid scopeID)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("ScopeID", scopeID.ToString());
            data.Add("LocationID", location.Guid.ToString());

            if (!SQLiteDB.Insert("TECLocationTECScope", data))
            {
                Console.WriteLine("Error: Couldn't add relation to TECLocationTECScope table.");
            }
        }

        #endregion Add Relation Functions
        #endregion Add Methods

        #region Edit Methods
        static private void editBidInfo(TECBid bid)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("BidInfoID", bid.InfoGuid.ToString());
            data.Add("BidName", bid.Name);
            data.Add("BidNumber", bid.BidNumber);
            data.Add("DueDate", bid.DueDate.ToString(FMT));
            data.Add("Salesperson", bid.Salesperson);
            data.Add("Estimator", bid.Estimator);
            data.Add("PMCoef", bid.Labor.PMCoef.ToString());
            data.Add("ENGCoef", bid.Labor.ENGCoef.ToString());
            data.Add("CommCoef", bid.Labor.CommCoef.ToString());
            data.Add("SoftCoef", bid.Labor.SoftCoef.ToString());
            data.Add("GraphCoef", bid.Labor.GraphCoef.ToString());
            data.Add("ElectricalRate", bid.Labor.ElectricalRate.ToString());

            if (!SQLiteDB.Replace("TECBidInfo", data))
            {
                Console.WriteLine("Error: Couldn't update bid info in TECBidInfo table.");
            }
        }

        static private void editSystem(TECSystem system)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("SystemID", system.Guid.ToString());
            data.Add("Name", system.Name);
            data.Add("Description", system.Description);
            data.Add("Quantity", system.Quantity.ToString());
            data.Add("BudgetPrice", system.BudgetPrice.ToString());

            if (!SQLiteDB.Replace("TECSystem", data))
            {
                Console.WriteLine("Error: Couldn't update system in TECSystem table");
            }
        }

        static private void editEquipment(TECEquipment equipment)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("EquipmentID", equipment.Guid.ToString());
            data.Add("Name", equipment.Name);
            data.Add("Description", equipment.Description);
            data.Add("Quantity", equipment.Quantity.ToString());
            data.Add("BudgetPrice", equipment.BudgetPrice.ToString());

            if (!SQLiteDB.Replace("TECEquipment", data))
            {
                Console.WriteLine("Error: Couldn't update equipment in TECEquipment table.");
            }
        }
        
        static private void editSubScope(TECSubScope subScope)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("SubScopeID", subScope.Guid.ToString());
            data.Add("Name", subScope.Name);
            data.Add("Description", subScope.Description);
            data.Add("Quantity", subScope.Quantity.ToString());

            if (!SQLiteDB.Replace("TECSubScope", data))
            {
                Console.WriteLine("Error: Couldn't update subScope in TECSubScope table.");
            }
        }

        static private void editDevice(TECDevice device)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("DeviceID", device.Guid.ToString());
            data.Add("Name", device.Name);
            data.Add("Description", device.Description);
            data.Add("Quantity", device.Quantity.ToString());
            data.Add("Cost", device.Cost.ToString());
            data.Add("Wire", device.Wire);

            if (!SQLiteDB.Replace("TECDevice", data))
            {
                Console.WriteLine("Error: Couldn't update device in TECDevice table.");
            }
        }

        static private void editManufacturer(TECManufacturer manufacturer)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("ManufacturerID", manufacturer.Guid.ToString());
            data.Add("Name", manufacturer.Name);
            data.Add("Multiplier", manufacturer.Multiplier.ToString());

            if (!SQLiteDB.Replace("TECManfuacturer", data))
            {
                Console.WriteLine("Error: Couldn't update manufacturer in TECManufacturer table.");
            }
        }

        static private void editPoint(TECPoint point)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("PointID", point.Guid.ToString());
            data.Add("Name", point.Name);
            data.Add("Description", point.Description);
            data.Add("Type", point.Type.ToString());
            data.Add("Quantity", point.Quantity.ToString());

            if (!SQLiteDB.Replace("TECPoint", data))
            {
                Console.WriteLine("Error: Couldn't update point in TECPoint table.");
            }
        }

        static private void editNote(TECNote note)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("NoteID", note.Guid.ToString());
            data.Add("NoteText", note.Text);

            if (!SQLiteDB.Replace("TECNote", data))
            {
                Console.WriteLine("Error: Couldn't update item in TECNote table");
            }
        }

        static private void editExclusion(TECExclusion exclusion)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("ExclusionID", exclusion.Guid.ToString());
            data.Add("ExclusionText", exclusion.Text);

            if (!SQLiteDB.Replace("TECExclusion", data))
            {
                Console.WriteLine("Error: Couldn't update item in TECExclusion table.");
            }
        }

        static private void editScopeBranch(TECScopeBranch branch)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("ScopeBranchID", branch.Guid.ToString());
            data.Add("Name", branch.Name);
            data.Add("Description", branch.Description);

            if (!SQLiteDB.Replace("TECScopeBranch", data))
            {
                Console.WriteLine("Error: Couldn't update item in TECScopeBranch table.");
            }
        }

        static private void editDrawing(TECDrawing drawing)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("DrawingID", drawing.Guid.ToString());
            data.Add("Name", drawing.Name);
            data.Add("Description", drawing.Description);
            
            if (!SQLiteDB.Replace("TECDrawing", data))
            {
                Console.WriteLine("Error: Couldn't update item in TECDrawing table.");
            }
        }

        static private void editVisualScope(TECVisualScope vs)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("VisualScopeID", vs.Guid.ToString());
            data.Add("XPos", vs.X.ToString());
            data.Add("YPos", vs.Y.ToString());

            if (!SQLiteDB.Replace("TECVisualScope", data))
            {
                Console.WriteLine("Error: Couldn't update item in TECVisualScope table.");
            }
        }

        static private void editLocation(TECLocation location)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("LocationID", location.Guid.ToString());
            data.Add("Name", location.Name);

            if (!SQLiteDB.Replace("TECLocation", data))
            {
                Console.WriteLine("Error: Couldn't update item in TECLocation table.");
            }
        }
        #endregion Edit Methods

        #region Remove Methods
        #region Remove Objects
        static private void removeSystem(TECSystem system)
        {
            SQLiteDB.Delete("TECSystem", "SystemID", system.Guid);
        }

        static private void removeEquipment(TECEquipment equipment)
        {
            SQLiteDB.Delete("TECEquipment", "EquipmentID", equipment.Guid);
        }

        static private void removeSubScope(TECSubScope subScope)
        {
            SQLiteDB.Delete("TECSubScope", "SubScopeID", subScope.Guid);
        }

        static private void removeDevice(TECDevice device)
        {
            SQLiteDB.Delete("TECDevice", "DeviceID", device.Guid);
        }

        static private void removePoint(TECPoint point)
        {
            SQLiteDB.Delete("TECPoint", "PointID", point.Guid);
        }

        static private void removeScopeBranch(TECScopeBranch branch)
        {
            SQLiteDB.Delete("TECScopeBranch", "ScopeBranchID", branch.Guid);
        }

        static private void removeNote(TECNote note)
        {
            SQLiteDB.Delete("TECNote", "NoteID", note.Guid);
        }

        static private void removeExclusion(TECExclusion exclusion)
        {
            SQLiteDB.Delete("TECExclusion", "ExclusionID", exclusion.Guid);
        }

        static private void removeLocation(TECLocation location)
        {
            SQLiteDB.Delete("TECLocation", "LocationID", location.Guid);
        }
        #endregion Remove Objects

        #region Remove Relations
        private static void removeSystemEquipmentRelation(TECEquipment equipment)
        {
            SQLiteDB.Delete("TECSystemTECEquipment", "EquipmentID", equipment.Guid);
        }

        private static void removeEquipmentSubScopeRelation(TECSubScope subScope)
        {
            SQLiteDB.Delete("TECEquipmentTECSubScope", "SubScopeID", subScope.Guid);
        }

        private static void removeSubScopeDeviceRelation(TECDevice device)
        {
            SQLiteDB.Delete("TECSubScopeTECDevice", "DeviceID", device.Guid);
        }

        private static void removeSubScopePointRelation(TECPoint point)
        {
            SQLiteDB.Delete("TECSubScopeTECPoint", "PointID", point.Guid);
        }

        private static void removeScopeBranchHierarchyRelation(TECScopeBranch branch)
        {
            SQLiteDB.Delete("TECScopeBranchHierarchy", "BranchID", branch.Guid);

        }
        #endregion
        #endregion Remove Methods

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
            DateTime dueDate = DateTime.ParseExact(dueDateString, FMT, CultureInfo.InvariantCulture);

            string salesperson = bidInfoRow["Salesperson"].ToString();
            string estimator = bidInfoRow["Estimator"].ToString();

            TECBid bid = new TECBid(name, bidNumber, dueDate, salesperson, estimator, new ObservableCollection<TECScopeBranch>(), new ObservableCollection<TECSystem>(), new ObservableCollection<TECDevice>(), new ObservableCollection<TECManufacturer>(), new ObservableCollection<TECNote>(), new ObservableCollection<TECExclusion>(), new ObservableCollection<TECTag>(), infoGuid);

            try
            {
                bid.Labor.PMCoef = UtilitiesMethods.StringToDouble(bidInfoRow["PMCoef"].ToString());
                bid.Labor.ENGCoef = UtilitiesMethods.StringToDouble(bidInfoRow["ENGCoef"].ToString());
                bid.Labor.CommCoef = UtilitiesMethods.StringToDouble(bidInfoRow["CommCoef"].ToString());
                bid.Labor.SoftCoef = UtilitiesMethods.StringToDouble(bidInfoRow["SoftCoef"].ToString());
                bid.Labor.GraphCoef = UtilitiesMethods.StringToDouble(bidInfoRow["GraphCoef"].ToString());
                bid.Labor.ElectricalRate = UtilitiesMethods.StringToDouble(bidInfoRow["ElectricalRate"].ToString());
            }
            catch
            {
                Console.WriteLine("Reading labor values from database failed. Using default values.");
                bid.Labor = new TECLabor();
            }

            return bid;
        }

        static private ObservableCollection<TECScopeBranch> getMainScopeBranches()
        {
            ObservableCollection<TECScopeBranch> mainBranches = new ObservableCollection<TECScopeBranch>();

            string command =    "select * from TECScopeBranch where ScopeBranchID not in ";
            command +=          "(select ChildID from TECScopeBranchHierarchy)";

            DataTable mainBranchDT = SQLiteDB.getDataFromCommand(command);

            foreach (DataRow row in mainBranchDT.Rows)
            {
                Guid scopeBranchID = new Guid(row["ScopeBranchID"].ToString());
                string name = row["Name"].ToString();
                string description = row["Description"].ToString();

                ObservableCollection<TECScopeBranch> childBranches = getChildBranches(scopeBranchID);

                TECScopeBranch branch = new TECScopeBranch(name, description, childBranches, scopeBranchID);

                branch.Tags = getTagsInScope(scopeBranchID);

                mainBranches.Add(branch);
            }

            return mainBranches;
        }

        static private ObservableCollection<TECScopeBranch> getChildBranches(Guid parentID)
        {
            ObservableCollection<TECScopeBranch> childBranches = new ObservableCollection<TECScopeBranch>();

            string command =    "select * from TECScopeBranch where ScopeBranchID in ";
            command +=          "(select ChildID from TECScopeBranchHierarchy where ParentID = '";
            command +=          parentID;
            command +=          "')";

            DataTable childBranchDT = SQLiteDB.getDataFromCommand(command);

            foreach (DataRow row in childBranchDT.Rows)
            {
                Guid childBranchID = new Guid(row["ScopeBranchID"].ToString());
                string name = row["Name"].ToString();
                string description = row["Description"].ToString();

                ObservableCollection<TECScopeBranch> grandChildBranches = getChildBranches(childBranchID);

                TECScopeBranch branch = new TECScopeBranch(name, description, grandChildBranches, childBranchID);

                branch.Tags = getTagsInScope(childBranchID);

                childBranches.Add(branch);
            }

            return childBranches;
        }

        static private ObservableCollection<TECSystem> getAllSystems()
        {
            ObservableCollection<TECSystem> systems = new ObservableCollection<TECSystem>();
            DataTable systemsDT = SQLiteDB.getDataFromTable("TECSystem");

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
                system.Location = getLocationInScope(systemID);

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
                Guid equipmentID = new Guid(row["EquipmentID"].ToString());
                string name = row["Name"].ToString();
                string description = row["Description"].ToString();
                string quantityString = row["Quantity"].ToString();
                string budgetPriceString = row["BudgetPrice"].ToString();

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

        static private ObservableCollection<TECSubScope> getOrphanSubScope()
        {
            ObservableCollection<TECSubScope> subScope = new ObservableCollection<TECSubScope>();

            string command = "select * from TECSubScope where SubScopeID not in ";
            command += "(select SubScopeID from TECEquipmentTECSubScope)";

            DataTable subScopeDT = SQLiteDB.getDataFromCommand(command);

            foreach (DataRow row in subScopeDT.Rows)
            {
                Guid subScopeID = new Guid(row["SubScopeID"].ToString());
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

            DataTable devicesDT = SQLiteDB.getDataFromTable("TECDevice");

            foreach (DataRow row in devicesDT.Rows)
            {
                Guid deviceID = new Guid(row["DeviceID"].ToString());
                string name = row["Name"].ToString();
                string description = row["Description"].ToString();
                string costString = row["Cost"].ToString();
                string wire = row["Wire"].ToString();string quantityString = row["Quantity"].ToString();

                int quantity;
                if (!int.TryParse(quantityString, out quantity))
                {
                    quantity = 1;
                    Console.WriteLine("Cannot convert quantity to int in device, setting to 1");
                }

                double cost;
                if (!double.TryParse(costString, out cost))
                {
                    cost = 0;
                    Console.WriteLine("Cannot convert cost to double, setting to 0");
                }

                TECManufacturer manufacturer = getManufacturerInDevice(deviceID);

                TECDevice deviceToAdd = new TECDevice(name, description, cost, wire, manufacturer, deviceID);

                deviceToAdd.Quantity = quantity;
                deviceToAdd.Tags = getTagsInScope(deviceID);

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

                manufacturers.Add(new TECManufacturer(name, "", multiplier, manufacturerID));
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

            string command =    "select * from TECEquipment where EquipmentID in ";
            command +=          "(select EquipmentID from TECSystemTECEquipment where SystemID = '";
            command +=          systemID;
            command +=          "')";

            DataTable equipmentDT = SQLiteDB.getDataFromCommand(command);

            foreach (DataRow row in equipmentDT.Rows)
            {
                Guid equipmentID = new Guid(row["EquipmentID"].ToString());
                string name = row["Name"].ToString();
                string description = row["Description"].ToString();
                string quantityString = row["Quantity"].ToString();
                string budgetPriceString = row["BudgetPrice"].ToString();

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
                equipmentToAdd.Location = getLocationInScope(equipmentID);

                equipment.Add(equipmentToAdd);
            }

            return equipment;
        }

        static private ObservableCollection<TECSubScope> getSubScopeInEquipment(Guid equipmentID)
        {
            ObservableCollection<TECSubScope> subScope = new ObservableCollection<TECSubScope>();

            string command =    "select * from TECSubScope where SubScopeID in ";
            command +=          "(select SubScopeID from TECEquipmentTECSubScope where EquipmentID = '";
            command +=          equipmentID;
            command +=          "')";

            DataTable subScopeDT = SQLiteDB.getDataFromCommand(command);

            foreach (DataRow row in subScopeDT.Rows)
            {
                Guid subScopeID = new Guid(row["SubScopeID"].ToString());
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
                subScopeToAdd.Location = getLocationInScope(subScopeID);
                subScope.Add(subScopeToAdd);
            }

            return subScope;
        }

        static private ObservableCollection<TECDevice> getDevicesInSubScope(Guid subScopeID)
        {
            //Console.WriteLine("getDevicesInSubScope() called");

            ObservableCollection<TECDevice> devices = new ObservableCollection<TECDevice>();

            string command = "select * from TECDevice where DeviceID in ";
            command += "(select DeviceID from TECSubScopeTECDevice where SubScopeID = '";
            command += subScopeID;
            command += "')";

            DataTable devicesDT = SQLiteDB.getDataFromCommand(command);

            foreach (DataRow row in devicesDT.Rows)
            {
                Guid deviceID = new Guid(row["DeviceID"].ToString());
                string name = row["Name"].ToString();
                string description = row["Description"].ToString();
                string costString = row["Cost"].ToString();
                string wire = row["Wire"].ToString();
                string quantityString = row["Quantity"].ToString();

                int quantity;
                if (!int.TryParse(quantityString, out quantity))
                {
                    quantity = 1;
                    Console.WriteLine("Cannot convert quantity to int in device, setting to 1");
                }

                double cost;
                if (!double.TryParse(costString, out cost))
                {
                    cost = 0;
                    Console.WriteLine("Cannot convert cost to double, setting to 0");
                }

                TECManufacturer manufacturer = getManufacturerInDevice(deviceID);

                TECDevice deviceToAdd = new TECDevice(name, description, cost, wire, manufacturer, deviceID);

                deviceToAdd.Quantity = quantity;
                deviceToAdd.Tags = getTagsInScope(deviceID);

                devices.Add(deviceToAdd);
            }

            return devices;
        }

        static private ObservableCollection<TECPoint> getPointsInSubScope(Guid subScopeID)
        {
            ObservableCollection<TECPoint> points = new ObservableCollection<TECPoint>();

            string command = "select * from TECPoint where PointID in ";
            command += "(select PointID from TECSubScopeTECPoint where SubScopeID = '";
            command += subScopeID;
            command += "')";

            DataTable pointsDT = SQLiteDB.getDataFromCommand(command);

            foreach (DataRow row in pointsDT.Rows)
            {
                Guid pointID = new Guid(row["PointID"].ToString());
                string name = row["Name"].ToString();
                string description = row["Description"].ToString();
                string type = row["Type"].ToString();
                string quantityString = row["Quantity"].ToString();

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

                return new TECManufacturer(name, "", multiplier, manufacturerID);
            }
            else
            {
                return new TECManufacturer();
            }
        }

        static private ObservableCollection<TECNote> getNotes()
        {
            ObservableCollection<TECNote> notes = new ObservableCollection<TECNote>();
            DataTable notesDT = SQLiteDB.getDataFromTable("TECNote");

            foreach (DataRow row in notesDT.Rows)
            {
                Guid noteID = new Guid(row["NoteID"].ToString());
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

            string command = "select * from TECDrawing";

            DataTable ghostDrawingsDT;

            try
            {
                ghostDrawingsDT = SQLiteDB.getDataFromCommand(command);

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
                    int pageNum = UtilitiesMethods.StringToInt(row["PageNum"].ToString());
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
                    double xPos = UtilitiesMethods.StringToDouble(row["XPos"].ToString());
                    double yPos = UtilitiesMethods.StringToDouble(row["YPos"].ToString());

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
        #endregion //Loading from DB Methods

        #region Create Object Tables Methods
        static private void createBidObjectTables()
        {
            SQLiteDB.nonQueryCommand(newBidInfoTable());
            SQLiteDB.nonQueryCommand(newNoteTable());
            SQLiteDB.nonQueryCommand(newExclusionTable());
            SQLiteDB.nonQueryCommand(newScopeBranchTable());
            SQLiteDB.nonQueryCommand(newSystemTable());
            SQLiteDB.nonQueryCommand(newEquipmentTable());
            SQLiteDB.nonQueryCommand(newSubScopeTable());
            SQLiteDB.nonQueryCommand(newDeviceTable());
            SQLiteDB.nonQueryCommand(newPointTable());
            SQLiteDB.nonQueryCommand(newTagTable());
            SQLiteDB.nonQueryCommand(newManufacturerTable());
            SQLiteDB.nonQueryCommand(newDrawingTable());
            SQLiteDB.nonQueryCommand(newPageTable());
            SQLiteDB.nonQueryCommand(newVisualScopeTable());
        }
        static private void createTemplateObjectTables()
        {
            SQLiteDB.nonQueryCommand(newSystemTable());
            SQLiteDB.nonQueryCommand(newEquipmentTable());
            SQLiteDB.nonQueryCommand(newSubScopeTable());
            SQLiteDB.nonQueryCommand(newDeviceTable());
            SQLiteDB.nonQueryCommand(newPointTable());
            SQLiteDB.nonQueryCommand(newTagTable());
            SQLiteDB.nonQueryCommand(newManufacturerTable());
        }

        static private string newBidInfoTable()
        {
            return "CREATE TABLE 'TECBidInfo' (" +
                "'BidInfoID'    TEXT," +
                "'BidName'      TEXT," +
                "'BidNumber'    TEXT," +
                "'DueDate'      TEXT," +
                "'Salesperson'  TEXT," +
                "'Estimator'    TEXT," +

                "'PMCoef'       REAL," +
                "'ENGCoef'      REAL," +
                "'CommCoef'     REAL," +
                "'SoftCoef'     REAL," +
                "'GraphCoef'    REAL," +
                
                "'ElectricalRate'   REAL," +
                "PRIMARY KEY(BidInfoID)" +
            ");";
        }
        static private string newNoteTable()
        {
            return "CREATE TABLE 'TECNote' (" +
                "'NoteID'       TEXT," +
                "'NoteText'     TEXT," +
                "PRIMARY KEY(NoteID)" +
            ");";
        }
        static private string newExclusionTable()
        {
            return "CREATE TABLE 'TECExclusion' (" +
                "'ExclusionID'      TEXT," +
                "'ExclusionText'    TEXT," +
                "PRIMARY KEY(ExclusionID)" +
            ");";
        }
        static private string newScopeBranchTable()
        {
            return "CREATE TABLE `TECScopeBranch` (" +
                "`ScopeBranchID`	TEXT," +
                "`Name`	TEXT," +
                "`Description`	TEXT," +
                "PRIMARY KEY(ScopeBranchID)" +
            ");";
        }
        static private string newSystemTable()
        {
            return "CREATE TABLE `TECSystem` (" +
                "`SystemID`	TEXT," +
                "`Name`	TEXT," +
                "`Description`	TEXT," +
                "'Quantity'     INTEGER," +
                "'BudgetPrice'  REAL," +
                "PRIMARY KEY(SystemID)" +
            ");";
        }
        static private string newEquipmentTable()
        {
            return "CREATE TABLE `TECEquipment` (" +
                "`EquipmentID`	TEXT," +
                "`Name`	TEXT," +
                "`Description`	TEXT," +
                "'Quantity'     INTEGER," +
                "'BudgetPrice'  REAL," +
                "PRIMARY KEY(EquipmentID)" +
            ");";
        }
        static private string newSubScopeTable()
        {
            return "CREATE TABLE `TECSubScope` (" +
                "`SubScopeID`	TEXT," +
                "`Name`	TEXT," +
                "`Description`	TEXT," +
                "'Quantity'     INTEGER," +
                "PRIMARY KEY(SubScopeID)" +
            ");";
        }
        static private string newDeviceTable()
        {
            return "CREATE TABLE `TECDevice` (" +
                "`DeviceID`	TEXT," +
                "'Name' TEXT," +
                "'Description'  TEXT," +
                "'Quantity'     INTEGER," +
                "`Cost`	REAL," +
                "`Wire`	TEXT," +
                "PRIMARY KEY(DeviceID)" +
            ");";
        }
        static private string newPointTable()
        {
            return "CREATE TABLE `TECPoint` (" +
                "`PointID`	TEXT," +
                "`Name`	TEXT," +
                "`Description`	TEXT," +
                "'Quantity'     INTEGER," +
                "`Type`	TEXT," +
                "PRIMARY KEY(PointID)" +
            ");";
        }
        static private string newTagTable()
        {
            return "CREATE TABLE 'TECTag' (" +
                "'TagID'        TEXT," +
                "'TagString'    TEXT," +
                "PRIMARY KEY(TagID)" +
            ");";
        }
        static private string newManufacturerTable()
        {
            return "CREATE TABLE 'TECManufacturer' (" +
                "'ManufacturerID' TEXT," +
                "'Name' TEXT," +
                "'Multiplier' REAL," +
                "PRIMARY KEY(ManufacturerID)" +
            ");";
        }
        static private string newDrawingTable()
        {
            return "CREATE TABLE 'TECDrawing' (" +
                "'DrawingID' TEXT," +
                "'Name' TEXT," +
                "'Description' TEXT," +
                "PRIMARY KEY(DrawingID)" +
            ");";
        }
        static private string newPageTable()
        {
            return "CREATE TABLE 'TECPage' (" +
                "'PageID' TEXT," +
                "'Image' BLOB," +
                "'PageNum' INTEGER," +
                "PRIMARY KEY(PageID)" +
            ");";
        }
        static private string newLocationTable()
        {
            return "CREATE TABLE 'TECLocation' (" +
                "'LocationID' TEXT," +
                "'Name' TEXT," +
                "PRIMARY KEY(LocationID)" +
            ");";
        }

        static private string newVisualScopeTable()
        {
            return "CREATE TABLE 'TECVisualScope' (" +
                "'VisualScopeID' TEXT," +
                "'XPos' REAL," +
                "'YPos' REAL," +
                "PRIMARY KEY(VisualScopeID)" +
            ");";
        }

        #endregion //Create Object Tables

        #region Create Relation Tables Methods
        static private void createBidRelationTables()
        {
            SQLiteDB.nonQueryCommand(newScopeBranchHierarchyTable());
            SQLiteDB.nonQueryCommand(newSystemEquipmentTable());
            SQLiteDB.nonQueryCommand(newEquipmentSubScopeTable());
            SQLiteDB.nonQueryCommand(newSubScopeDeviceTable());
            SQLiteDB.nonQueryCommand(newSubScopePointTable());

            SQLiteDB.nonQueryCommand(newScopeTagTable());
            SQLiteDB.nonQueryCommand(newDeviceManufacturerTable());

            SQLiteDB.nonQueryCommand(newDrawingPageTable());
            SQLiteDB.nonQueryCommand(newPageVisScopeTable());
            SQLiteDB.nonQueryCommand(newVisScopeScopeTable());
        }
        static private void createTemplateRelationTables()
        {
            SQLiteDB.nonQueryCommand(newSystemEquipmentTable());
            SQLiteDB.nonQueryCommand(newEquipmentSubScopeTable());
            SQLiteDB.nonQueryCommand(newSubScopeDeviceTable());
            SQLiteDB.nonQueryCommand(newSubScopePointTable());

            SQLiteDB.nonQueryCommand(newScopeTagTable());
            SQLiteDB.nonQueryCommand(newDeviceManufacturerTable());
        }

        static private string newScopeBranchHierarchyTable()
        {
            return "CREATE TABLE `TECScopeBranchHierarchy` (" +
                "`ParentID`	TEXT," +
                "`ChildID`	TEXT" +
            ");";
        }
        static private string newSystemEquipmentTable()
        {
            return "CREATE TABLE `TECSystemTECEquipment` (" +
                "`SystemID`	TEXT," +
                "`EquipmentID`	TEXT" +
            ");";
        }
        static private string newEquipmentSubScopeTable()
        {
            return "CREATE TABLE `TECEquipmentTECSubScope` (" +
                "`EquipmentID`	TEXT," +
                "`SubScopeID`	TEXT" +
            ");";
        }
        static private string newSubScopeDeviceTable()
        {
            return "CREATE TABLE `TECSubScopeTECDevice` (" +
                "`SubScopeID`	TEXT," +
                "`DeviceID`	TEXT" +
            ");";
        }
        static private string newSubScopePointTable()
        {
            return "CREATE TABLE `TECSubScopeTECPoint` (" +
                "`SubScopeID`	TEXT," +
                "`PointID`	TEXT" +
            ");";
        }

        static private string newScopeTagTable()
        {
            return "CREATE TABLE `TECScopeTECTag` (" +
                "`ScopeID`	TEXT," +
                "`TagID`	TEXT" +
            ");";
        }

        static private string newDeviceManufacturerTable()
        {
            return "CREATE TABLE 'TECDeviceTECManufacturer' (" +
                "'DeviceID' TEXT," +
                "'ManufacturerID' TEXT"  +
            ");";
        }

        static private string newDrawingPageTable()
        {
            return "CREATE TABLE 'TECDrawingTECPage' (" +
                "'DrawingID' TEXT," +
                "'PageID' TEXT" +
            ");";
        }
        static private string newPageVisScopeTable()
        {
            return "CREATE TABLE 'TECPageTECVisualScope' (" +
                "'PageID' TEXT," +
                "'VisualScopeID' TEXT" +
            ");";
        }
        static private string newVisScopeScopeTable()
        {
            return "Create Table 'TECVisualScopeTECScope' (" +
                "'VisualScopeID' TEXT," +
                "'ScopeID' TEXT" +
            ");";
        }
        static private string newLocationScopeTable()
        {
            return "Create Table 'TECLocationTECScope' (" +
                "'LocationID' TEXT," +
                "'ScopeID' TEXT" +
                ");";
        }
        #endregion //Create Relation Tables

        #region Link Methods
        static private void linkAllVisualScope(ObservableCollection<TECDrawing> bidDrawings, ObservableCollection<TECSystem> bidSystems)
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

        static private void linkAllLocations(ObservableCollection<TECLocation> locations, ObservableCollection<TECSystem> systems)
        {
            ObservableCollection<TECScope> scopeToLink = new ObservableCollection<TECScope>();

            foreach (TECSystem sys in systems)
            {
                if (sys.Location != null) { scopeToLink.Add(sys); }
                foreach (TECEquipment equip in sys.Equipment)
                {
                    if (equip.Location != null) { scopeToLink.Add(equip); }
                    foreach (TECSubScope ss in equip.SubScope)
                    {
                        if (ss.Location != null) { scopeToLink.Add(ss); }
                    }
                }
            }

            ObservableCollection<TECScope> scopeToRemove = new ObservableCollection<TECScope>();
            foreach (TECLocation location in locations)
            {
                foreach (TECScope scope in scopeToLink)
                {
                    if (scope.Location.Guid == location.Guid)
                    {
                        scope.Location = location;
                        scopeToRemove.Add(scope);
                    }
                }
                foreach (TECScope scope in scopeToRemove)
                {
                    scopeToLink.Remove(scope);
                }
                scopeToRemove.Clear();
            }
            
        }
        #endregion Link Methods
    }
}
