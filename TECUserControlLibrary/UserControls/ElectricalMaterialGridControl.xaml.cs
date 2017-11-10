using EstimatingLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace TECUserControlLibrary.UserControls
{
    /// <summary>
    /// Interaction logic for ConnectionTypeGridControl.xaml
    /// </summary>
    public partial class ElectricalMaterialGridControl : UserControl
    {
        #region DPs

        /// <summary>
        /// Gets or sets the DevicesSource which is displayed
        /// </summary>
        public IEnumerable<TECElectricalMaterial> ElectricalMaterialSource
        {
            get { return (IEnumerable<TECElectricalMaterial>)GetValue(ConnectionTypesSourceProperty); }
            set { SetValue(ConnectionTypesSourceProperty, value); }
        }

        /// <summary>
        /// Identified the DevicesSource dependency property
        /// </summary>
        public static readonly DependencyProperty ConnectionTypesSourceProperty =
            DependencyProperty.Register("ElectricalMaterialSource", typeof(IEnumerable<TECElectricalMaterial>),
              typeof(ElectricalMaterialGridControl), new PropertyMetadata(default(IEnumerable<TECElectricalMaterial>)));


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
              typeof(ElectricalMaterialGridControl));
        

        public TECElectricalMaterial Selected
        {
            get { return (TECElectricalMaterial)GetValue(SelectedProperty); }
            set { SetValue(SelectedProperty, value); }
        }

        public static readonly DependencyProperty SelectedProperty =
            DependencyProperty.Register("Selected", typeof(TECElectricalMaterial),
                typeof(ElectricalMaterialGridControl), new FrameworkPropertyMetadata(null)
                {
                    BindsTwoWayByDefault = true,
                    DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                });

        #endregion

        public ElectricalMaterialGridControl()
        {
            InitializeComponent();
        }
    }
}
