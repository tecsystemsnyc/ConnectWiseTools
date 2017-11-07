using EstimatingLibrary;
using System.Windows;
using System.Windows.Controls;

namespace TECUserControlLibrary.UserControls.PropertyControls
{
    /// <summary>
    /// Interaction logic for LocatedPropertiesControl.xaml
    /// </summary>
    public partial class LocatedPropertiesControl : UserControl
    {
        
        public TECLocated Selected
        {
            get { return (TECLocated)GetValue(SelectedProperty); }
            set { SetValue(SelectedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Selected.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedProperty =
            DependencyProperty.Register("Selected", typeof(TECLocated), typeof(LocatedPropertiesControl));
        
        public LocatedPropertiesControl()
        {
            InitializeComponent();
        }
    }
}
