using System.Windows;
using System.Windows.Controls;
using TECUserControlLibrary.ViewModels;

namespace TECUserControlLibrary.Views
{
    /// <summary>
    /// Interaction logic for SplashView.xaml
    /// </summary>
    public partial class SplashView : UserControl
    {
        /// <summary>
        /// Gets or sets the ViewModel which is used
        /// </summary>
        public SplashVM ViewModel
        {
            get { return (SplashVM)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// Identified the ViewModel dependency property
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(SplashVM),
              typeof(SplashView));



        public Visibility BidVisibility
        {
            get { return (Visibility)GetValue(BidVisibilityProperty); }
            set { SetValue(BidVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BidVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BidVisibilityProperty =
            DependencyProperty.Register("BidVisibility", typeof(Visibility), typeof(SplashView));




        public SplashView()
        {
            InitializeComponent();
        }
    }
}
