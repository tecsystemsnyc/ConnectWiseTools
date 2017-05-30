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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TECUserControlLibrary.Models;

namespace TECUserControlLibrary.UserControls
{
    public partial class BMSControllerGridControl : UserControl
    {
        public BMSController ControllerSource
        {
            get { return (BMSController)GetValue(ControllerSourceProperty); }
            set { SetValue(ControllerSourceProperty, value); }
        }

        public static readonly DependencyProperty ControllerSourceProperty =
            DependencyProperty.Register("ControllerSource", typeof(BMSController),
              typeof(BMSControllerGridControl), new PropertyMetadata(default(BMSController)));

        public BMSControllerGridControl()
        {
            InitializeComponent();
        }
    }
}
