using System;
using System.Windows;
using System.Windows.Controls;

namespace TECUserControlLibrary
{
    /// <summary>
    /// Interaction logic for ScopeControl.xaml
    /// </summary>
    public partial class ScopeControl : UserControl
    {
        #region DPs

        /// <summary>
        /// Gets or sets the Name Label which is displayed
        /// </summary>
        public String NameLabel
        {
            get { return (String)GetValue(NameLabelProperty); }
            set { SetValue(NameLabelProperty, value); }
        }

        /// <summary>
        /// Identified the Name Label dependency property
        /// </summary>
        public static readonly DependencyProperty NameLabelProperty =
            DependencyProperty.Register("NameLabel", typeof(String),
              typeof(ScopeControl), new PropertyMetadata(""));

        /// <summary>
        /// Gets or sets the Description Label which is displayed
        /// </summary>
        public String DescriptionLabel
        {
            get { return (String)GetValue(DescriptionLabelProperty); }
            set { SetValue(DescriptionLabelProperty, value); }
        }

        /// <summary>
        /// Identified the Description Label dependency property
        /// </summary>
        public static readonly DependencyProperty DescriptionLabelProperty =
            DependencyProperty.Register("DescriptionLabel", typeof(String),
              typeof(ScopeControl), new PropertyMetadata(""));

        #endregion
        public ScopeControl()
        {
            InitializeComponent();
        }
    }
}
