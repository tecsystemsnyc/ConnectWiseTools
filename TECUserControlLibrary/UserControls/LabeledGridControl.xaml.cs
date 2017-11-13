using EstimatingLibrary;
using GongSolutions.Wpf.DragDrop;
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

        /// <summary>
        /// Gets or sets the DevicesSource which is displayed
        /// </summary>
        public ObservableCollection<TECLabeled> LabeledSource
        {
            get { return (ObservableCollection<TECLabeled>)GetValue(LabeledSourceProperty); }
            set { SetValue(LabeledSourceProperty, value); }
        }

        /// <summary>
        /// Identified the DevicesSource dependency property
        /// </summary>
        public static readonly DependencyProperty LabeledSourceProperty =
            DependencyProperty.Register("LabeledSource", typeof(ObservableCollection<TECLabeled>),
              typeof(LabeledGridControl), new PropertyMetadata(default(ObservableCollection<TECLabeled>)));


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


        #endregion

        public LabeledGridControl()
        {
            InitializeComponent();
        }
    }
}
