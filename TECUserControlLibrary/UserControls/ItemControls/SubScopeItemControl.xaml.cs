using EstimatingLibrary;
using System.Windows;

namespace TECUserControlLibrary.UserControls.ItemControls
{
    /// <summary>
    /// Interaction logic for SubScopeItemControl.xaml
    /// </summary>
    public partial class SubScopeItemControl : BaseItemControl
    {


        public TECSubScope SubScope
        {
            get { return (TECSubScope)GetValue(SubScopeProperty); }
            set { SetValue(SubScopeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SubScope.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SubScopeProperty =
            DependencyProperty.Register("SubScope", typeof(TECSubScope), typeof(SubScopeItemControl));



        public SubScopeItemControl()
        {
            InitializeComponent();
        }
    }
}
