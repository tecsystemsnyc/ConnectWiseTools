using ConnectWiseInformationInterface.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace ConnectWiseInformationInterface.Controls
{
    /// <summary>
    /// Interaction logic for OppTypeListControl.xaml
    /// </summary>
    public partial class OppTypeListControl : UserControl
    {


        public IEnumerable<OppTypeBool> ItemsSource
        {
            get { return (IEnumerable<OppTypeBool>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemsSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable<OppTypeBool>), typeof(OppTypeListControl));



        public OppTypeListControl()
        {
            InitializeComponent();
        }
    }
}
