using GalaSoft.MvvmLight.Threading;
using NLog;
using System;
using System.Windows;

namespace EstimateBuilder
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static Logger logger = LogManager.GetCurrentClassLogger();

        public App() : base()
        {
            this.Dispatcher.UnhandledException += logUnhandledException;
            DispatcherHelper.Initialize();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            logger.Debug("Estimate Builder starting up.");

            if (e.Args.Length > 0)
            {
                string startUpFilePath = e.Args[0];
                logger.Debug("StartUp file path: {0}", startUpFilePath);
                EstimateBuilder.Properties.Settings.Default.StartUpFilePath = startUpFilePath;
                EstimateBuilder.Properties.Settings.Default.Save();
            }
            else
            {
                logger.Debug("No startup arguments passed.");
            }
            base.OnStartup(e);
        }

        private void logUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            logger.Fatal("Unhandled exception: {0}", e.Exception.Message);
            logger.Fatal("Inner exception: {0}", e.Exception.InnerException.Message);
            logger.Fatal("Stack trace: {0}", e.Exception.StackTrace);
            MessageBox.Show("Fatal error occured, view logs for more information.", "Fatal Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            System.Environment.Exit(0);
        }
    }
}
