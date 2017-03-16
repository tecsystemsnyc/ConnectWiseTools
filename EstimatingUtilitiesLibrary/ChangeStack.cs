using DebugLibrary;
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
        private const bool DEBUG_REGISTER = false;
        
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
            { controller.PropertyChanged += Object_PropertyChanged; }
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
            foreach(TECConnection connection in Bid.Connections)
            { connection.PropertyChanged += Object_PropertyChanged; }
        }
        private void registerTemplatesChanges(TECTemplates Templates)
        {
            //Template Changed
            Templates.PropertyChanged += Object_PropertyChanged;
            foreach (TECSystem system in Templates.SystemTemplates)
            { registerSystems(system); }
            foreach(TECEquipment equipment in Templates.EquipmentTemplates)
            { registerEquipment(equipment); }
            foreach(TECSubScope subScope in Templates.SubScopeTemplates)
            {  registerSubScope(subScope); }
            foreach(TECDevice device in Templates.DeviceCatalog)
            { device.PropertyChanged += Object_PropertyChanged; }
            foreach(TECTag tag in Templates.Tags)
            { tag.PropertyChanged += Object_PropertyChanged; }
            foreach(TECManufacturer manufacturer in Templates.ManufacturerCatalog)
            { manufacturer.PropertyChanged += Object_PropertyChanged; }
            foreach(TECController controller in Templates.ControllerTemplates)
            { controller.PropertyChanged += Object_PropertyChanged; }
            foreach (TECConnectionType connectionType in Templates.ConnectionTypeCatalog)
            { connectionType.PropertyChanged += Object_PropertyChanged; }
            foreach (TECConduitType conduitType in Templates.ConduitTypeCatalog)
            { conduitType.PropertyChanged += Object_PropertyChanged; }
            foreach(TECAssociatedCost cost in Templates.AssociatedCostsCatalog)
            { cost.PropertyChanged += Object_PropertyChanged; }
            foreach (TECMiscCost addition in Templates.MiscCostTemplates)
            { addition.PropertyChanged += Object_PropertyChanged; }
            foreach (TECMiscWiring addition in Templates.MiscWiringTemplates)
            { addition.PropertyChanged += Object_PropertyChanged; }
            foreach (TECPanelType addition in Templates.PanelTypeCatalog)
            { addition.PropertyChanged += Object_PropertyChanged; }
            foreach (TECPanel panel in Templates.PanelTemplates)
            { panel.PropertyChanged += Object_PropertyChanged; }
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

        public void Undo()
        {
            isDoing = true;
            Tuple<Change, object, object> StackItem = UndoStack.Last();
            DebugHandler.LogDebugMessage("Undoing:       " + StackItem.Item1.ToString() + "    #Undo: " + UndoStack.Count() + "    #Redo: " + RedoStack.Count(), DEBUG_STACK);
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

            string message = "After Undoing: " + StackItem.Item1.ToString() + "    #Undo: " + UndoStack.Count() + "    #Redo: " + RedoStack.Count() + "\n";
            DebugHandler.LogDebugMessage(message, DEBUG_STACK);

            isDoing = false;
        }
        public void Redo()
        {
            isDoing = true;
            Tuple<Change, object, object> StackItem = RedoStack.Last();

            string message = "Redoing:       " + StackItem.Item1.ToString() + "    #Undo: " + UndoStack.Count() + "    #Redo: " + RedoStack.Count();
            DebugHandler.LogDebugMessage(message, DEBUG_STACK);


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

            message = "After Redoing: " + StackItem.Item1.ToString() + "    #Undo: " + UndoStack.Count() + "    #Redo: " + RedoStack.Count() + "\n";
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
                else if (StackItem.Item3 is TECConnectionType)
                {
                    ((TECBid)StackItem.Item2).ConnectionTypes.Remove((TECConnectionType)StackItem.Item3);
                }
                else if (StackItem.Item3 is TECConduitType)
                {
                    ((TECBid)StackItem.Item2).ConduitTypes.Remove((TECConduitType)StackItem.Item3);
                }
                else if (StackItem.Item3 is TECAssociatedCost)
                {
                    ((TECBid)StackItem.Item2).AssociatedCostsCatalog.Remove((TECAssociatedCost)StackItem.Item3);
                }
                else if (StackItem.Item3 is TECMiscCost)
                {
                    ((TECBid)StackItem.Item2).MiscCosts.Remove((TECMiscCost)StackItem.Item3);
                }
                else if (StackItem.Item3 is TECMiscWiring)
                {
                    ((TECBid)StackItem.Item2).MiscWiring.Remove((TECMiscWiring)StackItem.Item3);
                }
                else if (StackItem.Item3 is TECPanel)
                {
                    ((TECBid)StackItem.Item2).Panels.Remove((TECPanel)StackItem.Item3);
                }
                else if (StackItem.Item3 is TECPanelType)
                {
                    ((TECBid)StackItem.Item2).PanelTypeCatalog.Remove((TECPanelType)StackItem.Item3);
                }
            }
            else if (StackItem.Item2 is TECScope && StackItem.Item3 is TECAssociatedCost)
            { ((TECScope)StackItem.Item2).AssociatedCosts.Remove((TECAssociatedCost)StackItem.Item3); }
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
            else if (StackItem.Item2 is TECDrawing)
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
                else if (StackItem.Item3 is TECConnectionType)
                {
                    ((TECTemplates)StackItem.Item2).ConnectionTypeCatalog.Remove((TECConnectionType)StackItem.Item3);
                }
                else if (StackItem.Item3 is TECConduitType)
                {
                    ((TECTemplates)StackItem.Item2).ConduitTypeCatalog.Remove((TECConduitType)StackItem.Item3);
                }
                else if (StackItem.Item3 is TECAssociatedCost)
                {
                    ((TECTemplates)StackItem.Item2).AssociatedCostsCatalog.Remove((TECAssociatedCost)StackItem.Item3);
                }
                else if (StackItem.Item3 is TECMiscCost)
                {
                    ((TECTemplates)StackItem.Item2).MiscCostTemplates.Remove((TECMiscCost)StackItem.Item3);
                }
                else if (StackItem.Item3 is TECMiscWiring)
                {
                    ((TECTemplates)StackItem.Item2).MiscWiringTemplates.Remove((TECMiscWiring)StackItem.Item3);
                }
                else if (StackItem.Item3 is TECPanel)
                {
                    ((TECTemplates)StackItem.Item2).PanelTemplates.Remove((TECPanel)StackItem.Item3);
                }
                else if (StackItem.Item3 is TECPanelType)
                {
                    ((TECTemplates)StackItem.Item2).PanelTypeCatalog.Remove((TECPanelType)StackItem.Item3);
                }
            }
            else
            {
                string message = "Target object: " + StackItem.Item2 + " and reference object " + StackItem.Item3 + " not handled in add";
                DebugHandler.LogDebugMessage(message, DEBUG_STACK);
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
                else if (StackItem.Item3 is TECConnectionType)
                {
                    ((TECBid)StackItem.Item2).ConnectionTypes.Add((TECConnectionType)StackItem.Item3);
                }
                else if (StackItem.Item3 is TECConduitType)
                {
                    ((TECBid)StackItem.Item2).ConduitTypes.Add((TECConduitType)StackItem.Item3);
                }
                else if (StackItem.Item3 is TECAssociatedCost)
                {
                    ((TECBid)StackItem.Item2).AssociatedCostsCatalog.Add((TECAssociatedCost)StackItem.Item3);
                }
                else if (StackItem.Item3 is TECMiscCost)
                {
                    ((TECBid)StackItem.Item2).MiscCosts.Add((TECMiscCost)StackItem.Item3);
                }
                else if (StackItem.Item3 is TECMiscWiring)
                {
                    ((TECBid)StackItem.Item2).MiscWiring.Add((TECMiscWiring)StackItem.Item3);
                }
                else if (StackItem.Item3 is TECPanel)
                {
                    ((TECBid)StackItem.Item2).Panels.Add((TECPanel)StackItem.Item3);
                }
                else if (StackItem.Item3 is TECPanelType)
                {
                    ((TECBid)StackItem.Item2).PanelTypeCatalog.Add((TECPanelType)StackItem.Item3);
                }
            }
            else if (StackItem.Item2 is TECScope && StackItem.Item3 is TECAssociatedCost)
            { ((TECScope)StackItem.Item2).AssociatedCosts.Add((TECAssociatedCost)StackItem.Item3); }
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
                else if (StackItem.Item3 is TECConnectionType)
                {
                    ((TECTemplates)StackItem.Item2).ConnectionTypeCatalog.Add((TECConnectionType)StackItem.Item3);
                }
                else if (StackItem.Item3 is TECConduitType)
                {
                    ((TECTemplates)StackItem.Item2).ConduitTypeCatalog.Add((TECConduitType)StackItem.Item3);
                }
                else if (StackItem.Item3 is TECAssociatedCost)
                {
                    ((TECTemplates)StackItem.Item2).AssociatedCostsCatalog.Add((TECAssociatedCost)StackItem.Item3);
                }
                else if (StackItem.Item3 is TECMiscCost)
                {
                    ((TECTemplates)StackItem.Item2).MiscCostTemplates.Add((TECMiscCost)StackItem.Item3);
                }
                else if (StackItem.Item3 is TECMiscWiring)
                {
                    ((TECTemplates)StackItem.Item2).MiscWiringTemplates.Add((TECMiscWiring)StackItem.Item3);
                }
                else if (StackItem.Item3 is TECPanel)
                {
                    ((TECTemplates)StackItem.Item2).PanelTemplates.Add((TECPanel)StackItem.Item3);
                }
                else if (StackItem.Item3 is TECPanelType)
                {
                    ((TECTemplates)StackItem.Item2).PanelTypeCatalog.Add((TECPanelType)StackItem.Item3);
                }
            }
            else{
                string message = "Target object: " + StackItem.Item2 + " and reference object " + StackItem.Item3 + " not handled in remove";
                DebugHandler.LogDebugMessage(message, DEBUG_STACK);
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

        private void handlePropertyChanged(PropertyChangedEventArgs e)
        {
            string message = "Propertychanged: " + e.PropertyName;
            DebugHandler.LogDebugMessage(message, DEBUG_PROPERTIES);

            if (!isDoing){ RedoStack.Clear(); }
            if (e is PropertyChangedExtendedEventArgs<Object>)
            {
                PropertyChangedExtendedEventArgs<Object> args = e as PropertyChangedExtendedEventArgs<Object>;
                Tuple<Change, Object, Object> item;
                object oldValue = args.OldValue;
                object newValue = args.NewValue;
                if (e.PropertyName == "Add")
                {
                    message = "Add change: " + oldValue;
                    DebugHandler.LogDebugMessage(message, DEBUG_PROPERTIES);

                    item = Tuple.Create<Change, Object, Object>(Change.Add, oldValue, newValue);
                    ((TECObject)newValue).PropertyChanged += Object_PropertyChanged;
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

                    item = Tuple.Create<Change, Object, Object>(Change.Remove, oldValue, newValue);
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
                    
                    item = Tuple.Create<Change, Object, Object>(Change.Edit, oldValue, newValue);
                    SaveStack.Add(item);

                    message = "Undo count: " + UndoStack.Count + " Save Count: " + SaveStack.Count;
                    DebugHandler.LogDebugMessage(message, DEBUG_STACK);
                }
                else if (e.PropertyName == "ChildChanged")
                {
                    message = "Child change: " + oldValue;
                    DebugHandler.LogDebugMessage(message, DEBUG_PROPERTIES);
                    
                    item = Tuple.Create<Change, Object, Object>(Change.Edit, oldValue, newValue);
                    SaveStack.Add(item);

                    message = "Undo count: " + UndoStack.Count + " Save Count: " + SaveStack.Count;
                    DebugHandler.LogDebugMessage(message, DEBUG_STACK);
                }
                else if (e.PropertyName == "LocationChanged")
                {
                    message = "Location change: " + oldValue;
                    DebugHandler.LogDebugMessage(message, DEBUG_PROPERTIES);
                    
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

                    message = "Undo count: " + UndoStack.Count + " Save Count: " + SaveStack.Count;
                    DebugHandler.LogDebugMessage(message, DEBUG_STACK);
                }
                else if(e.PropertyName == "MetaAdd")
                {
                    message = "MetaAdd change: " + oldValue;
                    DebugHandler.LogDebugMessage(message, DEBUG_PROPERTIES);

                    item = Tuple.Create<Change, Object, Object>(Change.Add, oldValue, newValue);
                    ((TECObject)newValue).PropertyChanged += Object_PropertyChanged;
                    SaveStack.Add(item);

                    message = "Undo count: " + UndoStack.Count + " Save Count: " + SaveStack.Count;
                    DebugHandler.LogDebugMessage(message, DEBUG_STACK);
                }
                else if (e.PropertyName == "MetaRemove")
                {
                    message = "MetaRemove change: " + oldValue;
                    DebugHandler.LogDebugMessage(message, DEBUG_PROPERTIES);
                    
                    item = Tuple.Create<Change, Object, Object>(Change.Remove, oldValue, newValue);
                    ((TECObject)newValue).PropertyChanged -= Object_PropertyChanged;
                    SaveStack.Add(item);

                    message = "Undo count: " + UndoStack.Count + " Save Count: " + SaveStack.Count;
                    DebugHandler.LogDebugMessage(message, DEBUG_STACK);
                }
                else
                {
                    message = "Edit change: " + oldValue;
                    DebugHandler.LogDebugMessage(message, DEBUG_PROPERTIES);

                    item = Tuple.Create<Change, Object, Object>(Change.Edit, oldValue, newValue);
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
            else if (newItem is TECController)
            {
                handleControllerChildren(newItem as TECController, stackItem.Item1);
            }
            else if (newItem is TECDevice && (stackItem.Item2 is TECBid || stackItem.Item2 is TECTemplates))
            {
                handleDeviceChildren(newItem as TECDevice, stackItem.Item1);
            }
            else if (newItem is TECConduitType || newItem is TECConnectionType)
            {
                handleScopeChildren(newItem as TECScope, stackItem.Item1);
            }
            else if (newItem is TECConnection)
            {
                handleConnectionChildren(newItem as TECConnection, stackItem.Item1);
            }

            else if (newItem is TECDrawing)
            {
                foreach (TECPage page in ((TECDrawing)newItem).Pages)
                {
                    SaveStack.Add(new Tuple<Change, object, object>(Change.Add, newItem, page));
                    page.PropertyChanged += Object_PropertyChanged;
                }
            }
        }

        private void handleSystemChildren(TECSystem item, Change change)
        {
            handleScopeChildren(item as TECScope, change);
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
            handleScopeChildren(item as TECScope, change);
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
            handleScopeChildren(item as TECScope, change);
            Tuple<Change, object, object> stackItem;
            foreach (TECPoint newPoint in item.Points)
            {
                handleScopeChildren(newPoint as TECScope, change);
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
            }
        }

        private void handleConnectionChildren(TECConnection item, Change change)
        {
            Tuple<Change, object, object> stackItem;
            if (item.ConduitType != null)
            {
                stackItem = Tuple.Create(change, (object)item, (object)item.ConduitType);
                SaveStack.Add(stackItem);
            }
        }

        private void handleDeviceChildren(TECDevice item, Change change)
        {
            handleScopeChildren(item as TECScope, change);
            Tuple<Change, object, object> stackItem;
            stackItem = Tuple.Create(change, (object)item, (object)item.Manufacturer);
            SaveStack.Add(stackItem);
            stackItem = Tuple.Create(change, (object)item, (object)item.ConnectionType);
            SaveStack.Add(stackItem);
        }

        private void handleControllerChildren(TECController item, Change change)
        {
            handleScopeChildren(item as TECScope, change);
            Tuple<Change, object, object> stackItem;
            stackItem = Tuple.Create(change, (object)item, (object)item.Manufacturer);
            SaveStack.Add(stackItem);
        }

        private void handleScopeChildren(TECScope item, Change change)
        {
            Tuple<Change, object, object> stackItem;
            foreach(TECAssociatedCost cost in item.AssociatedCosts)
            {
                stackItem = Tuple.Create(change, (object)item, (object)cost);
                SaveStack.Add(stackItem);
            }
            foreach(TECTag tag in item.Tags)
            {
                stackItem = Tuple.Create(change, (object)item, (object)tag);
                SaveStack.Add(stackItem);
            }
        }
        #endregion

        #region Event Handlers
        private void Object_PropertyChanged(object sender, PropertyChangedEventArgs e) { handlePropertyChanged(e); }
        
        #endregion //Event Handlers
    }
}
