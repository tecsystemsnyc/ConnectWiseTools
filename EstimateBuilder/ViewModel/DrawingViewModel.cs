using GalaSoft.MvvmLight;
using EstimatingLibrary;
using System.Collections.ObjectModel;
using GongSolutions.Wpf.DragDrop;
using System.Windows;
using System;
using System.Windows.Input;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.CommandWpf;
using System.Collections.Generic;
using EstimatingUtilitiesLibrary;
using TECUserControlLibrary;

namespace EstimateBuilder.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class DrawingViewModel : ViewModelBase, IDropTarget
    {
        private TECBid _bid;
        private TECDrawing _currentDrawing;
        private TECPage _currentPage;
        private ObservableCollection<TECSystem> _displaySystems;
        private ObservableCollection<TECEquipment> _displayEquipment;
        private ObservableCollection<TECSubScope> _displaySubScope;
        private ObservableCollection<TECVisualConnection> _displayConnections;
        private string _controllerName;
        private TECController _selectedControllerTemplate;

        private Tuple<TECObject, TECVisualScope> connectionStart;

        private Dictionary<TECDrawing, int> pageIndexes;

        public TECBid Bid
        {
            get { return _bid; }
            set
            {
                _bid = value;
                RaisePropertyChanged("Bid");
                registerBid();
                populateDisplayed();
            }
        }
        public TECDrawing CurrentDrawing
        {
            get { return _currentDrawing; }
            set
            {
                _currentDrawing = value;
                RaisePropertyChanged("CurrentDrawing");
                if (CurrentDrawing != null)
                {
                    CurrentPage = CurrentDrawing.Pages[getIndex(CurrentDrawing)];
                }
                else
                {
                    CurrentPage = null;
                }
            }
        }
        public TECPage CurrentPage
        {
            get { return _currentPage; }
            set
            {
                _currentPage = value;
                RaisePropertyChanged("CurrentPage");
                RaisePropertyChanged("CurrentIndex");
            }
        }
        public int CurrentIndex
        {
            get
            {
                return getIndex(CurrentDrawing) + 1;
            }
            set
            {
                if (CurrentDrawing != null)
                {
                    int newPage = value;
                    if (newPage < 1)
                    {
                        newPage = 1;
                    }
                    else if (newPage >= CurrentDrawing.Pages.Count)
                    {
                        newPage = CurrentDrawing.Pages.Count;
                    }
                    pageIndexes[CurrentDrawing] = newPage - 1;
                    CurrentPage = CurrentDrawing.Pages[getIndex(CurrentDrawing)];
                    RaisePropertyChanged("CurrentIndex");
                    RaisePropertyChanged("CurrentPage");
                }
            }
        }
        public ObservableCollection<TECSystem> DisplaySystems
        {
            get { return _displaySystems; }
            set
            {
                _displaySystems = value;
                RaisePropertyChanged("DisplaySystems");
            }
        }
        public ObservableCollection<TECEquipment> DisplayEquipment
        {
            get { return _displayEquipment; }
            set
            {
                _displayEquipment = value;
                RaisePropertyChanged("DisplayEquipment");
            }
        }
        public ObservableCollection<TECSubScope> DisplaySubScope
        {
            get { return _displaySubScope; }
            set
            {
                _displaySubScope = value;
                RaisePropertyChanged("DisplaySubScope");
            }
        }
        public ObservableCollection<TECVisualConnection> DisplayConnections
        {
            get { return _displayConnections; }
            set
            {
                _displayConnections = value;
                RaisePropertyChanged("DisplayConnections");
            }
        }
        public string ControllerName {
            get { return _controllerName; }
            set
            {
                _controllerName = value;
                RaisePropertyChanged("ControllerName");
            }
        }
        public TECController SelectedControllerTemplate
        {
            get { return _selectedControllerTemplate; }
            set
            {
                _selectedControllerTemplate = value;
                RaisePropertyChanged("SelectedControllerTemplate");
            }
        }

        public ICommand PreviousPageCommand { get; private set; }
        public ICommand NextPageCommand { get; private set; }
        public ICommand ConnectCommand { get; private set; }
        public ICommand AddControllerCommand { get; private set; }

        private bool isConnecting;

        public DrawingViewModel()
        {
            isConnecting = false;

            PreviousPageCommand = new RelayCommand(PreviousPageExecute);
            NextPageCommand = new RelayCommand(NextPageExecute);
            ConnectCommand = new RelayCommand<Tuple<TECObject, TECVisualScope>>(vs => ConnectExecute(vs), vs => CanConnectExecute(vs));
            AddControllerCommand = new RelayCommand(AddControllerExecute);

            pageIndexes = new Dictionary<TECDrawing, int>();

            DisplaySystems = new ObservableCollection<TECSystem>();
            DisplayEquipment = new ObservableCollection<TECEquipment>();
            DisplaySubScope = new ObservableCollection<TECSubScope>();
            DisplayConnections = new ObservableCollection<TECVisualConnection>();

            Bid = new TECBid();
            registerBid();
            /*
            MessengerInstance.Register<GenericMessage<TECBid>>(this, PopulateBid);
            MessengerInstance.Send<NotificationMessage>(new NotificationMessage("DrawingViewModelLoaded"));
            */
        }
        
        #region Methods

        #region Commands

        private void PreviousPageExecute()
        {
            if (CurrentDrawing != null)
            {
                int index = getIndex(CurrentDrawing);
                if (index > 0)
                {
                    index -= 1;
                    pageIndexes[CurrentDrawing] = index;
                }

                CurrentPage = CurrentDrawing.Pages[index];
            }
        }

        private void NextPageExecute()
        {
            if (CurrentDrawing != null)
            {
                int index = getIndex(CurrentDrawing);
                if (index < (CurrentDrawing.Pages.Count - 1))
                {
                    index += 1;
                    pageIndexes[CurrentDrawing] = index;
                }

                CurrentPage = CurrentDrawing.Pages[index];
            }

            
        }

        private bool CanConnectExecute(Tuple<TECObject, TECVisualScope> arg)
        {
            if(arg != null)
            {
                if (!isConnecting)
                {
                    return canStartConnection(arg.Item1);
                }
                else
                {
                    var newConnection = createConnection(connectionStart, arg, 1.0);
                    return canAcceptConnection(arg.Item1, newConnection);
                }
            }
            else
            {
                return false;
            }
        }
        private void ConnectExecute(Tuple<TECObject, TECVisualScope> arg)
        {
            if (isConnecting)
            {
                Console.WriteLine("Ending");
                var newConnection = createConnection(connectionStart, arg, 1.0);
                connectConnections(connectionStart.Item1, arg.Item1, newConnection);
                Bid.Connections.Add(newConnection);
                bool isShown = false;
                foreach(TECVisualConnection vc in CurrentPage.Connections)
                {
                    if(((vc.Scope1 == connectionStart.Item2) && (vc.Scope2 == arg.Item2))
                        || ((vc.Scope2 == connectionStart.Item2) && (vc.Scope1 == arg.Item2)))
                    {
                        isShown = true;
                    }
                }
                if (!isShown)
                {
                    TECVisualConnection connectionToAdd = new TECVisualConnection(connectionStart.Item2, arg.Item2);
                    connectionToAdd.Connections = new ObservableCollection<TECConnection>();
                    connectionToAdd.Connections.Add(newConnection);
                    CurrentPage.Connections.Add(connectionToAdd);
                }
                isConnecting = false;
            }
            else
            {
                Console.WriteLine("Starting");
                connectionStart = arg;
                isConnecting = true;
            }
            
        }
        
        private void AddControllerExecute()
        {
            var newController = new TECController();
            newController.Name = ControllerName;
            newController.Types = SelectedControllerTemplate.Types;
            newController.Tags = SelectedControllerTemplate.Tags;
            newController.Description = SelectedControllerTemplate.Description;
            newController.Cost = SelectedControllerTemplate.Cost;
            Bid.Controllers.Add(newController);
        }
        #endregion

        #region Message Methods
        /*
        public void PopulateBid(GenericMessage<TECBid> genericMessage)
        {
            Bid.Systems.CollectionChanged -= Systems_CollectionChanged;
            Bid = genericMessage.Content;
            Bid.Systems.CollectionChanged += Systems_CollectionChanged;

            CurrentDrawing = null;
            pageIndexes.Clear();
            DisplayScope = new ObservableCollection<TECScope>();
            ObservableCollection<TECScope> checkScope = new ObservableCollection<TECScope>();
            foreach(TECDrawing drawing in Bid.Drawings)
            {
                foreach(TECPage page in drawing.Pages)
                {
                    foreach(TECVisualScope scope in page.PageScope)
                    {
                        checkScope.Add(scope.Scope);
                    }
                }
            }
            foreach(TECSystem system in Bid.Systems)
            {
                foreach(TECEquipment equipment in system.Equipment)
                {
                    foreach(TECSubScope sub in equipment.SubScope)
                    {
                        if (!checkScope.Contains(sub))
                        {
                            DisplayScope.Add(sub);
                        }
                    }
                }
            }
            
            if (Bid.Drawings.Count > 0)
            {
                foreach (TECDrawing drawing in Bid.Drawings)
                {
                    pageIndexes.Add(drawing, 0);
                }
                
                CurrentDrawing = Bid.Drawings[0];
                CurrentPage = CurrentDrawing.Pages[getIndex(CurrentDrawing)];
            }
        }
        */
        #endregion Message Methods

        #region Drag Drop
        void IDropTarget.DragOver(IDropInfo dropInfo)
        {
            TECScope sourceItem = dropInfo.Data as TECScope;
            var targetCollection = dropInfo.TargetCollection;
            if ((sourceItem != null) && (CurrentPage != null))
            {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                dropInfo.Effects = DragDropEffects.Copy;
            }
        }

        void IDropTarget.Drop(IDropInfo dropInfo)
        {
            TECScope sourceItem = dropInfo.Data as TECScope;
            Point dropPoint = dropInfo.DropPosition;
            TECVisualScope newScope = new TECVisualScope(sourceItem, dropPoint.X, dropPoint.Y);
            CurrentPage.PageScope.Add(newScope);
            /*
            if(sourceItem is TECSystem)
            {
                DisplayScope.Remove(dropInfo.Data as TECSystem);
            }
            */
        }
        #endregion
        
        #region Event Handlers
        private void collectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach(TECScope item in e.OldItems)
                {
                    if(item is TECSystem)
                    {
                        DisplaySystems.Remove(item as TECSystem);
                    } else if (item is TECEquipment)
                    {
                        DisplayEquipment.Remove(item as TECEquipment);
                    }
                    else if (item is TECSubScope)
                    {
                        DisplaySubScope.Remove(item as TECSubScope);
                    }
                  
                }
            } else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (Object item in e.NewItems)
                {
                    if (item is TECSystem)
                    {
                        DisplaySystems.Add(item as TECSystem);
                        ((TECSystem)item).Equipment.CollectionChanged += collectionChanged;
                        foreach (TECEquipment equipment in ((TECSystem)item).Equipment)
                        {
                            equipment.SubScope.CollectionChanged += collectionChanged;
                        }
                    }
                    else if (item is TECEquipment)
                    {
                        DisplayEquipment.Add(item as TECEquipment);
                        ((TECEquipment)item).SubScope.CollectionChanged += collectionChanged;
                    }
                    else if (item is TECSubScope)
                    {
                        DisplaySubScope.Add(item as TECSubScope);
                    }

                }
            }
        }
        #endregion

        #region Helper Methods

        int getIndex(TECDrawing drawing)
        {
            if (drawing != null)
            {
                int index;
                if (pageIndexes.TryGetValue(drawing, out index))
                {
                    return index;
                }
                else
                {
                    pageIndexes.Add(drawing, 0);
                    return 0;
                }
            }
            else
            {
                return 0;
            }
            
        }
        private void registerBid()
        {
            Bid.Systems.CollectionChanged += collectionChanged;
            foreach (TECSystem system in Bid.Systems)
            {
                system.Equipment.CollectionChanged += collectionChanged;
                foreach(TECEquipment equipment in system.Equipment)
                {
                    equipment.SubScope.CollectionChanged += collectionChanged;
                }
            }
        }
        private void populateDisplayed()
        {
            Console.WriteLine("populating");
            foreach (TECSystem system in Bid.Systems)
            {
                DisplaySystems.Add(system);
                foreach (TECEquipment equipment in system.Equipment)
                {
                    DisplayEquipment.Add(equipment);
                    foreach(TECSubScope subScope  in equipment.SubScope)
                    {
                        DisplaySubScope.Add(subScope);
                    }
                }
            }

            CurrentDrawing = null;
            pageIndexes.Clear();
            ObservableCollection<TECScope> checkScope = new ObservableCollection<TECScope>();
            foreach (TECDrawing drawing in Bid.Drawings)
            {
                foreach (TECPage page in drawing.Pages)
                {
                    foreach (TECVisualScope scope in page.PageScope)
                    {
                        checkScope.Add(scope.Scope);
                    }
                }
            }
            

            if (Bid.Drawings.Count > 0)
            {
                foreach (TECDrawing drawing in Bid.Drawings)
                {
                    pageIndexes.Add(drawing, 0);
                }

                CurrentDrawing = Bid.Drawings[0];
                CurrentPage = CurrentDrawing.Pages[getIndex(CurrentDrawing)];
            }
        }

        private bool canAcceptConnection(TECObject item, TECConnection connection)
        {
            bool canConnect = false;

            if(item is TECController)
            {
                var controller = item as TECController;

                var availableConnections = controller.AvailableConnections;
                
                foreach(ConnectionType conType in connection.ConnectionTypes)
                {
                    if (availableConnections.Contains(conType))
                    {
                        availableConnections.Remove(conType);
                        canConnect = true;
                    }
                    else
                    {
                        canConnect = false;
                    }
                }
            } 
            else if (item is TECSubScope)
            {
                var subScope = item as TECSubScope;
                if(subScope.Connection == null)
                {
                    var availableConnections = subScope.AvailableConnections;

                    foreach (ConnectionType conType in connection.ConnectionTypes)
                    {
                        if (availableConnections.Contains(conType))
                        {
                            availableConnections.Remove(conType);
                            canConnect = true;
                        }
                        else
                        {
                            canConnect = false;
                        }
                    }
                }
                else
                {
                    canConnect = false;
                }
            }

            return canConnect;
        }
        private bool canStartConnection(TECObject item)
        {
            bool canStart = false;
            if(item is TECController)
            {
                var controller = item as TECController;
                var avaiableConnections = controller.AvailableConnections;
                if(avaiableConnections.Count > 0)
                {
                    canStart = true;
                }
            } else if(item is TECSubScope)
            {
                var subScope = item as TECSubScope;
                if(subScope.Connection == null)
                {
                    canStart = true;
                }
            }
            
            return canStart;
        }

        private TECConnection createConnection(Tuple<TECObject, TECVisualScope> item1, Tuple<TECObject, TECVisualScope> item2, double scale)
        {
            double length = UtilitiesMethods.getLength(item1.Item2, item2.Item2, scale);
            var newConnection = new TECConnection();
            newConnection.Length = length;
            if (item1.Item1 is TECController)
            {
                newConnection.Controller = item1.Item1 as TECController;
                if(item2.Item1 is TECSubScope)
                {
                    var sub = item2.Item1 as TECSubScope;
                    foreach (ConnectionType type in sub.ConnectionTypes)
                    {
                        newConnection.ConnectionTypes.Add(type);
                    }
                }
                else
                {
                    var controller = item2.Item1 as TECController;
                    foreach (ConnectionType type in controller.AvailableConnections)
                    {
                        newConnection.ConnectionTypes.Add(type);
                    }
                }
                
            }
            else
            {
                newConnection.Controller = item2.Item1 as TECController;
                var sub = item1.Item1 as TECSubScope;
                foreach (ConnectionType type in sub.ConnectionTypes)
                {
                    newConnection.ConnectionTypes.Add(type);
                }
            }
            newConnection.Scope.Add(item2.Item1 as TECScope);

            return newConnection;
        }

        private void connectConnections(TECObject item1, TECObject item2, TECConnection connection)
        {
            if(item1 is TECController)
            {
                ((TECController)item1).Connections.Add(connection);
            } else if (item1 is TECSubScope)
            {
                ((TECSubScope)item1).Connection = connection;
            }

            if (item2 is TECController)
            {
                ((TECController)item2).Connections.Add(connection);
            }
            else if (item2 is TECSubScope)
            {
                ((TECSubScope)item2).Connection = connection;
            }
        }
        
        #endregion Helper Methods

        #endregion
    }
}