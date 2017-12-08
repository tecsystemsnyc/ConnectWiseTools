using EstimatingLibrary;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using TECUserControlLibrary.Models;
using TECUserControlLibrary.Utilities;

namespace TECUserControlLibrary.UserControls.ListControls
{
    /// <summary>
    /// Interaction logic for NetworkConnectionListControl.xaml
    /// </summary>
    public partial class NetworkConnectionListControl : BaseListControl<TECNetworkConnection>
    {


        public IEnumerable<TECElectricalMaterial> ConduitTypes
        {
            get { return (IEnumerable<TECElectricalMaterial>)GetValue(ConduitTypesProperty); }
            set { SetValue(ConduitTypesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ConduitTypes.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ConduitTypesProperty =
            DependencyProperty.Register("ConduitTypes", typeof(IEnumerable<TECElectricalMaterial>), typeof(NetworkConnectionListControl));



        public NetworkConnectionListControl()
        {
            InitializeComponent();
        }
    }

    public class NetworkConnectionToItemConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                return new NetworkConnectionVM(value as TECNetworkConnection, parameter as IEnumerable<TECElectricalMaterial>);
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();

        }
        #endregion
    }
}
