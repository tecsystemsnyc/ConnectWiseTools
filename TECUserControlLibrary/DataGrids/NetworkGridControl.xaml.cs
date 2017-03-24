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
using TECUserControlLibrary.Models;

namespace TECUserControlLibrary.DataGrids
{
    /// <summary>
    /// Interaction logic for NetworkGridControl.xaml
    /// </summary>
    public partial class NetworkGridControl : UserControl
    {
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
              typeof(NetworkGridControl));

        public ObservableCollection<object> ControllerConnectionsSource
        {
            get { return (ObservableCollection<object>)GetValue(ControllerConnectionsSourceProperty); }
            set { SetValue(ControllerConnectionsSourceProperty, value); }
        }

        public static readonly DependencyProperty ControllerConnectionsSourceProperty =
            DependencyProperty.Register("ControllerConnectionsSource", typeof(ObservableCollection<object>),
                typeof(NetworkGridControl), new PropertyMetadata(default(ObservableCollection<object>)));

        public NetworkGridControl()
        {
            InitializeComponent();
        }
    }
}
