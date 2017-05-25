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
    /// Interaction logic for IOGridControl.xaml
    /// </summary>
    public partial class IOGridControl : UserControl
    {
        /// <summary>
        /// Gets or sets the SystemSource which is displayed
        /// </summary>
        public ObservableCollection<TECIO> IOSource
        {
            get { return (ObservableCollection<TECIO>)GetValue(IOSourceProperty); }
            set { SetValue(IOSourceProperty, value); }
        }

        /// <summary>
        /// Identified the SystemSource dependency property
        /// </summary>
        public static readonly DependencyProperty IOSourceProperty =
            DependencyProperty.Register("IOSource", typeof(ObservableCollection<TECIO>),
              typeof(IOGridControl), new PropertyMetadata(default(ObservableCollection<TECIO>)));

        public IOGridControl()
        {
            InitializeComponent();
        }
    }
}
