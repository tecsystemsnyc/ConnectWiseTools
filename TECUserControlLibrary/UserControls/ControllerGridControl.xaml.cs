using EstimatingLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace TECUserControlLibrary.UserControls
{
    /// <summary>
    /// Interaction logic for ControllerGridControl.xaml
    /// </summary>
    public partial class ControllerGridControl : UserControl
    {
        #region DPs

        /// <summary>
        /// Gets or sets the DevicesSource which is displayed
        /// </summary>
        public ObservableCollection<TECController> ControllersSource
        {
            get { return (ObservableCollection<TECController>)GetValue(ControllersSourceProperty); }
            set { SetValue(ControllersSourceProperty, value); }
        }

        /// <summary>
        /// Identified the DevicesSource dependency property
        /// </summary>
        public static readonly DependencyProperty ControllersSourceProperty =
            DependencyProperty.Register("ControllersSource", typeof(ObservableCollection<TECController>),
              typeof(ControllerGridControl), new PropertyMetadata(default(ObservableCollection<TECController>)));
        #endregion
        public ControllerGridControl()
        {
            InitializeComponent();
        }
    }
}
