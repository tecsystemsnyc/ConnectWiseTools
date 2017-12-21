using EstimatingLibrary.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;

namespace EstimatingLibrary.Utilities
{
    public static class ModelLinkingHelper
    {
        #region Public Methods
        public static bool LinkBid(TECBid bid, Dictionary<Guid, List<Guid>> guidDictionary)
        {
            bool needsSave = false;

            ObservableCollection<TECController> allControllers = new ObservableCollection<TECController>();
            ObservableCollection<TECSubScope> allSubScope = new ObservableCollection<TECSubScope>();

            linkCatalogs(bid.Catalogs);

            foreach (TECTypical typical in bid.Systems)
            {
                foreach (TECController controller in typical.Controllers)
                {
                    allControllers.Add(controller);
                }
                foreach (TECSystem instance in typical.Instances)
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
                foreach (TECEquipment equip in typical.Equipment)
                {
                    foreach (TECSubScope ss in equip.SubScope)
                    {
                        allSubScope.Add(ss);
                    }
                }

                linkSystemToCatalogs(typical, bid.Catalogs);
                linkLocation(typical, bid.Locations);
                linkPanelsToControllers(typical.Panels, typical.Controllers);

                createScopeDictionary(typical, guidDictionary);

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
            List<INetworkConnectable> allChildren = new List<INetworkConnectable>();
            allChildren.AddRange(allControllers);
            allChildren.AddRange(allSubScope);
            linkNetworkConnections(allControllers, allChildren);
            linkSubScopeConnections(allControllers, allSubScope);
            linkPanelsToControllers(bid.Panels, bid.Controllers);

            foreach(TECController controller in allControllers)
            {
                bool controllerNeedsSave = addRequiredIOModules(controller);
                if (controllerNeedsSave)
                {
                    needsSave = true;
                }
            }

            foreach(TECTypical system in bid.Systems)
            {
                system.RefreshRegistration();
            }
            return needsSave;
        }

        public static bool LinkTemplates(TECTemplates templates, Dictionary<Guid, List<Guid>> templateReferences)
        {
            bool needsSave = false;

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

            linkTemplateReferences(templates, templateReferences);

            return needsSave;
        }
        
        public static void LinkSystem(TECSystem system, TECScopeManager scopeManager, Dictionary<Guid, Guid> guidDictionary)
        {
            linkSystemToCatalogs(system, scopeManager.Catalogs);
            linkSubScopeConnections(system.Controllers, system.GetAllSubScope(), guidDictionary);
            linkNetworkConnections(system.Controllers, system.GetAllSubScope(), guidDictionary);
            linkPanelsToControllers(system.Panels, system.Controllers, guidDictionary);
            if(system is TECTypical typical)
            {
                typical.RefreshRegistration();
            }
        }

        //Was LinkCharacteristicInstances()
        public static void LinkTypicalInstanceDictionary(ObservableListDictionary<TECObject> oldDictionary, TECTypical newTypical)
        {
            ObservableListDictionary<TECObject> newCharacteristicInstances = new ObservableListDictionary<TECObject>();
            foreach (TECSystem instance in newTypical.Instances)
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
                linkCharacteristicCollections(newTypical.MiscCosts, instance.MiscCosts, oldDictionary, newCharacteristicInstances);
                linkCharacteristicCollections(newTypical.ScopeBranches, instance.ScopeBranches, oldDictionary, newCharacteristicInstances);
            }
            newTypical.TypicalInstanceDictionary = newCharacteristicInstances;
        }

        #region public static void LinkScopeItem(TECScope scope, TECBid Bid)
        public static void LinkScopeItem(TECSystem scope, TECBid bid)
        {
            linkScopeChildrenToCatalogs(scope, bid.Catalogs);
            linkLocation(scope, bid.Locations);
            if(scope is TECTypical typical)
            {
                foreach (TECSystem instance in typical.Instances)
                {
                    LinkScopeItem(instance, bid);
                }
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
                linkHardwareToManufacturers(module, catalogs.Manufacturers);
            }
            foreach (TECElectricalMaterial connectionType in catalogs.ConnectionTypes)
            {
                linkElectricalMaterialComponentToCatalogs(connectionType, catalogs);
            }
            foreach (TECElectricalMaterial conduitType in catalogs.ConduitTypes)
            {
                linkElectricalMaterialComponentToCatalogs(conduitType, catalogs);
            }
            foreach(TECPanelType panelType in catalogs.PanelTypes)
            {
                linkHardwareToManufacturers(panelType, catalogs.Manufacturers);
                linkScopeChildrenToCatalogs(panelType, catalogs);
            }
            foreach(TECControllerType controllerType in catalogs.ControllerTypes)
            {
                linkControllerTypeToCatalogs(controllerType, catalogs);
            }
            foreach(TECValve valve in catalogs.Valves)
            {
                linkValveToCatalogs(valve, catalogs);
            }
        }

