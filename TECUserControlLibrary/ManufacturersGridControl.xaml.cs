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

namespace TECUserControlLibrary
{
    /// <summary>
    /// Interaction logic for ManufacturersGridControl.xaml
    /// </summary>
    public partial class ManufacturersGridControl : UserControl
    {
        #region DPs

        /// <summary>
        /// Gets or sets the DevicesSource which is displayed
        /// </summary>
        public ObservableCollection<TECManufacturer> ManufacturersSource
        {
            get { return (ObservableCollection<TECManufacturer>)GetValue(ManufacturersSourceProperty); }
            set { SetValue(ManufacturersSourceProperty, value); }
        }

        /// <summary>
        /// Identified the DevicesSource dependency property
        /// </summary>
        public static readonly DependencyProperty ManufacturersSourceProperty =
            DependencyProperty.Register("ManufacturersSource", typeof(ObservableCollection<TECManufacturer>),
              typeof(ManufacturersGridControl), new PropertyMetadata(default(ObservableCollection<TECManufacturer>)));


        #endregion

        public ManufacturersGridControl()
        {
            InitializeComponent();
        }
    }
}
