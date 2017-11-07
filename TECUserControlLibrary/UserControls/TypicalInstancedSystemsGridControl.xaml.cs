using EstimatingLibrary;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace TECUserControlLibrary.UserControls
{
    /// <summary>
    /// Interaction logic for ControlledScopeSystemsGrid.xaml
    /// </summary>
    public partial class TypicalInstancedSystemsGridControl : UserControl
    {
        #region DPs

        /// <summary>
        /// Gets or sets the TypicalSystemsSource which is displayed
        /// </summary>
        public ObservableCollection<TECSystem> TypicalSystemsSource
        {
            get { return (ObservableCollection<TECSystem>)GetValue(TypicalSystemsSourceProperty); }
            set { SetValue(TypicalSystemsSourceProperty, value); }
        }

        /// <summary>
        /// Identified the TypicalSystemsSource dependency property
        /// </summary>
        public static readonly DependencyProperty TypicalSystemsSourceProperty =
            DependencyProperty.Register("TypicalSystemsSource", typeof(ObservableCollection<TECSystem>),
              typeof(TypicalInstancedSystemsGridControl), new PropertyMetadata(default(ObservableCollection<TECSystem>)));

        public TECSystem SelectedTypicalSystem
        {
            get { return (TECSystem)GetValue(SelectedTypicalSystemProperty); }
            set { SetValue(SelectedTypicalSystemProperty, value); }
        }

        public static readonly DependencyProperty SelectedTypicalSystemProperty =
            DependencyProperty.Register("SelectedTypicalSystem", typeof(TECSystem),
                typeof(TypicalInstancedSystemsGridControl), new FrameworkPropertyMetadata(null)
                {
                    BindsTwoWayByDefault = true,
                    DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                });

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
              typeof(TypicalInstancedSystemsGridControl));
        #endregion
        public TypicalInstancedSystemsGridControl()
        {
            InitializeComponent();
        }
    }
}
