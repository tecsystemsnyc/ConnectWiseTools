using System.Windows;
using System.Windows.Controls;
using TECUserControlLibrary.Utilities;
using TECUserControlLibrary.ViewModels;

namespace TECUserControlLibrary.Views
{
    /// <summary>
    /// Interaction logic for DocumentBuilderControl.xaml
    /// </summary>
    public partial class ProposalView : UserControl
    {

        public ProposalVM ViewModel
        {
            get { return (ProposalVM)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(ProposalVM),
              typeof(ProposalView));

        public ProposalIndex SelectedProposalIndex
        {
            get { return (ProposalIndex)GetValue(SelectedProposalIndexProperty); }
            set { SetValue(SelectedProposalIndexProperty, value); }
        }
        public static readonly DependencyProperty SelectedProposalIndexProperty =
            DependencyProperty.Register("SelectedProposalIndex", typeof(ProposalIndex),
              typeof(ProposalView), new PropertyMetadata(default(ProposalIndex)));

        public ProposalView()
        {
            InitializeComponent();
        }
    }
}
