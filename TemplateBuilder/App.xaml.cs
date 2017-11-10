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

            // Check if this was launched by double-clicking a doc. If so, use that as the
            // startup file name.
            if (AppDomain.CurrentDomain.SetupInformation
                .ActivationArguments?.ActivationData != null
            && AppDomain.CurrentDomain.SetupInformation
                .ActivationArguments.ActivationData.Length > 0)
            {
                string fname = "No filename given";
                try
                {
                    fname = AppDomain.CurrentDomain.SetupInformation
             .ActivationArguments.ActivationData[0];

                    // It comes in as a URI; this helps to convert it to a path.
                    Uri uri = new Uri(fname);
                    fname = uri.LocalPath;

                    TemplateBuilder.Properties.Settings.Default.StartUpFilePath = fname;
                    TemplateBuilder.Properties.Settings.Default.Save();
                }
                catch (Exception ex)
                {
                    // For some reason, this couldn't be read as a URI.
                    logger.Error(ex, "StartUp file could not be read.");
                    string message = "File could not be read by Template Builder.";
                    MessageBox.Show(message);
                    return;
                }
            }
            else
            {
                logger.Debug("No activation arguments passed.");
            }

            base.OnStartup(e);
        }

        private void logUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            logger.Fatal("Unhandled exception: {0}", e.Exception.Message);
            logger.Fatal("Stack trace: {0}", e.Exception.StackTrace);
            MessageBox.Show("Fatal error occured, view logs for more information.", "Fatal Error!", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
