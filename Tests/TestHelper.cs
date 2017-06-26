using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EstimatingLibrary;
using EstimatingUtilitiesLibrary;
using System.Collections.ObjectModel;
using System.IO;
using System.Collections;

namespace Tests
{
    public static class TestHelper
    {
        static private Random rand = new Random();
        static public string StaticTestBidPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + @"\Resources\StaticTestBid.tbdb";
        static public string StaticTestTemplatesPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + @"\Resources\StaticTestTemplates.ttdb";
        static public string TestPDF1 = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + @"\Resources\Sales Office Update.pdf";
        static public string TestPDF2 = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + @"\Resources\pdf-sample.pdf";

        public static TECBid CreateTestBid()
        {
            TECBid bid = new TECBid();

            //Bid Info
            bid.Name = "Bid Name";
            bid.BidNumber = "1701-117";
            bid.DueDate = DateTime.Now;
            bid.Salesperson = "Mrs. Test";
            bid.Estimator = "Mr. Test";

            //Bid Parameters
            bid.Parameters.IsTaxExempt = true;

            //Bid Labor
            bid.Labor = CreateTestLabor();
            bid.Catalogs = CreateTestCatalogs();
            
            //Locations
            var cellar = new TECLocation();
            cellar.Name = "Cellar";
            var location1 = new TECLocation();
            location1.Name = "1st Floor";
            var location2 = new TECLocation();
            location2.Name = "2nd Floor";
            var location3 = new TECLocation();
            location3.Name = "3rd Floor";

            bid.Locations.Add(cellar);
            bid.Locations.Add(location1);
            bid.Locations.Add(location2);
            bid.Locations.Add(location3);

            
            //Scope Branches
            var branch1 = new TECScopeBranch();
            branch1.Name = "Branch 1";
            branch1.Description = "1st Description";
            var branch2 = new TECScopeBranch();
            branch2.Name = "Branch 2";
            branch2.Description = "2nd Description";
            var branch3 = new TECScopeBranch();
            branch3.Name = "Branch 3";
            branch3.Description = "3rd Description";

            bid.ScopeTree.Add(branch1);
            branch1.Branches.Add(branch2);
            branch2.Branches.Add(branch3);

            //Notes
            var note1 = new TECNote();
            note1.Text = "Note 1";

            bid.Notes.Add(note1);

            //Exclusions
            var exclusion1 = new TECExclusion();
            exclusion1.Text = "Exclusion 1";

            bid.Exclusions.Add(exclusion1);

            //Controller
            TECController expectedController = new TECController(Guid.NewGuid(), bid.Catalogs.Manufacturers.RandomObject());
            expectedController.Name = "Test Controller";
            expectedController.Description = "Test description";
            expectedController.Cost = 42.6;

            TECIO ioToAdd = new TECIO();
            ioToAdd.Type = IOType.AI;
            ioToAdd.Quantity = 5;
            expectedController.IO.Add(ioToAdd);
            bid.Controllers.Add(expectedController);
            
            //Misc Cost
            TECMisc cost = new TECMisc();
            cost.Name = "Test Cost";
            cost.Cost = 69.69;
            cost.Quantity = 69;
            cost.Type = CostType.TEC;

            bid.MiscCosts.Add(cost);

            //Misc wiring
            TECMisc wiring = new TECMisc();
            wiring.Name = "Test Wiring";
            wiring.Cost = 69.69;
            wiring.Quantity = 69;
            wiring.Type = CostType.Electrical;

            bid.MiscCosts.Add(wiring);
            

            //Panels
            TECPanel panel = new TECPanel(bid.Catalogs.PanelTypes.RandomObject());
            panel.Name = "Test Panel";
            panel.Controllers.Add(expectedController);
            panel.AssociatedCosts.Add(bid.Catalogs.AssociatedCosts.RandomObject());
            panel.AssociatedCosts.Add(bid.Catalogs.AssociatedCosts.RandomObject());

            bid.Panels.Add(panel);
            
            //Systems
            var system1 = CreateTestSystem(bid.Catalogs);
            AssignSecondaryProperties(system1, bid);
            system1.Name = "System 1";
            system1.Description = "Locations all the way";
            system1.BudgetPriceModifier = 234.5;
            system1.Quantity = 2345;
            
            var system2 = CreateTestSystem(bid.Catalogs);
            AssignSecondaryProperties(system2, bid);
            system2.Name = "System 2";
            system2.Description = "Description 2";
            system2.BudgetPriceModifier = 234.52;
            system2.Quantity = 23452;

            var system3 = CreateTestSystem(bid.Catalogs);
            AssignSecondaryProperties(system3, bid);
            system3.Name = "System 3";
            system3.Description = "No Location";
            system3.BudgetPriceModifier = 349;

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
            var equipment1 = new TECEquipment();
            AssignSecondaryProperties(equipment1, bid);
            equipment1.Name = "Equipment 1";
            equipment1.Description = "Description 1";
            equipment1.BudgetUnitPrice = 123.4;
            equipment1.Quantity = 1234;

            var equipment2 = new TECEquipment();
            AssignSecondaryProperties(equipment2, bid);
            equipment2.Name = "Equipment 2";
            equipment2.Description = "Description 2";
            equipment2.BudgetUnitPrice = 0;

            system1.Equipment.Add(equipment1);
            system3.Equipment.Add(equipment2);

            //SubScope
            var subScope1 = new TECSubScope();
            AssignSecondaryProperties(subScope1, bid);
            subScope1.Name = "SubScope 1";
            subScope1.Description = "Description 1";
            subScope1.Quantity = 654;
            subScope1.AssociatedCosts.Add(bid.Catalogs.AssociatedCosts.RandomObject());
            subScope1.AssociatedCosts.Add(bid.Catalogs.AssociatedCosts.RandomObject());
            subScope1.AssociatedCosts.Add(bid.Catalogs.AssociatedCosts.RandomObject());

            var subScope2 = new TECSubScope();
            AssignSecondaryProperties(subScope2, bid);
            subScope2.Name = "Empty SubScope";
            subScope2.Description = "Description 2";
            subScope2.AssociatedCosts.Add(bid.Catalogs.AssociatedCosts.RandomObject());

            equipment1.SubScope.Add(subScope1);
            equipment2.SubScope.Add(subScope2);

            //Points
            var point1 = new TECPoint();
            point1.Name = "Point 1";
            point1.Description = "Description 1";
            point1.Type = PointTypes.Serial;
            point1.Quantity = 321;

            subScope1.Points.Add(point1);

            //Connections
            TECConnection testConnection = expectedController.AddSubScope(subScope1);
            testConnection.ConduitType = bid.Catalogs.ConduitTypes.RandomObject();
            testConnection.Length = 42;

            return bid;
        }

