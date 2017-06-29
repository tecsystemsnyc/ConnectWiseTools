using DebugLibrary;
using EstimatingLibrary.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary.Utilities
{
    public static class ModelLinkingHelper
    {
        #region Public Methods
        public static void LinkBid(TECBid bid, Dictionary<Guid, List<Guid>> guidDictionary)
        {
            ObservableCollection<TECController> allControllers = new ObservableCollection<TECController>();
            ObservableCollection<TECSubScope> allSubScope = new ObservableCollection<TECSubScope>();

            linkCatalogs(bid.Catalogs);

            foreach (TECSystem sys in bid.Systems)
            {
                foreach (TECController controller in sys.Controllers)
                {
                    allControllers.Add(controller);
                }
                foreach (TECSystem instance in sys.SystemInstances)
                {
                    foreach (TECController controller in instance.Controllers)
                    {
                        allControllers.Add(controller);
                    }
                    foreach (TECEquipment equip in instance.Equipment)
                    {
                        foreach (TECSubScope ss in equip.SubScope)
                        {
                            allSubScope.Add(ss);
                        }
                    }
                    linkPanelsToControllers(instance.Panels, instance.Controllers);
                }
                foreach (TECEquipment equip in sys.Equipment)
                {
                    foreach (TECSubScope ss in equip.SubScope)
                    {
                        allSubScope.Add(ss);
                    }
                }

                linkSystemToCatalogs(sys, bid.Catalogs);
                linkLocation(sys, bid.Locations);
                linkPanelsToControllers(sys.Panels, sys.Controllers);

                createScopeDictionary(sys, guidDictionary);
            }

            foreach (TECController controller in bid.Controllers)
            {
                allControllers.Add(controller);

                linkControllerToCatalogs(controller, bid.Catalogs);
            }

            foreach (TECPanel panel in bid.Panels)
            {
                linkPanelToCatalogs(panel, bid.Catalogs);
            }

            linkNetworkConnections(allControllers);
            linkSubScopeConnections(allControllers, allSubScope);
            linkPanelsToControllers(bid.Panels, bid.Controllers);
        }

        public static void LinkTemplates(TECTemplates templates)
        {
            linkCatalogs(templates.Catalogs);

            foreach (TECSystem sys in templates.SystemTemplates)
            {
                linkSystemToCatalogs(sys, templates.Catalogs);

                ObservableCollection<TECSubScope> allSysSubScope = new ObservableCollection<TECSubScope>();
                foreach (TECEquipment equip in sys.Equipment)
                {
                    foreach (TECSubScope ss in equip.SubScope)
                    {
                        allSysSubScope.Add(ss);
                    }
                }
                linkSubScopeConnections(sys.Controllers, allSysSubScope);
                linkPanelsToControllers(sys.Panels, sys.Controllers);
            }
            foreach (TECEquipment equip in templates.EquipmentTemplates)
            {
                linkEquipmentToCatalogs(equip, templates.Catalogs);
            }
            foreach (TECSubScope ss in templates.SubScopeTemplates)
            {
                linkSubScopeToCatalogs(ss, templates.Catalogs);
            }
            foreach (TECController controller in templates.ControllerTemplates)
            {
                linkControllerToCatalogs(controller, templates.Catalogs);
            }
            foreach (TECPanel panel in templates.PanelTemplates)
            {
                linkPanelToCatalogs(panel, templates.Catalogs);
            }
        }

        public static void LinkSystem(TECSystem system, TECScopeManager scopeManager, Dictionary<Guid, Guid> guidDictionary)
        {
            ObservableCollection<TECSubScope> allSubScope = new ObservableCollection<TECSubScope>();
            foreach (TECEquipment equip in system.Equipment)
            {
                foreach (TECSubScope ss in equip.SubScope)
                {
                    allSubScope.Add(ss);
                }
            }

            ObservableCollection<TECController> controllers = null;
            if (scopeManager is TECBid)
            {
                controllers = new ObservableCollection<TECController>();
                foreach(TECController controller in system.Controllers)
                {
                    controllers.Add(controller);
                }
                foreach(TECController controller in (scopeManager as TECBid).Controllers)
                {
                    controllers.Add(controller);
                }
            }
            else if (scopeManager is TECTemplates)
            {
                controllers = system.Controllers;
            }

            linkSystemToCatalogs(system, scopeManager.Catalogs);
            linkSubScopeConnections(controllers, allSubScope, guidDictionary);
            linkPanelsToControllers(system.Panels, system.Controllers, guidDictionary);
        }

        //Was LinkCharacteristicInstances()
        public static void LinkTypicalInstanceDictionary(ObservableItemToInstanceList<TECScope> oldDictionary, TECSystem newTypical)
        {
            ObservableItemToInstanceList<TECScope> newCharacteristicInstances = new ObservableItemToInstanceList<TECScope>();
            foreach (TECSystem instance in newTypical.SystemInstances)
            {
                linkCharacteristicCollections(newTypical.Equipment, instance.Equipment, oldDictionary, newCharacteristicInstances);
                foreach (TECEquipment equipment in newTypical.Equipment)
                {
                    foreach (TECEquipment instanceEquipment in instance.Equipment)
                    {
                        linkCharacteristicCollections(equipment.SubScope, instanceEquipment.SubScope, oldDictionary, newCharacteristicInstances);
                        foreach (TECSubScope subscope in equipment.SubScope)
                        {
                            foreach (TECSubScope instanceSubScope in instanceEquipment.SubScope)
                            {
                                linkCharacteristicCollections(subscope.Points, instanceSubScope.Points, oldDictionary, newCharacteristicInstances);
                            }
                        }
                    }
                }
                linkCharacteristicCollections(newTypical.Controllers, instance.Controllers, oldDictionary, newCharacteristicInstances);
                linkCharacteristicCollections(newTypical.Panels, instance.Panels, oldDictionary, newCharacteristicInstances);
            }
            newTypical.CharactersticInstances = newCharacteristicInstances;
        }

        #region public static void LinkScopeItem(TECScope scope, TECBid Bid)
        public static void LinkScopeItem(TECSystem scope, TECBid bid)
        {
            linkScopeChildrenToCatalogs(scope, bid.Catalogs);
            linkLocation(scope, bid.Locations);
            foreach (TECSystem instance in scope.SystemInstances)
            {
                LinkScopeItem(instance, bid);
            }
            foreach (TECEquipment equip in scope.Equipment)
            {
                LinkScopeItem(equip, bid);
            }
        }

        public static void LinkScopeItem(TECEquipment scope, TECBid bid)
        {
            linkScopeChildrenToCatalogs(scope, bid.Catalogs);
            linkLocation(scope, bid.Locations);
            foreach (TECSubScope ss in scope.SubScope)
            {
                LinkScopeItem(ss, bid);
            }
        }

        public static void LinkScopeItem(TECSubScope scope, TECBid bid)
        {
            linkScopeChildrenToCatalogs(scope, bid.Catalogs);
            linkLocation(scope, bid.Locations);
            foreach (TECPoint point in scope.Points)
            {
                LinkScopeItem(point, bid);
            }
            foreach (TECDevice dev in scope.Devices)
            {
                LinkScopeItem(dev, bid);
            }
        }

        public static void LinkScopeItem(TECScope scope, TECScopeManager manager)
        {
            linkScopeChildrenToCatalogs(scope, manager.Catalogs);
        }
        #endregion

        public static void LinkBidToCatalogs(TECBid bid)
        {
            linkCatalogs(bid.Catalogs);
            foreach(TECSystem typical in bid.Systems)
            {
                linkSystemToCatalogs(typical, bid.Catalogs);
            }
            foreach(TECController controller in bid.Controllers)
            {
                linkControllerToCatalogs(controller, bid.Catalogs);
            }
            foreach(TECPanel panel in bid.Panels)
            {
                linkPanelToCatalogs(panel, bid.Catalogs);
            }
        }
        #endregion

        #region Private Methods
        #region Catalog Linking
        private static void linkCatalogs(TECCatalogs catalogs)
        {
            foreach (TECDevice device in catalogs.Devices)
            {
                linkDeviceToCatalogs(device, catalogs);
            }
            foreach (TECIOModule module in catalogs.IOModules)
            {
                linkModuleToManufacturer(module, catalogs.Manufacturers);
            }
            foreach (TECConnectionType connectionType in catalogs.ConnectionTypes)
            {
                linkElectricalMaterialComponentToCatalogs(connectionType, catalogs);
            }
            foreach (TECConduitType conduitType in catalogs.ConduitTypes)
            {
                linkElectricalMaterialComponentToCatalogs(conduitType, catalogs);
            }
        }

        private static void linkSystemToCatalogs(TECSystem system, TECCatalogs catalogs)
        {
            //Should assume linking a typical system with potential instances, controllers and panels.

            linkScopeChildrenToCatalogs(system, catalogs);
            foreach(TECSystem instance in system.SystemInstances)
            {
                linkSystemToCatalogs(system, catalogs);
            }
            foreach(TECController controller in system.Controllers)
            {
                linkControllerToCatalogs(controller, catalogs);
            }
            foreach(TECPanel panel in system.Panels)
            {
                linkPanelToCatalogs(panel, catalogs);
            }
        }

        private static void linkEquipmentToCatalogs(TECEquipment equip, TECCatalogs catalogs)
        {
            throw new NotImplementedException();
        }

        private static void linkSubScopeToCatalogs(TECSubScope ss, TECCatalogs catalogs)
        {
            throw new NotImplementedException();
        }

        private static void linkDeviceToCatalogs(TECDevice dev, TECCatalogs catalogs)
        {
            linkDeviceToConnectionTypes(dev, catalogs.ConnectionTypes);
            linkDeviceToManufacturer(dev, catalogs.Manufacturers);
            linkScopeChildrenToCatalogs(dev, catalogs);
        }

        private static void linkControllerToCatalogs(TECController controller, TECCatalogs catalogs)
        {
            linkControllerToManufacturer(controller, catalogs.Manufacturers);
            foreach(TECIO io in controller.IO)
            {
                linkIOToModule(io, catalogs.IOModules);
            }
            foreach(TECConnection connection in controller.ChildrenConnections)
            {
                linkConnectionToCatalogs(connection, catalogs);
            }
        }

        private static void linkPanelToCatalogs(TECPanel panel, TECCatalogs catalogs)
        {
            throw new NotImplementedException();
        }

        private static void linkElectricalMaterialComponentToCatalogs(ElectricalMaterialComponent component, TECCatalogs catalogs)
        {
            linkScopeChildrenToCatalogs((component as TECScope), catalogs);
            linkElectricalMaterialComponentToRatedCosts(component, catalogs.AssociatedCosts);
        }

        private static void linkConnectionToCatalogs(TECConnection connection, TECCatalogs catalogs)
        {
            throw new NotImplementedException();
        }
        #endregion

        private static void linkPanelsToControllers(ObservableCollection<TECPanel> panels, ObservableCollection<TECController> controllers, Dictionary<Guid, Guid> guidDictionary = null)
        {
            throw new NotImplementedException();
        }

        private static void linkNetworkConnections(ObservableCollection<TECController> controllers, Dictionary<Guid, Guid> guidDictionary = null)
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

        private static void linkSubScopeConnections(ObservableCollection<TECController> controllers, ObservableCollection<TECSubScope> subscope, Dictionary<Guid, Guid> guidDictionary = null)
        {
            foreach (TECSubScope subScope in subscope)
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
                                subScope.LinkConnection(ssConnect);
                            }
                        }
                    }
                }
            }
        }

        private static void linkScopeChildrenToCatalogs(TECScope scope, TECCatalogs catalogs)
        {
            linkAssociatedCostsInScope(catalogs.AssociatedCosts, scope);
            linkTagsInScope(catalogs.Tags, scope);
        }

        private static void linkModuleToManufacturer(TECIOModule module, ObservableCollection<TECManufacturer> manufacturers)
        {
            foreach (TECManufacturer manufacturer in manufacturers)
            {
                if (module.Manufacturer.Guid == manufacturer.Guid)
                {
                    module.Manufacturer = manufacturer;
                }
            }
        }

        private static void linkDeviceToConnectionTypes(TECDevice device, ObservableCollection<TECConnectionType> connectionTypes)
        {
            ObservableCollection<TECConnectionType> linkedTypes = new ObservableCollection<TECConnectionType>();
            foreach (TECConnectionType deviceType in device.ConnectionTypes)
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

        private static void linkDeviceToManufacturer(TECDevice device, ObservableCollection<TECManufacturer> manufacturers)
        {
            foreach (TECManufacturer man in manufacturers)
            {
                if (device.Manufacturer.Guid == man.Guid)
                {
                    device.Manufacturer = man;
                }
            }
        }

        private static void linkElectricalMaterialComponentToRatedCosts(ElectricalMaterialComponent component, ObservableCollection<TECCost> costs)
        {
            ObservableCollection<TECCost> costsToAssign = new ObservableCollection<TECCost>();
            foreach (TECCost cost in costs)
            {
                foreach (TECCost scopeCost in component.RatedCosts)
                {
                    if (scopeCost.Guid == cost.Guid)
                    { costsToAssign.Add(cost); }
                }
            }
            component.RatedCosts = costsToAssign;
        }

        private static void linkControllerToManufacturer(TECController controller, ObservableCollection<TECManufacturer> manufacturers)
        {
            foreach (TECManufacturer manufacturer in manufacturers)
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

        private static void linkIOToModule(TECIO io, ObservableCollection<TECIOModule> modules)
        {
            if (io.IOModule != null)
            {
                foreach (TECIOModule module in modules)
                {
                    if (io.IOModule.Guid == module.Guid)
                    {
                        io.IOModule = module;
                        break;
                    }
                }
            }
        }

        #region Location Linking
        private static void linkLocation(TECSystem system, ObservableCollection<TECLocation> locations)
        {
            linkLocation(system as TECScope, locations);
            foreach (TECEquipment equip in system.Equipment)
            {
                linkLocation(equip, locations);
            }
        }

        private static void linkLocation(TECEquipment equipment, ObservableCollection<TECLocation> locations)
        {
            linkLocation(equipment as TECScope, locations);
            foreach (TECSubScope ss in equipment.SubScope)
            {
                linkLocation(ss, locations);
            }
        }

        static private void linkLocation(TECScope scope, ObservableCollection<TECLocation> locations)
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

        #endregion

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

        private static void createScopeDictionary(TECSystem typical, Dictionary<Guid, List<Guid>> guidDictionary)
        {
            throw new NotImplementedException();
        }

        #region Scope Children
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
        static private void linkTagsInScope(ObservableCollection<TECTag> tags, TECScope scope)
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
        #endregion
        #endregion

        //#region Link Methods

        //static private void linkAssociatedCostsInDevice(ObservableCollection<TECCost> costs, TECDevice device)
        //{
        //    ObservableCollection<TECCost> costsToAssign = new ObservableCollection<TECCost>();
        //    foreach (TECCost cost in costs)
        //    {
        //        foreach (TECCost devCost in device.AssociatedCosts)
        //        {
        //            if (devCost.Guid == cost.Guid)
        //            { costsToAssign.Add(cost); }
        //        }
        //    }
        //    device.AssociatedCosts = costsToAssign;
        //}
        //#endregion



        //static private void linkSystems(ObservableCollection<TECSystem> systems, TECScopeManager scopeManager)
        //{
        //    foreach(TECSystem system in systems)
        //    {
        //        LinkSystem(system, scopeManager);
        //    }
        //}
        //static private void linkScopeChildren(TECScope scope, TECScopeManager scopeManager)
        //{
        //    linkTags(scopeManager.Catalogs.Tags, scope);
        //    linkAssociatedCostsInScope(scopeManager.Catalogs.AssociatedCosts, scope);
        //    if (scopeManager is TECBid)
        //    {
        //        linkLocationsInScope((scopeManager as TECBid).Locations, scope);
        //    }
        //}
        //static private void linkScopeChildrenInEquipment(TECEquipment equipment, TECScopeManager scopeManager)
        //{
        //    linkScopeChildren(equipment, scopeManager);
        //    foreach (TECSubScope ss in equipment.SubScope)
        //    {
        //        linkScopeChildren(ss, scopeManager);
        //    }
        //}

        //static private void linkAllVisualScope(ObservableCollection<TECDrawing> bidDrawings, ObservableCollection<TECSystem> bidSystems, ObservableCollection<TECController> bidControllers)
        //{
        //    //This function links visual scope with scope in Systems, Equipment, SubScope and Devices if they have the same GUID.
        //    foreach (TECDrawing drawing in bidDrawings)
        //    {
        //        foreach (TECPage page in drawing.Pages)
        //        {
        //            foreach (TECVisualScope vs in page.PageScope)
        //            {
        //                foreach (TECSystem system in bidSystems)
        //                {
        //                    if (vs.Scope.Guid == system.Guid)
        //                    {
        //                        vs.Scope = system;
        //                        break;
        //                    }
        //                    else
        //                    {
        //                        foreach (TECEquipment equipment in system.Equipment)
        //                        {
        //                            if (vs.Scope.Guid == equipment.Guid)
        //                            {
        //                                vs.Scope = equipment;
        //                                break;
        //                            }
        //                            else
        //                            {
        //                                foreach (TECSubScope subScope in equipment.SubScope)
        //                                {
        //                                    if (vs.Scope.Guid == subScope.Guid)
        //                                    {
        //                                        vs.Scope = subScope;
        //                                        break;
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }

        //                }
        //                foreach (TECController controller in bidControllers)
        //                {
        //                    if (vs.Scope.Guid == controller.Guid)
        //                    {
        //                        vs.Scope = controller;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}
        //static private void linkAllLocations(ObservableCollection<TECLocation> locations, ObservableCollection<TECSystem> bidSystems)
        //{
        //    foreach (TECLocation location in locations)
        //    {
        //        foreach (TECSystem system in bidSystems)
        //        {
        //            if (system.Location != null && system.Location.Guid == location.Guid)
        //            { system.Location = location; }
        //            foreach (TECEquipment equipment in system.Equipment)
        //            {
        //                if (equipment.Location != null && equipment.Location.Guid == location.Guid)
        //                { equipment.Location = location; }
        //                foreach (TECSubScope subScope in equipment.SubScope)
        //                {
        //                    if (subScope.Location != null && subScope.Location.Guid == location.Guid)
        //                    { subScope.Location = location; }
        //                }
        //            }
        //        }
        //    }
        //}
        

        //static private void linkAllConnections(ObservableCollection<TECController> controllers, ObservableCollection<TECSystem> systems)
        //{
        //    ObservableCollection<TECController> allControllers = new ObservableCollection<TECController>();
        //    foreach(TECController controller in controllers)
        //    {
        //        allControllers.Add(controller);
        //    }

        //    foreach (TECSystem system in systems)
        //    {
        //        linkConnections(controllers, system.Equipment);
        //        foreach(TECSystem instance in system.SystemInstances)
        //        {
        //            linkConnections(controllers, instance.Equipment);
        //            foreach(TECController controller in instance.Controllers)
        //            {
        //                allControllers.Add(controller);
        //            }
        //        }
        //    }
        //    linkNetworkConnections(allControllers);
        //}

        //static private void linkAllDevices(ObservableCollection<TECSystem> bidSystems, ObservableCollection<TECDevice> deviceCatalog)
        //{
        //    foreach (TECSystem system in bidSystems)
        //    {
        //        foreach (TECEquipment equipment in system.Equipment)
        //        {
        //            foreach (TECSubScope sub in equipment.SubScope)
        //            {
        //                var linkedDevices = new ObservableCollection<TECDevice>();
        //                foreach (TECDevice device in sub.Devices)
        //                {
        //                    foreach (TECDevice cDev in deviceCatalog)
        //                    {
        //                        if (cDev.Guid == device.Guid)
        //                        { linkedDevices.Add(cDev); }
        //                    }
        //                }
        //                sub.Devices = linkedDevices;
        //            }
        //        }
        //    }
        //}
        //static private void linkAllDevicesFromSystems(ObservableCollection<TECSystem> systems, ObservableCollection<TECDevice> deviceCatalog)
        //{
        //    foreach (TECSystem system in systems)
        //    { linkAllDevicesFromEquipment(system.Equipment, deviceCatalog); }
        //}
        //static private void linkAllDevicesFromEquipment(ObservableCollection<TECEquipment> equipment, ObservableCollection<TECDevice> deviceCatalog)
        //{
        //    foreach (TECEquipment equip in equipment)
        //    { linkAllDevicesFromSubScope(equip.SubScope, deviceCatalog); }
        //}
        //static private void linkAllDevicesFromSubScope(ObservableCollection<TECSubScope> subScope, ObservableCollection<TECDevice> deviceCatalog)
        //{
        //    foreach (TECSubScope sub in subScope)
        //    {
        //        var linkedDevices = new ObservableCollection<TECDevice>();
        //        foreach (TECDevice device in sub.Devices)
        //        {
        //            foreach (TECDevice cDev in deviceCatalog)
        //            {
        //                if (cDev.Guid == device.Guid)
        //                { linkedDevices.Add(cDev); }
        //            }
        //        }
        //        sub.Devices = linkedDevices;
        //    }
        //}

        //static private void linkAllConnectionTypes(ObservableCollection<TECController> controllers, ObservableCollection<TECConnectionType> connectionTypes)
        //{
        //    foreach (TECController controller in controllers)
        //    {
        //        foreach (TECConnection connection in controller.ChildrenConnections)
        //        {
        //            if (connection is TECNetworkConnection)
        //            {
        //                TECNetworkConnection netConnect = connection as TECNetworkConnection;
        //                foreach (TECConnectionType type in connectionTypes)
        //                {
        //                    if (netConnect.ConnectionType.Guid == type.Guid)
        //                    {
        //                        netConnect.ConnectionType = type;
        //                        break;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        //static private void linkTagsInBid(ObservableCollection<TECTag> tags, TECBid bid)
        //{
        //    foreach (TECSystem system in bid.Systems)
        //    {
        //        linkTags(tags, system);
        //        foreach (TECEquipment equipment in system.Equipment)
        //        {
        //            linkTags(tags, equipment);
        //            foreach (TECSubScope subScope in equipment.SubScope)
        //            {
        //                linkTags(tags, subScope);
        //                foreach (TECDevice device in subScope.Devices)
        //                { linkTags(tags, device); }
        //                foreach (TECPoint point in subScope.Points)
        //                { linkTags(tags, point); }
        //            }
        //        }
        //    }
        //    foreach (TECController controller in bid.Controllers)
        //    { linkTags(tags, controller); }
        //    foreach (TECDevice device in bid.Catalogs.Devices)
        //    { linkTags(tags, device); }
        //}
        //static private void linkTagsInTemplates(ObservableCollection<TECTag> tags, TECTemplates templates)
        //{
        //    foreach (TECSystem system in templates.SystemTemplates)
        //    {
        //        linkTags(tags, system);
        //        foreach (TECEquipment equipment in system.Equipment)
        //        {
        //            linkTags(tags, equipment);
        //            foreach (TECSubScope subScope in equipment.SubScope)
        //            {
        //                linkTags(tags, subScope);
        //                foreach (TECDevice device in subScope.Devices)
        //                { linkTags(tags, device); }
        //                foreach (TECPoint point in subScope.Points)
        //                { linkTags(tags, point); }
        //            }
        //        }
        //    }
        //    foreach (TECEquipment equipment in templates.EquipmentTemplates)
        //    {
        //        linkTags(tags, equipment);
        //        foreach (TECSubScope subScope in equipment.SubScope)
        //        {
        //            linkTags(tags, subScope);
        //            foreach (TECDevice device in subScope.Devices)
        //            { linkTags(tags, device); }
        //            foreach (TECPoint point in subScope.Points)
        //            { linkTags(tags, point); }
        //        }
        //    }
        //    foreach (TECSubScope subScope in templates.SubScopeTemplates)
        //    {
        //        linkTags(tags, subScope);
        //        foreach (TECDevice device in subScope.Devices)
        //        { linkTags(tags, device); }
        //        foreach (TECPoint point in subScope.Points)
        //        { linkTags(tags, point); }
        //    }
        //    foreach (TECController controller in templates.ControllerTemplates)
        //    { linkTags(tags, controller); }
        //    foreach (TECDevice device in templates.Catalogs.Devices)
        //    { linkTags(tags, device); }
        //}


        //static private void linkAssociatedCostsWithScope(TECScopeManager scopeManager)
        //{
        //    if (scopeManager is TECBid)
        //    {
        //        TECBid bid = scopeManager as TECBid;
        //        foreach (TECSystem system in bid.Systems)
        //        { linkAssociatedCostsInSystem(bid.Catalogs.AssociatedCosts, system); }
        //        foreach (TECController scope in bid.Controllers)
        //        { linkAssociatedCostsInScope(bid.Catalogs.AssociatedCosts, scope); }
        //        foreach (TECPanel scope in bid.Panels)
        //        { linkAssociatedCostsInScope(bid.Catalogs.AssociatedCosts, scope); }

        //    }
        //    else if (scopeManager is TECTemplates)
        //    {
        //        TECTemplates templates = scopeManager as TECTemplates;
        //        foreach (TECSystem system in templates.SystemTemplates)
        //        { linkAssociatedCostsInSystem(templates.Catalogs.AssociatedCosts, system); }
        //        foreach (TECEquipment equipment in templates.EquipmentTemplates)
        //        { linkAssociatedCostsInEquipment(templates.Catalogs.AssociatedCosts, equipment); }
        //        foreach (TECSubScope subScope in templates.SubScopeTemplates)
        //        { linkAssociatedCostsInSubScope(templates.Catalogs.AssociatedCosts, subScope); }
        //        foreach (TECController scope in templates.ControllerTemplates)
        //        { linkAssociatedCostsInScope(templates.Catalogs.AssociatedCosts, scope); }
        //        foreach (TECPanel scope in templates.PanelTemplates)
        //        { linkAssociatedCostsInScope(templates.Catalogs.AssociatedCosts, scope); }
        //    }
        //}
        //static private void linkAssociatedCostsInSubScope(ObservableCollection<TECCost> costs, TECSubScope subScope)
        //{
        //    ObservableCollection<TECCost> costsToAssign = new ObservableCollection<TECCost>();
        //    foreach (TECCost cost in costs)
        //    {
        //        foreach (TECCost childCost in subScope.AssociatedCosts)
        //        {
        //            if (childCost.Guid == cost.Guid)
        //            { costsToAssign.Add(cost); }
        //        }
        //    }
        //    subScope.AssociatedCosts = costsToAssign;
        //    foreach (TECDevice device in subScope.Devices)
        //    { linkAssociatedCostsInDevice(costs, device); }
        //}
        //static private void linkAssociatedCostsInEquipment(ObservableCollection<TECCost> costs, TECEquipment equipment)
        //{
        //    ObservableCollection<TECCost> costsToAssign = new ObservableCollection<TECCost>();
        //    foreach (TECCost cost in costs)
        //    {
        //        foreach (TECCost childCost in equipment.AssociatedCosts)
        //        {
        //            if (childCost.Guid == cost.Guid)
        //            { costsToAssign.Add(cost); }
        //        }
        //    }
        //    equipment.AssociatedCosts = costsToAssign;
        //    foreach (TECSubScope subScope in equipment.SubScope)
        //    { linkAssociatedCostsInSubScope(costs, subScope); }
        //}
        //static private void linkAssociatedCostsInSystem(ObservableCollection<TECCost> costs, TECSystem system)
        //{
        //    ObservableCollection<TECCost> costsToAssign = new ObservableCollection<TECCost>();
        //    foreach (TECCost cost in costs)
        //    {
        //        foreach (TECCost childCost in system.AssociatedCosts)
        //        {
        //            if (childCost.Guid == cost.Guid)
        //            { costsToAssign.Add(cost); }
        //        }
        //    }
        //    system.AssociatedCosts = costsToAssign;
        //    foreach (TECEquipment equipment in system.Equipment)
        //    { linkAssociatedCostsInEquipment(costs, equipment); }
        //}
        //static private void linkAssociatedCostsInScope(ObservableCollection<TECCost> costs, TECScope scope)
        //{
        //    ObservableCollection<TECCost> costsToAssign = new ObservableCollection<TECCost>();
        //    foreach (TECCost cost in costs)
        //    {
        //        foreach (TECCost scopeCost in scope.AssociatedCosts)
        //        {
        //            if (scopeCost.Guid == cost.Guid)
        //            { costsToAssign.Add(cost); }
        //        }
        //    }
        //    scope.AssociatedCosts = costsToAssign;
        //}

        //static private void linkConduitTypeWithConnections(ObservableCollection<TECConduitType> conduitTypes, ObservableCollection<TECController> controllers)
        //{
        //    foreach (TECController controller in controllers)
        //    {
        //        foreach (TECConnection connection in controller.ChildrenConnections)
        //        {
        //            if (connection.ConduitType != null)
        //            {
        //                foreach (TECConduitType conduitType in conduitTypes)
        //                {
        //                    if (connection.ConduitType.Guid == conduitType.Guid)
        //                    {
        //                        connection.ConduitType = conduitType;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}
        //static private void linkPanelTypesInPanel(ObservableCollection<TECPanelType> panelTypes, ObservableCollection<TECPanel> panels)
        //{
        //    foreach (TECPanel panel in panels)
        //    {
        //        foreach (TECPanelType type in panelTypes)
        //        {
        //            if (panel.Type != null)
        //            {
        //                if (panel.Type.Guid == type.Guid)
        //                {
        //                    panel.Type = type;
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //}
        //static private void linkControllersInPanels(ObservableCollection<TECController> controllers, ObservableCollection<TECPanel> panels, Dictionary<Guid, Guid> guidDictionary = null)
        //{
        //    foreach (TECPanel panel in panels)
        //    {
        //        ObservableCollection<TECController> controllersToLink = new ObservableCollection<TECController>();
        //        foreach (TECController panelController in panel.Controllers)
        //        {
        //            foreach (TECController controller in controllers)
        //            {
        //                if (panelController.Guid == controller.Guid)
        //                {
        //                    controllersToLink.Add(controller);
        //                    break;
        //                }
        //                else if (guidDictionary != null && guidDictionary[panelController.Guid] == guidDictionary[controller.Guid])
        //                {
        //                    controllersToLink.Add(controller);
        //                    break;
        //                }
        //            }
        //        }
        //        panel.Controllers = controllersToLink;
        //    }
        //}


        //private static void linkControlledScopeWithInstances(TECBid bid, Dictionary<Guid, List<Guid>> placeholderDict)
        //{
        //    foreach (TECSystem system in bid.Systems)
        //    {
        //        foreach(TECSystem instance in system.SystemInstances)
        //        {
        //            foreach(TECEquipment equipment in system.Equipment)
        //            {
        //                linkCharacteristicWithInstances(equipment, instance.Equipment, placeholderDict, system.CharactersticInstances);
        //                foreach(TECSubScope subscope in equipment.SubScope)
        //                {
        //                    foreach(TECEquipment instanceEquipment in instance.Equipment)
        //                    {
        //                        linkCharacteristicWithInstances(subscope, instanceEquipment.SubScope, placeholderDict, system.CharactersticInstances);
        //                        foreach(TECPoint point in subscope.Points)
        //                        {
        //                            foreach(TECSubScope instanceSubScope in instanceEquipment.SubScope)
        //                            {
        //                                linkCharacteristicWithInstances(point, instanceSubScope.Points, placeholderDict, system.CharactersticInstances);
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            foreach(TECController controller in system.Controllers)
        //            {
        //                linkCharacteristicWithInstances(controller, instance.Controllers, placeholderDict, system.CharactersticInstances);
        //            }
        //            foreach(TECPanel panel in system.Panels)
        //            {
        //                linkCharacteristicWithInstances(panel, instance.Panels, placeholderDict, system.CharactersticInstances);
        //            }
        //        }
        //    }
        //}
        //private static void linkCharacteristicWithInstances(TECScope characteristic, IList instances, Dictionary<Guid, List<Guid>> referenceDict, ObservableItemToInstanceList<TECScope> characteristicList)
        //{
        //    foreach(TECScope item in instances)
        //    {
        //        if (referenceDict[characteristic.Guid].Contains(item.Guid))
        //        {
        //            characteristicList.AddItem(characteristic, item);
        //        }
        //    }
        //}
        //#endregion Link Methods

    }
}
