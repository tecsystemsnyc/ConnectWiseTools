using GalaSoft.MvvmLight.Threading;
using NLog;
using System;
using System.Windows;

namespace TemplateBuilder
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
            logger.Debug("Template Builder starting up.");
            if (AppDomain.CurrentDomain.SetupInformation
                .ActivationArguments?.ActivationData != null
                && AppDomain.CurrentDomain.SetupInformation
                .ActivationArguments.ActivationData.Length > 0)
            {
                try
                {
                    string fname = AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData[0];

                    // It comes in as a URI; this helps to convert it to a path.
                    Uri uri = new Uri(fname);
                    string startUpFilePath = uri.LocalPath;
                    logger.Debug("StartUp file path: {0}", startUpFilePath);
                    TemplateBuilder.Properties.Settings.Default.StartUpFilePath = startUpFilePath;
                    TemplateBuilder.Properties.Settings.Default.Save();
                }
                catch (Exception)
                {
                    logger.Error("Couldn't process startup arguments as a path.");
                }
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
