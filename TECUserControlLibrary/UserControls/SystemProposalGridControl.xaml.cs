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
    /// Interaction logic for SystemProposalGridControl.xaml
    /// </summary>
    public partial class SystemProposalGridControl : UserControl
    {
        public ObservableCollection<TECTypical> SystemsSource
        {
            get { return (ObservableCollection<TECTypical>)GetValue(SystemsSourceProperty); }
            set { SetValue(SystemsSourceProperty, value); }
        }

        /// <summary>
        /// Identified the SystemSource dependency property
        /// </summary>
        public static readonly DependencyProperty SystemsSourceProperty =
            DependencyProperty.Register("SystemsSource", typeof(ObservableCollection<TECTypical>),
              typeof(SystemProposalGridControl), new PropertyMetadata(default(ObservableCollection<TECTypical>)));

        public SystemProposalGridControl()
        {
            InitializeComponent();
        }
    }
}
