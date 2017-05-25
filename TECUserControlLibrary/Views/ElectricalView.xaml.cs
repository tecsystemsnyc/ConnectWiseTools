using System;
using System.Windows;
using System.Windows.Controls;
using TECUserControlLibrary.ViewModels;

namespace TECUserControlLibrary.Views
{
    /// <summary>
    /// Description for ElectricalView.
    /// </summary>
    public partial class ElectricalView : UserControl
    {
        /// <summary>
        /// Gets or sets the ViewModel which is used
        /// </summary>
        public ElectricalVM ViewModel
        {
            get { return (ElectricalVM)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// Identified the ViewModel dependency property
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(ElectricalVM),
              typeof(ElectricalView));

        /// <summary>
        /// Initializes a new instance of the ElectricalView class.
        /// </summary>
        public ElectricalView()
        {
            InitializeComponent();
        }
    }
}