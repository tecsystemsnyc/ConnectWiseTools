
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
            double returnValue;

            string inString = (string)value;
            inString = inString.Trim(new Char[] { ' ','$', ',', '.' });

            bool parsed = double.TryParse(inString, out returnValue);
            Console.WriteLine("ConvertBack value passed: " + value);
            if (!parsed)
            {
                Console.WriteLine("Cast to double failed in budgetPrice ConvertBack()");
                returnValue = -1;
            }
            return returnValue;
        }

        #endregion
    }

    public class ManufacturerConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (((TECManufacturer)value).Name == "Unnamed Manufacturer")
            {
                return null;
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
                return new TECManufacturer();
            } else
            {
                return value;
            }
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
                return new TECLocation("None");
            }
            else
            {
                return value;
            }
        }
        
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
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

    public class EnumerationToIntegerConverter : BaseConverter, IValueConverter
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
}
