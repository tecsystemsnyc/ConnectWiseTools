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
using System.Windows.Shapes;
using TECUserControlLibrary.ViewModels;

namespace TECUserControlLibrary.Views
{
    /// <summary>
    /// Interaction logic for BudgetWindow.xaml
    /// </summary>
    public partial class BudgetView : UserControl
    {

        /// <summary>
        /// Gets or sets the ViewModel which is used
        /// </summary>
        public BudgetVM ViewModel
        {
            get { return (BudgetVM)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// Identified the ViewModel dependency property
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(BudgetVM),
              typeof(BudgetView));

        public BudgetView()
        {
            InitializeComponent();
        }
    }
}
