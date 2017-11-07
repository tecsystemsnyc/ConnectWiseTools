using EstimatingLibrary;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using TECUserControlLibrary.ViewModels;

namespace TECUserControlLibrary.Views
{
    /// <summary>
    /// Interaction logic for ByLocationControl.xaml
    /// </summary>
    public partial class LocationView : UserControl
    {

        #region DPs

        /// <summary>
        /// Gets or sets the LabeledSource which is displayed
        /// </summary>
        public ObservableCollection<TECLabeled> LabeledSource
        {
            get { return (ObservableCollection<TECLabeled>)GetValue(LabeledSourceProperty); }
            set { SetValue(LabeledSourceProperty, value); }
        }

        /// <summary>
        /// Identified the LabeledSource dependency property
        /// </summary>
        public static readonly DependencyProperty LabeledSourceProperty =
            DependencyProperty.Register("LabeledSource", typeof(ObservableCollection<TECLabeled>),
              typeof(LocationView), new PropertyMetadata(default(ObservableCollection<TECLabeled>)));

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
              typeof(LocationView), new PropertyMetadata(default(ObservableCollection<TECSystem>)));


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
              typeof(LocationView), new PropertyMetadata(default(ObservableCollection<TECEquipment>)));

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
              typeof(LocationView), new PropertyMetadata(default(ObservableCollection<TECSubScope>)));

        /// <summary>
        /// Gets or sets the ViewModel which is used
        /// </summary>
        public LocationVM ViewModel
        {
            get { return (LocationVM)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// Identified the ViewModel dependency property
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(LocationVM),
              typeof(LocationView));

        #endregion

        public LocationView()
        {
            InitializeComponent();
        }
    }
}
