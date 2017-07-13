using System;
using System.Windows;
using System.Windows.Controls;
using TECUserControlLibrary.ViewModels;

namespace TECUserControlLibrary.Views
{
    /// <summary>
    /// Description for ReviewView.
    /// </summary>
    public partial class ReviewView : UserControl
    {
        /// <summary>
        /// Gets or sets the ViewModel which is used
        /// </summary>
        public ReviewVM ViewModel
        {
            get { return (ReviewVM)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// Identified the ViewModel dependency property
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(ReviewVM),
              typeof(ReviewView));

        /// <summary>
        /// Initializes a new instance of the ReviewView class.
        /// </summary>
        public ReviewView()
        {
            InitializeComponent();
        }
    }
}