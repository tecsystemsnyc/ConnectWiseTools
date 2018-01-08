using EstimatingLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.ObjectModel;
using System.Windows;
using TECUserControlLibrary.Interfaces;
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

            SystemConnectionsVM vm = new SystemConnectionsVM(typical, new ObservableCollection<TECElectricalMaterial>());
            
            //Act
            Mock<ISubScopeConnectionItem> ssItem1 = new Mock<ISubScopeConnectionItem>();
            ssItem1.SetupAllProperties();
            ssItem1.Object.NeedsUpdate = false;

            Mock<ISubScopeConnectionItem> ssItem2 = new Mock<ISubScopeConnectionItem>();
            ssItem2.SetupAllProperties();
            ssItem2.Object.NeedsUpdate = true;

            vm.ConnectedSubScope.Add(ssItem1.Object);
            vm.ConnectedSubScope.Add(ssItem2.Object);

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

            SystemConnectionsVM vm = new SystemConnectionsVM(typical, new ObservableCollection<TECElectricalMaterial>());

            //Act
            Mock<ISubScopeConnectionItem> ssItem1 = new Mock<ISubScopeConnectionItem>();
            ssItem1.SetupAllProperties();
            ssItem1.Object.NeedsUpdate = false;

            Mock<ISubScopeConnectionItem> ssItem2 = new Mock<ISubScopeConnectionItem>();
            ssItem2.SetupAllProperties();
            ssItem2.Object.NeedsUpdate = false;

            vm.ConnectedSubScope.Add(ssItem1.Object);
            vm.ConnectedSubScope.Add(ssItem2.Object);

            //Assert
            Assert.IsTrue(vm.CanLeave, "CanLeave should be true.");
        }

        [TestMethod]
        public void TestDoesntNeedUpdateSelectController()
        {
            //Arrange
            Mock<IUserConfirmable> confirmable = new Mock<IUserConfirmable>();
            confirmable
                .Setup(x => x.Show(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<MessageBoxButton>(), It.IsAny<MessageBoxImage>()))
                .Callback(() => Assert.Fail("Confirmable should not have been shown."));

            TECCatalogs catalogs = TestHelper.CreateTestCatalogs();

            TECTypical typical = new TECTypical();
            TECController controller = new TECController(catalogs.ControllerTypes[0], true);
            typical.AddController(controller);

            SystemConnectionsVM vm = new SystemConnectionsVM(typical, new ObservableCollection<TECElectricalMaterial>());
            vm.ConfirmationObject = confirmable.Object;

            Mock<ISubScopeConnectionItem> ssItem1 = new Mock<ISubScopeConnectionItem>();
            ssItem1.SetupAllProperties();
            ssItem1.Object.NeedsUpdate = false;

            Mock<ISubScopeConnectionItem> ssItem2 = new Mock<ISubScopeConnectionItem>();
            ssItem2.SetupAllProperties();
            ssItem2.Object.NeedsUpdate = false;

            vm.ConnectedSubScope.Add(ssItem1.Object);
            vm.ConnectedSubScope.Add(ssItem2.Object);

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
                .Setup(x => x.Show(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<MessageBoxButton>(), It.IsAny<MessageBoxImage>()))
                .Returns(MessageBoxResult.Cancel);

            TECBid bid = TestHelper.CreateEmptyCatalogBid();

            TECTypical typical = new TECTypical();
            TECController controller = new TECController(bid.Catalogs.ControllerTypes[0], true);
            typical.AddController(controller);

            SystemConnectionsVM vm = new SystemConnectionsVM(typical, new ObservableCollection<TECElectricalMaterial>());
            vm.ConfirmationObject = confirmable.Object;

            Mock<ISubScopeConnectionItem> ssItem1 = new Mock<ISubScopeConnectionItem>();
            ssItem1.SetupAllProperties();
            ssItem1.Object.NeedsUpdate = false;

            Mock<ISubScopeConnectionItem> ssItem2 = new Mock<ISubScopeConnectionItem>();
            ssItem2.SetupAllProperties();
            ssItem2.Object.NeedsUpdate = true;

            vm.ConnectedSubScope.Add(ssItem1.Object);
            vm.ConnectedSubScope.Add(ssItem2.Object);

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
                .Setup(x => x.Show(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<MessageBoxButton>(), It.IsAny<MessageBoxImage>()))
                .Returns(MessageBoxResult.Yes);

            TECBid bid = TestHelper.CreateEmptyCatalogBid();

            TECTypical typical = new TECTypical();
            TECController controller = new TECController(bid.Catalogs.ControllerTypes[0], true);
            typical.AddController(controller);

            SystemConnectionsVM vm = new SystemConnectionsVM(typical, new ObservableCollection<TECElectricalMaterial>());
            vm.ConfirmationObject = confirmable.Object;
            
            Mock<ISubScopeConnectionItem> ssItem1 = new Mock<ISubScopeConnectionItem>();
            ssItem1.SetupAllProperties();
            ssItem1.Object.NeedsUpdate = false;

            Mock<ISubScopeConnectionItem> ssItem2 = new Mock<ISubScopeConnectionItem>();
            ssItem2.SetupAllProperties();
            ssItem2.Object.NeedsUpdate = true;

            vm.ConnectedSubScope.Add(ssItem1.Object);
            vm.ConnectedSubScope.Add(ssItem2.Object);

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
                .Setup(x => x.Show(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<MessageBoxButton>(), It.IsAny<MessageBoxImage>()))
                .Returns(MessageBoxResult.No);

            TECBid bid = TestHelper.CreateEmptyCatalogBid();

            TECTypical typical = new TECTypical();
            TECController controller = new TECController(bid.Catalogs.ControllerTypes[0], true);
            typical.AddController(controller);

            SystemConnectionsVM vm = new SystemConnectionsVM(typical, new ObservableCollection<TECElectricalMaterial>());
            vm.ConfirmationObject = confirmable.Object;

            Mock<ISubScopeConnectionItem> ssItem1 = new Mock<ISubScopeConnectionItem>();
            ssItem1.SetupAllProperties();
            ssItem1.Object.NeedsUpdate = false;

            Mock<ISubScopeConnectionItem> ssItem2 = new Mock<ISubScopeConnectionItem>();
            ssItem2.SetupAllProperties();
            ssItem2.Object.NeedsUpdate = true;

            vm.ConnectedSubScope.Add(ssItem1.Object);
            vm.ConnectedSubScope.Add(ssItem2.Object);

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
