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
    /// Interaction logic for EquipmentControl.xaml
    /// </summary>
    public partial class EquipmentControl : BaseItemControl
    {
        
        public TECEquipment Equipment
        {
            get { return (TECEquipment)GetValue(EquipmentProperty); }
            set { SetValue(EquipmentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Equipment.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EquipmentProperty =
            DependencyProperty.Register("Equipment", typeof(TECEquipment), typeof(EquipmentControl));
        
        public EquipmentControl()
        {
            InitializeComponent();
        }
    }
}
