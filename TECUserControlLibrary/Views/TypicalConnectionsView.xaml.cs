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
    /// Interaction logic for SystemConnectionsView.xaml
    /// </summary>
    public partial class TypicalConnectionsView : UserControl
    {
        public TypicalConnectionsVM ViewModel
        {
            get { return (TypicalConnectionsVM)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(TypicalConnectionsVM), typeof(TypicalHierarchyView),
                new PropertyMetadata(default(TypicalConnectionsVM)));

        public static readonly RoutedEvent UpdateEvent =
        EventManager.RegisterRoutedEvent("Update", RoutingStrategy.Bubble,
        typeof(RoutedEventHandler), typeof(TypicalConnectionsView));

        public event RoutedEventHandler Update
        {
            add { AddHandler(UpdateEvent, value); }
            remove { RemoveHandler(UpdateEvent, value); }
        }


        public TypicalConnectionsView()
        {
            InitializeComponent();
        }

        protected void updateStarted(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(UpdateEvent, this));
        }
    }
}
