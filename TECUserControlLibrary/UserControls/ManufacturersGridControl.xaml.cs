using EstimatingLibrary;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace TECUserControlLibrary.UserControls
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


        public TECManufacturer SelectedItem
        {
            get { return (TECManufacturer)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(TECManufacturer), typeof(ManufacturersGridControl));
        
        #endregion

        public ManufacturersGridControl()
        {
            InitializeComponent();
        }
    }
}
