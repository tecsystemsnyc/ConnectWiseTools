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
    /// Interaction logic for PointsGridControl.xaml
    /// </summary>
    public partial class PointsGridControl : UserControl
    {
        #region DPs

        /// <summary>
        /// Gets or sets the DevicesSource which is displayed
        /// </summary>
        public ObservableCollection<TECPoint> PointsSource
        {
            get { return (ObservableCollection<TECPoint>)GetValue(PointsSourceProperty); }
            set { SetValue(PointsSourceProperty, value); }
        }

        /// <summary>
        /// Identified the DevicesSource dependency property
        /// </summary>
        public static readonly DependencyProperty PointsSourceProperty =
            DependencyProperty.Register("PointsSource", typeof(ObservableCollection<TECPoint>),
              typeof(PointsGridControl), new PropertyMetadata(default(ObservableCollection<TECPoint>)));

        #endregion
        public PointsGridControl()
        {
            InitializeComponent();
        }
    }
}
