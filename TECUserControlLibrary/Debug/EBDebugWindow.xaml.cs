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
        private ICommand addController;


        public EBDebugWindow(TECBid bid)
        {
            InitializeComponent();
            this.bid = bid;
            setupCommands();
            addResources();
        }

        private void setupCommands()
        {
            addController = new RelayCommand(addControllerExecute);
        }

        private void addResources()
        {
            this.Resources.Add("AddControllerCommand", addController);
        }

        private void addControllerExecute()
        {
            TECControllerType type = new TECControllerType(bid.Catalogs.Manufacturers[0]);
            type.IO = new System.Collections.ObjectModel.ObservableCollection<TECIO>() { new TECIO(IOType.BACnetIP) };

            bid.Catalogs.ControllerTypes.Add(type);

            TECController controller = new TECController(type, false);
            bid.AddController(controller);
        }
    }
}
