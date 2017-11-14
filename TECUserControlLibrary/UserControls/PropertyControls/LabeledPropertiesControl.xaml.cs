using EstimatingLibrary;
using System.Windows;
using System.Windows.Controls;

namespace TECUserControlLibrary.UserControls.PropertyControls
{
    /// <summary>
    /// Interaction logic for LabeledPropertiesControl.xaml
    /// </summary>
    public partial class LabeledPropertiesControl : UserControl
    {

        public TECLabeled Selected
        {
            get { return (TECLabeled)GetValue(SelectedProperty); }
            set { SetValue(SelectedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Selected.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedProperty =
            DependencyProperty.Register("Selected", typeof(TECLabeled), typeof(LabeledPropertiesControl));


        public LabeledPropertiesControl()
        {
            InitializeComponent();
        }
    }
}
