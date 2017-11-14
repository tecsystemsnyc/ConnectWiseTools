using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Windows.Media.Imaging;

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

        public static BitmapImage ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destBitmap = new Bitmap(width, height);

            destBitmap.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destBitmap))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }
            MemoryStream ms = new MemoryStream();
            destBitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            ms.Position = 0;
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = ms;
            bi.EndInit();
            bi.Freeze();

            destBitmap.Dispose();

            return bi;
        }

        #region Cast Conversions

        #region String Extensions

        public static int ToInt(this string str, int? def = null)
        {
            int i;
            if (!int.TryParse(str, out i))
            {
                if (def != null)
                {
                    return (def ?? default(int));
                }
                else
                {
                    throw new InvalidCastException("StringToInt() failed. String: " + str);
                }
            }
            else
            {
                return i;
            }
        }

        public static double ToDouble(this string str, double? def = null)
        {
            double d;
            if (!double.TryParse(str, out d))
            {
                if (def != null)
                {
                    return (def ?? default(double));
                }
                else
                {
                    throw new InvalidCastException("StringToDouble() failed. String: " + str);
                }
            }
            else
            {
                return d;
            }
        }

        #endregion String Extensions

        public static int ToInt(this bool b)
        {
            if (b) { return 1; }
            else { return 0; }
        }

        public static bool ToBool(this int i)
        {
            if (i == 1) { return true; }
            else if (i == 0) { return false; }
            else { throw new InvalidCastException("Int to Bool cast failed. Int: " + i); }
        }

        #region Enum Conversions
        public static T StringToEnum<T>(string str)
        {
            return (T)Enum.Parse(typeof(T), str);
        }
        public static T StringToEnum<T>(string str, T def)
        {
            try
            {
                return (T)Enum.Parse(typeof(T), str);
            }
            catch
            {
                return def;
            }
        }
        #endregion

        #endregion Cast Conversions

        public static IEnumerable GetChildCollection(Type childType, object parentObject, Type parentType = null)
        {
            if(parentType == null)
            {
                parentType = parentObject.GetType();
            }
            foreach (PropertyInfo info in parentType.GetProperties())
            {
                if (info.GetGetMethod() != null && info.PropertyType == typeof(ObservableCollection<>).MakeGenericType(new[] { childType }))
                    return parentObject.GetType().GetProperty(info.Name).GetValue(parentObject, null) as IEnumerable;
            }
            return null;
        }
        
        public static bool IsLowerVersion(string currentVersion, string sampleVersion)
        {
            var isLowerVersion = false;
            char delimiter = '.';
            var currentStrings = currentVersion.Split(delimiter);
            var sampleStrings = sampleVersion.Split(delimiter);
            if (sampleStrings.Length != 4)
            {
                return true;
            }
            else if (currentStrings[0].ToInt() > sampleStrings[0].ToInt())
            {
                return true;
            }
            else if (currentStrings[0].ToInt() == sampleStrings[0].ToInt())
            {
                if (currentStrings[1].ToInt() > sampleStrings[1].ToInt())
                {
                    return true;
                }
                else if (currentStrings[1].ToInt() == sampleStrings[1].ToInt())
                {
                    if (currentStrings[2].ToInt() > sampleStrings[2].ToInt())
                    {
                        return true;
                    }
                    else if (currentStrings[2].ToInt() == sampleStrings[2].ToInt())
                    {
                        if (currentStrings[3].ToInt() > sampleStrings[3].ToInt())
                        {
                            return true;
                        }
                    }

                }
            }

            return isLowerVersion;
        }
        public static bool StringContainsStrings(string reference, string[] criteria)
        {
            bool containsAll = true;
            foreach (string critereon in criteria)
            {
                if (!reference.Contains(critereon))
                {
                    return false;
                }
            }
            return containsAll;
        }

    }
}
