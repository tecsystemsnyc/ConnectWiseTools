using EstimatingLibrary;
using GalaSoft.MvvmLight;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using TECUserControlLibrary.Models;

namespace EstimateBuilder.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ElectricalViewModel : ViewModelBase
    {
        private TECBid _bid;
        public TECBid Bid
        {
            get { return _bid; }
            set
            {
                _bid = value;
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

        public ElectricalViewModel(TECBid bid)
        {
            refresh(bid);
        }

        public void refresh(TECBid bid)
        {
            Bid = bid;
            ConduitTypeSelections = new ObservableCollection<TECConduitType>();
            var noneConduit = new TECConduitType();
            noneConduit.Name = "None";
            ConduitTypeSelections.Add(noneConduit);
            foreach (TECConduitType type in Bid.ConduitTypes)
            {
                ConduitTypeSelections.Add(type);
            }
            Bid.Controllers.CollectionChanged += collectionChanged;
            updateCollections();
            registerSubScope();
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
            if(e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach(object item in e.NewItems)
                {
                    if(item is TECSystem)
                    {
                        (item as TECSystem).Equipment.CollectionChanged += collectionChanged;
                    }
                    else if (item is TECEquipment)
                    {
                        (item as TECEquipment).SubScope.CollectionChanged += collectionChanged;
                    }
                }
            }
            else if(e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
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
        private void SubConnectionToAdd_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            handlePropertyChanged(e);
        }
        private void handlePropertyChanged(PropertyChangedEventArgs e)
        {
            if (e is PropertyChangedExtendedEventArgs<Object>)
            {
                PropertyChangedExtendedEventArgs<Object> args = e as PropertyChangedExtendedEventArgs<Object>;
                if (e.PropertyName == "Connection")
                {
                    if (args.OldValue != null)
                    {
                        var connectionToRemove = new TECSubScopeConnection();
                        foreach (TECSubScopeConnection connection in Bid.Connections)
                        {
                            if ((args.OldValue as TECConnection).Guid == connection.Guid)
                            {
                                connectionToRemove = connection;
                                break;
                            }
                        }
                        Bid.Connections.Remove(connectionToRemove);
                    }
                    if (args.NewValue != null)
                    {
                        Bid.Connections.Add(args.NewValue as TECConnection);
                    }
                }
            }
        }

        private void updateCollections()
        {
            updateSubScopeConnections();
            updateControllerSelections();
        }

        private void updateControllerSelections()
        {
            ControllerSelections = new ObservableCollection<TECController>();
            var noneController = new TECController();
            noneController.Name = "None";
            ControllerSelections.Add(noneController);
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
                        TECSubScope subScopetoAdd = subScope;
                        TECSubScopeConnection connectionToAdd = null;
                        TECController controllerToAdd = null;

                        foreach (TECSubScopeConnection connection in Bid.Connections)
                        {
                            if (connection.SubScope == subScope)
                            {
                                connectionToAdd = connection;
                                controllerToAdd = connection.ParentController;
                            }
                        }

                        var subConnectionToAdd = new SubScopeConnection(connectionToAdd, controllerToAdd, subScopetoAdd);

                        subConnectionToAdd.ParentSystem = system;
                        subConnectionToAdd.ParentEquipment = equipment;

                        SubScopeConnectionCollection.Add(subConnectionToAdd);

                        subConnectionToAdd.PropertyChanged += SubConnectionToAdd_PropertyChanged;
                    }
                }
            }
        }



    }
}