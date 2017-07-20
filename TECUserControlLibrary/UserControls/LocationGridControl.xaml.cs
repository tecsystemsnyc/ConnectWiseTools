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
using TECUserControlLibrary.ViewModels;

namespace TECUserControlLibrary.UserControls
{
    /// <summary>
    /// Interaction logic for LocationGridControl.xaml
    /// </summary>
    public partial class LocationGridControl : UserControl
    {

        #region DPs

        /// <summary>
        /// Gets or sets the DevicesSource which is displayed
        /// </summary>
        public ObservableCollection<TECLabeled> LocationSource
        {
            get { return (ObservableCollection<TECLabeled>)GetValue(LocationSourceProperty); }
            set { SetValue(LocationSourceProperty, value); }
        }

        /// <summary>
        /// Identified the DevicesSource dependency property
        /// </summary>
        public static readonly DependencyProperty LocationSourceProperty =
            DependencyProperty.Register("LocationSource", typeof(ObservableCollection<TECLabeled>),
              typeof(LocationGridControl), new PropertyMetadata(default(ObservableCollection<TECLabeled>)));


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
              typeof(LocationGridControl));
        #endregion

        public LocationGridControl()
        {
            InitializeComponent();
        }
    }
}
