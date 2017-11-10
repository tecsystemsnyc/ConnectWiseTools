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

        static App()
        {
            DispatcherHelper.Initialize();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;

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

                    EstimateBuilder.Properties.Settings.Default.StartUpFilePath = fname;
                }
                catch (Exception ex)
                {
                    // For some reason, this couldn't be read as a URI.
                    logger.Error(ex, "StartUp file could not be read.");
                    string message = "File could not be read by Estimate Builder.";
                    MessageBox.Show(message);
                    return;
                }
            }

            base.OnStartup(e);
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            //if (DebugHandler.isReleased)
            //{
            //    logger.Error("Exception: " + e.Exception);
            //    logger.Error("Inner Exception: " + e.Exception.InnerException);
            //    logger.Error("Stack Trace: " + e.Exception.StackTrace);
            //    e.Handled = true;
            //}
        }
    }
}
