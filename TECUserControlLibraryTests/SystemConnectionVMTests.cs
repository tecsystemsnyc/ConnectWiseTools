﻿using EstimatingLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECUserControlLibrary.Interfaces;
using TECUserControlLibrary.Models;
using TECUserControlLibrary.ViewModels;
using Tests;

namespace TECUserControlLibraryTests
{
    [TestClass]
    public class SystemConnectionVMTests
    {
        #region System Connection VM
        [TestMethod]
        public void TestNeedsUpdateCantLeave()
        {
            //Arrange
            TECCatalogs catalogs = TestHelper.CreateTestCatalogs();

            TECTypical typical = new TECTypical();
            TECController controller = new TECController(catalogs.ControllerTypes[0], true);
            typical.AddController(controller);

            SystemConnectionsVM vm = new SystemConnectionsVM(typical, new List<TECElectricalMaterial>());
            
            //Act
            Mock<ISubScopeConnectionItem> ssItem1 = new Mock<ISubScopeConnectionItem>();
            ssItem1.SetupAllProperties();
            ssItem1.Object.NeedsUpdate = false;

            Mock<ISubScopeConnectionItem> ssItem2 = new Mock<ISubScopeConnectionItem>();
            ssItem2.SetupAllProperties();
            ssItem2.Object.NeedsUpdate = true;

            vm.SubScope.Add(ssItem1.Object);
            vm.SubScope.Add(ssItem2.Object);

            //Assert
            Assert.IsFalse(vm.CanLeave, "CanLeave should be false.");
        }

        [TestMethod]
        public void TestDoesntNeedUpdateCanLeave()
        {
            //Arrange
            TECCatalogs catalogs = TestHelper.CreateTestCatalogs();

            TECTypical typical = new TECTypical();
            TECController controller = new TECController(catalogs.ControllerTypes[0], true);
            typical.AddController(controller);

            SystemConnectionsVM vm = new SystemConnectionsVM(typical, new List<TECElectricalMaterial>());

            //Act
            Mock<ISubScopeConnectionItem> ssItem1 = new Mock<ISubScopeConnectionItem>();
            ssItem1.SetupAllProperties();
            ssItem1.Object.NeedsUpdate = false;

            Mock<ISubScopeConnectionItem> ssItem2 = new Mock<ISubScopeConnectionItem>();
            ssItem2.SetupAllProperties();
            ssItem2.Object.NeedsUpdate = false;

            vm.SubScope.Add(ssItem1.Object);
            vm.SubScope.Add(ssItem2.Object);

            //Assert
            Assert.IsTrue(vm.CanLeave, "CanLeave should be true.");
        }

        [TestMethod]
        public void TestDoesntNeedUpdateSelectController()
        {
            //Arrange
            Mock<IUserConfirmable> confirmable = new Mock<IUserConfirmable>();
            confirmable
                .Setup(x => x.Show(It.IsAny<string>()))
                .Callback(() => Assert.Fail("Confirmable should not have been shown."));

            TECCatalogs catalogs = TestHelper.CreateTestCatalogs();

            TECTypical typical = new TECTypical();
            TECController controller = new TECController(catalogs.ControllerTypes[0], true);
            typical.AddController(controller);

            SystemConnectionsVM vm = new SystemConnectionsVM(typical, new List<TECElectricalMaterial>());
            vm.ConfirmationObject = confirmable.Object;

            Mock<ISubScopeConnectionItem> ssItem1 = new Mock<ISubScopeConnectionItem>();
            ssItem1.SetupAllProperties();
            ssItem1.Object.NeedsUpdate = false;

            Mock<ISubScopeConnectionItem> ssItem2 = new Mock<ISubScopeConnectionItem>();
            ssItem2.SetupAllProperties();
            ssItem2.Object.NeedsUpdate = false;

            vm.SubScope.Add(ssItem1.Object);
            vm.SubScope.Add(ssItem2.Object);

            //Act
            vm.SelectedController = controller;

            //Assert
            Assert.AreEqual(controller, vm.SelectedController, "Selected controller isn't set.");
        }

