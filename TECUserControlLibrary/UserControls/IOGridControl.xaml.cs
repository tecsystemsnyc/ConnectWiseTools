using EstimatingLibrary;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace TECUserControlLibrary.UserControls
{
    /// <summary>
    /// Interaction logic for IOGridControl.xaml
    /// </summary>
    public partial class IOGridControl : UserControl
    {
        /// <summary>
        /// Gets or sets the SystemSource which is displayed
        /// </summary>
        public ObservableCollection<TECIO> IOSource
        {
            get { return (ObservableCollection<TECIO>)GetValue(IOSourceProperty); }
            set { SetValue(IOSourceProperty, value); }
        }

        /// <summary>
        /// Identified the SystemSource dependency property
        /// </summary>
        public static readonly DependencyProperty IOSourceProperty =
            DependencyProperty.Register("IOSource", typeof(ObservableCollection<TECIO>),
              typeof(IOGridControl), new PropertyMetadata(default(ObservableCollection<TECIO>)));

        /// <summary>
        /// Gets or sets the ViewModel which is used
        /// </summary>
        public Object ViewModel
        {
            get { return (Object)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// Identified the ViewModel dependency property
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(Object),
              typeof(IOGridControl));

        public IOGridControl()
        {
            InitializeComponent();
        }
    }
}
