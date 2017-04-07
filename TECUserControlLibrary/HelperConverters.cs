
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using System.Linq;
using System.Text;
using TECUserControlLibrary;
using EstimatingLibrary;
using EstimatingUtilitiesLibrary;
using System.Globalization;
using System.IO;
using System.Drawing.Imaging;
using System.Windows.Controls;
using System.Drawing;
using System.Windows.Media;
using TECUserControlLibrary.UserControls;
using System.Collections.ObjectModel;

namespace TECUserControlLibrary.HelperConverters
{
    [System.Windows.Markup.MarkupExtensionReturnType(typeof(IValueConverter))]

    public class TemplatesVisibilityConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((Visibility)value == Visibility.Visible)
            {
                return 200;
            }
            else if ((Visibility)value == Visibility.Hidden)
            {
                return 0;
            }
            else
            {
                return 200;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class VisbilityToBooleanConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (Visibility)value == Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        #endregion
    }

    public class BooleanToVisibilityConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NullReferenceException();
        }

        #endregion
    }

    public class VisbilityToContentConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((Visibility)value == Visibility.Visible)
            {
                return "-";
            } else
            {
                return "+";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class BudgetPriceConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            
            if (((double)value) >= 0)
            {
                return value;
            }
            else
            {
                return "None";
            }
        }

        
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string inString = (string)value;
            inString = inString.Trim(new Char[] { ' ','$', ',', '.' });

            return inString.ToDouble(-1);
        }

        #endregion
    }

    public class PercentageToNumberConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
        
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string inString = (string)value;
            inString = inString.Trim(new Char[] { '%' });

            return inString.ToDouble(0);
        }

        #endregion
    }

    public class ManufacturerConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            
            return value;

        }
        
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
             return value;
        }

        #endregion
    }

    public class SelectedLocationToLocationConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                var location = new TECLocation();
                location.Name = "None";
                return location;
            }
            else
            {
                return value;
            }
        }
        
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return value;
            }
            else
            {
                if (((TECLocation)value).Name == "None")
                {
                    return null;
                }
                else
                {
                    return value;
                }
            }
        }

        #endregion
    }

    public class IgnoreNewItemPlaceholderConverter : BaseConverter, IValueConverter
    {
        public static readonly IgnoreNewItemPlaceholderConverter Instance = new IgnoreNewItemPlaceholderConverter();

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && value.ToString() == "{NewItemPlaceholder}")
                return DependencyProperty.UnsetValue;
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class IgnoreNewItemPlaceholderToNullConverter : BaseConverter, IValueConverter
    {
        public static readonly IgnoreNewItemPlaceholderConverter Instance = new IgnoreNewItemPlaceholderConverter();

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && value.ToString() == "{NewItemPlaceholder}")
                return null;
            else
                return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
    
    public class PercentageConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string percentageStr = (Math.Round((double)value * 100)).ToString();
            return percentageStr + "%";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class CountVisibilityConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //if((int)value == 0)
            //{
            //    return Visibility.Collapsed;
            //} else
            //{
            //    return Visibility.Visible;
            //}
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class NullToVisibilityConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if(value == null)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class NullToBoolConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Console.WriteLine("Converting");
            if(value == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class SelectedItemToControllerConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                var controller = new TECController();
                controller.Name = "None";
                return controller;
            }
            else
            {
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return value;
            }
            else
            {
                if (((TECController)value).Name == "None")
                {
                    return null;
                }
                else
                {
                    return value;
                }
            }
        }
        #endregion
    }
    public class SelectedItemToPanelConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                var panel = new TECPanel();
                panel.Name = "None";
                return panel;
            }
            else
            {
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return value;
            }
            else
            {
                if (((TECPanel)value).Name == "None")
                {
                    return null;
                }
                else
                {
                    return value;
                }
            }
        }

        #endregion
    }
    public class SelectedItemToConduitTypeConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                var conduitType = new TECConduitType();
                conduitType.Name = "None";
                return conduitType;
            }
            else
            {
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return value;
            }
            else
            {
                if (((TECConduitType)value).Name == "None")
                {
                    return null;
                }
                else
                {
                    return value;
                }
            }
        }
        #endregion
    }

    public class ConnectionIOConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<IOType> val = (List<IOType>)value;
            if (val.Count > 0)
            {
                return val[0];
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<IOType> io = new List<IOType>();
            io.Add((IOType)value);
            return io;
        }

        #endregion
    }

    public class IOTypeConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ObservableCollection<IOType>)
            {
                ObservableCollection<IOType> ioTypes = value as ObservableCollection<IOType>;
                ObservableCollection<string> stringTypes = new ObservableCollection<string>();

                foreach (IOType type in ioTypes)
                {
                    stringTypes.Add(TECIO.convertTypeToString(type));
                }

                return stringTypes;
            }
            else if (value is IOType)
            {
                return (TECIO.convertTypeToString((IOType)value));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                return (TECIO.convertStringToType((string)value));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
    public class SelectedItemToIOModuleConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                var module = new TECIOModule();
                module.Name = "None";
                return module;
            }
            else
            {
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if(value == null)
            {
                return value;
            }
            else
            {
                if (((TECIOModule)value).Name == "None")
                {
                    return null;
                }
                else
                {
                    return value;
                }
            }
            
        }
        #endregion
    }
    public class DataContextToEnableConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if(value.ToString() == "{NewItemPlaceholder}")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();

        }
        #endregion
    }

    #region Enumeration Converters

    public class EditIndexToIntegerConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int outInt = System.Convert.ToInt32(value);
            return outInt;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (EditIndex)value;
        }

        #endregion
    }

    public class GridIndexToIntegerConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int outInt = System.Convert.ToInt32(value);
            return outInt;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (GridIndex)value;
        }

        #endregion
    }

    public class TemplateGridIndexToIntegerConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int outInt = System.Convert.ToInt32(value);
            return outInt;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (TemplateGridIndex)value;
        }

        #endregion
    }

    public class ScopeCollectionIndexToIntegerConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int outInt = System.Convert.ToInt32(value);
            return outInt;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (ScopeCollectionIndex)value;
        }

        #endregion
    }

    public class LocationScopeTypeToIntegerConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int outInt = System.Convert.ToInt32(value);
            return outInt;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (LocationScopeType)value;
        }

        #endregion
    }

    #endregion
}
