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

namespace TECUserControlLibrary.Views
{
    /// <summary>
    /// Interaction logic for UnconnectedSubScopeView.xaml
    /// </summary>
    public partial class UnconnectedSubScopeView : UserControl
    {
        public double SystemWidth
        {
            get { return (double)GetValue(SystemWidthProperty); }
            set { SetValue(SystemWidthProperty, value); }
        }

        public static readonly DependencyProperty SystemWidthProperty =
            DependencyProperty.Register("SystemWidth", typeof(double),
              typeof(UnconnectedSubScopeView), new PropertyMetadata(0.0));

        public double EquipmentWidth
        {
            get { return (double)GetValue(EquipmentWidthProperty); }
            set { SetValue(EquipmentWidthProperty, value); }
        }

        public static readonly DependencyProperty EquipmentWidthProperty =
            DependencyProperty.Register("EquipmentWidth", typeof(double),
              typeof(UnconnectedSubScopeView), new PropertyMetadata(0.0));

        public IEnumerable<TECSystem> SystemSource
        {
            get { return (ObservableCollection<TECSystem>)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("SystemSource", typeof(IEnumerable<TECSystem>),
              typeof(UnconnectedSubScopeView), new PropertyMetadata(default(IEnumerable<TECSystem>)));

        public TECSystem SelectedSystem
        {
            get { return (TECSystem)GetValue(SelectedSystemProperty); }
            set { SetValue(SelectedSystemProperty, value); }
        }

        public static readonly DependencyProperty SelectedSystemProperty =
            DependencyProperty.Register("SelectedSystem", typeof(TECSystem),
                typeof(UnconnectedSubScopeView), new FrameworkPropertyMetadata(default(TECSystem))
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
                typeof(UnconnectedSubScopeView), new FrameworkPropertyMetadata(default(TECEquipment))
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
                typeof(UnconnectedSubScopeView), new FrameworkPropertyMetadata(default(TECSubScope))
                {
                    BindsTwoWayByDefault = true,
                    DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                });

        public UnconnectedSubScopeView()
        {
            InitializeComponent();
        }

        private void systemBack_Click(object sender, RoutedEventArgs e)
        {
            SelectedEquipment = null;
        }

        private void equipmentBack_Click(object sender, RoutedEventArgs e)
        {
            SelectedSubScope = null;
        }

        private void systemMove_Completed(object sender, EventArgs e)
        {
            SystemWidth = 0.0;
        }

        private void systemMoveBack_Completed(object sender, EventArgs e)
        {
            SystemWidth = this.ActualWidth / 2;
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
