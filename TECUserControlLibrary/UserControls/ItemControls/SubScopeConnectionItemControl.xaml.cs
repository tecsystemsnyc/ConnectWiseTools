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
using TECUserControlLibrary.Models;

namespace TECUserControlLibrary.UserControls.ItemControls
{
    /// <summary>
    /// Interaction logic for SubScopeConnectionItemControl.xaml
    /// </summary>
    public partial class SubScopeConnectionItemControl : UserControl
    {
        public TypicalSubScopeConnection TypicalSubScopeConnection
        {
            get { return (TypicalSubScopeConnection)GetValue(TypicalSubScopeConnectionProperty); }
            set
            {
                SetValue(TypicalSubScopeConnectionProperty, value);
                SetValue(SubScopeConnectionProperty, value.Connection);
            }
        }

        // Using a DependencyProperty as the backing store for TypicalSubScopeConnection.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TypicalSubScopeConnectionProperty =
            DependencyProperty.Register("TypicalSubScopeConnection", typeof(TypicalSubScopeConnection), typeof(SubScopeConnectionItemControl), new PropertyMetadata(0));



        public TECSubScopeConnection SubScopeConnection
        {
            get { return (TECSubScopeConnection)GetValue(SubScopeConnectionProperty); }
            set { SetValue(SubScopeConnectionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SubScopeConnection.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SubScopeConnectionProperty =
            DependencyProperty.Register("SubScopeConnection", typeof(TECSubScopeConnection), typeof(SubScopeConnectionItemControl));
        
        public ObservableCollection<TECElectricalMaterial> ConduitTypes
        {
            get { return (ObservableCollection<TECElectricalMaterial>)GetValue(ConduitTypesProperty); }
            set { SetValue(ConduitTypesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ConduitTypes.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ConduitTypesProperty =
            DependencyProperty.Register("ConduitTypes", typeof(ObservableCollection<TECElectricalMaterial>), typeof(SubScopeConnectionItemControl), new PropertyMetadata(0));
        
        public bool ReadOnly
        {
            get { return (bool)GetValue(ReadOnlyProperty); }
            set { SetValue(ReadOnlyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ReadOnly.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ReadOnlyProperty =
            DependencyProperty.Register("ReadOnly", typeof(bool), typeof(SubScopeConnectionItemControl), new PropertyMetadata(false));
        

        public SubScopeConnectionItemControl()
        {
            InitializeComponent();
        }
    }
}
