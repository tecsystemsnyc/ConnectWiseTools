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

namespace TECUserControlLibrary
{
    /// <summary>
    /// Interaction logic for SubScopeWiringGridControl.xaml
    /// </summary>
    public partial class SubScopeWiringGridControl : UserControl
    {
        #region DPs

        /// <summary>
        /// Gets or sets the SubScopeSource which is displayed
        /// </summary>
        public ObservableCollection<Tuple<string, string, TECSubScope>> SubScopeSource
        {
            get { return (ObservableCollection<Tuple<string, string, TECSubScope>>)GetValue(SubScopeSourceProperty); }
            set { SetValue(SubScopeSourceProperty, value); }
        }

        /// <summary>
        /// Identified the SubScopeSource dependency property
        /// </summary>
        public static readonly DependencyProperty SubScopeSourceProperty =
            DependencyProperty.Register("SubScopeSource", typeof(ObservableCollection<Tuple<string, string, TECSubScope>>),
              typeof(SubScopeWiringGridControl), new PropertyMetadata(default(ObservableCollection<Tuple<string, string, TECSubScope>>)));

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
              typeof(SubScopeWiringGridControl));

        #endregion

        public SubScopeWiringGridControl()
        {
            InitializeComponent();
        }
    }
}