        private static void linkControllerTypeToCatalogs(TECControllerType controllerType, TECCatalogs catalogs)
        {
            linkHardwareToManufacturers(controllerType, catalogs.Manufacturers);
            linkScopeChildrenToCatalogs(controllerType, catalogs);
            linkControllerTypeToIOModules(controllerType, catalogs.IOModules);
        }

        private static void linkControllerTypeToIOModules(TECControllerType controllerType, ObservableCollection<TECIOModule> iOModules)
        {
            ObservableCollection<TECIOModule> modules = new ObservableCollection<TECIOModule>();
            foreach(TECIOModule module in controllerType.IOModules)
            {
                foreach (TECIOModule item in iOModules)
                {
                    if (module.Guid == item.Guid)
                    {
                        modules.Add(item);
                        break;
                    }
                }
            }
            controllerType.IOModules = modules;
        }

        private static void linkControllerTypeToIOModules(TECController controller, ObservableCollection<TECIOModule> iOModules)
        {
            ObservableCollection<TECIOModule> modules = new ObservableCollection<TECIOModule>();
            foreach (TECIOModule module in controller.IOModules)
            {
                foreach (TECIOModule item in iOModules)
                {
                    if (module.Guid == item.Guid)
                    {
                        modules.Add(item);
                        break;
                    }
                }
            }
            controller.IOModules = modules;
        }

        private static void linkSystemToCatalogs(TECSystem system, TECCatalogs catalogs)
        {
            //Should assume linking a typical system with potential instances, controllers and panels.

            linkScopeChildrenToCatalogs(system, catalogs);
            if(system is TECTypical typical)
            {
                foreach (TECSystem instance in typical.Instances)
                {
                    linkSystemToCatalogs(instance, catalogs);
                }
            }
            
            foreach(TECController controller in system.Controllers)
            {
                linkControllerToCatalogs(controller, catalogs);
            }
            foreach(TECPanel panel in system.Panels)
            {
                linkPanelToCatalogs(panel, catalogs);
            }
            foreach(TECEquipment equip in system.Equipment)
            {
                linkEquipmentToCatalogs(equip, catalogs);
            }
        }

        private static void linkEquipmentToCatalogs(TECEquipment equip, TECCatalogs catalogs)
        {
            linkScopeChildrenToCatalogs(equip, catalogs);
            foreach (TECSubScope subScope in equip.SubScope)
            {
                linkSubScopeToCatalogs(subScope, catalogs);
            }
        }

        private static void linkSubScopeToCatalogs(TECSubScope ss, TECCatalogs catalogs)
        {
            linkScopeChildrenToCatalogs(ss, catalogs);
            linkSubScopeToDevices(ss, catalogs.Devices, catalogs.Valves);
        }

        private static void linkDeviceToCatalogs(TECDevice dev, TECCatalogs catalogs)
        {
            linkDeviceToConnectionTypes(dev, catalogs.ConnectionTypes);
            linkHardwareToManufacturers(dev, catalogs.Manufacturers);
            linkScopeChildrenToCatalogs(dev, catalogs);
        }

        private static void linkValveToCatalogs(TECValve valve, TECCatalogs catalogs)
        {
            linkHardwareToManufacturers(valve, catalogs.Manufacturers);
            linkScopeChildrenToCatalogs(valve, catalogs);
            linkValveToActuators(valve, catalogs.Devices);
        }

        private static void linkControllerToCatalogs(TECController controller, TECCatalogs catalogs)
        {
            linkControllerToControllerType(controller, catalogs.ControllerTypes);
            foreach(TECConnection connection in controller.ChildrenConnections)
            {
                linkConnectionToCatalogs(connection, catalogs);
            }
            linkScopeChildrenToCatalogs(controller, catalogs);
            linkControllerTypeToIOModules(controller, catalogs.IOModules);
        }

        private static void linkPanelToCatalogs(TECPanel panel, TECCatalogs catalogs)
        {
            linkPanelToPanelType(panel, catalogs.PanelTypes);
            linkScopeChildrenToCatalogs(panel, catalogs);
        }

