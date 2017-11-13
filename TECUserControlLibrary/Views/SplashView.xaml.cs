using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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


        public string BidPath
        {
            get { return (string)GetValue(BidPathProperty); }
            set { SetValue(BidPathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BidPath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BidPathProperty =
            DependencyProperty.Register("BidPath", typeof(string), typeof(SplashView), new PropertyMetadata(""));


        public ICommand GetBidPathCommand
        {
            get { return (ICommand)GetValue(GetBidPathCommandProperty); }
            set { SetValue(GetBidPathCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GetBidPathCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GetBidPathCommandProperty =
            DependencyProperty.Register("GetBidPathCommand", typeof(ICommand), typeof(SplashView));





        public SplashView()
        {
            InitializeComponent();
        }
    }
}
