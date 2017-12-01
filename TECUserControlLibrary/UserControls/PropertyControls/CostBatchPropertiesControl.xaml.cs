using EstimatingLibrary.Interfaces;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using TECUserControlLibrary.Models;
using TECUserControlLibrary.Utilities;

namespace TECUserControlLibrary.UserControls.PropertyControls
{
    /// <summary>
    /// Interaction logic for INotifyCostChangedPropertiesControl.xaml
    /// </summary>
    public partial class CostBatchPropertiesControl : UserControl
    {
        public INotifyCostChanged Selected
        {
            get { return (INotifyCostChanged)GetValue(SelectedProperty); }
            set { SetValue(SelectedProperty, value); }
        }

        public static readonly DependencyProperty SelectedProperty =
            DependencyProperty.Register("Selected", typeof(INotifyCostChanged),
              typeof(CostBatchPropertiesControl));

        public CostBatchPropertiesControl()
        {
            InitializeComponent();
        }
    }

    public class CostBatchToPropertiesItemConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                return new CostBatchPropertiesItem(value as INotifyCostChanged);

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
