using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass]
    public class ChangeWatcherTests
    {
        private TECBid bid;
        private ChangeWatcher cw;

        private TECChangedEventArgs changedArgs;
        private TECChangedEventArgs instanceChangedArgs;
        private List<TECCost> costDeltas;
        private int pointDelta;

        private bool changedRaised;
        private bool instanceChangedRaised;
        private bool costChangedRaised;
        private bool pointChangedRaised;

        [TestInitialize]
        public void TestInitialize()
        {
            bid = TestHelper.CreateTestBid();
            cw = new ChangeWatcher(bid);

            changedRaised = false;
            instanceChangedRaised = false;
            costChangedRaised = false;
            pointChangedRaised = false;

            cw.Changed += (args) =>
            {
                changedArgs = args;
            };
            cw.InstanceChanged += (args) =>
            {
                instanceChangedArgs = args;
            };
            cw.CostChanged += (costs) =>
            {
                costDeltas = costs;
            };
            cw.PointChanged += (numPoints) =>
            {
                pointDelta = numPoints;
            };
        }

        #region Change Events Tests
        [TestMethod]
        public void AddSystemToBid()
        {
            //Arrange
            TECSystem system = new TECSystem();

            //Act
            bid.Systems.Add(system);

            //Assert
            //Check Raised
            Assert.IsTrue(changedRaised, "Changed event on the ChangeWatcher wasn't raised.");

            Assert.IsFalse(instanceChangedRaised, "InstanceChanged event on the ChangeWatcher wasn't raised.");
            Assert.IsFalse(costChangedRaised, "CostChanged event on the ChangeWatcher wasn't raised.");
            Assert.IsFalse(pointChangedRaised, "PointChanged event on the ChangeWatcher wasn't raised.");

            //Check Event Args
            checkEventArgs(changedArgs, Change.Add, "System", bid, system);
            
        }

        [TestMethod]
        public void AddControllerToBid()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void AddPanelToBid()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void AddMiscToBid()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void AddScopeBranchToBid()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void AddNoteToBid()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void AddExclusionToBid()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void AddLocationToBid()
        {
            throw new NotImplementedException();
        }
        #endregion

        private void checkEventArgs(TECChangedEventArgs args, Change change, string propertyName, TECObject sender, object value, object oldValue = null)
        {
            Assert.AreEqual(args.Change, change, "Change type is wrong.");
            Assert.AreEqual(args.PropertyName, propertyName, "PropertyName is wrong.");
            Assert.AreEqual(args.Sender, sender, "Sender is wrong.");
            Assert.AreEqual(args.Value, value, "Value is wrong.");

            if (oldValue != null)
            {
                Assert.AreEqual(args.OldValue, oldValue, "OldValue is wrong.");
            }
        }
    }
}
