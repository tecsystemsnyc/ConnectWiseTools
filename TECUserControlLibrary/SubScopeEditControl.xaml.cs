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
    /// Interaction logic for SubScopeEditControl.xaml
    /// </summary>
    public partial class SubScopeEditControl : UserControl
    {

        #region DPs
        /*
        /// <summary>
        /// Gets or sets the DevicesSource which is displayed
        /// </summary>
        public TECSubScope SubScope
        {
            get { return (TECSubScope)GetValue(SubScopeProperty); }
            set { SetValue(SubScopeProperty, value); }
        }

        /// <summary>
        /// Identified the DevicesSource dependency property
        /// </summary>
        public static readonly DependencyProperty SubScopeProperty =
            DependencyProperty.Register("SubScope", typeof(TECSubScope),
              typeof(SubScopeEditControl), new PropertyMetadata(default(TECSubScope)));
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
              typeof(SubScopeEditControl));


        #endregion

        public SubScopeEditControl()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception e)
            {
                string message = "SubScope Edit Control Initalization Failed: " + e.Message;
                Console.WriteLine(message);
                throw new Exception(message);
            }
        }
    }
}
