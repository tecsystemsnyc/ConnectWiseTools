using EstimatingLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TECUserControlLibrary
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
