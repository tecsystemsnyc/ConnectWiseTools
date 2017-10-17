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
using EstimatingLibrary;

namespace TECUserControlLibrary.UserControls.ItemControls
{
    /// <summary>
    /// Interaction logic for ValveControl.xaml
    /// </summary>
    public partial class ValveControl : BaseItemControl
    {

        public TECValve Valve
        {
            get { return (TECValve)GetValue(ValveProperty); }
            set { SetValue(ValveProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Valve.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValveProperty =
            DependencyProperty.Register("Valve", typeof(TECValve), typeof(ValveControl));


        public ValveControl()
        {
            InitializeComponent();
        }
    }
}
