using EstimatingLibrary;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TECUserControlLibrary.Models;

namespace TECUserControlLibrary.ViewModels
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class NetworkViewModel : ViewModelBase, IDropTarget
    {
        #region Properties
        private TECBid _bid;
        public TECBid Bid
        {
            get { return _bid; }
            set
            {
                _bid = value;
                RaisePropertyChanged("Bid");
                update();
            }
        }

        private ObservableCollection<TECController> _serverControllers;
        public ObservableCollection<TECController> ServerControllers
        {
            get { return _serverControllers; }
            set
            {
                _serverControllers = value;
                RaisePropertyChanged("ServerControllers");
            }
        }

        private ObservableCollection<BMSController> _bmsControllers;
        public ObservableCollection<BMSController> BMSControllers
        {
            get { return _bmsControllers; }
            set
            {
                if (BMSControllers != null)
                {
                    foreach (BMSController controller in BMSControllers)
                    {
                        controller.PropertyChanged -= BMSController_PropertyChanged;
                    }
                }
                _bmsControllers = value;
                foreach (BMSController controller in BMSControllers)
                {
                    controller.PropertyChanged += BMSController_PropertyChanged;
                }
                RaisePropertyChanged("BMSControllers");
            }
        }

        private ObservableCollection<TECController> _standaloneControllers;
        public ObservableCollection<TECController> StandaloneControllers
        {
            get { return _standaloneControllers; }
            set
            {
                _standaloneControllers = value;
                RaisePropertyChanged("StandaloneControllers");
            }
        }

        private ObservableCollection<TECController> _networkControllers;
        public ObservableCollection<TECController> NetworkControllers
        {
            get { return _networkControllers; }
            set
            {
                _networkControllers = value;
                RaisePropertyChanged("NetworkControllers");
            }
        }

        private ObservableCollection<TECConduitType> _possibleConduitTypes;
        public ObservableCollection<TECConduitType> PossibleConduitTypes
        {
            get { return _possibleConduitTypes; }
            set
            {
                _possibleConduitTypes = value;
                RaisePropertyChanged("PossibleConduitTypes");
            }
        }

        public IOType SelectedIO { get; set; }
        public TECConnectionType SelectedWire { get; set; }
        #endregion

        #region Commands

        public ICommand AddConnectionCommand { get; private set; }
        public ICommand ParentControllerChangedCommand { get; private set; }

        #endregion

        public NetworkViewModel(TECBid bid)
        {
            _bid = bid;

            update();

            AddConnectionCommand = new RelayCommand<TECController>(x => AddConnectionExecute(x));
        }

        #region Methods
        public void Refresh(TECBid bid)
        {
            Bid = bid;
        }

        private void update()
        {
            //Reset Collections
            ServerControllers = new ObservableCollection<TECController>();
            BMSControllers = new ObservableCollection<BMSController>();
            StandaloneControllers = new ObservableCollection<TECController>();
            NetworkControllers = new ObservableCollection<TECController>();
            PossibleConduitTypes = new ObservableCollection<TECConduitType>();

            Bid.Controllers.CollectionChanged += Controllers_CollectionChanged;
            Bid.ConduitTypes.CollectionChanged += ConduitTypes_CollectionChanged;
            ServerControllers.CollectionChanged += ServerControllers_CollectionChanged;
            BMSControllers.CollectionChanged += BMSControllers_CollectionChanged;

            NetworkControllers.CollectionChanged += NetworkControllers_CollectionChanged;

            //Sort all controllers
            foreach (TECController controller in Bid.Controllers)
            {
                sortAndAddController(controller);
            }

            TECConduitType noneConduit = new TECConduitType();
            noneConduit.Name = "None";
            PossibleConduitTypes.Add(noneConduit);
            foreach (TECConduitType type in Bid.ConduitTypes)
            {
                PossibleConduitTypes.Add(type);
            }
        }

        

        private void sortAndAddController(TECController controller)
        {
            if (controller.IsServer)
            {
                ServerControllers.Add(controller);
            }
            else if (controller.IsBMS || controller.ChildNetworkConnections.Count > 0)
            {
                controller.Type = ControllerType.IsBMS;
                BMSController newBMSController = new BMSController(controller, NetworkControllers);
                BMSControllers.Add(newBMSController);
            }
            else if (controller.ParentConnection == null)
            {
                StandaloneControllers.Add(controller);
                controller.Type = ControllerType.IsStandalone;
            }
            else
            {
                controller.Type = ControllerType.IsNetworked;
            }
        }

        

        private void removeController(TECController controller)
        {
            controller.ParentConnection = null;

            //Remove from server controllers
            if (ServerControllers.Contains(controller))
            {
                ServerControllers.Remove(controller);
            }

            //Remove from BMS controllers
            BMSController controllerToRemove = null;
            foreach (BMSController bmsController in BMSControllers)
            {
                if (bmsController.Controller == controller)
                {
                    controllerToRemove = bmsController;
                    List<TECController> childrenToRemove = new List<TECController>();
                    foreach (TECNetworkConnection netConnect in controller.ChildNetworkConnections)
                    {
                        foreach (TECController control in netConnect.ChildrenControllers)
                        {
                            if (control.Type == ControllerType.IsNetworked)
                            {
                                childrenToRemove.Add(control);
                            }
                        }
                    }
                    foreach (TECController control in childrenToRemove)
                    {
                        controller.RemoveController(control);
                        sortAndAddController(control);
                    }

                    break;
                }
            }
            if (controllerToRemove != null)
            {
                BMSControllers.Remove(controllerToRemove);
            }

            //Remove from standalone controllers
            if (StandaloneControllers.Contains(controller))
            {
                StandaloneControllers.Remove(controller);
            }
        }
        #endregion

        #region Commands Methods

        private void AddConnectionExecute(TECController controller)
        {
            if (SelectedIO != 0 && SelectedWire != null)
            {
                TECNetworkConnection newConnection = new TECNetworkConnection();
                newConnection.IOType = SelectedIO;
                newConnection.ConnectionType = SelectedWire;
                newConnection.ParentController = controller;
                controller.ChildrenConnections.Add(newConnection);
            }
        }

        #endregion

        #region Event Handlers

        private void Controllers_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (object item in e.NewItems)
                {
                    if (item is TECController)
                    {
                        sortAndAddController(item as TECController);
                    }
                }
            }
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    if (item is TECController)
                    {
                        removeController(item as TECController);
                    }
                }
            }
        }

        private void ConduitTypes_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (object item in e.NewItems)
                {
                    if (item is TECConduitType)
                    {
                        PossibleConduitTypes.Add(item as TECConduitType);
                    }
                }
            }
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    if (item is TECController)
                    {
                        PossibleConduitTypes.Remove(item as TECConduitType);
                    }
                }
            }
        }

        private void ServerControllers_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (object item in e.NewItems)
                {
                    if (item is TECController)
                    {
                        NetworkControllers.Add(item as TECController);
                    }
                }
            }
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    if (item is TECController)
                    {
                        NetworkControllers.Remove(item as TECController);
                    }
                }
            }
        }

        private void BMSControllers_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (object item in e.NewItems)
                {
                    if (item is BMSController)
                    {
                        BMSController newBMSController = (item as BMSController);
                        newBMSController.PropertyChanged += BMSController_PropertyChanged;
                        NetworkControllers.Add(newBMSController.Controller);
                    }
                }
            }
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    if (item is BMSController)
                    {
                        BMSController oldBMSController = (item as BMSController);
                        oldBMSController.PropertyChanged -= BMSController_PropertyChanged;
                        NetworkControllers.Remove((item as BMSController).Controller);
                    }
                }
            }
        }

        private void NetworkControllers_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (object item in e.NewItems)
                {
                    if (item is TECController)
                    {
                        (item as TECController).PropertyChanged += NetworkController_PropertyChanged;
                        foreach (BMSController bmsController in BMSControllers)
                        {
                            bmsController.PossibleParents = NetworkControllers;
                        }
                    }
                }
            }
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    if (item is TECController)
                    {
                        (item as TECController).PropertyChanged -= NetworkController_PropertyChanged;
                        foreach (BMSController bmsController in BMSControllers)
                        {
                            bmsController.PossibleParents = NetworkControllers;
                        }
                    }
                }
            }
        }

        private void NetworkController_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ParentController")
            {
                TECController controller = (sender as TECController);
                if (controller.ParentConnection != null)
                {
                    if (controller.ParentConnection.PossibleIO.Count > 0)
                    {
                        if (controller.ParentConnection.PossibleIO.Contains(IOType.BACnetIP))
                        {
                            controller.ParentConnection.IOType = IOType.BACnetIP;
                        }
                        else
                        {
                            controller.ParentConnection.IOType = controller.ParentConnection.PossibleIO[0];
                        }
                    }
                    if (Bid.ConnectionTypes.Count > 0)
                    {
                        controller.ParentConnection.ConnectionType = Bid.ConnectionTypes[0];
                    }
                }
            }
        }

        private void BMSController_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ParentController")
            {
                foreach (BMSController bmsController in BMSControllers)
                {
                    bmsController.RaiseIsConnected();
                    bmsController.PossibleParents = NetworkControllers;
                }
            }
        }

        #endregion

        #region Drag Drop

        public void DragOver(IDropInfo dropInfo)
        {
            object sourceItem = dropInfo.Data;
            object targetCollection = dropInfo.TargetCollection;

            Type sourceType = sourceItem.GetType();
            Type targetType = targetCollection.GetType().GetTypeInfo().GenericTypeArguments[0];

            if (sourceItem != null && !(sourceItem is TECConnection))
            {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                dropInfo.Effects = DragDropEffects.Move;
            }
        }

        public void Drop(IDropInfo dropInfo)
        {
            if (dropInfo.VisualTarget != dropInfo.DragInfo.VisualSource)
            {
                object sourceItem = dropInfo.Data;
                object targetCollection = dropInfo.TargetCollection;
                object sourceCollection = dropInfo.DragInfo.SourceCollection;

                Type sourceType = sourceItem.GetType();
                Type targetType = targetCollection.GetType().GetTypeInfo().GenericTypeArguments[0];

                //Handle removal from source collection
                if (sourceCollection == ServerControllers)
                {
                    ServerControllers.Remove(sourceItem as TECController);
                }
                else if (sourceCollection == BMSControllers)
                {
                    BMSControllers.Remove(sourceItem as BMSController);
                }
                else if (sourceCollection == StandaloneControllers)
                {
                    StandaloneControllers.Remove(sourceItem as TECController);
                }
                else if (sourceType == typeof(TECConnection))
                {
                    return;
                }
                else
                {
                    bool foundCollection = false;
                    List<TECController> parentControllers = new List<TECController>();
                    foreach (TECController control in ServerControllers)
                    {
                        parentControllers.Add(control);
                    }
                    foreach (BMSController control in BMSControllers)
                    {
                        parentControllers.Add(control.Controller);
                    }
                    //See if source collection is a child controller connection
                    foreach (TECController parent in parentControllers)
                    {
                        foreach (TECNetworkConnection connection in parent.ChildrenConnections)
                        {
                            if (sourceCollection == connection.ChildrenControllers)
                            {
                                foundCollection = true;
                                parent.RemoveController(sourceItem as TECController);
                                break;
                            }
                        }
                        if (foundCollection) break;
                    }
                    if (!foundCollection)
                    {
                        throw new NotImplementedException();
                    }
                }

                //Handle addition to target collection
                TECController sourceController = null;
                if (sourceItem is TECController)
                {
                    sourceController = (sourceItem as TECController);
                }
                else if (sourceItem is BMSController)
                {
                    sourceController = (sourceItem as BMSController).Controller;
                }

                if (targetType == typeof(TECController))
                {
                    if (targetCollection == ServerControllers)
                    {
                        sourceController.Type = ControllerType.IsServer;
                        sortAndAddController(sourceController);
                    }
                    else if (targetCollection == StandaloneControllers)
                    {
                        sourceController.Type = ControllerType.IsStandalone;
                        sortAndAddController(sourceController);
                    }
                    else
                    {
                        bool foundCollection = false;
                        //See if target collection is a child controller connection
                        foreach (BMSController bmsController in BMSControllers)
                        {
                            foreach (TECNetworkConnection connection in bmsController.Controller.ChildrenConnections)
                            {
                                if (targetCollection == connection.ChildrenControllers)
                                {
                                    foundCollection = true;
                                    sourceController.Type = ControllerType.IsNetworked;
                                    connection.ChildrenControllers.Add(sourceController);
                                    break;
                                }
                            }
                            if (foundCollection) break;
                        }
                    }
                }
                else if (targetType == typeof(BMSController))
                {
                    sourceController.Type = ControllerType.IsBMS;
                    sortAndAddController(sourceController);
                }
                else if (targetType == typeof(TECConnection))
                {
                    return;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        #endregion
    }
}