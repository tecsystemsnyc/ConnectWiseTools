using System.Windows;
using System.Windows.Controls;
using TECUserControlLibrary.ViewModels;

namespace TECUserControlLibrary.Views
{
    /// <summary>
    /// Description for ScopeEditorView.
    /// </summary>
    public partial class ScopeEditorView : UserControl
    {
        /// <summary>
        /// Gets or sets the ViewModel which is used
        /// </summary>
        public ScopeEditorVM ViewModel
        {
            get { return (ScopeEditorVM)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// Identified the ViewModel dependency property
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(ScopeEditorVM),
              typeof(ScopeEditorView));

        public double TemplatesWidth
        {
            get { return (double)GetValue(TemplatesWidthProperty); }
            set { SetValue(TemplatesWidthProperty, value); }
        }
        
        public static readonly DependencyProperty TemplatesWidthProperty =
            DependencyProperty.Register("TemplatesWidth", typeof(double),
              typeof(ScopeEditorView), new PropertyMetadata(250.0));

        /// <summary>
        /// Initializes a new instance of the ScopeEditorView class.
        /// </summary>
        public ScopeEditorView()
        {
            InitializeComponent();
        }
    }
}