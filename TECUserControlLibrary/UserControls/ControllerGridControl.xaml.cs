using EstimatingLibrary;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace TECUserControlLibrary.UserControls
{
    /// <summary>
    /// Interaction logic for ControllerGridControl.xaml
    /// </summary>
    public partial class ControllerGridControl : UserControl
    {
        #region DPs

        /// <summary>
        /// Gets or sets the DevicesSource which is displayed
        /// </summary>
        public ObservableCollection<TECController> ControllersSource
        {
            get { return (ObservableCollection<TECController>)GetValue(ControllersSourceProperty); }
            set { SetValue(ControllersSourceProperty, value); }
        }

        /// <summary>
        /// Identified the DevicesSource dependency property
        /// </summary>
        public static readonly DependencyProperty ControllersSourceProperty =
            DependencyProperty.Register("ControllersSource", typeof(ObservableCollection<TECController>),
              typeof(ControllerGridControl), new PropertyMetadata(default(ObservableCollection<TECController>)));

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
              typeof(ControllerGridControl));

        #endregion
        public ControllerGridControl()
        {
            InitializeComponent();
        }
    }
}
