using EstimatingLibrary;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

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
