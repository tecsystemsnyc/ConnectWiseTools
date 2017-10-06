using EstimatingLibrary;
using System;
using System.Collections.Generic;
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
using TECUserControlLibrary.ViewModels;

namespace TECUserControlLibrary.Views
{
    /// <summary>
    /// Interaction logic for NetworkView.xaml
    /// </summary>
    public partial class NetworkView : UserControl
    {
        public NetworkVM ViewModel
        {
            get { return (NetworkVM)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(NetworkVM),
                typeof(NetworkView));
        
        public double ConnectableWidth
        {
            get { return (double)GetValue(ConnectableWidthProperty); }
            set
            {
                SetValue(ConnectableWidthProperty, value);
                var width = ConnectableWidth;
                Console.WriteLine("Value to set: " + value + " Actual: " + width);
            }
        }

        // Using a DependencyProperty as the backing store for ChildrenWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ConnectableWidthProperty =
            DependencyProperty.Register("ConnectableWidth", typeof(double), typeof(NetworkView), new PropertyMetadata(0.0));



        public double HalfWidth
        {
            get { return (double)GetValue(HalfWidthProperty); }
            set { SetValue(HalfWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HalfWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HalfWidthProperty =
            DependencyProperty.Register("HalfWidth", typeof(double), typeof(NetworkView), new PropertyMetadata(0.0));



        public TECNetworkConnection SelectedConnection
        {
            get { return (TECNetworkConnection)GetValue(SelectedConnectionProperty); }
            set { SetValue(SelectedConnectionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedConnection.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedConnectionProperty =
            DependencyProperty.Register("SelectedConnection", typeof(TECNetworkConnection), typeof(NetworkView), new PropertyMetadata(default(TECNetworkConnection)));






        public NetworkView()
        {
            InitializeComponent();
            ConnectionsBorder.SizeChanged += (sender, e) =>
            {
                if (e.WidthChanged)
                {
                    HalfWidth = e.NewSize.Width / 2;
                    if (ConnectableWidth != 0)
                    {
                        ConnectableWidth = e.NewSize.Width/2;
                    }
                }
            };
        }

        private void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedConnection = null;
        }
    }
}
