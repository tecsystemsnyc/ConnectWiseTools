using EstimatingLibrary;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace TECUserControlLibrary.UserControls
{
    /// <summary>
    /// Interaction logic for ConnectionTypeGridControl.xaml
    /// </summary>
    public partial class ConnectionTypeGridControl : UserControl
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
              typeof(ConnectionTypeGridControl), new PropertyMetadata(default(ObservableCollection<TECElectricalMaterial>)));


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
              typeof(ConnectionTypeGridControl));
        #endregion

        public ConnectionTypeGridControl()
        {
            InitializeComponent();
        }
    }
}
