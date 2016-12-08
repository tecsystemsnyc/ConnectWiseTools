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
    /// Interaction logic for ScopeControl.xaml
    /// </summary>
    public partial class ScopeControl : UserControl
    {
        #region DPs

        /// <summary>
        /// Gets or sets the Name Label which is displayed
        /// </summary>
        public String NameLabel
        {
            get { return (String)GetValue(NameLabelProperty); }
            set { SetValue(NameLabelProperty, value); }
        }

        /// <summary>
        /// Identified the Name Label dependency property
        /// </summary>
        public static readonly DependencyProperty NameLabelProperty =
            DependencyProperty.Register("NameLabel", typeof(String),
              typeof(ScopeControl), new PropertyMetadata(""));

        /// <summary>
        /// Gets or sets the Description Label which is displayed
        /// </summary>
        public String DescriptionLabel
        {
            get { return (String)GetValue(DescriptionLabelProperty); }
            set { SetValue(DescriptionLabelProperty, value); }
        }

        /// <summary>
        /// Identified the Description Label dependency property
        /// </summary>
        public static readonly DependencyProperty DescriptionLabelProperty =
            DependencyProperty.Register("DescriptionLabel", typeof(String),
              typeof(ScopeControl), new PropertyMetadata(""));

        #endregion
        public ScopeControl()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception e)
            {
                string message = "Scope Control Initalization Failed: " + e.Message;
                Console.WriteLine(message);
                throw new Exception(message);
            }
        }
    }
}
