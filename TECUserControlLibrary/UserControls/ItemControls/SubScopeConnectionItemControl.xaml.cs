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
        public SubScopeConnectionItem SubScopeConnectionItem
        {
            get { return (SubScopeConnectionItem)GetValue(SubScopeConnectionItemProperty); }
            set { SetValue(SubScopeConnectionItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SubScopeConnection.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SubScopeConnectionItemProperty =
            DependencyProperty.Register("SubScopeConnectionItem", typeof(SubScopeConnectionItem), typeof(SubScopeConnectionItemControl));
        
        public IEnumerable<TECElectricalMaterial> ConduitTypes
        {
            get { return (IEnumerable<TECElectricalMaterial>)GetValue(ConduitTypesProperty); }
            set { SetValue(ConduitTypesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ConduitTypes.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ConduitTypesProperty =
            DependencyProperty.Register("ConduitTypes", typeof(IEnumerable<TECElectricalMaterial>),
                typeof(SubScopeConnectionItemControl));
        
        public bool ReadOnly
        {
            get { return (bool)GetValue(ReadOnlyProperty); }
            set { SetValue(ReadOnlyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ReadOnly.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ReadOnlyProperty =
            DependencyProperty.Register("ReadOnly", typeof(bool), typeof(SubScopeConnectionItemControl), new PropertyMetadata(false));



        public ICommand UpdateCommand
        {
            get { return (ICommand)GetValue(UpdateCommandProperty); }
            set { SetValue(UpdateCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UpdateCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UpdateCommandProperty =
            DependencyProperty.Register("UpdateCommand", typeof(ICommand), typeof(SubScopeConnectionItemControl));



        public Visibility UpdateButtonVisibility
        {
            get { return (Visibility)GetValue(UpdateButtonVisibilityProperty); }
            set { SetValue(UpdateButtonVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UpdateButtonVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UpdateButtonVisibilityProperty =
            DependencyProperty.Register("UpdateButtonVisibility", typeof(Visibility), typeof(SubScopeConnectionItemControl));




        public SubScopeConnectionItemControl()
        {
            InitializeComponent();
        }
    }
}
