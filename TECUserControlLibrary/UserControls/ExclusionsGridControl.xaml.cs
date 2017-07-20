using EstimatingLibrary;
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

namespace TECUserControlLibrary.UserControls
{
    /// <summary>
    /// Interaction logic for ExclusionsGridControl.xaml
    /// </summary>
    public partial class ExclusionsGridControl : UserControl
    {

        public ObservableCollection<TECLabeled> ExclusionsSource
        {
            get { return (ObservableCollection<TECLabeled>)GetValue(ExclusionsSourceProperty); }
            set { SetValue(ExclusionsSourceProperty, value); }
        }

        /// <summary>
        /// Identified the SystemSource dependency property
        /// </summary>
        public static readonly DependencyProperty ExclusionsSourceProperty =
            DependencyProperty.Register("ExclusionsSource", typeof(ObservableCollection<TECLabeled>),
              typeof(ExclusionsGridControl), new PropertyMetadata(default(ObservableCollection<TECLabeled>)));

        public ExclusionsGridControl()
        {
            InitializeComponent();
        }
    }
}
