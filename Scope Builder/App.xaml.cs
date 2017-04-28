using DebugLibrary;
using System.Windows;
using GalaSoft.MvvmLight.Threading;
using System.Windows.Controls;
using System.Deployment.Application;
using System;

namespace Scope_Builder
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
        protected override void OnStartup(StartupEventArgs args)
        {
            // Check if this was launched by double-clicking a doc. If so, use that as the
            // startup file name.

            if (ApplicationDeployment.IsNetworkDeployed)
            {
                if (AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData != null && AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData.Length > 0)
                {
                    string fname = "No filename given";
                    try
                    {
                        fname = AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData[0];
                        // It comes in as a URI; this helps to convert it to a path.
                        Uri uri = new Uri(fname);
                        fname = uri.LocalPath;

                        Scope_Builder.Properties.Settings.Default.StartupFile = fname;
                    }
                    catch (Exception e)
                    {
                        DebugHandler.LogError("Could not open startup file. Exception: " + e.Message);
                    }
                }
            }
            
            base.OnStartup(args);

        }
        
    }
}
