using EstimatingLibrary;
using System.Windows;

namespace TECUserControlLibrary.UserControls.ItemControls
{
    /// <summary>
    /// Interaction logic for ControllerItemControl.xaml
    /// </summary>
    public partial class ControllerItemControl : BaseItemControl
    {


        public TECController Controller
        {
            get { return (TECController)GetValue(ControllerProperty); }
            set { SetValue(ControllerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Controller.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ControllerProperty =
            DependencyProperty.Register("Controller", typeof(TECController), typeof(ControllerItemControl));

        public ControllerItemControl()
        {
            InitializeComponent();
        }
    }
}
