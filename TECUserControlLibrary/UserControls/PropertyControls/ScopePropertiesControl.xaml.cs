using EstimatingLibrary;
using System.Windows;
using System.Windows.Controls;

namespace TECUserControlLibrary.UserControls.PropertyControls
{
    /// <summary>
    /// Interaction logic for ScopePropertiesControl.xaml
    /// </summary>
    public partial class ScopePropertiesControl : UserControl
    {
        public TECScope Selected
        {
            get { return (TECScope)GetValue(SelectedProperty); }
            set { SetValue(SelectedProperty, value); }
        }
        
        public static readonly DependencyProperty SelectedProperty =
            DependencyProperty.Register("Selected", typeof(TECScope),
              typeof(ScopePropertiesControl));

        public ScopePropertiesControl()
        {
            InitializeComponent();
        }
    }
}
