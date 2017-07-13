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
              typeof(AssociatedCostsGridControl));

        #endregion
        public AssociatedCostsGridControl()
        {
            InitializeComponent();
        }
    }
}
