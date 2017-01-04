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
        private ObservableCollection<TECScope> _displayScope;
        private ObservableCollection<TECVisualConnection> _displayConnections;

        private TECVisualScope startingVS;

        private Dictionary<TECDrawing, int> pageIndexes;

        public TECBid Bid
        {
            get { return _bid; }
            set
            {
                _bid = value;
                RaisePropertyChanged("Bid");
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
        public ObservableCollection<TECScope> DisplayScope
        {
            get { return _displayScope; }
            set
            {
                _displayScope = value;
                RaisePropertyChanged("DisplayScope");
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

        public ICommand PreviousPageCommand { get; private set; }
        public ICommand NextPageCommand { get; private set; }
        public ICommand ConnectCommand { get; private set; }
        public ICommand AddControllerCommand { get; private set; }

        private bool isConnecting;

        private string _connectingText;
        public string ConnectingText
        {
            get { return _connectingText; }
            set
            {
                _connectingText = value;
                RaisePropertyChanged("ConnectingText");
            }
        }

        public DrawingViewModel()
        {
            isConnecting = false;
            ConnectingText = "Start Connection";

            PreviousPageCommand = new RelayCommand(PreviousPageExecute);
            NextPageCommand = new RelayCommand(NextPageExecute);
            ConnectCommand = new RelayCommand<TECVisualScope>(vs => ConnectExecute(vs), vs => CanConnectExecute(vs));
            AddControllerCommand = new RelayCommand(AddControllerExecute);

            pageIndexes = new Dictionary<TECDrawing, int>();

            DisplayScope = new ObservableCollection<TECScope>();
            DisplayConnections = new ObservableCollection<TECVisualConnection>();

            Bid = new TECBid();
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

        private bool CanConnectExecute(TECVisualScope arg)
        {
            if(arg != null)
            {
                if (startingVS == null)
                {
                    return true;
                }
                else if (startingVS.Scope is TECController)
                {
                    return true;
                }
                else if (!(startingVS.Scope is TECController) && (arg.Scope is TECController))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        private void ConnectExecute(TECVisualScope vs)
        {
            if (isConnecting)
            {
                Console.WriteLine("Ending");
                double length = UtilitiesMethods.getLength(startingVS, vs, 1.0);
                var newConnection = new TECConnection();
                newConnection.Length = length;
                if(startingVS.Scope is TECController)
                {
                    newConnection.Controller = startingVS.Scope as TECController;
                } else
                {
                    newConnection.Controller = vs.Scope as TECController;
                }
                newConnection.Scope.Add(vs.Scope);
                Bid.Connections.Add(newConnection);
                TECVisualConnection connectionToAdd = new TECVisualConnection(startingVS, vs);
                connectionToAdd.Connections.Add(newConnection);
                CurrentPage.Connections.Add(connectionToAdd);
                ConnectingText = "Start Connection";
                isConnecting = false;
            }
            else
            {
                Console.WriteLine("Starting");
                startingVS = vs;
                ConnectingText = "End Connection";
                isConnecting = true;
            }
            
        }
        
        private void AddControllerExecute()
        {
            Bid.Controllers.Add(new TECController("Controller", "", Guid.NewGuid(), 100));
            Console.WriteLine("Controller Added");
        }
        #endregion

        #region Message Methods

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
            if(sourceItem is TECSystem)
            {
                DisplayScope.Remove(dropInfo.Data as TECSystem);
            }
            
        }
        #endregion
        
        #region Event Handlers
        private void Systems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach(TECScope item in e.OldItems)
                {
                    DisplayScope.Remove(item);
                    foreach(TECDrawing drawing in Bid.Drawings)
                    {
                        foreach(TECPage page in drawing.Pages)
                        {
                            var vScopeToRemove = new List<TECVisualScope>();
                            foreach(TECVisualScope scope in page.PageScope)
                            {
                                if(scope.Scope == item)
                                {
                                    vScopeToRemove.Add(scope);
                                }
                            }
                            foreach(TECVisualScope rScope in vScopeToRemove)
                            {
                                page.PageScope.Remove(rScope);
                            }
                        }
                    }
                }
            } else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (TECSystem item in e.NewItems)
                {
                    DisplayScope.Add(item);
                    foreach (TECEquipment equipment in item.Equipment)
                    {
                        DisplayScope.Add(equipment);
                        foreach (TECSubScope sub in equipment.SubScope)
                        {
                            DisplayScope.Add(sub);
                        }
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
        
        #endregion Helper Methods

        #endregion
    }
}