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
    /// Interaction logic for IOModuleGridControl.xaml
    /// </summary>
    public partial class IOModuleGridControl : UserControl
    {
        /// <summary>
        /// Gets or sets the DevicesSource which is displayed
        /// </summary>
        public ObservableCollection<TECIOModule> IOModuleSource
        {
            get { return (ObservableCollection<TECIOModule>)GetValue(IOModuleSourceProperty); }
            set { SetValue(IOModuleSourceProperty, value); }
        }

        /// <summary>
        /// Identified the DevicesSource dependency property
        /// </summary>
        public static readonly DependencyProperty IOModuleSourceProperty =
            DependencyProperty.Register("IOModuleSource", typeof(ObservableCollection<TECIOModule>),
              typeof(IOModuleGridControl), new PropertyMetadata(default(ObservableCollection<TECIOModule>)));

        public IOModuleGridControl()
        {
            InitializeComponent();
        }
    }
}
