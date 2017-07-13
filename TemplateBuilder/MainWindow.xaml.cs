using System.ComponentModel;
using System.Windows;
using TemplateBuilder.MVVM;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight;

namespace TemplateBuilder
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
    }
}