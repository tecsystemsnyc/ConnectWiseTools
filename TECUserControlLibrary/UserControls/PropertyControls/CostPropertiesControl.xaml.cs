using EstimatingLibrary;
using System.Windows;
using System.Windows.Controls;

namespace TECUserControlLibrary.UserControls.PropertyControls
{
    /// <summary>
    /// Interaction logic for CostPropertiesControl.xaml
    /// </summary>
    public partial class CostPropertiesControl : UserControl
    {

        public TECCost Selected
        {
            get { return (TECCost)GetValue(SelectedProperty); }
            set { SetValue(SelectedProperty, value); }
        }
        
        public static readonly DependencyProperty SelectedProperty =
            DependencyProperty.Register("Selected", typeof(TECCost),
              typeof(CostPropertiesControl));

        public CostPropertiesControl()
        {
            InitializeComponent();
        }
    }
}
