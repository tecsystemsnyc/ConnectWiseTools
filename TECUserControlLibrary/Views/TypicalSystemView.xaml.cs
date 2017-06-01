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
    /// Interaction logic for AddControlledScopeView.xaml
    /// </summary>
    public partial class TypicalSystemsView : UserControl
    {
        /// <summary>
        /// Gets or sets the ViewModel which is used
        /// </summary>
        public TypicalSystemVM ViewModel
        {
            get { return (TypicalSystemVM)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// Identified the ViewModel dependency property
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(TypicalSystemVM),
              typeof(TypicalSystemsView));
        
        public ControlledScopeEditIndex SelectedEditIndex
        {
            get { return (ControlledScopeEditIndex)GetValue(SelectedEditIndexProperty); }
            set { SetValue(SelectedEditIndexProperty, value); }
        }
        
        public static readonly DependencyProperty SelectedEditIndexProperty =
            DependencyProperty.Register("SelectedEditIndex", typeof(ControlledScopeEditIndex),
              typeof(TypicalSystemsView), new PropertyMetadata(default(ControlledScopeEditIndex)));

        public ControlledScopeItemIndex SelectedItemIndex
        {
            get { return (ControlledScopeItemIndex)GetValue(SelectedItemIndexProperty); }
            set { SetValue(SelectedItemIndexProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemIndexProperty =
            DependencyProperty.Register("SelectedItemIndex", typeof(ControlledScopeItemIndex),
              typeof(TypicalSystemsView), new PropertyMetadata(default(ControlledScopeItemIndex)));
        
        public TypicalSystemsView()
        {
            InitializeComponent();
        }
    }
}
