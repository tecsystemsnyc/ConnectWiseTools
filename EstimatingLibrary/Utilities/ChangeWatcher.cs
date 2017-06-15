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
    public enum ChangeType { Object, Instance };
    public class ChangeWatcher
    {
        public Action<object, PropertyChangedEventArgs> Changed;
        public Action<object, PropertyChangedEventArgs> InstanceChanged;

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
            { registerSystem(system); }
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
            {
                registerController(controller, ChangeType.Object);
                registerController(controller, ChangeType.Instance);
            }
            foreach (TECMisc cost in Bid.MiscCosts)
            { cost.PropertyChanged += Object_PropertyChanged; }
            foreach (TECPanel panel in Bid.Panels)
            { panel.PropertyChanged += Object_PropertyChanged; }
        }
        private void registerTemplatesChanges(TECTemplates Templates)
        {
            registerScopeManager(Templates);
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
            foreach (TECConnectionType connectionType in catalogs.ConnectionTypes)
            { connectionType.PropertyChanged += Object_PropertyChanged; }
            foreach (TECConduitType conduitType in catalogs.ConduitTypes)
            { conduitType.PropertyChanged += Object_PropertyChanged; }
            foreach (TECCost cost in catalogs.AssociatedCosts)
            { cost.PropertyChanged += Object_PropertyChanged; }
            foreach (TECTag tag in catalogs.Tags)
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
        private void registerSystem(TECSystem scope, ChangeType changeType = ChangeType.Object)
        {
            scope.PropertyChanged += Object_PropertyChanged;
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
            foreach (TECIO io in controller.IO)
            {
                io.PropertyChanged += Object_PropertyChanged;
                if (changeType == ChangeType.Instance)
                {
                    io.PropertyChanged += Instance_PropertyChanged;
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
            else if (newItem is TECDrawing)
            {
                foreach (TECPage page in ((TECDrawing)newItem).Pages)
                {
                    if (changeType == ChangeType.Instance)
                    {
                        page.PropertyChanged += Instance_PropertyChanged;
                    }
                    else
                    {
                        page.PropertyChanged += Object_PropertyChanged;
                    }
                }
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
                    instance.PropertyChanged += Instance_PropertyChanged;
                    instance.PropertyChanged += Object_PropertyChanged;
                }
                else if (change == Change.Remove)
                {
                    instance.PropertyChanged -= Instance_PropertyChanged;
                    instance.PropertyChanged -= Object_PropertyChanged;
                }
                handleSystemChildren(instance, change, ChangeType.Instance);
                handleSystemChildren(instance, change, ChangeType.Object);
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
                foreach (TECIO io in controller.IO)
                {
                    
                    if (changeType == ChangeType.Instance)
                    {
                        io.PropertyChanged += Instance_PropertyChanged;
                    } else
                    {
                        io.PropertyChanged += Object_PropertyChanged;
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
                foreach (TECIO io in controller.IO)
                {
                    if (changeType == ChangeType.Instance)
                    {
                        io.PropertyChanged += Instance_PropertyChanged;
                    } else{
                        io.PropertyChanged -= Object_PropertyChanged;

                    }
                }
            }
        }

        private void handleSystem(TECSystem sys, Change change, ChangeType changeType)
        {
            foreach (TECEquipment equip in sys.Equipment)
            {
                if (change == Change.Add)
                {
                    if(changeType == ChangeType.Instance)
                    {
                        equip.PropertyChanged += Instance_PropertyChanged;
                    }
                    else
                    {
                        equip.PropertyChanged += Object_PropertyChanged;
                    }
                }
                else if (change == Change.Remove)
                {
                    if (changeType == ChangeType.Instance)
                    {
                        equip.PropertyChanged -= Instance_PropertyChanged;
                    }
                    else
                    {
                        equip.PropertyChanged -= Object_PropertyChanged;
                    }
                }
                handleEquipmentChildren(equip, change, changeType);
            }
            foreach (TECController controller in sys.Controllers)
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
            }
            foreach (TECPanel panel in sys.Panels)
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
            foreach(TECSystem instance in sys.SystemInstances)
            {
                if (change == Change.Add)
                {
                    instance.PropertyChanged += Object_PropertyChanged;
                    instance.PropertyChanged += Instance_PropertyChanged;
                }
                else if (change == Change.Remove)
                {
                    instance.PropertyChanged -= Object_PropertyChanged;
                    instance.PropertyChanged -= Instance_PropertyChanged;
                }
            }
        }

        private void Object_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            handlePropertyChanged(sender, e);
            Changed?.Invoke(sender, e);
        }
        private void Instance_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            handleInstanceChanged(sender, e);
            InstanceChanged?.Invoke(sender, e);
        }
        private void handlePropertyChanged(object sender, PropertyChangedEventArgs e)
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
                    handleChildren(newValue, Change.Add, ChangeType.Object);
                    checkForRaiseInstance(sender, args, Change.Add);
                }
                else if (e.PropertyName == "Remove")
                {
                    message = "Remove change: " + oldValue;
                    DebugHandler.LogDebugMessage(message, DebugBooleans.Properties);

                    ((TECObject)newValue).PropertyChanged -= Object_PropertyChanged;
                    handleChildren(newValue, Change.Remove, ChangeType.Object);
                    checkForRaiseInstance(sender, args, Change.Remove);

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
        private void handleInstanceChanged(object sender, PropertyChangedEventArgs e)
        {
            string message = "InstanceChanged: " + e.PropertyName;
            DebugHandler.LogDebugMessage(message, DebugBooleans.Properties);

            if (e is PropertyChangedExtendedEventArgs<Object>)
            {
                PropertyChangedExtendedEventArgs<Object> args = e as PropertyChangedExtendedEventArgs<Object>;
                object oldValue = args.OldValue;
                object newValue = args.NewValue;
                if (e.PropertyName == "Add")
                {
                    message = "Add change: " + oldValue;
                    ((TECObject)newValue).PropertyChanged += Instance_PropertyChanged;
                    DebugHandler.LogDebugMessage(message, DebugBooleans.Properties);
                    handleChildren(newValue, Change.Add, ChangeType.Instance);
                }
                else if (e.PropertyName == "Remove")
                {
                    message = "Remove change: " + oldValue;
                    DebugHandler.LogDebugMessage(message, DebugBooleans.Properties);

                    ((TECObject)newValue).PropertyChanged -= Instance_PropertyChanged;
                    handleChildren(newValue, Change.Remove, ChangeType.Instance);

                }
                else if (e.PropertyName == "MetaAdd")
                {
                    message = "MetaAdd change: " + oldValue;
                    DebugHandler.LogDebugMessage(message, DebugBooleans.Properties);

                    ((TECObject)newValue).PropertyChanged += Instance_PropertyChanged;

                }
                else if (e.PropertyName == "MetaRemove")
                {
                    message = "MetaRemove change: " + oldValue;
                    DebugHandler.LogDebugMessage(message, DebugBooleans.Properties);

                    ((TECObject)newValue).PropertyChanged -= Instance_PropertyChanged;
                }
                else if (e.PropertyName == "RemovedSubScope") { }
                else
                {
                    if (oldValue is TECBid && newValue is TECBid)
                    {
                        if (e.PropertyName == "Parameters")
                        {
                            (newValue as TECBid).Parameters.PropertyChanged += Instance_PropertyChanged;
                        }
                        else if (e.PropertyName == "Labor")
                        {
                            (newValue as TECBid).Labor.PropertyChanged += Instance_PropertyChanged;
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

        private void checkForRaiseInstance(object sender, PropertyChangedExtendedEventArgs<object> args, Change change)
        {
            var oldValue = args.OldValue;
            var newValue = args.NewValue;
            if (oldValue is TECSystem && newValue is TECSystem) {
                InstanceChanged?.Invoke(sender, args);
                if(change == Change.Add)
                {
                    (newValue as TECSystem).PropertyChanged += Instance_PropertyChanged;
                }
                else if (change == Change.Remove)
                {
                    (newValue as TECSystem).PropertyChanged -= Instance_PropertyChanged;
                }
                handleSystem(newValue as TECSystem, change, ChangeType.Instance);
            }
            else if (oldValue is TECBid && newValue is TECController ||
                oldValue is TECBid && newValue is TECPanel)
            {
                InstanceChanged?.Invoke(sender, args);
                if (change == Change.Add)
                {
                    (newValue as TECScope).PropertyChanged += Instance_PropertyChanged;
                }
                else if (change == Change.Remove)
                {
                    (newValue as TECScope).PropertyChanged -= Instance_PropertyChanged;
                }
                if (newValue is TECController)
                {
                    handleControllerChildren(newValue as TECController, change, ChangeType.Instance);
                }
                
            }
        }
    }
}
