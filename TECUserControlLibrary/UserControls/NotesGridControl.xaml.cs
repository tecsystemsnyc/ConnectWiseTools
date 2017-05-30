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

namespace TECUserControlLibrary.UserControls
{
    /// <summary>
    /// Interaction logic for NotesGridControl.xaml
    /// </summary>
    public partial class NotesGridControl : UserControl
    {

        public ObservableCollection<TECNote> NotesSource
        {
            get { return (ObservableCollection<TECNote>)GetValue(NotesSourceProperty); }
            set { SetValue(NotesSourceProperty, value); }
        }

        /// <summary>
        /// Identified the SystemSource dependency property
        /// </summary>
        public static readonly DependencyProperty NotesSourceProperty =
            DependencyProperty.Register("NotesSource", typeof(ObservableCollection<TECNote>),
              typeof(NotesGridControl), new PropertyMetadata(default(ObservableCollection<TECNote>)));

        public NotesGridControl()
        {
            InitializeComponent();
        }
    }
}
