using System;
using System.Collections.Generic;
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
using TECUserControlLibrary.ViewModels;

namespace TECUserControlLibrary.Views
{
    /// <summary>
    /// Interaction logic for ControlledScopeView.xaml
    /// </summary>
    public partial class ControlledScopeView : UserControl
    {
        public ControlledScopeVM ViewModel
        {
            get { return (ControlledScopeVM)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(ControlledScopeVM),
              typeof(ControlledScopeView));

        public ControlledScopeView()
        {
            InitializeComponent();
        }
    }
}
