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

        public bool ReadOnly
        {
            get { return (bool)GetValue(ReadOnlyProperty); }
            set { SetValue(ReadOnlyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ReadOnly.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ReadOnlyProperty =
            DependencyProperty.Register("ReadOnly", typeof(bool), typeof(CostPropertiesControl), new PropertyMetadata(false));


        public CostPropertiesControl()
        {
            InitializeComponent();
        }
    }
}
