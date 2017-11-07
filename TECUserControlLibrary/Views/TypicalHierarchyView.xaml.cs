using System.Windows;
using System.Windows.Controls;
using TECUserControlLibrary.ViewModels;

namespace TECUserControlLibrary.Views
{
    /// <summary>
    /// Interaction logic for TypicalHierarchyView.xaml
    /// </summary>
    public partial class TypicalHierarchyView : UserControl
    {

        public TypicalHierarchyVM ViewModel
        {
            get { return (TypicalHierarchyVM)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(TypicalHierarchyVM), typeof(TypicalHierarchyView), new PropertyMetadata(default(TypicalHierarchyVM)));


        public TypicalHierarchyView()
        {
            InitializeComponent();
        }

    }
}
