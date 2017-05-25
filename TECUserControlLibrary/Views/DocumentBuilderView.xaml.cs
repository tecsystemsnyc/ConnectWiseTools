using EstimatingUtilitiesLibrary;
using System.Windows;
using System.Windows.Controls;
using TECUserControlLibrary.ViewModels;

namespace TECUserControlLibrary.Views
{
    /// <summary>
    /// Interaction logic for DocumentBuilderControl.xaml
    /// </summary>
    public partial class DocumentBuilderView : UserControl
    {

        public DocumentBuilderVM ViewModel
        {
            get { return (DocumentBuilderVM)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(DocumentBuilderVM),
              typeof(DocumentBuilderView));

        public ProposalIndex SelectedProposalIndex
        {
            get { return (ProposalIndex)GetValue(SelectedProposalIndexProperty); }
            set { SetValue(SelectedProposalIndexProperty, value); }
        }
        public static readonly DependencyProperty SelectedProposalIndexProperty =
            DependencyProperty.Register("SelectedProposalIndex", typeof(ProposalIndex),
              typeof(DocumentBuilderView), new PropertyMetadata(default(ProposalIndex)));

        public DocumentBuilderView()
        {
            InitializeComponent();
        }
    }
}
