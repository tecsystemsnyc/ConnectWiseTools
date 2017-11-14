using System.Windows;
using System.Windows.Controls;
using TECUserControlLibrary.ViewModels;

namespace TECUserControlLibrary.Views
{
    /// <summary>
    /// Interaction logic for HardwareSummaryView.xaml
    /// </summary>
    public partial class HardwareSummaryView : UserControl
    {
        public HardwareSummaryView()
        {
            InitializeComponent();
        }



        public HardwareSummaryVM ViewModel
        {
            get { return (HardwareSummaryVM)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(HardwareSummaryVM), typeof(HardwareSummaryView));



        public string HardwareItemType
        {
            get { return (string)GetValue(HardwareItemTypeProperty); }
            set { SetValue(HardwareItemTypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HardwareItemsHeading.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HardwareItemTypeProperty =
            DependencyProperty.Register("HardwareItemType", typeof(string), typeof(HardwareSummaryView), new PropertyMetadata(""));
    }
}
