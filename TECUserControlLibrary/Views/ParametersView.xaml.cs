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

namespace TECUserControlLibrary.Views
{
    /// <summary>
    /// Interaction logic for ParamatersView.xaml
    /// </summary>
    public partial class ParametersView : UserControl
    {

        public IEnumerable<TECParameters> Source
        {
            get { return (IEnumerable<TECParameters>)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Source.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(IEnumerable<TECParameters>), typeof(ParametersView));


        public ParametersView()
        {
            InitializeComponent();
        }
    }
}
