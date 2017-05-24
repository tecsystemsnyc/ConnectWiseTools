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
    /// Interaction logic for PointEditControl.xaml
    /// </summary>
    public partial class PointEditControl : UserControl
    {

        #region DPs
        /*
        /// <summary>
        /// Gets or sets the DevicesSource which is displayed
        /// </summary>
        public TECPoint Point
        {
            get { return (TECPoint)GetValue(PointProperty); }
            set { SetValue(PointProperty, value); }
        }

        /// <summary>
        /// Identified the DevicesSource dependency property
        /// </summary>
        public static readonly DependencyProperty PointProperty =
            DependencyProperty.Register("Point", typeof(TECPoint),
              typeof(PointEditControl), new PropertyMetadata(default(TECPoint)));
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
              typeof(PointEditControl));


        #endregion

        public PointEditControl()
        {
            InitializeComponent();
        }
    }
}
