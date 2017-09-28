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

}
