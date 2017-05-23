using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EstimatingLibrary;
using EstimatingUtilitiesLibrary;
using System.Collections.ObjectModel;
using System.IO;

namespace Tests
{
    public static class TestHelper
    {
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
            bid.Labor.PMCoef = 0.1;
            bid.Labor.PMRate = 0.11;
            bid.Labor.PMExtraHours = 1.1;

            bid.Labor.ENGCoef = 0.2;
            bid.Labor.ENGRate = 0.22;
            bid.Labor.ENGExtraHours = 2.2;

            bid.Labor.CommCoef = 0.3;
            bid.Labor.CommRate = 0.33;
            bid.Labor.CommExtraHours = 3.3;

            bid.Labor.SoftCoef = 0.4;
            bid.Labor.SoftRate = 0.44;
            bid.Labor.SoftExtraHours = 4.4;

            bid.Labor.GraphCoef = 0.5;
            bid.Labor.GraphRate = 0.55;
            bid.Labor.GraphExtraHours = 5.5;

            bid.Labor.ElectricalRate = 0.6;
            bid.Labor.ElectricalSuperRate = 0.66;

            bid.Labor.ElectricalNonUnionRate = 0.7;
            bid.Labor.ElectricalSuperNonUnionRate = 0.77;

            bid.Labor.ElectricalIsOnOvertime = true;
            bid.Labor.ElectricalIsUnion = true;

            //Tags
            var tag1 = new TECTag();
            tag1.Text = "Tag 1";
            var tag2 = new TECTag();
            tag2.Text = "Test Tag";

            bid.Catalogs.Tags.Add(tag1);
            bid.Catalogs.Tags.Add(tag2);


            //Associated Costs
            var testCost = new TECCost();
            testCost.Name = "Test Cost";
            bid.Catalogs.AssociatedCosts.Add(testCost);

            var testCost2 = new TECCost();
            testCost2.Name = "Other Cost";
            bid.Catalogs.AssociatedCosts.Add(testCost2);
            
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

            //Systems
            var system1 = new TECSystem();
            system1.Name = "System 1";
            system1.Description = "Locations all the way";
            system1.BudgetPriceModifier = 234.5;
            system1.Quantity = 2345;
            system1.Location = location1;

            system1.Tags.Add(tag1);

            var system2 = new TECSystem();
            system2.Name = "System 2";
            system2.Description = "Description 2";
            system2.BudgetPriceModifier = 234.52;
            system2.Quantity = 23452;
            system2.Location = location2;

            var system3 = new TECSystem();
            system3.Name = "System 3";
            system3.Description = "No Location";
            system3.BudgetPriceModifier = 349;

            //Add to bid
            bid.Systems.Add(system1);
            bid.Systems.Add(system2);
            bid.Systems.Add(system3);

            //Equipment
            var equipment1 = new TECEquipment();
            equipment1.Name = "Equipment 1";
            equipment1.Description = "Description 1";
            equipment1.BudgetUnitPrice = 123.4;
            equipment1.Quantity = 1234;
            equipment1.Location = location1;

            equipment1.Tags.Add(tag1);

            var equipment2 = new TECEquipment();
            equipment1.Name = "Equipment 2";
            equipment1.Description = "Description 2";
            equipment1.BudgetUnitPrice = 0;

            system1.Equipment.Add(equipment1);
            system3.Equipment.Add(equipment2);

            //SubScope
            var subScope1 = new TECSubScope();
            subScope1.Name = "SubScope 1";
            subScope1.Description = "Description 1";
            subScope1.Quantity = 654;
            subScope1.Location = location3;
            subScope1.AssociatedCosts.Add(testCost);
            subScope1.AssociatedCosts.Add(testCost);
            subScope1.AssociatedCosts.Add(testCost2);
            subScope1.Tags.Add(tag1);

            var subScope2 = new TECSubScope();
            subScope2.Name = "Empty SubScope";
            subScope2.Description = "Description 2";
            subScope2.AssociatedCosts.Add(testCost);
            equipment1.SubScope.Add(subScope1);
            equipment2.SubScope.Add(subScope2);

            //Conduit Types
            var conduitType1 = new TECConduitType();
            conduitType1.Name = "Test Conduit 1";
            conduitType1.Cost = 13;
            conduitType1.Labor = 14;

            bid.Catalogs.ConduitTypes.Add(conduitType1);

            var conduitType2 = new TECConduitType();
            conduitType2.Name = "Test Conduit 2";
            conduitType2.Cost = 13;
            conduitType2.Labor = 14;

            bid.Catalogs.ConduitTypes.Add(conduitType2);

            //ConnectionTypes
            var connectionType1 = new TECConnectionType();
            connectionType1.Name = "ThreeC18";
            connectionType1.Cost = 10;
            connectionType1.Labor = 12;

            var connectionType2 = new TECConnectionType();
            connectionType2.Name = "FourC18";

            bid.Catalogs.ConnectionTypes.Add(connectionType1);
            bid.Catalogs.ConnectionTypes.Add(connectionType2);

