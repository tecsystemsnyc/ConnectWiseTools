using EstimatingLibrary;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TECUserControlLibrary.Views
{
    /// <summary>
    /// Interaction logic for ParamatersView.xaml
    /// </summary>
    public partial class ParametersView : UserControl
    {

        public IEnumerable<TECParameters> Source
        {
            get { return (IEnumerable<TECParameters>)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Source.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(IEnumerable<TECParameters>), typeof(ParametersView));



        public ICommand AddCommand
        {
            get { return (ICommand)GetValue(AddCommandProperty); }
            set { SetValue(AddCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AddCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AddCommandProperty =
            DependencyProperty.Register("AddCommand", typeof(ICommand), typeof(ParametersView));



        public ParametersView()
        {
            InitializeComponent();
        }
    }
}
