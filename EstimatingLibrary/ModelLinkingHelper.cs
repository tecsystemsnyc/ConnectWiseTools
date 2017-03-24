using DebugLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public static class ModelLinkingHelper
    {

        public static void LinkBid(TECBid bid)
        {
            linkAllVisualScope(bid.Drawings, bid.Systems, bid.Controllers);
            linkAllLocations(bid.Locations, bid.Systems);
            linkConnectionTypeWithDevices(bid.ConnectionTypes, bid.DeviceCatalog);
            linkAllDevices(bid.Systems, bid.DeviceCatalog);
            linkManufacturersWithDevices(bid.ManufacturerCatalog, bid.DeviceCatalog);
            linkTagsInBid(bid.Tags, bid);
            linkManufacturersWithControllers(bid.ManufacturerCatalog, bid.Controllers);
            linkAssociatedCostsWithScope(bid);
            linkConduitTypeWithConnections(bid.ConduitTypes, bid.Connections);
            linkPanelTypesInPanel(bid.PanelTypeCatalog, bid.Panels);
            linkControllersInPanels(bid.Controllers, bid.Panels);
            linkAllConnections(bid.Connections, bid.Controllers, bid.Systems);
        }

        public static void LinkTemplates(TECTemplates templates)
        {
            linkAllDevicesFromSystems(templates.SystemTemplates, templates.DeviceCatalog);
            linkAllDevicesFromEquipment(templates.EquipmentTemplates, templates.DeviceCatalog);
            linkAllDevicesFromSubScope(templates.SubScopeTemplates, templates.DeviceCatalog);
            linkManufacturersWithDevices(templates.ManufacturerCatalog, templates.DeviceCatalog);
            linkConnectionTypeWithDevices(templates.ConnectionTypeCatalog, templates.DeviceCatalog);
            linkTagsInTemplates(templates.Tags, templates);
            linkManufacturersWithControllers(templates.ManufacturerCatalog, templates.ControllerTemplates);
            linkAssociatedCostsWithScope(templates);
            linkPanelTypesInPanel(templates.PanelTypeCatalog, templates.PanelTemplates);
            linkControllersInPanels(templates.ControllerTemplates, templates.PanelTemplates);
            linkControlledScope(templates.ControlledScopeTemplates, templates);
        }

        public static void LinkControlledScopeObjects(ObservableCollection<TECSystem> systems, ObservableCollection<TECController> controllers,
            ObservableCollection<TECPanel> panels, ObservableCollection<TECConnection> connections, TECBid bid, Dictionary<Guid, Guid> guidDictionary = null)
        {

            linkAllDevicesFromSystems(systems, bid.DeviceCatalog);
            linkPanelTypesInPanel(bid.PanelTypeCatalog, panels);
            linkConduitTypeWithConnections(bid.ConduitTypes, connections);
            linkManufacturersWithControllers(bid.ManufacturerCatalog, controllers);

            foreach (TECSystem sys in systems)
            {
                linkScopeChildrenInSystem(sys, bid);
            }
            linkControllersInPanels(controllers, panels, guidDictionary);
            foreach (TECController control in controllers)
            {
                linkScopeChildren(control, bid);
            }
            foreach (TECPanel panel in panels)
            {
                linkScopeChildren(panel, bid);
            }
            linkAllConnections(connections, controllers, systems, guidDictionary);
        }

        #region Link Methods
        static private void linkScopeChildren(TECScope scope, object bidOrTemp)
        {
            if (bidOrTemp is TECBid)
            {
                linkTags((bidOrTemp as TECBid).Tags, scope);
                linkAssociatedCostsInScope((bidOrTemp as TECBid).AssociatedCostsCatalog, scope);
                linkLocationsInScope((bidOrTemp as TECBid).Locations, scope);
                
            }
            else if (bidOrTemp is TECTemplates)
            {
                linkTags((bidOrTemp as TECTemplates).Tags, scope);
                linkAssociatedCostsInScope((bidOrTemp as TECTemplates).AssociatedCostsCatalog, scope);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
        static private void linkScopeChildrenInSystem(TECSystem system, object bidOrTemp)
        {
            linkScopeChildren(system, bidOrTemp);
            foreach(TECEquipment equip in system.Equipment)
            {
                linkScopeChildren(equip, bidOrTemp);
                foreach(TECSubScope ss in equip.SubScope)
                {
                    linkScopeChildren(ss, bidOrTemp);
                }
            }
        }

        static private void linkAllVisualScope(ObservableCollection<TECDrawing> bidDrawings, ObservableCollection<TECSystem> bidSystems, ObservableCollection<TECController> bidControllers)
        {
            //This function links visual scope with scope in Systems, Equipment, SubScope and Devices if they have the same GUID.
            foreach(TECDrawing drawing in bidDrawings)
            {
                foreach(TECPage page in drawing.Pages)
                {
                    foreach(TECVisualScope vs in page.PageScope)
                    {
                        foreach(TECSystem system in bidSystems)
                        {
                            if (vs.Scope.Guid == system.Guid)
                            {
                                vs.Scope = system;
                                break;
                            }
                            else
                            {
                                foreach(TECEquipment equipment in system.Equipment)
                                {
                                    if (vs.Scope.Guid == equipment.Guid)
                                    {
                                        vs.Scope = equipment;
                                        break;
                                    }
                                    else
                                    {
                                        foreach(TECSubScope subScope in equipment.SubScope)
                                        {
                                            if (vs.Scope.Guid == subScope.Guid)
                                            {
                                                vs.Scope = subScope;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        foreach(TECController controller in bidControllers)
                        {
                            if(vs.Scope.Guid == controller.Guid)
                            {
                                vs.Scope = controller;
                            }
                        }
                    }
                }
            }
        }
        static private void linkAllLocations(ObservableCollection<TECLocation> locations, ObservableCollection<TECSystem> bidSystems)
        {
            foreach (TECLocation location in locations)
            {
                foreach (TECSystem system in bidSystems)
                {
                    if (system.Location != null && system.Location.Guid == location.Guid)
                    { system.Location = location; }
                    foreach (TECEquipment equipment in system.Equipment)
                    {
                        if (equipment.Location != null && equipment.Location.Guid == location.Guid)
                        { equipment.Location = location; }
                        foreach (TECSubScope subScope in equipment.SubScope)
                        {
                            if (subScope.Location != null && subScope.Location.Guid == location.Guid)
                            { subScope.Location = location; }
                        }
                    }
                }
            }
        }
        static private void linkLocationsInScope(ObservableCollection<TECLocation> locations, TECScope scope)
        {
            foreach (TECLocation location in locations)
            {
                if (scope.Location != null && scope.Location.Guid == location.Guid)
                {
                    scope.Location = location;
                    break;
                }
            }
        }
        static private void linkAllConnections(object connections, ObservableCollection<TECController> controllers, ObservableCollection<TECSystem> systems, Dictionary<Guid, Guid> guidDictionary = null)
        {
            if (connections is ObservableCollection<TECSubScopeConnection> || connections is ObservableCollection<TECNetworkConnection> || connections is ObservableCollection<TECConnection>)
            {
                foreach (TECConnection connection in (IList)connections)
                {
                    //All Connections
                    foreach (TECController controller in controllers)
                    {
                        bool isCopy = guidDictionary != null && guidDictionary[connection.ParentController.Guid] == guidDictionary[controller.Guid];
                        if (controller.Guid == connection.ParentController.Guid || isCopy)
                        { connection.ParentController = controller; }
                    }

                    #region If Connection is NetworkConnection
                    if (connection is TECNetworkConnection)
                    {
                        TECNetworkConnection netConnect = connection as TECNetworkConnection;

                        List<TECController> controllerToRemove = new List<TECController>();
                        List<TECController> controllerToAdd = new List<TECController>();

                        foreach (TECController controller in controllers)
                        {
                            foreach (TECController childController in netConnect.ChildrenControllers)
                            {
                                bool isCopy = guidDictionary != null && guidDictionary[childController.Guid] == guidDictionary[controller.Guid];
                                if (childController.Guid == controller.Guid || isCopy)
                                {
                                    controllerToRemove.Add(childController);
                                    controllerToAdd.Add(controller);
                                }
                            }
                        }

                        foreach (TECController controller in controllerToRemove)
                        { netConnect.ChildrenControllers.Remove(controller); }
                        foreach (TECController controller in controllerToAdd)
                        { netConnect.ChildrenControllers.Add(controller); }
                    }
                    #endregion

                    #region If Connection is SubScopeConnection
                    else if (connection is TECSubScopeConnection)
                    {
                        TECSubScopeConnection ssConnect = connection as TECSubScopeConnection;

                        List<TECScope> ssToRemove = new List<TECScope>();
                        List<TECScope> ssToAdd = new List<TECScope>();

                        foreach (TECSystem system in systems)
                        {
                            foreach (TECEquipment equipment in system.Equipment)
                            {
                                foreach (TECSubScope subScope in equipment.SubScope)
                                {
                                    foreach (TECSubScope childSS in ssConnect.SubScope)
                                    {
                                        bool isCopy = guidDictionary != null && guidDictionary[childSS.Guid] == guidDictionary[subScope.Guid];
                                        if (childSS.Guid == subScope.Guid || isCopy)
                                        {
                                            ssToRemove.Add(childSS);
                                            ssToAdd.Add(subScope);
                                        }
                                    }
                                }
                            }
                        }

                        foreach (TECSubScope ss in ssToRemove)
                        { ssConnect.SubScope.Remove(ss); }
                        foreach (TECSubScope ss in ssToAdd)
                        { ssConnect.SubScope.Add(ss); }
                    }
                    #endregion

                    else
                    {
                        throw new NotImplementedException();
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
                                { linkedDevices.Add(cDev); }
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
            { linkAllDevicesFromEquipment(system.Equipment, deviceCatalog); }
        }
        static private void linkAllDevicesFromEquipment(ObservableCollection<TECEquipment> equipment, ObservableCollection<TECDevice> deviceCatalog)
        {
            foreach (TECEquipment equip in equipment)
            { linkAllDevicesFromSubScope(equip.SubScope, deviceCatalog); }
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
                        { linkedDevices.Add(cDev); }
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
        static private void linkManufacturersWithControllers(ObservableCollection<TECManufacturer> mans, ObservableCollection<TECController> controllers)
        {
            foreach (TECController controller in controllers)
            {
                foreach (TECManufacturer manufacturer in mans)
                {
                    if (controller.Manufacturer != null)
                    {
                        if (controller.Manufacturer.Guid == manufacturer.Guid)
                        {
                            controller.Manufacturer = manufacturer;
                        }
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
                        { linkTags(tags, device); }
                        foreach (TECPoint point in subScope.Points)
                        { linkTags(tags, point); }
                    }
                }
            }
            foreach (TECController controller in bid.Controllers)
            { linkTags(tags, controller); }
            foreach (TECDevice device in bid.DeviceCatalog)
            { linkTags(tags, device); }
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
                        { linkTags(tags, device); }
                        foreach (TECPoint point in subScope.Points)
                        { linkTags(tags, point); }
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
                    { linkTags(tags, device); }
                    foreach (TECPoint point in subScope.Points)
                    { linkTags(tags, point); }
                }
            }
            foreach (TECSubScope subScope in templates.SubScopeTemplates)
            {
                linkTags(tags, subScope);
                foreach (TECDevice device in subScope.Devices)
                { linkTags(tags, device); }
                foreach (TECPoint point in subScope.Points)
                { linkTags(tags, point); }
            }
            foreach (TECController controller in templates.ControllerTemplates)
            { linkTags(tags, controller); }
            foreach (TECDevice device in templates.DeviceCatalog)
            { linkTags(tags, device); }
        }
        static private void linkTags(ObservableCollection<TECTag> tags, TECScope scope)
        {
            ObservableCollection<TECTag> linkedTags = new ObservableCollection<TECTag>();
            foreach (TECTag tag in scope.Tags)
            {
                foreach (TECTag referenceTag in tags)
                {
                    if (tag.Guid == referenceTag.Guid)
                    { linkedTags.Add(referenceTag); }
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
        static private void linkAssociatedCostsWithScope(Object bidOrTemp)
        {
            if (bidOrTemp is TECBid)
            {
                TECBid bid = bidOrTemp as TECBid;
                foreach (TECSystem system in bid.Systems)
                { linkAssociatedCostsInSystem(bid.AssociatedCostsCatalog, system); }
                foreach (TECConnectionType scope in bid.ConnectionTypes)
                { linkAssociatedCostsInScope(bid.AssociatedCostsCatalog, scope); }
                foreach (TECConduitType scope in bid.ConduitTypes)
                { linkAssociatedCostsInScope(bid.AssociatedCostsCatalog, scope); }
                foreach (TECController scope in bid.Controllers)
                { linkAssociatedCostsInScope(bid.AssociatedCostsCatalog, scope); }

            }
            else if (bidOrTemp is TECTemplates)
            {
                TECTemplates templates = bidOrTemp as TECTemplates;
                foreach (TECSystem system in templates.SystemTemplates)
                { linkAssociatedCostsInSystem(templates.AssociatedCostsCatalog, system); }
                foreach (TECEquipment equipment in templates.EquipmentTemplates)
                { linkAssociatedCostsInEquipment(templates.AssociatedCostsCatalog, equipment); }
                foreach (TECSubScope subScope in templates.SubScopeTemplates)
                { linkAssociatedCostsInSubScope(templates.AssociatedCostsCatalog, subScope); }
                foreach (TECDevice device in templates.DeviceCatalog)
                { linkAssociatedCostsInDevice(templates.AssociatedCostsCatalog, device); }
                foreach (TECConnectionType scope in templates.ConnectionTypeCatalog)
                { linkAssociatedCostsInScope(templates.AssociatedCostsCatalog, scope); }
                foreach (TECConduitType scope in templates.ConduitTypeCatalog)
                { linkAssociatedCostsInScope(templates.AssociatedCostsCatalog, scope); }
                foreach (TECController scope in templates.ControllerTemplates)
                { linkAssociatedCostsInScope(templates.AssociatedCostsCatalog, scope); }
            }
        }
        static private void linkAssociatedCostsInDevice(ObservableCollection<TECAssociatedCost> costs, TECDevice device)
        {
            ObservableCollection<TECAssociatedCost> costsToAssign = new ObservableCollection<TECAssociatedCost>();
            foreach (TECAssociatedCost cost in costs)
            {
                foreach (TECAssociatedCost devCost in device.AssociatedCosts)
                {
                    if (devCost.Guid == cost.Guid)
                    { costsToAssign.Add(cost); }
                }
            }
            device.AssociatedCosts = costsToAssign;
        }
        static private void linkAssociatedCostsInSubScope(ObservableCollection<TECAssociatedCost> costs, TECSubScope subScope)
        {
            ObservableCollection<TECAssociatedCost> costsToAssign = new ObservableCollection<TECAssociatedCost>();
            foreach (TECAssociatedCost cost in costs)
            {
                foreach (TECAssociatedCost childCost in subScope.AssociatedCosts)
                {
                    if (childCost.Guid == cost.Guid)
                    { costsToAssign.Add(cost); }
                }
            }
            subScope.AssociatedCosts = costsToAssign;
            foreach (TECDevice device in subScope.Devices)
            { linkAssociatedCostsInDevice(costs, device); }
        }
        static private void linkAssociatedCostsInEquipment(ObservableCollection<TECAssociatedCost> costs, TECEquipment equipment)
        {
            ObservableCollection<TECAssociatedCost> costsToAssign = new ObservableCollection<TECAssociatedCost>();
            foreach (TECAssociatedCost cost in costs)
            {
                foreach (TECAssociatedCost childCost in equipment.AssociatedCosts)
                {
                    if (childCost.Guid == cost.Guid)
                    { costsToAssign.Add(cost); }
                }
            }
            equipment.AssociatedCosts = costsToAssign;
            foreach (TECSubScope subScope in equipment.SubScope)
            { linkAssociatedCostsInSubScope(costs, subScope); }
        }
        static private void linkAssociatedCostsInSystem(ObservableCollection<TECAssociatedCost> costs, TECSystem system)
        {
            ObservableCollection<TECAssociatedCost> costsToAssign = new ObservableCollection<TECAssociatedCost>();
            foreach (TECAssociatedCost cost in costs)
            {
                foreach (TECAssociatedCost childCost in system.AssociatedCosts)
                {
                    if (childCost.Guid == cost.Guid)
                    { costsToAssign.Add(cost); }
                }
            }
            system.AssociatedCosts = costsToAssign;
            foreach (TECEquipment equipment in system.Equipment)
            { linkAssociatedCostsInEquipment(costs, equipment); }
        }
        static private void linkAssociatedCostsInScope(ObservableCollection<TECAssociatedCost> costs, TECScope scope)
        {
            ObservableCollection<TECAssociatedCost> costsToAssign = new ObservableCollection<TECAssociatedCost>();
            foreach (TECAssociatedCost cost in costs)
            {
                foreach (TECAssociatedCost scopeCost in scope.AssociatedCosts)
                {
                    if (scopeCost.Guid == cost.Guid)
                    { costsToAssign.Add(cost); }
                }
            }
            scope.AssociatedCosts = costsToAssign;
        }
       
        static private void linkConduitTypeWithConnections(ObservableCollection<TECConduitType> conduitTypes, object connections)
        {
            if (connections is ObservableCollection<TECSubScopeConnection> || connections is ObservableCollection<TECNetworkConnection> || connections is ObservableCollection<TECConnection>)
            {
                foreach (TECConnection connection in (IList)connections)
                {
                    if (connection.ConduitType != null)
                    {
                        foreach (TECConduitType conduitType in conduitTypes)
                        {
                            if (connection.ConduitType.Guid == conduitType.Guid)
                            {
                                connection.ConduitType = conduitType;
                            }
                        }
                    }
                }
            }
            else
            {
                throw new NotImplementedException();
            }
            
        }
        static private void linkPanelTypesInPanel(ObservableCollection<TECPanelType> panelTypes, ObservableCollection<TECPanel> panels)
        {
            foreach(TECPanel panel in panels)
            {
                foreach(TECPanelType type in panelTypes)
                {
                    if (panel.Type != null)
                    {
                        if (panel.Type.Guid == type.Guid)
                        {
                            panel.Type = type;
                            break;
                        }
                    }
                }
            }
        }
        static private void linkControlledScope(ObservableCollection<TECControlledScope> controlledScope, TECTemplates templates)
        {
            foreach(TECControlledScope scope in controlledScope)
            {
                linkAllDevicesFromSystems(scope.Systems, templates.DeviceCatalog);
                linkPanelTypesInPanel(templates.PanelTypeCatalog, scope.Panels);
                linkConduitTypeWithConnections(templates.ConduitTypeCatalog, scope.Connections);
                linkManufacturersWithControllers(templates.ManufacturerCatalog, scope.Controllers);
                foreach(TECSystem sys in scope.Systems)
                {
                    linkScopeChildrenInSystem(sys, templates);
                }
                linkControllersInPanels(scope.Controllers, scope.Panels);
                foreach (TECController control in scope.Controllers)
                {
                    linkScopeChildren(control, templates);
                }
                foreach (TECPanel panel in scope.Panels)
                {
                    linkScopeChildren(panel, templates);
                }
                linkAllConnections(scope.Connections, scope.Controllers, scope.Systems);
            }
        }
        static private void linkControllersInPanels(ObservableCollection<TECController> controllers, ObservableCollection<TECPanel> panels, Dictionary<Guid, Guid> guidDictionary = null)
        {
            foreach(TECPanel panel in panels)
            {
                ObservableCollection<TECController> controllersToLink = new ObservableCollection<TECController>();
                foreach(TECController panelController in panel.Controllers)
                {
                    foreach(TECController controller in controllers)
                    {
                        if (panelController.Guid == controller.Guid)
                        {
                            controllersToLink.Add(controller);
                            break;
                        }
                        else if (guidDictionary != null && guidDictionary[panelController.Guid] == guidDictionary[controller.Guid])
                        {
                            controllersToLink.Add(controller);
                            break;
                        }
                    }
                }
                panel.Controllers = controllersToLink;
            }
        }

        //static private void linkScopeObjects(object scopeReferenceList, object scopeObjectList)
        //{
        //    var linkedList = new ObservableCollection<TECScope>();
        //    foreach(TECScope refScope in (IList)scopeReferenceList)
        //    {
        //        foreach(TECScope objectScope in (IList)scopeObjectList)
        //        {
        //            if(refScope.Guid == objectScope.Guid)
        //            {
        //                linkedList.Add(objectScope as TECScope);
        //            }
        //        }
        //    }
        //    scopeReferenceList = linkedList;
        //}
        #endregion Link Methods

    }
}
