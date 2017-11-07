using EstimatingLibrary;
using System.Windows;

namespace TECUserControlLibrary.UserControls.ItemControls
{
    /// <summary>
    /// Interaction logic for DeviceControl.xaml
    /// </summary>
    public partial class DeviceControl : BaseItemControl
    {

        public TECDevice Device
        {
            get { return (TECDevice)GetValue(DeviceProperty); }
            set { SetValue(DeviceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Device.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DeviceProperty =
            DependencyProperty.Register("Device", typeof(TECDevice), typeof(DeviceControl));


        public DeviceControl()
        {
            InitializeComponent();
        }
    }
}
