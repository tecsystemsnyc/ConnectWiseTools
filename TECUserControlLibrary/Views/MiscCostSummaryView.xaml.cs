using System.Windows;
using System.Windows.Controls;
using TECUserControlLibrary.ViewModels;

namespace TECUserControlLibrary.Views
{
    /// <summary>
    /// Interaction logic for MiscCostSummaryView.xaml
    /// </summary>
    public partial class MiscCostSummaryView : UserControl
    {
        public MiscCostSummaryView()
        {
            InitializeComponent();
        }

        public MiscCostsSummaryVM ViewModel
        {
            get { return (MiscCostsSummaryVM)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(MiscCostsSummaryVM), typeof(MiscCostSummaryView));

    }
}
