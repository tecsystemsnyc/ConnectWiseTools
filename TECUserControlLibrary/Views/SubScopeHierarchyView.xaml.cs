
using EstimatingLibrary;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Animation;
using TECUserControlLibrary.ViewModels;

namespace TECUserControlLibrary.Views
{
    /// <summary>
    /// Interaction logic for SubScopeHierarchyView.xaml
    /// </summary>
    public partial class SubScopeHierarchyView : UserControl
    {
        public double ModalHeight
        {
            get { return (double)GetValue(ModalHeightProperty); }
            set { SetValue(ModalHeightProperty, value); }
        }

        public static readonly DependencyProperty ModalHeightProperty =
            DependencyProperty.Register("ModalHeight", typeof(double),
              typeof(SubScopeHierarchyView), new PropertyMetadata(1.0));



        public IEnumerable<TECSubScope> SubScopeSource
        {
            get { return (IEnumerable<TECSubScope>)GetValue(SubScopeSourceProperty); }
            set { SetValue(SubScopeSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SubScopeSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SubScopeSourceProperty =
            DependencyProperty.Register("SubScopeSource", typeof(IEnumerable<TECSubScope>), typeof(SubScopeHierarchyView));



        public TECSubScope SelectedSubScope
        {
            get { return (TECSubScope)GetValue(SelectedSubScopeProperty); }
            set { SetValue(SelectedSubScopeProperty, value); }
        }

        public static readonly DependencyProperty SelectedSubScopeProperty =
            DependencyProperty.Register("SelectedSubScope", typeof(TECSubScope),
                typeof(SubScopeHierarchyView), new FrameworkPropertyMetadata(default(TECSubScope))
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
                typeof(SubScopeHierarchyView), new FrameworkPropertyMetadata(default(TECDevice))
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
              typeof(SubScopeHierarchyView));

        public SubScopeHierarchyVM ViewModel
        {
            get { return (SubScopeHierarchyVM)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(SubScopeHierarchyVM), typeof(SubScopeHierarchyView), new PropertyMetadata(default(SystemHierarchyVM)));


        public SubScopeHierarchyView()
        {
            InitializeComponent();
            SizeChanged += handleSizeChanged;

        }

        private void handleSizeChanged(object sender, SizeChangedEventArgs e)
        {
            
            if (e.HeightChanged)
            {
                if (ModalHeight != 0.0)
                {
                    ModalHeight = e.NewSize.Height;
                }
            }
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
    }
}
