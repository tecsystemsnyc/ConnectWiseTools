using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using TECUserControlLibrary.Models;

namespace TECUserControlLibrary.UserControls
{
    /// <summary>
    /// Interaction logic for CostSummaryItemGridControl.xaml
    /// </summary>
    public partial class CostSummaryItemGridControl : UserControl
    {
        public CostSummaryItemGridControl()
        {
            InitializeComponent();
        }

        public IEnumerable<CostSummaryItem> ItemsSource
        {
            get { return (IEnumerable<CostSummaryItem>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemsSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable<CostSummaryItem>), typeof(CostSummaryItemGridControl));


    }
}
