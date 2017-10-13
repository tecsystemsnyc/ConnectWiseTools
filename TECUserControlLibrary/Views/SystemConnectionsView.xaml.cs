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
    public partial class SystemConnectionsView : UserControl
    {
        public SystemConnectionsVM ViewModel
        {
            get { return (SystemConnectionsVM)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(SystemConnectionsVM), typeof(SystemConnectionsView),
                new PropertyMetadata(default(SystemConnectionsVM)));

        public static readonly RoutedEvent UpdateEvent =
        EventManager.RegisterRoutedEvent("Update", RoutingStrategy.Bubble,
        typeof(RoutedEventHandler), typeof(SystemConnectionsView));

        public event RoutedEventHandler Update
        {
            add { AddHandler(UpdateEvent, value); }
            remove { RemoveHandler(UpdateEvent, value); }
        }


        public SystemConnectionsView()
        {
            InitializeComponent();
        }

        protected void updateStarted(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(UpdateEvent, this));
        }
    }
}
