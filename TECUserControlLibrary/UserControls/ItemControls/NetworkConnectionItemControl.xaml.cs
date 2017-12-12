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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TECUserControlLibrary.Models;

namespace TECUserControlLibrary.UserControls.ItemControls
{
    /// <summary>
    /// Interaction logic for NetworkConnectionItemControl.xaml
    /// </summary>
    public partial class NetworkConnectionItemControl : UserControl
    {


        public NetworkConnectionVM NetworkConnection
        {
            get { return ( NetworkConnectionVM)GetValue(NetworkConnectionProperty); }
            set { SetValue(NetworkConnectionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NetworkConnection.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NetworkConnectionProperty =
            DependencyProperty.Register("NetworkConnection", typeof(NetworkConnectionVM), typeof(NetworkConnectionItemControl));



        public ICommand RemoveConnectionCommand
        {
            get { return (ICommand)GetValue(RemoveConnectionCommandProperty); }
            set { SetValue(RemoveConnectionCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RemoveConnectionCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RemoveConnectionCommandProperty =
            DependencyProperty.Register("RemoveConnectionCommand", typeof(ICommand), typeof(NetworkConnectionItemControl));

        public NetworkConnectionItemControl()
        {
            InitializeComponent();
        }
    }
}
