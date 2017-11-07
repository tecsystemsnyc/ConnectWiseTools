using System.Windows;
using System.Windows.Controls;
using TECUserControlLibrary.ViewModels;

namespace TECUserControlLibrary.Views
{
    /// <summary>
    /// Interaction logic for ControllersPanelsView.xaml
    /// </summary>
    public partial class ControllersPanelsView : UserControl
    {
        public ControllersPanelsVM ViewModel
        {
            get { return (ControllersPanelsVM)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(ControllersPanelsVM),
              typeof(ControllersPanelsView));

        public ControllersPanelsView()
        {
            InitializeComponent();
        }
    }
}
