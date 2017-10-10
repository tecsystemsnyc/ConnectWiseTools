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
using EstimatingLibrary;

namespace TECUserControlLibrary.UserControls.ItemControls
{
    /// <summary>
    /// Interaction logic for SystemControl.xaml
    /// </summary>
    public partial class SystemControl : UserControl
    {


        public TECSystem ControlSystem
        {
            get { return (TECSystem)GetValue(SystemProperty); }
            set { SetValue(SystemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for System.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SystemProperty =
            DependencyProperty.Register("ControlSystem", typeof(TECSystem), typeof(SystemControl));
        
        public SystemControl()
        {
            InitializeComponent();
        }
    }
}
