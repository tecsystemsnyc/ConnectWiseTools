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
        }

        private void addResources()
        {
            this.Resources.Add("TestNetworkCommand", testNetwork);
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
    }
}
