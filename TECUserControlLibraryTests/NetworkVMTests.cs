using EstimatingLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECUserControlLibrary.Models;
using TECUserControlLibrary.ViewModels;

namespace Tests
{
    [TestClass]
    public class NetworkVMTests
    {
        #region Is Connected Tests
        [TestMethod]
        public void MakeServer_ConnectPanel_ConnectUnitary()
        {
            //Arrange
            TECBid bid = new TECBid();
            bid.Catalogs = TestHelper.CreateTestCatalogs();

            TECControllerType controllerType = new TECControllerType(bid.Catalogs.Manufacturers.RandomObject());
            TECIO io = new TECIO();
            io.Type = IOType.BACnetIP;
            io.Quantity = 100;
            controllerType.IO.Add(io);
            bid.Catalogs.ControllerTypes.Add(controllerType);

            TECController server = new TECController(controllerType);
            bid.Controllers.Add(server);

            TECController panel = new TECController(controllerType);
            bid.Controllers.Add(panel);

            TECController unitary = new TECController(controllerType);
            bid.Controllers.Add(unitary);

            NetworkVM netVM = new NetworkVM(bid);

            NetworkController netServer = null;
            NetworkController netPanel = null;
            NetworkController netUnitary = null;
            foreach (NetworkController netController in netVM.UnitaryControllersVM.NetworkControllers)
            {
                if (netController.Controller == server)
                {
                    netServer = netController;
                }
                else if (netController.Controller == panel)
                {
                    netPanel = netController;
                }
                else if (netController.Controller == unitary)
                {
                    netUnitary = netController;
                }
            }

            //Act
            netServer.IsServer = true;
            netPanel.ParentController = netServer.Controller;
            netUnitary.ParentController = netPanel.Controller;

            //Assert
            Assert.IsTrue(netServer.IsConnected);
            Assert.IsTrue(netPanel.IsConnected);
            Assert.IsTrue(netUnitary.IsConnected);
        }

        [TestMethod]
        public void MakeServer_ConnectUnitary_ConnectPanel()
        {
            //Arrange
            TECBid bid = new TECBid();
            bid.Catalogs = TestHelper.CreateTestCatalogs();

            TECControllerType controllerType = new TECControllerType(bid.Catalogs.Manufacturers.RandomObject());
            TECIO io = new TECIO();
            io.Type = IOType.BACnetIP;
            io.Quantity = 100;
            controllerType.IO.Add(io);
            bid.Catalogs.ControllerTypes.Add(controllerType);

            TECController server = new TECController(controllerType);
            bid.Controllers.Add(server);

            TECController panel = new TECController(controllerType);
            bid.Controllers.Add(panel);

            TECController unitary = new TECController(controllerType);
            bid.Controllers.Add(unitary);

            NetworkVM netVM = new NetworkVM(bid);

            NetworkController netServer = null;
            NetworkController netPanel = null;
            NetworkController netUnitary = null;
            foreach (NetworkController netController in netVM.UnitaryControllersVM.NetworkControllers)
            {
                if (netController.Controller == server)
                {
                    netServer = netController;
                }
                else if (netController.Controller == panel)
                {
                    netPanel = netController;
                }
                else if (netController.Controller == unitary)
                {
                    netUnitary = netController;
                }
            }

            //Act
            netServer.IsServer = true;
            netUnitary.ParentController = netPanel.Controller;
            netPanel.ParentController = netServer.Controller;

            //Assert
            Assert.IsTrue(netServer.IsConnected);
            Assert.IsTrue(netPanel.IsConnected);
            Assert.IsTrue(netUnitary.IsConnected);
        }

