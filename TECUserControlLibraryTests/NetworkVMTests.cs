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
            throw new NotImplementedException();
            ////Arrange
            //TECBid bid = TestHelper.CreateEmptyCatalogBid();

            //ChangeWatcher cw = new ChangeWatcher(bid);
            //NetworkVM netVM = new NetworkVM(bid, cw);

            //TECController server = new TECController(bid.Catalogs.ControllerTypes[0], false);
            //server.IsServer = true;
            //TECController controller = new TECController(bid.Catalogs.ControllerTypes[0], false);
            //bid.AddController(server);
            //bid.AddController(controller);
            //TECNetworkConnection netConnect = server.AddNetworkConnection(false,
            //    new List<TECConnectionType>() { bid.Catalogs.ConnectionTypes[0] }, IOType.BACnetIP);
            
            //ConnectableItem serverItem = null;
            //ConnectableItem controllerItem = null;

            //foreach(ConnectableItem item in netVM.Parentables)
            //{
            //    if (item.Item == server)
            //    {
            //        serverItem = item;
            //    }
            //    else if (item.Item == controller)
            //    {
            //        controllerItem = item;
            //    }
            //}

            ////Act
            //netConnect.AddINetworkConnectable(controller);

            ////Assert
            //checkIsConnected(serverItem, true);
            //checkIsConnected(controllerItem, true);

            //netVM.Refresh(bid, cw);

            //checkIsConnected(serverItem, true);
            //checkIsConnected(controllerItem, true);
        }

        [TestMethod]
        public void ConnectSubScopeToServer()
        {
            throw new NotImplementedException();
            ////Arrange
            //TECBid bid = TestHelper.CreateEmptyCatalogBid();

            //ChangeWatcher cw = new ChangeWatcher(bid);
            //NetworkVM netVM = new NetworkVM(bid, cw);

            //List<TECConnectionType> connectionTypes = new List<TECConnectionType>() { bid.Catalogs.ConnectionTypes[0] };
            //TECDevice device = new TECDevice(connectionTypes, bid.Catalogs.Manufacturers[0]);
            //bid.Catalogs.Devices.Add(device);

            //TECController server = new TECController(bid.Catalogs.ControllerTypes[0], false);
            //server.IsServer = true;
            //bid.AddController(server);
            //TECNetworkConnection netConnect = server.AddNetworkConnection(false, connectionTypes, IOType.BACnetIP);

            //TECTypical typical = new TECTypical();
            //TECEquipment typEquip = new TECEquipment(true);
            //TECSubScope typSS = new TECSubScope(true);
            //TECPoint typPoint = new TECPoint(true);
            //typPoint.Type = IOType.BACnetIP;
            //typPoint.Quantity = 5;
            //typSS.AddPoint(typPoint);
            //typSS.Devices.Add(device);
            //typEquip.SubScope.Add(typSS);
            //typical.Equipment.Add(typEquip);
            //bid.Systems.Add(typical);

            //TECSystem instance = typical.AddInstance(bid);
            //TECSubScope ss = instance.Equipment[0].SubScope[0];

            //ConnectableItem serverItem = null;
            //ConnectableItem ssItem = null;

            //foreach (ConnectableItem item in netVM.Parentables)
            //{
            //    if (item.Item == server)
            //    {
            //        serverItem = item;
            //    }
            //}
            //foreach (ConnectableItem item in netVM.NonParentables)
            //{
            //    if (item.Item == ss)
            //    {
            //        ssItem = item;
            //    }
            //}

            ////Act
            //netConnect.AddINetworkConnectable(ss);

            ////Assert
            //checkIsConnected(serverItem, true);
            //checkIsConnected(ssItem, true);

            //netVM.Refresh(bid, cw);

            //checkIsConnected(serverItem, true);
            //checkIsConnected(ssItem, true);
        }

        [TestMethod]
        public void ConnectServerToServer()
        {
            throw new NotImplementedException();
            ////Arrange
            //TECBid bid = TestHelper.CreateEmptyCatalogBid();

            //ChangeWatcher cw = new ChangeWatcher(bid);
            //NetworkVM netVM = new NetworkVM(bid, cw);

            //TECController server1 = new TECController(bid.Catalogs.ControllerTypes[0], false);
            //server1.IsServer = true;
            //TECController server2 = new TECController(bid.Catalogs.ControllerTypes[0], false);
            //server2.IsServer = true;
            //bid.AddController(server1);
            //bid.AddController(server2);

            //List<TECConnectionType> connectionTypes = new List<TECConnectionType>() { bid.Catalogs.ConnectionTypes[0] };

            //TECNetworkConnection netConnect = server1.AddNetworkConnection(false, connectionTypes, IOType.BACnetIP);

            //ConnectableItem serverItem1 = null;
            //ConnectableItem serverItem2 = null;

            //foreach(ConnectableItem item in netVM.Parentables)
            //{
            //    if (item.Item == server1)
            //    {
            //        serverItem1 = item;
            //    }
            //    else if (item.Item == server2)
            //    {
            //        serverItem2 = item;
            //    }
            //}

            ////Act
            //netConnect.AddINetworkConnectable(server2);

            ////Assert
            //checkIsConnected(serverItem1, true);
            //checkIsConnected(serverItem2, true);

            //netVM.Refresh(bid, cw);

            //checkIsConnected(serverItem1, true);
            //checkIsConnected(serverItem2, true);
        }

        [TestMethod]
        public void ConnectControllerToConnectedController()
        {
            throw new NotImplementedException();
            ////Arrange
            //TECBid bid = TestHelper.CreateEmptyCatalogBid();

            //ChangeWatcher cw = new ChangeWatcher(bid);
            //NetworkVM netVM = new NetworkVM(bid, cw);

            //TECController server = new TECController(bid.Catalogs.ControllerTypes[0], false);
            //server.IsServer = true;
            //TECController parentController = new TECController(bid.Catalogs.ControllerTypes[0], false);
            //TECController childController = new TECController(bid.Catalogs.ControllerTypes[0], false);
            //bid.AddController(server);
            //bid.AddController(parentController);
            //bid.AddController(childController);

            //List<TECConnectionType> connectionTypes = new List<TECConnectionType>() { bid.Catalogs.ConnectionTypes[0] };

            //TECNetworkConnection serverConnect = server.AddNetworkConnection(false, connectionTypes, IOType.BACnetIP);
            //serverConnect.AddINetworkConnectable(parentController);
            //TECNetworkConnection netConnect = parentController.AddNetworkConnection(false, connectionTypes, IOType.BACnetIP);

            //ConnectableItem serverItem = null;
            //ConnectableItem parentItem = null;
            //ConnectableItem childItem = null;

            //foreach(ConnectableItem item in netVM.Parentables)
            //{
            //    if (item.Item == server)
            //    {
            //        serverItem = item;
            //    }
            //    else if (item.Item == parentController)
            //    {
            //        parentItem = item;
            //    }
            //    else if (item.Item == childController)
            //    {
            //        childItem = item;
            //    }
            //}

            ////Act
            //netConnect.AddINetworkConnectable(childController);

            ////Assert
            //checkIsConnected(serverItem, true);
            //checkIsConnected(parentItem, true);
            //checkIsConnected(childItem, true);

            //netVM.Refresh(bid, cw);

            //checkIsConnected(serverItem, true);
            //checkIsConnected(parentItem, true);
            //checkIsConnected(childItem, true);
        }

        [TestMethod]
        public void ConnectSubScopeToConnectedController()
        {
            throw new NotImplementedException();
            ////Arrange
            //TECBid bid = TestHelper.CreateEmptyCatalogBid();

            //ChangeWatcher cw = new ChangeWatcher(bid);
            //NetworkVM netVM = new NetworkVM(bid, cw);

            //TECController server = new TECController(bid.Catalogs.ControllerTypes[0], false);
            //server.IsServer = true;
            //TECController parentController = new TECController(bid.Catalogs.ControllerTypes[0], false);
            //bid.AddController(server);
            //bid.AddController(parentController);

            //List<TECConnectionType> connectionTypes = new List<TECConnectionType>() { bid.Catalogs.ConnectionTypes[0] };
            //TECDevice device = new TECDevice(connectionTypes, bid.Catalogs.Manufacturers[0]);
            //bid.Catalogs.Devices.Add(device);

            //TECNetworkConnection serverConnect = server.AddNetworkConnection(false, connectionTypes, IOType.BACnetIP);
            //serverConnect.AddINetworkConnectable(parentController);
            //TECNetworkConnection netConnect = parentController.AddNetworkConnection(false, connectionTypes, IOType.BACnetIP);

            //TECTypical typical = new TECTypical();
            //TECEquipment typEquip = new TECEquipment(true);
            //TECSubScope typSS = new TECSubScope(true);
            //TECPoint typPoint = new TECPoint(true);
            //typPoint.Type = IOType.BACnetIP;
            //typPoint.Quantity = 5;
            //typSS.AddPoint(typPoint);
            //typSS.Devices.Add(device);
            //typEquip.SubScope.Add(typSS);
            //typical.Equipment.Add(typEquip);
            //bid.Systems.Add(typical);

            //TECSystem instance = typical.AddInstance(bid);
            //TECSubScope ss = instance.Equipment[0].SubScope[0];

            //ConnectableItem serverItem = null;
            //ConnectableItem controllerItem = null;
            //ConnectableItem ssItem = null;

            //foreach (ConnectableItem item in netVM.Parentables)
            //{
            //    if (item.Item == server)
            //    {
            //        serverItem = item;
            //    }
            //    else if (item.Item == parentController)
            //    {
            //        controllerItem = item;
            //    }
            //}
            //foreach (ConnectableItem item in netVM.NonParentables)
            //{
            //    if (item.Item == ss)
            //    {
            //        ssItem = item;
            //    }
            //}

            ////Act
            //netConnect.AddINetworkConnectable(ss);

            ////Assert
            //checkIsConnected(serverItem, true);
            //checkIsConnected(controllerItem, true);
            //checkIsConnected(ssItem, true);

            //netVM.Refresh(bid, cw);

            //checkIsConnected(serverItem, true);
            //checkIsConnected(controllerItem, true);
            //checkIsConnected(ssItem, true);
        }

        [TestMethod]
        public void ConnectParentControllerToServer()
        {
            throw new NotImplementedException();
            ////Arrange
            //TECBid bid = TestHelper.CreateEmptyCatalogBid();

            //ChangeWatcher cw = new ChangeWatcher(bid);
            //NetworkVM netVM = new NetworkVM(bid, cw);

            //TECController server = new TECController(bid.Catalogs.ControllerTypes[0], false);
            //server.IsServer = true;
            //TECController parentController = new TECController(bid.Catalogs.ControllerTypes[0], false);
            //TECController childController = new TECController(bid.Catalogs.ControllerTypes[0], false);
            //bid.AddController(server);
            //bid.AddController(parentController);
            //bid.AddController(childController);

            //List<TECConnectionType> connectionTypes = new List<TECConnectionType>() { bid.Catalogs.ConnectionTypes[0] };

            //TECNetworkConnection serverConnect = server.AddNetworkConnection(false, connectionTypes, IOType.BACnetIP);
            //TECNetworkConnection parentConnect = parentController.AddNetworkConnection(false, connectionTypes, IOType.BACnetIP);
            //parentConnect.AddINetworkConnectable(childController);

            //ConnectableItem serverItem = null;
            //ConnectableItem parentItem = null;
            //ConnectableItem childItem = null;

            //foreach(ConnectableItem item in netVM.Parentables)
            //{
            //    if (item.Item == server)
            //    {
            //        serverItem = item;
            //    }
            //    else if (item.Item == parentController)
            //    {
            //        parentItem = item;
            //    }
            //    else if (item.Item == childController)
            //    {
            //        childItem = item;
            //    }
            //}

            ////Act
            //serverConnect.AddINetworkConnectable(parentController);

            ////Assert
            //checkIsConnected(serverItem, true);
            //checkIsConnected(parentItem, true);
            //checkIsConnected(childItem, true);

            //netVM.Refresh(bid, cw);

            //checkIsConnected(serverItem, true);
            //checkIsConnected(parentItem, true);
            //checkIsConnected(childItem, true);
        }

        [TestMethod]
        public void DisconnectControllerFromServer()
        {
            throw new NotImplementedException();
            ////Arrange
            //TECBid bid = TestHelper.CreateEmptyCatalogBid();

            //ChangeWatcher cw = new ChangeWatcher(bid);
            //NetworkVM netVM = new NetworkVM(bid, cw);

            //TECController server = new TECController(bid.Catalogs.ControllerTypes[0], false);
            //server.IsServer = true;
            //TECController controller = new TECController(bid.Catalogs.ControllerTypes[0], false);
            //bid.AddController(server);
            //bid.AddController(controller);

            //List<TECConnectionType> connectionTypes = new List<TECConnectionType>() { bid.Catalogs.ConnectionTypes[0] };

            //TECNetworkConnection serverConnect = server.AddNetworkConnection(false, connectionTypes, IOType.BACnetIP);
            //serverConnect.AddINetworkConnectable(controller);

            //ConnectableItem serverItem = null;
            //ConnectableItem controllerItem = null;

            //foreach(ConnectableItem item in netVM.Parentables)
            //{
            //    if (item.Item == server)
            //    {
            //        serverItem = item;
            //    }
            //    else if (item.Item == controller)
            //    {
            //        controllerItem = item;
            //    }
            //}

            ////Act
            //serverConnect.RemoveINetworkConnectable(controller);

            ////Assert
            //checkIsConnected(serverItem, true);
            //checkIsConnected(controllerItem, false);

            //netVM.Refresh(bid, cw);

            //checkIsConnected(serverItem, true);
            //checkIsConnected(controllerItem, false);
        }

        [TestMethod]
        public void DisconnectSubScopeFromServer()
        {
            throw new NotImplementedException();
            ////Arrange
            //TECBid bid = TestHelper.CreateEmptyCatalogBid();

            //ChangeWatcher cw = new ChangeWatcher(bid);
            //NetworkVM netVM = new NetworkVM(bid, cw);

            //List<TECConnectionType> connectionTypes = new List<TECConnectionType>() { bid.Catalogs.ConnectionTypes[0] };
            //TECDevice device = new TECDevice(connectionTypes, bid.Catalogs.Manufacturers[0]);
            //bid.Catalogs.Devices.Add(device);

            //TECController server = new TECController(bid.Catalogs.ControllerTypes[0], false);
            //server.IsServer = true;
            //bid.AddController(server);
            //TECNetworkConnection netConnect = server.AddNetworkConnection(false, connectionTypes, IOType.BACnetIP);

            //TECTypical typical = new TECTypical();
            //TECEquipment typEquip = new TECEquipment(true);
            //TECSubScope typSS = new TECSubScope(true);
            //TECPoint typPoint = new TECPoint(true);
            //typPoint.Type = IOType.BACnetIP;
            //typPoint.Quantity = 5;
            //typSS.AddPoint(typPoint);
            //typSS.Devices.Add(device);
            //typEquip.SubScope.Add(typSS);
            //typical.Equipment.Add(typEquip);
            //bid.Systems.Add(typical);

            //TECSystem instance = typical.AddInstance(bid);
            //TECSubScope ss = instance.Equipment[0].SubScope[0];

            //netConnect.AddINetworkConnectable(ss);

            //ConnectableItem serverItem = null;
            //ConnectableItem ssItem = null;

            //foreach (ConnectableItem item in netVM.Parentables)
            //{
            //    if (item.Item == server)
            //    {
            //        serverItem = item;
            //    }
            //}
            //foreach (ConnectableItem item in netVM.NonParentables)
            //{
            //    if (item.Item == ss)
            //    {
            //        ssItem = item;
            //    }
            //}

            ////Act
            //netConnect.RemoveINetworkConnectable(ss);

            ////Assert
            //checkIsConnected(serverItem, true);
            //checkIsConnected(ssItem, false);

            //netVM.Refresh(bid, cw);

            //checkIsConnected(serverItem, true);
            //checkIsConnected(ssItem, false);
        }

        [TestMethod]
        public void DisconnectServerFromServer()
        {
            throw new NotImplementedException();
            ////Arrange
            //TECBid bid = TestHelper.CreateEmptyCatalogBid();

            //ChangeWatcher cw = new ChangeWatcher(bid);
            //NetworkVM netVM = new NetworkVM(bid, cw);

            //TECController server1 = new TECController(bid.Catalogs.ControllerTypes[0], false);
            //server1.IsServer = true;
            //TECController server2 = new TECController(bid.Catalogs.ControllerTypes[0], false);
            //server2.IsServer = true;
            //bid.AddController(server1);
            //bid.AddController(server2);

            //List<TECConnectionType> connectionTypes = new List<TECConnectionType>() { bid.Catalogs.ConnectionTypes[0] };

            //TECNetworkConnection netConnect = server1.AddNetworkConnection(false, connectionTypes, IOType.BACnetIP);
            //netConnect.AddINetworkConnectable(server2);

            //ConnectableItem serverItem1 = null;
            //ConnectableItem serverItem2 = null;

            //foreach (ConnectableItem item in netVM.Parentables)
            //{
            //    if (item.Item == server1)
            //    {
            //        serverItem1 = item;
            //    }
            //    else if (item.Item == server2)
            //    {
            //        serverItem2 = item;
            //    }
            //}

            ////Act
            //netConnect.RemoveINetworkConnectable(server2);

            ////Assert
            //checkIsConnected(serverItem1, true);
            //checkIsConnected(serverItem2, true);

            //netVM.Refresh(bid, cw);

            //checkIsConnected(serverItem1, true);
            //checkIsConnected(serverItem2, true);
        }

        [TestMethod]
        public void DisconnectParentControllerFromServer()
        {
            throw new NotImplementedException();
            ////Arrange
            //TECBid bid = TestHelper.CreateEmptyCatalogBid();

            //ChangeWatcher cw = new ChangeWatcher(bid);
            //NetworkVM netVM = new NetworkVM(bid, cw);

            //TECController server = new TECController(bid.Catalogs.ControllerTypes[0], false);
            //server.IsServer = true;
            //TECController parentController = new TECController(bid.Catalogs.ControllerTypes[0], false);
            //TECController childController = new TECController(bid.Catalogs.ControllerTypes[0], false);
            //bid.AddController(server);
            //bid.AddController(parentController);
            //bid.AddController(childController);

            //List<TECConnectionType> connectionTypes = new List<TECConnectionType>() { bid.Catalogs.ConnectionTypes[0] };

            //TECNetworkConnection serverConnect = server.AddNetworkConnection(false, connectionTypes, IOType.BACnetIP);
            //serverConnect.AddINetworkConnectable(parentController);
            //TECNetworkConnection netConnect = parentController.AddNetworkConnection(false, connectionTypes, IOType.BACnetIP);
            //netConnect.AddINetworkConnectable(childController);

            //ConnectableItem serverItem = null;
            //ConnectableItem parentItem = null;
            //ConnectableItem childItem = null;

            //foreach (ConnectableItem item in netVM.Parentables)
            //{
            //    if (item.Item == server)
            //    {
            //        serverItem = item;
            //    }
            //    else if (item.Item == parentController)
            //    {
            //        parentItem = item;
            //    }
            //    else if (item.Item == childController)
            //    {
            //        childItem = item;
            //    }
            //}

            ////Act
            //serverConnect.RemoveINetworkConnectable(parentController);

            ////Assert
            //checkIsConnected(serverItem, true);
            //checkIsConnected(parentItem, false);
            //checkIsConnected(childItem, false);

            //netVM.Refresh(bid, cw);

            //checkIsConnected(serverItem, true);
            //checkIsConnected(parentItem, false);
            //checkIsConnected(childItem, false);
        }
        #endregion

        #region Make Server Tests
        [TestMethod]
        public void MakeServer()
        {
            throw new NotImplementedException();
            ////Arrange
            //TECBid bid = TestHelper.CreateEmptyCatalogBid();

            //ChangeWatcher cw = new ChangeWatcher(bid);
            //NetworkVM netVM = new NetworkVM(bid, cw);

            //TECController parent = new TECController(bid.Catalogs.ControllerTypes[0], false);
            //TECController server = new TECController(bid.Catalogs.ControllerTypes[0], false);
            //server.IsServer = true;
            //TECController controller = new TECController(bid.Catalogs.ControllerTypes[0], false);
            //bid.AddController(parent);
            //bid.AddController(server);
            //bid.AddController(controller);

            //List<TECConnectionType> connectionTypes = new List<TECConnectionType>() { bid.Catalogs.ConnectionTypes[0] };
            //TECDevice device = new TECDevice(connectionTypes, bid.Catalogs.Manufacturers[0]);
            //bid.Catalogs.Devices.Add(device);

            //TECTypical typical = new TECTypical();
            //TECEquipment typEquip = new TECEquipment(true);
            //TECSubScope typSS = new TECSubScope(true);
            //TECPoint typPoint = new TECPoint(true);
            //typPoint.Type = IOType.BACnetIP;
            //typPoint.Quantity = 5;
            //typSS.AddPoint(typPoint);
            //typSS.Devices.Add(device);
            //typEquip.SubScope.Add(typSS);
            //typical.Equipment.Add(typEquip);
            //bid.Systems.Add(typical);

            //TECSystem instance = typical.AddInstance(bid);
            //TECSubScope ss = instance.Equipment[0].SubScope[0];

            //TECNetworkConnection serverConnect = parent.AddNetworkConnection(false, connectionTypes, IOType.BACnetIP);
            //TECNetworkConnection controllerConnect = parent.AddNetworkConnection(false, connectionTypes, IOType.BACnetIP);
            //TECNetworkConnection ssConnect = parent.AddNetworkConnection(false, connectionTypes, IOType.BACnetIP);

            //serverConnect.AddINetworkConnectable(server);
            //controllerConnect.AddINetworkConnectable(controller);
            //ssConnect.AddINetworkConnectable(ss);

            //ConnectableItem parentItem = null;
            //ConnectableItem serverItem = null;
            //ConnectableItem controllerItem = null;
            //ConnectableItem ssItem = null;

            //foreach (ConnectableItem item in netVM.Parentables)
            //{
            //    if (item.Item == parent)
            //    {
            //        parentItem = item;
            //    }
            //    else if (item.Item == server)
            //    {
            //        serverItem = item;
            //    }
            //    else if (item.Item == controller)
            //    {
            //        controllerItem = item;
            //    }
            //}
            //foreach (ConnectableItem item in netVM.NonParentables)
            //{
            //    if (item.Item == ss)
            //    {
            //        ssItem = item;
            //    }
            //}

            ////Act
            //parent.IsServer = true;

            ////Assert
            //checkIsConnected(parentItem, true);
            //checkIsConnected(serverItem, true);
            //checkIsConnected(controllerItem, true);
            //checkIsConnected(ssItem, true);

            //netVM.Refresh(bid, cw);

            //checkIsConnected(parentItem, true);
            //checkIsConnected(serverItem, true);
            //checkIsConnected(controllerItem, true);
            //checkIsConnected(ssItem, true);
        }

        [TestMethod]
        public void MakeNotServer()
        {
            throw new NotImplementedException();
            ////Arrange
            //TECBid bid = TestHelper.CreateEmptyCatalogBid();

            //ChangeWatcher cw = new ChangeWatcher(bid);
            //NetworkVM netVM = new NetworkVM(bid, cw);

            //TECController parent = new TECController(bid.Catalogs.ControllerTypes[0], false);
            //parent.IsServer = true;
            //TECController server = new TECController(bid.Catalogs.ControllerTypes[0], false);
            //server.IsServer = true;
            //TECController controller = new TECController(bid.Catalogs.ControllerTypes[0], false);
            //bid.AddController(parent);
            //bid.AddController(server);
            //bid.AddController(controller);

            //List<TECConnectionType> connectionTypes = new List<TECConnectionType>() { bid.Catalogs.ConnectionTypes[0] };
            //TECDevice device = new TECDevice(connectionTypes, bid.Catalogs.Manufacturers[0]);
            //bid.Catalogs.Devices.Add(device);

            //TECTypical typical = new TECTypical();
            //TECEquipment typEquip = new TECEquipment(true);
            //TECSubScope typSS = new TECSubScope(true);
            //TECPoint typPoint = new TECPoint(true);
            //typPoint.Type = IOType.BACnetIP;
            //typPoint.Quantity = 5;
            //typSS.AddPoint(typPoint);
            //typSS.Devices.Add(device);
            //typEquip.SubScope.Add(typSS);
            //typical.Equipment.Add(typEquip);
            //bid.Systems.Add(typical);

            //TECSystem instance = typical.AddInstance(bid);
            //TECSubScope ss = instance.Equipment[0].SubScope[0];

            //TECNetworkConnection serverConnect = parent.AddNetworkConnection(false, connectionTypes, IOType.BACnetIP);
            //TECNetworkConnection controllerConnect = parent.AddNetworkConnection(false, connectionTypes, IOType.BACnetIP);
            //TECNetworkConnection ssConnect = parent.AddNetworkConnection(false, connectionTypes, IOType.BACnetIP);

            //serverConnect.AddINetworkConnectable(server);
            //controllerConnect.AddINetworkConnectable(controller);
            //ssConnect.AddINetworkConnectable(ss);

            //ConnectableItem parentItem = null;
            //ConnectableItem serverItem = null;
            //ConnectableItem controllerItem = null;
            //ConnectableItem ssItem = null;

            //foreach (ConnectableItem item in netVM.Parentables)
            //{
            //    if (item.Item == parent)
            //    {
            //        parentItem = item;
            //    }
            //    else if (item.Item == server)
            //    {
            //        serverItem = item;
            //    }
            //    else if (item.Item == controller)
            //    {
            //        controllerItem = item;
            //    }
            //}
            //foreach (ConnectableItem item in netVM.NonParentables)
            //{
            //    if (item.Item == ss)
            //    {
            //        ssItem = item;
            //    }
            //}

            ////Act
            //parent.IsServer = false;

            ////Assert
            //checkIsConnected(parentItem, false);
            //checkIsConnected(serverItem, true);
            //checkIsConnected(controllerItem, false);
            //checkIsConnected(ssItem, false);

            //netVM.Refresh(bid, cw);

            //checkIsConnected(parentItem, false);
            //checkIsConnected(serverItem, true);
            //checkIsConnected(controllerItem, false);
            //checkIsConnected(ssItem, false);
        }
        #endregion

        #region Remove Tests
        [TestMethod]
        public void RemoveServerWithChildren()
        {
            throw new NotImplementedException();
            ////Arrange
            //TECBid bid = TestHelper.CreateEmptyCatalogBid();

            //ChangeWatcher cw = new ChangeWatcher(bid);
            //NetworkVM netVM = new NetworkVM(bid, cw);

            //TECController parent = new TECController(bid.Catalogs.ControllerTypes[0], false);
            //parent.IsServer = true;
            //TECController server = new TECController(bid.Catalogs.ControllerTypes[0], false);
            //server.IsServer = true;
            //TECController controller = new TECController(bid.Catalogs.ControllerTypes[0], false);
            //bid.AddController(parent);
            //bid.AddController(server);
            //bid.AddController(controller);

            //List<TECConnectionType> connectionTypes = new List<TECConnectionType>() { bid.Catalogs.ConnectionTypes[0] };
            //TECDevice device = new TECDevice(connectionTypes, bid.Catalogs.Manufacturers[0]);
            //bid.Catalogs.Devices.Add(device);

            //TECTypical typical = new TECTypical();
            //TECEquipment typEquip = new TECEquipment(true);
            //TECSubScope typSS = new TECSubScope(true);
            //TECPoint typPoint = new TECPoint(true);
            //typPoint.Type = IOType.BACnetIP;
            //typPoint.Quantity = 5;
            //typSS.AddPoint(typPoint);
            //typSS.Devices.Add(device);
            //typEquip.SubScope.Add(typSS);
            //typical.Equipment.Add(typEquip);
            //bid.Systems.Add(typical);

            //TECSystem instance = typical.AddInstance(bid);
            //TECSubScope ss = instance.Equipment[0].SubScope[0];

            //TECNetworkConnection serverConnect = parent.AddNetworkConnection(false, connectionTypes, IOType.BACnetIP);
            //TECNetworkConnection controllerConnect = parent.AddNetworkConnection(false, connectionTypes, IOType.BACnetIP);
            //TECNetworkConnection ssConnect = parent.AddNetworkConnection(false, connectionTypes, IOType.BACnetIP);

            //serverConnect.AddINetworkConnectable(server);
            //controllerConnect.AddINetworkConnectable(controller);
            //ssConnect.AddINetworkConnectable(ss);
            
            //ConnectableItem serverItem = null;
            //ConnectableItem controllerItem = null;
            //ConnectableItem ssItem = null;

            //foreach (ConnectableItem item in netVM.Parentables)
            //{
            //    if (item.Item == server)
            //    {
            //        serverItem = item;
            //    }
            //    else if (item.Item == controller)
            //    {
            //        controllerItem = item;
            //    }
            //}
            //foreach (ConnectableItem item in netVM.NonParentables)
            //{
            //    if (item.Item == ss)
            //    {
            //        ssItem = item;
            //    }
            //}

            ////Act
            //bid.RemoveController(parent);

            ////Assert
            //checkIsConnected(serverItem, true);
            //checkIsConnected(controllerItem, false);
            //checkIsConnected(ssItem, false);

            //netVM.Refresh(bid, cw);
            
            //checkIsConnected(serverItem, true);
            //checkIsConnected(controllerItem, false);
            //checkIsConnected(ssItem, false);
        }

        [TestMethod]
        public void RemoveConnectedParent()
        {
            throw new NotImplementedException();
            ////Arrange
            //TECBid bid = TestHelper.CreateEmptyCatalogBid();

            //ChangeWatcher cw = new ChangeWatcher(bid);
            //NetworkVM netVM = new NetworkVM(bid, cw);

            //TECController server = new TECController(bid.Catalogs.ControllerTypes[0], false);
            //server.IsServer = true;
            //server.Name = "Server";
            //TECController parent = new TECController(bid.Catalogs.ControllerTypes[0], false);
            //parent.Name = "Parent";
            //TECController controller = new TECController(bid.Catalogs.ControllerTypes[0], false);
            //controller.Name = "Controller";
            //bid.AddController(parent);
            //bid.AddController(server);
            //bid.AddController(controller);

            //List<TECConnectionType> connectionTypes = new List<TECConnectionType>() { bid.Catalogs.ConnectionTypes[0] };
            //TECDevice device = new TECDevice(connectionTypes, bid.Catalogs.Manufacturers[0]);
            //bid.Catalogs.Devices.Add(device);

            //TECTypical typical = new TECTypical();
            //TECEquipment typEquip = new TECEquipment(true);
            //TECSubScope typSS = new TECSubScope(true);
            //TECPoint typPoint = new TECPoint(true);
            //typPoint.Type = IOType.BACnetIP;
            //typPoint.Quantity = 5;
            //typSS.AddPoint(typPoint);
            //typSS.Devices.Add(device);
            //typEquip.SubScope.Add(typSS);
            //typical.Equipment.Add(typEquip);
            //bid.Systems.Add(typical);

            //TECSystem instance = typical.AddInstance(bid);
            //TECSubScope ss = instance.Equipment[0].SubScope[0];

            //TECNetworkConnection serverConnect = server.AddNetworkConnection(false, connectionTypes, IOType.BACnetIP);
            //TECNetworkConnection controllerConnect = parent.AddNetworkConnection(false, connectionTypes, IOType.BACnetIP);
            //TECNetworkConnection ssConnect = parent.AddNetworkConnection(false, connectionTypes, IOType.BACnetIP);

            //serverConnect.AddINetworkConnectable(parent);
            //controllerConnect.AddINetworkConnectable(controller);
            //ssConnect.AddINetworkConnectable(ss);

            //ConnectableItem serverItem = null;
            //ConnectableItem controllerItem = null;
            //ConnectableItem ssItem = null;

            //foreach (ConnectableItem item in netVM.Parentables)
            //{
            //    if (item.Item == server)
            //    {
            //        serverItem = item;
            //    }
            //    else if (item.Item == controller)
            //    {
            //        controllerItem = item;
            //    }
            //}
            //foreach (ConnectableItem item in netVM.NonParentables)
            //{
            //    if (item.Item == ss)
            //    {
            //        ssItem = item;
            //    }
            //}

            ////Act
            //bid.RemoveController(parent);

            ////Assert
            //checkIsConnected(serverItem, true);
            //checkIsConnected(controllerItem, false);
            //checkIsConnected(ssItem, false);

            //netVM.Refresh(bid, cw);

            //checkIsConnected(serverItem, true);
            //checkIsConnected(controllerItem, false);
            //checkIsConnected(ssItem, false);
        }
        #endregion
        #endregion

        //private void checkIsConnected(ConnectableItem item, bool isConnected)
        //{
        //    if (isConnected)
        //    {
        //        Assert.IsTrue(item.IsConnected, "ConnectableItem isn't connected when it should be.");
        //    }
        //    else
        //    {
        //        Assert.IsFalse(item.IsConnected, "ConnectableItem is connected when it shouldn't be.");
        //    }
        //}
    }
}
