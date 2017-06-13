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
            linkIOModules(bid.Controllers, bid.Catalogs.IOModules);
            linkAllConnectionTypes(bid.Controllers, bid.Catalogs.ConnectionTypes);
            linkSystems(bid.Systems, bid);
            linkAllConnections(bid.Controllers, bid.Systems);
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
            linkSystems(templates.SystemTemplates, templates);
            linkIOModules(templates.ControllerTemplates, templates.Catalogs.IOModules);
        }
        public static void LinkSystem(TECSystem system,
            TECScopeManager scopeManager, Dictionary<Guid, Guid> guidDictionary = null)
        {
            linkAllDevicesFromEquipment(system.Equipment, scopeManager.Catalogs.Devices);
            linkPanelTypesInPanel(scopeManager.Catalogs.PanelTypes, system.Panels);
            linkConduitTypeWithConnections(scopeManager.Catalogs.ConduitTypes, system.Controllers);
            linkManufacturersWithControllers(scopeManager.Catalogs.Manufacturers, system.Controllers);
            linkIOModules(system.Controllers, scopeManager.Catalogs.IOModules);
            foreach (TECEquipment equip in system.Equipment)
            {
                linkScopeChildrenInEquipment(equip, scopeManager);
            }
            linkControllersInPanels(system.Controllers, system.Panels, guidDictionary);
            foreach (TECController control in system.Controllers)
            {
                linkScopeChildren(control, scopeManager);
            }
            foreach (TECPanel panel in system.Panels)
            {
                linkScopeChildren(panel, scopeManager);
            }
            linkConnections(system.Controllers, system.Equipment, guidDictionary);
            linkNetworkConnections(system.Controllers, guidDictionary);
            linkSystems(system.SystemInstances, scopeManager);
        }

        public static void LinkCharacteristicInstances(ObservableItemToInstanceList<TECScope> oldCharacteristicInstances, TECSystem system)
        {
            ObservableItemToInstanceList<TECScope> newCharacteristicInstances = new ObservableItemToInstanceList<TECScope>();
            foreach(TECSystem instance in system.SystemInstances)
            {
                linkCharacteristicCollections(system.Equipment, instance.Equipment, oldCharacteristicInstances, newCharacteristicInstances);
                foreach(TECEquipment equipment in system.Equipment)
                {
                    foreach (TECEquipment instanceEquipment in instance.Equipment)
                    {
                        linkCharacteristicCollections(equipment.SubScope, instanceEquipment.SubScope, oldCharacteristicInstances, newCharacteristicInstances);
                        foreach(TECSubScope subscope in equipment.SubScope)
                        {
                            foreach(TECSubScope instanceSubScope in instanceEquipment.SubScope)
                            {
                                linkCharacteristicCollections(subscope.Points, instanceSubScope.Points, oldCharacteristicInstances, newCharacteristicInstances);
                            }
                        }
                    }
                }
                linkCharacteristicCollections(system.Controllers, instance.Controllers, oldCharacteristicInstances, newCharacteristicInstances);
                linkCharacteristicCollections(system.Panels, instance.Panels, oldCharacteristicInstances, newCharacteristicInstances);
            }
            system.CharactersticInstances = newCharacteristicInstances;
        }
        #endregion

        #region Link Methods
        static private void linkCatalogs(TECCatalogs catalogs)
        {
            linkConnectionTypeWithDevices(catalogs.ConnectionTypes, catalogs.Devices);
            linkManufacturersWithDevices(catalogs.Manufacturers, catalogs.Devices);
            linkManufacturersWithIOModules(catalogs.Manufacturers, catalogs.IOModules);

        }
        static private void linkCharacteristicCollections(IList characteristic, IList instances,
            ObservableItemToInstanceList<TECScope> oldCharacteristicInstances,
            ObservableItemToInstanceList<TECScope> newCharacteristicInstances)
        {
            foreach (var item in oldCharacteristicInstances.GetFullDictionary())
            {
                foreach (TECScope charItem in characteristic)
                {
                    if (item.Key.Guid == charItem.Guid)
                    {
                        foreach (var sub in item.Value)
                        {
                            foreach (TECScope subInstance in instances)
                            {
                                if (subInstance.Guid == sub.Guid)
                                {
                                    newCharacteristicInstances.AddItem(charItem, subInstance);
                                }
                            }
                        }

                    }
                }
            }
        }

        static private void linkSystems(ObservableCollection<TECSystem> systems, TECScopeManager scopeManager)
        {
            foreach(TECSystem system in systems)
            {
                LinkSystem(system, scopeManager);
            }
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
        static private void linkScopeChildrenInEquipment(TECEquipment equipment, TECScopeManager scopeManager)
        {
            linkScopeChildren(equipment, scopeManager);
            foreach (TECSubScope ss in equipment.SubScope)
            {
                linkScopeChildren(ss, scopeManager);
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

        static private void linkConnections(ObservableCollection<TECController> controllers, ObservableCollection<TECEquipment> equipment, Dictionary<Guid, Guid> guidDictionary = null)
        {
            foreach (TECEquipment equip in equipment)
            {
                foreach (TECSubScope subScope in equip.SubScope)
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
        static private void linkNetworkConnections(ObservableCollection<TECController> controllers, Dictionary<Guid, Guid> guidDictionary = null)
        {
            foreach (TECController controller in controllers)
            {
                foreach (TECConnection connection in controller.ChildrenConnections)
                {
                    if (connection is TECNetworkConnection)
                    {
                        TECNetworkConnection netConnect = connection as TECNetworkConnection;
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
        }
        static private void linkAllConnections(ObservableCollection<TECController> controllers, ObservableCollection<TECSystem> systems)
        {
            foreach (TECSystem system in systems)
            {
                linkConnections(controllers, system.Equipment);
                foreach(TECSystem instance in system.SystemInstances)
                {
                    linkConnections(controllers, instance.Equipment);
                }
            }
            linkNetworkConnections(controllers);
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
        static private void linkAssociatedCostsInDevice(ObservableCollection<TECCost> costs, TECDevice device)
        {
            ObservableCollection<TECCost> costsToAssign = new ObservableCollection<TECCost>();
            foreach (TECCost cost in costs)
            {
                foreach (TECCost devCost in device.AssociatedCosts)
                {
                    if (devCost.Guid == cost.Guid)
                    { costsToAssign.Add(cost); }
                }
            }
            device.AssociatedCosts = costsToAssign;
        }
        static private void linkAssociatedCostsInSubScope(ObservableCollection<TECCost> costs, TECSubScope subScope)
        {
            ObservableCollection<TECCost> costsToAssign = new ObservableCollection<TECCost>();
            foreach (TECCost cost in costs)
            {
                foreach (TECCost childCost in subScope.AssociatedCosts)
                {
                    if (childCost.Guid == cost.Guid)
                    { costsToAssign.Add(cost); }
                }
            }
            subScope.AssociatedCosts = costsToAssign;
            foreach (TECDevice device in subScope.Devices)
            { linkAssociatedCostsInDevice(costs, device); }
        }
        static private void linkAssociatedCostsInEquipment(ObservableCollection<TECCost> costs, TECEquipment equipment)
        {
            ObservableCollection<TECCost> costsToAssign = new ObservableCollection<TECCost>();
            foreach (TECCost cost in costs)
            {
                foreach (TECCost childCost in equipment.AssociatedCosts)
                {
                    if (childCost.Guid == cost.Guid)
                    { costsToAssign.Add(cost); }
                }
            }
            equipment.AssociatedCosts = costsToAssign;
            foreach (TECSubScope subScope in equipment.SubScope)
            { linkAssociatedCostsInSubScope(costs, subScope); }
        }
        static private void linkAssociatedCostsInSystem(ObservableCollection<TECCost> costs, TECSystem system)
        {
            ObservableCollection<TECCost> costsToAssign = new ObservableCollection<TECCost>();
            foreach (TECCost cost in costs)
            {
                foreach (TECCost childCost in system.AssociatedCosts)
                {
                    if (childCost.Guid == cost.Guid)
                    { costsToAssign.Add(cost); }
                }
            }
            system.AssociatedCosts = costsToAssign;
            foreach (TECEquipment equipment in system.Equipment)
            { linkAssociatedCostsInEquipment(costs, equipment); }
        }
        static private void linkAssociatedCostsInScope(ObservableCollection<TECCost> costs, TECScope scope)
        {
            ObservableCollection<TECCost> costsToAssign = new ObservableCollection<TECCost>();
            foreach (TECCost cost in costs)
            {
                foreach (TECCost scopeCost in scope.AssociatedCosts)
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
            foreach (TECSystem system in bid.Systems)
            {
                foreach(TECSystem instance in system.SystemInstances)
                {
                    foreach(TECEquipment equipment in system.Equipment)
                    {
                        linkCharacteristicWithInstances(equipment, instance.Equipment, placeholderDict, system.CharactersticInstances);
                        foreach(TECSubScope subscope in equipment.SubScope)
                        {
                            foreach(TECEquipment instanceEquipment in instance.Equipment)
                            {
                                linkCharacteristicWithInstances(subscope, instanceEquipment.SubScope, placeholderDict, system.CharactersticInstances);
                                foreach(TECPoint point in subscope.Points)
                                {
                                    foreach(TECSubScope instanceSubScope in instanceEquipment.SubScope)
                                    {
                                        linkCharacteristicWithInstances(point, instanceSubScope.Points, placeholderDict, system.CharactersticInstances);
                                    }
                                }
                            }
                        }
                    }
                    foreach(TECController controller in system.Controllers)
                    {
                        linkCharacteristicWithInstances(controller, instance.Controllers, placeholderDict, system.CharactersticInstances);
                    }
                    foreach(TECPanel panel in system.Panels)
                    {
                        linkCharacteristicWithInstances(panel, instance.Panels, placeholderDict, system.CharactersticInstances);
                    }
                }
            }
        }
        private static void linkCharacteristicWithInstances(TECScope characteristic, IList instances, Dictionary<Guid, List<Guid>> referenceDict, ObservableItemToInstanceList<TECScope> characteristicList)
        {
            foreach(TECScope item in instances)
            {
                if (referenceDict[characteristic.Guid].Contains(item.Guid))
                {
                    characteristicList.AddItem(characteristic, item);
                }
            }
        }
        #endregion Link Methods

    }
}
