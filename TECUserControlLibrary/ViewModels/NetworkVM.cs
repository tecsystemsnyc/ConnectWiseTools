using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using GalaSoft.MvvmLight;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TECUserControlLibrary.Models;
using TECUserControlLibrary.Utilities;

namespace TECUserControlLibrary.ViewModels
{
    public class NetworkVM : ViewModelBase, IDropTarget
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

            refreshPossibleParents();

            changeWatcher = new ChangeWatcher(bid);

            NetworkControllersVM.DragHandler = DragOver;
            NetworkControllersVM.DropHandler = Drop;

            UnitaryControllersVM.DragHandler = DragOver;
            UnitaryControllersVM.DropHandler = Drop;
        }

        public void Refresh(TECBid bid)
        {
            NetworkControllersVM.Refresh(bid);
            UnitaryControllersVM.Refresh();

            refreshPossibleParents();

            changeWatcher = new ChangeWatcher(bid);
        }

        private void sortController(TECController controller)
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

        private void removeController(TECController controller)
        {
            if (controller.NetworkType == NetworkType.DDC || controller.NetworkType == NetworkType.Server)
            {
                NetworkControllersVM.RemoveController(controller);
            }
            else
            {
                UnitaryControllersVM.RemoveController(controller);
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
                    else if (targetObject is TECSystem && referenceObject is TECSystem)
                    {
                        foreach(TECController controller in (targetObject as TECSystem).Controllers)
                        {
                            sortController(controller);
                        }
                    }
                }
                else if (args.PropertyName == "Remove" || args.PropertyName == "RemoveCatalog")
                {
                    if (targetObject is TECController && (referenceObject is TECBid || referenceObject is TECSystem))
                    {
                        removeController(targetObject as TECController);
                        (targetObject as TECController).RemoveAllConnections();
                    }
                    else if (targetObject is TECSystem && referenceObject is TECSystem)
                    {
                        foreach(TECController controller in (targetObject as TECSystem).Controllers)
                        {
                            removeController(controller);
                            controller.RemoveAllConnections();
                        }
                    }
                }
                else if (args.PropertyName == "NetworkType" && targetObject is TECController)
                {
                    refreshIsConnected();
                }
            }
        }

        private void refreshPossibleParents()
        {
            NetworkControllersVM.RefreshPossibleParents(NetworkControllersVM.NetworkControllers);
            UnitaryControllersVM.RefreshPossibleParents(NetworkControllersVM.NetworkControllers);
        }

        private void refreshIsConnected()
        {
            foreach (NetworkController netController in NetworkControllersVM.NetworkControllers)
            {
                netController.RefreshIsConnected();
            }
            foreach (NetworkController netController in UnitaryControllersVM.NetworkControllers)
            {
                netController.RefreshIsConnected();
            }
        }

        #region Drag/Drop
        public void DragOver(IDropInfo dropInfo)
        {
            var sourceItem = dropInfo.Data;
            Type sourceType;
            if (sourceItem is IList && ((IList)sourceItem).Count > 0)
            { sourceType = ((IList)sourceItem)[0].GetType(); }
            else
            { sourceType = sourceItem.GetType(); }
            Type targetType = dropInfo.TargetCollection.GetType().GetTypeInfo().GenericTypeArguments[0];

            if (sourceType == typeof(NetworkController))
            {
                if (targetType == typeof(TECController))
                {
                    dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                    dropInfo.Effects = System.Windows.DragDropEffects.Copy;
                }
                else
                {
                    UIHelpers.StandardDragOver(dropInfo);
                }
            }
        }

        public void Drop(IDropInfo dropInfo)
        {
            if (dropInfo.VisualTarget != dropInfo.DragInfo.VisualSource)
            {
                object sourceItem = dropInfo.Data;
                object targetCollection = dropInfo.TargetCollection;
                object sourceCollection = dropInfo.DragInfo.SourceCollection;

                if (sourceItem is IList)
                {
                    foreach (object item in ((IList)sourceItem))
                    {
                        handleDropItem(item, sourceCollection, targetCollection);
                    }
                }
                else
                {
                    handleDropItem(sourceItem, sourceCollection, targetCollection);
                }
            }
        }

        private void handleDropItem(object sourceItem, object sourceCollection, object targetCollection)
        {
            NetworkController netController = sourceItem as NetworkController;

            if (sourceCollection == NetworkControllersVM.NetworkControllers && targetCollection == UnitaryControllersVM.NetworkControllers)
            {
                NetworkControllersVM.NetworkControllers.Remove(netController);
                netController.Controller.NetworkType = NetworkType.Unitary;
                netController.Controller.RemoveAllConnections();
                UnitaryControllersVM.NetworkControllers.Add(netController);

                refreshPossibleParents();
            }
            else if (sourceCollection == UnitaryControllersVM.NetworkControllers)
            {
                if (targetCollection == NetworkControllersVM.NetworkControllers)
                {
                    UnitaryControllersVM.NetworkControllers.Remove(netController);
                    netController.Controller.NetworkType = NetworkType.DDC;
                    NetworkControllersVM.NetworkControllers.Add(netController);

                    refreshPossibleParents();
                }
                else if (targetCollection is ObservableCollection<TECController>)
                {
                    ObservableCollection<TECController> daisyChain = targetCollection as ObservableCollection<TECController>;

                    if (!daisyChain.Contains(netController.Controller))
                    {
                        TECController parentController = null;
                        TECNetworkConnection parentConnection = null;
                        foreach (NetworkController controller in NetworkControllersVM.NetworkControllers)
                        {
                            foreach (TECConnection connection in controller.Controller.ChildrenConnections)
                            {
                                if (connection is TECNetworkConnection && (connection as TECNetworkConnection).ChildrenControllers == daisyChain)
                                {
                                    parentConnection = (connection as TECNetworkConnection);
                                    parentController = controller.Controller;
                                    break;
                                }
                            }
                            if (parentController != null) break;
                        }
                        parentController.AddController(netController.Controller, parentConnection);
                    }
                }
            }
        }
        #endregion
    }
}
