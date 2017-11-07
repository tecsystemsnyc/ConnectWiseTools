using EstimatingLibrary;
using EstimatingLibrary.Interfaces;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using TECUserControlLibrary.ViewModels;

namespace TECUserControlLibrary.Views
{
    /// <summary>
    /// Interaction logic for DevicesPointsControl.xaml
    /// </summary>
    public partial class DevicesPointsView : UserControl
    {
        #region DPs

        /// <summary>
        /// Gets or sets the DevicesSource which is displayed
        /// </summary>
        public ObservableCollection<TECPoint> PointsSource
        {
            get { return (ObservableCollection<TECPoint>)GetValue(PointsSourceProperty); }
            set { SetValue(PointsSourceProperty, value); }
        }

        /// <summary>
        /// Identified the DevicesSource dependency property
        /// </summary>
        public static readonly DependencyProperty PointsSourceProperty =
            DependencyProperty.Register("PointsSource", typeof(ObservableCollection<TECPoint>),
              typeof(DevicesPointsView), new PropertyMetadata(default(ObservableCollection<TECPoint>)));


        /// <summary>
        /// Gets or sets the DevicesSource which is displayed
        /// </summary>
        public ObservableCollection<IEndDevice> DevicesSource
        {
            get { return (ObservableCollection<IEndDevice>)GetValue(DevicesSourceProperty); }
            set { SetValue(DevicesSourceProperty, value); }
        }

        /// <summary>
        /// Identified the DevicesSource dependency property
        /// </summary>
        public static readonly DependencyProperty DevicesSourceProperty =
            DependencyProperty.Register("DevicesSource", typeof(ObservableCollection<IEndDevice>),
              typeof(DevicesPointsView), new PropertyMetadata(default(ObservableCollection<IEndDevice>)));

        /// <summary>
        /// Gets or sets the ViewModel which is used
        /// </summary>
        public DevicesPointsVM ViewModel
        {
            get { return (DevicesPointsVM)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// Identified the ViewModel dependency property
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(DevicesPointsVM),
              typeof(DevicesPointsView));

        #endregion
        public DevicesPointsView()
        {
            InitializeComponent();
        }
    }
}
