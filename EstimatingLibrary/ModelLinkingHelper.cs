﻿using DebugLibrary;
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
            linkAllConnections(bid.Connections, bid.Controllers, bid.Systems);
            linkConnectionTypeWithDevices(bid.ConnectionTypes, bid.DeviceCatalog);
            linkAllDevices(bid.Systems, bid.DeviceCatalog);
            linkManufacturersWithDevices(bid.ManufacturerCatalog, bid.DeviceCatalog);
            linkTagsInBid(bid.Tags, bid);
            linkManufacturersWithControllers(bid.ManufacturerCatalog, bid.Controllers);
            linkAssociatedCostsWithScope(bid);
            linkConduitTypesInBid(bid);
            linkPanelTypesInPanel(bid.PanelTypeCatalog, bid.Panels);
            linkControllersInPanels(bid.Controllers, bid.Panels);
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
            linkConduitTypesInTemplates(templates);
            linkConnectionsInTemplates(templates);
            linkControllersInPanels(templates.ControllerTemplates, templates.PanelTemplates);
            linkControlledScope(templates.ControlledScopeTemplates, templates);
        }

        #region Link Methods
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
        static private void linkAllConnections(ObservableCollection<TECConnection> connections, ObservableCollection<TECController> controllers, ObservableCollection<TECSystem> bidSystems)
        {
            foreach(TECController controller in controllers)
            {
                var linkedConnections = new ObservableCollection<TECConnection>();
                foreach(TECConnection controllerConnection in controller.Connections)
                {
                    foreach (TECConnection connection in connections)
                    {
                        if(controllerConnection.Guid == connection.Guid)
                        {
                            linkedConnections.Add(connection);
                            connection.Scope.Add(controller);
                        }
                    }
                }
                controller.Connections = linkedConnections;
            }
            foreach(TECSystem system in bidSystems)
            {
                foreach(TECEquipment equipment in system.Equipment)
                {
                    foreach(TECSubScope subScope in equipment.SubScope)
                    {
                        if(subScope.Connection != null)
                        {
                            foreach (TECConnection connection in connections)
                            {
                                if (subScope.Connection.Guid == connection.Guid)
                                {
                                    subScope.Connection = connection;
                                    connection.Scope.Add(subScope);
                                }
                            }
                        }
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
        static private void linkManufacturersWithControllers(ObservableCollection<TECManufacturer> mans, ObservableCollection<TECController> controllers)
        {
            foreach (TECController controller in controllers)
            {
                foreach (TECManufacturer manufacturer in mans)
                {
                    if (controller.Manufacturer.Guid == manufacturer.Guid)
                    {
                        controller.Manufacturer = manufacturer;
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
        static private void linkConduitTypesInBid(TECBid bid)
        {
            linkConduitTypeWithConnections(bid.ConduitTypes, bid.Connections);
        }
        static private void linkConduitTypesInTemplates(TECTemplates templates)
        {
            linkConduitTypeWithConnections(templates.ConduitTypeCatalog, templates.ConnectionTemplates);
        }
      
        static private void linkConduitTypeWithConnections(ObservableCollection<TECConduitType> conduitTypes, ObservableCollection<TECConnection> connections)
        {
            foreach (TECConnection connection in connections)
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
        static private void linkPanelTypesInPanel(ObservableCollection<TECPanelType> panelTypes, ObservableCollection<TECPanel> panels)
        {
            foreach(TECPanel panel in panels)
            {
                foreach(TECPanelType type in panelTypes)
                {
                    if(type != null)
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
                linkScopeObjects(scope.Systems, templates.SystemTemplates);
                linkScopeObjects(scope.Controllers, templates.ControllerTemplates);
                linkScopeObjects(scope.Panels, templates.PanelTemplates);
                linkScopeObjects(scope.Connections, templates.ConnectionTemplates);
            }
        }
        static private void linkControllersInPanels(ObservableCollection<TECController> controllers, ObservableCollection<TECPanel> panels)
        {
            foreach(TECPanel panel in panels)
            {
                linkScopeObjects(panel.Controllers, controllers);
            }
        }
        static private void linkConnectionsInController(ObservableCollection<TECConnection> connections, TECController controller)
        {
            linkScopeObjects(controller.Connections, connections);
        }
        static private void linkConnectionsInSubScope(ObservableCollection<TECConnection> connections, TECSubScope subScope)
        {
            if(subScope.Connection != null)
            {
                foreach (TECConnection connection in connections)
                {
                    if (connection.Guid == subScope.Connection.Guid)
                    {
                        subScope.Connection = connection;
                    }
                }
            }
        }
        static private void linkConnectionsInEquipment(ObservableCollection<TECConnection> connections, TECEquipment equipment)
        {
            foreach(TECSubScope subscope in equipment.SubScope)
            {
                linkConnectionsInSubScope(connections, subscope);
            }
        }
        static private void linkConnectionsInSystem(ObservableCollection<TECConnection> connections, TECSystem system)
        {
            foreach(TECEquipment equipment in system.Equipment)
            {
                linkConnectionsInEquipment(connections, equipment);
            }
        }
        static private void linkConnectionsInTemplates(TECTemplates templates)
        {
            foreach(TECSystem system in templates.SystemTemplates)
            {
                linkConnectionsInSystem(templates.ConnectionTemplates, system);
            }
            foreach(TECEquipment equipment in templates.EquipmentTemplates)
            {
                linkConnectionsInEquipment(templates.ConnectionTemplates, equipment);
            }
            foreach(TECSubScope subScope in templates.SubScopeTemplates)
            {
                linkConnectionsInSubScope(templates.ConnectionTemplates, subScope);
            }
            foreach(TECController controller in templates.ControllerTemplates)
            {
                linkConnectionsInController(templates.ConnectionTemplates, controller);
            }
        }
        static private void linkScopeObjects(object scopeReferenceList, object scopeObjectList)
        {
            var linkedList = new ObservableCollection<TECScope>();
            foreach(TECScope refScope in (IList)scopeReferenceList)
            {
                foreach(TECScope objectScope in (IList)scopeObjectList)
                {
                    if(refScope.Guid == objectScope.Guid)
                    {
                        linkedList.Add(objectScope as TECScope);
                    }
                }
            }
            scopeReferenceList = linkedList;
        }
        #endregion Link Methods

    }
}
