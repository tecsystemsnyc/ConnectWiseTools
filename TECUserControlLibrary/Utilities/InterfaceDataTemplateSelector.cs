using EstimatingLibrary;
using EstimatingLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    public class CostBatchInterfaceSelector : InterfaceDataTemplateSelector<INotifyCostChanged> { }
    public class PointInterfaceSelector : InterfaceDataTemplateSelector<INotifyPointChanged> { }


    public class ScopeTemplateSelector : DataTemplateSelector
    {
        public DataTemplate SystemTemplate { get; set; }
        public DataTemplate EquipmentTemplate { get; set; }
        public DataTemplate SubScopeTemplate { get; set; }
        public DataTemplate ControllerTemplate { get; set; }
        public DataTemplate PanelTemplate { get; set; }

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
                    case ScopeTemplateIndex.Panel:
                        return PanelTemplate;
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

}
