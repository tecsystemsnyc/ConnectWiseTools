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
using TECUserControlLibrary.Models;

namespace TECUserControlLibrary.UserControls
{
    /// <summary>
    /// Interaction logic for RatedCostSummaryItemGridControl.xaml
    /// </summary>
    public partial class RatedCostSummaryItemGridControl : UserControl
    {
        public RatedCostSummaryItemGridControl()
        {
            InitializeComponent();
        }

        public IEnumerable<RatedCostSummaryItem> ItemsSource
        {
            get { return (IEnumerable<RatedCostSummaryItem>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemsSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable<RatedCostSummaryItem>), typeof(RatedCostSummaryItemGridControl));


    }
}
