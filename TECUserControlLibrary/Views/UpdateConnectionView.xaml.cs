using System.Windows;
using System.Windows.Controls;
using TECUserControlLibrary.ViewModels;

namespace TECUserControlLibrary.Views
{
    /// <summary>
    /// Interaction logic for UpdateConnectionView.xaml
    /// </summary>
    public partial class UpdateConnectionView : UserControl
    {

        public UpdateConnectionVM ViewModel
        {
            get { return (UpdateConnectionVM)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(UpdateConnectionVM), typeof(UpdateConnectionView));

        public static readonly RoutedEvent UpdatedEvent =
        EventManager.RegisterRoutedEvent("Updated", RoutingStrategy.Bubble,
        typeof(RoutedEventHandler), typeof(UpdateConnectionView));

        public event RoutedEventHandler Updated
        {
            add { AddHandler(UpdatedEvent, value); }
            remove { RemoveHandler(UpdatedEvent, value); }
        }


        public UpdateConnectionView()
        {
            InitializeComponent();
        }

        private void doneButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(UpdatedEvent, this));
        }
    }
}
