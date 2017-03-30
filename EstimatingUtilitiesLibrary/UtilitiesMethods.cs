using EstimatingLibrary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using System.Reflection;

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

        public static double getLength(TECVisualScope scope1, TECVisualScope scope2, double scale)
        {
            var length = Math.Pow((Math.Pow((scope1.X - scope2.X), 2) + Math.Pow((scope1.Y - scope2.Y), 2)), 0.5) * scale;

            return length;
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

        public static object GetChildCollection(object childObject, object parentObject)
        {
            Type childType = childObject.GetType();

            foreach (PropertyInfo info in parentObject.GetType().GetProperties())
            {
                if (info.GetGetMethod() != null && info.PropertyType == typeof(ObservableCollection<>).MakeGenericType(new[] { childType }))
                    return parentObject.GetType().GetProperty(info.Name).GetValue(parentObject, null);
            }
            return null;
        }

        public static void AddCatalogsToBid(TECBid bid, TECTemplates templates)
        {
            bid.DeviceCatalog = templates.DeviceCatalog;
            bid.ManufacturerCatalog = templates.ManufacturerCatalog;
            bid.PanelTypeCatalog = templates.PanelTypeCatalog;
            bid.ConduitTypes = templates.ConduitTypeCatalog;
            bid.ConnectionTypes = templates.ConnectionTypeCatalog;
            bid.Tags = templates.Tags;
            bid.IOModuleCatalog = templates.IOModuleCatalog;
            bid.AssociatedCostsCatalog = templates.AssociatedCostsCatalog;
        }

    }

    public enum EditIndex { System, Equipment, SubScope, Device, Point, Controller, Panel, Nothing };
    public enum GridIndex { Scope = 1, DDC, Location, Proposal, Budget };
    public enum TemplateGridIndex { None, Systems, Equipment, SubScope, Devices, DDC, Materials, Constants, ControlledScope };
    public enum ScopeCollectionIndex { None, System, Equipment, SubScope, Devices, Tags, Manufacturers, AddDevices, AddControllers, Controllers, AssociatedCosts, ControlledScope, Panels, AddPanel };
    public enum LocationScopeType { System, Equipment, SubScope};
    public enum MaterialType { Wiring, Conduit, PanelTypes, AssociatedCosts, IOModules};
}
