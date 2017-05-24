using EstimatingUtilitiesLibrary;
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

namespace TECUserControlLibrary
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

        public Object ViewModel
        {
            get { return (Object)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(Object),
                typeof(MaterialView));

        public MaterialView()
        {
            InitializeComponent();
        }
    }
}
