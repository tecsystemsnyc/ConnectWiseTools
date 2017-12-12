using EstimatingLibrary;
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
    /// Interaction logic for SubScopePropertiesControl.xaml
    /// </summary>
    public partial class SubScopePropertiesControl : UserControl
    {

        public TECSubScope Selected
        {
            get { return (TECSubScope)GetValue(SelectedProperty); }
            set { SetValue(SelectedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Selected.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedProperty =
            DependencyProperty.Register("Selected", typeof(TECSubScope), typeof(SubScopePropertiesControl));


        public SubScopePropertiesControl()
        {
            InitializeComponent();
        }
    }
}
