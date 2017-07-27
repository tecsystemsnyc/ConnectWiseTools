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
            foreach(NetworkController netController in netVM.UnitaryControllersVM.NetworkControllers)
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

    }
}
