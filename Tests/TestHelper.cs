using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EstimatingLibrary;
using EstimatingUtilitiesLibrary;
using System.Collections.ObjectModel;

namespace Tests
{
    public static class TestHelper
    {
        static public string StaticTestBidPath = Environment.CurrentDirectory + @"\Test Files\UnitTestBid.bdb";

        public static TECBid CreateTestBid()
        {
            TECBid bid = new TECBid();

            //SubScope
            var subScope1 = new TECSubScope();
            var allSubScope = new ObservableCollection<TECSubScope>();
            allSubScope.Add(subScope1);

            //Equipment
            var equipment1 = new TECEquipment("Equipment 1", "Description 1", 123.4, allSubScope);
            var allEquipment = new ObservableCollection<TECEquipment>();
            allEquipment.Add(equipment1);

            //Systems
            var system1 = new TECSystem("System 1", "Description 1", 234.5, allEquipment);
            system1.Quantity = 1;

            //Bid
            bid.Systems.Add(system1);

            return bid;
        }

        public static TECBid LoadTestBid(string path)
        {
            return EstimatingLibraryDatabase.LoadDBToBid(path, new TECTemplates());
        }

        public static TECSystem LoadTestSystem(string path)
        {
            TECBid bid = LoadTestBid(path);

            if (bid.Systems.Count > 0)
            {
                return bid.Systems[0];
            }
            else
            {
                throw new NullReferenceException();
            }
        }

        public static TECEquipment LoadTestEquipment(string path)
        {
            TECSystem system = LoadTestSystem(path);

            if (system.Equipment.Count > 0)
            {
                return system.Equipment[0];
            }
            else
            {
                throw new NullReferenceException();
            }
        }

        public static TECSubScope LoadTestSubScope(string path)
        {
            TECEquipment equip = LoadTestEquipment(path);

            if (equip.SubScope.Count > 0)
            {
                return equip.SubScope[0];
            }
            else
            {
                throw new NullReferenceException();
            }
        }

        public static TECDevice LoadTestDevice(string path)
        {
            TECSubScope ss = LoadTestSubScope(path);

            if (ss.Devices.Count > 0)
            {
                return ss.Devices[0];
            }
            else
            {
                throw new NullReferenceException();
            }
        }

        public static TECPoint LoadTestPoint(string path)
        {
            TECSubScope ss = LoadTestSubScope(path);

            if (ss.Points.Count > 0)
            {
                return ss.Points[0];
            }
            else
            {
                throw new NullReferenceException();
            }
        }
    }
}
