using System.Windows;
using System.Windows.Controls;
using TECUserControlLibrary.ViewModels;

namespace TECUserControlLibrary.Views
{
    /// <summary>
    /// Interaction logic for MaterialSummaryView.xaml
    /// </summary>
    public partial class MaterialSummaryView : UserControl
    {
        public MaterialSummaryView()
        {
            InitializeComponent();
        }

        

        public MaterialSummaryVM ViewModel
        {
            get { return (MaterialSummaryVM)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(MaterialSummaryVM), typeof(MaterialSummaryView));
    }
}
