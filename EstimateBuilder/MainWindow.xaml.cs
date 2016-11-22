using System.Windows;
using EstimateBuilder.ViewModel;
using System.ComponentModel;

namespace EstimateBuilder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();
        }

        void Window_Closing(object sender, CancelEventArgs e)
        {
            string message = "Are you sure you want to quit? There could be unsaved changes.";
            MessageBoxResult result = MessageBox.Show(message, "Really quit?", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                Properties.Settings.Default.Save();
            }
        }
    }
}