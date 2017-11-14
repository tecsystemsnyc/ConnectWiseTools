using EstimatingLibrary.Interfaces;
using System.Windows;
using System.Windows.Controls;

namespace TECUserControlLibrary.UserControls.PropertyControls
{
    /// <summary>
    /// Interaction logic for PointPropetiesControl.xaml
    /// </summary>
    public partial class PointPropetiesControl : UserControl
    {
        public INotifyPointChanged Selected
        {
            get { return (INotifyPointChanged)GetValue(SelectedProperty); }
            set { SetValue(SelectedProperty, value); }
        }

        public static readonly DependencyProperty SelectedProperty =
            DependencyProperty.Register("Selected", typeof(INotifyPointChanged),
              typeof(PointPropetiesControl));

        public PointPropetiesControl()
        {
            InitializeComponent();
        }
    }
}
