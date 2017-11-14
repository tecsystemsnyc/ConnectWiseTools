using System;
using System.Windows;
using System.Windows.Controls;

namespace TECUserControlLibrary.UserControls
{
    /// <summary>
    /// Interaction logic for ScopeItemsControl.xaml
    /// </summary>
    public partial class ScopeItemsControl : UserControl
    {
        #region DPs

        public Object ScopeSource
        {
            get { return (Object)GetValue(ScopeSourceProperty); }
            set { SetValue(ScopeSourceProperty, value); }
        }

        public static readonly DependencyProperty ScopeSourceProperty =
            DependencyProperty.Register("ScopeSource", typeof(Object),
              typeof(ScopeItemsControl));


        #endregion

        public ScopeItemsControl()
        {
            InitializeComponent();
        }
    }
}
