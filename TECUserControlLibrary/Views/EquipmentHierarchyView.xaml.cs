using EstimatingLibrary;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Animation;
using TECUserControlLibrary.ViewModels;

namespace TECUserControlLibrary.Views
{
    /// <summary>
    /// Interaction logic for EquipmentHierarchyView.xaml
    /// </summary>
    public partial class EquipmentHierarchyView : UserControl
    {
        public double ModalHeight
        {
            get { return (double)GetValue(ModalHeightProperty); }
            set { SetValue(ModalHeightProperty, value); }
        }
        public static readonly DependencyProperty ModalHeightProperty =
            DependencyProperty.Register("ModalHeight", typeof(double),
              typeof(EquipmentHierarchyView), new PropertyMetadata(1.0));

        public double EquipmentWidth
        {
            get { return (double)GetValue(EquipmentWidthProperty); }
            set { SetValue(EquipmentWidthProperty, value); }
        }

        public static readonly DependencyProperty EquipmentWidthProperty =
            DependencyProperty.Register("EquipmentWidth", typeof(double),
              typeof(EquipmentHierarchyView), new PropertyMetadata(0.0));

        public double HalfWidth
        {
            get { return (double)GetValue(HalfWidthProperty); }
            set { SetValue(HalfWidthProperty, value); }
        }

        public static readonly DependencyProperty HalfWidthProperty =
            DependencyProperty.Register("HalfWidth", typeof(double),
              typeof(EquipmentHierarchyView), new PropertyMetadata(0.0));
        
        public ObservableCollection<TECEquipment> EquipmentSource
        {
            get { return (ObservableCollection<TECEquipment>)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("EquipmentSource", typeof(ObservableCollection<TECEquipment>),
              typeof(EquipmentHierarchyView), new PropertyMetadata(default(ObservableCollection<TECEquipment>)));

        public TECEquipment SelectedEquipment
        {
            get { return (TECEquipment)GetValue(SelectedEquipmentProperty); }
            set { SetValue(SelectedEquipmentProperty, value); }
        }

        public static readonly DependencyProperty SelectedEquipmentProperty =
            DependencyProperty.Register("SelectedEquipment", typeof(TECEquipment),
                typeof(EquipmentHierarchyView), new FrameworkPropertyMetadata(default(TECEquipment))
                {
                    BindsTwoWayByDefault = true,
                    DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                });

        public TECSubScope SelectedSubScope
        {
            get { return (TECSubScope)GetValue(SelectedSubScopeProperty); }
            set { SetValue(SelectedSubScopeProperty, value); }
        }

        public static readonly DependencyProperty SelectedSubScopeProperty =
            DependencyProperty.Register("SelectedSubScope", typeof(TECSubScope),
                typeof(EquipmentHierarchyView), new FrameworkPropertyMetadata(default(TECSubScope))
                {
                    BindsTwoWayByDefault = true,
                    DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                });

        public TECDevice SelectedDevice
        {
            get { return (TECDevice)GetValue(SelectedDeviceProperty); }
            set { SetValue(SelectedDeviceProperty, value); }
        }

        public static readonly DependencyProperty SelectedDeviceProperty =
            DependencyProperty.Register("SelectedDevice", typeof(TECDevice),
                typeof(EquipmentHierarchyView), new FrameworkPropertyMetadata(default(TECDevice))
                {
                    BindsTwoWayByDefault = true,
                    DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                });

        public event RoutedEventHandler EquipmentSelected
        {
            add { AddHandler(SelectedEvent, value); }
            remove { RemoveHandler(SelectedEvent, value); }
        }

        public static readonly RoutedEvent SelectedEvent =
        EventManager.RegisterRoutedEvent("EquipmentSelected", RoutingStrategy.Bubble,
        typeof(RoutedEventHandler), typeof(EquipmentHierarchyView));

        protected void Equipment_Selected(object sender, RoutedEventArgs e)
        {
            RoutedEventArgs args = new RoutedEventArgs(SelectedEvent);
            RaiseEvent(new RoutedEventArgs(SelectedEvent, this));
        }

        public EquipmentHierarchyVM ViewModel
        {
            get { return (EquipmentHierarchyVM)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(EquipmentHierarchyVM), typeof(EquipmentHierarchyView));



        public IDropTarget DropHandler
        {
            get { return (IDropTarget)GetValue(DropHandlerProperty); }
            set { SetValue(DropHandlerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DropHandler.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DropHandlerProperty =
            DependencyProperty.Register("DropHandler", typeof(IDropTarget), typeof(EquipmentHierarchyView));



        public EquipmentHierarchyView()
        {
            InitializeComponent();
            SizeChanged += (sender, e) =>
            {
                if (e.WidthChanged)
                {
                    if(HalfWidth == 0 || EquipmentWidth != 0)
                    {
                        EquipmentWidth = e.NewSize.Width / 2;
                    }
                    HalfWidth = e.NewSize.Width / 2;
                }
                if (e.HeightChanged)
                {
                    if (ModalHeight != 0.0)
                    {
                        ModalHeight = e.NewSize.Height;
                    }
                }
            };
            
        }

        private void equipmentBack_Click(object sender, RoutedEventArgs e)
        {
            subScopeList.SelectedItem = null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SelectedSubScope = null;
            SelectedEquipment = null;
            SelectedDevice = null;
        }
        private void Add_Clicked(object sender, RoutedEventArgs e)
        {
            Storyboard moveBack = (Storyboard)FindResource("modalIn");
            moveBack.Begin();
        }
        private void modalOut_Completed(object sender, EventArgs e)
        {
            ModalHeight = this.ActualHeight;
        }

        private void modalIn_Completed(object sender, EventArgs e)
        {
            ModalHeight = 0;
        }
        private void equipmentMove_Completed(object sender, EventArgs e)
        {
            EquipmentWidth = 0;
        }

        private void equipmentMoveBack_Completed(object sender, EventArgs e)
        {
            EquipmentWidth = this.ActualWidth / 2;
        }
    }
}
