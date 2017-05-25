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
using EstimatingUtilitiesLibrary;

namespace TECUserControlLibrary
{
    /// <summary>
    /// Interaction logic for ByLocationControl.xaml
    /// </summary>
    public partial class ByLocationControl : UserControl
    {

        #region DPs

        /// <summary>
        /// Gets or sets the LocationSource which is displayed
        /// </summary>
        public ObservableCollection<TECLocation> LocationSource
        {
            get { return (ObservableCollection<TECLocation>)GetValue(LocationSourceProperty); }
            set { SetValue(LocationSourceProperty, value); }
        }

        /// <summary>
        /// Identified the LocationSource dependency property
        /// </summary>
        public static readonly DependencyProperty LocationSourceProperty =
            DependencyProperty.Register("LocationSource", typeof(ObservableCollection<TECLocation>),
              typeof(ByLocationControl), new PropertyMetadata(default(ObservableCollection<TECLocation>)));

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
              typeof(ByLocationControl), new PropertyMetadata(default(ObservableCollection<TECSystem>)));


        /// <summary>
        /// Gets or sets the EquipmentSource which is displayed
        /// </summary>
        public ObservableCollection<TECEquipment> EquipmentSource
        {
            get { return (ObservableCollection<TECEquipment>)GetValue(EquipmentSourceProperty); }
            set { SetValue(EquipmentSourceProperty, value); }
        }

        /// <summary>
        /// Identified the EquipmentSource dependency property
        /// </summary>
        public static readonly DependencyProperty EquipmentSourceProperty =
            DependencyProperty.Register("EquipmentSource", typeof(ObservableCollection<TECEquipment>),
              typeof(ByLocationControl), new PropertyMetadata(default(ObservableCollection<TECEquipment>)));

        /// <summary>
        /// Gets or sets the SubScopeSource which is displayed
        /// </summary>
        public ObservableCollection<TECSubScope> SubScopeSource
        {
            get { return (ObservableCollection<TECSubScope>)GetValue(SubScopeSourceProperty); }
            set { SetValue(SubScopeSourceProperty, value); }
        }

        /// <summary>
        /// Identified the SubScopeSource dependency property
        /// </summary>
        public static readonly DependencyProperty SubScopeSourceProperty =
            DependencyProperty.Register("SubScopeSource", typeof(ObservableCollection<TECSubScope>),
              typeof(ByLocationControl), new PropertyMetadata(default(ObservableCollection<TECSubScope>)));

        #endregion

        public ByLocationControl()
        {
            InitializeComponent();
        }
    }
}
