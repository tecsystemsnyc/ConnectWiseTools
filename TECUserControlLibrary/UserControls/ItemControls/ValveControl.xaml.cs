using EstimatingLibrary;
using System.Windows;

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
