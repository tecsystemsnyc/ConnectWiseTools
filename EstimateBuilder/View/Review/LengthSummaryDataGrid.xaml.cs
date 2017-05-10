using EstimateBuilder.Model;
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

namespace EstimateBuilder.View.Review
{
    public partial class LengthSummaryDataGrid : UserControl
    {
        public ObservableCollection<LengthSummaryItem> LengthSummaryItemsSource
        {
            get { return (ObservableCollection<LengthSummaryItem>)GetValue(LengthSummaryItemsSourceProperty); }
            set { SetValue(LengthSummaryItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty LengthSummaryItemsSourceProperty =
            DependencyProperty.Register("LengthSummaryItemsSource", typeof(ObservableCollection<LengthSummaryItem>),
              typeof(LengthSummaryDataGrid), new PropertyMetadata(default(ObservableCollection<LengthSummaryItem>)));

        public LengthSummaryDataGrid()
        {
            InitializeComponent();
        }
    }
}
