using EstimatingLibrary;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using TECUserControlLibrary.Interfaces;

namespace TECUserControlLibrary.UserControls.ListControls
{
    /// <summary>
    /// Interaction logic for TypicalSubScopeListControl.xaml
    /// </summary>
    public partial class SubScopeConnectionListControl : BaseListControl<ISubScopeConnectionItem>
    {
        public IEnumerable<TECElectricalMaterial> ConduitTypes
        {
            get { return (IEnumerable<TECElectricalMaterial>)GetValue(ConduitTypesProperty); }
            set { SetValue(ConduitTypesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ConduitTypes.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ConduitTypesProperty =
            DependencyProperty.Register("ConduitTypes", typeof(IEnumerable<TECElectricalMaterial>), typeof(SubScopeConnectionListControl));

        public bool ReadOnly
        {
            get { return (bool)GetValue(ReadOnlyProperty); }
            set { SetValue(ReadOnlyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ReadOnly.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ReadOnlyProperty =
            DependencyProperty.Register("ReadOnly", typeof(bool), typeof(SubScopeConnectionListControl), new PropertyMetadata(false));



        public ICommand UpdateCommand
        {
            get { return (ICommand)GetValue(UpdateCommandProperty); }
            set { SetValue(UpdateCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UpdateCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UpdateCommandProperty =
            DependencyProperty.Register("UpdateCommand", typeof(ICommand), typeof(SubScopeConnectionListControl));



        public Visibility UpdateButtonVisibility
        {
            get { return (Visibility)GetValue(UpdateButtonVisibilityProperty); }
            set { SetValue(UpdateButtonVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UpdateButtonVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UpdateButtonVisibilityProperty =
            DependencyProperty.Register("UpdateButtonVisibility", typeof(Visibility), typeof(SubScopeConnectionListControl));





        public SubScopeConnectionListControl()
        {
            InitializeComponent();
        }
    }
}
