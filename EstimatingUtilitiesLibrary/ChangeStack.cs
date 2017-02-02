using EstimatingLibrary;
using System;
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
    public enum Change {Add, Remove, Edit};

    public class ChangeStack
    {
        //List of change, target object, reference object
        //Example: Add, Bid, System
        //Example: Edit, New Object, Old Object
        public List<Tuple<Change, object, object>> UndoStack { get; set; }
        public List<Tuple<Change, object, object>> RedoStack { get; set; }
        public List<Tuple<Change, object, object>> SaveStack { get; set; }
        public TECBid Bid;
        public TECTemplates Templates;

        private const bool DEBUG_PROPERTIES = false;
        private const bool DEBUG_STACK = false;
        
        private bool isDoing = false;
        
        #region Constructors
        public ChangeStack()
        {
            UndoStack = new List<Tuple<Change, object, object>>();
            RedoStack = new List<Tuple<Change, object, object>>();
            SaveStack = new List<Tuple<Change, object, object>>();
        }
        public ChangeStack(TECBid bid)
        {
            Bid = bid;
            registerBidChanges(bid);
            UndoStack = new List<Tuple<Change, object, object>>();
            RedoStack = new List<Tuple<Change, object, object>>();
            SaveStack = new List<Tuple<Change, object, object>>();
        }
        public ChangeStack(TECTemplates templates)
        {
            Templates = templates;
            registerTemplatesChanges(templates);
            UndoStack = new List<Tuple<Change, object, object>>();
            RedoStack = new List<Tuple<Change, object, object>>();
            SaveStack = new List<Tuple<Change, object, object>>();
        }
        #endregion

        #region Methods

        private void registerBidChanges(TECBid Bid)
        {
            //Bid Changed
            Bid.PropertyChanged += Object_PropertyChanged;
            Bid.Labor.PropertyChanged += Object_PropertyChanged;
            //System Changed
            foreach (TECScopeBranch branch in Bid.ScopeTree) {
                branch.PropertyChanged += Object_PropertyChanged;
                registerScope(branch); }
            //Notes changed
            foreach (TECNote note in Bid.Notes) { note.PropertyChanged += Object_PropertyChanged; }
            //Exclusions changed
            foreach (TECExclusion exclusion in Bid.Exclusions) { exclusion.PropertyChanged += Object_PropertyChanged; }
            //Locations changed
            foreach (TECLocation location in Bid.Locations) { location.PropertyChanged += Object_PropertyChanged; }
            //Manufacturers Changed
            foreach (TECManufacturer manufacturer in Bid.ManufacturerCatalog) { manufacturer.PropertyChanged += Object_PropertyChanged; }
            foreach (TECSystem system in Bid.Systems)
            {
                system.PropertyChanged += Object_PropertyChanged;
                //Equipment Collection Changed
                foreach (TECEquipment equipment in system.Equipment)
                {
                    //equipment Changed
                    equipment.PropertyChanged += Object_PropertyChanged;
                    foreach (TECSubScope subScope in equipment.SubScope)
                    {
                        //Subscope Changed
                        subScope.PropertyChanged += Object_PropertyChanged;
                        foreach (TECDevice device in subScope.Devices)
                        {
                            //Device Changed
                            device.PropertyChanged += Object_PropertyChanged;
                        }
                        foreach (TECPoint point in subScope.Points)
                        {
                            //Point Changed
                            point.PropertyChanged += Object_PropertyChanged;
                        }
                    }
                }
            }
            //Bid.Drawings.CollectionChanged += Bid_CollectionChanged;
            foreach (TECDrawing drawing in Bid.Drawings)
            {
                drawing.PropertyChanged += Object_PropertyChanged;
                foreach (TECPage page in drawing.Pages)
                {
                    page.PropertyChanged += Object_PropertyChanged;
                    foreach (TECVisualScope vs in page.PageScope)
                    {
                        vs.PropertyChanged += Object_PropertyChanged;
                    }
                }
            }
            foreach (TECController controller in Bid.Controllers)
            {
                controller.PropertyChanged += Object_PropertyChanged;
            }
        }

        private void registerTemplatesChanges(TECTemplates Templates)
        {
            //Template Changed
            Templates.PropertyChanged += Object_PropertyChanged;
            foreach (TECSystem system in Templates.SystemTemplates)
            {
                registerSystems(system);
            }
            foreach(TECEquipment equipment in Templates.EquipmentTemplates)
            {
                registerEquipment(equipment);
            }
            foreach(TECSubScope subScope in Templates.SubScopeTemplates)
            {
                registerSubScope(subScope);
            }
            foreach(TECDevice device in Templates.DeviceCatalog)
            {
                //Device Changed
                device.PropertyChanged += Object_PropertyChanged;
            }
            foreach(TECTag tag in Templates.Tags)
            {
                tag.PropertyChanged += Object_PropertyChanged;
            }
            foreach(TECManufacturer manufacturer in Templates.ManufacturerCatalog)
            {
                manufacturer.PropertyChanged += Object_PropertyChanged;
            }
            foreach(TECController controller in Templates.ControllerTemplates)
            {
                controller.PropertyChanged += Object_PropertyChanged;
            }
        }

        private void registerSubScope(TECSubScope subScope)
        {
            //Subscope Changed
            subScope.PropertyChanged += Object_PropertyChanged;
            foreach (TECDevice device in subScope.Devices)
            {
                //Device Changed
                device.PropertyChanged += Object_PropertyChanged;
            }
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
            foreach(TECScopeBranch scope in branch.Branches)
            {
                scope.PropertyChanged += Object_PropertyChanged;
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

        public void Undo()
        {
            isDoing = true;
            Tuple<Change, object, object> StackItem = UndoStack.Last();
            if (DEBUG_STACK) { Console.WriteLine("Undoing:       " + StackItem.Item1.ToString() + "    #Undo: " + UndoStack.Count() + "    #Redo: " + RedoStack.Count()); }
            if (StackItem.Item1 == Change.Add)
            {
                handleAdd(StackItem);
                UndoStack.Remove(StackItem);
                UndoStack.Remove(UndoStack.Last());
                RedoStack.Add(StackItem);
            } else if(StackItem.Item1 == Change.Remove)
            {
                handleRemove(StackItem);
                UndoStack.Remove(StackItem);
                UndoStack.Remove(UndoStack.Last());
                RedoStack.Add(StackItem);
            }
            else if (StackItem.Item1 == Change.Edit)
            {
                int index = UndoStack.IndexOf(StackItem);
                RedoStack.Add(Tuple.Create<Change, object, object>(Change.Edit, copy(StackItem.Item3), StackItem.Item3));
                handleEdit(StackItem);
                for (int x = (UndoStack.Count - 1); x >= index; x--)
                {
                    UndoStack.RemoveAt(x);
                }
            }
            if (DEBUG_STACK) { Console.WriteLine("After Undoing: " + StackItem.Item1.ToString() + "    #Undo: " + UndoStack.Count() + "    #Redo: " + RedoStack.Count() + "\n"); }
            isDoing = false;
        }

        public void Redo()
        {
            isDoing = true;
            Tuple<Change, object, object> StackItem = RedoStack.Last();
            if (DEBUG_STACK) { Console.WriteLine("Redoing:       " + StackItem.Item1.ToString() + "    #Undo: " + UndoStack.Count() + "    #Redo: " + RedoStack.Count()); }
            if (StackItem.Item1 == Change.Add)
            {
                handleRemove(StackItem);
                RedoStack.Remove(StackItem);
            }
            else if (StackItem.Item1 == Change.Remove)
            {
                handleAdd(StackItem);
                RedoStack.Remove(StackItem);
            }
            else if (StackItem.Item1 == Change.Edit)
            {
                int index = 0;
                if (UndoStack.Count > 0)
                {
                    index = UndoStack.IndexOf(UndoStack.Last());
                } 
                
                handleEdit(StackItem);
                RedoStack.Remove(StackItem);
                for (int x = (UndoStack.Count - 2); x > index; x--)
                {
                    UndoStack.RemoveAt(x);
                }
            }
            if (DEBUG_STACK) { Console.WriteLine("After Redoing: " + StackItem.Item1.ToString() + "    #Undo: " + UndoStack.Count() + "    #Redo: " + RedoStack.Count() + "\n"); }
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
        
        private void handleAdd(Tuple<Change, object, object> StackItem)
        {
            if (StackItem.Item2 is TECBid)
            {
                if(StackItem.Item3 is TECScopeBranch)
                {
                    ((TECBid)StackItem.Item2).ScopeTree.Remove((TECScopeBranch)StackItem.Item3);
                } else if (StackItem.Item3 is TECSystem)
                {
                    ((TECBid)StackItem.Item2).Systems.Remove((TECSystem)StackItem.Item3);
                } else if (StackItem.Item3 is TECNote)
                {
                    ((TECBid)StackItem.Item2).Notes.Remove((TECNote)StackItem.Item3);
                }
                else if (StackItem.Item3 is TECExclusion)
                {
                    ((TECBid)StackItem.Item2).Exclusions.Remove((TECExclusion)StackItem.Item3);
                }
                else if (StackItem.Item3 is TECDrawing)
                {
                    ((TECBid)StackItem.Item2).Drawings.Remove((TECDrawing)StackItem.Item3);
                }
                else if (StackItem.Item3 is TECManufacturer)
                {
                    ((TECBid)StackItem.Item2).ManufacturerCatalog.Remove((TECManufacturer)StackItem.Item3);
                }
                else if (StackItem.Item3 is TECDevice)
                {
                    ((TECBid)StackItem.Item2).DeviceCatalog.Remove((TECDevice)StackItem.Item3);
                }
                else if (StackItem.Item3 is TECTag)
                {
                    ((TECBid)StackItem.Item2).Tags.Remove((TECTag)StackItem.Item3);
                }
                else if (StackItem.Item3 is TECLocation)
                {
                    ((TECBid)StackItem.Item2).Locations.Remove((TECLocation)StackItem.Item3);
                }
                else if (StackItem.Item3 is TECController)
                {
                    ((TECBid)StackItem.Item2).Controllers.Remove((TECController)StackItem.Item3);
                }
            }
            else if (StackItem.Item2 is TECSystem)
            {
                ((TECSystem)StackItem.Item2).Equipment.Remove((TECEquipment)StackItem.Item3);
            }
            else if (StackItem.Item2 is TECEquipment)
            {
                ((TECEquipment)StackItem.Item2).SubScope.Remove((TECSubScope)StackItem.Item3);
            }
            else if (StackItem.Item2 is TECSubScope)
            {
                if (StackItem.Item3 is TECDevice)
                {
                    ((TECSubScope)StackItem.Item2).Devices.Remove((TECDevice)StackItem.Item3);
                }
                else if (StackItem.Item3 is TECPoint)
                {
                    ((TECSubScope)StackItem.Item2).Points.Remove((TECPoint)StackItem.Item3);
                }
            }
            else if (StackItem.Item2 is TECPage)
            {
                ((TECPage)StackItem.Item2).PageScope.Remove((TECVisualScope)StackItem.Item3);
            }
            else if (StackItem.Item3 is TECDrawing)
            {
                ((TECDrawing)StackItem.Item2).Pages.Remove((TECPage)StackItem.Item3);
            }

            else if(StackItem.Item2 is TECTemplates)
            {
                if(StackItem.Item3 is TECSystem)
                {
                    ((TECTemplates)StackItem.Item2).SystemTemplates.Remove((TECSystem)StackItem.Item3);
                }
                else if (StackItem.Item3 is TECEquipment)
                {
                    ((TECTemplates)StackItem.Item2).EquipmentTemplates.Remove((TECEquipment)StackItem.Item3);
                }
                else if (StackItem.Item3 is TECSubScope)
                {
                    ((TECTemplates)StackItem.Item2).SubScopeTemplates.Remove((TECSubScope)StackItem.Item3);
                }
                else if (StackItem.Item3 is TECDevice)
                {
                    ((TECTemplates)StackItem.Item2).DeviceCatalog.Remove((TECDevice)StackItem.Item3);
                }
                else if (StackItem.Item3 is TECTag)
                {
                    ((TECTemplates)StackItem.Item2).Tags.Remove((TECTag)StackItem.Item3);
                }
                else if (StackItem.Item3 is TECManufacturer)
                {
                    ((TECTemplates)StackItem.Item2).ManufacturerCatalog.Remove((TECManufacturer)StackItem.Item3);
                }
                else if (StackItem.Item3 is TECController)
                {
                    ((TECTemplates)StackItem.Item2).ControllerTemplates.Remove((TECController)StackItem.Item3);
                }
            }
        }

        private void handleRemove(Tuple<Change, object, object> StackItem)
        {
            if (StackItem.Item2 is TECBid)
            {
                if (StackItem.Item3 is TECScopeBranch)
                {
                    ((TECBid)StackItem.Item2).ScopeTree.Add((TECScopeBranch)StackItem.Item3);
                }
                else if (StackItem.Item3 is TECSystem)
                {
                    ((TECBid)StackItem.Item2).Systems.Add((TECSystem)StackItem.Item3);
                }
                else if (StackItem.Item3 is TECNote)
                {
                    ((TECBid)StackItem.Item2).Notes.Add((TECNote)StackItem.Item3);
                }
                else if (StackItem.Item3 is TECExclusion)
                {
                    ((TECBid)StackItem.Item2).Exclusions.Add((TECExclusion)StackItem.Item3);
                }
                else if (StackItem.Item3 is TECDrawing)
                {
                    ((TECBid)StackItem.Item2).Drawings.Add((TECDrawing)StackItem.Item3);
                }
                else if (StackItem.Item3 is TECManufacturer)
                {
                    ((TECBid)StackItem.Item2).ManufacturerCatalog.Add((TECManufacturer)StackItem.Item3);
                }
                else if (StackItem.Item3 is TECDevice)
                {
                    ((TECBid)StackItem.Item2).DeviceCatalog.Add((TECDevice)StackItem.Item3);
                }
                else if (StackItem.Item3 is TECTag)
                {
                    ((TECBid)StackItem.Item2).Tags.Add((TECTag)StackItem.Item3);
                }
                else if (StackItem.Item3 is TECLocation)
                {
                    ((TECBid)StackItem.Item2).Locations.Add((TECLocation)StackItem.Item3);
                }
                else if (StackItem.Item3 is TECController)
                {
                    ((TECBid)StackItem.Item2).Controllers.Add((TECController)StackItem.Item3);
                }
            }
            else if (StackItem.Item2 is TECSystem)
            {
                ((TECSystem)StackItem.Item2).Equipment.Add((TECEquipment)StackItem.Item3);
            }
            else if (StackItem.Item2 is TECEquipment)
            {
                ((TECEquipment)StackItem.Item2).SubScope.Add((TECSubScope)StackItem.Item3);
            }
            else if (StackItem.Item2 is TECSubScope)
            {
                if (StackItem.Item3 is TECDevice)
                {
                    ((TECSubScope)StackItem.Item2).Devices.Add((TECDevice)StackItem.Item3);
                }
                else if (StackItem.Item3 is TECPoint)
                {
                    ((TECSubScope)StackItem.Item2).Points.Add((TECPoint)StackItem.Item3);
                }
            }
            else if (StackItem.Item2 is TECPage)
            {
                ((TECPage)StackItem.Item2).PageScope.Add((TECVisualScope)StackItem.Item3);
            }
            else if (StackItem.Item3 is TECDrawing)
            {
                ((TECDrawing)StackItem.Item2).Pages.Add((TECPage)StackItem.Item3);
            }

            else if (StackItem.Item2 is TECTemplates)
            {
                if (StackItem.Item3 is TECSystem)
                {
                    ((TECTemplates)StackItem.Item2).SystemTemplates.Add((TECSystem)StackItem.Item3);
                }
                else if (StackItem.Item3 is TECEquipment)
                {
                    ((TECTemplates)StackItem.Item2).EquipmentTemplates.Add((TECEquipment)StackItem.Item3);
                }
                else if (StackItem.Item3 is TECSubScope)
                {
                    ((TECTemplates)StackItem.Item2).SubScopeTemplates.Add((TECSubScope)StackItem.Item3);
                }
                else if (StackItem.Item3 is TECDevice)
                {
                    ((TECTemplates)StackItem.Item2).DeviceCatalog.Add((TECDevice)StackItem.Item3);
                }
                else if (StackItem.Item3 is TECTag)
                {
                    ((TECTemplates)StackItem.Item2).Tags.Add((TECTag)StackItem.Item3);
                }
                else if (StackItem.Item3 is TECManufacturer)
                {
                    ((TECTemplates)StackItem.Item2).ManufacturerCatalog.Add((TECManufacturer)StackItem.Item3);
                }
                else if (StackItem.Item3 is TECController)
                {
                    ((TECTemplates)StackItem.Item2).ControllerTemplates.Add((TECController)StackItem.Item3);
                }
            }
        }

        private void handleEdit(Tuple<Change, object, object> StackItem)
        {
            var newItem = StackItem.Item3;
            var oldItem = StackItem.Item2;
            var properties = newItem.GetType().GetProperties();
            
            foreach (var property in properties)
            {
                if(property.GetSetMethod() != null)
                {
                    property.SetValue(newItem, property.GetValue(oldItem));
                }
                else
                {
                    if (DEBUG_PROPERTIES) { Console.WriteLine("Property could not be set: " + property.Name); }
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
                    if (DEBUG_PROPERTIES) { Console.WriteLine("Property could not be set: " + property.Name); }
                }
            }
            return outObj;
        }

        private void handlePropertyChanged(PropertyChangedEventArgs e)
        {
            if (DEBUG_PROPERTIES) { Console.WriteLine("Propertychanged: " + e.PropertyName); }
            if (!isDoing){ RedoStack.Clear(); }
            if (e is PropertyChangedExtendedEventArgs<Object>)
            {
                PropertyChangedExtendedEventArgs<Object> args = e as PropertyChangedExtendedEventArgs<Object>;
                Tuple<Change, Object, Object> item;
                object oldValue = args.OldValue;
                object newValue = args.NewValue;
                if (e.PropertyName == "Add")
                {
                    if (DEBUG_PROPERTIES) { Console.WriteLine("Add change: " + oldValue); }
                    item = Tuple.Create<Change, Object, Object>(Change.Add, oldValue, newValue);
                    ((TECObject)newValue).PropertyChanged += Object_PropertyChanged;
                    handleChildren(item);
                    UndoStack.Add(item);
                    SaveStack.Add(item);
                    if (DEBUG_STACK) { Console.WriteLine("Undo count: " + UndoStack.Count + " Save Count: " + SaveStack.Count); }
                }
                else if (e.PropertyName == "Remove")
                {
                    if (DEBUG_PROPERTIES) { Console.WriteLine("Remove change: " + oldValue); }
                    item = Tuple.Create<Change, Object, Object>(Change.Remove, oldValue, newValue);
                    ((TECObject)newValue).PropertyChanged -= Object_PropertyChanged;
                    handleChildren(item);
                    UndoStack.Add(item);
                    SaveStack.Add(item);
                    if (DEBUG_STACK) { Console.WriteLine("Undo count: " + UndoStack.Count + " Save Count: " + SaveStack.Count); }
                }
                else if (e.PropertyName == "Edit")
                {
                    if (DEBUG_PROPERTIES) { Console.WriteLine("Edit change: " + oldValue); }
                    item = Tuple.Create<Change, Object, Object>(Change.Edit, oldValue, newValue);
                    SaveStack.Add(item);
                    if (DEBUG_STACK) { Console.WriteLine("Undo count: " + UndoStack.Count + " Save Count: " + SaveStack.Count); }
                }
                else if (e.PropertyName == "ChildChanged")
                {
                    if (DEBUG_PROPERTIES) { Console.WriteLine("Child change: " + oldValue); }
                    item = Tuple.Create<Change, Object, Object>(Change.Edit, oldValue, newValue);
                    SaveStack.Add(item);
                    if (DEBUG_STACK) { Console.WriteLine("Undo count: " + UndoStack.Count + " Save Count: " + SaveStack.Count); }
                }
                else if (e.PropertyName == "LocationChanged")
                {
                    if (DEBUG_PROPERTIES) { Console.WriteLine("Location change: " + oldValue); }
                    var oldNew = newValue as Tuple<Object, Object>;
                    var toSave = new List<Tuple<Change, object, object>>();
                    if (oldNew.Item1 != null)
                    {
                        toSave.Add(Tuple.Create<Change, Object, Object>(Change.Remove, oldValue, oldNew.Item1));
                    }
                    if (oldNew.Item2 != null)
                    {
                        toSave.Add(Tuple.Create<Change, Object, Object>(Change.Add, oldValue, oldNew.Item2));
                    }
                    foreach(var save in toSave)
                    {
                        SaveStack.Add(save);
                    }
                    if (DEBUG_STACK) { Console.WriteLine("Undo count: " + UndoStack.Count + " Save Count: " + SaveStack.Count); }
                }
                else if(e.PropertyName == "MetaAdd")
                {
                    if (DEBUG_PROPERTIES) { Console.WriteLine("MetaAdd change: " + oldValue); }
                    item = Tuple.Create<Change, Object, Object>(Change.Add, oldValue, newValue);
                    ((TECObject)newValue).PropertyChanged += Object_PropertyChanged;
                    SaveStack.Add(item);
                    if (DEBUG_STACK) { Console.WriteLine("Undo count: " + UndoStack.Count + " Save Count: " + SaveStack.Count); }
                }
                else if (e.PropertyName == "MetaRemove")
                {
                    if (DEBUG_PROPERTIES) { Console.WriteLine("MetaRemove change: " + oldValue); }
                    item = Tuple.Create<Change, Object, Object>(Change.Remove, oldValue, newValue);
                    ((TECObject)newValue).PropertyChanged -= Object_PropertyChanged;
                    SaveStack.Add(item);
                    if (DEBUG_STACK) { Console.WriteLine("Undo count: " + UndoStack.Count + " Save Count: " + SaveStack.Count); }
                }
                else
                {
                    if (DEBUG_PROPERTIES) { Console.WriteLine("Edit change: " + oldValue); }
                    item = Tuple.Create<Change, Object, Object>(Change.Edit, oldValue, newValue);
                    UndoStack.Add(item);
                    SaveStack.Add(item);
                    if (DEBUG_STACK) { Console.WriteLine("Undo count: " + UndoStack.Count + " Save Count: " + SaveStack.Count); }
                }
                
            }
            else
            {
                if(DEBUG_PROPERTIES) { Console.WriteLine("Property not compatible: " + e.PropertyName); }
                if (DEBUG_STACK) { Console.WriteLine("Undo count: " + UndoStack.Count + " Save Count: " + SaveStack.Count); }
            }
        }
        
        private void handleChildren(Tuple<Change, object, object> stackItem)
        {
            var newItem = stackItem.Item3;
            
            if (newItem is TECSystem)
            {
                handleSystemChildren(newItem as TECSystem, stackItem.Item1);
            } else if (newItem is TECEquipment)
            {
                handleEquipmentChildren(newItem as TECEquipment, stackItem.Item1);
            } else if (newItem is TECSubScope)
            {
                handleSubScopeChildren(newItem as TECSubScope, stackItem.Item1);
            }

            else if (newItem is TECDrawing)
            {
                foreach (TECPage page in ((TECDrawing)newItem).Pages)
                {
                    //Console.WriteLine("Page added in handle children");
                    SaveStack.Add(new Tuple<Change, object, object>(Change.Add, newItem, page));
                    page.PropertyChanged += Object_PropertyChanged;
                }
            }
        }

        private void handleSystemChildren(TECSystem item, Change change)
        {
            Tuple<Change, object, object> stackItem;
            foreach (TECEquipment newEquipment in item.Equipment)
            {
                stackItem = Tuple.Create(change, (object)item, (object)newEquipment);
                SaveStack.Add(stackItem);
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

        private void handleEquipmentChildren(TECEquipment item, Change change)
        {
            Tuple<Change, object, object> stackItem;
            foreach (TECSubScope newSubScope in item.SubScope)
            {
                stackItem = Tuple.Create(change, (object)item, (object)newSubScope);
                SaveStack.Add(stackItem);
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

        private void handleSubScopeChildren(TECSubScope item, Change change)
        {
            Tuple<Change, object, object> stackItem;
            foreach (TECPoint newPoint in item.Points)
            {
                stackItem = Tuple.Create(change, (object)item, (object)newPoint);
                SaveStack.Add(stackItem);
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
            foreach (TECDevice newDevice in item.Devices)
            {
                stackItem = Tuple.Create(change, (object)item, (object)newDevice);
                SaveStack.Add(stackItem);
                if (change == Change.Add)
                {
                    newDevice.PropertyChanged += Object_PropertyChanged;
                }
                else if (change == Change.Remove)
                {
                    newDevice.PropertyChanged -= Object_PropertyChanged;
                } 
                
                else
                {
                    throw new ArgumentException("Change type not valid.");
                }
            }
        }

        #endregion

        #region Event Handlers
        private void Object_PropertyChanged(object sender, PropertyChangedEventArgs e) { handlePropertyChanged(e); }
        
        #endregion //Event Handlers
    }
}
