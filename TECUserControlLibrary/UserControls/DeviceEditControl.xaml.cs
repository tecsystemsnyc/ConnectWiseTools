using System.Windows;
using System.Windows.Controls;
using TECUserControlLibrary.ViewModels;

namespace TECUserControlLibrary.UserControls
{
    /// <summary>
    /// Interaction logic for DeviceEditControl.xaml
    /// </summary>
    public partial class DeviceEditControl : UserControl
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
              typeof(DeviceEditControl));

        public DeviceEditControl()
        {
            InitializeComponent();
        }
    }
}
