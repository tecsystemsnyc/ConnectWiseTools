using System;
using System.Windows;
using System.Windows.Controls;
using GongSolutions.Wpf.DragDrop;

namespace TECUserControlLibrary.UserControls
{
    /// <summary>
    /// Interaction logic for ScopeItemsControl.xaml
    /// </summary>
    public partial class ScopeItemsControl : UserControl
    {
        #region DPs

        public Object ScopeSource
        {
            get { return (Object)GetValue(ScopeSourceProperty); }
            set { SetValue(ScopeSourceProperty, value); }
        }

        public static readonly DependencyProperty ScopeSourceProperty =
            DependencyProperty.Register("ScopeSource", typeof(Object),
              typeof(ScopeItemsControl));
        
        public bool CanDrop
        {
            get { return (bool)GetValue(CanDropProperty); }
            set { SetValue(CanDropProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CanDrop.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CanDropProperty =
            DependencyProperty.Register("CanDrop", typeof(bool), typeof(ScopeItemsControl), new PropertyMetadata(false));


        public IDropTarget DropHandler
        {
            get { return (IDropTarget)GetValue(DropHandlerProperty); }
            set { SetValue(DropHandlerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DropHandler.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DropHandlerProperty =
            DependencyProperty.Register("DropHandler", typeof(IDropTarget), typeof(ScopeItemsControl));
        
        #endregion

        public ScopeItemsControl()
        {
            InitializeComponent();
        }
    }
}
