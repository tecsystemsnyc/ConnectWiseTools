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

            TECIO io = new TECIO();
            io.Type = IOType.BACnetIP;
            io.Quantity = 100;

            TECController server = new TECController(bid.Catalogs.Manufacturers.RandomObject());
            server.IO.Add(io);
            bid.Controllers.Add(server);

            TECController panel = new TECController(bid.Catalogs.Manufacturers.RandomObject());
            panel.IO.Add(io.Copy() as TECIO);
            bid.Controllers.Add(panel);

            TECController unitary = new TECController(bid.Catalogs.Manufacturers.RandomObject());
            unitary.IO.Add(io.Copy() as TECIO);
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

            TECIO io = new TECIO();
            io.Type = IOType.BACnetIP;
            io.Quantity = 100;

            TECController server = new TECController(bid.Catalogs.Manufacturers.RandomObject());
            server.IO.Add(io);
            bid.Controllers.Add(server);

            TECController panel = new TECController(bid.Catalogs.Manufacturers.RandomObject());
            panel.IO.Add(io.Copy() as TECIO);
            bid.Controllers.Add(panel);

            TECController unitary = new TECController(bid.Catalogs.Manufacturers.RandomObject());
            unitary.IO.Add(io.Copy() as TECIO);
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

            TECIO io = new TECIO();
            io.Type = IOType.BACnetIP;
            io.Quantity = 100;

            TECController server = new TECController(bid.Catalogs.Manufacturers.RandomObject());
            server.IO.Add(io);
            bid.Controllers.Add(server);

            TECController panel = new TECController(bid.Catalogs.Manufacturers.RandomObject());
            panel.IO.Add(io.Copy() as TECIO);
            bid.Controllers.Add(panel);

            TECController unitary = new TECController(bid.Catalogs.Manufacturers.RandomObject());
            unitary.IO.Add(io.Copy() as TECIO);
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

            TECIO io = new TECIO();
            io.Type = IOType.BACnetIP;
            io.Quantity = 100;

            TECController server = new TECController(bid.Catalogs.Manufacturers.RandomObject());
            server.IO.Add(io);
            bid.Controllers.Add(server);

            TECController panel = new TECController(bid.Catalogs.Manufacturers.RandomObject());
            panel.IO.Add(io.Copy() as TECIO);
            bid.Controllers.Add(panel);

            TECController unitary = new TECController(bid.Catalogs.Manufacturers.RandomObject());
            unitary.IO.Add(io.Copy() as TECIO);
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

            TECIO io = new TECIO();
            io.Type = IOType.BACnetIP;
            io.Quantity = 100;

            TECController server = new TECController(bid.Catalogs.Manufacturers.RandomObject());
            server.IO.Add(io);
            bid.Controllers.Add(server);

            TECController panel = new TECController(bid.Catalogs.Manufacturers.RandomObject());
            panel.IO.Add(io.Copy() as TECIO);
            bid.Controllers.Add(panel);

            TECController unitary = new TECController(bid.Catalogs.Manufacturers.RandomObject());
            unitary.IO.Add(io.Copy() as TECIO);
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

            TECIO io = new TECIO();
            io.Type = IOType.BACnetIP;
            io.Quantity = 100;

            TECController server = new TECController(bid.Catalogs.Manufacturers.RandomObject());
            server.IO.Add(io);
            bid.Controllers.Add(server);

            TECController panel = new TECController(bid.Catalogs.Manufacturers.RandomObject());
            panel.IO.Add(io.Copy() as TECIO);
            bid.Controllers.Add(panel);

            TECController unitary = new TECController(bid.Catalogs.Manufacturers.RandomObject());
            unitary.IO.Add(io.Copy() as TECIO);
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

            TECIO io = new TECIO();
            io.Type = IOType.BACnetIP;
            io.Quantity = 100;

            TECController server = new TECController(bid.Catalogs.Manufacturers.RandomObject());
            server.IO.Add(io.Copy() as TECIO);
            server.NetworkType = EstimatingLibrary.NetworkType.Server;
            bid.Controllers.Add(server);

            TECController ddc = new TECController(bid.Catalogs.Manufacturers.RandomObject());
            ddc.IO.Add(io.Copy() as TECIO);
            ddc.NetworkType = EstimatingLibrary.NetworkType.DDC;
            bid.Controllers.Add(ddc);

            TECController unitary = new TECController(bid.Catalogs.Manufacturers.RandomObject());
            unitary.IO.Add(io.Copy() as TECIO);
            unitary.NetworkType = EstimatingLibrary.NetworkType.Unitary;
            bid.Controllers.Add(unitary);

            TECController child = new TECController(bid.Catalogs.Manufacturers.RandomObject());
            child.IO.Add(io.Copy() as TECIO);
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

            TECIO bacnetIP = new TECIO();
            bacnetIP.Type = IOType.BACnetIP;
            bacnetIP.Quantity = 100;

            TECIO lon = new TECIO();
            lon.Type = IOType.LonWorks;
            lon.Quantity = 100;

            TECController bacnetController = new TECController(bid.Catalogs.Manufacturers.RandomObject());
            bacnetController.IO.Add(bacnetIP.Copy() as TECIO);
            bacnetController.NetworkType = EstimatingLibrary.NetworkType.Server;
            bid.Controllers.Add(bacnetController);

            TECController lonController = new TECController(bid.Catalogs.Manufacturers.RandomObject());
            lonController.IO.Add(lon.Copy() as TECIO);
            lonController.NetworkType = EstimatingLibrary.NetworkType.Server;
            bid.Controllers.Add(lonController);

            TECController childController = new TECController(bid.Catalogs.Manufacturers.RandomObject());
            childController.IO.Add(bacnetIP.Copy() as TECIO);
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
        public void IOAddedToParent()
        {
            //Arrange
            TECBid bid = new TECBid();
            bid.Catalogs = TestHelper.CreateTestCatalogs();

            TECIO bacnetIP = new TECIO();
            bacnetIP.Type = IOType.BACnetIP;
            bacnetIP.Quantity = 100;

            TECController parentController = new TECController(bid.Catalogs.Manufacturers.RandomObject());
            parentController.NetworkType = EstimatingLibrary.NetworkType.Server;
            bid.Controllers.Add(parentController);

            TECController childController = new TECController(bid.Catalogs.Manufacturers.RandomObject());
            childController.IO.Add(bacnetIP.Copy() as TECIO);
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

            //Act
            Assert.IsFalse(netChild.PossibleParents.Contains(parentController));

            parentController.IO.Add(bacnetIP.Copy() as TECIO);

            //Assert
            Assert.IsTrue(netChild.PossibleParents.Contains(parentController));
        }

        [TestMethod]
        public void IOAddedToChild()
        {
            //Arrange
            TECBid bid = new TECBid();
            bid.Catalogs = TestHelper.CreateTestCatalogs();

            TECIO bacnetIP = new TECIO();
            bacnetIP.Type = IOType.BACnetIP;
            bacnetIP.Quantity = 100;

            TECController parentController = new TECController(bid.Catalogs.Manufacturers.RandomObject());
            parentController.IO.Add(bacnetIP.Copy() as TECIO);
            parentController.NetworkType = EstimatingLibrary.NetworkType.Server;
            bid.Controllers.Add(parentController);

            TECController childController = new TECController(bid.Catalogs.Manufacturers.RandomObject());
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

            //Act
            Assert.IsFalse(netChild.PossibleParents.Contains(parentController));

            childController.IO.Add(bacnetIP.Copy() as TECIO);

            //Assert
            Assert.IsTrue(netChild.PossibleParents.Contains(parentController));
        }

        [TestMethod]
        public void IORemovedFromParent()
        {
            //Arrange
            TECBid bid = new TECBid();
            bid.Catalogs = TestHelper.CreateTestCatalogs();

            TECIO io = new TECIO();
            io.Type = IOType.BACnetIP;
            io.Quantity = 100;

            TECController parentController = new TECController(bid.Catalogs.Manufacturers.RandomObject());
            TECIO parentIO = io.Copy() as TECIO;
            parentController.IO.Add(parentIO);
            parentController.NetworkType = EstimatingLibrary.NetworkType.Server;
            bid.Controllers.Add(parentController);

            TECController childController = new TECController(bid.Catalogs.Manufacturers.RandomObject());
            childController.IO.Add(io.Copy() as TECIO);
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

            //Act
            Assert.IsTrue(netChild.PossibleParents.Contains(parentController));

            parentController.IO.Remove(parentIO);

            //Assert
            Assert.IsFalse(netChild.PossibleParents.Contains(parentController));
        }

        [TestMethod]
        public void IORemovedFromChild()
        {
            //Arrange
            TECBid bid = new TECBid();
            bid.Catalogs = TestHelper.CreateTestCatalogs();

            TECIO io = new TECIO();
            io.Type = IOType.BACnetIP;
            io.Quantity = 100;

            TECController parentController = new TECController(bid.Catalogs.Manufacturers.RandomObject());
            parentController.IO.Add(io.Copy() as TECIO);
            parentController.NetworkType = EstimatingLibrary.NetworkType.Server;
            bid.Controllers.Add(parentController);

            TECController childController = new TECController(bid.Catalogs.Manufacturers.RandomObject());
            TECIO childIO = io.Copy() as TECIO;
            childController.IO.Add(childIO);
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

            //Act
            Assert.IsTrue(netChild.PossibleParents.Contains(parentController));

            childController.IO.Remove(childIO);

            //Assert
            Assert.IsFalse(netChild.PossibleParents.Contains(parentController));
        }

        [TestMethod]
        public void IOUnavailable()
        {
            //Arrange
            TECBid bid = new TECBid();
            bid.Catalogs = TestHelper.CreateTestCatalogs();

            TECIO io = new TECIO();
            io.Type = IOType.BACnetIP;
            io.Quantity = 1;

            TECController parentController = new TECController(bid.Catalogs.Manufacturers.RandomObject());
            parentController.IO.Add(io.Copy() as TECIO);
            parentController.NetworkType = EstimatingLibrary.NetworkType.Server;
            bid.Controllers.Add(parentController);

            TECController adoptedChildController = new TECController(bid.Catalogs.Manufacturers.RandomObject());
            adoptedChildController.IO.Add(io.Copy() as TECIO);
            bid.Controllers.Add(adoptedChildController);

            TECController orphanChildController = new TECController(bid.Catalogs.Manufacturers.RandomObject());
            orphanChildController.IO.Add(io.Copy() as TECIO);
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

            TECIO io = new TECIO();
            io.Type = IOType.BACnetIP;
            io.Quantity = 100;

            TECController grandparent = new TECController(bid.Catalogs.Manufacturers.RandomObject());
            grandparent.IO.Add(io.Copy() as TECIO);
            grandparent.NetworkType = EstimatingLibrary.NetworkType.DDC;
            bid.Controllers.Add(grandparent);

            TECController parent = new TECController(bid.Catalogs.Manufacturers.RandomObject());
            parent.IO.Add(io.Copy() as TECIO);
            parent.NetworkType = EstimatingLibrary.NetworkType.DDC;
            bid.Controllers.Add(parent);

            TECController child = new TECController(bid.Catalogs.Manufacturers.RandomObject());
            child.IO.Add(io.Copy() as TECIO);
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
