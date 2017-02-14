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

            //Bid Labor
            bid.Labor.PMCoef = 0.1;
            bid.Labor.ENGCoef = 0.2;
            bid.Labor.CommCoef = 0.3;
            bid.Labor.SoftCoef = 0.4;
            bid.Labor.GraphCoef = 0.5;
            bid.Labor.ElectricalRate = 0.6;

            //Tags
            var tag1 = new TECTag("Tag 1");
            var tag2 = new TECTag("Test Tag");

            bid.Tags.Add(tag1);
            bid.Tags.Add(tag2);

            //Locations
            var cellar = new TECLocation("Cellar");
            var location1 = new TECLocation("1st Floor");
            var location2 = new TECLocation("2nd Floor");
            var location3 = new TECLocation("3rd Floor");

            
            bid.Locations.Add(cellar);
            bid.Locations.Add(location1);
            bid.Locations.Add(location2);
            bid.Locations.Add(location3);

            //Systems
            var system1 = new TECSystem("System 1", "Locations all the way", 234.5, new ObservableCollection<TECEquipment>());
            system1.Quantity = 2345;
            system1.Location = location1;
            
            system1.Tags.Add(tag1);

            var system2 = new TECSystem("System 2", "Description 2", 234.52, new ObservableCollection<TECEquipment>());
            system2.Quantity = 23452;
            system2.Location = location2;

            var system3 = new TECSystem("System 3", "No Location", 349, new ObservableCollection<TECEquipment>());


            //Add to bid
            bid.Systems.Add(system1);
            bid.Systems.Add(system2);
            bid.Systems.Add(system3);
            
            //Equipment
            var equipment1 = new TECEquipment("Equipment 1", "Description 1", 123.4, new ObservableCollection<TECSubScope>());
            equipment1.Quantity = 1234;
            equipment1.Location = location1;
           
            equipment1.Tags.Add(tag1);

            var equipment2 = new TECEquipment("Equipment 2", "Description 2", 0, new ObservableCollection<TECSubScope>());
            

            system1.Equipment.Add(equipment1);
            system3.Equipment.Add(equipment2);

            //SubScope
            var subScope1 = new TECSubScope("SubScope 1", "Description 1", new ObservableCollection<TECDevice>(), new ObservableCollection<TECPoint>());
            subScope1.Quantity = 654;
            subScope1.Location = location3;
            
            subScope1.Tags.Add(tag1);

            var subScope2 = new TECSubScope("Empty SubScope", "Description 2", new ObservableCollection<TECDevice>(), new ObservableCollection<TECPoint>());

            equipment1.SubScope.Add(subScope1);
            equipment2.SubScope.Add(subScope2);

            //Devices
            var device1 = new TECDevice("Device 1", "Description 1", 987.6, new TECManufacturer(), Guid.NewGuid());
            var connectionType1 = new TECConnectionType();
            connectionType1.Name = "FourC18";
            device1.ConnectionType = connectionType1;
            device1.Quantity = 3;
            device1.Tags.Add(tag1);
            
            subScope1.Devices.Add(device1);
            subScope1.Devices.Add(device1);
            subScope1.Devices.Add(device1);

            //Manufacturers
            var manufacturer1 = new TECManufacturer("Test", 0.8);
            
            device1.Manufacturer = manufacturer1;
            bid.ManufacturerCatalog.Add(manufacturer1);

            //Points
            var point1 = new TECPoint(PointTypes.Serial, "Point 1", "Description 1");
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
            var deviceC1 = new TECDevice("Device C1", "Description C1", 987.6, new TECManufacturer(), Guid.NewGuid());
            var connectionType2 = new TECConnectionType();
            connectionType2.Name = "FourC18";
            deviceC1.ConnectionType = connectionType2;
            bid.DeviceCatalog.Add(deviceC1);
            bid.DeviceCatalog.Add(device1);

            //Scope Branches
            var branch1 = new TECScopeBranch("Branch 1", "1st Description", new ObservableCollection<TECScopeBranch>());
            var branch2 = new TECScopeBranch("Branch 2", "2nd Description", new ObservableCollection<TECScopeBranch>());
            var branch3 = new TECScopeBranch("Branch 3", "3rd Description", new ObservableCollection<TECScopeBranch>());

            bid.ScopeTree.Add(branch1);
            branch1.Branches.Add(branch2);
            branch2.Branches.Add(branch3);
            
            //Notes
            var note1 = new TECNote("Note 1");

            bid.Notes.Add(note1);

            //Exclusions
            var exclusion1 = new TECExclusion("Exlusions 1");

            bid.Exclusions.Add(exclusion1);

            //Controller
            TECController controller = new TECController("Test Controller", "test description", Guid.NewGuid(), 42.6);
            TECIO ioToAdd = new TECIO(IOType.AI);
            ioToAdd.Quantity = 5;
            controller.IO.Add(ioToAdd);
            bid.Controllers.Add(controller);

            //ProposalScope
            TECSystem propSystem = new TECSystem("Prop System", "", 0, new ObservableCollection<TECEquipment>());

            TECScopeBranch propNote = new TECScopeBranch("Proposal Note", "", new ObservableCollection<TECScopeBranch>());
            TECScopeBranch propNoteNote = new TECScopeBranch("Proposal Note Note", "", new ObservableCollection<TECScopeBranch>());
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
            TECTag testTag = new TECTag("Test Tag");
            TECTag sysTag = new TECTag("System Tag");
            TECTag equipTag = new TECTag("Equipment Tag");
            TECTag ssTag = new TECTag("SubScope Tag");
            TECTag devTag = new TECTag("Device Tag");

            templates.Tags.Add(testTag);
            templates.Tags.Add(sysTag);
            templates.Tags.Add(equipTag);
            templates.Tags.Add(ssTag);
            templates.Tags.Add(devTag);

            //Manufacturers
            TECManufacturer testMan = new TECManufacturer("Test Manufacturer", 0.654);
            TECManufacturer testDevMan = new TECManufacturer("Child Manufacturer (Test Device)", 0.446);
            TECManufacturer childDevMan = new TECManufacturer("Child Manufacturer (Child Device)", 0.916);

            templates.ManufacturerCatalog.Add(testMan);
            templates.ManufacturerCatalog.Add(testDevMan);
            templates.ManufacturerCatalog.Add(childDevMan);

            //Devices
            TECConnectionType testDevConnType = new TECConnectionType();
            testDevConnType.Name = "FourC18";
            TECDevice testDev = new TECDevice("Test Device", "Device Description", 20.3, testDevMan, Guid.NewGuid());
            testDev.ConnectionType = testDevConnType;

            TECConnectionType childDevConnType = new TECConnectionType();
            childDevConnType.Name = "FourC18";
            TECDevice childDev = new TECDevice("Child Device", "Child Device Description", 54.1, childDevMan, Guid.NewGuid());
            childDev.ConnectionType = childDevConnType;

            testDev.Tags.Add(devTag);
            childDev.Tags.Add(devTag);

            templates.DeviceCatalog.Add(testDev);
            templates.DeviceCatalog.Add(childDev);

            //System
            TECSystem system = new TECSystem("Test System", "System Description", 587.3, new ObservableCollection<TECEquipment>());
            TECEquipment sysEquip = new TECEquipment("System Equipment", "Child Equipment", 489.5, new ObservableCollection<TECSubScope>());
            TECSubScope sysSS = new TECSubScope("System SubScope", "Child SubScope", new ObservableCollection<TECDevice>(), new ObservableCollection<TECPoint>());
            TECPoint sysPoint = new TECPoint(PointTypes.Serial, "System Point", "Child Point");

            sysSS.Points.Add(sysPoint);
            sysSS.Devices.Add(childDev);
            sysSS.Tags.Add(ssTag);

            sysEquip.SubScope.Add(sysSS);
            sysEquip.Tags.Add(equipTag);

            system.Equipment.Add(sysEquip);
            system.Tags.Add(sysTag);

            templates.SystemTemplates.Add(system);

            //Equipment
            TECEquipment equipment = new TECEquipment("Test Equipment", "Equipment Description", 193.2, new ObservableCollection<TECSubScope>());
            TECSubScope equipSS = new TECSubScope("Equipment SubScope", "Child SubScope", new ObservableCollection<TECDevice>(), new ObservableCollection<TECPoint>());
            TECPoint equipPoint = new TECPoint(PointTypes.AI, "Equipment Point", "Child Point");

            equipSS.Points.Add(equipPoint);
            equipSS.Devices.Add(childDev);
            equipSS.Tags.Add(ssTag);

            equipment.SubScope.Add(equipSS);
            equipment.Tags.Add(equipTag);

            templates.EquipmentTemplates.Add(equipment);

            //SubScope
            TECSubScope subScope = new TECSubScope("Test SubScope", "SubScope Description", new ObservableCollection<TECDevice>(), new ObservableCollection<TECPoint>());
            TECPoint ssPoint = new TECPoint(PointTypes.BO, "SubScope Point", "Child Point");

            subScope.Points.Add(ssPoint);
            subScope.Devices.Add(childDev);
            subScope.Tags.Add(ssTag);

            templates.SubScopeTemplates.Add(subScope);

            //Controller
            TECController controller = new TECController("Test Controller", "test description", Guid.NewGuid(), 42.6);
            TECIO ioToAdd = new TECIO(IOType.AI);
            ioToAdd.Quantity = 5;
            controller.IO.Add(ioToAdd);

            templates.ControllerTemplates.Add(controller);

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
