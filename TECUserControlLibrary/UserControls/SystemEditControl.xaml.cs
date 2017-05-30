using EstimatingLibrary;
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

namespace TECUserControlLibrary.UserControls
{
    /// <summary>
    /// Interaction logic for SystemEditControl.xaml
    /// </summary>
    public partial class SystemEditControl : UserControl
    {

        /// <summary>
        /// Gets or sets the ViewModel which is used
        /// </summary>
        public EditTabVM ViewModel
        {
            get { return (EditTabVM)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// Identified the ViewModel dependency property
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(EditTabVM),
              typeof(SystemEditControl));

        public SystemEditControl()
        {
            InitializeComponent();
        }
    }
}
