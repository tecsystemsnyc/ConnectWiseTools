using EstimatingLibrary;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace TECUserControlLibrary.UserControls
{
    /// <summary>
    /// Interaction logic for PanelTypeGridControl.xaml
    /// </summary>
    public partial class PanelTypeGridControl : UserControl
    {
        #region DPs
        /// <summary>
        /// Gets or sets the DevicesSource which is displayed
        /// </summary>
        public ObservableCollection<TECPanelType> PanelTypeSource
        {
            get { return (ObservableCollection<TECPanelType>)GetValue(PanelTypeSourceProperty); }
            set { SetValue(PanelTypeSourceProperty, value); }
        }

        /// <summary>
        /// Identified the DevicesSource dependency property
        /// </summary>
        public static readonly DependencyProperty PanelTypeSourceProperty =
            DependencyProperty.Register("PanelTypeSource", typeof(ObservableCollection<TECPanelType>),
              typeof(PanelTypeGridControl), new PropertyMetadata(default(ObservableCollection<TECPanelType>)));

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
              typeof(PanelTypeGridControl));
        #endregion

        public PanelTypeGridControl()
        {
            InitializeComponent();
        }
    }
}