        private static void linkElectricalMaterialComponentToCatalogs(TECElectricalMaterial component, TECCatalogs catalogs)
        {
            linkScopeChildrenToCatalogs((component as TECScope), catalogs);
            linkElectricalMaterialToRatedCosts(component, catalogs.AssociatedCosts);
        }

        private static void linkConnectionToCatalogs(TECConnection connection, TECCatalogs catalogs)
        {
            linkConnectionToConduitType(connection, catalogs.ConduitTypes);
            TECNetworkConnection netConnect = connection as TECNetworkConnection;if (netConnect != null)
            {
                linkNetworkConnectionToConnectionType(netConnect, catalogs.ConnectionTypes);
            }
        }

        
        #endregion

        private static void linkPanelsToControllers(ObservableCollection<TECPanel> panels, IEnumerable<TECController> controllers, Dictionary<Guid, Guid> guidDictionary = null)
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

        private static void linkNetworkConnections(IEnumerable<TECController> controllers, IEnumerable<INetworkConnectable> children,
            Dictionary<Guid, Guid> guidDictionary = null)
        {
            foreach (TECController controller in controllers)
            {
                foreach (TECConnection connection in controller.ChildrenConnections)
                {
                    if (connection is TECNetworkConnection)
                    {
                        TECNetworkConnection netConnect = connection as TECNetworkConnection;
                        ObservableCollection<INetworkConnectable> controllersToAdd = new ObservableCollection<INetworkConnectable>();
                        foreach (INetworkConnectable child in netConnect.Children)
                        {
                            foreach (INetworkConnectable item in children)
                            {
                                bool isCopy = (guidDictionary != null && guidDictionary[child.Guid] == guidDictionary[item.Guid]);
                                if (child.Guid == item.Guid || isCopy)
                                {
                                    controllersToAdd.Add(item);
                                    item.ParentConnection = netConnect;
                                }
                            }
                        }
                        netConnect.Children = controllersToAdd;
                    }
                }
            }
        }

        private static void linkSubScopeConnections(IEnumerable<TECController> controllers, IEnumerable<TECSubScope> subscope,
            Dictionary<Guid, Guid> guidDictionary = null)
        {
            foreach (TECSubScope subScope in subscope)
            {
                foreach (TECController controller in controllers)
                {
                    List<TECSubScopeConnection> newConnections = new List<TECSubScopeConnection>();
                    List<TECSubScopeConnection> oldConnections = new List<TECSubScopeConnection>();
                    foreach (TECConnection connection in controller.ChildrenConnections)
                    {
                        
                        if (connection is TECSubScopeConnection)
                        {
                            TECSubScopeConnection ssConnect = connection as TECSubScopeConnection;
                            bool isCopy = (guidDictionary != null && guidDictionary[ssConnect.SubScope.Guid] == guidDictionary[subScope.Guid]);
                            if (ssConnect.SubScope.Guid == subScope.Guid || isCopy)
                            {
                                TECSubScopeConnection linkedConnection = new TECSubScopeConnection(ssConnect, subScope, subScope.IsTypical || controller.IsTypical);
                                subScope.Connection = linkedConnection;
                                newConnections.Add(linkedConnection);
                                oldConnections.Add(ssConnect);
                                linkedConnection.ParentController = controller;
                            }
                        }
                    }
                    foreach(TECSubScopeConnection conn in newConnections)
                    {
                        controller.ChildrenConnections.Add(conn);
                    }
                    foreach(TECSubScopeConnection conn in oldConnections)
                    {
                        controller.ChildrenConnections.Remove(conn);
                    }
                }
            }
        }

        private static void linkScopeChildrenToCatalogs(TECScope scope, TECCatalogs catalogs)
        {
            linkAssociatedCostsInScope(catalogs.AssociatedCosts, scope);
            linkTagsInScope(catalogs.Tags, scope);
        }

