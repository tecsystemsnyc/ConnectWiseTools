using EstimatingLibrary;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace TECUserControlLibrary.UserControls
{
    /// <summary>
    /// Interaction logic for PanelsGrid.xaml
    /// </summary>
    public partial class PanelsGridControl : UserControl
    {
        #region DPs

        /// <summary>
        /// Gets or sets the SystemSource which is displayed
        /// </summary>
        public ObservableCollection<TECPanel> PanelsSource
        {
            get { return (ObservableCollection<TECPanel>)GetValue(PanelsSourceProperty); }
            set { SetValue(PanelsSourceProperty, value); }
        }

        /// <summary>
        /// Identified the SystemSource dependency property
        /// </summary>
        public static readonly DependencyProperty PanelsSourceProperty =
            DependencyProperty.Register("PanelsSource", typeof(ObservableCollection<TECPanel>),
              typeof(PanelsGridControl), new PropertyMetadata(default(ObservableCollection<TECPanel>)));


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
              typeof(PanelsGridControl));

        #endregion
        public PanelsGridControl()
        {
            InitializeComponent();
        }
    }
}
