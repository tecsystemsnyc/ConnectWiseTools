using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DebugLibrary
{
    public static class DebugHandler
    {
        //Bool representing whether or not the program has been deployed.
        public static bool isReleased
        {
            get
            {
                return ApplicationDeployment.IsNetworkDeployed;
            }
        }

        private const bool debugCreatesLog = false;

        //The folder inside of AppData where the log folder hierarchy will be stored.
        private const string APPDATA_FOLDER = @"TECSystems\Logs\";

        private static string logPath;

        public static void LogDebugMessage(string message, bool doLog = true)
        {
            if (doLog)
            {
                if (isReleased)
                {
                    addToLog(message);
                }
                else
                {
                    Console.WriteLine(message);
                    if (debugCreatesLog)
                    {
                        addToLog(message);
                    }
                }
            }
        }

        public static void LogError(string error, bool doLog = true)
        {
            if (doLog)
            {
                if (isReleased)
                {
                    addToLog(error);
                    MessageBox.Show(error);
                }
                else
                {
                    Console.WriteLine(error);
                    if (debugCreatesLog)
                    {
                        addToLog(error);
                    }
                }
            }
        }

        public static void LogError(Exception e, bool doLog = true)
        {
            LogError(e.Message, doLog);
        }

        private static void addToLog(string message)
            //Adds a line to the current log file. File will be stored in Appdata\TECSystems\Logs using the date folder hieararcy and the time of the first message as the file name. 
        {
            if (logPath == null)
                //If the logFile doesn't exist yet, create a new one in the proper date hierarchy folder and the current time as the file name.
            {
                logPath = createLogPath();
                File.Create(logPath);
            }

            using (StreamWriter writer = new StreamWriter(logPath, true))
            {
                writer.WriteLine(message);
            }
        }

        private static string createLogPath()
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string logFolder = Path.Combine(appData, APPDATA_FOLDER);

            DateTime date = DateTime.Now;
            CultureInfo culture = CultureInfo.CreateSpecificCulture("ja-JP");
            DateTimeFormatInfo dtfi = culture.DateTimeFormat;
            dtfi.DateSeparator = "\\";

            logFolder += date.ToString("d", dtfi);

            if (!Directory.Exists(logFolder)) { Directory.CreateDirectory(logFolder); }

            culture = CultureInfo.CreateSpecificCulture("hr-HR");
            dtfi = culture.DateTimeFormat;
            dtfi.TimeSeparator = "-";

            string logFileName = "Log-";
            logFileName += date.ToString("T", dtfi);
            return Path.Combine(logFolder, logFileName);
        }
    }
}
