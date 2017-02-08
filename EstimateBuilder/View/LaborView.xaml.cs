using System;
using System.Windows;
using System.Windows.Controls;

namespace EstimateBuilder.View
{
    /// <summary>
    /// Description for LaborView.
    /// </summary>
    public partial class LaborView : UserControl
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
              typeof(LaborView));

        /// <summary>
        /// Initializes a new instance of the LaborView class.
        /// </summary>
        public LaborView()
        {
            InitializeComponent();
        }
    }
}