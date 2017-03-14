using System;
using System.Collections.Generic;
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
using EstimatingLibrary;
using System.Collections.ObjectModel;

namespace TECUserControlLibrary.DataGrids
{

    /// <summary>
    /// Interaction logic for DevicesGridControl.xaml
    /// </summary>
    public partial class DevicesGridControl : UserControl
    {

        #region DPs

        /// <summary>
        /// Gets or sets the DevicesSource which is displayed
        /// </summary>
        public ObservableCollection<TECDevice> DevicesSource
        {
            get { return (ObservableCollection<TECDevice>)GetValue(DevicesSourceProperty); }
            set { SetValue(DevicesSourceProperty, value); }
        }

        /// <summary>
        /// Identified the DevicesSource dependency property
        /// </summary>
        public static readonly DependencyProperty DevicesSourceProperty =
            DependencyProperty.Register("DevicesSource", typeof(ObservableCollection<TECDevice>),
              typeof(DevicesGridControl), new PropertyMetadata(default(ObservableCollection<TECDevice>)));


        /// <summary>
        /// Gets or sets the ViewModel which is used
        /// </summary>
        public Object ViewModel
        {
            get { return (Object)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// Identified the ViewModel dependency property
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(Object),
              typeof(DevicesGridControl));
        #endregion

        public DevicesGridControl()
        {
            InitializeComponent();
        }
    }
}