        [TestMethod]
        public void ConnectPanel_MakeServer_ConnectUnitary()
        {
            //Arrange
            TECBid bid = new TECBid();
            bid.Catalogs = TestHelper.CreateTestCatalogs();

            TECControllerType controllerType = new TECControllerType(bid.Catalogs.Manufacturers.RandomObject());
            TECIO io = new TECIO();
            io.Type = IOType.BACnetIP;
            io.Quantity = 100;
            controllerType.IO.Add(io);
            bid.Catalogs.ControllerTypes.Add(controllerType);

            TECController server = new TECController(controllerType);
            bid.Controllers.Add(server);

            TECController panel = new TECController(controllerType);
            bid.Controllers.Add(panel);

            TECController unitary = new TECController(controllerType);
            bid.Controllers.Add(unitary);

            NetworkVM netVM = new NetworkVM(bid);

            NetworkController netServer = null;
            NetworkController netPanel = null;
            NetworkController netUnitary = null;
            foreach (NetworkController netController in netVM.UnitaryControllersVM.NetworkControllers)
            {
                if (netController.Controller == server)
                {
                    netServer = netController;
                }
                else if (netController.Controller == panel)
                {
                    netPanel = netController;
                }
                else if (netController.Controller == unitary)
                {
                    netUnitary = netController;
                }
            }

            //Act
            netPanel.ParentController = netServer.Controller;
            netServer.IsServer = true;
            netUnitary.ParentController = netPanel.Controller;

            //Assert
            Assert.IsTrue(netServer.IsConnected);
            Assert.IsTrue(netPanel.IsConnected);
            Assert.IsTrue(netUnitary.IsConnected);
        }

        [TestMethod]
        public void ConnectPanel_ConnectUnitary_MakeServer()
        {
            //Arrange
            TECBid bid = new TECBid();
            bid.Catalogs = TestHelper.CreateTestCatalogs();

            TECControllerType controllerType = new TECControllerType(bid.Catalogs.Manufacturers.RandomObject());
            TECIO io = new TECIO();
            io.Type = IOType.BACnetIP;
            io.Quantity = 100;
            controllerType.IO.Add(io);
            bid.Catalogs.ControllerTypes.Add(controllerType);

            TECController server = new TECController(controllerType);
            bid.Controllers.Add(server);

            TECController panel = new TECController(controllerType);
            bid.Controllers.Add(panel);

            TECController unitary = new TECController(controllerType);
            bid.Controllers.Add(unitary);

            NetworkVM netVM = new NetworkVM(bid);

            NetworkController netServer = null;
            NetworkController netPanel = null;
            NetworkController netUnitary = null;
            foreach (NetworkController netController in netVM.UnitaryControllersVM.NetworkControllers)
            {
                if (netController.Controller == server)
                {
                    netServer = netController;
                }
                else if (netController.Controller == panel)
                {
                    netPanel = netController;
                }
                else if (netController.Controller == unitary)
                {
                    netUnitary = netController;
                }
            }

            //Act
            netPanel.ParentController = netServer.Controller;
            netUnitary.ParentController = netPanel.Controller;
            netServer.IsServer = true;

            //Assert
            Assert.IsTrue(netServer.IsConnected);
            Assert.IsTrue(netPanel.IsConnected);
            Assert.IsTrue(netUnitary.IsConnected);
        }

        [TestMethod]
        public void ConnectUnitary_MakeServer_ConnectPanel()
        {
            //Arrange
            TECBid bid = new TECBid();
            bid.Catalogs = TestHelper.CreateTestCatalogs();

            TECControllerType controllerType = new TECControllerType(bid.Catalogs.Manufacturers.RandomObject());
            TECIO io = new TECIO();
            io.Type = IOType.BACnetIP;
            io.Quantity = 100;
            controllerType.IO.Add(io);
            bid.Catalogs.ControllerTypes.Add(controllerType);

            TECController server = new TECController(controllerType);
            bid.Controllers.Add(server);

            TECController panel = new TECController(controllerType);
            bid.Controllers.Add(panel);

            TECController unitary = new TECController(controllerType);
            bid.Controllers.Add(unitary);

            NetworkVM netVM = new NetworkVM(bid);

            NetworkController netServer = null;
            NetworkController netPanel = null;
            NetworkController netUnitary = null;
            foreach (NetworkController netController in netVM.UnitaryControllersVM.NetworkControllers)
            {
                if (netController.Controller == server)
                {
                    netServer = netController;
                }
                else if (netController.Controller == panel)
                {
                    netPanel = netController;
                }
                else if (netController.Controller == unitary)
                {
                    netUnitary = netController;
                }
            }

            //Act
            netUnitary.ParentController = netPanel.Controller;
            netServer.IsServer = true;
            netPanel.ParentController = netServer.Controller;

            //Assert
            Assert.IsTrue(netServer.IsConnected);
            Assert.IsTrue(netPanel.IsConnected);
            Assert.IsTrue(netUnitary.IsConnected);
        }

