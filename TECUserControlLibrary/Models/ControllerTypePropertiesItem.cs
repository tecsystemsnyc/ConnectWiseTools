using EstimatingLibrary;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECUserControlLibrary.Utilities;

namespace TECUserControlLibrary.Models
{
    public class ControllerTypePropertiesItem : ViewModelBase
    {
        TECControllerType ControllerType { get; }
        QuantityCollection<TECIOModule> IOModules { get; }

        public ControllerTypePropertiesItem(TECControllerType controllerType)
        {
            ControllerType = controllerType;
            IOModules = new QuantityCollection<TECIOModule>(controllerType.IOModules);
            //IOModules.
        }
    }
}
