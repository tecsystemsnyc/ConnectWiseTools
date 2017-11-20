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
            IOModules.QuantityChanged += ioModules_QuantityChanged;
        }

        private void ioModules_QuantityChanged(TECIOModule arg1, int arg2, int arg3)
        {
            int change = arg3 - arg2;
            if(change > 0)
            {
                for(int x = 0; x < change; x++)
                {
                    ControllerType.IOModules.Add(arg1);
                }
            } else if (change < 0)
            {
                for(int x = 0; x > change; x--)
                {
                    ControllerType.IOModules.Remove(arg1);
                }
            }
        }
    }
}
