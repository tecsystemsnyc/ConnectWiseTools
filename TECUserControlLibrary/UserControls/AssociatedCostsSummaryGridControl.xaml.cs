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
    public partial class AssociatedCostsSummaryGridControl : UserControl
    {
        public ObservableCollection<AssociatedCostSummaryItem> AssociatedCostSummaryItemsSource
        {
            get { return (ObservableCollection<AssociatedCostSummaryItem>)GetValue(AssociatedCostSummaryItemsSourceProperty); }
            set { SetValue(AssociatedCostSummaryItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty AssociatedCostSummaryItemsSourceProperty =
            DependencyProperty.Register("AssociatedCostSummaryItemsSource", typeof(ObservableCollection<AssociatedCostSummaryItem>),
              typeof(AssociatedCostsSummaryGridControl), new PropertyMetadata(default(ObservableCollection<AssociatedCostSummaryItem>)));

        public AssociatedCostsSummaryGridControl()
        {
            InitializeComponent();
        }
    }
}