        [TestMethod]
        public void TestNeedsUpdateSelectControllerCancel()
        {
            //Arrange
            Mock<IUserConfirmable> confirmable = new Mock<IUserConfirmable>();
            confirmable
                .Setup(x => x.Show(It.IsAny<string>()))
                .Returns((bool?)null);

            TECBid bid = TestHelper.CreateEmptyCatalogBid();

            TECTypical typical = new TECTypical();
            TECController controller = new TECController(bid.Catalogs.ControllerTypes[0], true);
            typical.AddController(controller);

            SystemConnectionsVM vm = new SystemConnectionsVM(typical, new List<TECElectricalMaterial>());
            vm.ConfirmationObject = confirmable.Object;

            Mock<ISubScopeConnectionItem> ssItem1 = new Mock<ISubScopeConnectionItem>();
            ssItem1.SetupAllProperties();
            ssItem1.Object.NeedsUpdate = false;

            Mock<ISubScopeConnectionItem> ssItem2 = new Mock<ISubScopeConnectionItem>();
            ssItem2.SetupAllProperties();
            ssItem2.Object.NeedsUpdate = true;

            vm.SubScope.Add(ssItem1.Object);
            vm.SubScope.Add(ssItem2.Object);

            //Act
            vm.SelectedController = controller;

            //Assert
            Assert.AreEqual(null, vm.SelectedController, "Selected controller isn't null.");
        }

        [TestMethod]
        public void TestNeedsUpdateSelectControllerYes()
        {
            //Arrange
            Mock<IUserConfirmable> confirmable = new Mock<IUserConfirmable>();
            confirmable
                .Setup(x => x.Show(It.IsAny<string>()))
                .Returns(true);

            TECBid bid = TestHelper.CreateEmptyCatalogBid();

            TECTypical typical = new TECTypical();
            TECController controller = new TECController(bid.Catalogs.ControllerTypes[0], true);
            typical.AddController(controller);

            SystemConnectionsVM vm = new SystemConnectionsVM(typical, new List<TECElectricalMaterial>());
            vm.ConfirmationObject = confirmable.Object;
            
            Mock<ISubScopeConnectionItem> ssItem1 = new Mock<ISubScopeConnectionItem>();
            ssItem1.SetupAllProperties();
            ssItem1.Object.NeedsUpdate = false;

            Mock<ISubScopeConnectionItem> ssItem2 = new Mock<ISubScopeConnectionItem>();
            ssItem2.SetupAllProperties();
            ssItem2.Object.NeedsUpdate = true;

            vm.SubScope.Add(ssItem1.Object);
            vm.SubScope.Add(ssItem2.Object);

            //Act
            vm.SelectedController = controller;

            //Assert
            Assert.IsFalse(ssItem1.Object.NeedsUpdate, "Item NeedsUpdate isn't false.");
            Assert.IsFalse(ssItem2.Object.NeedsUpdate, "Item NeedsUpdate isn't false.");
        }

        [TestMethod]
        public void TestNeedsUpdateSelectControllerNo()
        {
            //Arrange
            Mock<IUserConfirmable> confirmable = new Mock<IUserConfirmable>();
            confirmable
                .Setup(x => x.Show(It.IsAny<string>()))
                .Returns(false);

            TECBid bid = TestHelper.CreateEmptyCatalogBid();

            TECTypical typical = new TECTypical();
            TECController controller = new TECController(bid.Catalogs.ControllerTypes[0], true);
            typical.AddController(controller);

            SystemConnectionsVM vm = new SystemConnectionsVM(typical, new List<TECElectricalMaterial>());
            vm.ConfirmationObject = confirmable.Object;

            Mock<ISubScopeConnectionItem> ssItem1 = new Mock<ISubScopeConnectionItem>();
            ssItem1.SetupAllProperties();
            ssItem1.Object.NeedsUpdate = false;

            Mock<ISubScopeConnectionItem> ssItem2 = new Mock<ISubScopeConnectionItem>();
            ssItem2.SetupAllProperties();
            ssItem2.Object.NeedsUpdate = true;

            vm.SubScope.Add(ssItem1.Object);
            vm.SubScope.Add(ssItem2.Object);

            //Act
            vm.SelectedController = controller;

            //Assert
            Assert.AreEqual(controller, vm.SelectedController, "Controller isn't selected.");
        }
        #endregion

        #region Update Connection VM


        #region SubScopeUpdatedWrapper

        #endregion
        #endregion

        #region SubScopeConnectionItem

        #endregion
    }
}