            //Manufacturers
            var manufacturer1 = new TECManufacturer();
            manufacturer1.Name = "Test";
            manufacturer1.Multiplier = 0.8;
            bid.Catalogs.Manufacturers.Add(manufacturer1);

            //Devices
            ObservableCollection<TECConnectionType> types = new ObservableCollection<TECConnectionType>();
            types.Add(connectionType1);
            TECDevice device1 = new TECDevice(Guid.NewGuid(),
                types,
                manufacturer1);
            device1.Name = "Device 1";
            device1.Description = "Description 1";
            device1.Cost = 987.6;
            device1.Tags.Add(tag1);

            subScope1.Devices.Add(device1);
            subScope1.Devices.Add(device1);
            subScope1.Devices.Add(device1);
            

            //Points
            var point1 = new TECPoint();
            point1.Name = "Point 1";
            point1.Description = "Description 1";
            point1.Type = PointTypes.Serial;
            point1.Quantity = 321;
            point1.Tags.Add(tag1);

            subScope1.Points.Add(point1);

            ////VisualScope
            //var vScope = new TECVisualScope(system1, 4.2, 4.2);


            ////Drawings
            //var drawing1 = PDFConverter.convertPDFToDrawing(TestPDF1);
            //drawing1.Name = "Test";
            //drawing1.Description = "Desc";

            //bid.Drawings.Add(drawing1);

            //drawing1.Pages[0].PageScope.Add(vScope);

            //Devices Catalog
            ObservableCollection<TECConnectionType> contypes = new ObservableCollection<TECConnectionType>();
            types.Add(connectionType2);
            TECDevice deviceC1 = new TECDevice(Guid.NewGuid(), contypes, manufacturer1);
            deviceC1.Name = "Device C1";
            deviceC1.Description = "Description C1";
            deviceC1.Cost = 987.6;

            deviceC1.Manufacturer = manufacturer1;
            bid.Catalogs.Devices.Add(deviceC1);
            bid.Catalogs.Devices.Add(device1);

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
            TECController expectedController = new TECController(Guid.NewGuid(), manufacturer1);
            expectedController.Name = "Test Controller";
            expectedController.Description = "Test description";
            expectedController.Cost = 42.6;
            expectedController.Manufacturer = manufacturer1;

            TECIO ioToAdd = new TECIO();
            ioToAdd.Type = IOType.AI;
            ioToAdd.Quantity = 5;
            expectedController.IO.Add(ioToAdd);
            bid.Controllers.Add(expectedController);

            //ProposalScope
            TECSystem propSystem = new TECSystem();
            propSystem.Name = "Prop System";

            TECScopeBranch propNote = new TECScopeBranch();
            propNote.Name = "Proposal Note";
            TECScopeBranch propNoteNote = new TECScopeBranch();
            propNoteNote.Name = "Proposal Note Note";
            propNote.Branches.Add(propNoteNote);

            bid.Systems.Add(propSystem);

            TECProposalScope propScope = null;
            foreach (TECProposalScope pS in bid.ProposalScope)
            {
                if (pS.Scope.Guid == propSystem.Guid)
                {
                    propScope = pS;
                    break;
                }
            }
            propScope.IsProposed = true;
            propScope.Notes.Add(propNote);

            //Misc Cost
            TECMiscCost cost = new TECMiscCost();
            cost.Name = "Test Cost";
            cost.Cost = 69.69;
            cost.Quantity = 69;

            bid.MiscCosts.Add(cost);

            //Misc wiring
            TECMiscWiring wiring = new TECMiscWiring();
            wiring.Name = "Test Wiring";
            wiring.Cost = 69.69;
            wiring.Quantity = 69;

            bid.MiscWiring.Add(wiring);

            //Panel Types
            TECPanelType panelType = new TECPanelType();
            panelType.Cost = 123.4;
            panelType.Name = "Test Panel Type";

            bid.Catalogs.PanelTypes.Add(panelType);

            //Panels
            TECPanel panel = new TECPanel(panelType);
            panel.Name = "Test Panel";
            panel.Controllers.Add(expectedController);
            panel.AssociatedCosts.Add(testCost);
            panel.AssociatedCosts.Add(testCost);

            bid.Panels.Add(panel);

            //Connections
            TECConnection testConnection = expectedController.AddSubScope(subScope1);
            testConnection.ConduitType = conduitType1;
            testConnection.Length = 42;

            //IO Modules
            TECIOModule testIOModule = new TECIOModule();
            testIOModule.Name = "Test IO Module";
            testIOModule.Cost = 42;
            testIOModule.Manufacturer = manufacturer1;
            bid.Catalogs.IOModules.Add(testIOModule);
            ioToAdd.IOModule = testIOModule;

            var controlledScope = CreateTestControlledScope(bid.Catalogs);
            bid.ControlledScope.Add(controlledScope);

            //Bid
            return bid;
        }

