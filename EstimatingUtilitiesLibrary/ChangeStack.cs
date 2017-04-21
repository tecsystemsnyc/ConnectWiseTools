using DebugLibrary;
using EstimatingLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EstimatingUtilitiesLibrary
{
    public enum Change {Add, Remove, Edit, AddRelationship, RemoveRelationship};
    public class ChangeStack
    {
        //List of change, target object, reference object
        //Example: Add, Bid, System
        //Example: Edit, New Object, Old Object
        public List<StackItem> UndoStack { get; set; }
        public List<StackItem> RedoStack { get; set; }
        public ObservableCollection<StackItem> SaveStack { get; set; }
        public TECBid Bid;
        public TECTemplates Templates;

        private const bool DEBUG_PROPERTIES = false;
        private const bool DEBUG_STACK = false;
        private const bool DEBUG_REGISTER = false;
        
        private bool isDoing = false;
        
        #region Constructors
        public ChangeStack()
        {
            UndoStack = new List<StackItem>();
            RedoStack = new List<StackItem>();
            SaveStack = new ObservableCollection<StackItem>();
            SaveStack.CollectionChanged += SaveStack_CollectionChanged;
        }
        public ChangeStack(TECBid bid) : this()
        {
            Bid = bid;
            registerBidChanges(bid);
        }
        public ChangeStack(TECTemplates templates) : this()
        {
            Templates = templates;
            registerTemplatesChanges(templates);
        }
        #endregion

        #region Collection Watching
        private void SaveStack_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (object item in e.NewItems)
                {
                    var obj = item;
                }
            }
        }
        #endregion

        #region Event Handlers
        private void Object_PropertyChanged(object sender, PropertyChangedEventArgs e) { handlePropertyChanged(e); }
        private void handlePropertyChanged(PropertyChangedEventArgs e)
        {
            string message = "Propertychanged: " + e.PropertyName;
            DebugHandler.LogDebugMessage(message, DEBUG_PROPERTIES);

            if (!isDoing) { RedoStack.Clear(); }
            if (e is PropertyChangedExtendedEventArgs<Object>)
            {
                PropertyChangedExtendedEventArgs<Object> args = e as PropertyChangedExtendedEventArgs<Object>;
                StackItem item;
                object oldValue = args.OldValue;
                object newValue = args.NewValue;
                if (e.PropertyName == "Add")
                {
                    message = "Add change: " + oldValue;
                    DebugHandler.LogDebugMessage(message, DEBUG_PROPERTIES);

                    item = new StackItem(Change.Add, args);
                    item.TargetObject.PropertyChanged += Object_PropertyChanged;
                    handleChildren(item);
                    UndoStack.Add(item);
                    SaveStack.Add(item);

                    message = "Undo count: " + UndoStack.Count + " Save Count: " + SaveStack.Count;
                    DebugHandler.LogDebugMessage(message, DEBUG_STACK);
                }
                else if (e.PropertyName == "Remove")
                {
                    message = "Remove change: " + oldValue;
                    DebugHandler.LogDebugMessage(message, DEBUG_PROPERTIES);

                    item = new StackItem(Change.Remove, args);
                    ((TECObject)newValue).PropertyChanged -= Object_PropertyChanged;
                    handleChildren(item);
                    UndoStack.Add(item);
                    SaveStack.Add(item);

                    message = "Undo count: " + UndoStack.Count + " Save Count: " + SaveStack.Count;
                    DebugHandler.LogDebugMessage(message, DEBUG_STACK);
                }
                else if (e.PropertyName == "Edit")
                {
                    message = "Edit change: " + oldValue;
                    DebugHandler.LogDebugMessage(message, DEBUG_PROPERTIES);

                    item = new StackItem(Change.Edit, args);
                    SaveStack.Add(item);

                    message = "Undo count: " + UndoStack.Count + " Save Count: " + SaveStack.Count;
                    DebugHandler.LogDebugMessage(message, DEBUG_STACK);
                }
                else if (e.PropertyName == "ChildChanged")
                {
                    message = "Child change: " + oldValue;
                    DebugHandler.LogDebugMessage(message, DEBUG_PROPERTIES);

                    item = new StackItem(Change.Edit, args);
                    SaveStack.Add(item);

                    message = "Undo count: " + UndoStack.Count + " Save Count: " + SaveStack.Count;
                    DebugHandler.LogDebugMessage(message, DEBUG_STACK);
                }
                else if (e.PropertyName == "ObjectPropertyChanged")
                {
                    message = "Object changed: " + oldValue;
                    DebugHandler.LogDebugMessage(message, DEBUG_PROPERTIES);

                    var oldNew = newValue as Tuple<Object, Object>;
                    var toSave = new List<StackItem>();
                    if (oldNew.Item1 != null)
                    {
                        toSave.Add(new StackItem(Change.Remove, oldValue, oldNew.Item1, args.OldType, args.NewType));
                    }
                    if (oldNew.Item2 != null)
                    {
                        toSave.Add(new StackItem(Change.Add, oldValue, oldNew.Item2, args.OldType, args.NewType));
                    }
                    foreach (var save in toSave)
                    {
                        SaveStack.Add(save);
                    }

                    message = "Undo count: " + UndoStack.Count + " Save Count: " + SaveStack.Count;
                    DebugHandler.LogDebugMessage(message, DEBUG_STACK);
                }
                else if (e.PropertyName == "RelationshipPropertyChanged")
                {
                    message = "Object changed: " + oldValue;
                    DebugHandler.LogDebugMessage(message, DEBUG_PROPERTIES);

                    var oldNew = newValue as Tuple<Object, Object>;
                    var toSave = new List<StackItem>();
                    if (oldNew.Item1 != null)
                    {
                        toSave.Add(new StackItem(Change.RemoveRelationship, oldValue, oldNew.Item1, args.OldType, args.NewType));
                    }
                    if (oldNew.Item2 != null)
                    {
                        toSave.Add(new StackItem(Change.AddRelationship, oldValue, oldNew.Item2, args.OldType, args.NewType));
                    }
                    foreach (var save in toSave)
                    {
                        SaveStack.Add(save);
                    }

                    message = "Undo count: " + UndoStack.Count + " Save Count: " + SaveStack.Count;
                    DebugHandler.LogDebugMessage(message, DEBUG_STACK);
                }
                else if (e.PropertyName == "MetaAdd")
                {
                    message = "MetaAdd change: " + oldValue;
                    DebugHandler.LogDebugMessage(message, DEBUG_PROPERTIES);

                    item = new StackItem(Change.Add, args);
                    ((TECObject)newValue).PropertyChanged += Object_PropertyChanged;
                    SaveStack.Add(item);

                    message = "Undo count: " + UndoStack.Count + " Save Count: " + SaveStack.Count;
                    DebugHandler.LogDebugMessage(message, DEBUG_STACK);
                }
                else if (e.PropertyName == "MetaRemove")
                {
                    message = "MetaRemove change: " + oldValue;
                    DebugHandler.LogDebugMessage(message, DEBUG_PROPERTIES);

                    item = new StackItem(Change.Remove, args);
                    ((TECObject)newValue).PropertyChanged -= Object_PropertyChanged;
                    SaveStack.Add(item);

                    message = "Undo count: " + UndoStack.Count + " Save Count: " + SaveStack.Count;
                    DebugHandler.LogDebugMessage(message, DEBUG_STACK);
                }
                else if (e.PropertyName == "AddRelationship")
                {
                    message = "Add relationship change: " + oldValue;
                    DebugHandler.LogDebugMessage(message, DEBUG_PROPERTIES);

                    item = new StackItem(Change.AddRelationship, args);
                    UndoStack.Add(item);
                    SaveStack.Add(item);

                    message = "Undo count: " + UndoStack.Count + " Save Count: " + SaveStack.Count;
                    DebugHandler.LogDebugMessage(message, DEBUG_STACK);
                }
                else if (e.PropertyName == "RemoveRelationship")
                {
                    message = "Remove relationship change: " + oldValue;
                    DebugHandler.LogDebugMessage(message, DEBUG_PROPERTIES);

                    item = new StackItem(Change.RemoveRelationship, args);
                    UndoStack.Add(item);
                    SaveStack.Add(item);

                    message = "Undo count: " + UndoStack.Count + " Save Count: " + SaveStack.Count;
                    DebugHandler.LogDebugMessage(message, DEBUG_STACK);
                }
                else if (e.PropertyName == "RemovedSubScope") { }
                else if (e.PropertyName == "AddCatalog")
                {
                    message = "Add change: " + oldValue;
                    DebugHandler.LogDebugMessage(message, DEBUG_PROPERTIES);

                    item = new StackItem(Change.Add, args);
                    UndoStack.Add(item);
                    SaveStack.Add(item);

                    message = "Undo count: " + UndoStack.Count + " Save Count: " + SaveStack.Count;
                    DebugHandler.LogDebugMessage(message, DEBUG_STACK);
                }
                else if (e.PropertyName == "RemoveCatalog")
                {
                    message = "Remove change: " + oldValue;
                    DebugHandler.LogDebugMessage(message, DEBUG_PROPERTIES);

                    item = new StackItem(Change.Remove, args);
                    UndoStack.Add(item);
                    SaveStack.Add(item);

                    message = "Undo count: " + UndoStack.Count + " Save Count: " + SaveStack.Count;
                    DebugHandler.LogDebugMessage(message, DEBUG_STACK);
                }
                //else if (e.PropertyName == "Index Changed")
                //{

                //}
                else
                {
                    message = "Edit change: " + oldValue;
                    DebugHandler.LogDebugMessage(message, DEBUG_PROPERTIES);

                    item = new StackItem(Change.Edit, args);
                    UndoStack.Add(item);
                    SaveStack.Add(item);

                    message = "Undo count: " + UndoStack.Count + " Save Count: " + SaveStack.Count;
                    DebugHandler.LogDebugMessage(message, DEBUG_STACK);
                }
                
            }
            else
            {
                message = "Property not compatible: " + e.PropertyName;
                DebugHandler.LogDebugMessage(message, DEBUG_PROPERTIES);
                message = "Undo count: " + UndoStack.Count + " Save Count: " + SaveStack.Count;
                DebugHandler.LogDebugMessage(message, DEBUG_STACK);
            }
        }
        #endregion //Event Handlers

        #region Methods

        public void Undo()
        {
            isDoing = true;
            StackItem item = UndoStack.Last();
            DebugHandler.LogDebugMessage("Undoing:       " + item.Change.ToString() + "    #Undo: " + UndoStack.Count() + "    #Redo: " + RedoStack.Count(), DEBUG_STACK);
            if (item.Change == Change.Add || item.Change == Change.AddRelationship)
            {
                handleAdd(item);
                UndoStack.Remove(item);
                UndoStack.Remove(UndoStack.Last());
                RedoStack.Add(item);
            }
            else if (item.Change == Change.Remove || item.Change == Change.RemoveRelationship)
            {
                handleRemove(item);
                UndoStack.Remove(item);
                UndoStack.Remove(UndoStack.Last());
                RedoStack.Add(item);
            }
            else if (item.Change == Change.Edit)
            {
                int index = UndoStack.IndexOf(item);
                RedoStack.Add(new StackItem(Change.Edit, copy(item.TargetObject), item.TargetObject));
                handleEdit(item);
                for (int x = (UndoStack.Count - 1); x >= index; x--)
                {
                    UndoStack.RemoveAt(x);
                }
            }

            string message = "After Undoing: " + item.Change.ToString() + "    #Undo: " + UndoStack.Count() + "    #Redo: " + RedoStack.Count() + "\n";
            DebugHandler.LogDebugMessage(message, DEBUG_STACK);

            isDoing = false;
        }
        public void Redo()
        {
            isDoing = true;
            StackItem item = RedoStack.Last();

            string message = "Redoing:       " + item.Change.ToString() + "    #Undo: " + UndoStack.Count() + "    #Redo: " + RedoStack.Count();
            DebugHandler.LogDebugMessage(message, DEBUG_STACK);

            if (item.Change == Change.Add)
            {
                handleRemove(item);
                RedoStack.Remove(item);
            }
            else if (item.Change == Change.Remove)
            {
                handleAdd(item);
                RedoStack.Remove(item);
            }
            else if (item.Change == Change.Edit)
            {
                int index = 0;
                if (UndoStack.Count > 0)
                {
                    index = UndoStack.IndexOf(UndoStack.Last());
                }

                handleEdit(item);
                RedoStack.Remove(item);
                for (int x = (UndoStack.Count - 2); x > index; x--)
                {
                    UndoStack.RemoveAt(x);
                }
            }

            message = "After Redoing: " + item.Change.ToString() + "    #Undo: " + UndoStack.Count() + "    #Redo: " + RedoStack.Count() + "\n";
            DebugHandler.LogDebugMessage(message, DEBUG_STACK);

            isDoing = false;
        }
        public void ClearStacks()
        {
            UndoStack.Clear();
            RedoStack.Clear();
            SaveStack.Clear();
        }
        public ChangeStack Copy()
        {
            var outStack = new ChangeStack();
            foreach (var item in UndoStack)
            {
                outStack.UndoStack.Add(item);
            }
            foreach (var item in RedoStack)
            {
                outStack.RedoStack.Add(item);
            }
            foreach (var item in SaveStack)
            {
                outStack.SaveStack.Add(item);
            }
            return outStack;
        }

        private void registerBidChanges(TECBid Bid)
        {
            //Bid Changed
            Bid.PropertyChanged += Object_PropertyChanged;
            Bid.Labor.PropertyChanged += Object_PropertyChanged;
            Bid.Parameters.PropertyChanged += Object_PropertyChanged;
            //System Changed
            foreach (TECScopeBranch branch in Bid.ScopeTree)
            { registerScope(branch); }
            //Notes changed
            foreach (TECNote note in Bid.Notes)
            { note.PropertyChanged += Object_PropertyChanged; }
            //Exclusions changed
            foreach (TECExclusion exclusion in Bid.Exclusions)
            { exclusion.PropertyChanged += Object_PropertyChanged; }
            //Locations changed
            foreach (TECLocation location in Bid.Locations)
            { location.PropertyChanged += Object_PropertyChanged; }
            //Manufacturers Changed
            foreach (TECManufacturer manufacturer in Bid.ManufacturerCatalog)
            { manufacturer.PropertyChanged += Object_PropertyChanged; }
            foreach (TECSystem system in Bid.Systems)
            { registerSystems(system); }
            //Bid.Drawings.CollectionChanged += Bid_CollectionChanged;
            foreach (TECDrawing drawing in Bid.Drawings)
            {
                drawing.PropertyChanged += Object_PropertyChanged;
                foreach (TECPage page in drawing.Pages)
                {
                    page.PropertyChanged += Object_PropertyChanged;
                    foreach (TECVisualScope vs in page.PageScope)
                    { vs.PropertyChanged += Object_PropertyChanged; }
                }
            }
            foreach (TECController controller in Bid.Controllers)
            { registerController(controller); }
            foreach (TECProposalScope propScope in Bid.ProposalScope)
            { registerPropScope(propScope); }
            foreach(TECConnectionType connectionType in Bid.ConnectionTypes)
            { connectionType.PropertyChanged += Object_PropertyChanged; }
            foreach(TECConduitType conduitType in Bid.ConduitTypes)
            { conduitType.PropertyChanged += Object_PropertyChanged; }
            foreach(TECAssociatedCost cost in Bid.AssociatedCostsCatalog)
            { cost.PropertyChanged += Object_PropertyChanged; }
            foreach(TECMiscCost cost in Bid.MiscCosts)
            { cost.PropertyChanged += Object_PropertyChanged; }
            foreach (TECMiscWiring wiring in Bid.MiscWiring)
            { wiring.PropertyChanged += Object_PropertyChanged; }
            foreach (TECPanelType panelType in Bid.PanelTypeCatalog)
            { panelType.PropertyChanged += Object_PropertyChanged; }
            foreach (TECPanel panel in Bid.Panels)
            { panel.PropertyChanged += Object_PropertyChanged; }
            foreach (TECIOModule ioModule in Bid.IOModuleCatalog)
            {
                ioModule.PropertyChanged += Object_PropertyChanged;
            }
            foreach (TECDevice device in Bid.DeviceCatalog)
            { device.PropertyChanged += Object_PropertyChanged; }
        }
        private void registerTemplatesChanges(TECTemplates Templates)
        {
            //Template Changed
            Templates.PropertyChanged += Object_PropertyChanged;
            foreach (TECSystem system in Templates.SystemTemplates)
            { registerSystems(system); }
            foreach (TECEquipment equipment in Templates.EquipmentTemplates)
            { registerEquipment(equipment); }
            foreach (TECSubScope subScope in Templates.SubScopeTemplates)
            { registerSubScope(subScope); }
            foreach (TECDevice device in Templates.DeviceCatalog)
            { device.PropertyChanged += Object_PropertyChanged; }
            foreach (TECTag tag in Templates.Tags)
            { tag.PropertyChanged += Object_PropertyChanged; }
            foreach (TECManufacturer manufacturer in Templates.ManufacturerCatalog)
            { manufacturer.PropertyChanged += Object_PropertyChanged; }
            foreach (TECController controller in Templates.ControllerTemplates)
            { registerController(controller); }
            foreach (TECConnectionType connectionType in Templates.ConnectionTypeCatalog)
            { connectionType.PropertyChanged += Object_PropertyChanged; }
            foreach (TECConduitType conduitType in Templates.ConduitTypeCatalog)
            { conduitType.PropertyChanged += Object_PropertyChanged; }
            foreach (TECAssociatedCost cost in Templates.AssociatedCostsCatalog)
            { cost.PropertyChanged += Object_PropertyChanged; }
            foreach (TECMiscCost addition in Templates.MiscCostTemplates)
            { addition.PropertyChanged += Object_PropertyChanged; }
            foreach (TECMiscWiring addition in Templates.MiscWiringTemplates)
            { addition.PropertyChanged += Object_PropertyChanged; }
            foreach (TECPanelType addition in Templates.PanelTypeCatalog)
            { addition.PropertyChanged += Object_PropertyChanged; }
            foreach (TECPanel panel in Templates.PanelTemplates)
            { panel.PropertyChanged += Object_PropertyChanged; }
            foreach (TECControlledScope scope in Templates.ControlledScopeTemplates)
            { registerControlledScope(scope); }
            foreach (TECIOModule ioModule in Templates.IOModuleCatalog)
            {
                ioModule.PropertyChanged += Object_PropertyChanged;
            }
        }
        private void registerSubScope(TECSubScope subScope)
        {
            //Subscope Changed
            subScope.PropertyChanged += Object_PropertyChanged;
            foreach (TECPoint point in subScope.Points)
            {
                //Point Changed
                point.PropertyChanged += Object_PropertyChanged;
            }
        }
        private void registerEquipment(TECEquipment equipment)
        {
            //equipment Changed
            equipment.PropertyChanged += Object_PropertyChanged;
            foreach (TECSubScope subScope in equipment.SubScope)
            {
                registerSubScope(subScope);
            }
        }
        private void registerSystems(TECSystem system)
        {
            //SystemChanged
            system.PropertyChanged += Object_PropertyChanged;
            foreach (TECEquipment equipment in system.Equipment)
            {
                registerEquipment(equipment);
            }
        }
        private void registerScope(TECScopeBranch branch)
        {

            DebugHandler.LogDebugMessage(("Scope Branch Registered. Name: " + branch.Name), DEBUG_REGISTER);
            branch.PropertyChanged += Object_PropertyChanged;
            foreach(TECScopeBranch scope in branch.Branches)
            {
                registerScope(scope);
            }
        }
        private void unregisterScope(TECScopeBranch branch)
        {
            foreach (TECScopeBranch scope in branch.Branches)
            {
                scope.PropertyChanged -= Object_PropertyChanged;
                unregisterScope(scope);
            }
        }
        private void registerPropScope(TECProposalScope pScope)
        {
            pScope.PropertyChanged += Object_PropertyChanged;
            foreach(TECProposalScope child in pScope.Children)
            {
                registerPropScope(child);
            }
            foreach(TECScopeBranch child in pScope.Notes)
            {
                registerScope(child);
            }
        }
        private void registerControlledScope(TECControlledScope scope)
        {
            scope.PropertyChanged += Object_PropertyChanged;
            foreach(TECPanel panel in scope.Panels)
            {
                panel.PropertyChanged += Object_PropertyChanged;
            }
            foreach(TECController controller in scope.Controllers)
            {
                registerController(controller);
            }
            foreach(TECSystem system in scope.Systems)
            {
                registerSystems(system);
            }
        }
        private void registerController(TECController controller)
        {
            controller.PropertyChanged += Object_PropertyChanged;
            foreach(TECConnection connection in controller.ChildrenConnections)
            {
                connection.PropertyChanged += Object_PropertyChanged;
            }
            foreach(TECIO io in controller.IO)
            {
                io.PropertyChanged += Object_PropertyChanged;
            }
        }
        
        private void handleAdd(StackItem item)
        {
            try
            {
                var parentCollection = UtilitiesMethods.GetChildCollection(item.TargetType, item.ReferenceObject);
                ((IList)parentCollection).Remove(item.TargetObject);
            }
            catch
            {
                string message = "Target object: " + item.ReferenceObject + " and reference object " + item.TargetObject + " not handled in add";
                DebugHandler.LogDebugMessage(message, DEBUG_STACK);
            }
        }
        private void handleRemove(StackItem item)
        {
            try
            {
                var parentCollection = UtilitiesMethods.GetChildCollection(item.TargetType, item.ReferenceObject);
                ((IList)parentCollection).Add(item.TargetObject);
            }
            catch
            {
                string message = "Target object: " + item.ReferenceObject + " and reference object " + item.TargetObject + " not handled in remove";
                DebugHandler.LogDebugMessage(message, DEBUG_STACK);
            }
        }
        private void handleEdit(StackItem item)
        {
            var newItem = item.TargetObject;
            var oldItem = item.ReferenceObject;
            var properties = newItem.GetType().GetProperties();
            
            foreach (var property in properties)
            {
                if(property.GetSetMethod() != null)
                { property.SetValue(newItem, property.GetValue(oldItem)); }
                else
                {
                    string message = "Property could not be set: " + property.Name;
                    DebugHandler.LogDebugMessage(message, DEBUG_PROPERTIES);
                }
            }
        }

        private object copy(object obj)
        {
            var type = obj.GetType();
            ConstructorInfo ctor = type.GetConstructor(Type.EmptyTypes);
            var outObj = ctor.Invoke(new object[] { });

            var properties = type.GetRuntimeProperties();
            foreach (var property in properties)
            {
                if (property.GetSetMethod() != null)
                {
                    property.SetValue(outObj, property.GetValue(obj));
                }
                else
                {
                    string message = "Property could not be set: " + property.Name;
                    DebugHandler.LogDebugMessage(message, DEBUG_PROPERTIES);
                }
            }
            return outObj;
        }
        
        private void handleChildren(StackItem item)
        {
            var newItem = item.TargetObject;
            
            if (newItem is TECSystem)
            {
                handleSystemChildren(newItem as TECSystem, item.Change);
            } else if (newItem is TECEquipment)
            {
                handleEquipmentChildren(newItem as TECEquipment, item.Change);
            } else if (newItem is TECSubScope)
            {
                handleSubScopeChildren(newItem as TECSubScope, item.Change);
            }
            else if (newItem is TECController && (item.ReferenceObject is TECBid 
                || item.ReferenceObject is TECTemplates 
                || item.ReferenceObject is TECControlledScope))
            {
                handleControllerChildren(newItem as TECController, item.Change);
            }
            else if (newItem is TECDevice && (item.ReferenceObject is TECBid || item.ReferenceObject is TECTemplates))
            {
                handleDeviceChildren(newItem as TECDevice, item.Change);
            }
            else if (newItem is TECConduitType || newItem is TECConnectionType)
            {
                handleScopeChildren(newItem as TECScope, item.Change);
            }
            else if (newItem is TECConnection || newItem is TECSubScopeConnection || newItem is TECNetworkConnection )
            {
                handleConnectionChildren(newItem as TECConnection, item.Change);
            }
            else if (newItem is TECControlledScope)
            {
                handleControlledScope(newItem as TECControlledScope, item.Change);
            }
            else if (newItem is TECPanel)
            {
                handlePanelChildren(newItem as TECPanel, item.Change);
            }
            else if (newItem is TECDrawing)
            {
                foreach (TECPage page in ((TECDrawing)newItem).Pages)
                {
                    SaveStack.Add(new StackItem(Change.Add, newItem, page));
                    page.PropertyChanged += Object_PropertyChanged;
                }
            } else if (newItem is TECIOModule)
            {
                handleIOModuelChildren(newItem as TECIOModule, item.Change);
            }
        }
        private void handleSystemChildren(TECSystem system, Change change)
        {
            handleScopeChildren(system as TECScope, change);
            StackItem item;
            foreach (TECEquipment newEquipment in system.Equipment)
            {
                item = new StackItem(change, (object)system, (object)newEquipment);
                SaveStack.Add(item);
                if (change == Change.Add)
                {
                    newEquipment.PropertyChanged += Object_PropertyChanged;
                }
                else if (change == Change.Remove)
                {
                    newEquipment.PropertyChanged -= Object_PropertyChanged;
                }
                else
                {
                    throw new ArgumentException("Change type not valid.");
                }

                handleEquipmentChildren(newEquipment, change);
            }
        }
        private void handleEquipmentChildren(TECEquipment equipment, Change change)
        {
            handleScopeChildren(equipment as TECScope, change);
            StackItem item;
            foreach (TECSubScope newSubScope in equipment.SubScope)
            {
                item = new StackItem(change, (object)equipment, (object)newSubScope);
                SaveStack.Add(item);
                if (change == Change.Add)
                {
                    newSubScope.PropertyChanged += Object_PropertyChanged;
                }
                else if (change == Change.Remove)
                {
                    newSubScope.PropertyChanged -= Object_PropertyChanged;
                }
                else
                {
                    throw new ArgumentException("Change type not valid.");
                }

                handleSubScopeChildren(newSubScope, change);
            }
        }
        private void handleSubScopeChildren(TECSubScope subScope, Change change)
        {
            handleScopeChildren(subScope as TECScope, change);
            StackItem item;
            foreach (TECPoint newPoint in subScope.Points)
            {
                handleScopeChildren(newPoint as TECScope, change);
                item = new StackItem(change, (object)subScope, (object)newPoint);
                SaveStack.Add(item);
                if (change == Change.Add)
                {
                    newPoint.PropertyChanged += Object_PropertyChanged;
                }
                else if (change == Change.Remove)
                {
                    newPoint.PropertyChanged -= Object_PropertyChanged;
                }
                
                else
                {
                    throw new ArgumentException("Change type not valid.");
                }
            }
            foreach (TECDevice newDevice in subScope.Devices)
            {
                item = new StackItem(change, (object)subScope, (object)newDevice);
                SaveStack.Add(item);
            }
        }
        private void handleConnectionChildren(TECConnection connection, Change change)
        {
            //Conduit Type
            if (connection.ConduitType != null)
            {
                if(change == Change.Add)
                {
                    SaveStack.Add(new StackItem(Change.AddRelationship, connection, connection.ConduitType, typeof(TECConnection), typeof(TECConduitType)));

                } else if (change == Change.Remove)
                {
                    SaveStack.Add(new StackItem(Change.RemoveRelationship, connection, connection.ConduitType, typeof(TECConnection), typeof(TECConduitType)));

                }
            }
            
            #region If Connection is NetworkConnection
            if (connection is TECNetworkConnection)
            {
                TECNetworkConnection netConnect = connection as TECNetworkConnection;

                foreach (TECController controller in netConnect.ChildrenControllers)
                {
                    if (change == Change.Add)
                    {
                        SaveStack.Add(new StackItem(Change.AddRelationship, netConnect, controller));
                    }
                    else if (change == Change.Remove)
                    {
                        SaveStack.Add(new StackItem(Change.RemoveRelationship, netConnect, controller));
                    }
                }

                if (netConnect.ConnectionType != null)
                {
                    if (change == Change.Add)
                    {
                        SaveStack.Add(new StackItem(Change.AddRelationship, netConnect, netConnect.ConnectionType));
                    }
                    else if (change == Change.Remove)
                    {
                        SaveStack.Add(new StackItem(Change.RemoveRelationship, netConnect, netConnect.ConnectionType));
                    }
                }
            }
            #endregion

            #region If Connection is SubScopeConnection
            else if (connection is TECSubScopeConnection)
            {
                TECSubScopeConnection ssConnect = connection as TECSubScopeConnection;
                if (change == Change.Add)
                {
                    SaveStack.Add(new StackItem(Change.AddRelationship, ssConnect, ssConnect.SubScope));
                }
                else if (change == Change.Remove)
                {
                    SaveStack.Add(new StackItem(Change.RemoveRelationship, ssConnect, ssConnect.SubScope));
                }
            }
            #endregion

            else
            {
                throw new NotImplementedException();
            }
        }
        private void handleDeviceChildren(TECDevice device, Change change)
        {
            handleScopeChildren(device as TECScope, change);
            StackItem item;
            item = new StackItem(change, (object)device, (object)device.Manufacturer);
            SaveStack.Add(item);
            item = new StackItem(change, (object)device, (object)device.ConnectionType);
            SaveStack.Add(item);
        }
        private void handleControllerChildren(TECController controller, Change change)
        {
            handleScopeChildren(controller as TECScope, change);
            StackItem item;
            if(controller.Manufacturer != null)
            {
                item = new StackItem(change, controller, controller.Manufacturer);
                SaveStack.Add(item);
            }
            foreach (TECConnection connection in controller.ChildrenConnections)
            {
                item = new StackItem(change, controller, connection, typeof(TECController), typeof(TECConnection));
                SaveStack.Add(item);
                handleConnectionChildren(connection, change);
            }
            foreach (TECIO io in controller.IO)
            {
                item = new StackItem(change, controller, io, typeof(TECController), typeof(TECIO));
                SaveStack.Add(item);
                if(io.IOModule != null)
                {
                    item = new StackItem(change, io, io.IOModule);
                    SaveStack.Add(item);
                }
                
            }
        }
        private void handleScopeChildren(TECScope scope, Change change)
        {
            StackItem item;
            foreach(TECAssociatedCost cost in scope.AssociatedCosts)
            {
                item = new StackItem(change, (object)scope, (object)cost, typeof(TECScope), typeof(TECAssociatedCost));
                SaveStack.Add(item);
            }
            foreach(TECTag tag in scope.Tags)
            {
                item = new StackItem(change, (object)scope, (object)tag, typeof(TECScope), typeof(TECTag));
                SaveStack.Add(item);
            }
        }
        private void handleControlledScope(TECControlledScope scope, Change change)
        {
            StackItem item;
            foreach (TECSystem system in scope.Systems)
            {
                handleScopeChildren(system as TECScope, change);
                system.PropertyChanged += Object_PropertyChanged;
                item = new StackItem(change, (object)scope, (object)system);
                SaveStack.Add(item);
                handleSystemChildren(system, change);
            }
            foreach (TECController controller in scope.Controllers)
            {
                handleScopeChildren(controller as TECScope, change);
                controller.PropertyChanged += Object_PropertyChanged;
                item = new StackItem(change, (object)scope, (object)controller);
                SaveStack.Add(item);
                handleControllerChildren(controller, change);
            }
            foreach(TECPanel panel in scope.Panels)
            {
                handleScopeChildren(panel as TECScope, change);
                panel.PropertyChanged += Object_PropertyChanged;
                item = new StackItem(change, (object)scope, (object)panel);
                SaveStack.Add(item);
            }
        }
        private void handlePanelChildren(TECPanel panel, Change change)
        {
            handleScopeChildren(panel as TECScope, change);
            StackItem item;
            item = new StackItem(change, panel, panel.Type);
            SaveStack.Add(item);
            foreach (TECController controller in panel.Controllers)
            {
                if (change == Change.Add)
                {
                    item = new StackItem(Change.AddRelationship, (object)controller, (object)panel);
                    SaveStack.Add(item);
                }
                else if (change == Change.Remove)
                {
                    item = new StackItem(Change.RemoveRelationship, (object)controller, (object)panel);
                    SaveStack.Add(item);
                }
            }
        }
        private void handleIOModuelChildren(TECIOModule ioModule, Change change)
        {
            handleScopeChildren(ioModule as TECScope, change);
            StackItem item;
            if(ioModule.Manufacturer != null)
            {
                item = new StackItem(change, ioModule, ioModule.Manufacturer);
                SaveStack.Add(item);
            }
        }
        
        private void registerGeneric(TECObject obj)
        {
            obj.PropertyChanged += Object_PropertyChanged;
            var properties = obj.GetType().GetProperties();
            foreach(var property in properties)
            {
                if(property.GetSetMethod() != null)
                {
                    if (property.PropertyType is IList){
                        //foreach(object childObj in )
                    }
                }
            }

        }
        #endregion
        
    }
}
