using EstimatingLibrary;
using System.Windows;
using System.Windows.Controls;

namespace TECUserControlLibrary.Views
{
    /// <summary>
    /// Interaction logic for PropertiesView.xaml
    /// </summary>
    public partial class PropertiesView : UserControl
    {
        
        public TECObject Selected
        {
            get { return (TECObject)GetValue(SelectedProperty); }
            set { SetValue(SelectedProperty, value); }
        }

        /// <summary>
        /// Identified the ViewModel dependency property
        /// </summary>
        public static readonly DependencyProperty SelectedProperty  =
            DependencyProperty.Register("Selected", typeof(TECObject),
              typeof(PropertiesView));

        public PropertiesView()
        {
            InitializeComponent();
        }
    }
}
