using System;
using System.Collections.Generic;
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
using EstimatingUtilitiesLibrary;
using EstimatingLibrary;
using TECUserControlLibrary.ViewModels;

namespace TECUserControlLibrary.Views
{
    /// <summary>
    /// Interaction logic for EditTabControl.xaml
    /// </summary>
    public partial class EditTabView : UserControl
    {
        #region DPs

        /// <summary>
        /// Gets or sets the ViewModel which is used
        /// </summary>
        public EditTabVM ViewModel
        {
            get { return (EditTabVM)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// Identified the ViewModel dependency property
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(EditTabVM),
              typeof(EditTabView));

        /// <summary>
        /// Gets or sets the tab index which is displayed
        /// </summary>
        public EditIndex Index
        {
            get { return (EditIndex)GetValue(IndexProperty); }
            set { SetValue(IndexProperty, value); }
        }

        /// <summary>
        /// Identified the DevicesSource dependency property
        /// </summary>
        public static readonly DependencyProperty IndexProperty =
            DependencyProperty.Register("Index", typeof(EditIndex),
              typeof(EditTabView), new PropertyMetadata(default(EditIndex)));
        #endregion
        public EditTabView()
        {
            InitializeComponent();
        }
    }
}
