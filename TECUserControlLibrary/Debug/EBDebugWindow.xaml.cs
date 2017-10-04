using EstimatingLibrary;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
        }
        
        private void addResources()
        {
            this.Resources.Add("TestNetworkCommand", testNetwork);
            this.Resources.Add("AddTypicalCommand", addTypical);
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

            TECNetworkConnection connection = controller.AddNetworkConnection(false, new List<TECElectricalMaterial>() { bid.Catalogs.ConnectionTypes[0] }, IOType.BACnetIP);

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

            bid.Systems.Add(typical);
            typical.AddInstance(bid);
        }
    }
}
