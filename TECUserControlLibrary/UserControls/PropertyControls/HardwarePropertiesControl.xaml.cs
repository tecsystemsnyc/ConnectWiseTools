using EstimatingLibrary;
using GongSolutions.Wpf.DragDrop;
using System.Windows;
using System.Windows.Controls;

namespace TECUserControlLibrary.UserControls.PropertyControls
{
    /// <summary>
    /// Interaction logic for HardwarePropertiesControl.xaml
    /// </summary>
    public partial class HardwarePropertiesControl : UserControl
    {
        
        public TECHardware Selected
        {
            get { return (TECHardware)GetValue(SelectedProperty); }
            set { SetValue(SelectedProperty, value); }
        }

        public static readonly DependencyProperty SelectedProperty =
            DependencyProperty.Register("Selected", typeof(TECHardware),
              typeof(HardwarePropertiesControl));

        public bool ReadOnly
        {
            get { return (bool)GetValue(ReadOnlyProperty); }
            set { SetValue(ReadOnlyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ReadOnly.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ReadOnlyProperty =
            DependencyProperty.Register("ReadOnly", typeof(bool), typeof(HardwarePropertiesControl), new PropertyMetadata(false));

        public IDropTarget DropHandler
        {
            get { return (IDropTarget)GetValue(DropHandlerProperty); }
            set { SetValue(DropHandlerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DropHandler.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DropHandlerProperty =
            DependencyProperty.Register("DropHandler", typeof(IDropTarget), typeof(HardwarePropertiesControl));

        public HardwarePropertiesControl()
        {
            InitializeComponent();
        }
    }
}
