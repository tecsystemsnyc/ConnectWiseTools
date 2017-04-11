using EstimatingLibrary;
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

namespace TECUserControlLibrary
{
    /// <summary>
    /// Interaction logic for EquipmentEditControl.xaml
    /// </summary>
    public partial class EquipmentEditControl : UserControl
    {

        #region DPs
        /*
        /// <summary>
        /// Gets or sets the DevicesSource which is displayed
        /// </summary>
        public TECEquipment Equipment
        {
            get { return (TECEquipment)GetValue(EquipmentProperty); }
            set { SetValue(EquipmentProperty, value); }
        }

        /// <summary>
        /// Identified the DevicesSource dependency property
        /// </summary>
        public static readonly DependencyProperty EquipmentProperty =
            DependencyProperty.Register("Equipment", typeof(TECEquipment),
              typeof(EquipmentEditControl), new PropertyMetadata(default(TECEquipment)));
        */
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
              typeof(EquipmentEditControl));


        #endregion

        public EquipmentEditControl()
        {
            InitializeComponent();
        }
    }
}
