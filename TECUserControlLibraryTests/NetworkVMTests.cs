using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECUserControlLibrary.Models;
using TECUserControlLibrary.ViewModels;
using static TECUserControlLibrary.ViewModels.NetworkVM;

namespace Tests
{
    [TestClass]
    public class NetworkVMTests
    {
        #region Is Connected Tests
        #region Make Connection Tests
        [TestMethod]
        public void ConnectControllerToServer()
        {
            //Arrange
            TECBid bid = TestHelper.CreateEmptyCatalogBid();

            ChangeWatcher cw = new ChangeWatcher(bid);
            NetworkVM netVM = new NetworkVM(bid, cw);

            TECController server = new TECController(bid.Catalogs.ControllerTypes[0], false);
            server.IsServer = true;
            TECController controller = new TECController(bid.Catalogs.ControllerTypes[0], false);
            bid.Controllers.Add(server);
            bid.Controllers.Add(controller);
            TECNetworkConnection netConnect = server.AddNetworkConnection(false,
                new List<TECElectricalMaterial>() { bid.Catalogs.ConnectionTypes[0] }, IOType.BACnetIP);
            
            ConnectableItem serverItem = null;
            ConnectableItem controllerItem = null;

            foreach(ConnectableItem item in netVM.Parentables)
            {
                if (item.Item == server)
                {
                    serverItem = item;
                }
                else if (item.Item == controller)
                {
                    controllerItem = item;
                }
            }

            //Act
            netConnect.AddINetworkConnectable(controller);

            //Assert
            checkIsConnected(serverItem, true);
            checkIsConnected(controllerItem, true);

            netVM.Refresh(bid, cw);

            checkIsConnected(serverItem, true);
            checkIsConnected(controllerItem, true);
        }

        [TestMethod]
        public void ConnectSubScopeToServer()
        {
            //Arrange
            TECBid bid = TestHelper.CreateEmptyCatalogBid();

            ChangeWatcher cw = new ChangeWatcher(bid);
            NetworkVM netVM = new NetworkVM(bid, cw);

            List<TECElectricalMaterial> connectionTypes = new List<TECElectricalMaterial>() { bid.Catalogs.ConnectionTypes[0] };
            TECDevice device = new TECDevice(connectionTypes, bid.Catalogs.Manufacturers[0]);
            bid.Catalogs.Devices.Add(device);

            TECController server = new TECController(bid.Catalogs.ControllerTypes[0], false);
            server.IsServer = true;
            bid.Controllers.Add(server);
            TECNetworkConnection netConnect = server.AddNetworkConnection(false, connectionTypes, IOType.BACnetIP);

            TECTypical typical = new TECTypical();
            TECEquipment typEquip = new TECEquipment(true);
            TECSubScope typSS = new TECSubScope(true);
            TECPoint typPoint = new TECPoint(true);
            typPoint.Type = IOType.BACnetIP;
            typPoint.Quantity = 5;
            typSS.AddPoint(typPoint);
            typSS.Devices.Add(device);
            typEquip.SubScope.Add(typSS);
            typical.Equipment.Add(typEquip);
            bid.Systems.Add(typical);

            TECSystem instance = typical.AddInstance(bid);
            TECSubScope ss = instance.Equipment[0].SubScope[0];

            ConnectableItem serverItem = null;
            ConnectableItem ssItem = null;

            foreach (ConnectableItem item in netVM.Parentables)
            {
                if (item.Item == server)
                {
                    serverItem = item;
                }
            }
            foreach (ConnectableItem item in netVM.NonParentables)
            {
                if (item.Item == ss)
                {
                    ssItem = item;
                }
            }

            //Act
            netConnect.AddINetworkConnectable(ss);

            //Assert
            checkIsConnected(serverItem, true);
            checkIsConnected(ssItem, true);

            netVM.Refresh(bid, cw);

            checkIsConnected(serverItem, true);
            checkIsConnected(ssItem, true);
        }

