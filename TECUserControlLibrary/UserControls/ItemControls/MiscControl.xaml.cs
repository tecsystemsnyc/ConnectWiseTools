using EstimatingLibrary;
using System.Windows;

namespace TECUserControlLibrary.UserControls.ItemControls
{
    /// <summary>
    /// Interaction logic for MiscControl.xaml
    /// </summary>
    public partial class MiscControl : BaseItemControl
    {
        public TECMisc Misc
        {
            get { return (TECMisc)GetValue(MiscProperty); }
            set { SetValue(MiscProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Misc.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MiscProperty =
            DependencyProperty.Register("Misc", typeof(TECMisc), typeof(MiscControl));


        public MiscControl()
        {
            InitializeComponent();
        }
    }
}
