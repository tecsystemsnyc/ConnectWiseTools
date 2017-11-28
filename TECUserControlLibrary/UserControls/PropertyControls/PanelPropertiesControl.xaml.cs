using EstimatingLibrary;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace TECUserControlLibrary.UserControls.PropertyControls
{
    /// <summary>
    /// Interaction logic for PanelPropertiesControl.xaml
    /// </summary>
    public partial class PanelPropertiesControl : UserControl
    {

        public TECPanel Selected
        {
            get { return (TECPanel)GetValue(SelectedProperty); }
            set { SetValue(SelectedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Selected.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedProperty =
            DependencyProperty.Register("Selected", typeof(TECPanel), typeof(PanelPropertiesControl));
    
        public IEnumerable<TECPanelType> TypeSource
        {
            get { return (IEnumerable<TECPanelType>)GetValue(TypeSourceProperty); }
            set { SetValue(TypeSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TypeSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TypeSourceProperty =
            DependencyProperty.Register("TypeSource", typeof(IEnumerable<TECPanelType>), typeof(PanelPropertiesControl));


        public PanelPropertiesControl()
        {
            InitializeComponent();
        }
    }
}