        [TestMethod]
        public void ConnectServerToServer()
        {
            //Arrange
            TECBid bid = TestHelper.CreateEmptyCatalogBid();

            ChangeWatcher cw = new ChangeWatcher(bid);
            NetworkVM netVM = new NetworkVM(bid, cw);

            TECController server1 = new TECController(bid.Catalogs.ControllerTypes[0], false);
            server1.IsServer = true;
            TECController server2 = new TECController(bid.Catalogs.ControllerTypes[0], false);
            server2.IsServer = true;
            bid.Controllers.Add(server1);
            bid.Controllers.Add(server2);

            List<TECElectricalMaterial> connectionTypes = new List<TECElectricalMaterial>() { bid.Catalogs.ConnectionTypes[0] };

            TECNetworkConnection netConnect = server1.AddNetworkConnection(false, connectionTypes, IOType.BACnetIP);

            ConnectableItem serverItem1 = null;
            ConnectableItem serverItem2 = null;

            foreach(ConnectableItem item in netVM.Parentables)
            {
                if (item.Item == server1)
                {
                    serverItem1 = item;
                }
                else if (item.Item == server2)
                {
                    serverItem2 = item;
                }
            }

            //Act
            netConnect.AddINetworkConnectable(server2);

            //Assert
            checkIsConnected(serverItem1, true);
            checkIsConnected(serverItem2, true);

            netVM.Refresh(bid, cw);

            checkIsConnected(serverItem1, true);
            checkIsConnected(serverItem2, true);
        }

        [TestMethod]
        public void ConnectControllerToConnectedController()
        {
            //Arrange
            TECBid bid = TestHelper.CreateEmptyCatalogBid();

            ChangeWatcher cw = new ChangeWatcher(bid);
            NetworkVM netVM = new NetworkVM(bid, cw);

            TECController server = new TECController(bid.Catalogs.ControllerTypes[0], false);
            server.IsServer = true;
            TECController parentController = new TECController(bid.Catalogs.ControllerTypes[0], false);
            TECController childController = new TECController(bid.Catalogs.ControllerTypes[0], false);
            bid.Controllers.Add(server);
            bid.Controllers.Add(parentController);
            bid.Controllers.Add(childController);

            List<TECElectricalMaterial> connectionTypes = new List<TECElectricalMaterial>() { bid.Catalogs.ConnectionTypes[0] };

            TECNetworkConnection serverConnect = server.AddNetworkConnection(false, connectionTypes, IOType.BACnetIP);
            serverConnect.AddINetworkConnectable(parentController);
            TECNetworkConnection netConnect = parentController.AddNetworkConnection(false, connectionTypes, IOType.BACnetIP);

            ConnectableItem serverItem = null;
            ConnectableItem parentItem = null;
            ConnectableItem childItem = null;

            foreach(ConnectableItem item in netVM.Parentables)
            {
                if (item.Item == server)
                {
                    serverItem = item;
                }
                else if (item.Item == parentController)
                {
                    parentItem = item;
                }
                else if (item.Item == childController)
                {
                    childItem = item;
                }
            }

            //Act
            netConnect.AddINetworkConnectable(childController);

            //Assert
            checkIsConnected(serverItem, true);
            checkIsConnected(parentItem, true);
            checkIsConnected(childItem, true);

            netVM.Refresh(bid, cw);

            checkIsConnected(serverItem, true);
            checkIsConnected(parentItem, true);
            checkIsConnected(childItem, true);
        }

        [TestMethod]
        public void ConnectSubScopeToConnectedController()
        {
            //Arrange
            TECBid bid = TestHelper.CreateEmptyCatalogBid();

            ChangeWatcher cw = new ChangeWatcher(bid);
            NetworkVM netVM = new NetworkVM(bid, cw);

            TECController server = new TECController(bid.Catalogs.ControllerTypes[0], false);
            server.IsServer = true;
            TECController parentController = new TECController(bid.Catalogs.ControllerTypes[0], false);
            bid.Controllers.Add(server);
            bid.Controllers.Add(parentController);

            List<TECElectricalMaterial> connectionTypes = new List<TECElectricalMaterial>() { bid.Catalogs.ConnectionTypes[0] };
            TECDevice device = new TECDevice(connectionTypes, bid.Catalogs.Manufacturers[0]);
            bid.Catalogs.Devices.Add(device);

            TECNetworkConnection serverConnect = server.AddNetworkConnection(false, connectionTypes, IOType.BACnetIP);
            serverConnect.AddINetworkConnectable(parentController);
            TECNetworkConnection netConnect = parentController.AddNetworkConnection(false, connectionTypes, IOType.BACnetIP);

            TECTypical typical = new TECTypical();
            TECEquipment typEquip = new TECEquipment(true);
            TECSubScope typSS = new TECSubScope(true);
            TECPoint typPoint = new TECPoint(true);
            typPoint.Type = IOType.BACnetIP;
            typPoint.Quantity = 5;
            typSS.AddPoint(typPoint);
            typSS.Devices.Add(device);
            typEquip.SubScope.Add(typSS);
            typical.Equipment.Add(typEquip);
            bid.Systems.Add(typical);

            TECSystem instance = typical.AddInstance(bid);
            TECSubScope ss = instance.Equipment[0].SubScope[0];

            ConnectableItem serverItem = null;
            ConnectableItem controllerItem = null;
            ConnectableItem ssItem = null;

            foreach (ConnectableItem item in netVM.Parentables)
            {
                if (item.Item == server)
                {
                    serverItem = item;
                }
                else if (item.Item == parentController)
                {
                    controllerItem = item;
                }
            }
            foreach (ConnectableItem item in netVM.NonParentables)
            {
                if (item.Item == ss)
                {
                    ssItem = item;
                }
            }

            //Act
            netConnect.AddINetworkConnectable(ss);

            //Assert
            checkIsConnected(serverItem, true);
            checkIsConnected(controllerItem, true);
            checkIsConnected(ssItem, true);

            netVM.Refresh(bid, cw);

            checkIsConnected(serverItem, true);
            checkIsConnected(controllerItem, true);
            checkIsConnected(ssItem, true);
        }

