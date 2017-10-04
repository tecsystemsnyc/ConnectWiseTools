using EstimatingLibrary;
using GongSolutions.Wpf.DragDrop;
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

namespace TECUserControlLibrary.Views
{
    /// <summary>
    /// Interaction logic for TypicalHierarchyView.xaml
    /// </summary>
    public partial class TypicalHierarchyView : UserControl
    {
        public double HalfWidth
        {
            get { return (double)GetValue(HalfWidthProperty); }
            set { SetValue(HalfWidthProperty, value); }
        }

        public static readonly DependencyProperty HalfWidthProperty =
            DependencyProperty.Register("HalfWidth", typeof(double),
              typeof(TypicalHierarchyView), new PropertyMetadata(0.0));

        public double TypicalWidth
        {
            get { return (double)GetValue(TypicalWidthProperty); }
            set { SetValue(TypicalWidthProperty, value); }
        }

        public static readonly DependencyProperty TypicalWidthProperty =
            DependencyProperty.Register("TypicalWidth", typeof(double),
              typeof(TypicalHierarchyView), new PropertyMetadata(0.0));

        public double SystemWidth
        {
            get { return (double)GetValue(SystemWidthProperty); }
            set { SetValue(SystemWidthProperty, value); }
        }

        public static readonly DependencyProperty SystemWidthProperty =
            DependencyProperty.Register("SystemWidth", typeof(double),
              typeof(TypicalHierarchyView), new PropertyMetadata(0.0));

        public double EquipmentWidth
        {
            get { return (double)GetValue(EquipmentWidthProperty); }
            set { SetValue(EquipmentWidthProperty, value); }
        }

        public static readonly DependencyProperty EquipmentWidthProperty =
            DependencyProperty.Register("EquipmentWidth", typeof(double),
              typeof(TypicalHierarchyView), new PropertyMetadata(0.0));

        public IEnumerable<TECTypical> TypicalSource
        {
            get { return (IEnumerable<TECTypical>)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("TypicalSource", typeof(IEnumerable<TECTypical>),
              typeof(TypicalHierarchyView), new PropertyMetadata(default(IEnumerable<TECTypical>)));

        public TECTypical SelectedTypical
        {
            get { return (TECTypical)GetValue(SelectedTypicalProperty); }
            set { SetValue(SelectedTypicalProperty, value); }
        }

        public static readonly DependencyProperty SelectedTypicalProperty =
            DependencyProperty.Register("SelectedTypical", typeof(TECTypical),
                typeof(TypicalHierarchyView), new FrameworkPropertyMetadata(default(TECTypical))
                {
                    BindsTwoWayByDefault = true,
                    DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                });

        public TECSystem SelectedSystem
        {
            get { return (TECSystem)GetValue(SelectedSystemProperty); }
            set { SetValue(SelectedSystemProperty, value); }
        }

        public static readonly DependencyProperty SelectedSystemProperty =
            DependencyProperty.Register("SelectedSystem", typeof(TECSystem),
                typeof(TypicalHierarchyView), new FrameworkPropertyMetadata(default(TECSystem))
                {
                    BindsTwoWayByDefault = true,
                    DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                });

        public TECEquipment SelectedEquipment
        {
            get { return (TECEquipment)GetValue(SelectedEquipmentProperty); }
            set { SetValue(SelectedEquipmentProperty, value); }
        }

        public static readonly DependencyProperty SelectedEquipmentProperty =
            DependencyProperty.Register("SelectedEquipment", typeof(TECEquipment),
                typeof(TypicalHierarchyView), new FrameworkPropertyMetadata(default(TECEquipment))
                {
                    BindsTwoWayByDefault = true,
                    DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                });

        public TECSubScope SelectedSubScope
        {
            get { return (TECSubScope)GetValue(SelectedSubScopeProperty); }
            set { SetValue(SelectedSubScopeProperty, value); }
        }

        public static readonly DependencyProperty SelectedSubScopeProperty =
            DependencyProperty.Register("SelectedSubScope", typeof(TECSubScope),
                typeof(TypicalHierarchyView), new FrameworkPropertyMetadata(default(TECSubScope))
                {
                    BindsTwoWayByDefault = true,
                    DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                });

        public TECDevice SelectedDevice
        {
            get { return (TECDevice)GetValue(SelectedDeviceProperty); }
            set { SetValue(SelectedDeviceProperty, value); }
        }

        public static readonly DependencyProperty SelectedDeviceProperty =
            DependencyProperty.Register("SelectedDevice", typeof(TECDevice),
                typeof(TypicalHierarchyView), new FrameworkPropertyMetadata(default(TECDevice))
                {
                    BindsTwoWayByDefault = true,
                    DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                });

        public IDropTarget DropHandler
        {
            get { return (IDropTarget)GetValue(DropHandlerProperty); }
            set { SetValue(DropHandlerProperty, value); }
        }

        public static readonly DependencyProperty DropHandlerProperty =
            DependencyProperty.Register("DropHandler", typeof(IDropTarget),
              typeof(TypicalHierarchyView));

        public TypicalHierarchyView()
        {
            InitializeComponent();
            SizeChanged += (sender, e) =>
            {
                if (e.WidthChanged)
                {
                    if (HalfWidth == 0)
                    {
                        TypicalWidth = e.NewSize.Width / 2;
                        SystemWidth = e.NewSize.Width / 2;
                        EquipmentWidth = e.NewSize.Width / 2;
                    }
                    else if (TypicalWidth != 0)
                    {
                        TypicalWidth = e.NewSize.Width / 2;

                    }
                    else if (SystemWidth != 0)
                    {
                        SystemWidth = e.NewSize.Width / 2;
                    }
                    else if (EquipmentWidth != 0)
                    {
                        EquipmentWidth = e.NewSize.Width / 2;
                    }
                    HalfWidth = e.NewSize.Width / 2;
                }
            };
        }

        private void BackToEquipment_Click(object sender, RoutedEventArgs e)
        {
            SelectedSubScope = null;
            SelectedEquipment = null;
            SelectedDevice = null;
        }
        private void BackToSystem_Click(object sender, RoutedEventArgs e)
        {
            SelectedEquipment = null;
            SelectedSystem = null;
        }

        private void BackToTypical_Click(object sender, RoutedEventArgs e)
        {
            SelectedTypical = null;
            SelectedSystem = null;
        }

    }
}
