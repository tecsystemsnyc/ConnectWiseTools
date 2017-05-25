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
    /// Interaction logic for PanelsGrid.xaml
    /// </summary>
    public partial class PanelsGrid : UserControl
    {
        #region DPs

        /// <summary>
        /// Gets or sets the SystemSource which is displayed
        /// </summary>
        public ObservableCollection<TECPanel> PanelsSource
        {
            get { return (ObservableCollection<TECPanel>)GetValue(PanelsSourceProperty); }
            set { SetValue(PanelsSourceProperty, value); }
        }

        /// <summary>
        /// Identified the SystemSource dependency property
        /// </summary>
        public static readonly DependencyProperty PanelsSourceProperty =
            DependencyProperty.Register("PanelsSource", typeof(ObservableCollection<TECPanel>),
              typeof(PanelsGrid), new PropertyMetadata(default(ObservableCollection<TECPanel>)));

        #endregion
        public PanelsGrid()
        {
            InitializeComponent();
        }
    }
}
