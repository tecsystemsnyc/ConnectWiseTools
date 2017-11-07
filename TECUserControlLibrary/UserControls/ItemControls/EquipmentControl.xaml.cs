using EstimatingLibrary;
using System.Windows;

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
