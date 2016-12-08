using EstimatingLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TECUserControlLibrary
{
    /// <summary>
    /// Interaction logic for ScopeItemsControl.xaml
    /// </summary>
    public partial class ScopeItemsControl : UserControl
    {
        #region DPs
        
        public Object ScopeSource
        {
            get { return (Object)GetValue(ScopeSourceProperty); }
            set { SetValue(ScopeSourceProperty, value); }
        }

        public static readonly DependencyProperty ScopeSourceProperty =
            DependencyProperty.Register("ScopeSource", typeof(Object),
              typeof(ScopeItemsControl));
        

        #endregion

        public ScopeItemsControl()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception e)
            {
                string message = "Scope Items Control Initalization Failed: " + e.Message;
                Console.WriteLine(message);
                throw new Exception(message);
            }
        }
    }
}
