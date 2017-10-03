using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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

        public object Selected
        {
            get { return (object)GetValue(SelectedProperty); }
            set { SetValue(SelectedProperty, value); }
        }

        public static readonly DependencyProperty SelectedProperty =
            DependencyProperty.Register("Selected", typeof(object),
                typeof(MaterialView), new FrameworkPropertyMetadata(null)
                {
                    BindsTwoWayByDefault = true,
                    DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                });

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
