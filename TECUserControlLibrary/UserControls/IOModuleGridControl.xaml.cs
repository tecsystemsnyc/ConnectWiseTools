using EstimatingLibrary;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace TECUserControlLibrary.UserControls
{
    /// <summary>
    /// Interaction logic for IOModuleGridControl.xaml
    /// </summary>
    public partial class IOModuleGridControl : UserControl
    {
        /// <summary>
        /// Gets or sets the DevicesSource which is displayed
        /// </summary>
        public ObservableCollection<TECIOModule> IOModuleSource
        {
            get { return (ObservableCollection<TECIOModule>)GetValue(IOModuleSourceProperty); }
            set { SetValue(IOModuleSourceProperty, value); }
        }

        /// <summary>
        /// Identified the DevicesSource dependency property
        /// </summary>
        public static readonly DependencyProperty IOModuleSourceProperty =
            DependencyProperty.Register("IOModuleSource", typeof(ObservableCollection<TECIOModule>),
              typeof(IOModuleGridControl), new PropertyMetadata(default(ObservableCollection<TECIOModule>)));

        public IOModuleGridControl()
        {
            InitializeComponent();
        }
    }
}
