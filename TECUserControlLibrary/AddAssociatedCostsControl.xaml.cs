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

namespace TECUserControlLibrary
{
    /// <summary>
    /// Interaction logic for AddAssociatedCostsControl.xaml
    /// </summary>
    public partial class AddAssociatedCostsControl : UserControl
    {

        public Object ViewModel
        {
            get { return (Object)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(Object),
              typeof(AddAssociatedCostsControl));

        public ICommand AddAssociatedCostCommand
        {
            get { return (ICommand)GetValue(AddAssociatedCostCommandProperty); }
            set { SetValue(AddAssociatedCostCommandProperty, value); }
        }

        public static readonly DependencyProperty AddAssociatedCostCommandProperty =
            DependencyProperty.Register("AddAssociatedCostCommand", typeof(ICommand),
              typeof(AddAssociatedCostsControl));

        public ObservableCollection<TECAssociatedCost> AssociatedCostList
        {
            get { return (ObservableCollection<TECAssociatedCost>)GetValue(AssociatedCostListProperty); }
            set { SetValue(AssociatedCostListProperty, value); }
        }

        public static readonly DependencyProperty AssociatedCostListProperty =
            DependencyProperty.Register("AssociatedCostList", typeof(ObservableCollection<TECAssociatedCost>),
              typeof(AddAssociatedCostsControl));

        public AddAssociatedCostsControl()
        {
            InitializeComponent();
        }
    }
}
