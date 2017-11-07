using EstimatingLibrary;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace TECUserControlLibrary.UserControls
{
    /// <summary>
    /// Interaction logic for ScopeBranchAsNoteGridControl.xaml
    /// </summary>
    public partial class ScopeBranchAsNoteGridControl : UserControl
    {
        public ObservableCollection<TECScopeBranch> ScopeTreeSource
        {
            get { return (ObservableCollection<TECScopeBranch>)GetValue(ScopeTreeSourceProperty); }
            set { SetValue(ScopeTreeSourceProperty, value); }
        }

        /// <summary>
        /// Identified the SystemSource dependency property
        /// </summary>
        public static readonly DependencyProperty ScopeTreeSourceProperty =
            DependencyProperty.Register("ScopeTreeSource", typeof(ObservableCollection<TECScopeBranch>),
              typeof(ScopeBranchAsNoteGridControl), new PropertyMetadata(default(ObservableCollection<TECScopeBranch>)));

        public ScopeBranchAsNoteGridControl()
        {
            InitializeComponent();
        }
    }
}
