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

        public static TECBid CreateTestBid()
        {
            TECBid bid = new TECBid();

            //Bid Info
            bid.Name = "Bid Name";
            bid.BidNumber = "1701-117";
            bid.DueDate = DateTime.Now;
            bid.Salesperson = "Mrs. Test";
            bid.Estimator = "Mr. Test";

            //Locations
            var location1 = new TECLocation("1st Floor");
            var location2 = new TECLocation("2nd Floor");
            var location3 = new TECLocation("3rd Floor");
            var allLocations = new ObservableCollection<TECLocation>();
            allLocations.Add(location1);
            allLocations.Add(location2);
            allLocations.Add(location3);

            //Points
            var point1 = new TECPoint(PointTypes.Serial, "Point 1", "Description 1");
            point1.Quantity = 321;
            var allPoints = new ObservableCollection<TECPoint>();
            allPoints.Add(point1);

            //Devices
            var device1 = new TECDevice("Device 1", "Description 1", 987.6, "Test Wire", new TECManufacturer());
            device1.Quantity = 987;
            var allDevices = new ObservableCollection<TECDevice>();
            allDevices.Add(device1);

            //SubScope
            var subScope1 = new TECSubScope("SubScope 1", "Description 1", allDevices, allPoints);
            subScope1.Quantity = 654;
            subScope1.Location = location3;

            var allSubScope = new ObservableCollection<TECSubScope>();
            allSubScope.Add(subScope1);

            //Equipment
            var equipment1 = new TECEquipment("Equipment 1", "Description 1", 123.4, allSubScope);
            equipment1.Quantity = 1234;
            equipment1.Location = location1;

            var allEquipment = new ObservableCollection<TECEquipment>();
            allEquipment.Add(equipment1);

            //Systems
            var system1 = new TECSystem("System 1", "Description 1", 234.5, allEquipment);
            system1.Quantity = 2345;
            system1.Location = location1;

            var system2 = new TECSystem("System 2", "Description 2", 234.52, new ObservableCollection<TECEquipment>());
            system2.Quantity = 23452;
            system2.Location = location2;

            var allSystems = new ObservableCollection<TECSystem>();
            allSystems.Add(system1);
            allSystems.Add(system2);

            //Pages
            var pages1 = new TECPage("Testpath", 2);
            var allPages = new ObservableCollection<TECPage>();

            //Drawings
            var drawing1 = new TECDrawing("Test", "Desc", Guid.NewGuid(), allPages);
            var allDrawings = new ObservableCollection<TECDrawing>();
            allDrawings.Add(drawing1);

            //Manufacturers
            var manufacturer1 = new TECManufacturer("Test", "Desc", 0.8);
            var allManufacturers = new ObservableCollection<TECManufacturer>();
            allManufacturers.Add(manufacturer1);

            //Tags
            var tag1 = new TECTag("Test");
            var allTags = new ObservableCollection<TECTag>();
            allTags.Add(tag1);

            //Devices Catalog
            var deviceC1 = new TECDevice("Device C1", "Description C1", 987.6, "Test Wire", new TECManufacturer());
            var deviceCatalog = new ObservableCollection<TECDevice>();
            deviceCatalog.Add(deviceC1);
            deviceCatalog.Add(device1);

            //Bid
            bid.Systems = allSystems;
            bid.DeviceCatalog = deviceCatalog;
            bid.Drawings = allDrawings;
            bid.ManufacturerCatalog = allManufacturers;
            bid.Tags = allTags;
            bid.Locations = allLocations;

            return bid;
        }

        public static TECBid LoadTestBid(string path)
        {
            return EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());
        }
    }
}
