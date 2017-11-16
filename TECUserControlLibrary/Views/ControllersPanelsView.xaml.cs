using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using TECUserControlLibrary.ViewModels;

namespace TECUserControlLibrary.Views
{
    /// <summary>
    /// Interaction logic for ControllersPanelsView.xaml
    /// </summary>
    public partial class ControllersPanelsView : UserControl
    {
        public double ModalHeight
        {
            get { return (double)GetValue(ModalHeightProperty); }
            set { SetValue(ModalHeightProperty, value); }
        }

        public static readonly DependencyProperty ModalHeightProperty =
           DependencyProperty.Register("ModalHeight", typeof(double),
             typeof(ControllersPanelsView), new PropertyMetadata(1.0));


        public ControllersPanelsVM ViewModel
        {
            get { return (ControllersPanelsVM)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(ControllersPanelsVM),
              typeof(ControllersPanelsView));

        public ControllersPanelsView()
        {
            InitializeComponent();
            SizeChanged += handleSizeChanged;
        }

        private void handleSizeChanged(object sender, SizeChangedEventArgs e)
        {
           
            if (e.HeightChanged)
            {
                if (ModalHeight != 0.0)
                {
                    ModalHeight = e.NewSize.Height;
                }
            }
        }

        private void Add_Clicked(object sender, RoutedEventArgs e)
        {
            Storyboard moveBack = (Storyboard)FindResource("modalIn");
            moveBack.Begin();
        }

        private void modalOut_Completed(object sender, EventArgs e)
        {
            ModalHeight = this.ActualHeight;
        }

        private void modalIn_Completed(object sender, EventArgs e)
        {
            ModalHeight = 0;
        }
    }
}
