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
    /// Interaction logic for ControlledScopeSystemsGrid.xaml
    /// </summary>
    public partial class ControlledScopeSystemsGridContol : UserControl
    {
        #region DPs

        /// <summary>
        /// Gets or sets the ControlledScopeSource which is displayed
        /// </summary>
        public ObservableCollection<TECSystem> ControlledScopeSource
        {
            get { return (ObservableCollection<TECSystem>)GetValue(ControlledScopeSourceProperty); }
            set { SetValue(ControlledScopeSourceProperty, value); }
        }

        /// <summary>
        /// Identified the ControlledScopeSource dependency property
        /// </summary>
        public static readonly DependencyProperty ControlledScopeSourceProperty =
            DependencyProperty.Register("ControlledScopeSource", typeof(ObservableCollection<TECSystem>),
              typeof(ControlledScopeSystemsGridContol), new PropertyMetadata(default(ObservableCollection<TECSystem>)));
        #endregion
        public ControlledScopeSystemsGridContol()
        {
            InitializeComponent();
        }
    }
}
