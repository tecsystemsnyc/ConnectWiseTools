using EstimatingLibrary;
using System.Windows;

namespace TECUserControlLibrary.UserControls.ItemControls
{
    /// <summary>
    /// Interaction logic for ElectricalMaterialControl.xaml
    /// </summary>
    public partial class ElectricalMaterialControl : BaseItemControl
    {

        public TECElectricalMaterial ElectricalMaterial
        {
            get { return (TECElectricalMaterial)GetValue(ElectricalMaterialProperty); }
            set { SetValue(ElectricalMaterialProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ElectricalMaterial.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ElectricalMaterialProperty =
            DependencyProperty.Register("ElectricalMaterial", typeof(TECElectricalMaterial), typeof(ElectricalMaterialControl));


        public ElectricalMaterialControl()
        {
            InitializeComponent();
        }
    }
}
