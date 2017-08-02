using DebugLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary.Utilities
{

    public enum Change { Add, Remove, Edit };
    
    public class ChangeWatcher
    {
        private enum ChangeType { Object, Instance };

        public Action<object, PropertyChangedEventArgs> Changed;
        public Action<PropertyChangedExtendedEventArgs> ExtendedChanged;
        public Action<PropertyChangedExtendedEventArgs> InstanceChanged;

        private TECScopeManager scopeManager;

        public ChangeWatcher(TECScopeManager scopeManager)
        {
            this.scopeManager = scopeManager;
            registerScopeManager(scopeManager);
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
            Bid.Parameters.PropertyChanged += Object_PropertyChanged;
            foreach (TECScopeBranch branch in Bid.ScopeTree)
            { registerScopeBranch(branch); }
            foreach (TECLabeled note in Bid.Notes)
            { note.PropertyChanged += Object_PropertyChanged; }
            foreach (TECLabeled exclusion in Bid.Exclusions)
            { exclusion.PropertyChanged += Object_PropertyChanged; }
            foreach (TECLabeled location in Bid.Locations)
            { location.PropertyChanged += Object_PropertyChanged; }
            foreach (TECSystem system in Bid.Systems)
            { registerSystem(system); }
            foreach (TECController controller in Bid.Controllers)
            {
                registerController(controller, ChangeType.Instance);
            }
            foreach (TECMisc cost in Bid.MiscCosts)
            {
                cost.PropertyChanged += Object_PropertyChanged;
                cost.PropertyChanged += Instance_PropertyChanged;
            }
            foreach (TECPanel panel in Bid.Panels)
            {
                panel.PropertyChanged += Object_PropertyChanged;
                panel.PropertyChanged += Instance_PropertyChanged;
            }
        }
        private void registerTemplatesChanges(TECTemplates Templates)
        {
            foreach (TECSystem system in Templates.SystemTemplates)
            { registerSystem(system); }
            foreach (TECEquipment equipment in Templates.EquipmentTemplates)
            { registerEquipment(equipment); }
            foreach (TECSubScope subScope in Templates.SubScopeTemplates)
            { registerSubScope(subScope); }
            foreach (TECController controller in Templates.ControllerTemplates)
            { registerController(controller); }
            foreach (TECMisc addition in Templates.MiscCostTemplates)
            { addition.PropertyChanged += Object_PropertyChanged; }
            foreach (TECPanel panel in Templates.PanelTemplates)
            { panel.PropertyChanged += Object_PropertyChanged; }
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
            foreach (TECElectricalMaterial connectionType in catalogs.ConnectionTypes)
            { connectionType.PropertyChanged += Object_PropertyChanged; }
            foreach (TECElectricalMaterial conduitType in catalogs.ConduitTypes)
            { conduitType.PropertyChanged += Object_PropertyChanged; }
            foreach (TECCost cost in catalogs.AssociatedCosts)
            { cost.PropertyChanged += Object_PropertyChanged; }
            foreach (TECLabeled tag in catalogs.Tags)
            { tag.PropertyChanged += Object_PropertyChanged; }
        }
        private void registerSubScope(TECSubScope subScope, ChangeType changeType = ChangeType.Object)
        {
            //Subscope Changed
            subScope.PropertyChanged += Object_PropertyChanged;
            if (changeType == ChangeType.Instance)
            {
                subScope.PropertyChanged += Instance_PropertyChanged;
            }
            foreach (TECPoint point in subScope.Points)
            {
                //Point Changed
                point.PropertyChanged += Object_PropertyChanged;
                if (changeType == ChangeType.Instance)
                {
                    point.PropertyChanged += Instance_PropertyChanged;
                }
            }
        }
        private void registerEquipment(TECEquipment equipment, ChangeType changeType = ChangeType.Object)
        {
            //equipment Changed
            equipment.PropertyChanged += Object_PropertyChanged;
            if (changeType == ChangeType.Instance)
            {
                equipment.PropertyChanged += Instance_PropertyChanged;
            }
            foreach (TECSubScope subScope in equipment.SubScope)
            {
                registerSubScope(subScope, changeType);
            }
        }
        private void registerScopeBranch(TECScopeBranch branch)
        {
            branch.PropertyChanged += Object_PropertyChanged;
            foreach (TECScopeBranch scope in branch.Branches)
            {
                registerScopeBranch(scope);
            }
        }
        private void registerSystem(TECSystem scope, ChangeType changeType = ChangeType.Object)
        {
            scope.PropertyChanged += Object_PropertyChanged;
            if (changeType == ChangeType.Instance)
            {
                scope.PropertyChanged += Instance_PropertyChanged;
            }
            foreach (TECPanel panel in scope.Panels)
            {
                panel.PropertyChanged += Object_PropertyChanged;
                if(changeType == ChangeType.Instance)
                {
                    panel.PropertyChanged += Instance_PropertyChanged;
                }
            }
            foreach (TECController controller in scope.Controllers)
            {
                registerController(controller, changeType);
            }
            foreach (TECEquipment equipment in scope.Equipment)
            {
                registerEquipment(equipment, changeType);
            }
            foreach(TECSystem system in scope.SystemInstances)
            {
                registerSystem(system, ChangeType.Instance);
            }
            foreach(TECMisc misc in scope.MiscCosts)
            {
                misc.PropertyChanged += Object_PropertyChanged;
                misc.PropertyChanged += Instance_PropertyChanged;
            }
        }
        private void registerController(TECController controller, ChangeType changeType = ChangeType.Object)
        {
            controller.PropertyChanged += Object_PropertyChanged;
            if (changeType == ChangeType.Instance)
            {
                controller.PropertyChanged += Instance_PropertyChanged;
            }
            foreach (TECConnection connection in controller.ChildrenConnections)
            {
                connection.PropertyChanged += Object_PropertyChanged;
                if(changeType == ChangeType.Instance)
                {
                    connection.PropertyChanged += Instance_PropertyChanged;
                }
            }
        }

        private void handleChildren(object newItem, Change change, ChangeType changeType)
        {
            if (newItem is TECSystem)
            {
                handleSystemChildren(newItem as TECSystem, change, changeType);
            }
            else if (newItem is TECEquipment)
            {
                handleEquipmentChildren(newItem as TECEquipment, change, changeType);
            }
            else if (newItem is TECSubScope)
            {
                handleSubScopeChildren(newItem as TECSubScope, change, changeType);
            }
            else if (newItem is TECController)
            {
                handleControllerChildren(newItem as TECController, change, changeType);
            }
        }
        private void handleSystemChildren(TECSystem system, Change change, ChangeType changeType)
        {
            foreach (TECEquipment newEquipment in system.Equipment)
            {
                if (change == Change.Add)
                {
                    if(changeType == ChangeType.Instance)
                    {
                        newEquipment.PropertyChanged += Instance_PropertyChanged;
                    } else
                    {
                        newEquipment.PropertyChanged += Object_PropertyChanged;
                    }
                }
                else if (change == Change.Remove)
                {
                    if (changeType == ChangeType.Instance)
                    {
                        newEquipment.PropertyChanged -= Instance_PropertyChanged;
                    }else
                    {
                        newEquipment.PropertyChanged -= Object_PropertyChanged;
                    }
                }
                handleEquipmentChildren(newEquipment, change, changeType);
            }
            foreach(TECController controller in system.Controllers)
            {
                if (change == Change.Add)
                {
                    if (changeType == ChangeType.Instance)
                    {
                        controller.PropertyChanged += Instance_PropertyChanged;
                    }
                    else
                    {
                        controller.PropertyChanged += Object_PropertyChanged;
                    }
                }
                else if (change == Change.Remove)
                {
                    if (changeType == ChangeType.Instance)
                    {
                        controller.PropertyChanged -= Instance_PropertyChanged;
                    }
                    else
                    {
                        controller.PropertyChanged -= Object_PropertyChanged;
                    }
                }
                handleControllerChildren(controller, change, changeType);
            }
            foreach(TECPanel panel in system.Panels)
            {
                if (change == Change.Add)
                {
                    if (changeType == ChangeType.Instance)
                    {
                        panel.PropertyChanged += Instance_PropertyChanged;
                    }
                    else
                    {
                        panel.PropertyChanged += Object_PropertyChanged;
                    }
                }
                else if (change == Change.Remove)
                {
                    if (changeType == ChangeType.Instance)
                    {
                        panel.PropertyChanged -= Instance_PropertyChanged;
                    }
                    else
                    {
                        panel.PropertyChanged -= Object_PropertyChanged;
                    }
                }
            }
            foreach (TECScopeBranch branch in system.ScopeBranches)
            {
                if (change == Change.Add)
                {
                    if (changeType == ChangeType.Instance)
                    {
                        branch.PropertyChanged += Instance_PropertyChanged;
                    }
                    else
                    {
                        branch.PropertyChanged += Object_PropertyChanged;
                    }
                }
                else if (change == Change.Remove)
                {
                    if (changeType == ChangeType.Instance)
                    {
                        branch.PropertyChanged -= Instance_PropertyChanged;
                    }
                    else
                    {
                        branch.PropertyChanged -= Object_PropertyChanged;
                    }
                }
            }
            foreach (TECSystem instance in system.SystemInstances)
            {
                if (change == Change.Add)
                {
                    if (changeType == ChangeType.Instance)
                    {
                        instance.PropertyChanged += Instance_PropertyChanged;
                    }
                    else
                    {
                        instance.PropertyChanged += Object_PropertyChanged;
                    }
                }
                else if (change == Change.Remove)
                {
                    if (changeType == ChangeType.Instance)
                    {
                        instance.PropertyChanged -= Instance_PropertyChanged;
                    }
                    else
                    {
                        instance.PropertyChanged -= Object_PropertyChanged;
                    }
                }
                handleSystemChildren(instance, change, changeType);
            }
        }
        private void handleEquipmentChildren(TECEquipment equipment, Change change, ChangeType changeType)
        {
            foreach (TECSubScope newSubScope in equipment.SubScope)
            {
                if (change == Change.Add)
                {
                    if (changeType == ChangeType.Instance)
                    {
                        newSubScope.PropertyChanged += Instance_PropertyChanged;
                    }else
                    {
                        newSubScope.PropertyChanged += Object_PropertyChanged;

                    }
                }
                else if (change == Change.Remove)
                {
                    if (changeType == ChangeType.Instance)
                    {
                        newSubScope.PropertyChanged -= Instance_PropertyChanged;
                    } else
                    {
                        newSubScope.PropertyChanged -= Object_PropertyChanged;

                    }
                }
                handleSubScopeChildren(newSubScope, change, changeType);
            }
        }
        private void handleSubScopeChildren(TECSubScope subScope, Change change, ChangeType changeType)
        {

            foreach (TECPoint newPoint in subScope.Points)
            {
                if (change == Change.Add)
                {
                    if (changeType == ChangeType.Instance)
                    {
                        newPoint.PropertyChanged += Instance_PropertyChanged;
                    } else
                    {
                        newPoint.PropertyChanged += Object_PropertyChanged;

                    }
                }
                else if (change == Change.Remove)
                {
                    if (changeType == ChangeType.Instance)
                    {
                        newPoint.PropertyChanged -= Instance_PropertyChanged;
                    } else
                    {
                        newPoint.PropertyChanged -= Object_PropertyChanged;

                    }
                }
            }

        }
        private void handleControllerChildren(TECController controller, Change change, ChangeType changeType)
        {
            if(change == Change.Add)
            {
                foreach (TECConnection connection in controller.ChildrenConnections)
                {
                    
                    if (changeType == ChangeType.Instance)
                    {
                        connection.PropertyChanged += Instance_PropertyChanged;
                    } else{
                        connection.PropertyChanged += Object_PropertyChanged;
                    }

                }
            }
            else if(change == Change.Remove)
            {
                foreach (TECConnection connection in controller.ChildrenConnections)
                {
                    
                    if (changeType == ChangeType.Instance)
                    {
                        connection.PropertyChanged += Instance_PropertyChanged;
                    } else {
                        connection.PropertyChanged -= Object_PropertyChanged;
                    }
                }
            }
        }
        
        private void Object_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChangedExtendedEventArgs args = e as PropertyChangedExtendedEventArgs;
            if(args != null)
            {
                handleExtendedPropertyChanged(args);
                ExtendedChanged?.Invoke(args);
            }
            Changed?.Invoke(sender, e);
        }
        private void Instance_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChangedExtendedEventArgs args = e as PropertyChangedExtendedEventArgs;
            if (args != null)
            {
                if (args.Value is TECSubScopeConnection)
                {
                    if (!isTypicalConnection(args.Value as TECSubScopeConnection, scopeManager as TECBid))
                    {
                        handleInstanceChanged(args);
                        InstanceChanged?.Invoke(args);
                    }
                }
                else
                {
                    handleInstanceChanged(args);
                    InstanceChanged?.Invoke(args);
                }
            }
        }
        private void handleExtendedPropertyChanged(PropertyChangedExtendedEventArgs args)
        {
            string message = "Propertychanged: " + args.PropertyName;
            DebugHandler.LogDebugMessage(message, DebugBooleans.Properties);
            
            if (args.Change == Change.Add)
            {
                message = "Add change: " + args.PropertyName;
                DebugHandler.LogDebugMessage(message, DebugBooleans.Properties);

                ((TECObject)args.Value).PropertyChanged += Object_PropertyChanged;
                handleChildren(args.Value, Change.Add, ChangeType.Object);
                checkForRaiseInstance(args);
            }
            else if (args.Change == Change.Remove)
            {
                message = "Remove change: " + args.PropertyName;
                DebugHandler.LogDebugMessage(message, DebugBooleans.Properties);

                ((TECObject)args.Value).PropertyChanged -= Object_PropertyChanged;
                handleChildren(args.Value, Change.Remove, ChangeType.Object);
                checkForRaiseInstance(args);
            }
            else if (args.Change == Change.Edit)
            {
                if (args.Value is TECBidParameters || args.Value is TECLabor)
                {
                    (args.Value as TECObject).PropertyChanged += Object_PropertyChanged;
                }
                else if (args.Sender is TECBidParameters || args.Sender is TECLabor)
                {
                    InstanceChanged?.Invoke(args);
                }
            }
            else
            {
                throw new NotImplementedException("Change type not recognized.");
            }
        }
        private void handleInstanceChanged(PropertyChangedExtendedEventArgs args)
        {
            string message = "InstanceChanged: " + args.PropertyName;
            DebugHandler.LogDebugMessage(message, DebugBooleans.Properties);

            if (args.Change == Change.Add)
            {
                message = "Add change: " + args.PropertyName;
                DebugHandler.LogDebugMessage(message, DebugBooleans.Properties);

                ((TECObject)args.Value).PropertyChanged += Instance_PropertyChanged;
                handleChildren(args.Value, Change.Add, ChangeType.Instance);
            }
            else if (args.Change == Change.Remove)
            {
                message = "Remove change: " + args.PropertyName;
                DebugHandler.LogDebugMessage(message, DebugBooleans.Properties);

                ((TECObject)args.Value).PropertyChanged -= Instance_PropertyChanged;
                handleChildren(args.Value, Change.Remove, ChangeType.Instance);
            }
            else if (args.Change == Change.Edit)
            {
                if (args.Value is TECBidParameters || args.Value is TECLabor)
                {
                    (args.Value as TECObject).PropertyChanged += Instance_PropertyChanged;
                }
            }
            
        }

        private bool isTypicalConnection(TECSubScopeConnection ssConnect, TECBid bid)
        {
            if(ssConnect != null && bid != null)
            {
                foreach(TECSystem system in bid.Systems)
                {
                    if (system.SubScope.Contains(ssConnect.SubScope))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void checkForRaiseInstance(PropertyChangedExtendedEventArgs args)
        {
            if (args.Sender is TECSystem && args.Value is TECSystem) {
                InstanceChanged?.Invoke(args);
                if(args.Change == Change.Add)
                {
                    (args.Value as TECSystem).PropertyChanged += Instance_PropertyChanged;
                }
                else if (args.Change == Change.Remove)
                {
                    (args.Value as TECSystem).PropertyChanged -= Instance_PropertyChanged;
                }
                handleSystemChildren(args.Value as TECSystem, args.Change, ChangeType.Instance);
            }
            else if (
                args.Sender is TECBid && args.Value is TECController ||
                args.Sender is TECBid && args.Value is TECPanel ||
                args.Sender is TECBid && args.Value is TECMisc ||
                args.Sender is TECSystem && args.Value is TECMisc)
            {
                InstanceChanged?.Invoke(args);
                if (args.Change == Change.Add)
                {
                    (args.Value as TECObject).PropertyChanged += Instance_PropertyChanged;
                }
                else if (args.Change == Change.Remove)
                {
                    (args.Value as TECObject).PropertyChanged -= Instance_PropertyChanged;
                }
                if (args.Value is TECController)
                {
                    handleControllerChildren(args.Value as TECController, args.Change, ChangeType.Instance);
                }
                
            }
            else if (args.Change == Change.Remove && args.Sender is TECBid && args.Value is TECSystem)
            {
                 InstanceChanged?.Invoke(args);
            }
        }
    }
}
