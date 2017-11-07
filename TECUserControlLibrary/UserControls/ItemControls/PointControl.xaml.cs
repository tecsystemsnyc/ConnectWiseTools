using EstimatingLibrary;
using System.Windows;

namespace TECUserControlLibrary.UserControls.ItemControls
{
    /// <summary>
    /// Interaction logic for PointControl.xaml
    /// </summary>
    public partial class PointControl : BaseItemControl
    {


        public TECPoint Point
        {
            get { return (TECPoint)GetValue(PointProperty); }
            set { SetValue(PointProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Point.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PointProperty =
            DependencyProperty.Register("Point", typeof(TECPoint), typeof(PointControl));


        public PointControl()
        {
            InitializeComponent();
        }
    }
}
