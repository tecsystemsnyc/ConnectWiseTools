using EstimateBuilder.ViewModel;
using EstimatingLibrary;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace EstimateBuilder.View
{
    /// <summary>
    /// Description for DrawingView.
    /// </summary>
    public partial class DrawingView : UserControl
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
              typeof(DrawingView));

        /// <summary>
        /// Initializes a new instance of the DrawingView class.
        /// </summary>
        public DrawingView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handle the user dragging the rectangle.
        /// </summary>
        private void Thumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            Thumb thumb = (Thumb)sender;
            TECVisualScope scope = (TECVisualScope)thumb.DataContext;

            // Update the the position of the rectangle in the view-model.

            scope.X += e.HorizontalChange;
            scope.Y += e.VerticalChange;
        }
    }
}