        [TestMethod]
        public void ConnectParentControllerToServer()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void DisconnectControllerFromServer()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void DisconnectSubScopeFromServer()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void DisconnectServerFromServer()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void DisconnectParentControllerFromServer()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Make Server Tests
        [TestMethod]
        public void MakeServer()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void MakeNotServer()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void MakeServerWithChildServer()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void MakeNotServerWithChildServer()
        {
            throw new NotImplementedException();
        }
        #endregion
        #endregion

        #region Possible Parents Tests
        [TestMethod]
        public void NetworkType()
        {
            ////Arrange
            //TECBid bid = new TECBid();
            //bid.Catalogs = TestHelper.CreateTestCatalogs();

            //TECControllerType controllerType = new TECControllerType(bid.Catalogs.Manufacturers[0]);
            //TECIO io = new TECIO();
            //io.Type = IOType.BACnetIP;
            //io.Quantity = 100;
            //controllerType.IO.Add(io);
            //bid.Catalogs.ControllerTypes.Add(controllerType);

            //TECController server = new TECController(controllerType, false);
            //server.NetworkType = EstimatingLibrary.NetworkType.Server;
            //bid.Controllers.Add(server);

            //TECController ddc = new TECController(controllerType, false);
            //ddc.NetworkType = EstimatingLibrary.NetworkType.DDC;
            //bid.Controllers.Add(ddc);

            //TECController unitary = new TECController(controllerType, false);
            //unitary.NetworkType = EstimatingLibrary.NetworkType.Unitary;
            //bid.Controllers.Add(unitary);

            //TECController child = new TECController(controllerType, false);
            //child.NetworkType = EstimatingLibrary.NetworkType.Unitary;
            //bid.Controllers.Add(child);

            //NetworkVM netVM = new NetworkVM(bid);

            //NetworkController netChild = null;
            //foreach (NetworkController netController in netVM.UnitaryControllersVM.NetworkControllers)
            //{
            //    if (netController.Controller == child)
            //    {
            //        netChild = netController;
            //    }
            //}

            ////Assert
            //Assert.IsTrue(netChild.PossibleParents.Contains(server));
            //Assert.IsTrue(netChild.PossibleParents.Contains(ddc));
            //Assert.IsFalse(netChild.PossibleParents.Contains(unitary));
            throw new NotImplementedException();
        }

        [TestMethod]
        public void IOMatches()
        {
            ////Arrange
            //TECBid bid = new TECBid();
            //bid.Catalogs = TestHelper.CreateTestCatalogs();

            //TECControllerType bacnetType = new TECControllerType(bid.Catalogs.Manufacturers[0]);
            //TECIO bacnetIP = new TECIO();
            //bacnetIP.Type = IOType.BACnetIP;
            //bacnetIP.Quantity = 100;
            //bacnetType.IO.Add(bacnetIP);
            //bid.Catalogs.ControllerTypes.Add(bacnetType);

            //TECControllerType lonType = new TECControllerType(bid.Catalogs.Manufacturers[0]);
            //TECIO lon = new TECIO();
            //lon.Type = IOType.LonWorks;
            //lon.Quantity = 100;
            //lonType.IO.Add(lon);
            //bid.Catalogs.ControllerTypes.Add(lonType);

            //TECController bacnetController = new TECController(bacnetType, false);
            //bacnetController.NetworkType = EstimatingLibrary.NetworkType.Server;
            //bid.Controllers.Add(bacnetController);

            //TECController lonController = new TECController(lonType, false);
            //lonController.NetworkType = EstimatingLibrary.NetworkType.Server;
            //bid.Controllers.Add(lonController);

            //TECController childController = new TECController(bacnetType, false);
            //bid.Controllers.Add(childController);

            //NetworkVM netVM = new NetworkVM(bid);

            //NetworkController netChild = null;
            //foreach (NetworkController netController in netVM.UnitaryControllersVM.NetworkControllers)
            //{
            //    if (netController.Controller == childController)
            //    {
            //        netChild = netController;
            //    }
            //}

            ////Assert
            //Assert.IsTrue(netChild.PossibleParents.Contains(bacnetController));
            //Assert.IsFalse(netChild.PossibleParents.Contains(lonController));
            throw new NotImplementedException();
        }

