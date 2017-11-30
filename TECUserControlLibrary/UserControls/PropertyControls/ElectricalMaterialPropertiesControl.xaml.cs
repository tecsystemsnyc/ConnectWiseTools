using EstimatingLibrary;
using GongSolutions.Wpf.DragDrop;
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

namespace TECUserControlLibrary.UserControls.PropertyControls
{
    /// <summary>
    /// Interaction logic for ElectricalMaterialPropertiesControl.xaml
    /// </summary>
    public partial class ElectricalMaterialPropertiesControl : UserControl
    {

        public TECElectricalMaterial Selected
        {
            get { return (TECElectricalMaterial)GetValue(SelectedProperty); }
            set { SetValue(SelectedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Selected.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedProperty =
            DependencyProperty.Register("Selected", typeof(TECElectricalMaterial), typeof(ElectricalMaterialPropertiesControl));


        public IDropTarget DropHandler
        {
            get { return (IDropTarget)GetValue(DropHandlerProperty); }
            set { SetValue(DropHandlerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DropHandler.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DropHandlerProperty =
            DependencyProperty.Register("DropHandler", typeof(IDropTarget), typeof(ElectricalMaterialPropertiesControl));



        public ElectricalMaterialPropertiesControl()
        {
            InitializeComponent();
        }
    }
}
