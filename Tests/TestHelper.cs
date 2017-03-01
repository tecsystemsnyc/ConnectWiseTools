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
        static public string StaticTestBidPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + @"\Resources\StaticTestBid.bdb";
        static public string StaticTestTemplatesPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + @"\Resources\StaticTestTemplates.tdb";
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
            bid.Parameters = new TECBidParameters();

            //Bid Labor
            bid.Labor = new TECLabor();
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

            //Tags
            var tag1 = new TECTag();
            tag1.Text = "Tag 1";
            var tag2 = new TECTag();
            tag2.Text = "Test Tag";

            bid.Tags.Add(tag1);
            bid.Tags.Add(tag2);

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
            system1.BudgetPrice = 234.5;
            system1.Quantity = 2345;
            system1.Location = location1;
            
            system1.Tags.Add(tag1);

            var system2 = new TECSystem();
            system2.Name = "System 2";
            system2.Description = "Description 2";
            system2.BudgetPrice = 234.52;
            system2.Quantity = 23452;
            system2.Location = location2;

            var system3 = new TECSystem();
            system3.Name = "System 3";
            system3.Description = "No Location";
            system3.BudgetPrice = 349;
            
            //Add to bid
            bid.Systems.Add(system1);
            bid.Systems.Add(system2);
            bid.Systems.Add(system3);
            
            //Equipment
            var equipment1 = new TECEquipment();
            equipment1.Name = "Equipment 1";
            equipment1.Description = "Description 1";
            equipment1.BudgetPrice = 123.4;
            equipment1.Quantity = 1234;
            equipment1.Location = location1;
           
            equipment1.Tags.Add(tag1);

            var equipment2 = new TECEquipment();
            equipment1.Name = "Equipment 2";
            equipment1.Description = "Description 2";
            equipment1.BudgetPrice = 0;

            system1.Equipment.Add(equipment1);
            system3.Equipment.Add(equipment2);

            //SubScope
            var subScope1 = new TECSubScope();
            subScope1.Name = "SubScope 1";
            subScope1.Description = "Description 1";
            subScope1.Quantity = 654;
            subScope1.Location = location3;
            
            subScope1.Tags.Add(tag1);

            var subScope2 = new TECSubScope();
            subScope2.Name = "Empty SubScope";
            subScope2.Description = "Description 2";
            equipment1.SubScope.Add(subScope1);
            equipment2.SubScope.Add(subScope2);

            //Devices
            TECDevice device1 = new TECDevice(Guid.NewGuid());
            device1.Name = "Device 1";
            device1.Description = "Description 1";
            device1.Cost = 987.6;
            device1.Tags.Add(tag1);
            
            subScope1.Devices.Add(device1);
            subScope1.Devices.Add(device1);
            subScope1.Devices.Add(device1);

            //ConnectionTypes
            var connectionType1 = new TECConnectionType();
            connectionType1.Name = "FourC18";
            connectionType1.Cost = 10;
            connectionType1.Labor = 12;

            var connectionType2 = new TECConnectionType();
            connectionType2.Name = "FourC18";

            device1.ConnectionType = connectionType1;
            bid.ConnectionTypes.Add(connectionType1);
            bid.ConnectionTypes.Add(connectionType2);
            //Manufacturers
            var manufacturer1 = new TECManufacturer();
            manufacturer1.Name = "Test";
            manufacturer1.Multiplier = 0.8;
            
            device1.Manufacturer = manufacturer1;
            bid.ManufacturerCatalog.Add(manufacturer1);

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
            TECDevice deviceC1 = new TECDevice(Guid.NewGuid());
            device1.Name = "Device C1";
            device1.Description = "Description C1";
            device1.Cost = 987.6;

            deviceC1.ConnectionType = connectionType2;
            bid.DeviceCatalog.Add(deviceC1);
            bid.DeviceCatalog.Add(device1);

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
            TECController expectedController = new TECController(Guid.NewGuid());
            expectedController.Name = "Test Controller";
            expectedController.Description = "Test description";
            expectedController.Cost = 42.6;

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
                }
            }
            propScope.IsProposed = true;
            propScope.Notes.Add(propNote);

            //Bid
            return bid;
        }

        public static TECTemplates CreateTestTemplates()
        {
            TECTemplates templates = new TECTemplates();

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

            templates.Tags.Add(testTag);
            templates.Tags.Add(sysTag);
            templates.Tags.Add(equipTag);
            templates.Tags.Add(ssTag);
            templates.Tags.Add(devTag);

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


            templates.ManufacturerCatalog.Add(testMan);
            templates.ManufacturerCatalog.Add(testDevMan);
            templates.ManufacturerCatalog.Add(childDevMan);

            //Connection Types
            TECConnectionType testDevConnType = new TECConnectionType();
            testDevConnType.Name = "FourC18";

            TECConnectionType childDevConnType = new TECConnectionType();
            childDevConnType.Name = "FourC18";

            templates.ConnectionTypeCatalog.Add(testDevConnType);
            templates.ConnectionTypeCatalog.Add(childDevConnType);

            //Conduit Types
            TECConduitType testConduitType = new TECConduitType();
            testConduitType.Name = "EMT";
            testConduitType.Cost = 12;
            testConduitType.Labor = 2;

            templates.ConduitTypeCatalog.Add(testConduitType);

            //Associated Costs
            TECAssociatedCost testAssociatedCost = new TECAssociatedCost();
            testAssociatedCost.Name = "Flex";
            testAssociatedCost.Cost = 42;

            templates.AssociatedCostsCatalog.Add(testAssociatedCost);

            //Devices
            TECDevice testDev = new TECDevice(Guid.NewGuid());
            testDev.Name = "Test Device";
            testDev.Description = "Device Description";
            testDev.Cost = 20.3;
            testDev.Manufacturer = testDevMan;
            testDev.ConnectionType = testDevConnType;

            
            TECDevice childDev = new TECDevice(Guid.NewGuid());
            childDev.Name = "Child Device";
            childDev.Description = "Child Device Description";
            childDev.Cost = 54.1;
            childDev.Manufacturer = childDevMan;
            childDev.ConnectionType = childDevConnType;
            
            testDev.Tags.Add(devTag);
            childDev.Tags.Add(devTag);

            templates.DeviceCatalog.Add(testDev);
            templates.DeviceCatalog.Add(childDev);

            //System
            TECSystem system = new TECSystem();
            system.Name = "Test System";
            system.Description = "System Description";
            system.BudgetPrice = 587.3;

            TECEquipment sysEquip = new TECEquipment();
            sysEquip.Name = "System Equipment";
            sysEquip.Description = "Child Equipment";
            sysEquip.BudgetPrice = 489.5;
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
            equipment.BudgetPrice = 193.2;
            TECSubScope equipSS = new TECSubScope();
            equipSS.Name = "Equipment SubScope";
            equipSS.Description = "Child SubScope";
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
            subScope.ConduitType = testConduitType;
            subScope.AssociatedCosts.Add(testAssociatedCost);

            templates.SubScopeTemplates.Add(subScope);

            //Controller
            TECController expectedController = new TECController(Guid.NewGuid());
            expectedController.Name = "Test Controller";
            expectedController.Description = "Test description";
            expectedController.Cost = 42.6;

            TECIO ioToAdd = new TECIO();
            ioToAdd.Type = IOType.AI;
            ioToAdd.Quantity = 5;
            expectedController.IO.Add(ioToAdd);
            expectedController.Manufacturer = testMan;

            templates.ControllerTemplates.Add(expectedController);

            return templates;
        }

        public static TECBid LoadTestBid(string path)
        {
            TECBid testBid = EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());
            return testBid;
        }

        public static TECTemplates LoadTestTemplates(string path)
        {
            return EstimatingLibraryDatabase.LoadDBToTemplates(path);
        }
    }
}
