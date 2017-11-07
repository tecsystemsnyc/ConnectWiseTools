using EstimatingLibrary;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace TECUserControlLibrary.UserControls
{
    /// <summary>
    /// Interaction logic for AssociatedCostsGridControl.xaml
    /// </summary>
    public partial class AssociatedCostsGridControl : UserControl
    {
        #region DPs

        public ObservableCollection<TECCost> CostsSource
        {
            get { return (ObservableCollection<TECCost>)GetValue(CostsSourceProperty); }
            set { SetValue(CostsSourceProperty, value); }
        }

        public static readonly DependencyProperty CostsSourceProperty =
            DependencyProperty.Register("CostsSource", typeof(ObservableCollection<TECCost>),
              typeof(AssociatedCostsGridControl), new PropertyMetadata(default(ObservableCollection<TECCost>)));


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
              typeof(AssociatedCostsGridControl));

        #endregion
        public AssociatedCostsGridControl()
        {
            InitializeComponent();
        }
    }
}