        private static void linkDeviceToConnectionTypes(TECDevice device, ObservableCollection<TECConnectionType> connectionTypes)
        {
            ObservableCollection<TECConnectionType> linkedTypes = new ObservableCollection<TECConnectionType>();
            foreach (TECConnectionType deviceType in device.ConnectionTypes)
            {
                bool found = false;
                foreach (TECConnectionType connectionType in connectionTypes)
                {
                    if (deviceType.Guid == connectionType.Guid)
                    {
                        linkedTypes.Add(connectionType);
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    throw new Exception("Device connection type not found.");
                }
            }
            device.ConnectionTypes = linkedTypes;
        }

        private static void linkHardwareToManufacturers(TECHardware hardware, ObservableCollection<TECManufacturer> manufacturers)
        {
            foreach (TECManufacturer man in manufacturers)
            {
                if (hardware.Manufacturer.Guid == man.Guid)
                {
                    hardware.Manufacturer = man;
                    return;
                }
            }
        }

        private static void linkValveToActuators(TECValve valve, IEnumerable<TECDevice> devices)
        {
            foreach (TECDevice device in devices)
            {
                if (valve.Actuator.Guid == device.Guid)
                {
                    valve.Actuator = device;
                    return;
                }
            }
        }

        private static void linkElectricalMaterialToRatedCosts(TECElectricalMaterial component, ObservableCollection<TECCost> costs)
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
        
        private static void linkConnectionToConduitType(TECConnection connection, ObservableCollection<TECElectricalMaterial> conduitTypes)
        {
            if (connection.ConduitType != null)
            {
                foreach (TECElectricalMaterial type in conduitTypes)
                {
                    if (connection.ConduitType.Guid == type.Guid)
                    {
                        connection.ConduitType = type;
                        return;
                    }
                }
            }
        }

        private static void linkNetworkConnectionToConnectionType(TECNetworkConnection netConnect, ObservableCollection<TECConnectionType> connectionTypes)
        {
            ObservableCollection<TECConnectionType> linkedTypes = new ObservableCollection<TECConnectionType>();
            foreach (TECConnectionType type in connectionTypes)
            {
                foreach(TECConnectionType connType in netConnect.ConnectionTypes)
                {
                    if (connType.Guid == type.Guid)
                    {
                        linkedTypes.Add(type);
                    }
                }
            }
            netConnect.ConnectionTypes = linkedTypes;
        }

        private static void linkPanelToPanelType(TECPanel panel, ObservableCollection<TECPanelType> panelTypes)
        {
            foreach(TECPanelType type in panelTypes)
            {
                if (panel.Type.Guid == type.Guid)
                {
                    panel.Type = type;
                    return;
                }
            }
        }

        private static void linkControllerToControllerType(TECController controller, IEnumerable<TECControllerType> controllerTypes)
        {
            foreach (TECControllerType type in controllerTypes)
            {
                if (controller.Type.Guid == type.Guid)
                {
                    controller.Type = type;
                    return;
                }
            }
        }
        
        private static void linkSubScopeToDevices(TECSubScope subScope, IEnumerable<TECDevice> devices, IEnumerable<TECValve> valves)
        {
            ObservableCollection<IEndDevice> replacements = new ObservableCollection<IEndDevice>();
            foreach (IEndDevice item in subScope.Devices)
            {
                bool found = false;
                foreach (TECDevice catalogDevice in devices)
                {
                    if (item.Guid == catalogDevice.Guid)
                    {
                        replacements.Add(catalogDevice);
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    foreach (TECValve catalogValve in valves)
                    {
                        if (item.Guid == catalogValve.Guid)
                        {
                            replacements.Add(catalogValve);
                            found = true;
                            break;
                        }
                    }
                }
                if (!found)
                {
                    throw new Exception("Subscope device not found.");
                }
               

            }
            subScope.Devices = replacements;
        }


        private static void linkTemplateReferences(TECTemplates templates, Dictionary<Guid, List<Guid>> templateReferences)
        {
            List<TECSubScope> allSubScope = new List<TECSubScope>();
            List<TECEquipment> allEquipment = new List<TECEquipment>();
            foreach(TECEquipment equipment in templates.EquipmentTemplates)
            {
                allSubScope.AddRange(equipment.SubScope);
            }
            foreach(TECSystem system in templates.SystemTemplates)
            {
                allEquipment.AddRange(system.Equipment);
                allSubScope.AddRange(system.GetAllSubScope());
            }
            foreach(TECSubScope template in templates.SubScopeTemplates)
            {
                List<TECSubScope> references = findReferences(template, allSubScope, templateReferences);
                if(references.Count > 0)
                {
                    templates.SubScopeSynchronizer.LinkExisting(template, references);
                }
            }
            foreach(TECEquipment template in templates.EquipmentTemplates)
            {
                List<TECEquipment> references = findReferences(template, allEquipment, templateReferences);
                if (references.Count > 0)
                {
                    templates.EquipmentSynchronizer.LinkExisting(template, references);
                }
                foreach(TECSubScope subScope in template.SubScope)
                {
                    List<TECSubScope> subReferences = findReferences(subScope, allSubScope, templateReferences);
                    if(subReferences.Count > 0)
                    {
                        templates.SubScopeSynchronizer.LinkExisting(subScope, subReferences);
                    }
                }
            }

        }
        private static List<T> findReferences<T>(T template, IEnumerable<T> referenceList, Dictionary<Guid, List<Guid>> templateReferences) where T : TECObject
        {
            List<T> references = new List<T>();
            foreach (T item in referenceList)
            {
                if (templateReferences.ContainsKey(template.Guid) && templateReferences[template.Guid].Contains(item.Guid))
                {
                    references.Add(item);
                }
            }
            return references;
        }

        #region Location Linking
        private static void linkLocation(TECSystem system, ObservableCollection<TECLabeled> locations)
        {
            linkLocation(system as TECLocated, locations);
            if(system is TECTypical typical)
            {
                foreach (TECSystem instance in typical.Instances)
                {
                    linkLocation(instance, locations);
                }
            }
            
            foreach (TECEquipment equip in system.Equipment)
            {
                linkLocation(equip, locations);
            }
        }

        private static void linkLocation(TECEquipment equipment, ObservableCollection<TECLabeled> locations)
        {
            linkLocation(equipment as TECLocated, locations);
            foreach (TECSubScope ss in equipment.SubScope)
            {
                linkLocation(ss, locations);
            }
        }

        static private void linkLocation(TECLocated scope, ObservableCollection<TECLabeled> locations)
        {
            if (scope.Location != null)
            {
                bool found = false;
                foreach (TECLabeled location in locations)
                {
                    if (scope.Location.Guid == location.Guid)
                    {
                        scope.Location = location;
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    throw new Exception("Location in scope not found.");
                }
            }
        }

        #endregion

        #region System Instance Reference Methods
        /// <summary>
        /// Rereferences the objects in a typical, instances scope dictionary after copying a typical system.
        /// </summary>
        /// <param name="characteristic">The typical items (equipment, panels, controllers) in the typical system</param>
        /// <param name="instances">The instances of those items in child system instance</param>
        /// <param name="oldCharacteristicInstances">A previosuly linked scope dictionary, from the original system before copying</param>
        /// <param name="newCharacteristicInstances">The scope dictionary that must be linked</param>
        static private void linkCharacteristicCollections(IList characteristic, IList instances,
            ObservableListDictionary<TECObject> oldCharacteristicInstances,
            ObservableListDictionary<TECObject> newCharacteristicInstances)
        {
            foreach (var item in oldCharacteristicInstances.GetFullDictionary())
            {
                foreach (TECObject charItem in characteristic)
                {
                    if (item.Key.Guid == charItem.Guid)
                    {
                        foreach (var sub in item.Value)
                        {
                            foreach (TECObject subInstance in instances)
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

        /// <summary>
        /// Creates the typical, instances scope dictionary of a system after loading.
        /// </summary>
        /// <param name="typical">The typical system</param>
        /// <param name="guidDictionary">The dictionary of typical to instances guids loaded</param>
        private static void createScopeDictionary(TECTypical typical, Dictionary<Guid, List<Guid>> guidDictionary)
        {
            if(guidDictionary.Count == 0)
            {
                return;
            }
            foreach (TECSystem instance in typical.Instances)
            {
                foreach (TECEquipment equipment in typical.Equipment)
                {
                    linkCharacteristicWithInstances(equipment, instance.Equipment, guidDictionary, typical.TypicalInstanceDictionary);
                    foreach (TECSubScope subscope in equipment.SubScope)
                    {
                        foreach (TECEquipment instanceEquipment in instance.Equipment)
                        {
                            linkCharacteristicWithInstances(subscope, instanceEquipment.SubScope, guidDictionary, typical.TypicalInstanceDictionary);
                            foreach (TECPoint point in subscope.Points)
                            {
                                foreach (TECSubScope instanceSubScope in instanceEquipment.SubScope)
                                {
                                    linkCharacteristicWithInstances(point, instanceSubScope.Points, guidDictionary, typical.TypicalInstanceDictionary);
                                }
                            }
                        }
                    }
                }
                foreach (TECController controller in typical.Controllers)
                {
                    linkCharacteristicWithInstances(controller, instance.Controllers, guidDictionary, typical.TypicalInstanceDictionary);
                }
                foreach (TECPanel panel in typical.Panels)
                {
                    linkCharacteristicWithInstances(panel, instance.Panels, guidDictionary, typical.TypicalInstanceDictionary);
                }
                foreach(TECMisc misc in typical.MiscCosts)
                {
                    linkCharacteristicWithInstances(misc, instance.MiscCosts, guidDictionary, typical.TypicalInstanceDictionary);
                }
                foreach(TECScopeBranch branch in typical.ScopeBranches)
                {
                    linkCharacteristicWithInstances(branch, instance.ScopeBranches, guidDictionary, typical.TypicalInstanceDictionary);
                }
            }
        }
        /// <summary>
        /// Generically references scope to instances of scope into a dictionary from a guid, guids dictionary
        /// </summary>
        /// <param name="characteristic"></param>
        /// <param name="instances"></param>
        /// <param name="referenceDict"></param>
        /// <param name="characteristicList"></param>
        private static void linkCharacteristicWithInstances(TECObject characteristic, IList instances,
            Dictionary<Guid, List<Guid>> referenceDict,
            ObservableListDictionary<TECObject> characteristicList)
        {
            foreach (TECObject item in instances)
            {
                if (referenceDict[characteristic.Guid].Contains(item.Guid))
                {
                    characteristicList.AddItem(characteristic, item);
                }
            }
        }
        #endregion

        #region Scope Children
        static private void linkAssociatedCostsInScope(ObservableCollection<TECCost> costs, TECScope scope)
        {
            ObservableCollection<TECCost> costsToAssign = new ObservableCollection<TECCost>();
            foreach (TECCost scopeCost in scope.AssociatedCosts)
            {
                bool found = false;
                foreach (TECCost catalogCost in costs)
                {
                    if (scopeCost.Guid == catalogCost.Guid)
                    {
                        costsToAssign.Add(catalogCost);
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    throw new Exception("Associated cost not found.");
                }
            }
            scope.AssociatedCosts = costsToAssign;
        }
        static private void linkTagsInScope(ObservableCollection<TECTag> tags, TECScope scope)
        {
            ObservableCollection<TECTag> linkedTags = new ObservableCollection<TECTag>();
            foreach (TECTag tag in scope.Tags)
            {
                bool found = false;
                foreach (TECTag referenceTag in tags)
                {
                    if (tag.Guid == referenceTag.Guid)
                    {
                        linkedTags.Add(referenceTag);
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    throw new Exception("Tag not found.");
                }
            }
            scope.Tags = linkedTags;
        }
        #endregion

        private static bool addRequiredIOModules(TECController controller)
        {
            //The IO needed by the points connected to the controller
            IOCollection necessaryIO = new IOCollection();
            bool needsSave = false;

            foreach (TECSubScopeConnection ssConnect in 
                controller.ChildrenConnections.Where(con => con is TECSubScopeConnection))
            {
                foreach (TECIO io in ssConnect.SubScope.IO.ListIO())
                {
                    for(int i = 0; i < io.Quantity; i++)
                    {
                        //The point IO that exists on our controller at the moment.
                        IOCollection totalPointIO = getPointIO(controller);
                        necessaryIO.AddIO(io.Type);
                        //Check if our io that exists satisfies the IO that we need.
                        if (!totalPointIO.Contains(necessaryIO))
                        {
                            needsSave = true;
                            bool moduleFound = false;
                            //If it doesn't, we need to add an IO module that will satisfy it.
                            foreach (TECIOModule module in controller.Type.IOModules)
                            {
                                //We only need to check for the type of the last IO that we added.
                                if (module.IOCollection().Contains(io.Type) && controller.CanAddModule(module))
                                {
                                    controller.AddModule(module);
                                    moduleFound = true;
                                    break;
                                }
                            }
                            if (!moduleFound)
                            {
                                controller.RemoveAllConnections();
                                MessageBox.Show(string.Format("The controller type of the controller '{0}' is incompatible with the connected points. Please review the controller's connections.",
                                                                    controller.Name));

                                return true;
                            }
                        }
                    }
                }
            }
            return needsSave;

            IOCollection getPointIO(TECController con)
            {
                IOCollection pointIOCollection = new IOCollection();
                foreach (TECIO pointIO in controller.TotalIO.ListIO().Where(io => (TECIO.PointIO.Contains(io.Type) || TECIO.UniversalIO.Contains(io.Type))))
                {
                    pointIOCollection.AddIO(pointIO);
                }
                return pointIOCollection;
            }
        }
        #endregion
    }
}
