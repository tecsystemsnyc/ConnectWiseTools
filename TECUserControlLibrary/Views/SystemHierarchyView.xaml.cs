using EstimatingLibrary;
using GongSolutions.Wpf.DragDrop;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TECUserControlLibrary.UserControls;
using TECUserControlLibrary.UserControls.ListControls;
using TECUserControlLibrary.Utilities;
using TECUserControlLibrary.ViewModels;

namespace TECUserControlLibrary.Views
{
    /// <summary>
    /// Interaction logic for SystemHierarchyView.xaml
    /// </summary>
    public partial class SystemHierarchyView : UserControl
    {
        
        public double ModalHeight
        {
            get { return (double)GetValue(ModalHeightProperty); }
            set { SetValue(ModalHeightProperty, value); }
        }
        
        public bool IsTypical
        {
            get { return (bool)GetValue(IsTypicalProperty); }
            set { SetValue(IsTypicalProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsTypical.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsTypicalProperty =
            DependencyProperty.Register("IsTypical", typeof(bool), typeof(SystemHierarchyView), new PropertyMetadata(false));
        
        public static readonly DependencyProperty ModalHeightProperty =
            DependencyProperty.Register("ModalHeight", typeof(double),
              typeof(SystemHierarchyView), new PropertyMetadata(1.0, new PropertyChangedCallback(OnUpdateConnectionVMChanged)));
        
        private static void OnUpdateConnectionVMChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            SystemHierarchyView thisView = dependencyObject as SystemHierarchyView;
            Grid grid = (Grid)thisView.FindName("mainGrid");
            grid.IsEnabled = (double)e.NewValue >= thisView.ActualHeight;
        }

        public double HalfWidth
        {
            get { return (double)GetValue(HalfWidthProperty); }
            set { SetValue(HalfWidthProperty, value); }
        }

        public static readonly DependencyProperty HalfWidthProperty =
            DependencyProperty.Register("HalfWidth", typeof(double),
              typeof(SystemHierarchyView), new PropertyMetadata(0.0));

        public double SystemWidth
        {
            get { return (double)GetValue(SystemWidthProperty); }
            set { SetValue(SystemWidthProperty, value); }
        }

        public static readonly DependencyProperty SystemWidthProperty =
            DependencyProperty.Register("SystemWidth", typeof(double),
              typeof(SystemHierarchyView), new PropertyMetadata(0.0));

        public double EquipmentWidth
        {
            get { return (double)GetValue(EquipmentWidthProperty); }
            set { SetValue(EquipmentWidthProperty, value); }
        }

        public static readonly DependencyProperty EquipmentWidthProperty =
            DependencyProperty.Register("EquipmentWidth", typeof(double),
              typeof(SystemHierarchyView), new PropertyMetadata(0.0));

        public IEnumerable<TECSystem> SystemSource
        {
            get { return (ObservableCollection<TECSystem>)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("SystemSource", typeof(IEnumerable<TECSystem>),
              typeof(SystemHierarchyView), new PropertyMetadata(default(IEnumerable<TECSystem>)));

        public TECSystem SelectedSystem
        {
            get { return (TECSystem)GetValue(SelectedSystemProperty); }
            set { SetValue(SelectedSystemProperty, value); }
        }

        public static readonly DependencyProperty SelectedSystemProperty =
            DependencyProperty.Register("SelectedSystem", typeof(TECSystem),
                typeof(SystemHierarchyView), new FrameworkPropertyMetadata(default(TECSystem))
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
                typeof(SystemHierarchyView), new FrameworkPropertyMetadata(default(TECEquipment))
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
                typeof(SystemHierarchyView), new FrameworkPropertyMetadata(default(TECSubScope))
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
                typeof(SystemHierarchyView), new FrameworkPropertyMetadata(default(TECDevice))
                {
                    BindsTwoWayByDefault = true,
                    DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                });

        public TECController SelectedController
        {
            get { return (TECController)GetValue(SelectedControllerProperty); }
            set { SetValue(SelectedControllerProperty, value); }
        }

        public static readonly DependencyProperty SelectedControllerProperty =
            DependencyProperty.Register("SelectedController", typeof(TECController),
                typeof(SystemHierarchyView), new FrameworkPropertyMetadata(default(TECController))
                {
                    BindsTwoWayByDefault = true,
                    DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                });

        public TECPanel SelectedPanel
        {
            get { return (TECPanel)GetValue(SelectedPanelProperty); }
            set { SetValue(SelectedPanelProperty, value); }
        }

        public static readonly DependencyProperty SelectedPanelProperty =
            DependencyProperty.Register("SelectedPanel", typeof(TECPanel),
                typeof(SystemHierarchyView), new FrameworkPropertyMetadata(default(TECPanel))
                {
                    BindsTwoWayByDefault = true,
                    DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                });

        public TECMisc SelectedMisc
        {
            get { return (TECMisc)GetValue(SelectedMiscProperty); }
            set { SetValue(SelectedMiscProperty, value); }
        }

        public static readonly DependencyProperty SelectedMiscProperty =
            DependencyProperty.Register("SelectedMisc", typeof(TECMisc),
                typeof(SystemHierarchyView), new FrameworkPropertyMetadata(default(TECMisc))
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
              typeof(SystemHierarchyView));
        
        public SystemHierarchyVM ViewModel
        {
            get { return (SystemHierarchyVM)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(SystemHierarchyVM), typeof(SystemHierarchyView), new PropertyMetadata(default(SystemHierarchyVM)));
        
        public SystemHierarchyView()
        {
            InitializeComponent();
            SizeChanged += handleSizeChanged;
        }

        virtual protected void handleSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.WidthChanged)
            {
                if (HalfWidth == 0)
                {
                    SystemWidth = e.NewSize.Width / 2;
                    EquipmentWidth = e.NewSize.Width / 2;
                }
                else if (SystemWidth != 0)
                {
                    SystemWidth = e.NewSize.Width / 2;
                    EquipmentWidth = e.NewSize.Width / 2;
                }
                else if (EquipmentWidth != 0)
                {
                    EquipmentWidth = e.NewSize.Width / 2;
                }
                
                HalfWidth = e.NewSize.Width / 2;
            }
            if (e.HeightChanged)
            {
                if (ModalHeight != 0.0)
                {
                    ModalHeight = e.NewSize.Height;
                }
            }
        }

        private void systemBack_Click(object sender, RoutedEventArgs e)
        {
            EquipmentListControl eList = UIHelpers.FindVisualChild<EquipmentListControl>(this);
            if(eList != null)
            {
                eList.SelectedItem = null;
            }
            componentComboBox.SelectedValue = SystemComponentIndex.Equipment;
        }

        private void equipmentBack_Click(object sender, RoutedEventArgs e)
        {
            subScopeList.SelectedItem = null;
        }

        private void componentComboBox_Selected(object sender, RoutedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            EquipmentListControl eList = UIHelpers.FindVisualChild<EquipmentListControl>(this);
            if (eList != null) { 
                if(eList.SelectedItem != null)
                {

                    eList.SelectedItem = null;
                    Storyboard moveBack = (Storyboard)FindResource("systemMoveBack");
                    moveBack.Begin();
                }
            }
            ControllerListControl cList = UIHelpers.FindVisualChild<ControllerListControl>(this);
            if (cList != null)
            {
                cList.SelectedItem = null;
            }
            PanelListControl pList = UIHelpers.FindVisualChild<PanelListControl>(this);
            if (pList != null)
            {
                pList.SelectedItem = null;
            }
            SystemComponentIndex selectedValue = (SystemComponentIndex)comboBox.SelectedValue;
            if (selectedValue == SystemComponentIndex.Electrical ||
                selectedValue == SystemComponentIndex.Misc ||
                selectedValue == SystemComponentIndex.Proposal ||
                selectedValue == SystemComponentIndex.Controllers)
            {
                Storyboard move = (Storyboard)FindResource("systemMove");
                move.Begin();
            }
            
        }

        private void Add_Clicked(object sender, RoutedEventArgs e)
        {
            Storyboard moveBack = (Storyboard)FindResource("modalIn");
            moveBack.Begin();
        }

        private void systemMove_Completed(object sender, EventArgs e)
        {
            SystemWidth = 0.0;
        }

        private void systemMoveBack_Completed(object sender, EventArgs e)
        {
            SystemWidth = this.ActualWidth / 2;
        }

        private void modalOut_Completed(object sender, EventArgs e)
        {
            ModalHeight = this.ActualHeight;
        }

        private void modalIn_Completed(object sender, EventArgs e)
        {
            ModalHeight = 0;
        }

        private void equipmentMove_Completed(object sender, EventArgs e)
        {
            EquipmentWidth = 0;
        }

        private void equipmentMoveBack_Completed(object sender, EventArgs e)
        {
            EquipmentWidth = this.ActualWidth / 2;
        }
    }
}
