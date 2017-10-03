﻿using EstimatingLibrary;
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

namespace TECUserControlLibrary.UserControls
{
    /// <summary>
    /// Interaction logic for HardwarePropertiesControl.xaml
    /// </summary>
    public partial class HardwarePropertiesControl : UserControl
    {
        public TECHardware Selected
        {
            get { return (TECHardware)GetValue(SelectedProperty); }
            set { SetValue(SelectedProperty, value); }
        }

        public static readonly DependencyProperty SelectedProperty =
            DependencyProperty.Register("Selected", typeof(TECHardware),
              typeof(HardwarePropertiesControl));

        public HardwarePropertiesControl()
        {
            InitializeComponent();
        }
    }
}
