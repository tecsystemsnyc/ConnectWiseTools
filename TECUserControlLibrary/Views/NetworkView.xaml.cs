using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using TECUserControlLibrary.UserControls.ListControls;
using TECUserControlLibrary.ViewModels;

namespace TECUserControlLibrary.Views
{
    /// <summary>
    /// Interaction logic for NetworkView.xaml
    /// </summary>
    public partial class NetworkView : UserControl
    {
        public NetworkVM ViewModel
        {
            get { return (NetworkVM)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(NetworkVM),
                typeof(NetworkView));
        
        
        public NetworkView()
        {
            InitializeComponent();
        }
    }
}
