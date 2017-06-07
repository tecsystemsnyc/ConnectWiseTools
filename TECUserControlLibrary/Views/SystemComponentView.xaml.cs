using System.Windows;
using System.Windows.Controls;
using TECUserControlLibrary.Utilities;
using TECUserControlLibrary.ViewModels;

namespace TECUserControlLibrary.Views
{
    /// <summary>
    /// Interaction logic for SystemComponentView.xaml
    /// </summary>
    public partial class SystemComponentView : UserControl
    {
        /// <summary>
        /// Gets or sets the ViewModel which is used
        /// </summary>
        public SystemComponentVM ViewModel
        {
            get { return (SystemComponentVM)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// Identified the ViewModel dependency property
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(SystemComponentVM),
              typeof(SystemComponentView));

        public ControlledScopeItemIndex SelectedItemIndex
        {
            get { return (ControlledScopeItemIndex)GetValue(SelectedItemIndexProperty); }
            set { SetValue(SelectedItemIndexProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemIndexProperty =
            DependencyProperty.Register("SelectedItemIndex", typeof(ControlledScopeItemIndex),
              typeof(SystemComponentView), new PropertyMetadata(default(ControlledScopeItemIndex)));

        public SystemComponentView()
        {
            InitializeComponent();
        }
    }
}
