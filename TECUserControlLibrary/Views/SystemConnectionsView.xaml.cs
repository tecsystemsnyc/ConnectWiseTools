using System.Windows;
using System.Windows.Controls;
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



        public bool IsTypical
        {
            get { return (bool)GetValue(IsTypicalProperty); }
            set { SetValue(IsTypicalProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsTypical.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsTypicalProperty =
            DependencyProperty.Register("IsTypical", typeof(bool), typeof(SystemConnectionsView), new PropertyMetadata(false));



        public UpdateConnectionVM UpdateConnectionVM
        {
            get { return (UpdateConnectionVM)GetValue(UpdateConnectionVMProperty); }
            set { SetValue(UpdateConnectionVMProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UpdateConnectionVM.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UpdateConnectionVMProperty =
            DependencyProperty.Register("UpdateConnectionVM", typeof(UpdateConnectionVM), 
                typeof(SystemConnectionsView), new PropertyMetadata(new PropertyChangedCallback(OnUpdateConnectionVMChanged)));
        
        private static void OnUpdateConnectionVMChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if ((e.NewValue as UpdateConnectionVM) != null)
            {
                SystemConnectionsView thisView = dependencyObject as SystemConnectionsView;
                if (thisView != null)
                {
                    thisView.RaiseEvent(new RoutedEventArgs(UpdateEvent, thisView));
                }
            }
        }

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
