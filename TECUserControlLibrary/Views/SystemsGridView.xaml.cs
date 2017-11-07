using EstimatingLibrary;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using TECUserControlLibrary.ViewModels;

namespace TECUserControlLibrary.Views
{
    /// <summary>
    /// Interaction logic for SystemsGridControl.xaml
    /// </summary>
    public partial class SystemsGridView : UserControl
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
              typeof(SystemsGridView), new PropertyMetadata(default(ObservableCollection<TECSystem>)));

        /// <summary>
        /// Gets or sets the ViewModel which is used
        /// </summary>
        public SystemsVM ViewModel
        {
            get { return (SystemsVM)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// Identified the ViewModel dependency property
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(SystemsVM),
              typeof(SystemsGridView));


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
              typeof(SystemsGridView), new PropertyMetadata(true));


        #endregion

        public SystemsGridView()
        {
            InitializeComponent();
        }
        
    }
}
