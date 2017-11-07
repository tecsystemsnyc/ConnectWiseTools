using EstimatingLibrary;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace TECUserControlLibrary.UserControls
{

    public partial class ControlledScopeGridControl : UserControl
    {
        #region DPs
        public ObservableCollection<TECSystem> SystemSource
        {
            get { return (ObservableCollection<TECSystem>)GetValue(SystemSourceProperty); }
            set { SetValue(SystemSourceProperty, value); }
        }

        
        public static readonly DependencyProperty SystemSourceProperty =
            DependencyProperty.Register("SystemSource", typeof(ObservableCollection<TECSystem>),
              typeof(ControlledScopeGridControl), new PropertyMetadata(default(ObservableCollection<TECSystem>)));

        public object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(ControlledScopeGridControl), new FrameworkPropertyMetadata(null)
        {
            BindsTwoWayByDefault = true,
            DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
        });

        /// <summary>
        /// Gets or sets wether user can add rows 
        /// </summary>
        public bool UserCanAddRows
        {
            get { return (bool)GetValue(AllowAddingNewProperty); }
            set { SetValue(AllowAddingNewProperty, value); }
        }

        /// <summary>
        /// Identified the AllowAddingNew dependency property
        /// </summary>
        public static readonly DependencyProperty AllowAddingNewProperty =
            DependencyProperty.Register("UserCanAddRows", typeof(bool),
              typeof(ControlledScopeGridControl), new PropertyMetadata(true));


        public Visibility ShowInstanceCount
        {
            get { return (Visibility)GetValue(ShowInstanceCountProperty); }
            set { SetValue(ShowInstanceCountProperty, value); }
        }
        
        public static readonly DependencyProperty ShowInstanceCountProperty =
            DependencyProperty.Register("ShowInstanceCount", typeof(Visibility),
              typeof(ControlledScopeGridControl), new PropertyMetadata(Visibility.Visible));

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
              typeof(ControlledScopeGridControl));
        #endregion
        public ControlledScopeGridControl()
        {
            InitializeComponent();
           
        }
    }
}
