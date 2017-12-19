using EstimatingLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Tests
{
    public static class TestHelper
    {
        public static TECBid CreateTestBid()
        {
            TECBid bid = new TECBid();

            //Bid Info
            bid.Name = "Bid Name";
            bid.BidNumber = "1701-117";
            bid.DueDate = DateTime.Now;
            bid.Salesperson = "Mrs. Test";
            bid.Estimator = "Mr. Test";

            //Bid Objects
            bid.ExtraLabor = CreateTestExtraLabor(bid.Guid);
            bid.Parameters = CreateTestParameters(bid.Guid);
            bid.Catalogs = CreateTestCatalogs();
            
            //Locations
            var cellar = new TECLabeled();
            cellar.Label = "Cellar";
            var location1 = new TECLabeled();
            location1.Label = "1st Floor";
            var location2 = new TECLabeled();
            location2.Label = "2nd Floor";
            var location3 = new TECLabeled();
            location3.Label = "3rd Floor";

            bid.Locations.Add(cellar);
            bid.Locations.Add(location1);
            bid.Locations.Add(location2);
            bid.Locations.Add(location3);

            
            //Scope Branches
            var branch1 = new TECScopeBranch(false);
            branch1.Label = "Branch 1";
            var branch2 = new TECScopeBranch(false);
            branch2.Label = "Branch 2";
            var branch3 = new TECScopeBranch(false);
            branch3.Label = "Branch 3";

            bid.ScopeTree.Add(branch1);
            branch1.Branches.Add(branch2);
            branch2.Branches.Add(branch3);

            //Notes
            var note1 = new TECLabeled();
            note1.Label = "Note 1";

            bid.Notes.Add(note1);

            //Exclusions
            var exclusion1 = new TECLabeled();
            exclusion1.Label = "Exclusion 1";

            bid.Exclusions.Add(exclusion1);

            //Controller
            TECController expectedController = new TECController(Guid.NewGuid(), bid.Catalogs.ControllerTypes[0], false);
            expectedController.Name = "Test Controller";
            expectedController.Description = "Test description";
            
            bid.AddController(expectedController);
            
            //Misc Cost
            TECMisc cost = new TECMisc(CostType.TEC, false);
            cost.Name = "Test Cost";
            cost.Cost = 69.69;
            cost.Quantity = 69;

            bid.MiscCosts.Add(cost);

            //Misc wiring
            TECMisc wiring = new TECMisc(CostType.Electrical, false);
            wiring.Name = "Test Wiring";
            wiring.Cost = 69.69;
            wiring.Quantity = 69;

            bid.MiscCosts.Add(wiring);
            

            //Panels
            TECPanel panel = new TECPanel(bid.Catalogs.PanelTypes[0], false);
            panel.Name = "Test Panel";
            panel.Controllers.Add(expectedController);
            panel.AssociatedCosts.Add(bid.Catalogs.AssociatedCosts[0]);
            panel.AssociatedCosts.Add(bid.Catalogs.AssociatedCosts[0]);

            bid.Panels.Add(panel);
            
            //Systems
            var system1 = CreateTestTypical(bid.Catalogs);
            system1.Name = "System 1";
            system1.Description = "Locations all the way";
            
            var system2 = CreateTestTypical(bid.Catalogs);
            system2.Name = "System 2";
            system2.Description = "Description 2";

            var system3 = CreateTestTypical(bid.Catalogs);
            system3.Name = "System 3";
            system3.Description = "";

            bid.Systems.Add(system1);
            bid.Systems.Add(system2);
            bid.Systems.Add(system3);

            system1.AddInstance(bid);
            system2.AddInstance(bid);
            system3.AddInstance(bid);

            system1.AddInstance(bid);
            system2.AddInstance(bid);
            system3.AddInstance(bid);

            //Equipment
            var equipment1 = new TECEquipment(true);
            equipment1.Name = "Equipment 1";
            equipment1.Description = "Description 1";

            var equipment2 = new TECEquipment(true);
            equipment2.Name = "Equipment 2";
            equipment2.Description = "Description 2";

            system1.Equipment.Add(equipment1);
            system3.Equipment.Add(equipment2);

            //SubScope
            var subScope1 = new TECSubScope(true);
            subScope1.Name = "SubScope 1";
            subScope1.Description = "Description 1";
            subScope1.AssociatedCosts.Add(bid.Catalogs.AssociatedCosts[0]);
            subScope1.AssociatedCosts.Add(bid.Catalogs.AssociatedCosts[0]);
            subScope1.AssociatedCosts.Add(bid.Catalogs.AssociatedCosts[0]);

            var subScope2 = new TECSubScope(true);
            subScope2.Name = "Empty SubScope";
            subScope2.Description = "Description 2";
            subScope2.AssociatedCosts.Add(bid.Catalogs.AssociatedCosts[0]);

            equipment1.SubScope.Add(subScope1);
            equipment2.SubScope.Add(subScope2);

            //Points
            var point1 = new TECPoint(true);
            point1.Label = "Point 1";
            point1.Type = IOType.AI;
            point1.Quantity = 2;

            subScope1.Points.Add(point1);

            var point2 = new TECPoint(true);
            point2.Label = "Point 2";
            point2.Type = IOType.AI;
            point2.Quantity = 2;

            subScope2.Points.Add(point2);

            //Connections
            TECConnection testConnection = expectedController.AddSubScope(subScope1);
            testConnection.ConduitType = bid.Catalogs.ConduitTypes[0];
            testConnection.Length = 42;

            AssignAllSecondaryProperties(bid);

            TECTypical noLocation = new TECTypical();
            noLocation.Name = "No Location";
            noLocation.Equipment.Add(new TECEquipment(true));
            noLocation.Equipment[0].SubScope.Add(new TECSubScope(true));
            bid.Systems.Add(noLocation);

            return bid;
        }

        public static TECBid CreateEmptyCatalogBid()
        {
            TECBid bid = new TECBid();
            bid.Catalogs = CreateTestCatalogs();
            return bid;
        }

        public static TECTemplates CreateTestTemplates()
        {
            TECTemplates templates = new TECTemplates();

            //Labor
            //templates.Labor = CreateTestLabor();
            templates.Parameters.Add(CreateTestParameters(Guid.NewGuid()));
            templates.Catalogs = CreateTestCatalogs();

            //Tags
            TECLabeled testTag = new TECLabeled();
            testTag.Label = "Test Tag";
            TECLabeled sysTag = new TECLabeled();
            sysTag.Label = "System Tag";
            TECLabeled equipTag = new TECLabeled();
            equipTag.Label = "Equipment Tag";
            TECLabeled ssTag = new TECLabeled();
            ssTag.Label = "SubScope Tag";
            TECLabeled devTag = new TECLabeled();
            devTag.Label = "Device Tag";

            templates.Catalogs.Tags.Add(testTag);
            templates.Catalogs.Tags.Add(sysTag);
            templates.Catalogs.Tags.Add(equipTag);
            templates.Catalogs.Tags.Add(ssTag);
            templates.Catalogs.Tags.Add(devTag);

            //Manufacturers
            TECManufacturer testMan = new TECManufacturer();
            testMan.Label = "Test Manufacturer";
            testMan.Multiplier = 0.654;
            TECManufacturer testDevMan = new TECManufacturer();
            testDevMan.Label = "Child Manufacturer (Test Device)";
            testDevMan.Multiplier = 0.446;
            TECManufacturer childDevMan = new TECManufacturer();
            childDevMan.Label = "Child Manufacturer (Child Device)";
            childDevMan.Multiplier = 0.916;
            
            templates.Catalogs.Manufacturers.Add(testMan);
            templates.Catalogs.Manufacturers.Add(testDevMan);
            templates.Catalogs.Manufacturers.Add(childDevMan);

            //Connection Types
            TECConnectionType testDevConnType = new TECConnectionType();
            testDevConnType.Name = "FourC18";

            TECConnectionType childDevConnType = new TECConnectionType();
            childDevConnType.Name = "ThreeC18";

            templates.Catalogs.ConnectionTypes.Add(testDevConnType);
            templates.Catalogs.ConnectionTypes.Add(childDevConnType);

            //Conduit Types
            TECElectricalMaterial testConduitType = new TECElectricalMaterial();
            testConduitType.Name = "EMT";
            testConduitType.Cost = 12;
            testConduitType.Labor = 2;

            templates.Catalogs.ConduitTypes.Add(testConduitType);

            TECElectricalMaterial otherConduitType = new TECElectricalMaterial();
            otherConduitType.Name = "RGS";
            otherConduitType.Cost = 18;
            otherConduitType.Labor = 4;

            templates.Catalogs.ConduitTypes.Add(otherConduitType);

            //Associated Costs
            TECCost testAssociatedCost = new TECCost(CostType.Electrical);
            testAssociatedCost.Name = "Flex";
            testAssociatedCost.Cost = 42;

            templates.Catalogs.AssociatedCosts.Add(testAssociatedCost);

            var testCost2 = new TECCost(CostType.TEC);
            testCost2.Name = "Other Cost";
            templates.Catalogs.AssociatedCosts.Add(testCost2);

            //IO Modules
            TECIOModule testIOModule = new TECIOModule(testMan);
            testIOModule.Name = "Test IO Module";
            testIOModule.Price = 42;
            testIOModule.Manufacturer = testMan;
            templates.Catalogs.IOModules.Add(testIOModule);

            //Devices
            ObservableCollection<TECConnectionType> contypes2 = new ObservableCollection<TECConnectionType>();
            contypes2.Add(testDevConnType);
            TECDevice testDev = new TECDevice(Guid.NewGuid(), contypes2, testDevMan);
            testDev.Name = "Test Device";
            testDev.Description = "Device Description";
            testDev.Price = 20.3;

            ObservableCollection<TECConnectionType> contypes3 = new ObservableCollection<TECConnectionType>();
            contypes3.Add(childDevConnType);
            TECDevice childDev = new TECDevice(Guid.NewGuid(), contypes3, childDevMan);
            childDev.Name = "Child Device";
            childDev.Description = "Child Device Description";
            childDev.Price = 54.1;

            testDev.Tags.Add(devTag);
            childDev.Tags.Add(devTag);

            templates.Catalogs.Devices.Add(testDev);
            templates.Catalogs.Devices.Add(childDev);

            //System
            TECSystem system = new TECSystem(false);
            system.Name = "Test System";
            system.Description = "System Description";
            
            TECEquipment sysEquip = new TECEquipment(false);
            sysEquip.Name = "System Equipment";
            sysEquip.Description = "Child Equipment";
            TECSubScope sysSS = new TECSubScope(false);
            sysSS.Name = "System SubScope";
            sysSS.Description = "Child SubScope";
            sysSS.AssociatedCosts.Add(testAssociatedCost);
            TECPoint sysPoint = new TECPoint(false);
            sysPoint.Type = IOType.BACnetIP;
            sysPoint.Label = "System Point";

            sysSS.Points.Add(sysPoint);
            sysSS.Devices.Add(childDev);
            sysSS.Tags.Add(ssTag);

            sysEquip.SubScope.Add(sysSS);
            sysEquip.Tags.Add(equipTag);

            system.Equipment.Add(sysEquip);
            system.Tags.Add(sysTag);

            templates.SystemTemplates.Add(system);

            //Equipment
            TECEquipment equipment = new TECEquipment(false);
            equipment.Name = "Test Equipment";
            equipment.Description = "Equipment Description";
            TECSubScope equipSS = new TECSubScope(false);
            equipSS.Name = "Equipment SubScope";
            equipSS.Description = "Child SubScope";
            TECPoint equipPoint = new TECPoint(false);
            equipPoint.Type = IOType.AI;
            equipPoint.Label = "Equipment Point";

            equipSS.Points.Add(equipPoint);
            equipSS.Devices.Add(childDev);
            equipSS.Tags.Add(ssTag);

            equipment.SubScope.Add(equipSS);
            equipment.Tags.Add(equipTag);

            templates.EquipmentTemplates.Add(equipment);

            /*System with reference Equipment
            TECSystem equipSystem = new TECSystem(false);
            equipSystem.Name = "Sys RefEquip";
            equipSystem.Equipment.Add(templates.EquipmentSynchronizer.NewItem(equipment));

            templates.SystemTemplates.Add(equipSystem);
            */

            //SubScope
            TECSubScope subScope = new TECSubScope(false);
            subScope.Name = "Test SubScope";
            subScope.Description = "SubScope Description";
            TECPoint ssPoint = new TECPoint(false);
            ssPoint.Type = IOType.DO;
            ssPoint.Label = "SubScope Point";

            subScope.Points.Add(ssPoint);
            subScope.Devices.Add(childDev);
            subScope.Tags.Add(ssTag);
            subScope.AssociatedCosts.Add(testAssociatedCost);

            templates.SubScopeTemplates.Add(subScope);

            /*Equipment with reference SubScope
            TECEquipment ssEquip = new TECEquipment(false);
            ssEquip.Name = "Equip RefSS";
            ssEquip.SubScope.Add(templates.SubScopeSynchronizer.NewItem(subScope));

            templates.EquipmentTemplates.Add(ssEquip);
            */


            //Controller
            var expectedControllerType = new TECControllerType(testMan);
            expectedControllerType.Price = 42.6;
            TECIO ioToAdd = new TECIO(IOType.AI);
            ioToAdd.Quantity = 5;
            TECIO otherIO = new TECIO(IOType.BACnetMSTP);
            otherIO.Quantity = 3;
            expectedControllerType.IO.Add(ioToAdd);
            expectedControllerType.IO.Add(otherIO);
            templates.Catalogs.ControllerTypes.Add(expectedControllerType);

            TECController expectedController = new TECController(expectedControllerType, false);
            expectedController.Name = "Test Controller";
            expectedController.Description = "Test description";

            TECController controlledController = new TECController(expectedControllerType, false);
            controlledController.Name = "Controlled Controller";
            
            templates.ControllerTemplates.Add(expectedController);

            //Misc Cost
            TECMisc cost = new TECMisc(CostType.TEC, false);
            cost.Name = "Test Cost";
            cost.Cost = 79.79;
            cost.Quantity = 67;

            templates.MiscCostTemplates.Add(cost);

            //Misc wiring
            TECMisc wiring = new TECMisc(CostType.Electrical, false);
            wiring.Name = "Test Wiring";
            wiring.Cost = 69.69;
            wiring.Quantity = 69;

            templates.MiscCostTemplates.Add(wiring);

            //Panel Types
            TECPanelType panelType = new TECPanelType(testMan);
            panelType.Price = 123.4;
            panelType.Name = "Test Panel Type";

            templates.Catalogs.PanelTypes.Add(panelType);

            //Panels
            TECPanel panel = new TECPanel(panelType, false);
            panel.Name = "Test Panel";
            panel.Controllers.Add(expectedController);
            panel.AssociatedCosts.Add(testAssociatedCost);
            panel.AssociatedCosts.Add(testAssociatedCost);

            TECPanel controlledPanel = new TECPanel(panelType, false);
            controlledPanel.Name = "Controlled Panel";

            templates.PanelTemplates.Add(panel);

            //Connections
            //TECSubScopeConnection controlledConnection = new TECSubScopeConnection();
            //controlledConnection.ConduitType = testConduitType;
            //controlledConnection.Length = 42;
            //controlledConnection.ParentController = controlledController;

            //controlledController.ChildrenConnections.Add(controlledConnection);

            //Controlled Scope
            //TECSystem testConScope = CreateTestSystem(templates.Catalogs);
            //testConScope.Label = "Test Controlled Scope";
            //testConScope.Description = "Test Description";
            //var controlledScopeEquipment = equipment.DragDropCopy() as TECEquipment;
            //testConScope.Equipment.Add(controlledScopeEquipment);
            //var controlledScopePanel = controlledPanel.DragDropCopy() as TECPanel;
            //controlledScopePanel.Type = panelType;
            //testConScope.Panels.Add(controlledScopePanel);
            //var controlledScopeController = controlledController.DragDropCopy() as TECController;
            //controlledScopePanel.Controllers.Add(controlledScopeController);
            //testConScope.Controllers.Add(controlledScopeController);
            //var connection = controlledScopeController.AddSubScope(subScope.DragDropCopy() as TECSubScope);
            //connection.Length = 42;
            //controlledScopeController.ChildrenConnections[0].ConduitType = testConduitType;

            //templates.SystemTemplates.Add(testConScope);

            return templates;
        }

        public static TECCatalogs CreateTestCatalogs()
        {
            TECCatalogs outCatalogs = new TECCatalogs();

            //Associated Costs
            TECCost elecCost = new TECCost(CostType.Electrical);
            elecCost.Name = "Elec Cost";
            elecCost.Cost = 156.61;
            elecCost.Labor = 456.64;
            elecCost.Type = CostType.Electrical;
            outCatalogs.AssociatedCosts.Add(elecCost);

            TECCost tecCost = new TECCost(CostType.TEC);
            tecCost.Name = "TEC Cost";
            tecCost.Cost = 46.43;
            tecCost.Labor = 61.45;
            tecCost.Type = CostType.TEC;
            outCatalogs.AssociatedCosts.Add(tecCost);

            //Tags
            var tag1 = new TECLabeled();
            tag1.Label = "Tag 1";
            var tag2 = new TECLabeled();
            tag2.Label = "Test Tag";

            outCatalogs.Tags.Add(tag1);
            outCatalogs.Tags.Add(tag2);

            //Conduit Types
            var conduitType1 = new TECElectricalMaterial();
            conduitType1.Name = "Test Conduit 1";
            conduitType1.Cost = 64.49;
            conduitType1.Labor = 463.87;
            AssignSecondaryProperties(conduitType1, outCatalogs);
            conduitType1.RatedCosts.Add(tecCost);
            conduitType1.RatedCosts.Add(elecCost);

            outCatalogs.ConduitTypes.Add(conduitType1);

            var conduitType2 = new TECElectricalMaterial();
            conduitType2.Name = "Test Conduit 2";
            conduitType2.Cost = 13.45;
            conduitType2.Labor = 9873.40;
            AssignSecondaryProperties(conduitType2, outCatalogs);
            conduitType2.RatedCosts.Add(tecCost);
            conduitType2.RatedCosts.Add(elecCost);

            outCatalogs.ConduitTypes.Add(conduitType2);

            //ConnectionTypes
            var connectionType1 = new TECConnectionType();
            connectionType1.Name = "FourC18";
            connectionType1.Cost = 64.63;
            connectionType1.Labor = 98.16;
            AssignSecondaryProperties(connectionType1, outCatalogs);
            connectionType1.RatedCosts.Add(tecCost);
            connectionType1.RatedCosts.Add(elecCost);

            var connectionType2 = new TECConnectionType();
            connectionType2.Name = "ThreeC18";
            connectionType2.Cost = 73.16;
            connectionType2.Labor = 35.49;
            AssignSecondaryProperties(connectionType1, outCatalogs);
            connectionType1.RatedCosts.Add(tecCost);
            connectionType1.RatedCosts.Add(elecCost);

            outCatalogs.ConnectionTypes.Add(connectionType1);
            outCatalogs.ConnectionTypes.Add(connectionType2);

            //Manufacturers
            var manufacturer1 = new TECManufacturer();
            manufacturer1.Label = "Test";
            manufacturer1.Multiplier = .51;
            
            outCatalogs.Manufacturers.Add(manufacturer1);

            //Devices
            ObservableCollection<TECConnectionType> contypes4 = new ObservableCollection<TECConnectionType>();
            contypes4.Add(connectionType1);
            TECDevice device1 = new TECDevice(Guid.NewGuid(), contypes4, manufacturer1);
            device1.Name = "Device 1";
            device1.Description = "Description 1";
            device1.Price = 64.96;
            device1.Tags.Add(tag1);
            AssignSecondaryProperties(device1, outCatalogs);

            outCatalogs.Devices.Add(device1);

            //IO Modules
            TECIOModule testIOModule = new TECIOModule(manufacturer1);
            testIOModule.Name = "Test IO Module";
            testIOModule.Price = 13.46;
            testIOModule.Manufacturer = manufacturer1;
            outCatalogs.IOModules.Add(testIOModule);

            //Controller Types
            TECControllerType controllerType = new TECControllerType(manufacturer1);
            controllerType.Name = "Test Controller Type";
            controllerType.Price = 196.73;
            controllerType.Labor = 61.34;
            AssignSecondaryProperties(controllerType, outCatalogs);

            TECIO io = new TECIO(IOType.BACnetIP);
            io.Quantity = 100;
            controllerType.IO.Add(io);

            io = new TECIO(IOType.AI);
            io.Quantity = 11;
            controllerType.IO.Add(io);

            outCatalogs.ControllerTypes.Add(controllerType);

            //Panel Types
            TECPanelType panelType = new TECPanelType(manufacturer1);
            panelType.Price = 16.64;
            panelType.Labor = 91.46;
            panelType.Name = "Test Panel Type";
            AssignSecondaryProperties(panelType, outCatalogs);

            outCatalogs.PanelTypes.Add(panelType);

            //Valves
            TECDevice actuator = new TECDevice(new ObservableCollection<TECConnectionType>() { connectionType1 },
                manufacturer1);
            actuator.Name = "actuator";
            outCatalogs.Devices.Add(actuator);
            TECValve valve = new TECValve(manufacturer1, actuator);
            outCatalogs.Valves.Add(valve);

            return outCatalogs;
        }

        public static TECTypical CreateTestTypical(TECCatalogs catalogs)
        {
            TECTypical outScope = new TECTypical();
            outScope.Tags.Add(catalogs.Tags[0]);
            outScope.ProposeEquipment = true;
            var panel = CreateTestPanel(true, catalogs);

            outScope.Panels.Add(panel);
            var equipment = CreateTestEquipment(true, catalogs);
            outScope.Equipment.Add(equipment);

            var controller = CreateTestController(true, catalogs);
            outScope.AddController(controller);

            ConnectEquipmentToController(equipment, controller);
            panel.Controllers.Add(controller);

            var scopeBranch = new TECScopeBranch(true);
            outScope.ScopeBranches.Add(scopeBranch);

            var tecMisc = CreateTestMisc(CostType.TEC, true);
            var elecMisc = CreateTestMisc(CostType.Electrical, true);
            outScope.MiscCosts.Add(tecMisc);
            outScope.MiscCosts.Add(elecMisc);

            outScope.AssociatedCosts.Add(catalogs.AssociatedCosts[0]);

            return outScope;
        }
        public static TECDevice CreateTestDevice(TECCatalogs catalogs)
        {

            var connectionTypes = new ObservableCollection<TECConnectionType>();
            connectionTypes.Add(catalogs.ConnectionTypes[0]);
            var manufacturer = catalogs.Manufacturers[0];

            double cost = 12.61;

            var assCosts = new ObservableCollection<TECCost>();
            int costNum = 9;
            for(int x = 0; x < costNum; x++)
            {
                assCosts.Add(catalogs.AssociatedCosts[0]);
            }

            TECDevice device = new TECDevice(connectionTypes, manufacturer);
            device.Price = cost;
            device.AssociatedCosts = assCosts;
            device.Tags.Add(catalogs.Tags[0]);
            return device;
        }
        public static TECSubScope CreateTestSubScope(bool isTypical, TECCatalogs catalogs)
        {
            var device = catalogs.Devices[0];
            var point = new TECPoint(isTypical);
            point.Type = IOType.AI;

            var subScope = new TECSubScope(isTypical);
            subScope.Tags.Add(catalogs.Tags[0]);
            subScope.Devices.Add(device);
            subScope.Points.Add(point);
            return subScope;
        }
        public static TECPoint CreateTestPoint(bool isTypical, TECCatalogs catalogs)
        {
            TECPoint point = new TECPoint(isTypical);
            point.Type = (IOType)Enum.GetNames(typeof(IOType)).Length;
            return point;
        }
        public static TECEquipment CreateTestEquipment(bool isTypical, TECCatalogs catalogs)
        {
            var equipment = new TECEquipment(isTypical);
            equipment.Tags.Add(catalogs.Tags[0]);

            int subNumber = 5;
            for(int x = 0; x < subNumber; x++)
            {
                equipment.SubScope.Add(CreateTestSubScope(isTypical, catalogs));
            }
            
            return equipment;
        }
        public static TECController CreateTestController(bool isTypical, TECCatalogs catalogs)
        {
            var type = catalogs.ControllerTypes[0];

            var controller = new TECController(type, isTypical);
            controller.Tags.Add(catalogs.Tags[0]);
            return controller;
        }
        public static TECPanel CreateTestPanel(bool isTypical, TECCatalogs catalogs)
        {
            var panelType = catalogs.PanelTypes[0];
            
            var panel = new TECPanel(panelType, isTypical);
            panel.Tags.Add(catalogs.Tags[0]);
            return panel;
        }
        public static TECExtraLabor CreateTestExtraLabor(Guid bidGuid)
        {
            TECExtraLabor labor = new TECExtraLabor(bidGuid);
            labor.PMExtraHours = 1.1;
            labor.CommExtraHours = 3.3;
            labor.ENGExtraHours = 2.2;
            labor.SoftExtraHours = 4.4;
            labor.GraphExtraHours = 5.5;
            return labor;

        }
        public static TECParameters CreateTestParameters(Guid bidGuid)
        {
            var parameters = new TECParameters(bidGuid);
            parameters.IsTaxExempt = true;

            parameters.PMCoef = 0.1;
            parameters.PMRate = 0.11;

            parameters.ENGCoef = 0.2;
            parameters.ENGRate = 0.22;

            parameters.CommCoef = 0.3;
            parameters.CommRate = 0.33;

            parameters.SoftCoef = 0.4;
            parameters.SoftRate = 0.44;

            parameters.GraphCoef = 0.5;
            parameters.GraphRate = 0.55;

            parameters.ElectricalRate = 0.6;
            parameters.ElectricalSuperRate = 0.66;
            parameters.ElectricalSuperRatio = 1.0 / 7.0;

            parameters.ElectricalNonUnionRate = 0.7;
            parameters.ElectricalSuperNonUnionRate = 0.77;

            parameters.ElectricalIsOnOvertime = true;
            parameters.ElectricalIsUnion = true;
            return parameters;
        }
        public static TECMisc CreateTestMisc(CostType type, bool isTypical)
        {
            TECMisc misc = new TECMisc(type, isTypical);
            misc.Cost = 323.61;
            misc.Labor = 49.78;
            misc.Quantity = 3;
           
            return misc;
        }
        
        public static void ConnectEquipmentToController(TECEquipment equipment, TECController controller)
        {
            foreach(TECSubScope subscope in equipment.SubScope)
            {
                controller.AddSubScope(subscope);
            }
        }

        public static TECObject FindObjectInSystems(ObservableCollection<TECTypical> systems, TECObject reference)
        {
            foreach(TECTypical system in systems)
            {
                if(system.Guid == reference.Guid)
                { return system; }
                else
                {
                    foreach(TECEquipment equipment in system.Equipment)
                    {
                        if(equipment.Guid == reference.Guid)
                        { return equipment; }
                        else
                        {
                            foreach(TECSubScope subScope in equipment.SubScope)
                            {
                                if(subScope.Guid == reference.Guid)
                                { return subScope; }
                                else
                                {
                                    foreach (TECDevice device in subScope.Devices)
                                    {
                                        if (device.Guid == reference.Guid)
                                        { return device; }
                                    }
                                    foreach (TECPoint point in subScope.Points)
                                    {
                                        if (point.Guid == reference.Guid)
                                        { return point; }
                                    }
                                }
                            }
                        }
                    }
                    foreach(TECPanel panel in system.Panels)
                    {
                        if(panel.Guid == reference.Guid)
                        { return panel; }
                    }
                    foreach(TECController controller in system.Controllers)
                    {
                        if(controller.Guid == reference.Guid)
                        { return controller; }
                    }
                    foreach (TECScopeBranch branch in system.ScopeBranches)
                    {
                        if (branch.Guid == reference.Guid)
                        { return branch; }
                    }
                }
            }
            return null;
        }
        public static TECController FindControllerInController(IEnumerable<TECController> controllers, TECController reference)
        {
            foreach(TECController controller in controllers)
            {
                if(controller.Guid == reference.Guid)
                {
                    return controller;
                }
            }
            return null;
        }
        public static TECConnection FindConnectionInController(TECController controller, TECConnection reference)
        {
            foreach(TECConnection connection in controller.ChildrenConnections)
            {
                if(connection.Guid == reference.Guid)
                {
                    return connection;
                }
            }
            return null;
        }

        public static void AssignSecondaryProperties(TECLocated located, TECBid bid)
        {
            located.Location = bid.Locations[0];
            AssignSecondaryProperties(located, bid.Catalogs);
        }
        public static void AssignSecondaryProperties(TECScope scope, TECCatalogs catalogs)
        {
            if (scope.Tags.Count == 0)
            {
                scope.Tags.Add(catalogs.Tags[0]);
            }
            bool tecAdded = false;
            bool elecAdded = false;
            foreach(TECCost cost in catalogs.AssociatedCosts)
            {
                if (cost.Type == CostType.TEC)
                {
                    if (!tecAdded)
                    {
                        scope.AssociatedCosts.Add(cost);
                        tecAdded = true;
                    }
                }
                else if (cost.Type == CostType.Electrical)
                {
                    if (!elecAdded)
                    {
                        scope.AssociatedCosts.Add(cost);
                        elecAdded = true;
                    }
                }
                if (tecAdded && elecAdded) break;
            }
        }

        public static void AssignAllSecondaryProperties(TECBid bid)
        {
            foreach(TECTypical system in bid.Systems)
            {
                AssignSecondaryProperties(system, bid);
                foreach(TECEquipment equipment in system.Equipment)
                {
                    AssignSecondaryProperties(equipment, bid);
                    foreach(TECSubScope subScope in equipment.SubScope)
                    {
                        AssignSecondaryProperties(subScope, bid);
                    }
                }
                foreach(TECSystem instance in system.Instances)
                {
                    AssignSecondaryProperties(instance, bid);
                    foreach (TECEquipment equipment in instance.Equipment)
                    {
                        AssignSecondaryProperties(equipment, bid);
                        foreach (TECSubScope subScope in equipment.SubScope)
                        {
                            AssignSecondaryProperties(subScope, bid);
                        }
                    }
                }
            }
        }

        public static bool areDoublesEqual(double first, double second, double maxDiff = 1.0 / 1000.0)
        {
            var absFirst = Math.Abs(first);
            if(Math.Abs(first - second) > (maxDiff))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool IsInBid(TECSubScope subScope, TECBid bid)
        {
            foreach(TECTypical typical in bid.Systems)
            {
                if (typical.GetAllSubScope().Contains(subScope))
                {
                    return true;
                }
                foreach(TECSystem instance in typical.Instances)
                {
                    if (instance.GetAllSubScope().Contains(subScope))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static TECObject ObjectWithGuid(Guid guid, TECBid bid)
        {
            if(bid.Guid == guid)
            {
                return bid;
            }
            if(bid.Catalogs.Guid == guid)
            {
                return bid.Catalogs;
            }
            if(bid.Parameters.Guid == guid)
            {
                return bid.Parameters;
            }
            foreach(TECTypical typical in bid.Systems)
            {
                if(typical.Guid == guid)
                {
                    return typical;
                }
                foreach(TECEquipment equipment in typical.Equipment)
                {
                    if (equipment.Guid == guid)
                    {
                        return equipment;
                    }
                    foreach(TECSubScope subScope in equipment.SubScope)
                    {
                        if(subScope.Guid == guid)
                        {
                            return subScope;
                        }
                        foreach(TECPoint point in subScope.Points)
                        {
                            if(point.Guid == guid)
                            {
                                return point;
                            }
                        }
                    }
                }
                foreach (TECController controller in typical.Controllers)
                {
                    if (controller.Guid == guid)
                    {
                        return controller;
                    }
                    foreach (TECConnection connection in controller.ChildrenConnections)
                    {
                        if (connection.Guid == guid)
                        {
                            return connection;
                        }
                    }
                }
                foreach (TECPanel panel in typical.Panels)
                {
                    if (panel.Guid == guid)
                    {
                        return panel;
                    }
                }
                foreach (TECMisc misc in typical.MiscCosts)
                {
                    if (misc.Guid == guid)
                    {
                        return misc;
                    }
                }
                foreach (TECSystem system in typical.Instances)
                {
                    if (system.Guid == guid)
                    {
                        return system;
                    }
                    foreach (TECEquipment equipment in system.Equipment)
                    {
                        if (equipment.Guid == guid)
                        {
                            return equipment;
                        }
                        foreach (TECSubScope subScope in equipment.SubScope)
                        {
                            if (subScope.Guid == guid)
                            {
                                return subScope;
                            }
                            foreach (TECPoint point in subScope.Points)
                            {
                                if (point.Guid == guid)
                                {
                                    return point;
                                }
                            }
                        }
                    }
                    foreach (TECController controller in system.Controllers)
                    {
                        if (controller.Guid == guid)
                        {
                            return controller;
                        }
                        foreach (TECConnection connection in controller.ChildrenConnections)
                        {
                            if (connection.Guid == guid)
                            {
                                return connection;
                            }
                        }
                    }
                    foreach (TECPanel panel in system.Panels)
                    {
                        if (panel.Guid == guid)
                        {
                            return panel;
                        }
                    }
                    foreach (TECMisc misc in system.MiscCosts)
                    {
                        if (misc.Guid == guid)
                        {
                            return misc;
                        }
                    }
                }
            }
            foreach(TECController controller in bid.Controllers)
            {
                if(controller.Guid == guid)
                {
                    return controller;
                }
                foreach(TECConnection connection in controller.ChildrenConnections)
                {
                    if(connection.Guid == guid)
                    {
                        return connection;
                    }
                }
            }
            foreach(TECPanel panel in bid.Panels)
            {
                if(panel.Guid == guid)
                {
                    return panel;
                }
            }
            foreach (TECMisc misc in bid.MiscCosts)
            {
                if (misc.Guid == guid)
                {
                    return misc;
                }
            }
            return null;
        }
        
    }
}
