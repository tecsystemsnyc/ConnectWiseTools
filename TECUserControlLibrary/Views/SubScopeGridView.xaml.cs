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
    public partial class SubScopeGridView : UserControl
    {
        #region DPs

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
              typeof(SubScopeGridView), new PropertyMetadata(default(ObservableCollection<TECSubScope>)));

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
              typeof(SubScopeGridView), new PropertyMetadata(true));

        /// <summary>
        /// Gets or sets the ViewModel which is used
        /// </summary>
        public SubScopeVM ViewModel
        {
            get { return (SubScopeVM)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// Identified the ViewModel dependency property
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(SubScopeVM),
              typeof(SubScopeGridView));

        #endregion

        public SubScopeGridView()
        {
            InitializeComponent();
        }
    }
}
