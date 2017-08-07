using EstimatingLibrary;
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
              typeof(string), new PropertyMetadata("Label"));

        #endregion

        public LabeledGridControl()
        {
            InitializeComponent();
        }
    }
}
