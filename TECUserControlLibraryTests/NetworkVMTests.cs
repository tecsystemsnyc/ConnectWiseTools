using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using TECUserControlLibrary.ViewModels;

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
            bid.AddController(server);
            bid.AddController(controller);
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
            bid.AddController(server);
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
            bid.AddController(server1);
            bid.AddController(server2);

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
            bid.AddController(server);
            bid.AddController(parentController);
            bid.AddController(childController);

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
            bid.AddController(server);
            bid.AddController(parentController);

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
            //Arrange
            TECBid bid = TestHelper.CreateEmptyCatalogBid();

            ChangeWatcher cw = new ChangeWatcher(bid);
            NetworkVM netVM = new NetworkVM(bid, cw);

            TECController server = new TECController(bid.Catalogs.ControllerTypes[0], false);
            server.IsServer = true;
            TECController parentController = new TECController(bid.Catalogs.ControllerTypes[0], false);
            TECController childController = new TECController(bid.Catalogs.ControllerTypes[0], false);
            bid.AddController(server);
            bid.AddController(parentController);
            bid.AddController(childController);

            List<TECElectricalMaterial> connectionTypes = new List<TECElectricalMaterial>() { bid.Catalogs.ConduitTypes[0] };

            TECNetworkConnection serverConnect = server.AddNetworkConnection(false, connectionTypes, IOType.BACnetIP);
            TECNetworkConnection parentConnect = parentController.AddNetworkConnection(false, connectionTypes, IOType.BACnetIP);
            parentConnect.AddINetworkConnectable(childController);

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
            serverConnect.AddINetworkConnectable(parentController);

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
        public void DisconnectControllerFromServer()
        {
            //Arrange
            TECBid bid = TestHelper.CreateEmptyCatalogBid();

            ChangeWatcher cw = new ChangeWatcher(bid);
            NetworkVM netVM = new NetworkVM(bid, cw);

            TECController server = new TECController(bid.Catalogs.ControllerTypes[0], false);
            server.IsServer = true;
            TECController controller = new TECController(bid.Catalogs.ControllerTypes[0], false);
            bid.AddController(server);
            bid.AddController(controller);

            List<TECElectricalMaterial> connectionTypes = new List<TECElectricalMaterial>() { bid.Catalogs.ConduitTypes[0] };

            TECNetworkConnection serverConnect = server.AddNetworkConnection(false, connectionTypes, IOType.BACnetIP);
            serverConnect.AddINetworkConnectable(controller);

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
            serverConnect.RemoveINetworkConnectable(controller);

            //Assert
            checkIsConnected(serverItem, true);
            checkIsConnected(controllerItem, false);

            netVM.Refresh(bid, cw);

            checkIsConnected(serverItem, true);
            checkIsConnected(controllerItem, false);
        }

        [TestMethod]
        public void DisconnectSubScopeFromServer()
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
            bid.AddController(server);
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

            netConnect.AddINetworkConnectable(ss);

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
            netConnect.RemoveINetworkConnectable(ss);

            //Assert
            checkIsConnected(serverItem, true);
            checkIsConnected(ssItem, false);

            netVM.Refresh(bid, cw);

            checkIsConnected(serverItem, true);
            checkIsConnected(ssItem, false);
        }

        [TestMethod]
        public void DisconnectServerFromServer()
        {
            //Arrange
            TECBid bid = TestHelper.CreateEmptyCatalogBid();

            ChangeWatcher cw = new ChangeWatcher(bid);
            NetworkVM netVM = new NetworkVM(bid, cw);

            TECController server1 = new TECController(bid.Catalogs.ControllerTypes[0], false);
            server1.IsServer = true;
            TECController server2 = new TECController(bid.Catalogs.ControllerTypes[0], false);
            server2.IsServer = true;
            bid.AddController(server1);
            bid.AddController(server2);

            List<TECElectricalMaterial> connectionTypes = new List<TECElectricalMaterial>() { bid.Catalogs.ConnectionTypes[0] };

            TECNetworkConnection netConnect = server1.AddNetworkConnection(false, connectionTypes, IOType.BACnetIP);
            netConnect.AddINetworkConnectable(server2);

            ConnectableItem serverItem1 = null;
            ConnectableItem serverItem2 = null;

            foreach (ConnectableItem item in netVM.Parentables)
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
            netConnect.RemoveINetworkConnectable(server2);

            //Assert
            checkIsConnected(serverItem1, true);
            checkIsConnected(serverItem2, true);

            netVM.Refresh(bid, cw);

            checkIsConnected(serverItem1, true);
            checkIsConnected(serverItem2, true);
        }

        [TestMethod]
        public void DisconnectParentControllerFromServer()
        {
            //Arrange
            TECBid bid = TestHelper.CreateEmptyCatalogBid();

            ChangeWatcher cw = new ChangeWatcher(bid);
            NetworkVM netVM = new NetworkVM(bid, cw);

            TECController server = new TECController(bid.Catalogs.ControllerTypes[0], false);
            server.IsServer = true;
            TECController parentController = new TECController(bid.Catalogs.ControllerTypes[0], false);
            TECController childController = new TECController(bid.Catalogs.ControllerTypes[0], false);
            bid.AddController(server);
            bid.AddController(parentController);
            bid.AddController(childController);

            List<TECElectricalMaterial> connectionTypes = new List<TECElectricalMaterial>() { bid.Catalogs.ConnectionTypes[0] };

            TECNetworkConnection serverConnect = server.AddNetworkConnection(false, connectionTypes, IOType.BACnetIP);
            serverConnect.AddINetworkConnectable(parentController);
            TECNetworkConnection netConnect = parentController.AddNetworkConnection(false, connectionTypes, IOType.BACnetIP);
            netConnect.AddINetworkConnectable(childController);

            ConnectableItem serverItem = null;
            ConnectableItem parentItem = null;
            ConnectableItem childItem = null;

            foreach (ConnectableItem item in netVM.Parentables)
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
            serverConnect.RemoveINetworkConnectable(parentController);

            //Assert
            checkIsConnected(serverItem, true);
            checkIsConnected(parentItem, false);
            checkIsConnected(childItem, false);

            netVM.Refresh(bid, cw);

            checkIsConnected(serverItem, true);
            checkIsConnected(parentItem, false);
            checkIsConnected(childItem, false);
        }
        #endregion

        #region Make Server Tests
        [TestMethod]
        public void MakeServer()
        {
            //Arrange
            TECBid bid = TestHelper.CreateEmptyCatalogBid();

            ChangeWatcher cw = new ChangeWatcher(bid);
            NetworkVM netVM = new NetworkVM(bid, cw);

            TECController parent = new TECController(bid.Catalogs.ControllerTypes[0], false);
            TECController server = new TECController(bid.Catalogs.ControllerTypes[0], false);
            server.IsServer = true;
            TECController controller = new TECController(bid.Catalogs.ControllerTypes[0], false);
            bid.AddController(parent);
            bid.AddController(server);
            bid.AddController(controller);

            List<TECElectricalMaterial> connectionTypes = new List<TECElectricalMaterial>() { bid.Catalogs.ConnectionTypes[0] };
            TECDevice device = new TECDevice(connectionTypes, bid.Catalogs.Manufacturers[0]);
            bid.Catalogs.Devices.Add(device);

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

            TECNetworkConnection serverConnect = parent.AddNetworkConnection(false, connectionTypes, IOType.BACnetIP);
            TECNetworkConnection controllerConnect = parent.AddNetworkConnection(false, connectionTypes, IOType.BACnetIP);
            TECNetworkConnection ssConnect = parent.AddNetworkConnection(false, connectionTypes, IOType.BACnetIP);

            serverConnect.AddINetworkConnectable(server);
            controllerConnect.AddINetworkConnectable(controller);
            ssConnect.AddINetworkConnectable(ss);

            ConnectableItem parentItem = null;
            ConnectableItem serverItem = null;
            ConnectableItem controllerItem = null;
            ConnectableItem ssItem = null;

            foreach (ConnectableItem item in netVM.Parentables)
            {
                if (item.Item == parent)
                {
                    parentItem = item;
                }
                else if (item.Item == server)
                {
                    serverItem = item;
                }
                else if (item.Item == controller)
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
            parent.IsServer = true;

            //Assert
            checkIsConnected(parentItem, true);
            checkIsConnected(serverItem, true);
            checkIsConnected(controllerItem, true);
            checkIsConnected(ssItem, true);

            netVM.Refresh(bid, cw);

            checkIsConnected(parentItem, true);
            checkIsConnected(serverItem, true);
            checkIsConnected(controllerItem, true);
            checkIsConnected(ssItem, true);
        }

        [TestMethod]
        public void MakeNotServer()
        {
            //Arrange
            TECBid bid = TestHelper.CreateEmptyCatalogBid();

            ChangeWatcher cw = new ChangeWatcher(bid);
            NetworkVM netVM = new NetworkVM(bid, cw);

            TECController parent = new TECController(bid.Catalogs.ControllerTypes[0], false);
            parent.IsServer = true;
            TECController server = new TECController(bid.Catalogs.ControllerTypes[0], false);
            server.IsServer = true;
            TECController controller = new TECController(bid.Catalogs.ControllerTypes[0], false);
            bid.AddController(parent);
            bid.AddController(server);
            bid.AddController(controller);

            List<TECElectricalMaterial> connectionTypes = new List<TECElectricalMaterial>() { bid.Catalogs.ConnectionTypes[0] };
            TECDevice device = new TECDevice(connectionTypes, bid.Catalogs.Manufacturers[0]);
            bid.Catalogs.Devices.Add(device);

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

            TECNetworkConnection serverConnect = parent.AddNetworkConnection(false, connectionTypes, IOType.BACnetIP);
            TECNetworkConnection controllerConnect = parent.AddNetworkConnection(false, connectionTypes, IOType.BACnetIP);
            TECNetworkConnection ssConnect = parent.AddNetworkConnection(false, connectionTypes, IOType.BACnetIP);

            serverConnect.AddINetworkConnectable(server);
            controllerConnect.AddINetworkConnectable(controller);
            ssConnect.AddINetworkConnectable(ss);

            ConnectableItem parentItem = null;
            ConnectableItem serverItem = null;
            ConnectableItem controllerItem = null;
            ConnectableItem ssItem = null;

            foreach (ConnectableItem item in netVM.Parentables)
            {
                if (item.Item == parent)
                {
                    parentItem = item;
                }
                else if (item.Item == server)
                {
                    serverItem = item;
                }
                else if (item.Item == controller)
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
            parent.IsServer = false;

            //Assert
            checkIsConnected(parentItem, false);
            checkIsConnected(serverItem, true);
            checkIsConnected(controllerItem, false);
            checkIsConnected(ssItem, false);

            netVM.Refresh(bid, cw);

            checkIsConnected(parentItem, false);
            checkIsConnected(serverItem, true);
            checkIsConnected(controllerItem, false);
            checkIsConnected(ssItem, false);
        }
        #endregion

        #region Remove Tests
        [TestMethod]
        public void RemoveServerWithChildren()
        {
            //Arrange
            TECBid bid = TestHelper.CreateEmptyCatalogBid();

            ChangeWatcher cw = new ChangeWatcher(bid);
            NetworkVM netVM = new NetworkVM(bid, cw);

            TECController parent = new TECController(bid.Catalogs.ControllerTypes[0], false);
            parent.IsServer = true;
            TECController server = new TECController(bid.Catalogs.ControllerTypes[0], false);
            server.IsServer = true;
            TECController controller = new TECController(bid.Catalogs.ControllerTypes[0], false);
            bid.AddController(parent);
            bid.AddController(server);
            bid.AddController(controller);

            List<TECElectricalMaterial> connectionTypes = new List<TECElectricalMaterial>() { bid.Catalogs.ConnectionTypes[0] };
            TECDevice device = new TECDevice(connectionTypes, bid.Catalogs.Manufacturers[0]);
            bid.Catalogs.Devices.Add(device);

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

            TECNetworkConnection serverConnect = parent.AddNetworkConnection(false, connectionTypes, IOType.BACnetIP);
            TECNetworkConnection controllerConnect = parent.AddNetworkConnection(false, connectionTypes, IOType.BACnetIP);
            TECNetworkConnection ssConnect = parent.AddNetworkConnection(false, connectionTypes, IOType.BACnetIP);

            serverConnect.AddINetworkConnectable(server);
            controllerConnect.AddINetworkConnectable(controller);
            ssConnect.AddINetworkConnectable(ss);
            
            ConnectableItem serverItem = null;
            ConnectableItem controllerItem = null;
            ConnectableItem ssItem = null;

            foreach (ConnectableItem item in netVM.Parentables)
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
            foreach (ConnectableItem item in netVM.NonParentables)
            {
                if (item.Item == ss)
                {
                    ssItem = item;
                }
            }

            //Act
            bid.RemoveController(parent);

            //Assert
            checkIsConnected(serverItem, true);
            checkIsConnected(controllerItem, false);
            checkIsConnected(ssItem, false);

            netVM.Refresh(bid, cw);
            
            checkIsConnected(serverItem, true);
            checkIsConnected(controllerItem, false);
            checkIsConnected(ssItem, false);
        }

        [TestMethod]
        public void RemoveConnectedParent()
        {
            //Arrange
            TECBid bid = TestHelper.CreateEmptyCatalogBid();

            ChangeWatcher cw = new ChangeWatcher(bid);
            NetworkVM netVM = new NetworkVM(bid, cw);

            TECController server = new TECController(bid.Catalogs.ControllerTypes[0], false);
            server.IsServer = true;
            server.Name = "Server";
            TECController parent = new TECController(bid.Catalogs.ControllerTypes[0], false);
            parent.Name = "Parent";
            TECController controller = new TECController(bid.Catalogs.ControllerTypes[0], false);
            controller.Name = "Controller";
            bid.AddController(parent);
            bid.AddController(server);
            bid.AddController(controller);

            List<TECElectricalMaterial> connectionTypes = new List<TECElectricalMaterial>() { bid.Catalogs.ConnectionTypes[0] };
            TECDevice device = new TECDevice(connectionTypes, bid.Catalogs.Manufacturers[0]);
            bid.Catalogs.Devices.Add(device);

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

            TECNetworkConnection serverConnect = server.AddNetworkConnection(false, connectionTypes, IOType.BACnetIP);
            TECNetworkConnection controllerConnect = parent.AddNetworkConnection(false, connectionTypes, IOType.BACnetIP);
            TECNetworkConnection ssConnect = parent.AddNetworkConnection(false, connectionTypes, IOType.BACnetIP);

            serverConnect.AddINetworkConnectable(parent);
            controllerConnect.AddINetworkConnectable(controller);
            ssConnect.AddINetworkConnectable(ss);

            ConnectableItem serverItem = null;
            ConnectableItem controllerItem = null;
            ConnectableItem ssItem = null;

            foreach (ConnectableItem item in netVM.Parentables)
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
            foreach (ConnectableItem item in netVM.NonParentables)
            {
                if (item.Item == ss)
                {
                    ssItem = item;
                }
            }

            //Act
            bid.RemoveController(parent);

            //Assert
            checkIsConnected(serverItem, true);
            checkIsConnected(controllerItem, false);
            checkIsConnected(ssItem, false);

            netVM.Refresh(bid, cw);

            checkIsConnected(serverItem, true);
            checkIsConnected(controllerItem, false);
            checkIsConnected(ssItem, false);
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
            //bid.AddController(server);

            //TECController ddc = new TECController(controllerType, false);
            //ddc.NetworkType = EstimatingLibrary.NetworkType.DDC;
            //bid.AddController(ddc);

            //TECController unitary = new TECController(controllerType, false);
            //unitary.NetworkType = EstimatingLibrary.NetworkType.Unitary;
            //bid.AddController(unitary);

            //TECController child = new TECController(controllerType, false);
            //child.NetworkType = EstimatingLibrary.NetworkType.Unitary;
            //bid.AddController(child);

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
            //bid.AddController(bacnetController);

            //TECController lonController = new TECController(lonType, false);
            //lonController.NetworkType = EstimatingLibrary.NetworkType.Server;
            //bid.AddController(lonController);

            //TECController childController = new TECController(bacnetType, false);
            //bid.AddController(childController);

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
            //bid.AddController(parentController);

            //TECController adoptedChildController = new TECController(controllerType, false);
            //bid.AddController(adoptedChildController);

            //TECController orphanChildController = new TECController(controllerType, false);
            //bid.AddController(orphanChildController);

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
            //bid.AddController(grandparent);

            //TECController parent = new TECController(controllerType, false);
            //parent.NetworkType = EstimatingLibrary.NetworkType.DDC;
            //bid.AddController(parent);

            //TECController child = new TECController(controllerType, false);
            //child.NetworkType = EstimatingLibrary.NetworkType.DDC;
            //bid.AddController(child);

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
