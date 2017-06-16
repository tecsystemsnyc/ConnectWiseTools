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

namespace TECUserControlLibrary.UserControls
{
    /// <summary>
    /// Interaction logic for AssociatedCostsSummaryGrid.xaml
    /// </summary>
    public partial class CostsSummaryGridControl : UserControl
    {
        public ObservableCollection<CostSummaryItem> CostSummaryItemsSource
        {
            get { return (ObservableCollection<CostSummaryItem>)GetValue(CostSummaryItemsSourceProperty); }
            set { SetValue(CostSummaryItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty CostSummaryItemsSourceProperty =
            DependencyProperty.Register("CostSummaryItemsSource", typeof(ObservableCollection<CostSummaryItem>),
              typeof(CostsSummaryGridControl), new PropertyMetadata(default(ObservableCollection<CostSummaryItem>)));

        public CostsSummaryGridControl()
        {
            InitializeComponent();
        }
    }
}
