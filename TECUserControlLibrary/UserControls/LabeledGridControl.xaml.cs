using EstimatingLibrary;
using GongSolutions.Wpf.DragDrop;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace TECUserControlLibrary.UserControls
{
    /// <summary>
    /// Interaction logic for LabeledGridControl.xaml
    /// </summary>
    public partial class LabeledGridControl : UserControl
    {
        #region DPs
        
        public IEnumerable<TECLabeled> LabeledSource
        {
            get { return (IEnumerable<TECLabeled>)GetValue(LabeledSourceProperty); }
            set { SetValue(LabeledSourceProperty, value); }
        }
        
        public static readonly DependencyProperty LabeledSourceProperty =
            DependencyProperty.Register("LabeledSource", typeof(IEnumerable<TECLabeled>),
              typeof(LabeledGridControl));
        
        public string LabelName
        {
            get { return (string)GetValue(LabelNameProperty); }
            set { SetValue(LabelNameProperty, value); }
        }
        
        public static readonly DependencyProperty LabelNameProperty =
            DependencyProperty.Register("LabelName", typeof(string),
              typeof(LabeledGridControl), new PropertyMetadata("Label"));



        public IDropTarget DropHandler
        {
            get { return (IDropTarget)GetValue(DropHandlerProperty); }
            set { SetValue(DropHandlerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DropHandler.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DropHandlerProperty =
            DependencyProperty.Register("DropHandler", typeof(IDropTarget), typeof(LabeledGridControl));



        public bool ReadOnly
        {
            get { return (bool)GetValue(ReadOnlyProperty); }
            set { SetValue(ReadOnlyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CanEdit.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ReadOnlyProperty =
            DependencyProperty.Register("ReadOnly", typeof(bool), typeof(LabeledGridControl), new PropertyMetadata(false));




        #endregion

        public LabeledGridControl()
        {
            InitializeComponent();
        }
    }
}
