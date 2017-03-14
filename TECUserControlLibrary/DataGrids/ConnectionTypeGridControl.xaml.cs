using EstimatingLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace TECUserControlLibrary.DataGrids
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
        public ObservableCollection<TECConnectionType> ConnectionTypesSource
        {
            get { return (ObservableCollection<TECConnectionType>)GetValue(ConnectionTypesSourceProperty); }
            set { SetValue(ConnectionTypesSourceProperty, value); }
        }

        /// <summary>
        /// Identified the DevicesSource dependency property
        /// </summary>
        public static readonly DependencyProperty ConnectionTypesSourceProperty =
            DependencyProperty.Register("ConnectionTypesSource", typeof(ObservableCollection<TECConnectionType>),
              typeof(ConnectionTypeGridControl), new PropertyMetadata(default(ObservableCollection<TECConnectionType>)));


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
