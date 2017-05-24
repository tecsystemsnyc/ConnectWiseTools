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

namespace TECUserControlLibrary.DataGrids
{
    /// <summary>
    /// Interaction logic for ProposalScopeGridControl.xaml
    /// </summary>
    public partial class ProposalScopeGridControl : UserControl
    {

        public ObservableCollection<TECProposalScope> ProposalScopeSource
        {
            get { return (ObservableCollection<TECProposalScope>)GetValue(ProposalScopeSourceProperty); }
            set { SetValue(ProposalScopeSourceProperty, value); }
        }


        public static readonly DependencyProperty ProposalScopeSourceProperty =
            DependencyProperty.Register("ProposalScopeSource", typeof(ObservableCollection<TECProposalScope>),
              typeof(ProposalScopeGridControl), new PropertyMetadata(default(ObservableCollection<TECProposalScope>)));


        public Object ViewModel
        {
            get { return (Object)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }


        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(Object),
              typeof(ProposalScopeGridControl));

        public ProposalScopeGridControl()
        {
            InitializeComponent();
        }
    }
}
