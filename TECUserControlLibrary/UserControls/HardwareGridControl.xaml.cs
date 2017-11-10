using EstimatingLibrary;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace TECUserControlLibrary.UserControls
{

    /// <summary>
    /// Interaction logic for DevicesGridControl.xaml
    /// </summary>
    public partial class HardwareGridControl : UserControl
    {
        
        public IEnumerable<TECHardware> HardwareSource
        {
            get { return (IEnumerable<TECHardware>)GetValue(HardwareSourceProperty); }
            set { SetValue(HardwareSourceProperty, value); }
        }

        /// <summary>
        /// Identified the DevicesSource dependency property
        /// </summary>
        public static readonly DependencyProperty HardwareSourceProperty =
            DependencyProperty.Register("HardwareSource", typeof(IEnumerable<TECHardware>),
              typeof(HardwareGridControl));

        public TECHardware Selected
        {
            get { return (TECHardware)GetValue(SelectedHardwareProperty); }
            set { SetValue(SelectedHardwareProperty, value); }
        }

        public static readonly DependencyProperty SelectedHardwareProperty = 
            DependencyProperty.Register("Selected", typeof(TECDevice), 
                typeof(HardwareGridControl), new FrameworkPropertyMetadata(null)
        {
            BindsTwoWayByDefault = true,
            DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
        });
        
        public IDropTarget DropHandler
        {
            get { return (IDropTarget)GetValue(DropHandlerProperty); }
            set { SetValue(DropHandlerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DropHandler.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DropHandlerProperty =
            DependencyProperty.Register("DropHandler", typeof(IDropTarget), typeof(HardwareGridControl));



        public HardwareGridControl()
        {
            InitializeComponent();
        }
    }
}
