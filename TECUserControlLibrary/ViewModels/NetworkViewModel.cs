using EstimatingLibrary;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections;
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

        private ObservableCollection<TECController> _bmsControllers;
        public ObservableCollection<TECController> BMSControllers
        {
            get { return _bmsControllers; }
            set
            {
                _bmsControllers = value;
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

        public IOType SelectedIO { get; set; }
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

            NetworkControllers.CollectionChanged += NetworkControllers_CollectionChanged;
        }

        #region Methods
        private void update()
        {
            //Reset Collections
            ServerControllers = new ObservableCollection<TECController>();
            BMSControllers = new ObservableCollection<TECController>();
            StandaloneControllers = new ObservableCollection<TECController>();
            NetworkControllers = new ObservableCollection<TECController>();

            Bid.Controllers.CollectionChanged += Controllers_CollectionChanged;
            ServerControllers.CollectionChanged += ServerControllers_CollectionChanged;
            BMSControllers.CollectionChanged += BMSControllers_CollectionChanged;

            //Sort all controllers
            foreach (TECController controller in Bid.Controllers)
            {
                sortAndAddController(controller);
            }

            //Setup network collection
            TECController noneController = new TECController();
            noneController.Name = "None";
            NetworkControllers.Add(noneController);

            foreach (TECController controller in ServerControllers)
            {
                NetworkControllers.Add(controller);
            }
            foreach (TECController controller in BMSControllers)
            {
                NetworkControllers.Add(controller);
            }
        }

        private void sortAndAddController(TECController controller)
        {
            if (controller.IsServer)
            {
                ServerControllers.Add(controller);
            }
            else if (controller.IsBMS)
            {
                BMSControllers.Add(controller);
            }
            else if (controller.ParentConnection == null)
            {
                StandaloneControllers.Add(controller);
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
            if (BMSControllers.Contains(controller))
            {
                BMSControllers.Remove(controller);
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
            TECNetworkConnection newConnection = new TECNetworkConnection();
            newConnection.IOType = SelectedIO;
            newConnection.ParentController = controller;
            controller.ChildrenConnections.Add(newConnection);
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

        private void NetworkControllers_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (object item in e.NewItems)
                {
                    if (item is TECController)
                    {
                        NetworkControllers.Add(item as TECController);
                        (item as TECController).PropertyChanged += NetworkController_PropertyChanged;
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
                        (item as TECController).PropertyChanged -= NetworkController_PropertyChanged;
                    }
                }
            }
        }

        private void NetworkController_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ParentController")
            {
                TECController controller = (sender as TECController);
                controller.ParentConnection.IOType = controller.ParentConnection.PossibleIO[0];
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
                if (sourceType == typeof(TECController))
                {
                    removeController(sourceItem as TECController);
                }
                else if (sourceType == typeof(TECConnection))
                {
                    return;
                }
                else
                {
                    throw new NotImplementedException();
                }

                //Handle addition to target collection
                TECController sourceController = null;
                if (sourceItem is TECController)
                {
                    sourceController = (sourceItem as TECController);
                }

                if (targetType == typeof(TECController))
                {
                    if (targetCollection == ServerControllers)
                    {
                        sourceController.IsServer = true;
                        sortAndAddController(sourceController);
                    }
                    else if (targetCollection == BMSControllers)
                    {
                        sourceController.IsBMS = true;
                        sourceController.IsServer = false;
                        sortAndAddController(sourceController);
                    }
                    else if (targetCollection == StandaloneControllers)
                    {
                        sourceController.IsBMS = false;
                        sortAndAddController(sourceController);
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
                else if (targetType == typeof(TECScope))
                {
                    sourceController.IsBMS = false;
                    ((IList)targetCollection).Add(sourceController);
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