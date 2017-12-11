using EstimatingLibrary;
using EstimatingLibrary.Interfaces;
using System.Windows;
using System.Windows.Controls;

namespace TECUserControlLibrary.Utilities
{
    public class InterfaceDataTemplateSelector<T> : DataTemplateSelector
    {
        public DataTemplate DefaultTemplate { get; set; }
        public DataTemplate InterfaceTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if(item is T)
            {
                return InterfaceTemplate;
            }else
            {
                return DefaultTemplate;
            }
        }
    }

    public class ScopeSelector : InterfaceDataTemplateSelector<TECScope> { }
    public class CostSelector : InterfaceDataTemplateSelector<TECCost> { }
    public class HardwareSelector : InterfaceDataTemplateSelector<TECHardware> { }
    public class LocatedSelector : InterfaceDataTemplateSelector<TECLocated> { }
    public class LabeledSelector : InterfaceDataTemplateSelector<TECLabeled> { }
    public class ControllerSelector : InterfaceDataTemplateSelector<TECController> { }
    public class PanelSelector : InterfaceDataTemplateSelector<TECPanel> { }
    public class MiscSelector : InterfaceDataTemplateSelector<TECMisc> { }
    public class IOModuleSelector : InterfaceDataTemplateSelector<TECIOModule> { }
    public class ControllerTypeSelector : InterfaceDataTemplateSelector<TECControllerType> { }
    public class ConnectionSelector : InterfaceDataTemplateSelector<TECConnection> { }
    public class ElectricalMaterialSelector : InterfaceDataTemplateSelector<TECElectricalMaterial> { }
    public class ConnectionTypeSelector : InterfaceDataTemplateSelector<TECConnectionType> { }

    public class CostBatchInterfaceSelector : InterfaceDataTemplateSelector<INotifyCostChanged> { }
    public class PointInterfaceSelector : InterfaceDataTemplateSelector<INotifyPointChanged> { }
    public class NetworkParentableSelector : InterfaceDataTemplateSelector<INetworkParentable> { }
    public class EndDeviceSelector : InterfaceDataTemplateSelector<IEndDevice> { }

    public class ScopeTemplateSelector : DataTemplateSelector
    {
        public DataTemplate SystemTemplate { get; set; }
        public DataTemplate EquipmentTemplate { get; set; }
        public DataTemplate SubScopeTemplate { get; set; }
        public DataTemplate ControllerTemplate { get; set; }
        public DataTemplate MiscTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if(item is ScopeTemplateIndex index)
            {
                switch (index)
                {
                    case ScopeTemplateIndex.System:
                        return SystemTemplate;
                    case ScopeTemplateIndex.Equipment:
                        return EquipmentTemplate;
                    case ScopeTemplateIndex.SubScope:
                        return SubScopeTemplate;
                    case ScopeTemplateIndex.Controller:
                        return ControllerTemplate;
                    case ScopeTemplateIndex.Misc:
                        return MiscTemplate;
                    default:
                        return SystemTemplate;
                }
            }
            else
            {
                return SystemTemplate;
            }
        }
    }
    public class SystemComponentSelector : DataTemplateSelector
    {
        public DataTemplate EquipmentTemplate { get; set; }
        public DataTemplate ProposalTemplate { get; set; }
        public DataTemplate ControllerTemplate { get; set; }
        public DataTemplate MiscTemplate { get; set; }
        public DataTemplate ConnectionTemplate { get; set; }
        public DataTemplate NetworkTemplate { get; set; }


        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is SystemComponentIndex index)
            {
                switch (index)
                {
                    case SystemComponentIndex.Misc:
                        return MiscTemplate;
                    case SystemComponentIndex.Equipment:
                        return EquipmentTemplate;
                    case SystemComponentIndex.Proposal:
                        return ProposalTemplate;
                    case SystemComponentIndex.Controllers:
                        return ControllerTemplate;
                    case SystemComponentIndex.Electrical:
                        return ConnectionTemplate;
                    case SystemComponentIndex.Network:
                        return NetworkTemplate;
                    default:
                        return EquipmentTemplate;
                }
            }
            else
            {
                return EquipmentTemplate;
            }
        }
    }

    public class TypicalInstancesSelector : DataTemplateSelector
    {
        public DataTemplate TypicalTemplate { get; set; }
        public DataTemplate InstancesTemplate { get; set; }
        
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is TypicalInstanceEnum index)
            {
                switch (index)
                {
                    case TypicalInstanceEnum.Typical:
                        return TypicalTemplate;
                    case TypicalInstanceEnum.Instance:
                        return InstancesTemplate;
                }
                return InstancesTemplate;
            }
            else
            {
                return InstancesTemplate;
            }
        }
    }

    public class MaterialTypeSelector : DataTemplateSelector
    {
        public DataTemplate DeviceTemplate { get; set; }
        public DataTemplate ConnectionTypeTemplate { get; set; }
        public DataTemplate ConduitTypeTemplate { get; set; }
        public DataTemplate ControllerTypeTemplate { get; set; }
        public DataTemplate PanelTypeTemplate { get; set; }
        public DataTemplate AssociatedCostTemplate { get; set; }
        public DataTemplate IOModuleTemplate { get; set; }
        public DataTemplate ValveTemplate { get; set; }
        public DataTemplate ManufacturerTemplate { get; set; }
        public DataTemplate TagTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is MaterialType index)
            {
                switch (index)
                {
                    case MaterialType.Device:
                        return DeviceTemplate;
                    case MaterialType.ConnectionType:
                        return ConnectionTypeTemplate;
                    case MaterialType.ConduitType:
                        return ConduitTypeTemplate;
                    case MaterialType.ControllerType:
                        return ControllerTypeTemplate;
                    case MaterialType.PanelType:
                        return PanelTypeTemplate;
                    case MaterialType.AssociatedCost:
                        return AssociatedCostTemplate;
                    case MaterialType.IOModule:
                        return IOModuleTemplate;
                    case MaterialType.Valve:
                        return ValveTemplate;
                    case MaterialType.Manufacturer:
                        return ManufacturerTemplate;
                    case MaterialType.Tag:
                        return TagTemplate;
                }
                return DeviceTemplate;
            }
            else
            {
                return DeviceTemplate;
            }
        }
    }
}
