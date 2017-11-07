using EstimatingLibrary;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace TECUserControlLibrary.UserControls
{
    /// <summary>
    /// Interaction logic for ConduitTypeGridControl.xaml
    /// </summary>
    public partial class ConduitTypeGridControl : UserControl
    {
        #region DPs

        public ObservableCollection<TECElectricalMaterial> ConduitTypesSource
        {
            get { return (ObservableCollection<TECElectricalMaterial>)GetValue(ConduitTypesSourceProperty); }
            set { SetValue(ConduitTypesSourceProperty, value); }
        }

        public static readonly DependencyProperty ConduitTypesSourceProperty =
            DependencyProperty.Register("ConduitTypesSource", typeof(ObservableCollection<TECElectricalMaterial>),
              typeof(ConduitTypeGridControl), new PropertyMetadata(default(ObservableCollection<TECElectricalMaterial>)));

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
              typeof(ConduitTypeGridControl));
        #endregion
        public ConduitTypeGridControl()
        {
            InitializeComponent();
        }
    }
}
