using EstimatingLibrary;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace TECUserControlLibrary.UserControls
{
    /// <summary>
    /// Interaction logic for DeviceConnectionTypeGrid.xaml
    /// </summary>
    public partial class DeviceConnectionTypeGridControl : UserControl
    {
        #region DPs

        /// <summary>
        /// Gets or sets the DevicesSource which is displayed
        /// </summary>
        public ObservableCollection<TECElectricalMaterial> ConnectionTypesSource
        {
            get { return (ObservableCollection<TECElectricalMaterial>)GetValue(ConnectionTypesSourceProperty); }
            set { SetValue(ConnectionTypesSourceProperty, value); }
        }

        /// <summary>
        /// Identified the DevicesSource dependency property
        /// </summary>
        public static readonly DependencyProperty ConnectionTypesSourceProperty =
            DependencyProperty.Register("ConnectionTypesSource", typeof(ObservableCollection<TECElectricalMaterial>),
              typeof(DeviceConnectionTypeGridControl), new PropertyMetadata(default(ObservableCollection<TECElectricalMaterial>)));
        
        #endregion
        public DeviceConnectionTypeGridControl()
        {
            InitializeComponent();
        }
    }
}