        [TestMethod]
        public void ConnectUnitary_ConnectPanel_MakeServer()
        {
            //Arrange
            TECBid bid = new TECBid();
            bid.Catalogs = TestHelper.CreateTestCatalogs();

            TECControllerType controllerType = new TECControllerType(bid.Catalogs.Manufacturers.RandomObject());
            TECIO io = new TECIO();
            io.Type = IOType.BACnetIP;
            io.Quantity = 100;
            controllerType.IO.Add(io);
            bid.Catalogs.ControllerTypes.Add(controllerType);

            TECController server = new TECController(controllerType);
            bid.Controllers.Add(server);

            TECController panel = new TECController(controllerType);
            bid.Controllers.Add(panel);

            TECController unitary = new TECController(controllerType);
            bid.Controllers.Add(unitary);

            NetworkVM netVM = new NetworkVM(bid);

            NetworkController netServer = null;
            NetworkController netPanel = null;
            NetworkController netUnitary = null;
            foreach (NetworkController netController in netVM.UnitaryControllersVM.NetworkControllers)
            {
                if (netController.Controller == server)
                {
                    netServer = netController;
                }
                else if (netController.Controller == panel)
                {
                    netPanel = netController;
                }
                else if (netController.Controller == unitary)
                {
                    netUnitary = netController;
                }
            }

            //Act
            netUnitary.ParentController = netPanel.Controller;
            netPanel.ParentController = netServer.Controller;
            netServer.IsServer = true;

            //Assert
            Assert.IsTrue(netServer.IsConnected);
            Assert.IsTrue(netPanel.IsConnected);
            Assert.IsTrue(netUnitary.IsConnected);
        }
        #endregion

        #region Possible Parents Tests
        [TestMethod]
        public void NetworkType()
        {
            //Arrange
            TECBid bid = new TECBid();
            bid.Catalogs = TestHelper.CreateTestCatalogs();

            TECControllerType controllerType = new TECControllerType(bid.Catalogs.Manufacturers.RandomObject());
            TECIO io = new TECIO();
            io.Type = IOType.BACnetIP;
            io.Quantity = 100;
            controllerType.IO.Add(io);
            bid.Catalogs.ControllerTypes.Add(controllerType);

            TECController server = new TECController(controllerType);
            server.NetworkType = EstimatingLibrary.NetworkType.Server;
            bid.Controllers.Add(server);

            TECController ddc = new TECController(controllerType);
            ddc.NetworkType = EstimatingLibrary.NetworkType.DDC;
            bid.Controllers.Add(ddc);

            TECController unitary = new TECController(controllerType);
            unitary.NetworkType = EstimatingLibrary.NetworkType.Unitary;
            bid.Controllers.Add(unitary);

            TECController child = new TECController(controllerType);
            child.NetworkType = EstimatingLibrary.NetworkType.Unitary;
            bid.Controllers.Add(child);

            NetworkVM netVM = new NetworkVM(bid);

            NetworkController netChild = null;
            foreach (NetworkController netController in netVM.UnitaryControllersVM.NetworkControllers)
            {
                if (netController.Controller == child)
                {
                    netChild = netController;
                }
            }

            //Assert
            Assert.IsTrue(netChild.PossibleParents.Contains(server));
            Assert.IsTrue(netChild.PossibleParents.Contains(ddc));
            Assert.IsFalse(netChild.PossibleParents.Contains(unitary));
        }

        [TestMethod]
        public void IOMatches()
        {
            //Arrange
            TECBid bid = new TECBid();
            bid.Catalogs = TestHelper.CreateTestCatalogs();

            TECControllerType bacnetType = new TECControllerType(bid.Catalogs.Manufacturers.RandomObject());
            TECIO bacnetIP = new TECIO();
            bacnetIP.Type = IOType.BACnetIP;
            bacnetIP.Quantity = 100;
            bacnetType.IO.Add(bacnetIP);
            bid.Catalogs.ControllerTypes.Add(bacnetType);

            TECControllerType lonType = new TECControllerType(bid.Catalogs.Manufacturers.RandomObject());
            TECIO lon = new TECIO();
            lon.Type = IOType.LonWorks;
            lon.Quantity = 100;
            lonType.IO.Add(lon);
            bid.Catalogs.ControllerTypes.Add(lonType);

            TECController bacnetController = new TECController(bacnetType);
            bacnetController.NetworkType = EstimatingLibrary.NetworkType.Server;
            bid.Controllers.Add(bacnetController);

            TECController lonController = new TECController(lonType);
            lonController.NetworkType = EstimatingLibrary.NetworkType.Server;
            bid.Controllers.Add(lonController);

            TECController childController = new TECController(bacnetType);
            bid.Controllers.Add(childController);

            NetworkVM netVM = new NetworkVM(bid);

            NetworkController netChild = null;
            foreach (NetworkController netController in netVM.UnitaryControllersVM.NetworkControllers)
            {
                if (netController.Controller == childController)
                {
                    netChild = netController;
                }
            }

            //Assert
            Assert.IsTrue(netChild.PossibleParents.Contains(bacnetController));
            Assert.IsFalse(netChild.PossibleParents.Contains(lonController));
        }

