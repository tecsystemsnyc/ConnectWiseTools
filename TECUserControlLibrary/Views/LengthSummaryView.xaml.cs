using System.Windows;
using System.Windows.Controls;
using TECUserControlLibrary.ViewModels;

namespace TECUserControlLibrary.Views
{
    /// <summary>
    /// Interaction logic for LengthSummaryView.xaml
    /// </summary>
    public partial class LengthSummaryView : UserControl
    {
        public LengthSummaryView()
        {
            InitializeComponent();
        }

        public LengthSummaryVM ViewModel
        {
            get { return (LengthSummaryVM)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(LengthSummaryVM), typeof(LengthSummaryView));



        public string LengthItemType
        {
            get { return (string)GetValue(LengthItemTypeProperty); }
            set { SetValue(LengthItemTypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LengthItemType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LengthItemTypeProperty =
            DependencyProperty.Register("LengthItemType", typeof(string), typeof(LengthSummaryView), new PropertyMetadata(""));


    }
}
