using EstimatingLibrary.Interfaces;
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

namespace TECUserControlLibrary.UserControls.PropertyControls
{
    /// <summary>
    /// Interaction logic for INotifyCostChangedPropertiesControl.xaml
    /// </summary>
    public partial class CostBatchPropertiesControl : UserControl
    {
        public INotifyCostChanged Selected
        {
            get { return (INotifyCostChanged)GetValue(SelectedProperty); }
            set { SetValue(SelectedProperty, value); }
        }

        public static readonly DependencyProperty SelectedProperty =
            DependencyProperty.Register("Selected", typeof(INotifyCostChanged),
              typeof(CostBatchPropertiesControl));

        public CostBatchPropertiesControl()
        {
            InitializeComponent();
        }
    }
}
