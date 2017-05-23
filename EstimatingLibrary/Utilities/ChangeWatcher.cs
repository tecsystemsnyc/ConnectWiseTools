using DebugLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary.Utilities
{

    public enum Change { Add, Remove };
    public class ChangeWatcher
    {
        public Action<object, PropertyChangedEventArgs> Changed;

        public ChangeWatcher(TECScopeManager scopeManager)
        {
            if (scopeManager is TECBid)
            {
                registerBidChanges(scopeManager as TECBid);
            }
            else if (scopeManager is TECTemplates)
            {
                registerTemplatesChanges(scopeManager as TECTemplates);
            }
        }
        public ChangeWatcher(TECSystem system)
        {
            registerSystem(system);
        }

        private void registerBidChanges(TECBid Bid)
        {
            registerScopeManager(Bid);
            Bid.Parameters.PropertyChanged += Object_PropertyChanged;
            foreach (TECScopeBranch branch in Bid.ScopeTree)
            { registerScope(branch); }
            foreach (TECNote note in Bid.Notes)
            { note.PropertyChanged += Object_PropertyChanged; }
            foreach (TECExclusion exclusion in Bid.Exclusions)
            { exclusion.PropertyChanged += Object_PropertyChanged; }
            foreach (TECLocation location in Bid.Locations)
            { location.PropertyChanged += Object_PropertyChanged; }
            foreach (TECSystem system in Bid.Systems)
            { registerSystems(system); }
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
            foreach (TECMiscCost cost in Bid.MiscCosts)
            { cost.PropertyChanged += Object_PropertyChanged; }
            foreach (TECMiscWiring wiring in Bid.MiscWiring)
            { wiring.PropertyChanged += Object_PropertyChanged; }
            foreach (TECPanel panel in Bid.Panels)
            { panel.PropertyChanged += Object_PropertyChanged; }
            foreach(TECControlledScope scope in Bid.ControlledScope)
            { registerControlledScope(scope); }
        }
        private void registerTemplatesChanges(TECTemplates Templates)
        {
            registerScopeManager(Templates);
            foreach (TECSystem system in Templates.SystemTemplates)
            { registerSystems(system); }
            foreach (TECEquipment equipment in Templates.EquipmentTemplates)
            { registerEquipment(equipment); }
            foreach (TECSubScope subScope in Templates.SubScopeTemplates)
            { registerSubScope(subScope); }
            foreach (TECController controller in Templates.ControllerTemplates)
            { registerController(controller); }
            foreach (TECMiscCost addition in Templates.MiscCostTemplates)
            { addition.PropertyChanged += Object_PropertyChanged; }
            foreach (TECMiscWiring addition in Templates.MiscWiringTemplates)
            { addition.PropertyChanged += Object_PropertyChanged; }
            foreach (TECPanel panel in Templates.PanelTemplates)
            { panel.PropertyChanged += Object_PropertyChanged; }
            foreach (TECControlledScope scope in Templates.ControlledScopeTemplates)
            { registerControlledScope(scope); }

        }
        private void registerScopeManager(TECScopeManager scopeManager)
        {
            scopeManager.PropertyChanged += Object_PropertyChanged;
            scopeManager.Labor.PropertyChanged += Object_PropertyChanged;
            scopeManager.Catalogs.PropertyChanged += Object_PropertyChanged;
            registerCatalogs(scopeManager.Catalogs);
        }
        private void registerCatalogs(TECCatalogs catalogs)
        {
            foreach (TECManufacturer manufacturer in catalogs.Manufacturers)
            { manufacturer.PropertyChanged += Object_PropertyChanged; }
            foreach (TECDevice device in catalogs.Devices)
            { device.PropertyChanged += Object_PropertyChanged; }
            foreach (TECIOModule ioModule in catalogs.IOModules)
            { ioModule.PropertyChanged += Object_PropertyChanged; }
            foreach (TECPanelType panelType in catalogs.PanelTypes)
            { panelType.PropertyChanged += Object_PropertyChanged; }
            foreach (TECConnectionType connectionType in catalogs.ConnectionTypes)
            { connectionType.PropertyChanged += Object_PropertyChanged; }
            foreach (TECConduitType conduitType in catalogs.ConduitTypes)
            { conduitType.PropertyChanged += Object_PropertyChanged; }
            foreach (TECAssociatedCost cost in catalogs.AssociatedCosts)
            { cost.PropertyChanged += Object_PropertyChanged; }
            foreach (TECTag tag in catalogs.Tags)
            { tag.PropertyChanged += Object_PropertyChanged; }
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
        //private void registerSystems(TECSystem system)
        //{
        //    //SystemChanged
        //    system.PropertyChanged += Object_PropertyChanged;
        //    foreach (TECEquipment equipment in system.Equipment)
        //    {
        //        registerEquipment(equipment);
        //    }
        //}
        private void registerScope(TECScopeBranch branch)
        {

            branch.PropertyChanged += Object_PropertyChanged;
            foreach (TECScopeBranch scope in branch.Branches)
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
            foreach (TECProposalScope child in pScope.Children)
            {
                registerPropScope(child);
            }
            foreach (TECScopeBranch child in pScope.Notes)
            {
                registerScope(child);
            }
        }
        private void registerSystem(TECSystem scope)
        {
            scope.PropertyChanged += Object_PropertyChanged;
            foreach (TECPanel panel in scope.Panels)
            {
                panel.PropertyChanged += Object_PropertyChanged;
            }
            foreach (TECController controller in scope.Controllers)
            {
                registerController(controller);
            }
            foreach (TECEquipment equipment in scope.Equipment)
            {
                registerEquipment(equipment);
            }
        }
        private void registerController(TECController controller)
        {
            controller.PropertyChanged += Object_PropertyChanged;
            foreach (TECConnection connection in controller.ChildrenConnections)
            {
                connection.PropertyChanged += Object_PropertyChanged;
            }
            foreach (TECIO io in controller.IO)
            {
                io.PropertyChanged += Object_PropertyChanged;
            }
        }

        private void handleChildren(object newItem, Change change)
        {
            if (newItem is TECSystem)
            {
                handleSystemChildren(newItem as TECSystem, change);
            }
            else if (newItem is TECEquipment)
            {
                handleEquipmentChildren(newItem as TECEquipment, change);
            }
            else if (newItem is TECSubScope)
            {
                handleSubScopeChildren(newItem as TECSubScope, change);
            }
            else if (newItem is TECControlledScope)
            {
                handleControlledScope(newItem as TECControlledScope, change);
            }
            else if (newItem is TECDrawing)
            {
                foreach (TECPage page in ((TECDrawing)newItem).Pages)
                {
                    page.PropertyChanged += Object_PropertyChanged;
                }
            }
            else if (newItem is TECController)
            {
                handleControllerChildren(newItem as TECController, change);
            }
        }
        private void handleSystemChildren(TECSystem system, Change change)
        {
            foreach (TECEquipment newEquipment in system.Equipment)
            {
                if (change == Change.Add)
                {
                    newEquipment.PropertyChanged += Object_PropertyChanged;
                }
                else if (change == Change.Remove)
                {
                    newEquipment.PropertyChanged -= Object_PropertyChanged;
                }
                handleEquipmentChildren(newEquipment, change);
            }
        }
        private void handleEquipmentChildren(TECEquipment equipment, Change change)
        {
            foreach (TECSubScope newSubScope in equipment.SubScope)
            {
                if (change == Change.Add)
                {
                    newSubScope.PropertyChanged += Object_PropertyChanged;
                }
                else if (change == Change.Remove)
                {
                    newSubScope.PropertyChanged -= Object_PropertyChanged;
                }
                handleSubScopeChildren(newSubScope, change);
            }
        }
        private void handleSubScopeChildren(TECSubScope subScope, Change change)
        {

            foreach (TECPoint newPoint in subScope.Points)
            {
                if (change == Change.Add)
                {
                    newPoint.PropertyChanged += Object_PropertyChanged;
                }
                else if (change == Change.Remove)
                {
                    newPoint.PropertyChanged -= Object_PropertyChanged;
                }
            }

        }
        private void handleControllerChildren(TECController controller, Change change)
        {
            if(change == Change.Add)
            {
                foreach (TECConnection connection in controller.ChildrenConnections)
                {
                    connection.PropertyChanged += Object_PropertyChanged;
                }
                foreach (TECIO io in controller.IO)
                {
                    io.PropertyChanged += Object_PropertyChanged;
                }
            }
            else if(change == Change.Remove)
            {
                foreach (TECConnection connection in controller.ChildrenConnections)
                {
                    connection.PropertyChanged -= Object_PropertyChanged;
                }
                foreach (TECIO io in controller.IO)
                {
                    io.PropertyChanged -= Object_PropertyChanged;
                }
            }
            
        }

        private void handleControlledScope(TECControlledScope scope, Change change)
        {
            foreach (TECSystem system in scope.Systems)
            {
                if (change == Change.Add)
                {
                    system.PropertyChanged += Object_PropertyChanged;
                }
                else if (change == Change.Remove)
                {
                    system.PropertyChanged -= Object_PropertyChanged;
                }
                handleSystemChildren(system, change);
            }
            foreach (TECController controller in scope.Controllers)
            {
                if (change == Change.Add)
                {
                    controller.PropertyChanged += Object_PropertyChanged;
                }
                else if (change == Change.Remove)
                {
                    controller.PropertyChanged -= Object_PropertyChanged;
                }
            }
            foreach (TECPanel panel in scope.Panels)
            {
                if (change == Change.Add)
                {
                    panel.PropertyChanged += Object_PropertyChanged;
                }
                else if (change == Change.Remove)
                {
                    panel.PropertyChanged -= Object_PropertyChanged;
                }
            }
            foreach(TECControlledScope instance in scope.ScopeInstances)
            {
                if (change == Change.Add)
                {
                    instance.PropertyChanged += Object_PropertyChanged;
                }
                else if (change == Change.Remove)
                {
                    instance.PropertyChanged -= Object_PropertyChanged;
                }
            }
        }

        private void Object_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            handlePropertyChanged(e);
            Changed?.Invoke(sender, e);
        }
        private void handlePropertyChanged(PropertyChangedEventArgs e)
        {
            string message = "Propertychanged: " + e.PropertyName;
            DebugHandler.LogDebugMessage(message, DebugBooleans.Properties);

            if (e is PropertyChangedExtendedEventArgs<Object>)
            {
                PropertyChangedExtendedEventArgs<Object> args = e as PropertyChangedExtendedEventArgs<Object>;
                object oldValue = args.OldValue;
                object newValue = args.NewValue;
                if (e.PropertyName == "Add")
                {
                    message = "Add change: " + oldValue;
                    ((TECObject)newValue).PropertyChanged += Object_PropertyChanged;
                    DebugHandler.LogDebugMessage(message, DebugBooleans.Properties);
                    handleChildren(newValue, Change.Add);
                }
                else if (e.PropertyName == "Remove")
                {
                    message = "Remove change: " + oldValue;
                    DebugHandler.LogDebugMessage(message, DebugBooleans.Properties);

                    ((TECObject)newValue).PropertyChanged -= Object_PropertyChanged;
                    handleChildren(newValue, Change.Remove);

                }
                else if (e.PropertyName == "MetaAdd")
                {
                    message = "MetaAdd change: " + oldValue;
                    DebugHandler.LogDebugMessage(message, DebugBooleans.Properties);

                    ((TECObject)newValue).PropertyChanged += Object_PropertyChanged;

                }
                else if (e.PropertyName == "MetaRemove")
                {
                    message = "MetaRemove change: " + oldValue;
                    DebugHandler.LogDebugMessage(message, DebugBooleans.Properties);

                    ((TECObject)newValue).PropertyChanged -= Object_PropertyChanged;
                }
                else if (e.PropertyName == "RemovedSubScope") { }
                else
                {
                    if(oldValue is TECBid && newValue is TECBid)
                    {
                        if(e.PropertyName == "Parameters")
                        {
                            (newValue as TECBid).Parameters.PropertyChanged += Object_PropertyChanged;
                        } else if(e.PropertyName == "Labor")
                        {
                            (newValue as TECBid).Labor.PropertyChanged += Object_PropertyChanged;
                        }
                    } 
                }
            }
            else
            {
                message = "Property not compatible: " + e.PropertyName;
                DebugHandler.LogDebugMessage(message, DebugBooleans.Properties);

            }
        }
    }
}