        public static TECTemplates CreateTestTemplates()
        {
            TECTemplates templates = new TECTemplates();

            //Labor
            templates.Labor = CreateTestLabor();

            //Tags
            TECTag testTag = new TECTag();
            testTag.Text = "Test Tag";
            TECTag sysTag = new TECTag();
            sysTag.Text = "System Tag";
            TECTag equipTag = new TECTag();
            equipTag.Text = "Equipment Tag";
            TECTag ssTag = new TECTag();
            ssTag.Text = "SubScope Tag";
            TECTag devTag = new TECTag();
            devTag.Text = "Device Tag";

            templates.Catalogs.Tags.Add(testTag);
            templates.Catalogs.Tags.Add(sysTag);
            templates.Catalogs.Tags.Add(equipTag);
            templates.Catalogs.Tags.Add(ssTag);
            templates.Catalogs.Tags.Add(devTag);

            //Manufacturers
            TECManufacturer testMan = new TECManufacturer();
            testMan.Name = "Test Manufacturer";
            testMan.Multiplier = 0.654;
            TECManufacturer testDevMan = new TECManufacturer();
            testDevMan.Name = "Child Manufacturer (Test Device)";
            testDevMan.Multiplier = 0.446;
            TECManufacturer childDevMan = new TECManufacturer();
            childDevMan.Name = "Child Manufacturer (Child Device)";
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
            TECConduitType testConduitType = new TECConduitType();
            testConduitType.Name = "EMT";
            testConduitType.Cost = 12;
            testConduitType.Labor = 2;

            templates.Catalogs.ConduitTypes.Add(testConduitType);

            TECConduitType otherConduitType = new TECConduitType();
            otherConduitType.Name = "RGS";
            otherConduitType.Cost = 18;
            otherConduitType.Labor = 4;

            templates.Catalogs.ConduitTypes.Add(otherConduitType);

            //Associated Costs
            TECCost testAssociatedCost = new TECCost();
            testAssociatedCost.Name = "Flex";
            testAssociatedCost.Cost = 42;
            testAssociatedCost.Type = CostType.Electrical;

            templates.Catalogs.AssociatedCosts.Add(testAssociatedCost);

            var testCost2 = new TECCost();
            testCost2.Name = "Other Cost";
            testAssociatedCost.Type = CostType.TEC;
            templates.Catalogs.AssociatedCosts.Add(testCost2);

            //IO Modules
            TECIOModule testIOModule = new TECIOModule();
            testIOModule.Name = "Test IO Module";
            testIOModule.Cost = 42;
            testIOModule.Manufacturer = testMan;
            templates.Catalogs.IOModules.Add(testIOModule);

            //Devices
            ObservableCollection<TECConnectionType> contypes2 = new ObservableCollection<TECConnectionType>();
            contypes2.Add(testDevConnType);
            TECDevice testDev = new TECDevice(Guid.NewGuid(), contypes2, testDevMan);
            testDev.Name = "Test Device";
            testDev.Description = "Device Description";
            testDev.Cost = 20.3;

            ObservableCollection<TECConnectionType> contypes3 = new ObservableCollection<TECConnectionType>();
            contypes3.Add(childDevConnType);
            TECDevice childDev = new TECDevice(Guid.NewGuid(), contypes3, childDevMan);
            childDev.Name = "Child Device";
            childDev.Description = "Child Device Description";
            childDev.Cost = 54.1;

            testDev.Tags.Add(devTag);
            childDev.Tags.Add(devTag);

            templates.Catalogs.Devices.Add(testDev);
            templates.Catalogs.Devices.Add(childDev);

            //System
            TECSystem system = new TECSystem();
            system.Name = "Test System";
            system.Description = "System Description";
            system.BudgetPriceModifier = 587.3;
            
            TECEquipment sysEquip = new TECEquipment();
            sysEquip.Name = "System Equipment";
            sysEquip.Description = "Child Equipment";
            sysEquip.BudgetUnitPrice = 489.5;
            TECSubScope sysSS = new TECSubScope();
            sysSS.Name = "System SubScope";
            sysSS.Description = "Child SubScope";
            sysSS.AssociatedCosts.Add(testAssociatedCost);
            TECPoint sysPoint = new TECPoint();
            sysPoint.Type = PointTypes.Serial;
            sysPoint.Name = "System Point";
            sysPoint.Description = "Child Point";

            sysSS.Points.Add(sysPoint);
            sysSS.Devices.Add(childDev);
            sysSS.Tags.Add(ssTag);

            sysEquip.SubScope.Add(sysSS);
            sysEquip.Tags.Add(equipTag);

            system.Equipment.Add(sysEquip);
            system.Tags.Add(sysTag);

            templates.SystemTemplates.Add(system);

            //Equipment
            TECEquipment equipment = new TECEquipment();
            equipment.Name = "Test Equipment";
            equipment.Description = "Equipment Description";
            equipment.BudgetUnitPrice = 193.2;
            TECSubScope equipSS = new TECSubScope();
            equipSS.Name = "Equipment SubScope";
            equipSS.Description = "Child SubScope";
            equipSS.AssociatedCosts.Add(testAssociatedCost);
            TECPoint equipPoint = new TECPoint();
            equipPoint.Type = PointTypes.AI;
            equipPoint.Name = "Equipment Point";
            equipPoint.Description = "Child Point";

            equipSS.Points.Add(equipPoint);
            equipSS.Devices.Add(childDev);
            equipSS.Tags.Add(ssTag);

            equipment.SubScope.Add(equipSS);
            equipment.Tags.Add(equipTag);

            templates.EquipmentTemplates.Add(equipment);

            //SubScope
            TECSubScope subScope = new TECSubScope();
            subScope.Name = "Test SubScope";
            subScope.Description = "SubScope Description";
            TECPoint ssPoint = new TECPoint();
            ssPoint.Type = PointTypes.BO;
            ssPoint.Name = "SubScope Point";
            ssPoint.Description = "Child Point";

            subScope.Points.Add(ssPoint);
            subScope.Devices.Add(childDev);
            subScope.Tags.Add(ssTag);
            subScope.AssociatedCosts.Add(testAssociatedCost);

            templates.SubScopeTemplates.Add(subScope);

            //Controller
            TECController expectedController = new TECController(testMan);
            expectedController.Name = "Test Controller";
            expectedController.Description = "Test description";
            expectedController.Cost = 42.6;

            TECController controlledController = new TECController(testMan);
            controlledController.Name = "Controlled Controller";

            TECIO ioToAdd = new TECIO();
            ioToAdd.Type = IOType.AI;
            ioToAdd.Quantity = 5;

            expectedController.IO.Add(ioToAdd);
            controlledController.IO.Add(ioToAdd);

            templates.ControllerTemplates.Add(expectedController);

            //Misc Cost
            TECMisc cost = new TECMisc();
            cost.Name = "Test Cost";
            cost.Cost = 69.69;
            cost.Quantity = 69;
            cost.Type = CostType.TEC;

            templates.MiscCostTemplates.Add(cost);

            //Misc wiring
            TECMisc wiring = new TECMisc();
            wiring.Name = "Test Wiring";
            wiring.Cost = 69.69;
            wiring.Quantity = 69;
            wiring.Type = CostType.Electrical;

            templates.MiscCostTemplates.Add(wiring);

            //Panel Types
            TECPanelType panelType = new TECPanelType();
            panelType.Cost = 123.4;
            panelType.Name = "Test Panel Type";

            templates.Catalogs.PanelTypes.Add(panelType);

            //Panels
            TECPanel panel = new TECPanel(panelType);
            panel.Name = "Test Panel";
            panel.Controllers.Add(expectedController);
            panel.AssociatedCosts.Add(testAssociatedCost);
            panel.AssociatedCosts.Add(testAssociatedCost);

            TECPanel controlledPanel = new TECPanel(panelType);
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
            //testConScope.Name = "Test Controlled Scope";
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

        public static TECBid LoadTestBid(string path)
        {
            TECBid testBid = DatabaseHelper.Load(path) as TECBid;
            return testBid;
        }

        public static TECTemplates LoadTestTemplates(string path)
        {
            return DatabaseHelper.Load(path) as TECTemplates;
        }

        public static TECBid CreateEstimatorBid()
        {
            TECBid bid = new TECBid();
            bid.Catalogs = CreateTestCatalogs();

            TECSubScope subScope = new TECSubScope();
            subScope.Devices.Add(bid.Catalogs.Devices[0]);
            TECEquipment equipment = new TECEquipment();
            equipment.SubScope.Add(subScope);
            TECSystem system = new TECSystem();
            system.Equipment.Add(equipment);
            bid.Systems.Add(system);

            TECController controller = new TECController(bid.Catalogs.Manufacturers[0]);
            bid.Controllers.Add(controller);
            controller.AddSubScope(subScope);
            subScope.Connection.Length = 10;
            subScope.Connection.ConduitLength = 10;

            return bid;
        }

        public static TECCatalogs CreateTestCatalogs()
        {
            TECCatalogs outCatalogs = new TECCatalogs();
            //Tags
            var tag1 = new TECTag();
            tag1.Text = "Tag 1";
            var tag2 = new TECTag();
            tag2.Text = "Test Tag";

            outCatalogs.Tags.Add(tag1);
            outCatalogs.Tags.Add(tag2);

            //Conduit Types
            var conduitType1 = new TECConduitType();
            conduitType1.Name = "Test Conduit 1";
            conduitType1.Cost = RandomInt(10, 100);
            conduitType1.Labor = RandomInt(10, 100);

            outCatalogs.ConduitTypes.Add(conduitType1);

            var conduitType2 = new TECConduitType();
            conduitType2.Name = "Test Conduit 2";
            conduitType2.Cost = RandomInt(10, 100);
            conduitType2.Labor = RandomInt(10, 100);

            outCatalogs.ConduitTypes.Add(conduitType2);

            //ConnectionTypes
            var connectionType1 = new TECConnectionType();
            connectionType1.Name = "FourC18";
            connectionType1.Cost = RandomInt(10, 100);
            connectionType1.Labor = RandomInt(10, 100);

            var connectionType2 = new TECConnectionType();
            connectionType2.Name = "FourC18";

            outCatalogs.ConnectionTypes.Add(connectionType1);
            outCatalogs.ConnectionTypes.Add(connectionType2);

            //Manufacturers
            var manufacturer1 = new TECManufacturer();
            manufacturer1.Name = "Test";
            manufacturer1.Multiplier = RandomDouble(0, 1);
            
            outCatalogs.Manufacturers.Add(manufacturer1);

            //Devices
            ObservableCollection<TECConnectionType> contypes4 = new ObservableCollection<TECConnectionType>();
            contypes4.Add(connectionType1);
            TECDevice device1 = new TECDevice(Guid.NewGuid(), contypes4, manufacturer1);
            device1.Name = "Device 1";
            device1.Description = "Description 1";
            device1.Cost = RandomInt(10, 100);
            device1.Tags.Add(tag1);

            outCatalogs.Devices.Add(device1);

            //IO Modules
            TECIOModule testIOModule = new TECIOModule();
            testIOModule.Name = "Test IO Module";
            testIOModule.Cost = RandomInt(10, 100);
            testIOModule.Manufacturer = manufacturer1;
            outCatalogs.IOModules.Add(testIOModule);

            //Panel Types
            TECPanelType panelType = new TECPanelType();
            panelType.Cost = RandomDouble(0, 1000);
            panelType.Labor = RandomDouble(0, 1000);
            panelType.Name = "Test Panel Type";

            outCatalogs.PanelTypes.Add(panelType);

            //Associated Costs
            TECCost testAssociatedCost = new TECCost();
            testAssociatedCost.Name = "Flex";
            testAssociatedCost.Cost = 42;
            testAssociatedCost.Labor = 39;
            testAssociatedCost.Type = CostType.Electrical;

            outCatalogs.AssociatedCosts.Add(testAssociatedCost);

            var testCost2 = new TECCost();
            testCost2.Name = "Other Cost";
            testCost2.Cost = 98;
            testCost2.Labor = 72;
            testCost2.Type = CostType.TEC;
            outCatalogs.AssociatedCosts.Add(testCost2);


            return outCatalogs;

        }

        public static TECSystem CreateTestSystem(TECCatalogs catalogs)
        {
            TECSystem outScope = new TECSystem();
            outScope.Tags.Add(catalogs.Tags.RandomObject());
            outScope.ProposeEquipment = true;
            var panel = new TECPanel(catalogs.PanelTypes[0]);

            outScope.Panels.Add(panel);
            var equipment = CreateTestEquipment(catalogs);
            outScope.Equipment.Add(equipment);

            var controller = new TECController(catalogs.Manufacturers[0]);
            outScope.Controllers.Add(controller);

            ConnectEquipmentToController(equipment, controller);
            panel.Controllers.Add(controller);

            var scopeBranch = new TECScopeBranch();
            outScope.ScopeBranches.Add(scopeBranch);

            var misc = new TECMisc();
            outScope.MiscCosts.Add(misc);

            outScope.AssociatedCosts.Add(catalogs.AssociatedCosts.RandomObject());

            return outScope;
        }
        public static TECDevice CreateTestDevice(TECCatalogs catalogs)
        {

            var connectionTypes = new ObservableCollection<TECConnectionType>();
            connectionTypes.Add(catalogs.ConnectionTypes.RandomObject());
            var manufacturer = catalogs.Manufacturers.RandomObject();

            double cost = RandomDouble(0, 100);

            var assCosts = new ObservableCollection<TECCost>();
            int costNum = RandomInt(0,10);
            for(int x = 0; x < costNum; x++)
            {
                assCosts.Add(catalogs.AssociatedCosts.RandomObject());
            }

            TECDevice device = new TECDevice(connectionTypes, manufacturer);
            device.Cost = cost;
            device.AssociatedCosts = assCosts;
            device.Tags.Add(catalogs.Tags.RandomObject());
            return device;
        }
        public static TECSubScope CreateTestSubScope(TECCatalogs catalogs)
        {
            var device = catalogs.Devices.RandomObject();
            var point = new TECPoint();
            point.Type = PointTypes.AI;
            point.Tags.Add(catalogs.Tags.RandomObject());

            var subScope = new TECSubScope();
            subScope.Tags.Add(catalogs.Tags.RandomObject());
            subScope.Devices.Add(device);
            subScope.Points.Add(point);
            return subScope;
        }
        public static TECPoint CreateTestPoint(TECCatalogs catalogs)
        {
            TECPoint point = new TECPoint();
            AssignSecondaryProperties(point, catalogs);
            point.Type = (PointTypes)Enum.GetNames(typeof(PointTypes)).Length;
            return point;
        }
        public static TECEquipment CreateTestEquipment(TECCatalogs catalogs)
        {
            var equipment = new TECEquipment();
            equipment.Tags.Add(catalogs.Tags.RandomObject());

            int subNumber = (new Random()).Next(1, 10);
            for(int x = 0; x < subNumber; x++)
            {
                equipment.SubScope.Add(CreateTestSubScope(catalogs));
            }
            
            return equipment;
        }
        public static TECController CreateTestController(TECCatalogs catalogs)
        {
            var manufacturer = catalogs.Manufacturers.RandomObject();

            var controlller = new TECController(manufacturer);
            controlller.Tags.Add(catalogs.Tags.RandomObject());
            return controlller;
        }
        public static TECPanel CreateTestPanel(TECCatalogs catalogs)
        {
            var panelType = catalogs.PanelTypes.RandomObject();

            var panel = new TECPanel(panelType);
            panel.Tags.Add(catalogs.Tags.RandomObject());
            return panel;
        }
        public static TECLabor CreateTestLabor()
        {
            var labor = new TECLabor();
            labor.PMCoef = 0.1;
            labor.PMRate = 0.11;
            labor.PMExtraHours = 1.1;

            labor.ENGCoef = 0.2;
            labor.ENGRate = 0.22;
            labor.ENGExtraHours = 2.2;

            labor.CommCoef = 0.3;
            labor.CommRate = 0.33;
            labor.CommExtraHours = 3.3;

            labor.SoftCoef = 0.4;
            labor.SoftRate = 0.44;
            labor.SoftExtraHours = 4.4;

            labor.GraphCoef = 0.5;
            labor.GraphRate = 0.55;
            labor.GraphExtraHours = 5.5;

            labor.ElectricalRate = 0.6;
            labor.ElectricalSuperRate = 0.66;
            labor.ElectricalSuperRatio = 1.0 / 7.0;

            labor.ElectricalNonUnionRate = 0.7;
            labor.ElectricalSuperNonUnionRate = 0.77;

            labor.ElectricalIsOnOvertime = true;
            labor.ElectricalIsUnion = true;
            return labor;
        }
        public static TECMisc CreateTestMisc(CostType type = 0)
        {
            TECMisc misc = new TECMisc();
            misc.Cost = RandomDouble(0, 1000);
            misc.Labor = RandomDouble(0, 1000);
            misc.Quantity = RandomInt(0, 100);
            if (type == 0)
            {
                misc.Type = (CostType)RandomInt(0, 3);
            }
            else
            {
                misc.Type = type;
            }
            return misc;
        }
        
        public static T RandomObject<T>(this ObservableCollection<T> list)
        {
            int index = 0;
            if(list.Count > 0)
            {
                index = RandomInt(0, list.Count);
                return list[index];
            }
            else
            {
                return default(T);
            }
            
            
        }
        public static int RandomInt(int min, int max)
        {
            return rand.Next(min, max);
        }
        public static double RandomDouble(double min, double max)
        {
            double multiplier = rand.NextDouble();
            return (multiplier * (max - min)) + min;
        }
        public static bool RandomBool()
        {
            int zeroOne = RandomInt(0, 2);
            return (zeroOne == 1);
        }
        public static void ConnectEquipmentToController(TECEquipment equipment, TECController controller)
        {
            foreach(TECSubScope subscope in equipment.SubScope)
            {
                controller.AddSubScope(subscope);
            }
        }

        public static TECDevice RandomDevice(this TECBid bid)
        {
            TECDevice device = null;
            foreach(TECSystem system in bid.Systems)
            {
                foreach(TECEquipment equipment in system.Equipment)
                {
                    foreach(TECSubScope subScope in equipment.SubScope)
                    {
                        device = subScope.Devices.RandomObject();
                        if(device != null)
                        {
                            return device;
                        }
                    }
                }
            }
            return device;
        }
        public static TECPoint RandomPoint(this TECBid bid)
        {
            TECPoint point = null;
            foreach (TECSystem system in bid.Systems)
            {
                foreach (TECEquipment equipment in system.Equipment)
                {
                    foreach (TECSubScope subScope in equipment.SubScope)
                    {
                        point = subScope.Points.RandomObject();
                        if (point != null)
                        {
                            return point;
                        }
                    }
                }
            }
            return point;
        }
        public static TECSubScope RandomSubScope(this TECBid bid)
        {
            TECSubScope subScope = null;
            foreach (TECSystem system in bid.Systems)
            {
                foreach (TECEquipment ewuipment in system.Equipment)
                {
                    subScope = ewuipment.SubScope.RandomObject();
                    if (subScope != null)
                    {
                        return subScope;
                    }
                }
            }
            return subScope;
        }
        public static TECEquipment RandomEquipment(this TECBid bid)
        {
            TECEquipment equipment = null;
            foreach (TECSystem system in bid.Systems)
            {
                equipment = system.Equipment.RandomObject();
                if (equipment != null)
                {
                    return equipment;
                }
            }
            return equipment;
        }

        public static TECScope FindScopeInSystems(ObservableCollection<TECSystem> systems, TECScope reference)
        {
            foreach(TECSystem system in systems)
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
        public static TECController FindControllerInController(ObservableCollection<TECController> controllers, TECController reference)
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

        public static void AssignSecondaryProperties(TECScope scope, TECBid bid)
        {
            scope.Location = bid.Locations.RandomObject();
            AssignSecondaryProperties(scope, bid.Catalogs);
        }
        public static void AssignSecondaryProperties(TECScope scope, TECCatalogs catalogs)
        {
            if (scope.Tags.Count == 0)
            {
                scope.Tags.Add(catalogs.Tags.RandomObject());
            }
            if (scope.AssociatedCosts.Count == 0)
            {
                scope.AssociatedCosts.Add(catalogs.AssociatedCosts.RandomObject());
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
    }
}
