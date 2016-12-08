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
    /// Interaction logic for SystemEditControl.xaml
    /// </summary>
    public partial class SystemEditControl : UserControl
    {
        #region DPs
        /*
        /// <summary>
        /// Gets or sets the DevicesSource which is displayed
        /// </summary>
        public TECSystem System
        {
            get { return (TECSystem)GetValue(SystemProperty); }
            set { SetValue(SystemProperty, value); }
        }

        /// <summary>
        /// Identified the DevicesSource dependency property
        /// </summary>
        public static readonly DependencyProperty SystemProperty =
            DependencyProperty.Register("System", typeof(TECSystem),
              typeof(SystemEditControl), new PropertyMetadata(default(TECSystem)));
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
              typeof(SystemEditControl));


        #endregion
        public SystemEditControl()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception e)
            {
                string message = "Systems Edit Control Initalization Failed: " + e.Message;
                Console.WriteLine(message);
                throw new Exception(message);
            }
        }
    }
}
