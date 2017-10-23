using EstimatingLibrary;
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


            throw new NotImplementedException();
        }

        [TestMethod]
        public void TestDoesntNeedUpdateCanLeave()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void TestDoesntNeedUpdateSelectController()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void TestNeedsUpdateSelectControllerCancel()
        {
            //Arrange
            Mock<IUserConfirmable> confirmable = new Mock<IUserConfirmable>();
            confirmable.Setup(x => x.Show("")).Returns((bool?)null);

            TECBid bid = TestHelper.CreateEmptyCatalogBid();

            TECTypical typical = new TECTypical();
            TECController controller = new TECController(bid.Catalogs.ControllerTypes[0], true);
            typical.AddController(controller);

            SystemConnectionsVM vm = new SystemConnectionsVM(typical, new List<TECElectricalMaterial>());
            vm.ConfirmationObject = confirmable.Object;

            Mock<SubScopeConnectionItem> ssItem1 = new Mock<SubScopeConnectionItem>();
            ssItem1.SetupGet(x => x.NeedsUpdate).Returns(false);

            Mock<SubScopeConnectionItem> ssItem2 = new Mock<SubScopeConnectionItem>();
            ssItem2.SetupGet(x => x.NeedsUpdate).Returns(true);

            vm.SubScope.Add(ssItem1.Object);
            vm.SubScope.Add(ssItem2.Object);

            //Act
            vm.SelectedController = controller;

            //Assert
            Assert.AreEqual(null, vm.SelectedController, "Selected controller should be null.");
        }

        [TestMethod]
        public void TestNeedsUpdateSelectControllerYes()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void TestNeedsUpdateSelectControllerNo()
        {
            throw new NotImplementedException();
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
