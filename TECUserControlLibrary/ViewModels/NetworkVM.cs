using EstimatingLibrary;
using EstimatingLibrary.Interfaces;
using EstimatingLibrary.Utilities;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace TECUserControlLibrary.ViewModels
{
    public class NetworkVM : ViewModelBase, IDropTarget
    {
        private NetworkVM(IEnumerable<INetworkConnectable> connectables, ChangeWatcher watcher, TECCatalogs catalogs, Action<TECController> updateExecute = null, Func<TECController, bool> updateCanExecute = null)
        {
            throw new NotImplementedException();
        }

        public static NetworkVM GetNetworkVMFromBid(TECBid bid, ChangeWatcher watcher)
        {
            List<INetworkConnectable> connectables = new List<INetworkConnectable>();
            connectables.AddRange(bid.GetAllInstanceControllers());
            connectables.AddRange(bid.GetAllInstanceSubScope());
            return new NetworkVM(connectables, watcher, bid.Catalogs);
        }

        public static NetworkVM GetNetworkVMFromTypical(TECTypical typ, TECCatalogs catalogs)
        {
            List<INetworkConnectable> connectables = new List<INetworkConnectable>();
            connectables.AddRange(typ.Controllers);
            connectables.AddRange(typ.GetAllSubScope());
            return new NetworkVM(connectables, typ.Watcher, catalogs, updateExecute, updateCanExecute);

            void updateExecute(TECController controller)
            {
                throw new NotImplementedException();
            }

            bool updateCanExecute(TECController controller)
            {
                throw new NotImplementedException();
            }
        }

        public static NetworkVM GetNetworkVMFromSystem(TECSystem sys, TECCatalogs catalogs)
        {
            List<INetworkConnectable> connectables = new List<INetworkConnectable>();
            connectables.AddRange(sys.Controllers);
            connectables.AddRange(sys.GetAllSubScope());
            return new NetworkVM(connectables, watcher, catalogs);
        }
    }
}
