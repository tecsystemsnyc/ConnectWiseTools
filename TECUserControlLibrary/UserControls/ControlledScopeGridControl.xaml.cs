using EstimatingLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TECUserControlLibrary.UserControls
{
    /// <summary>
    /// Interaction logic for ControlledScopeDataGrid.xaml
    /// </summary>
    public partial class ControlledScopeGridControl : UserControl
    {
        #region DPs

        /// <summary>
        /// Gets or sets the ControlledScopeSource which is displayed
        /// </summary>
        public ObservableCollection<TECSystem> SystemSource
        {
            get { return (ObservableCollection<TECSystem>)GetValue(SystemSourceProperty); }
            set { SetValue(SystemSourceProperty, value); }
        }

        /// <summary>
        /// Identified the ControlledScopeSource dependency property
        /// </summary>
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
        #endregion
        public ControlledScopeGridControl()
        {
            InitializeComponent();
           
        }
    }
}
