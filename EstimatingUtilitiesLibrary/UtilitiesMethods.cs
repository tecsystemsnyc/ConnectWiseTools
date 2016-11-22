using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingUtilitiesLibrary
{
    public static class UtilitiesMethods
    {
        public static bool IsFileLocked(string filePath)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            if (!File.Exists(filePath))
            {
                return false;
            }

            FileInfo file = new FileInfo(filePath);

            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }

        public static int StringToInt(string str)
        {
            int i;
            if (!int.TryParse(str, out i))
            {
                throw new InvalidCastException("StringToInt() failed. String: " + str);
            }
            else
            {
                return i;
            }
        }

        public static double StringToDouble(string str)
        {
            double d;
            if (!double.TryParse(str, out d))
            {
                throw new InvalidCastException("StringToDouble() failed. String: " + str);
            }
            else
            {
                return d;
            }
        }

        public static string CommaSeparatedString(List<string> strings)
        {
            int i = 0;
            string css = "";
            foreach (string s in strings)
            {
                css += s;
                if (i < strings.Count - 1)
                {
                    css += ", ";
                }
                i++;
            }
            return css;
        }
    }

    public enum EditIndex { System, Equipment, SubScope, Device, Point };
    public enum GridIndex { Systems, Scope, Notes, Exclusions};
    public enum AddIndex { System, Equipment, SubScope, Devices, Tags};
}
