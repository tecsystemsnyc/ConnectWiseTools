using EstimatingLibrary;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using TECUserControlLibrary.ViewModels;

namespace TECUserControlLibrary.Views
{
    /// <summary>
    /// Interaction logic for SubScopeGridControl.xaml
    /// </summary>
    public partial class EquipmentGridView : UserControl
    {
        #region DPs

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
              typeof(EquipmentGridView), new PropertyMetadata(default(ObservableCollection<TECEquipment>)));


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
              typeof(EquipmentGridView), new PropertyMetadata(true));

        /// <summary>
        /// Gets or sets the ViewModel which is used
        /// </summary>
        public EquipmentVM ViewModel
        {
            get { return (EquipmentVM)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// Identified the ViewModel dependency property
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(EquipmentVM),
              typeof(EquipmentGridView));
        #endregion

        public EquipmentGridView()
        {
            InitializeComponent();
        }
    }
}
