using EstimatingLibrary;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

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
        public IDropTarget DropHandler
        {
            get { return (IDropTarget)GetValue(DropHandlerProperty); }
            set { SetValue(DropHandlerProperty, value); }
        }

        /// <summary>
        /// Identified the ViewModel dependency property
        /// </summary>
        public static readonly DependencyProperty DropHandlerProperty =
            DependencyProperty.Register("DropHandler", typeof(IDropTarget),
              typeof(AssociatedCostsGridControl));


        public TECCost Selected
        {
            get { return (TECCost)GetValue(SelectedProperty); }
            set { SetValue(SelectedProperty, value); }
        }
        
        public static readonly DependencyProperty SelectedProperty =
            DependencyProperty.Register("Selected", typeof(TECCost),
                typeof(AssociatedCostsGridControl), new FrameworkPropertyMetadata(null)
                {
                    BindsTwoWayByDefault = true,
                    DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                });
        #endregion
        public AssociatedCostsGridControl()
        {
            InitializeComponent();
        }
    }
}
