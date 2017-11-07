﻿using EstimatingLibrary;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace TECUserControlLibrary.UserControls
{
    /// <summary>
    /// Interaction logic for EquipmentHierarchy.xaml
    /// </summary>
    public partial class EquipmentHierarchy : UserControl
    {

        public double EquipmentWidth
        {
            get { return (double)GetValue(EquipmentWidthProperty); }
            set { SetValue(EquipmentWidthProperty, value); }
        }

        public static readonly DependencyProperty EquipmentWidthProperty =
            DependencyProperty.Register("EquipmentWidth", typeof(double),
              typeof(EquipmentHierarchy), new PropertyMetadata(0.0));

        public double HalfWidth
        {
            get { return (double)GetValue(HalfWidthProperty); }
            set { SetValue(HalfWidthProperty, value); }
        }

        public static readonly DependencyProperty HalfWidthProperty =
            DependencyProperty.Register("HalfWidth", typeof(double),
              typeof(EquipmentHierarchy), new PropertyMetadata(0.0));
        
        public ObservableCollection<TECEquipment> EquipmentSource
        {
            get { return (ObservableCollection<TECEquipment>)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("EquipmentSource", typeof(ObservableCollection<TECEquipment>),
              typeof(EquipmentHierarchy), new PropertyMetadata(default(ObservableCollection<TECEquipment>)));

        public TECEquipment SelectedEquipment
        {
            get { return (TECEquipment)GetValue(SelectedEquipmentProperty); }
            set { SetValue(SelectedEquipmentProperty, value); }
        }

        public static readonly DependencyProperty SelectedEquipmentProperty =
            DependencyProperty.Register("SelectedEquipment", typeof(TECEquipment),
                typeof(EquipmentHierarchy), new FrameworkPropertyMetadata(default(TECEquipment))
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
                typeof(EquipmentHierarchy), new FrameworkPropertyMetadata(default(TECSubScope))
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
                typeof(EquipmentHierarchy), new FrameworkPropertyMetadata(default(TECDevice))
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
        typeof(RoutedEventHandler), typeof(EquipmentHierarchy));

        protected void Equipment_Selected(object sender, RoutedEventArgs e)
        {
            RoutedEventArgs args = new RoutedEventArgs(SelectedEvent);
            RaiseEvent(new RoutedEventArgs(SelectedEvent, this));
        }

        public EquipmentHierarchy()
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
            };
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SelectedSubScope = null;
            SelectedEquipment = null;
            SelectedDevice = null;
        }
    }
}
