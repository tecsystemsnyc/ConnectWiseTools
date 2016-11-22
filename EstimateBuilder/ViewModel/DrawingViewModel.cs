using GalaSoft.MvvmLight;
using EstimatingLibrary;
using System.Collections.ObjectModel;
using GongSolutions.Wpf.DragDrop;
using System.Windows;
using System;
using System.Windows.Input;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using EstimatingUtilitiesLibrary;

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
        public ObservableCollection<TECSystem> DisplaySystems
        {
            get { return _displaySystems; }
            set
            {
                _displaySystems = value;
                RaisePropertyChanged("DisplaySystems");
            }
        }

        
        public ICommand PreviousPageCommand { get; private set; }
        public ICommand NextPageCommand { get; private set; }

        public DrawingViewModel()
        {
            PreviousPageCommand = new RelayCommand(PreviousPageExecute);
            NextPageCommand = new RelayCommand(NextPageExecute);

            pageIndexes = new Dictionary<TECDrawing, int>();

            DisplaySystems = new ObservableCollection<TECSystem>();

            Bid = new TECBid();
            MessengerInstance.Register<GenericMessage<TECBid>>(this, PopulateBid);

            MessengerInstance.Send<NotificationMessage>(new NotificationMessage("DrawingViewModelLoaded"));
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
        #endregion

        #region Message Methods

        public void PopulateBid(GenericMessage<TECBid> genericMessage)
        {
            Bid.Systems.CollectionChanged -= Systems_CollectionChanged;
            Bid = genericMessage.Content;
            Bid.Systems.CollectionChanged += Systems_CollectionChanged;

            CurrentDrawing = null;
            pageIndexes.Clear();
            DisplaySystems = new ObservableCollection<TECSystem>();
            ObservableCollection<TECScope> checkSystems = new ObservableCollection<TECScope>();
            foreach(TECDrawing drawing in Bid.Drawings)
            {
                foreach(TECPage page in drawing.Pages)
                {
                    foreach(TECVisualScope scope in page.PageScope)
                    {
                        checkSystems.Add(scope.Scope);
                    }
                }
            }
            foreach(TECSystem system in Bid.Systems)
            {
                if (!checkSystems.Contains(system))
                {
                    DisplaySystems.Add(system);
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
            TECSystem sourceItem = dropInfo.Data as TECSystem;
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
            DisplaySystems.Remove(dropInfo.Data as TECSystem);
        }
        #endregion
        
        #region Event Handlers
        private void Systems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach(TECSystem item in e.OldItems)
                {
                    DisplaySystems.Remove(item);
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
                    DisplaySystems.Add(item);
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