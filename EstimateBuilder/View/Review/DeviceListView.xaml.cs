﻿using System;
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

namespace EstimateBuilder.View.Review
{
    /// <summary>
    /// Interaction logic for DeviceListView.xaml
    /// </summary>
    public partial class DeviceListView : UserControl
    {
        public Object ViewModel
        {
            get { return (Object)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(Object),
              typeof(DeviceListView));

        public DeviceListView()
        {
            InitializeComponent();
        }
    }
}