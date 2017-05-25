using EstimatingUtilitiesLibrary;
using System;
using System.Collections.Generic;
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
using TECUserControlLibrary.ViewModels;

namespace TECUserControlLibrary.Views
{
    /// <summary>
    /// Interaction logic for TECMaterialView.xaml
    /// </summary>
    public partial class TECMaterialSummaryView : UserControl
    {
        public TECMaterialSummaryVM ViewModel
        {
            get { return (TECMaterialSummaryVM)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(TECMaterialSummaryVM),
              typeof(TECMaterialSummaryView));

        public TECMaterialIndex SelectedMaterialIndex
        {
            get { return (TECMaterialIndex)GetValue(SelectedMaterialIndexProperty); }
            set { SetValue(SelectedMaterialIndexProperty, value); }
        }
        public static readonly DependencyProperty SelectedMaterialIndexProperty =
            DependencyProperty.Register("SelectedMaterialIndex", typeof(TECMaterialIndex),
              typeof(TECMaterialSummaryView), new PropertyMetadata(default(TECMaterialIndex)));

        public TECMaterialSummaryView()
        {
            InitializeComponent();
        }
    }
}
