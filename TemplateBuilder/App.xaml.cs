using GalaSoft.MvvmLight.Threading;
using System.Windows;

namespace TemplateBuilder
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static App()
        {
            DispatcherHelper.Initialize();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;

            // Check if this was launched by double-clicking a doc. If so, use that as the
            // startup file name.

            //if (ApplicationDeployment.IsNetworkDeployed)
            //{
            //    if (AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData != null && AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData.Length > 0)
            //    {
            //        string fname = "No filename given";
            //        try
            //        {
            //            fname = AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData[0];
            //            // It comes in as a URI; this helps to convert it to a path.
            //            Uri uri = new Uri(fname);
            //            fname = uri.LocalPath;

            //            TemplateBuilder.Properties.Settings.Default.StartupFile = fname;
            //        }
            //        catch (Exception exc)
            //        {
            //            logger.Error("Could not open startup file. Exception: " + exc.Message);
            //        }
            //    }
            //}

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
