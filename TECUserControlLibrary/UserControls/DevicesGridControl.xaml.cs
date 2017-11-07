using EstimatingLibrary;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace TECUserControlLibrary.UserControls
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

        public static readonly DependencyProperty SelectedDeviceProperty = 
            DependencyProperty.Register("SelectedDevice", typeof(TECDevice), 
                typeof(DevicesGridControl), new FrameworkPropertyMetadata(null)
        {
            BindsTwoWayByDefault = true,
            DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
        });

        /// <summary>
        /// Gets or sets the ViewModel which is used
        /// </summary>
        public Object ViewModel
        {
            get { return (Object)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// Identified the ViewModel dependency property
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(Object),
              typeof(DevicesGridControl));
        #endregion

        public DevicesGridControl()
        {
            InitializeComponent();
        }
    }
}
