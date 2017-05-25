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
    /// Interaction logic for PanelTypeGridControl.xaml
    /// </summary>
    public partial class PanelTypeGridControl : UserControl
    {
        #region DPs
        /// <summary>
        /// Gets or sets the DevicesSource which is displayed
        /// </summary>
        public ObservableCollection<TECPanelType> PanelTypeSource
        {
            get { return (ObservableCollection<TECPanelType>)GetValue(PanelTypeSourceProperty); }
            set { SetValue(PanelTypeSourceProperty, value); }
        }

        /// <summary>
        /// Identified the DevicesSource dependency property
        /// </summary>
        public static readonly DependencyProperty PanelTypeSourceProperty =
            DependencyProperty.Register("PanelTypeSource", typeof(ObservableCollection<TECPanelType>),
              typeof(PanelTypeGridControl), new PropertyMetadata(default(ObservableCollection<TECPanelType>)));

        public PanelTypeGridControl()
        {
            InitializeComponent();
        }
    }
}
