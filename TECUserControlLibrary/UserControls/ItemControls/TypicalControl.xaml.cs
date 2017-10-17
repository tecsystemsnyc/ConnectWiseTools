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
    /// Interaction logic for TypicalControl.xaml
    /// </summary>
    public partial class TypicalControl : BaseItemControl
    {

        public TECTypical Typical
        {
            get { return (TECTypical)GetValue(TypicalProperty); }
            set { SetValue(TypicalProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Typical.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TypicalProperty =
            DependencyProperty.Register("Typical", typeof(TECTypical), typeof(TypicalControl));


        public TypicalControl()
        {
            InitializeComponent();
        }
    }
}
