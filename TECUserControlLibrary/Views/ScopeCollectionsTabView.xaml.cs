using System.Windows;
using System.Windows.Controls;
using TECUserControlLibrary.ViewModels;

namespace TECUserControlLibrary.Views
{
    /// <summary>
    /// Interaction logic for ScopeCollectionsTabControl.xaml
    /// </summary>
    public partial class ScopeCollectionsTabView : UserControl
    {
        #region DPs

        /// <summary>
        /// Gets or sets the ViewModel which is used
        /// </summary>
        public ScopeCollectionsTabVM ViewModel
        {
            get { return (ScopeCollectionsTabVM)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// Identified the ViewModel dependency property
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(ScopeCollectionsTabVM),
              typeof(ScopeCollectionsTabView));

        #endregion
        public ScopeCollectionsTabView()
        {
            InitializeComponent();
        }
    }
}
