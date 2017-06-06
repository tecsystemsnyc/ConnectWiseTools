using DebugLibrary;
using EstimatingLibrary;
using EstimatingLibrary.Utilities;
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
    public enum Change { Add, Remove, Edit, AddRelationship, RemoveRelationship };
    public class ChangeStack
    {
        //List of change, target object, reference object
        //Example: Add, Bid, System
        //Example: Edit, New Object, Old Object
        public ObservableCollection<StackItem> UndoStack { get; set; }
        public List<StackItem> RedoStack { get; set; }
        public ObservableCollection<StackItem> SaveStack { get; set; }
        public TECBid Bid;
        public TECTemplates Templates;

        private bool isDoing = false;
        private ChangeWatcher watcher;

        #region Constructors
        public ChangeStack()
        {
            UndoStack = new ObservableCollection<StackItem>();
            RedoStack = new List<StackItem>();
            SaveStack = new ObservableCollection<StackItem>();
            SaveStack.CollectionChanged += SaveStack_CollectionChanged;
            UndoStack.CollectionChanged += UndoStack_CollectionChanged;
        }
        
        public ChangeStack(TECScopeManager scopeManager) : this()
        {
            watcher = new ChangeWatcher(scopeManager);
            watcher.Changed += Object_PropertyChanged;
            if (scopeManager is TECBid)
            {
                Bid = scopeManager as TECBid;
            }
            else if (scopeManager is TECTemplates)
            {
                Templates = scopeManager as TECTemplates;
            }
            else
            {
                throw new NotImplementedException();
            }

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
        private void UndoStack_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
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
            DebugHandler.LogDebugMessage(message, DebugBooleans.Properties);

            if (!isDoing) { RedoStack.Clear(); }
            if (e is PropertyChangedExtendedEventArgs<Object>)
            {
                PropertyChangedExtendedEventArgs<Object> args = e as PropertyChangedExtendedEventArgs<Object>;
                StackItem item;
                object oldValue = args.OldValue;
                object newValue = args.NewValue;
                if (e.PropertyName == "Add" || e.PropertyName == "Remove")
                {
                    message = e.PropertyName + " change: " + oldValue;
                    DebugHandler.LogDebugMessage(message, DebugBooleans.Properties);
                    Change change = Change.Add;
                    if(e.PropertyName == "Remove")
                    {
                        change = Change.Remove;
                    }

                    item = new StackItem(change, args);
                    handleChildren(item);
                    UndoStack.Add(item);
                    SaveStack.Add(item);

                    message = "Undo count: " + UndoStack.Count + " Save Count: " + SaveStack.Count;
                    DebugHandler.LogDebugMessage(message, DebugBooleans.Stack);
                }
                else if (e.PropertyName == "ObjectPropertyChanged" || e.PropertyName == "RelationshipPropertyChanged")
                {
                    message = "Object changed: " + oldValue;
                    DebugHandler.LogDebugMessage(message, DebugBooleans.Properties);
                    Change addChange = Change.Add;
                    Change removeChange = Change.Remove;
                    if (e.PropertyName == "RelationshipPropertyChanged")
                    {
                        addChange = Change.AddRelationship;
                        removeChange = Change.RemoveRelationship;
                    }

                    var oldNew = newValue as Tuple<Object, Object>;
                    var toSave = new List<StackItem>();
                    if (oldNew.Item1 != null)
                    {
                        toSave.Add(new StackItem(removeChange, oldValue, oldNew.Item1, args.OldType, args.NewType));
                    }
                    if (oldNew.Item2 != null)
                    {
                        toSave.Add(new StackItem(addChange, oldValue, oldNew.Item2, args.OldType, args.NewType));
                    }
                    foreach (var save in toSave)
                    {
                        SaveStack.Add(save);
                    }

                    message = "Undo count: " + UndoStack.Count + " Save Count: " + SaveStack.Count;
                    DebugHandler.LogDebugMessage(message, DebugBooleans.Stack);
                }
                else if (e.PropertyName == "MetaAdd" || e.PropertyName == "MetaRemove")
                {
                    message = e.PropertyName + " change: " + oldValue;
                    DebugHandler.LogDebugMessage(message, DebugBooleans.Properties);
                    Change change = Change.Add;
                    if (e.PropertyName == "MetaRemove")
                    {
                        change = Change.Remove;
                    }

                    item = new StackItem(change, args);
                    SaveStack.Add(item);

                    message = "Undo count: " + UndoStack.Count + " Save Count: " + SaveStack.Count;
                    DebugHandler.LogDebugMessage(message, DebugBooleans.Stack);
                }
                else if (e.PropertyName == "AddRelationship" || e.PropertyName == "RemoveRelationship")
                {
                    message = e.PropertyName + " change: " + oldValue;
                    DebugHandler.LogDebugMessage(message, DebugBooleans.Properties);
                    Change change = Change.AddRelationship;
                    if (e.PropertyName == "RemoveRelationship")
                    {
                        change = Change.RemoveRelationship;
                    }

                    item = new StackItem(change, args);
                    UndoStack.Add(item);
                    SaveStack.Add(item);

                    message = "Undo count: " + UndoStack.Count + " Save Count: " + SaveStack.Count;
                    DebugHandler.LogDebugMessage(message, DebugBooleans.Stack);
                }
                else if (e.PropertyName == "AddCatalog" || e.PropertyName == "RemoveCatalog")
                {
                    message = e.PropertyName + " change: " + oldValue;
                    DebugHandler.LogDebugMessage(message, DebugBooleans.Properties);
                    Change change = Change.Add;
                    if (e.PropertyName == "RemoveCatalog")
                    {
                        change = Change.RemoveRelationship;
                    }
                    item = new StackItem(change, args);
                    UndoStack.Add(item);
                    SaveStack.Add(item);

                    message = "Undo count: " + UndoStack.Count + " Save Count: " + SaveStack.Count;
                    DebugHandler.LogDebugMessage(message, DebugBooleans.Stack);
                }
                else if (e.PropertyName == "Edit" || e.PropertyName == "ChildChanged")
                {
                    message = e.PropertyName + " change: " + oldValue;
                    DebugHandler.LogDebugMessage(message, DebugBooleans.Properties);

                    item = new StackItem(Change.Edit, args);
                    SaveStack.Add(item);

                    message = "Undo count: " + UndoStack.Count + " Save Count: " + SaveStack.Count;
                    DebugHandler.LogDebugMessage(message, DebugBooleans.Stack);
                }
                
                else if (e.PropertyName == "RemovedSubScope") { }
                else if (e.PropertyName == "Catalogs")
                {
                    message = "Catalog change: " + oldValue;
                    DebugHandler.LogDebugMessage(message, DebugBooleans.Properties);

                    item = new StackItem(Change.Add, args);
                    handleChildren(item);

                    message = "Undo count: " + UndoStack.Count + " Save Count: " + SaveStack.Count;
                    DebugHandler.LogDebugMessage(message, DebugBooleans.Stack);
                }
                else
                {
                    message = "Edit change: " + oldValue;
                    DebugHandler.LogDebugMessage(message, DebugBooleans.Properties);

                    item = new StackItem(Change.Edit, args);
                    UndoStack.Add(item);
                    SaveStack.Add(item);

                    message = "Undo count: " + UndoStack.Count + " Save Count: " + SaveStack.Count;
                    DebugHandler.LogDebugMessage(message, DebugBooleans.Stack);
                }

            }
            else
            {
                message = "Property not compatible: " + e.PropertyName;
                DebugHandler.LogDebugMessage(message, DebugBooleans.Properties);
                message = "Undo count: " + UndoStack.Count + " Save Count: " + SaveStack.Count;
                DebugHandler.LogDebugMessage(message, DebugBooleans.Stack);
            }
        }
        #endregion //Event Handlers

        #region Methods

        public void Undo()
        {
            isDoing = true;
            StackItem item = UndoStack.Last();
            DebugHandler.LogDebugMessage("Undoing:       " + item.Change.ToString() + "    #Undo: " + UndoStack.Count() + "    #Redo: " + RedoStack.Count(), DebugBooleans.Stack);
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
                RedoStack.Add(new StackItem(Change.Edit, (item.TargetObject as TECObject).Copy(), item.TargetObject));
                handleEdit(item);
                for (int x = (UndoStack.Count - 1); x >= index; x--)
                {
                    UndoStack.RemoveAt(x);
                }
            }

            string message = "After Undoing: " + item.Change.ToString() + "    #Undo: " + UndoStack.Count() + "    #Redo: " + RedoStack.Count() + "\n";
            DebugHandler.LogDebugMessage(message, DebugBooleans.Stack);

            isDoing = false;
        }
        public void Redo()
        {
            isDoing = true;
            StackItem item = RedoStack.Last();

            string message = "Redoing:       " + item.Change.ToString() + "    #Undo: " + UndoStack.Count() + "    #Redo: " + RedoStack.Count();
            DebugHandler.LogDebugMessage(message, DebugBooleans.Stack);

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
            DebugHandler.LogDebugMessage(message, DebugBooleans.Stack);

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
                DebugHandler.LogDebugMessage(message, DebugBooleans.Stack);
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
                DebugHandler.LogDebugMessage(message, DebugBooleans.Stack);
            }
        }
        private void handleEdit(StackItem item)
        {
            var newItem = item.TargetObject;
            var oldItem = item.ReferenceObject;
            var properties = newItem.GetType().GetProperties();

            foreach (var property in properties)
            {
                if (property.GetSetMethod() != null)
                { property.SetValue(newItem, property.GetValue(oldItem)); }
                else
                {
                    string message = "Property could not be set: " + property.Name;
                    DebugHandler.LogDebugMessage(message, DebugBooleans.Properties);
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
                    DebugHandler.LogDebugMessage(message, DebugBooleans.Properties);
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
            }
            else if (newItem is TECEquipment)
            {
                handleEquipmentChildren(newItem as TECEquipment, item.Change);
            }
            else if (newItem is TECSubScope)
            {
                handleSubScopeChildren(newItem as TECSubScope, item.Change);
            }
            else if (newItem is TECController && (item.ReferenceObject is TECBid
                || item.ReferenceObject is TECTemplates
                || item.ReferenceObject is TECSystem))
            {
                handleControllerChildren(newItem as TECController, item.Change);
            }
            else if (newItem is TECDevice && (item.ReferenceObject is TECCatalogs))
            {
                handleDeviceChildren(newItem as TECDevice, item.Change);
            }
            else if (newItem is TECConduitType || newItem is TECConnectionType)
            {
                handleScopeChildren(newItem as TECScope, item.Change);
            }
            else if (newItem is TECConnection || newItem is TECSubScopeConnection || newItem is TECNetworkConnection)
            {
                handleConnectionChildren(newItem as TECConnection, item.Change);
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
                }
            }
            else if (newItem is TECIOModule)
            {
                handleIOModuelChildren(newItem as TECIOModule, item.Change);
            }
            else if (newItem is TECCatalogs)
            {
                handleCatalogsChildren(newItem as TECCatalogs, item.Change);
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
                if (change == Change.Add)
                {
                    SaveStack.Add(new StackItem(Change.AddRelationship, connection, connection.ConduitType, typeof(TECConnection), typeof(TECConduitType)));

                }
                else if (change == Change.Remove)
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
            foreach(TECConnectionType type in device.ConnectionTypes)
            {
                item = new StackItem(change, (object)device, (object)type);
                SaveStack.Add(item);
            }
            
        }
        private void handleControllerChildren(TECController controller, Change change)
        {
            handleScopeChildren(controller as TECScope, change);
            StackItem item;
            if (controller.Manufacturer != null)
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
                if (io.IOModule != null)
                {
                    item = new StackItem(change, io, io.IOModule);
                    SaveStack.Add(item);
                }

            }
        }
        private void handleScopeChildren(TECScope scope, Change change)
        {
            StackItem item;
            foreach (TECCost cost in scope.AssociatedCosts)
            {
                item = new StackItem(change, (object)scope, (object)cost, typeof(TECScope), typeof(TECCost));
                SaveStack.Add(item);
            }
            foreach (TECTag tag in scope.Tags)
            {
                item = new StackItem(change, (object)scope, (object)tag, typeof(TECScope), typeof(TECTag));
                SaveStack.Add(item);
            }
        }
        private void handleSystemChildren(TECSystem scope, Change change)
        {
            StackItem item;
            foreach (TECEquipment equipment in scope.Equipment)
            {
                item = new StackItem(change, (object)scope, (object)equipment);
                SaveStack.Add(item);
                handleEquipmentChildren(equipment, change);
            }
            foreach (TECController controller in scope.Controllers)
            {
                item = new StackItem(change, (object)scope, (object)controller);
                SaveStack.Add(item);
                handleControllerChildren(controller, change);
            }
            foreach (TECPanel panel in scope.Panels)
            {
                item = new StackItem(change, (object)scope, (object)panel);
                SaveStack.Add(item);
                handlePanelChildren(panel, change);
            }
            foreach(TECSystem system in scope.SystemInstances)
            {
                item = new StackItem(change, (object)scope, (object)system);
                SaveStack.Add(item);
                handleSystemChildren(system, change);
            }
            foreach(var pair in scope.CharactersticInstances.GetFullDictionary())
            {
                var subChange = Change.AddRelationship;
                if (change == Change.Remove)
                { subChange = Change.RemoveRelationship; }
                foreach (var value in pair.Value)
                {
                    item = new StackItem(subChange, pair.Key, value, typeof(TECScope), typeof(TECScope));
                    SaveStack.Add(item);
                }
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
            if (ioModule.Manufacturer != null)
            {
                item = new StackItem(change, ioModule, ioModule.Manufacturer);
                SaveStack.Add(item);
            }
        }
        private void handleCatalogsChildren(TECCatalogs catalogs, Change change)
        {
            foreach (TECDevice device in catalogs.Devices)
            {
                SaveStack.Add(new StackItem(Change.Add, device, catalogs));
                handleDeviceChildren(device, change);
            }
            foreach (TECConnectionType type in catalogs.ConnectionTypes)
            {
                SaveStack.Add(new StackItem(Change.Add, type, catalogs));
                handleScopeChildren(type, change);
            }
            foreach (TECConduitType type in catalogs.ConduitTypes)
            {
                SaveStack.Add(new StackItem(Change.Add, type, catalogs));
                handleScopeChildren(type, change);
            }
            foreach (TECCost cost in catalogs.AssociatedCosts)
            {
                SaveStack.Add(new StackItem(Change.Add, cost, catalogs));
                handleScopeChildren(cost, change);
            }
            foreach (TECPanelType type in catalogs.PanelTypes)
            {
                SaveStack.Add(new StackItem(Change.Add, type, catalogs));
                handleScopeChildren(type, change);
            }
            foreach (TECIOModule ioModule in catalogs.IOModules)
            {
                SaveStack.Add(new StackItem(Change.Add, ioModule, catalogs));
                handleIOModuelChildren(ioModule, change);
            }
            foreach (TECManufacturer manufacturer in catalogs.Manufacturers)
            {
                SaveStack.Add(new StackItem(Change.Add, manufacturer, catalogs));
            }
            foreach (TECTag tag in catalogs.Tags)
            {
                SaveStack.Add(new StackItem(Change.Add, tag, catalogs));
            }

        }

        private void registerGeneric(TECObject obj)
        {
            obj.PropertyChanged += Object_PropertyChanged;
            var properties = obj.GetType().GetProperties();
            foreach (var property in properties)
            {
                if (property.GetSetMethod() != null)
                {
                    if (property.PropertyType is IList)
                    {
                        //foreach(object childObj in )
                    }
                }
            }

        }
        #endregion

    }
}