        [TestMethod]
        public void IOUnavailable()
        {
            ////Arrange
            //TECBid bid = new TECBid();
            //bid.Catalogs = TestHelper.CreateTestCatalogs();

            //TECControllerType controllerType = new TECControllerType(bid.Catalogs.Manufacturers[0]);
            //TECIO io = new TECIO();
            //io.Type = IOType.BACnetIP;
            //io.Quantity = 1;
            //controllerType.IO.Add(io);
            //bid.Catalogs.ControllerTypes.Add(controllerType);

            //TECController parentController = new TECController(controllerType, false);
            //parentController.NetworkType = EstimatingLibrary.NetworkType.Server;
            //bid.Controllers.Add(parentController);

            //TECController adoptedChildController = new TECController(controllerType, false);
            //bid.Controllers.Add(adoptedChildController);

            //TECController orphanChildController = new TECController(controllerType, false);
            //bid.Controllers.Add(orphanChildController);

            //NetworkVM netVM = new NetworkVM(bid);

            //NetworkController netAdopted = null;
            //NetworkController netOrphan = null;
            //foreach (NetworkController netController in netVM.UnitaryControllersVM.NetworkControllers)
            //{
            //    if (netController.Controller == adoptedChildController)
            //    {
            //        netAdopted = netController;
            //    }
            //    else if (netController.Controller == orphanChildController)
            //    {
            //        netOrphan = netController;
            //    }
            //}

            ////Act
            //Assert.IsTrue(netOrphan.PossibleParents.Contains(parentController));

            //netOrphan.ParentController = parentController;

            ////Assert
            //Assert.IsFalse(netOrphan.PossibleParents.Contains(parentController));
            throw new NotImplementedException();
        }

        [TestMethod]
        public void Descendant()
        {
            ////Arrange
            //TECBid bid = new TECBid();
            //bid.Catalogs = TestHelper.CreateTestCatalogs();

            //TECControllerType controllerType = new TECControllerType(bid.Catalogs.Manufacturers[0]);
            //TECIO io = new TECIO();
            //io.Type = IOType.BACnetIP;
            //io.Quantity = 100;
            //controllerType.IO.Add(io);
            //bid.Catalogs.ControllerTypes.Add(controllerType);

            //TECController grandparent = new TECController(controllerType, false);
            //grandparent.NetworkType = EstimatingLibrary.NetworkType.DDC;
            //bid.Controllers.Add(grandparent);

            //TECController parent = new TECController(controllerType, false);
            //parent.NetworkType = EstimatingLibrary.NetworkType.DDC;
            //bid.Controllers.Add(parent);

            //TECController child = new TECController(controllerType, false);
            //child.NetworkType = EstimatingLibrary.NetworkType.DDC;
            //bid.Controllers.Add(child);

            //NetworkVM netVM = new NetworkVM(bid);

            //NetworkController netGrand = null;
            //NetworkController netParent = null;
            //NetworkController netChild = null;
            //foreach (NetworkController netController in netVM.NetworkControllersVM.NetworkControllers)
            //{
            //    if (netController.Controller == grandparent)
            //    {
            //        netGrand = netController;
            //    }
            //    else if (netController.Controller == parent)
            //    {
            //        netParent = netController;
            //    }
            //    else if (netController.Controller == child)
            //    {
            //        netChild = netController;
            //    }
            //}

            ////Act
            //Assert.IsTrue(netGrand.PossibleParents.Contains(child));

            //netParent.ParentController = grandparent;

            //Assert.IsTrue(netGrand.PossibleParents.Contains(child));

            //netChild.ParentController = parent;

            ////Assert
            //Assert.IsFalse(netGrand.PossibleParents.Contains(child));
            throw new NotImplementedException();
        }

        #endregion

        private void checkIsConnected(ConnectableItem item, bool isConnected)
        {
            if (isConnected)
            {
                Assert.IsTrue(item.IsConnected, "ConnectableItem isn't connected when it should be.");
            }
            else
            {
                Assert.IsFalse(item.IsConnected, "ConnectableItem is connected when it shouldn't be.");
            }
        }
    }
}
