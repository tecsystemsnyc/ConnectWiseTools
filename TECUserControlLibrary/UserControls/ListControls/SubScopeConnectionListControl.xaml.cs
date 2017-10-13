using EstimatingLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

namespace TECUserControlLibrary.UserControls.ListControls
{
    /// <summary>
    /// Interaction logic for SubScopeConnectionListControl.xaml
    /// </summary>
    public partial class SubScopeConnectionListControl : BaseListControl<TECSubScopeConnection>
    {
        public ObservableCollection<TECElectricalMaterial> ConduitTypes
        {
            get { return (ObservableCollection<TECElectricalMaterial>)GetValue(ConduitTypesProperty); }
            set { SetValue(ConduitTypesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ConduitTypes.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ConduitTypesProperty =
            DependencyProperty.Register("ConduitTypes", typeof(ObservableCollection<TECElectricalMaterial>), typeof(SubScopeConnectionListControl), new PropertyMetadata(default(ObservableCollection<TECElectricalMaterial>)));

        public bool ReadOnly
        {
            get { return (bool)GetValue(ReadOnlyProperty); }
            set { SetValue(ReadOnlyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ReadOnly.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ReadOnlyProperty =
            DependencyProperty.Register("ReadOnly", typeof(bool), typeof(SubScopeConnectionListControl), new PropertyMetadata(false));
        

        public ICommand UpdateItemCommand
        {
            get { return (ICommand)GetValue(UpdateItemCommandProperty); }
            set { SetValue(UpdateItemCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UpdateItemCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UpdateItemCommandProperty =
            DependencyProperty.Register("UpdateItemCommand", typeof(ICommand), typeof(SubScopeConnectionListControl));



        public SubScopeConnectionListControl()
        {
            InitializeComponent();
        }
    }
}
