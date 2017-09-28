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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TECUserControlLibrary.UserControls
{
    /// <summary>
    /// Interaction logic for EquipmentHierarchy.xaml
    /// </summary>
    public partial class EquipmentHierarchy : UserControl
    {

        public double ParentWidth
        {
            get { return (double)GetValue(WidthProperty); }
            set { SetValue(WidthProperty, value); }
        }

        public static readonly DependencyProperty ParentWidthProperty =
            DependencyProperty.Register("ParentWidth", typeof(double),
              typeof(EquipmentHierarchy), new PropertyMetadata(default(double)));

        public ObservableCollection<TECEquipment> EquipmentSource
        {
            get { return (ObservableCollection<TECEquipment>)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("EquipmentSource", typeof(ObservableCollection<TECEquipment>),
              typeof(EquipmentHierarchy), new PropertyMetadata(default(ObservableCollection<TECEquipment>)));

        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(object),
                typeof(EquipmentHierarchy), new FrameworkPropertyMetadata(null)
                {
                    BindsTwoWayByDefault = true,
                    DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                });

        public EquipmentHierarchy()
        {
            ParentWidth = this.Width / 2;
            InitializeComponent();
        }
    }
}
