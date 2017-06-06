using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TECUserControlLibrary.ViewModels
{
    public class NetworkVM : ViewModelBase
    {
        public NetworkControllerVM NetworkControllersVM { get; private set; }
        public NetworkControllerVM UnitaryControllersVM { get; private set; }

        private ChangeWatcher _changeWatcher;
        public ChangeWatcher changeWatcher
        {
            get
            {
                return _changeWatcher;
            }
            set
            {
                if (_changeWatcher != null)
                {
                    _changeWatcher.InstanceChanged -= instanceChanged;
                }
                _changeWatcher = value;
                _changeWatcher.InstanceChanged += instanceChanged;
            }
        }

        public NetworkVM(TECBid bid)
        {
            NetworkControllersVM = new NetworkControllerVM(System.Windows.Visibility.Visible, bid);
            UnitaryControllersVM = new NetworkControllerVM(System.Windows.Visibility.Collapsed);
            
            foreach(TECController control in bid.Controllers)
            {
                sortController(control);
            }
            foreach (TECSystem typical in bid.Systems)
            {
                foreach (TECSystem instance in typical.SystemInstances)
                {
                    foreach(TECController control in instance.Controllers)
                    {
                        sortController(control);
                    }
                }
            }

            changeWatcher = new ChangeWatcher(bid);
        }

        public void Refresh(TECBid bid)
        {
            NetworkControllersVM.Refresh(bid);
            UnitaryControllersVM.Refresh();
            changeWatcher = new ChangeWatcher(bid);
        }

        public void sortController(TECController controller)
        {
            if (controller.NetworkType == NetworkType.DDC || controller.NetworkType == NetworkType.Server)
            {
                NetworkControllersVM.AddController(controller);
            }
            else
            {
                controller.NetworkType = NetworkType.Unitary;
                UnitaryControllersVM.AddController(controller);
            }
        }

        private void instanceChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e is PropertyChangedExtendedEventArgs<Object>)
            {
                PropertyChangedExtendedEventArgs<Object> args = e as PropertyChangedExtendedEventArgs<Object>;
                var targetObject = args.NewValue;
                var referenceObject = args.OldValue;
                if (args.PropertyName == "Add" || args.PropertyName == "AddCatalog")
                {
                    if (targetObject is TECController && (referenceObject is TECBid || referenceObject is TECSystem))
                    {
                        sortController(targetObject as TECController);
                    }
                }
            }
        }
    }
}
