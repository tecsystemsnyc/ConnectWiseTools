using EstimatingLibrary;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TECUserControlLibrary.UserControls
{
    /// <summary>
    /// Interaction logic for AddAssociatedCostsControl.xaml
    /// </summary>
    public partial class AddAssociatedCostsControl : UserControl
    {
        public ICommand AddAssociatedCostCommand
        {
            get { return (ICommand)GetValue(AddAssociatedCostCommandProperty); }
            set { SetValue(AddAssociatedCostCommandProperty, value); }
        }

        public static readonly DependencyProperty AddAssociatedCostCommandProperty =
            DependencyProperty.Register("AddAssociatedCostCommand", typeof(ICommand),
              typeof(AddAssociatedCostsControl));

        public ObservableCollection<TECCost> AssociatedCostList
        {
            get { return (ObservableCollection<TECCost>)GetValue(AssociatedCostListProperty); }
            set { SetValue(AssociatedCostListProperty, value); }
        }

        public static readonly DependencyProperty AssociatedCostListProperty =
            DependencyProperty.Register("AssociatedCostList", typeof(ObservableCollection<TECCost>),
              typeof(AddAssociatedCostsControl));

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
              typeof(AddAssociatedCostsControl));

        public AddAssociatedCostsControl()
        {
            InitializeComponent();
        }
    }
}
