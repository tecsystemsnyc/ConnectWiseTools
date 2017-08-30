using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Tests.CostTestingUtilities;
using static Tests.TestHelper;

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

            resetRaised();

            cw.Changed += (args) =>
            {
                changedArgs = args;
                changedRaised = true;
            };
            cw.InstanceChanged += (args) =>
            {
                instanceChangedArgs = args;
                instanceChangedRaised = true;
            };
            cw.CostChanged += (costs) =>
            {
                costDelta = costs;
                costChangedRaised = true;
            };
            cw.PointChanged += (numPoints) =>
            {
                pointDelta = numPoints;
                pointChangedRaised = true;
            };
        }

        #region Change Events Tests
        #region Add Tests
        [TestMethod]
        public void AddTypicalToBid()
        {
            //Arrange
            TECSystem typical = new TECSystem();
            //Ensure typical has points and cost:
            TECEquipment equip = new TECEquipment();
            TECSubScope ss = new TECSubScope();
            TECPoint point = new TECPoint();
            point.Type = PointTypes.AI;
            point.Quantity = 2;
            ss.Points.Add(point);
            TECDevice dev = bid.Catalogs.Devices.RandomObject();
            ss.Devices.Add(dev);
            equip.SubScope.Add(ss);
            typical.Equipment.Add(equip);

            //Act
            bid.Systems.Add(typical);

            //Assert
            checkRaised(false, false, false);
            checkChangedArgs(Change.Add, "Systems", bid, typical);
        }

        [TestMethod]
        public void AddControllerToBid()
        {
            //Arrange
            TECControllerType controllerType = bid.Catalogs.ControllerTypes.RandomObject();
            TECController controller = new TECController(controllerType);

            //Act
            bid.Controllers.Add(controller);

            //Assert
            checkRaised(true, true, false);
            checkInstanceChangedArgs(Change.Add, "Controllers", bid, controller);
            checkCostDelta(controller.CostBatch);
        }

        [TestMethod]
        public void AddPanelToBid()
        {
            //Arrange
            TECPanelType panelType = bid.Catalogs.PanelTypes.RandomObject();
            TECPanel panel = new TECPanel(panelType);

            //Act
            bid.Panels.Add(panel);

            //Assert
            checkRaised(true, true, false);
            checkInstanceChangedArgs(Change.Add, "Panels", bid, panel);
            checkCostDelta(panel.CostBatch);
        }

        [TestMethod]
        public void AddMiscToBid()
        {
            //Arrange
            TECMisc misc = new TECMisc(CostType.TEC);
            misc.Cost = RandomDouble(1, 100);
            misc.Labor = RandomDouble(1, 100);

            //Act
            bid.MiscCosts.Add(misc);

            //Assert
            checkRaised(true, true, false);
            checkInstanceChangedArgs(Change.Add, "MiscCosts", bid, misc);
            checkCostDelta(misc.CostBatch);
        }

        [TestMethod]
        public void AddScopeBranchToBid()
        {
            //Arrange
            TECScopeBranch sb = new TECScopeBranch();

            //Act
            bid.ScopeTree.Add(sb);

            //Assert
            checkRaised(false, false, false);
            checkChangedArgs(Change.Add, "ScopeTree", bid, sb);
        }

        [TestMethod]
        public void AddNoteToBid()
        {
            //Arrange
            TECLabeled note = new TECLabeled();

            //Act
            bid.Notes.Add(note);

            //Assert
            checkRaised(false, false, false);
            checkChangedArgs(Change.Add, "Notes", bid, note);
        }

        [TestMethod]
        public void AddExclusionToBid()
        {
            //Arrange
            TECLabeled exclusion = new TECLabeled();

            //Act
            bid.Exclusions.Add(exclusion);

            //Assert
            checkRaised(false, false, false);
            checkChangedArgs(Change.Add, "Exclusions", bid, exclusion);
        }

        [TestMethod]
        public void AddLocationToBid()
        {
            //Arrange
            TECLabeled location = new TECLabeled();

            //Act
            bid.Locations.Add(location);

            //Assert
            checkRaised(false, false, false);
            checkChangedArgs(Change.Add, "Locations", bid, location);
        }

        [TestMethod]
        public void AddInstanceToTypical()
        {
            //Arrange
            TECSystem typical = new TECSystem();
            //Ensure typical has points and cost:
            TECEquipment equip = new TECEquipment();
            TECSubScope ss = new TECSubScope();
            TECPoint point = new TECPoint();
            point.Type = PointTypes.AI;
            point.Quantity = 2;
            ss.Points.Add(point);
            TECDevice dev = bid.Catalogs.Devices.RandomObject();
            ss.Devices.Add(dev);
            equip.SubScope.Add(ss);
            typical.Equipment.Add(equip);
            bid.Systems.Add(typical);

            resetRaised();

            //Act
            TECSystem instance = typical.AddInstance(bid);

            //Assert
            checkRaised(true, true, true);
            checkInstanceChangedArgs(Change.Add, "Instances", typical, instance);
            checkCostDelta(instance.CostBatch);
            checkPointDelta(instance.PointNumber);
        }

        [TestMethod]
        public void AddEquipmentToTypical()
        {
            //Arrange
            TECSystem typical = new TECSystem();
            bid.Systems.Add(typical);
            TECEquipment equip = new TECEquipment();
            //Ensure equip has points and cost:
            TECSubScope ss = new TECSubScope();
            TECPoint point = new TECPoint();
            point.Type = PointTypes.AI;
            point.Quantity = 2;
            ss.Points.Add(point);
            TECDevice dev = bid.Catalogs.Devices.RandomObject();
            ss.Devices.Add(dev);
            equip.SubScope.Add(ss);

            resetRaised();

            //Act
            typical.Equipment.Add(equip);

            //Assert
            checkRaised(false, false, false);
            checkChangedArgs(Change.Add, "Equipment", typical, equip);
        }

        [TestMethod]
        public void AddControllerToTypical()
        {
            //Arrange
            TECSystem typical = new TECSystem();
            bid.Systems.Add(typical);
            TECControllerType type = bid.Catalogs.ControllerTypes.RandomObject();
            TECController controller = new TECController(type);

            resetRaised();

            //Act
            typical.Controllers.Add(controller);

            //Assert
            checkRaised(false, false, false);
            checkChangedArgs(Change.Add, "Controllers", typical, controller);
        }

        [TestMethod]
        public void AddPanelToTypical()
        {
            //Arrange
            TECSystem typical = new TECSystem();
            bid.Systems.Add(typical);
            TECPanelType type = bid.Catalogs.PanelTypes.RandomObject();
            TECPanel panel = new TECPanel(type);

            resetRaised();

            //Act
            typical.Panels.Add(panel);

            //Assert
            checkRaised(false, false, false);
            checkChangedArgs(Change.Add, "Panels", typical, panel);
        }

        [TestMethod]
        public void AddMiscCostToTypical()
        {
            //Arrange
            TECSystem typical = new TECSystem();
            bid.Systems.Add(typical);
            TECMisc misc = new TECMisc(CostType.TEC);

            resetRaised();

            //Act
            typical.MiscCosts.Add(misc);

            //Assert
            checkRaised(false, false, false);
            checkChangedArgs(Change.Add, "MiscCosts", typical, misc);
        }

        [TestMethod]
        public void AddScopeBranchToTypical()
        {
            //Arrange
            TECSystem typical = new TECSystem();
            bid.Systems.Add(typical);
            TECScopeBranch sb = new TECScopeBranch();

            resetRaised();

            //Act
            typical.ScopeBranches.Add(sb);

            //Assert
            checkRaised(false, false, false);
            checkChangedArgs(Change.Add, "ScopeBranches", typical, sb);
        }


        #endregion

        #endregion

        private void resetRaised()
        {
            changedRaised = false;
            instanceChangedRaised = false;
            costChangedRaised = false;
            pointChangedRaised = false;
        }

        #region Check Methods
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

        private void checkChangedArgs(Change change, string propertyName, TECObject sender, object value, object oldValue = null)
        {
            Assert.AreEqual(changedArgs.Change, change, "Change type is wrong.");
            Assert.AreEqual(changedArgs.PropertyName, propertyName, "PropertyName is wrong.");
            Assert.AreEqual(changedArgs.Sender, sender, "Sender is wrong.");
            Assert.AreEqual(changedArgs.Value, value, "Value is wrong.");

            if (oldValue != null)
            {
                Assert.AreEqual(changedArgs.OldValue, oldValue, "OldValue is wrong.");
            }
        }
        private void checkInstanceChangedArgs(Change change, string propertyName, TECObject sender, object value, object oldValue = null)
        {
            checkChangedArgs(change, propertyName, sender, value, oldValue);

            Assert.AreEqual(instanceChangedArgs.Change, change, "Change type is wrong.");
            Assert.AreEqual(instanceChangedArgs.PropertyName, propertyName, "PropertyName is wrong.");
            Assert.AreEqual(instanceChangedArgs.Sender, sender, "Sender is wrong.");
            Assert.AreEqual(instanceChangedArgs.Value, value, "Value is wrong.");

            if (oldValue != null)
            {
                Assert.AreEqual(instanceChangedArgs.OldValue, oldValue, "OldValue is wrong.");
            }
        }

        private void checkCostDelta(CostBatch cb)
        {
            Assert.AreEqual(cb.GetCost(CostType.TEC), costDelta.GetCost(CostType.TEC), DELTA, "ChangeWatcher TEC Cost delta is wrong.");
            Assert.AreEqual(cb.GetLabor(CostType.TEC), costDelta.GetLabor(CostType.TEC), DELTA, "ChangeWatcher TEC Labor delta is wrong.");
            Assert.AreEqual(cb.GetCost(CostType.Electrical), costDelta.GetCost(CostType.Electrical), DELTA, "ChangeWatcher Elec Cost delta is wrong.");
            Assert.AreEqual(cb.GetLabor(CostType.Electrical), costDelta.GetLabor(CostType.Electrical), DELTA, "ChangeWatcher Elec Labor delta is wrong.");
        }

        private void checkPointDelta(int points)
        {
            Assert.AreEqual(points, pointDelta, "ChangeWatcher point delta is wrong.");
        }
        #endregion
    }
}
