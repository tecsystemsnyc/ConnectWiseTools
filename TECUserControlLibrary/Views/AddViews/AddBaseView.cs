using System.Windows;
using System.Windows.Controls;
using TECUserControlLibrary.ViewModels.AddVMs;

namespace TECUserControlLibrary.Views.AddViews
{
    public class AddBaseView : UserControl
    {
        public static readonly RoutedEvent DoneEvent =
        EventManager.RegisterRoutedEvent("Done", RoutingStrategy.Bubble,
        typeof(RoutedEventHandler), typeof(AddBaseView));

        public event RoutedEventHandler Done
        {
            add { AddHandler(DoneEvent, value); }
            remove { RemoveHandler(DoneEvent, value); }
        }

        public static readonly RoutedEvent CancelEvent =
        EventManager.RegisterRoutedEvent("Cancel", RoutingStrategy.Bubble,
        typeof(RoutedEventHandler), typeof(AddBaseView));

        public event RoutedEventHandler Cancel
        {
            add { AddHandler(CancelEvent, value); }
            remove { RemoveHandler(CancelEvent, value); }
        }

        public IAddVM ViewModel
        {
            get { return (IAddVM)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(IAddVM), typeof(AddBaseView), new PropertyMetadata(default(object)));
        
        protected void doneButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(DoneEvent, this));
        }
        protected void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(CancelEvent, this));
        }
    }
}
