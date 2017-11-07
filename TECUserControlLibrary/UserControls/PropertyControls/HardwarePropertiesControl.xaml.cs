using EstimatingLibrary;
using System.Windows;
using System.Windows.Controls;

namespace TECUserControlLibrary.UserControls.PropertyControls
{
    /// <summary>
    /// Interaction logic for HardwarePropertiesControl.xaml
    /// </summary>
    public partial class HardwarePropertiesControl : UserControl
    {
        public TECHardware Selected
        {
            get { return (TECHardware)GetValue(SelectedProperty); }
            set { SetValue(SelectedProperty, value); }
        }

        public static readonly DependencyProperty SelectedProperty =
            DependencyProperty.Register("Selected", typeof(TECHardware),
              typeof(HardwarePropertiesControl));

        public HardwarePropertiesControl()
        {
            InitializeComponent();
        }
    }
}
