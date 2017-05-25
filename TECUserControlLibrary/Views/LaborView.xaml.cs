using System;
using System.Windows;
using System.Windows.Controls;
using TECUserControlLibrary.ViewModels;

namespace TECUserControlLibrary.Views
{
    /// <summary>
    /// Description for LaborView.
    /// </summary>
    public partial class LaborView : UserControl
    {

        /// <summary>
        /// Gets or sets the ViewModel which is used
        /// </summary>
        public LaborVM ViewModel
        {
            get { return (LaborVM)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// Identified the ViewModel dependency property
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(LaborVM),
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