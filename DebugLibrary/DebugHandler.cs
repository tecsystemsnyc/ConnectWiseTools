using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebugLibrary
{
    public static class DebugHandler
    {
        private const bool DEBUG_ALL = true;

        private const string APPDATA_FOLDER = @"TECSystems\Logs\";

        private static bool isReleased
        {
            get
            {
                return ApplicationDeployment.IsNetworkDeployed;
            }
        }
        private static string logFile;

        public static void LogDebugMessage(string message)
        {

        }

        public static void LogError(string error)
        {

        }

        public static void LogException(Exception e)
        {

        }

        private static void addToLog(string message)
        {
            if (logFile == null)
            {
                string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string logFolder = Path.Combine(appData, APPDATA_FOLDER);
            }
        }
    }
}