        [TestMethod]
        public void IOUnavailable()
        {
            //Arrange
            TECBid bid = new TECBid();
            bid.Catalogs = TestHelper.CreateTestCatalogs();

            TECControllerType controllerType = new TECControllerType(bid.Catalogs.Manufacturers.RandomObject());
            TECIO io = new TECIO();
            io.Type = IOType.BACnetIP;
            io.Quantity = 1;
            controllerType.IO.Add(io);
            bid.Catalogs.ControllerTypes.Add(controllerType);

            TECController parentController = new TECController(controllerType);
            parentController.NetworkType = EstimatingLibrary.NetworkType.Server;
            bid.Controllers.Add(parentController);

            TECController adoptedChildController = new TECController(controllerType);
            bid.Controllers.Add(adoptedChildController);

            TECController orphanChildController = new TECController(controllerType);
            bid.Controllers.Add(orphanChildController);

            NetworkVM netVM = new NetworkVM(bid);

            NetworkController netAdopted = null;
            NetworkController netOrphan = null;
            foreach (NetworkController netController in netVM.UnitaryControllersVM.NetworkControllers)
            {
                if (netController.Controller == adoptedChildController)
                {
                    netAdopted = netController;
                }
                else if (netController.Controller == orphanChildController)
                {
                    netOrphan = netController;
                }
            }

            //Act
            Assert.IsTrue(netOrphan.PossibleParents.Contains(parentController));

            netOrphan.ParentController = parentController;

            //Assert
            Assert.IsFalse(netOrphan.PossibleParents.Contains(parentController));
        }

        [TestMethod]
        public void Descendant()
        {
            //Arrange
            TECBid bid = new TECBid();
            bid.Catalogs = TestHelper.CreateTestCatalogs();

            TECControllerType controllerType = new TECControllerType(bid.Catalogs.Manufacturers.RandomObject());
            TECIO io = new TECIO();
            io.Type = IOType.BACnetIP;
            io.Quantity = 100;
            controllerType.IO.Add(io);
            bid.Catalogs.ControllerTypes.Add(controllerType);

            TECController grandparent = new TECController(controllerType);
            grandparent.NetworkType = EstimatingLibrary.NetworkType.DDC;
            bid.Controllers.Add(grandparent);

            TECController parent = new TECController(controllerType);
            parent.NetworkType = EstimatingLibrary.NetworkType.DDC;
            bid.Controllers.Add(parent);

            TECController child = new TECController(controllerType);
            child.NetworkType = EstimatingLibrary.NetworkType.DDC;
            bid.Controllers.Add(child);

            NetworkVM netVM = new NetworkVM(bid);

            NetworkController netGrand = null;
            NetworkController netParent = null;
            NetworkController netChild = null;
            foreach (NetworkController netController in netVM.NetworkControllersVM.NetworkControllers)
            {
                if (netController.Controller == grandparent)
                {
                    netGrand = netController;
                }
                else if (netController.Controller == parent)
                {
                    netParent = netController;
                }
                else if (netController.Controller == child)
                {
                    netChild = netController;
                }
            }

            //Act
            Assert.IsTrue(netGrand.PossibleParents.Contains(child));

            netParent.ParentController = grandparent;

            Assert.IsTrue(netGrand.PossibleParents.Contains(child));

            netChild.ParentController = parent;

            //Assert
            Assert.IsFalse(netGrand.PossibleParents.Contains(child));
        }

        #endregion
    }
}
