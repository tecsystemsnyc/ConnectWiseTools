using EstimatingLibrary;
using System;
using System.Collections.Generic;
using System.Globalization;
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

    public class NetworkConnectionToItemConverter : BaseConverter, IMultiValueConverter
    {
        #region IValueConverter Members

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values.Length > 2)
            {
                return new NetworkConnectionVM(values[0] as TECNetworkConnection, values[1] as IEnumerable<TECElectricalMaterial>);
            }
            else
            {
                return null;
            }
        }
        
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
