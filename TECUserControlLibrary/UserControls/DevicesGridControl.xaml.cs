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
using System.Collections.ObjectModel;

namespace TECUserControlLibrary.DataGrids
{

    /// <summary>
    /// Interaction logic for DevicesGridControl.xaml
    /// </summary>
    public partial class DevicesGridControl : UserControl
    {

        #region DPs

        /// <summary>
        /// Gets or sets the DevicesSource which is displayed
        /// </summary>
        public ObservableCollection<TECDevice> DevicesSource
        {
            get { return (ObservableCollection<TECDevice>)GetValue(DevicesSourceProperty); }
            set { SetValue(DevicesSourceProperty, value); }
        }

        /// <summary>
        /// Identified the DevicesSource dependency property
        /// </summary>
        public static readonly DependencyProperty DevicesSourceProperty =
            DependencyProperty.Register("DevicesSource", typeof(ObservableCollection<TECDevice>),
              typeof(DevicesGridControl), new PropertyMetadata(default(ObservableCollection<TECDevice>)));

        public TECDevice SelectedDevice
        {
            get { return (TECDevice)GetValue(SelectedDeviceProperty); }
            set { SetValue(SelectedDeviceProperty, value); }
        }

        public static readonly DependencyProperty SelectedDeviceProperty = DependencyProperty.Register("SelectedDevice", typeof(TECDevice), typeof(DevicesGridControl), new FrameworkPropertyMetadata(null)
        {
            BindsTwoWayByDefault = true,
            DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
        });
        #endregion

        public DevicesGridControl()
        {
            InitializeComponent();
        }
    }
}
