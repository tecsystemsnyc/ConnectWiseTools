using EstimatingLibrary;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace TECUserControlLibrary.UserControls
{
    /// <summary>
    /// Interaction logic for AddEquipmentControl.xaml
    /// </summary>
    public partial class AddEquipmentControl : UserControl
    {

        public TECEquipment ToAdd
        {
            get { return (TECEquipment)GetValue(ToAddProperty); }
            set { SetValue(ToAddProperty, value); }
        }

        public static readonly DependencyProperty ToAddProperty =
            DependencyProperty.Register("ToAdd", typeof(TECEquipment),
                typeof(AddEquipmentControl), new FrameworkPropertyMetadata(default(TECEquipment))
                {
                    BindsTwoWayByDefault = true,
                    DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                });

        public AddEquipmentControl()
        {
            InitializeComponent();
        }
    }
}
