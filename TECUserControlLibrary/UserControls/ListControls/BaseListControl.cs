using GongSolutions.Wpf.DragDrop;
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
using EstimatingLibrary;
using EstimatingLibrary.Interfaces;
using TECUserControlLibrary.ViewModels;

namespace TECUserControlLibrary.UserControls.ListControls
{
    /// <summary>
    /// Interaction logic for TypicalListControl.xaml
    /// </summary>
    public partial class BaseListControl<T> : UserControl
    {
        public IEnumerable<T> Source
        {
            get { return (IEnumerable<T>)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(IEnumerable<T>),
              typeof(BaseListControl<T>), new PropertyMetadata(default(IEnumerable<T>)));

        public T SelectedItem
        {
            get { return (T)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(T),
                typeof(BaseListControl<T>), new FrameworkPropertyMetadata(null)
                {
                    BindsTwoWayByDefault = true,
                    DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                });
        
        public IDropTarget DropHandler
        {
            get { return (IDropTarget)GetValue(DropHandlerProperty); }
            set { SetValue(DropHandlerProperty, value); }
        }
        
        public static readonly DependencyProperty DropHandlerProperty =
            DependencyProperty.Register("DropHandler", typeof(IDropTarget),
              typeof(BaseListControl<T>));

        public static readonly RoutedEvent SelectedEvent =
        EventManager.RegisterRoutedEvent("Selected", RoutingStrategy.Bubble,
        typeof(RoutedEventHandler), typeof(BaseListControl<T>));

        public event RoutedEventHandler Selected
        {
            add { AddHandler(SelectedEvent, value); }
            remove { RemoveHandler(SelectedEvent, value); }
        }
        
        public static readonly RoutedEvent DroppedEvent =
        EventManager.RegisterRoutedEvent("Dropped", RoutingStrategy.Bubble,
        typeof(RoutedEventHandler), typeof(BaseListControl<T>));

        public event RoutedEventHandler Dropped
        {
            add { AddHandler(DroppedEvent, value); }
            remove { RemoveHandler(DroppedEvent, value); }
        }
        
        public object ScopeParent
        {
            get { return (object)GetValue(ScopeParentProperty); }
            set { SetValue(ScopeParentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ScopeParent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScopeParentProperty =
            DependencyProperty.Register("ScopeParent", typeof(object), typeof(BaseListControl<T>));
        
        public bool IsDragSource
        {
            get { return (bool)GetValue(IsDragSourceProperty); }
            set { SetValue(IsDragSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsDragSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsDragSourceProperty =
            DependencyProperty.Register("IsDragSource", typeof(bool), typeof(BaseListControl<T>), new PropertyMetadata(false));
        
        public ICommand DeleteCommand
        {
            get { return (ICommand)GetValue(DeleteCommandProperty); }
            set { SetValue(DeleteCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DeleteCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DeleteCommandProperty =
            DependencyProperty.Register("DeleteCommand", typeof(ICommand), typeof(BaseListControl<T>));
        
        protected void ListView_Selected(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(SelectedEvent, this));
        }
        protected void ListView_MouseUp(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(SelectedEvent, this));
        }
        protected void ListView_Dropped(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(DroppedEvent, this));
        }
    }
}
