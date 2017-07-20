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
    /// Interaction logic for AddTagControl.xaml
    /// </summary>
    public partial class AddTagControl : UserControl
    {
        public ICommand AddTagCommand
        {
            get { return (ICommand)GetValue(AddTagCommandProperty); }
            set { SetValue(AddTagCommandProperty, value); }
        }

        public static readonly DependencyProperty AddTagCommandProperty =
            DependencyProperty.Register("AddTagCommand", typeof(ICommand),
              typeof(AddTagControl));

        public ObservableCollection<TECLabeled> TagList
        {
            get { return (ObservableCollection<TECLabeled>)GetValue(TagListProperty); }
            set { SetValue(TagListProperty, value); }
        }

        public static readonly DependencyProperty TagListProperty =
            DependencyProperty.Register("TagList", typeof(ObservableCollection<TECLabeled>),
              typeof(AddTagControl));

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
              typeof(AddTagControl));

        public AddTagControl()
        {
            InitializeComponent();
        }
    }
}
