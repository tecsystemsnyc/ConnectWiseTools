using EstimatingLibrary;
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

        #endregion

        public LabeledGridControl()
        {
            InitializeComponent();
        }
    }
}
