using EstimatingLibrary;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

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
        #endregion

        public SubScopeWiringGridControl()
        {
            InitializeComponent();
        }
    }
}
