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
        static private string testBidPath = Environment.CurrentDirectory + @"\Test Files\UnitTestBid.bdb";

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

        public static TECBid LoadTestBid()
        {
            return EstimatingLibraryDatabase.LoadDBToBid(testBidPath, new TECTemplates());
        }
    }
}
