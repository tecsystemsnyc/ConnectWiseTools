using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Tests.CostTestingUtilities;

namespace Tests
{
    [TestClass]
    public class ChangeWatcherTests
    {
        private TECBid bid;
        private ChangeWatcher cw;

        private TECChangedEventArgs changedArgs;
        private TECChangedEventArgs instanceChangedArgs;
        private CostBatch costDelta;
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
                costDelta = costs;
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
            checkRaised(false, false, false);
            checkEventArgs(changedArgs, Change.Add, "System", bid, system);
        }

        [TestMethod]
        public void AddControllerToBid()
        {
            //Arrange
            TECControllerType controllerType = bid.Catalogs.ControllerTypes.RandomObject();
            TECController controller = new TECController(controllerType);

            //Act
            bid.Controllers.Add(controller);

            Total tecTotal = CalculateTotal(controller, CostType.TEC);
            Total elecTotal = CalculateTotal(controller, CostType.Electrical);

            //Assert
            checkRaised(true, true, true);
            checkEventArgs(changedArgs, Change.Add, "Controller", bid, controller);
            checkEventArgs(instanceChangedArgs, Change.Add, "Controller", bid, controller);

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

        private void checkRaised(bool instanceChanged, bool costChanged, bool pointChanged)
        {
            Assert.IsTrue(changedRaised, "Changed event on the ChangeWatcher wasn't raised.");

            if (instanceChanged)
            {
                Assert.IsTrue(instanceChangedRaised, "InstanceChanged event on the ChangeWatcher wasn't raised when it should have been.");
            }
            else
            {
                Assert.IsFalse(instanceChangedRaised, "InstanceChanged event on the ChangeWatcher was raised when it shouldn't have been.");
            }
            
            if (costChanged)
            {
                Assert.IsTrue(costChangedRaised, "CostChanged event on the ChangeWatcher wasn't raised when it should have been.");
            }
            else
            {
                Assert.IsFalse(costChangedRaised, "CostChanged event on the ChangeWatcher was raised when it shouldn't have been.");
            }
            
            if (pointChanged)
            {
                Assert.IsTrue(pointChangedRaised, "PointChanged event on the ChangeWatcher wasn't raised when it should have been.");
            }
            else
            {
                Assert.IsFalse(pointChangedRaised, "PointChanged event on the ChangeWatcher was raised when it shouldn't have been.");
            }
        }

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

        private void checkCostDeltas(Total tecTotal, Total elecTotal)
        {
            double totalTECCost = 0;
            double totalTECLabor = 0;
            double totalElecCost = 0;
            double totalElecLabor = 0;
            //foreach(TECCost cost in costDelta)
            //{
            //    if (cost.Type == CostType.TEC)
            //    {
            //        totalTECCost += cost.Cost;
            //        totalTECLabor += cost.Labor;
            //    } 
            //    else if (cost.Type == CostType.Electrical)
            //    {
            //        totalElecCost += cost.Cost;
            //        totalElecLabor += cost.Labor;
            //    }
            //}

            throw new NotImplementedException();
            //Assert.AreEqual(tecTotal.Cost, totalTECCost, delta, )
        }
    }
}
