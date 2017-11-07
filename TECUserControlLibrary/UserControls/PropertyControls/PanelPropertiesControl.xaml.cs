using EstimatingLibrary;
using System.Windows;
using System.Windows.Controls;

namespace TECUserControlLibrary.UserControls.PropertyControls
{
    /// <summary>
    /// Interaction logic for PanelPropertiesControl.xaml
    /// </summary>
    public partial class PanelPropertiesControl : UserControl
    {

        public TECPanel Selected
        {
            get { return (TECPanel)GetValue(SelectedProperty); }
            set { SetValue(SelectedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Selected.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedProperty =
            DependencyProperty.Register("Selected", typeof(TECPanel), typeof(PanelPropertiesControl));


        public PanelPropertiesControl()
        {
            InitializeComponent();
        }
    }
}
