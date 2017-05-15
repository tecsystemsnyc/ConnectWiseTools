using System;
using System.Windows;
using System.Windows.Controls;

namespace EstimateBuilder.View.Review
{
    /// <summary>
    /// Description for ReviewView.
    /// </summary>
    public partial class ReviewView : UserControl
    {
        /// <summary>
        /// Gets or sets the ViewModel which is used
        /// </summary>
        public Object ViewModel
        {
            get { return (Object)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// Identified the ViewModel dependency property
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(Object),
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