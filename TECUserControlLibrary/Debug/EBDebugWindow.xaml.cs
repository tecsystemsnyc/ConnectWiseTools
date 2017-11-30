using EstimatingLibrary;
using EstimatingUtilitiesLibrary.Database;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace TECUserControlLibrary.Debug
{
    /// <summary>
    /// Interaction logic for DebugWindow.xaml
    /// </summary>
    public partial class EBDebugWindow : Window
    {
        private TECBid bid;
        private ICommand testNetwork;
        private ICommand addTypical;
        private ICommand throwException;
        private ICommand exportDBCommand;

        public EBDebugWindow(TECBid bid)
        {
            InitializeComponent();
            this.bid = bid;
            setupCommands();
            addResources();
        }

        private void setupCommands()
        {
            testNetwork = new RelayCommand(testNetworkExecute);
            addTypical = new RelayCommand(addTypicalExecute);
            throwException = new RelayCommand(throwExceptionExecute);
            exportDBCommand = new RelayCommand(exportDBCVSExecute);
        }
        
        private void addResources()
        {
            this.Resources.Add("TestNetworkCommand", testNetwork);
            this.Resources.Add("AddTypicalCommand", addTypical);
            this.Resources.Add("ThrowExceptionCommand", throwException);
            this.Resources.Add("ExportDBCommand", exportDBCommand);
        }

        private void testNetworkExecute()
        {
            TECControllerType type = new TECControllerType(bid.Catalogs.Manufacturers[0]);
            type.Name = "Controller Type";
            type.IO = new System.Collections.ObjectModel.ObservableCollection<TECIO>() { new TECIO(IOType.BACnetIP) };

            bid.Catalogs.ControllerTypes.Add(type);

            TECController controller = new TECController(type, false);
            controller.Name = "Test Server";
            controller.Description = "For testing.";
            controller.IsServer = true;

            bid.AddController(controller);

            TECController child = new TECController(type, false);
            child.Name = "Child";

            bid.AddController(child);

            TECController emptyController = new TECController(type, false);
            emptyController.Name = "EmptyController";

            bid.AddController(emptyController);

            TECNetworkConnection connection = controller.AddNetworkConnection(false, new List<TECConnectionType>() { bid.Catalogs.ConnectionTypes[0], bid.Catalogs.ConnectionTypes[1] }, IOType.BACnetIP);

            connection.AddINetworkConnectable(child);

            TECTypical typical = new TECTypical();
            TECEquipment equip = new TECEquipment(true);
            TECSubScope ss = new TECSubScope(true);
            ss.Name = "Test Subscope";
            ss.Devices.Add(bid.Catalogs.Devices[0]);
            TECPoint point = new TECPoint(true);
            point.Type = IOType.BACnetIP;
            point.Quantity = 1;
            ss.Points.Add(point);
            equip.SubScope.Add(ss);
            typical.Equipment.Add(equip);

            bid.Systems.Add(typical);
            typical.AddInstance(bid);
        }
        
        private void addTypicalExecute()
        {
            TECTypical typical = new TECTypical();
            typical.Name = "test";
            TECEquipment equipment = new TECEquipment(true);
            equipment.Name = "test equipment";
            TECSubScope ss = new TECSubScope(true);
            ss.Name = "Test Subscope";
            ss.Devices.Add(bid.Catalogs.Devices[0]);
            TECPoint point = new TECPoint(true);
            point.Type = IOType.BACnetIP;
            point.Quantity = 1;
            ss.Points.Add(point);
            equipment.SubScope.Add(ss);
            typical.Equipment.Add(equipment);

            TECSubScope connected = new TECSubScope(true);
            connected.Name = "Connected";
            connected.Devices.Add(bid.Catalogs.Devices[0]);
            TECPoint point2 = new TECPoint(true);
            point2.Type = IOType.AI;
            point2.Quantity = 1;
            connected.Points.Add(point2);
            equipment.SubScope.Add(connected);

            TECSubScope toConnect = new TECSubScope(true);
            toConnect.Name = "To Connect";
            toConnect.Devices.Add(bid.Catalogs.Devices[0]);
            TECPoint point3 = new TECPoint(true);
            point3.Type = IOType.AI;
            point3.Quantity = 1;
            toConnect.Points.Add(point3);
            equipment.SubScope.Add(toConnect);

            TECControllerType controllerType = new TECControllerType(new TECManufacturer());
            controllerType.IOModules.Add(bid.Catalogs.IOModules[0]);
            TECIO io = new TECIO(IOType.AI);
            io.Quantity = 10;
            controllerType.IO.Add(io);
            bid.Catalogs.IOModules[0].IO.Add(io);
            controllerType.Name = "Test Type";

            TECController controller = new TECController(controllerType, true);
            controller.IOModules.Add(bid.Catalogs.IOModules[0]);
            controller.Name = "Test Controller";
            typical.AddController(controller);
            TECController otherController = new TECController(controllerType, true);
            otherController.Name = "Other Controller";
            typical.AddController(otherController);
            TECConnection connection = controller.AddSubScope(connected);
            connection.Length = 10;
            connection.ConduitLength = 20;
            connection.ConduitType = bid.Catalogs.ConduitTypes[1];

            TECPanelType panelType = new TECPanelType(new TECManufacturer());
            panelType.Name = "test type";

            TECPanel panel = new TECPanel(panelType, true);
            panel.Name = "Test Panel";
            typical.Panels.Add(panel);

            TECMisc misc = new TECMisc(CostType.TEC, true);
            misc.Name = "test Misc";
            typical.MiscCosts.Add(misc);

            bid.Systems.Add(typical);
            typical.AddInstance(bid);
        }

        private void throwExceptionExecute()
        {
            throw new Exception("Test Exception.");
        }

        private void exportDBCVSExecute()
        {
            DatabaseManager<TECScopeManager>.ExportDef();
        }
    }
}
