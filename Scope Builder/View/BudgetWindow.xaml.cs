using EstimatingLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace Scope_Builder.View
{
    /// <summary>
    /// Interaction logic for BudgetWindow.xaml
    /// </summary>
    public partial class BudgetWindow : Window
    {

        public BudgetWindow()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception e)
            {
                string message = "Scope Builder Main Window Initalization Failed: " + e.Message;
                Console.WriteLine(message);
                throw new Exception(message);
            }

            Show();
        }
    }
}
