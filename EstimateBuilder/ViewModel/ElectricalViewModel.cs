using EstimatingLibrary;
using GalaSoft.MvvmLight;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using TECUserControlLibrary;
using TECUserControlLibrary.Models;

namespace EstimateBuilder.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ElectricalViewModel : ViewModelBase, IDropTarget
    {
        private TECBid _bid;
        public TECBid Bid
        {
            get { return _bid; }
            set
            {
                _bid = value;
                registerSubScope();
                RaisePropertyChanged("Bid");
            }
        }

        private ObservableCollection<TECController> _controllerSelections;
        public ObservableCollection<TECController> ControllerSelections
        {
            get { return _controllerSelections; }
            set
            {
                _controllerSelections = value;
                RaisePropertyChanged("ControllerSelections");
            }
        }
        private ObservableCollection<SubScopeConnection> _subScopeConnectionCollection;
        public ObservableCollection<SubScopeConnection> SubScopeConnectionCollection
        {
            get { return _subScopeConnectionCollection; }
            set
            {
                _subScopeConnectionCollection = value;
                RaisePropertyChanged("SubScopeConnectionCollection");
            }
        }
        private ObservableCollection<TECConduitType> _conduitTypeSelections;
        public ObservableCollection<TECConduitType> ConduitTypeSelections
        {
            get { return _conduitTypeSelections; }
            set
            {
                _conduitTypeSelections = value;
                RaisePropertyChanged("ConduitTypeSelections");
            }
        }

        private NoneObjects NoneContainer;

        public ElectricalViewModel(TECBid bid, NoneObjects noneContainer)
        {
            NoneContainer = noneContainer;
            Refresh(bid);
        }

        public void Refresh(TECBid bid)
        {
            Bid = bid;
            ConduitTypeSelections = new ObservableCollection<TECConduitType>();
            var noneConduit = new TECConduitType();
            noneConduit.Name = "None";
            ConduitTypeSelections.Add(noneConduit);
            foreach (TECConduitType type in Bid.Catalogs.ConduitTypes)
            {
                ConduitTypeSelections.Add(type);
            }
            Bid.Controllers.CollectionChanged += collectionChanged;
            updateCollections();
        }
        private void registerSubScope()
        {
            Bid.Systems.CollectionChanged += collectionChanged;
            foreach (TECSystem sys in Bid.Systems)
            {
                sys.Equipment.CollectionChanged += collectionChanged;

                foreach (TECEquipment equip in sys.Equipment)
                {
                    equip.SubScope.CollectionChanged += collectionChanged;
                }
            }
        }
        private void collectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (object item in e.NewItems)
                {
                    if (item is TECSystem)
                    {
                        (item as TECSystem).Equipment.CollectionChanged += collectionChanged;
                        foreach (TECEquipment equip in (item as TECSystem).Equipment)
                        {
                            equip.SubScope.CollectionChanged += collectionChanged;
                        }
                    }
                    else if (item is TECEquipment)
                    {
                        (item as TECEquipment).SubScope.CollectionChanged += collectionChanged;
                    }
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    if (item is TECSystem)
                    {
                        (item as TECSystem).Equipment.CollectionChanged -= collectionChanged;
                    }
                    else if (item is TECEquipment)
                    {
                        (item as TECEquipment).SubScope.CollectionChanged -= collectionChanged;
                    }
                }
            }
            updateCollections();
        }

        private void updateCollections()
        {
            updateSubScopeConnections();
            updateControllerSelections();
        }

        private void updateControllerSelections()
        {
            ControllerSelections = new ObservableCollection<TECController>();
            ControllerSelections.Add(NoneContainer.NoneController);
            foreach (TECController controller in Bid.Controllers)
            {
                ControllerSelections.Add(controller);
            }
        }
        private void updateSubScopeConnections()
        {
            SubScopeConnectionCollection = new ObservableCollection<SubScopeConnection>();
            foreach (TECSystem system in Bid.Systems)
            {
                foreach (TECEquipment equipment in system.Equipment)
                {
                    foreach (TECSubScope subScope in equipment.SubScope)
                    {
                        var subConnectionToAdd = new SubScopeConnection(subScope);

                        subConnectionToAdd.ParentSystem = system;
                        subConnectionToAdd.ParentEquipment = equipment;

                        SubScopeConnectionCollection.Add(subConnectionToAdd);
                    }
                }
            }
        }

        public void DragOver(IDropInfo dropInfo)
        {
        }

        public void Drop(IDropInfo dropInfo)
        {
        }
    }
}