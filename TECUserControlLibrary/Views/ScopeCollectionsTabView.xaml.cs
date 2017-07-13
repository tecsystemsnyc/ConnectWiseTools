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
