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
using EstimatingUtilitiesLibrary;
using EstimatingLibrary;

namespace TECUserControlLibrary
{
    /// <summary>
    /// Interaction logic for EditTabControl.xaml
    /// </summary>
    public partial class EditTabControl : UserControl
    {
        #region DPs

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
              typeof(EditTabControl));

        /// <summary>
        /// Gets or sets the tab index which is displayed
        /// </summary>
        public EditIndex Index
        {
            get { return (EditIndex)GetValue(IndexProperty); }
            set { SetValue(IndexProperty, value); }
        }

        /// <summary>
        /// Identified the DevicesSource dependency property
        /// </summary>
        public static readonly DependencyProperty IndexProperty =
            DependencyProperty.Register("Index", typeof(EditIndex),
              typeof(EditTabControl), new PropertyMetadata(default(EditIndex)));
        /*
        /// <summary>
        /// Gets or sets the tab system which is being edited
        /// </summary>
        public TECEquipment EditingEquipment
        {
            get { return (TECEquipment)GetValue(EditingEquipmentProperty); }
            set { SetValue(EditingEquipmentProperty, value); }
        }

        /// <summary>
        /// Identified the DevicesSource dependency property
        /// </summary>
        public static readonly DependencyProperty EditingEquipmentProperty =
            DependencyProperty.Register("EditingEquipment", typeof(TECEquipment),
              typeof(EditTabControl), new PropertyMetadata(default(TECEquipment)));

        /// <summary>
        /// Gets or sets the tab system which is being edited
        /// </summary>
        public TECSubScope EditingSubScope
        {
            get { return (TECSubScope)GetValue(EditingSubScopeProperty); }
            set { SetValue(EditingSubScopeProperty, value); }
        }

        /// <summary>
        /// Identified the DevicesSource dependency property
        /// </summary>
        public static readonly DependencyProperty EditingSubScopeProperty =
            DependencyProperty.Register("EditingSubScope", typeof(TECSubScope),
              typeof(EditTabControl), new PropertyMetadata(default(TECSubScope)));

        /// <summary>
        /// Gets or sets the tab system which is being edited
        /// </summary>
        public TECPoint EditingPoint
        {
            get { return (TECPoint)GetValue(EditingPointProperty); }
            set { SetValue(EditingPointProperty, value); }
        }

        /// <summary>
        /// Identified the DevicesSource dependency property
        /// </summary>
        public static readonly DependencyProperty EditingPointProperty =
            DependencyProperty.Register("EditingPoint", typeof(TECPoint),
              typeof(EditTabControl), new PropertyMetadata(default(TECPoint)));

        /// <summary>
        /// Gets or sets the tab system which is being edited
        /// </summary>
        public TECDevice EditingDevice
        {
            get { return (TECDevice)GetValue(EditingDeviceProperty); }
            set { SetValue(EditingDeviceProperty, value); }
        }

        /// <summary>
        /// Identified the DevicesSource dependency property
        /// </summary>
        public static readonly DependencyProperty EditingDeviceProperty =
            DependencyProperty.Register("EditingDevice", typeof(TECDevice),
              typeof(EditTabControl), new PropertyMetadata(default(TECDevice)));
        */
        #endregion
        public EditTabControl()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception e)
            {
                string message = "Edit Tab Control Initalization Failed: " + e.Message;
                Console.WriteLine(message);
                throw new Exception(message);
            }
        }
    }
}
