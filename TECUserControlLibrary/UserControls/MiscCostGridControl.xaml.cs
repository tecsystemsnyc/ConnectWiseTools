using EstimatingLibrary;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace TECUserControlLibrary.UserControls
{
    /// <summary>
    /// Interaction logic for CostGridControl.xaml
    /// </summary>
    public partial class MiscCostGridControl : UserControl
    {

        /// <summary>
        /// Gets or sets the DevicesSource which is displayed
        /// </summary>
        public ObservableCollection<TECMisc> CostSource
        {
            get { return (ObservableCollection<TECMisc>)GetValue(CostSourceProperty); }
            set { SetValue(CostSourceProperty, value); }
        }

        /// <summary>
        /// Identified the DevicesSource dependency property
        /// </summary>
        public static readonly DependencyProperty CostSourceProperty =
            DependencyProperty.Register("CostSource", typeof(ObservableCollection<TECMisc>),
              typeof(MiscCostGridControl), new PropertyMetadata(default(ObservableCollection<TECMisc>)));

        
        public bool UserCanAddRows
        {
            get { return (bool)GetValue(UserCanAddRowsProperty); }
            set { SetValue(UserCanAddRowsProperty, value); }
        }
        
        public static readonly DependencyProperty UserCanAddRowsProperty =
            DependencyProperty.Register("UserCanAddRows", typeof(bool),
              typeof(MiscCostGridControl), new PropertyMetadata(true));

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
              typeof(MiscCostGridControl));


        public MiscCostGridControl()
        {
            InitializeComponent();
        }
    }
}
