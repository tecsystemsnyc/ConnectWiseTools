using System.Windows;
using System.Windows.Controls;
using TECUserControlLibrary.Utilities;
using TECUserControlLibrary.ViewModels;

namespace TECUserControlLibrary.Views
{
    /// <summary>
    /// Interaction logic for MaterialView.xaml
    /// </summary>
    public partial class MaterialView : UserControl
    {
        /// <summary>
        /// Gets or sets the SelectedScopeType which is displayed
        /// </summary>
        public MaterialType SelectedMaterialType
        {
            get { return (MaterialType)GetValue(SelectedMaterialTypeProperty); }
            set { SetValue(SelectedMaterialTypeProperty, value); }
        }

        /// <summary>
        /// Identified the SelectedScopeType dependency property
        /// </summary>
        public static readonly DependencyProperty SelectedMaterialTypeProperty =
            DependencyProperty.Register("SelectedMaterialType", typeof(MaterialType),
              typeof(MaterialView), new PropertyMetadata(default(MaterialType)));

        public MaterialVM ViewModel
        {
            get { return (MaterialVM)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(MaterialVM),
                typeof(MaterialView));

        public MaterialView()
        {
            InitializeComponent();
        }
    }
}
