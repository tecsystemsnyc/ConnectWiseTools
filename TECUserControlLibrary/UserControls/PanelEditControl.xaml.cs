using System.Windows;
using System.Windows.Controls;
using TECUserControlLibrary.ViewModels;

namespace TECUserControlLibrary.UserControls
{
    /// <summary>
    /// Interaction logic for PanelEditControl.xaml
    /// </summary>
    public partial class PanelEditControl : UserControl
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
              typeof(PanelEditControl));

        public PanelEditControl()
        {
            InitializeComponent();
        }
    }
}
