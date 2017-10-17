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
using EstimatingLibrary;

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
