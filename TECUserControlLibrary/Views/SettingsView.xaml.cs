using System.Windows;
using System.Windows.Controls;
using TECUserControlLibrary.ViewModels;

namespace TECUserControlLibrary.Views
{
    public partial class SettingsView : UserControl
    {
        public SettingsVM ViewModel
        {
            get { return (SettingsVM)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(SettingsVM),
              typeof(SettingsView));

        public SettingsView()
        {
            InitializeComponent();
        }
    }
}