        public static TECTemplates CreateTestTemplates()
        {
            TECTemplates templates = new TECTemplates();

            //Labor
            templates.Labor.PMCoef = 1.1;
            templates.Labor.PMRate = 2.2;
            templates.Labor.ENGCoef = 3.3;
            templates.Labor.ENGRate = 4.4;
            templates.Labor.CommCoef = 5.5;
            templates.Labor.CommRate = 6.6;
            templates.Labor.SoftCoef = 7.7;
            templates.Labor.SoftRate = 8.8;
            templates.Labor.GraphCoef = 9.9;
            templates.Labor.GraphRate = 10.10;
            templates.Labor.ElectricalRate = 11.11;
            templates.Labor.ElectricalSuperRate = 12.12;
            templates.Labor.ElectricalNonUnionRate = 13.13;
            templates.Labor.ElectricalSuperNonUnionRate = 14.14;

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

            templates.Catalogs.AssociatedCosts.Add(testAssociatedCost);

            var testCost2 = new TECCost();
            testCost2.Name = "Other Cost";
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

            TECSystem controlledSystem = new TECSystem();
            controlledSystem.Name = "Controlled System";

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
            TECMiscCost cost = new TECMiscCost();
            cost.Name = "Test Cost";
            cost.Cost = 69.69;
            cost.Quantity = 69;

            templates.MiscCostTemplates.Add(cost);

            //Misc wiring
            TECMiscWiring wiring = new TECMiscWiring();
            wiring.Name = "Test Wiring";
            wiring.Cost = 69.69;
            wiring.Quantity = 69;

            templates.MiscWiringTemplates.Add(wiring);

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
            TECControlledScope testConScope = new TECControlledScope();
            testConScope.Name = "Test Controlled Scope";
            testConScope.Description = "Test Description";
            var controlledScopeSystem = controlledSystem.DragDropCopy() as TECSystem;
            testConScope.Systems.Add(controlledScopeSystem);
            var controlledScopePanel = controlledPanel.DragDropCopy() as TECPanel;
            controlledScopePanel.Type = panelType;
            testConScope.Panels.Add(controlledScopePanel);
            var controlledScopeController = controlledController.DragDropCopy() as TECController;
            controlledScopePanel.Controllers.Add(controlledScopeController);
            testConScope.Controllers.Add(controlledScopeController);
            var connection = controlledScopeController.AddSubScope(subScope.DragDropCopy() as TECSubScope);
            connection.Length = 42;
            controlledScopeController.ChildrenConnections[0].ConduitType = testConduitType;

            templates.ControlledScopeTemplates.Add(testConScope);

            return templates;
        }

        public static TECBid LoadTestBid(string path)
        {
            TECBid testBid = EstimatingLibraryDatabase.Load(path) as TECBid;
            return testBid;
        }

        public static TECTemplates LoadTestTemplates(string path)
        {
            return EstimatingLibraryDatabase.Load(path) as TECTemplates;
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
            conduitType1.Cost = 13;
            conduitType1.Labor = 14;

            outCatalogs.ConduitTypes.Add(conduitType1);

            var conduitType2 = new TECConduitType();
            conduitType2.Name = "Test Conduit 2";
            conduitType2.Cost = 13;
            conduitType2.Labor = 14;

            outCatalogs.ConduitTypes.Add(conduitType2);

            //ConnectionTypes
            var connectionType1 = new TECConnectionType();
            connectionType1.Name = "FourC18";
            connectionType1.Cost = 10;
            connectionType1.Labor = 12;

            var connectionType2 = new TECConnectionType();
            connectionType2.Name = "FourC18";

            outCatalogs.ConnectionTypes.Add(connectionType1);
            outCatalogs.ConnectionTypes.Add(connectionType2);

            //Manufacturers
            var manufacturer1 = new TECManufacturer();
            manufacturer1.Name = "Test";
            manufacturer1.Multiplier = 0.8;
            
            outCatalogs.Manufacturers.Add(manufacturer1);

            //Devices
            ObservableCollection<TECConnectionType> contypes4 = new ObservableCollection<TECConnectionType>();
            contypes4.Add(connectionType1);
            TECDevice device1 = new TECDevice(Guid.NewGuid(), contypes4, manufacturer1);
            device1.Name = "Device 1";
            device1.Description = "Description 1";
            device1.Cost = 987.6;
            device1.Tags.Add(tag1);

            outCatalogs.Devices.Add(device1);

            //IO Modules
            TECIOModule testIOModule = new TECIOModule();
            testIOModule.Name = "Test IO Module";
            testIOModule.Cost = 42;
            testIOModule.Manufacturer = manufacturer1;
            outCatalogs.IOModules.Add(testIOModule);

            //Panel Types
            TECPanelType panelType = new TECPanelType();
            panelType.Cost = 123.4;
            panelType.Name = "Test Panel Type";

            outCatalogs.PanelTypes.Add(panelType);

            return outCatalogs;

        }

        public static TECControlledScope CreateTestControlledScope(TECCatalogs catalogs)
        {
            TECControlledScope outScope = new TECControlledScope();

            var panel = new TECPanel(catalogs.PanelTypes[0]);

            outScope.Panels.Add(panel);

            var system = new TECSystem();
            outScope.Systems.Add(system);

            var controller = new TECController(catalogs.Manufacturers[0]);
            outScope.Controllers.Add(controller);

            panel.Controllers.Add(controller);

            return outScope;
        }
    }
}
