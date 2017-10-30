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


        public SummaryDataGrid()
        {
            InitializeComponent();
        }
    }
}
