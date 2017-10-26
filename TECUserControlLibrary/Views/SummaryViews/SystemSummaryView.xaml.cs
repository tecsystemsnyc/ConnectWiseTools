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
using TECUserControlLibrary.ViewModels.SummaryVMs;

namespace TECUserControlLibrary.Views.SummaryViews
{
    /// <summary>
    /// Interaction logic for SystemSummaryView.xaml
    /// </summary>
    public partial class SystemSummaryView : BaseView<SystemSummaryVM>
    {
      
        public SystemSummaryView()
        {
            InitializeComponent();
        }
    }
}