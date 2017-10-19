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
using TECUserControlLibrary.Models;
using TECUserControlLibrary.ViewModels;

namespace TECUserControlLibrary.UserControls.ListControls
{
    /// <summary>
    /// Interaction logic for ControllerInPanelListControl.xaml
    /// </summary>
    public partial class ControllerInPanelListControl : BaseListControl<ControllerInPanel>
    {

        public ControllersPanelsVM ViewModel
        {
            get { return (ControllersPanelsVM)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(ControllersPanelsVM), typeof(ControllerInPanelListControl));


        public ControllerInPanelListControl()
        {
            InitializeComponent();
        }
    }
}
