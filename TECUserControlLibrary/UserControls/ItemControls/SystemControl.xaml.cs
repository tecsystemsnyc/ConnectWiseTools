using EstimatingLibrary;
using System.Windows;

namespace TECUserControlLibrary.UserControls.ItemControls
{
    /// <summary>
    /// Interaction logic for SystemControl.xaml
    /// </summary>
    public partial class SystemControl : BaseItemControl
    {


        public TECSystem ControlSystem
        {
            get { return (TECSystem)GetValue(SystemProperty); }
            set { SetValue(SystemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for System.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SystemProperty =
            DependencyProperty.Register("ControlSystem", typeof(TECSystem), typeof(SystemControl));
        
        public SystemControl()
        {
            InitializeComponent();
        }
    }
}
