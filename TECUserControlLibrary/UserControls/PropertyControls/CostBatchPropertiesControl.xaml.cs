using EstimatingLibrary.Interfaces;
using System.Windows;
using System.Windows.Controls;

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
