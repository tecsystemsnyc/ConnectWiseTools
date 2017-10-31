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

namespace TECUserControlLibrary.UserControls
{
    /// <summary>
    /// Interaction logic for SummaryDataGrid.xaml
    /// </summary>
    public partial class SummaryDataGrid : UserControl
    {

        public DataTemplate GridTemplate
        {
            get { return (DataTemplate)GetValue(GridTemplateProperty); }
            set { SetValue(GridTemplateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GridTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GridTemplateProperty =
            DependencyProperty.Register("GridTemplate", typeof(DataTemplate), typeof(SummaryDataGrid));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(SummaryDataGrid), new PropertyMetadata(""));



        public string CostString
        {
            get { return (string)GetValue(CostStringProperty); }
            set { SetValue(CostStringProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CostString.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CostStringProperty =
            DependencyProperty.Register("CostString", typeof(string), typeof(SummaryDataGrid), new PropertyMetadata(""));

        public string LaborString
        {
            get { return (string)GetValue(LaborStringProperty); }
            set { SetValue(LaborStringProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LaborString.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LaborStringProperty =
            DependencyProperty.Register("LaborString", typeof(string), typeof(SummaryDataGrid), new PropertyMetadata(""));

        public double CostValue
        {
            get { return (double)GetValue(CostValueProperty); }
            set { SetValue(CostValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CostValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CostValueProperty =
            DependencyProperty.Register("CostValue", typeof(double), typeof(SummaryDataGrid), new PropertyMetadata(0.0));

        public double LaborValue
        {
            get { return (double)GetValue(LaborValueProperty); }
            set { SetValue(LaborValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LaborValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LaborValueProperty =
            DependencyProperty.Register("LaborValue", typeof(double), typeof(SummaryDataGrid), new PropertyMetadata(0.0));

        public SummaryDataGrid()
        {
            InitializeComponent();
        }
    }
}
