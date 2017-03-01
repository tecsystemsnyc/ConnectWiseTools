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

namespace TECUserControlLibrary
{
    /// <summary>
    /// Interaction logic for TagsGridControl.xaml
    /// </summary>
    public partial class TagsGridControl : UserControl
    {
        #region DPs

        /// <summary>
        /// Gets or sets the DevicesSource which is displayed
        /// </summary>
        public ObservableCollection<TECTag> TagsSource
        {
            get { return (ObservableCollection<TECTag>)GetValue(TagsSourceProperty); }
            set { SetValue(TagsSourceProperty, value); }
        }

        /// <summary>
        /// Identified the DevicesSource dependency property
        /// </summary>
        public static readonly DependencyProperty TagsSourceProperty =
            DependencyProperty.Register("TagsSource", typeof(ObservableCollection<TECTag>),
              typeof(TagsGridControl), new PropertyMetadata(default(ObservableCollection<TECTag>)));

        #endregion

        public TagsGridControl()
        {
                InitializeComponent();
        }
    }
}
