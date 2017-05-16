using DebugLibrary;
using EstimatingLibrary.Interfaces;
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
        #region Public Methods
        public static void LinkBid(TECBid bid, Dictionary<Guid, List<Guid>> placeholderDict = null)
        {
            if(placeholderDict != null)
            {
                linkControlledScopeWithInstances(bid, placeholderDict);
            }
            linkCatalogs(bid.Catalogs);
            linkAllVisualScope(bid.Drawings, bid.Systems, bid.Controllers);
            linkAllLocations(bid.Locations, bid.Systems);
            linkAllDevices(bid.Systems, bid.Catalogs.Devices);
            linkTagsInBid(bid.Catalogs.Tags, bid);
            linkManufacturersWithControllers(bid.Catalogs.Manufacturers, bid.Controllers);
            linkAssociatedCostsWithScope(bid);
            linkConduitTypeWithConnections(bid.Catalogs.ConduitTypes, bid.Controllers);
            linkPanelTypesInPanel(bid.Catalogs.PanelTypes, bid.Panels);
            linkControllersInPanels(bid.Controllers, bid.Panels);
            linkAllConnections(bid.Controllers, bid.Systems);
            linkIOModules(bid.Controllers, bid.Catalogs.IOModules);
            linkAllConnectionTypes(bid.Controllers, bid.Catalogs.ConnectionTypes);
            linkControlledScope(bid.ControlledScope, bid);
        }
        public static void LinkTemplates(TECTemplates templates)
        {
            linkCatalogs(templates.Catalogs);
            linkAllDevicesFromSystems(templates.SystemTemplates, templates.Catalogs.Devices);
            linkAllDevicesFromEquipment(templates.EquipmentTemplates, templates.Catalogs.Devices);
            linkAllDevicesFromSubScope(templates.SubScopeTemplates, templates.Catalogs.Devices);
            linkTagsInTemplates(templates.Catalogs.Tags, templates);
            linkManufacturersWithControllers(templates.Catalogs.Manufacturers, templates.ControllerTemplates);
            linkAssociatedCostsWithScope(templates);
            linkPanelTypesInPanel(templates.Catalogs.PanelTypes, templates.PanelTemplates);
            linkControllersInPanels(templates.ControllerTemplates, templates.PanelTemplates);
            linkControlledScope(templates.ControlledScopeTemplates, templates);
            linkIOModules(templates.ControllerTemplates, templates.Catalogs.IOModules);
        }
        public static void linkControlledScope(TECControlledScope controlledScope,
            TECScopeManager scopeManager, Dictionary<Guid, Guid> guidDictionary = null)
        {
            LinkControlledScopeObjects(controlledScope.Systems, controlledScope.Controllers,
                controlledScope.Panels, scopeManager, guidDictionary);
        }
        public static void LinkControlledScopeObjects(ObservableCollection<TECSystem> systems, ObservableCollection<TECController> controllers,
            ObservableCollection<TECPanel> panels, TECScopeManager scopeManager, Dictionary<Guid, Guid> guidDictionary = null)
        {
            linkAllDevicesFromSystems(systems, scopeManager.Catalogs.Devices);
            linkPanelTypesInPanel(scopeManager.Catalogs.PanelTypes, panels);
            linkConduitTypeWithConnections(scopeManager.Catalogs.ConduitTypes, controllers);
            linkManufacturersWithControllers(scopeManager.Catalogs.Manufacturers, controllers);
            linkIOModules(controllers, scopeManager.Catalogs.IOModules);
            foreach (TECSystem sys in systems)
            {
                linkScopeChildrenInSystem(sys, scopeManager);
            }
            linkControllersInPanels(controllers, panels, guidDictionary);
            foreach (TECController control in controllers)
            {
                linkScopeChildren(control, scopeManager);
            }
            foreach (TECPanel panel in panels)
            {
                linkScopeChildren(panel, scopeManager);
            }
            linkAllConnections(controllers, systems, guidDictionary);
        }
        #endregion

        #region Link Methods
        static private void linkCatalogs(TECCatalogs catalogs)
        {
            linkConnectionTypeWithDevices(catalogs.ConnectionTypes, catalogs.Devices);
            linkManufacturersWithDevices(catalogs.Manufacturers, catalogs.Devices);
            linkManufacturersWithIOModules(catalogs.Manufacturers, catalogs.IOModules);

        }

        static private void linkScopeChildren(TECScope scope, TECScopeManager scopeManager)
        {
            linkTags(scopeManager.Catalogs.Tags, scope);
            linkAssociatedCostsInScope(scopeManager.Catalogs.AssociatedCosts, scope);
            if (scopeManager is TECBid)
            {
                linkLocationsInScope((scopeManager as TECBid).Locations, scope);
            }
        }
        static private void linkScopeChildrenInSystem(TECSystem system, TECScopeManager scopeManager)
        {
            linkScopeChildren(system, scopeManager);
            foreach (TECEquipment equip in system.Equipment)
            {
                linkScopeChildren(equip, scopeManager);
                foreach (TECSubScope ss in equip.SubScope)
                {
                    linkScopeChildren(ss, scopeManager);
                }
            }
        }

        static private void linkAllVisualScope(ObservableCollection<TECDrawing> bidDrawings, ObservableCollection<TECSystem> bidSystems, ObservableCollection<TECController> bidControllers)
        {
            //This function links visual scope with scope in Systems, Equipment, SubScope and Devices if they have the same GUID.
            foreach (TECDrawing drawing in bidDrawings)
            {
                foreach (TECPage page in drawing.Pages)
                {
                    foreach (TECVisualScope vs in page.PageScope)
                    {
                        foreach (TECSystem system in bidSystems)
                        {
                            if (vs.Scope.Guid == system.Guid)
                            {
                                vs.Scope = system;
                                break;
                            }
                            else
                            {
                                foreach (TECEquipment equipment in system.Equipment)
                                {
                                    if (vs.Scope.Guid == equipment.Guid)
                                    {
                                        vs.Scope = equipment;
                                        break;
                                    }
                                    else
                                    {
                                        foreach (TECSubScope subScope in equipment.SubScope)
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
                        foreach (TECController controller in bidControllers)
                        {
                            if (vs.Scope.Guid == controller.Guid)
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
        static private void linkAllConnections(ObservableCollection<TECController> controllers, ObservableCollection<TECSystem> systems, Dictionary<Guid, Guid> guidDictionary = null)
        {
            foreach (TECSystem system in systems)
            {
                foreach (TECEquipment equipment in system.Equipment)
                {
                    foreach (TECSubScope subScope in equipment.SubScope)
                    {
                        foreach (TECController controller in controllers)
                        {
                            foreach (TECConnection connection in controller.ChildrenConnections)
                            {
                                if (connection is TECSubScopeConnection)
                                {
                                    TECSubScopeConnection ssConnect = connection as TECSubScopeConnection;
                                    bool isCopy = (guidDictionary != null && guidDictionary[ssConnect.SubScope.Guid] == guidDictionary[subScope.Guid]);
                                    if (ssConnect.SubScope.Guid == subScope.Guid || isCopy)
                                    {
                                        ssConnect.SubScope = subScope;
                                        subScope.Connection = ssConnect;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            foreach (TECController controller in controllers)
            {
                foreach (TECNetworkConnection netConnect in controller.ChildNetworkConnections)
                {
                    ObservableCollection<TECController> controllersToAdd = new ObservableCollection<TECController>();
                    foreach (TECController child in netConnect.ChildrenControllers)
                    {
                        foreach (TECController bidController in controllers)
                        {
                            bool isCopy = (guidDictionary != null && guidDictionary[child.Guid] == guidDictionary[bidController.Guid]);
                            if (child.Guid == bidController.Guid || isCopy)
                            {
                                controllersToAdd.Add(bidController);
                                bidController.ParentConnection = netConnect;
                            }
                        }
                    }
                    netConnect.ChildrenControllers = controllersToAdd;
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
        static private void linkAllConnectionTypes(ObservableCollection<TECController> controllers, ObservableCollection<TECConnectionType> connectionTypes)
        {
            foreach (TECController controller in controllers)
            {
                foreach (TECConnection connection in controller.ChildrenConnections)
                {
                    if (connection is TECNetworkConnection)
                    {
                        TECNetworkConnection netConnect = connection as TECNetworkConnection;
                        foreach (TECConnectionType type in connectionTypes)
                        {
                            if (netConnect.ConnectionType.Guid == type.Guid)
                            {
                                netConnect.ConnectionType = type;
                                break;
                            }
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
            foreach (TECDevice device in bid.Catalogs.Devices)
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
            foreach (TECDevice device in templates.Catalogs.Devices)
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
                ObservableCollection<TECConnectionType> linkedTypes = new ObservableCollection<TECConnectionType>();
                foreach(TECConnectionType deviceType in device.ConnectionTypes)
                {
                    foreach (TECConnectionType connectionType in connectionTypes)
                    {
                        if (deviceType.Guid == connectionType.Guid)
                        {
                            linkedTypes.Add(connectionType);
                        }
                    }
                }
                device.ConnectionTypes = linkedTypes;
            }
        }
        static private void linkAssociatedCostsWithScope(TECScopeManager scopeManager)
        {
            if (scopeManager is TECBid)
            {
                TECBid bid = scopeManager as TECBid;
                foreach (TECSystem system in bid.Systems)
                { linkAssociatedCostsInSystem(bid.Catalogs.AssociatedCosts, system); }
                foreach (TECConnectionType scope in bid.Catalogs.ConnectionTypes)
                { linkAssociatedCostsInScope(bid.Catalogs.AssociatedCosts, scope); }
                foreach (TECConduitType scope in bid.Catalogs.ConduitTypes)
                { linkAssociatedCostsInScope(bid.Catalogs.AssociatedCosts, scope); }
                foreach (TECController scope in bid.Controllers)
                { linkAssociatedCostsInScope(bid.Catalogs.AssociatedCosts, scope); }
                foreach (TECPanel scope in bid.Panels)
                { linkAssociatedCostsInScope(bid.Catalogs.AssociatedCosts, scope); }

            }
            else if (scopeManager is TECTemplates)
            {
                TECTemplates templates = scopeManager as TECTemplates;
                foreach (TECSystem system in templates.SystemTemplates)
                { linkAssociatedCostsInSystem(templates.Catalogs.AssociatedCosts, system); }
                foreach (TECEquipment equipment in templates.EquipmentTemplates)
                { linkAssociatedCostsInEquipment(templates.Catalogs.AssociatedCosts, equipment); }
                foreach (TECSubScope subScope in templates.SubScopeTemplates)
                { linkAssociatedCostsInSubScope(templates.Catalogs.AssociatedCosts, subScope); }
                foreach (TECDevice device in templates.Catalogs.Devices)
                { linkAssociatedCostsInDevice(templates.Catalogs.AssociatedCosts, device); }
                foreach (TECConnectionType scope in templates.Catalogs.ConnectionTypes)
                { linkAssociatedCostsInScope(templates.Catalogs.AssociatedCosts, scope); }
                foreach (TECConduitType scope in templates.Catalogs.ConduitTypes)
                { linkAssociatedCostsInScope(templates.Catalogs.AssociatedCosts, scope); }
                foreach (TECController scope in templates.ControllerTemplates)
                { linkAssociatedCostsInScope(templates.Catalogs.AssociatedCosts, scope); }
                foreach (TECPanel scope in templates.PanelTemplates)
                { linkAssociatedCostsInScope(templates.Catalogs.AssociatedCosts, scope); }
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

        static private void linkConduitTypeWithConnections(ObservableCollection<TECConduitType> conduitTypes, ObservableCollection<TECController> controllers)
        {
            foreach (TECController controller in controllers)
            {
                foreach (TECConnection connection in controller.ChildrenConnections)
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
        }
        static private void linkPanelTypesInPanel(ObservableCollection<TECPanelType> panelTypes, ObservableCollection<TECPanel> panels)
        {
            foreach (TECPanel panel in panels)
            {
                foreach (TECPanelType type in panelTypes)
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
        static private void linkControlledScope(ObservableCollection<TECControlledScope> controlledScope, TECScopeManager scopeManager)
        {
            foreach (TECControlledScope scope in controlledScope)
            {
                linkAllDevicesFromSystems(scope.Systems, scopeManager.Catalogs.Devices);
                linkPanelTypesInPanel(scopeManager.Catalogs.PanelTypes, scope.Panels);
                linkConduitTypeWithConnections(scopeManager.Catalogs.ConduitTypes, scope.Controllers);
                linkManufacturersWithControllers(scopeManager.Catalogs.Manufacturers, scope.Controllers);
                linkIOModules(scope.Controllers, scopeManager.Catalogs.IOModules);
                foreach (TECSystem sys in scope.Systems)
                {
                    linkScopeChildrenInSystem(sys, scopeManager);
                }
                linkControllersInPanels(scope.Controllers, scope.Panels);
                foreach (TECController control in scope.Controllers)
                {
                    linkScopeChildren(control, scopeManager);
                }
                foreach (TECPanel panel in scope.Panels)
                {
                    linkScopeChildren(panel, scopeManager);
                }
                linkAllConnections(scope.Controllers, scope.Systems);
                linkControlledScope(scope.ScopeInstances, scopeManager);
            }
        }
        static private void linkControllersInPanels(ObservableCollection<TECController> controllers, ObservableCollection<TECPanel> panels, Dictionary<Guid, Guid> guidDictionary = null)
        {
            foreach (TECPanel panel in panels)
            {
                ObservableCollection<TECController> controllersToLink = new ObservableCollection<TECController>();
                foreach (TECController panelController in panel.Controllers)
                {
                    foreach (TECController controller in controllers)
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
        static private void linkIOModules(ObservableCollection<TECController> controllers, ObservableCollection<TECIOModule> ioModules)
        {
            foreach (TECController controller in controllers)
            {
                foreach (TECIO io in controller.IO)
                {
                    if (io.IOModule != null)
                    {
                        foreach (TECIOModule module in ioModules)
                        {
                            if (io.IOModule.Guid == module.Guid)
                            {
                                io.IOModule = module;
                                break;
                            }
                        }
                    }

                }
            }
        }
        static private void linkManufacturersWithIOModules(ObservableCollection<TECManufacturer> manufacturers, ObservableCollection<TECIOModule> ioModules)
        {
            foreach (TECIOModule module in ioModules)
            {
                foreach (TECManufacturer manufacturer in manufacturers)
                {
                    if (module.Manufacturer.Guid == manufacturer.Guid)
                    {
                        module.Manufacturer = manufacturer;
                    }
                }
            }
        }


        private static void linkControlledScopeWithInstances(TECBid bid, Dictionary<Guid, List<Guid>> placeholderDict)
        {
            foreach (TECControlledScope scope in bid.ControlledScope)
            {
                foreach (TECSystem system in scope.Systems)
                {
                    linkCharacteristicSystemWithSystems(scope.CharactersticInstances, placeholderDict, system, bid.Systems);
                }
                foreach (TECController controller in scope.Controllers)
                {
                    linkCharacteristicScopeWithScope(scope.CharactersticInstances, placeholderDict, controller, bid.Controllers);
                }
                foreach (TECPanel panel in scope.Panels)
                {
                    linkCharacteristicScopeWithScope(scope.CharactersticInstances, placeholderDict, panel, bid.Panels);
                }
            }
        }
        private static void linkCharacteristicSystemWithSystems(ObservableItemToInstanceList<TECScope> characteristicList, Dictionary<Guid, List<Guid>> placeholderList,
            TECScope characteristicScope, ObservableCollection<TECSystem> systemInstances)
        {
            foreach (KeyValuePair<Guid, List<Guid>> item in placeholderList)
            {
                if (item.Key == characteristicScope.Guid)
                {
                    foreach (TECSystem system in systemInstances)
                    {
                        foreach (Guid guid in item.Value)
                        {
                            if (system.Guid == guid)
                            {
                                characteristicList.AddItem(characteristicScope, system);
                            }
                            break;
                        }
                        foreach (TECEquipment equipment in system.Equipment)
                        {
                            linkCharacteristicScopeWithScope(characteristicList, placeholderList, equipment, system.Equipment);
                            foreach (TECSubScope subscope in equipment.SubScope)
                            {
                                linkCharacteristicScopeWithScope(characteristicList, placeholderList, subscope, equipment.SubScope);
                                foreach (TECPoint point in subscope.Points)
                                {
                                    linkCharacteristicScopeWithScope(characteristicList, placeholderList, point, subscope.Points);
                                }
                            }
                        }
                    }
                    break;
                }
            }
        }

        private static void linkCharacteristicScopeWithScope(ObservableItemToInstanceList<TECScope> characteristicList, Dictionary<Guid, List<Guid>> placeholderList,
            TECScope characteristicScope, IList scopeInstances)
        {
            foreach (KeyValuePair<Guid, List<Guid>> item in placeholderList)
            {
                if (item.Key == characteristicScope.Guid)
                {
                    foreach (TECScope scope in scopeInstances)
                    {
                        foreach (Guid guid in item.Value)
                        {
                            if (scope.Guid == guid)
                            {
                                characteristicList.AddItem(characteristicScope, scope);
                            }
                            break;
                        }
                    }
                    break;
                }
            }
        }
        #endregion Link Methods

    }
}
