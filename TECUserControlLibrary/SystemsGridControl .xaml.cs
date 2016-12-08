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

namespace TECUserControlLibrary
{
    /// <summary>
    /// Interaction logic for SystemsGridControl.xaml
    /// </summary>
    public partial class SystemsGridControl : UserControl
    {
        #region DPs

        /// <summary>
        /// Gets or sets the SystemSource which is displayed
        /// </summary>
        public ObservableCollection<TECSystem> SystemsSource
        {
            get { return (ObservableCollection<TECSystem>)GetValue(SystemsSourceProperty); }
            set { SetValue(SystemsSourceProperty, value); }
        }

        /// <summary>
        /// Identified the SystemSource dependency property
        /// </summary>
        public static readonly DependencyProperty SystemsSourceProperty =
            DependencyProperty.Register("SystemsSource", typeof(ObservableCollection<TECSystem>),
              typeof(SystemsGridControl), new PropertyMetadata(default(ObservableCollection<TECSystem>)));

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
              typeof(SystemsGridControl));


        /// <summary>
        /// Gets or sets wether user can add rows 
        /// </summary>
        public bool AllowAddingNew
        {
            get { return (bool)GetValue(AllowAddingNewProperty); }
            set { SetValue(AllowAddingNewProperty, value); }
        }

        /// <summary>
        /// Identified the AllowAddingNew dependency property
        /// </summary>
        public static readonly DependencyProperty AllowAddingNewProperty =
            DependencyProperty.Register("AllowAddingNew", typeof(bool),
              typeof(SystemsGridControl), new PropertyMetadata(true));


        #endregion

        public SystemsGridControl()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception e)
            {
                string message = "Systems Grid Control Initalization Failed: " + e.Message;
                Console.WriteLine(message);
                throw new Exception(message);
            }
        }
    }
}
