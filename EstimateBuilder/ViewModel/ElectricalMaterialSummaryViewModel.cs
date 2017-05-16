using EstimateBuilder.Model;
using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace EstimateBuilder.ViewModel
{
    public class ElectricalMaterialSummaryViewModel : ViewModelBase
    {
        #region Properties
        private ChangeWatcher _changeWatcher;
        private ChangeWatcher changeWatcher
        {
            get { return _changeWatcher; }
            set
            {
                if (_changeWatcher != null)
                {
                    _changeWatcher.Changed -= bidChanged;
                }
                _changeWatcher = value;
                _changeWatcher.Changed += bidChanged;
            }
        }

        private ObservableCollection<LengthSummaryItem> _wireSummaryItems;
        public ObservableCollection<LengthSummaryItem> WireSummaryItems
        {
            get { return _wireSummaryItems; }
            set
            {
                _wireSummaryItems = value;
                RaisePropertyChanged("WireSummaryItems");
            }
        }

        private ObservableCollection<LengthSummaryItem> _conduitSummaryItems;
        public ObservableCollection<LengthSummaryItem> ConduitSummaryItems
        {
            get { return _conduitSummaryItems; }
            set
            {
                _conduitSummaryItems = value;
                RaisePropertyChanged("ConduitSummaryItems");
            }
        }

        private ObservableCollection<AssociatedCostSummaryItem> _associatedCostSummaryItems;
        public ObservableCollection<AssociatedCostSummaryItem> AssociatedCostSummaryItems
        {
            get { return _associatedCostSummaryItems; }
            set
            {
                _associatedCostSummaryItems = value;
                RaisePropertyChanged("AssociatedCostSummaryItems");
            }
        }

        private ObservableCollection<TECMiscWiring> _miscWiring;
        public ObservableCollection<TECMiscWiring> MiscWiring
        {
            get { return _miscWiring; }
            set
            {
                _miscWiring = value;
                RaisePropertyChanged("MiscWiring");
            }
        }
        
        private Dictionary<Guid, LengthSummaryItem> wireDictionary;
        private Dictionary<Guid, LengthSummaryItem> conduitDictionary;
        private Dictionary<Guid, AssociatedCostSummaryItem> associatedCostDictionary;

        private double _totalMiscWiring;
        public double TotalMiscWiring
        {
            get { return _totalMiscWiring; }
            set
            {
                _totalMiscWiring = value;
                RaisePropertyChanged("TotalMiscWiring");
                RaisePropertyChanged("TotalElectricalCost");
            }
        }

        private double _totalWireCost;
        public double TotalWireCost
        {
            get { return _totalWireCost; }
            set
            {
                _totalWireCost = value;
                RaisePropertyChanged("TotalWireCost");
                RaisePropertyChanged("TotalElectricalCost");
            }
        }

        private double _totalWireHours;
        public double TotalWireHours
        {
            get { return _totalWireHours; }
            set
            {
                _totalWireHours = value;
                RaisePropertyChanged("TotalWireHours");
                RaisePropertyChanged("TotalElectricalHours");
            }
        }

        private double _totalConduitCost;
        public double TotalConduitCost
        {
            get { return _totalConduitCost; }
            set
            {
                _totalConduitCost = value;
                RaisePropertyChanged("TotalConduitCost");
                RaisePropertyChanged("TotalElectricalCost");
            }
        }

        private double _totalConduitHours;
        public double TotalConduitHours
        {
            get { return _totalConduitHours; }
            set
            {
                _totalConduitHours = value;
                RaisePropertyChanged("TotalConduitHours");
                RaisePropertyChanged("TotalElectricalHours");
            }
        }

        private double _totalAssociatedCost;
        public double TotalAssociatedCost
        {
            get { return _totalAssociatedCost; }
            set
            {
                _totalAssociatedCost = value;
                RaisePropertyChanged("TotalAssociatedCost");
                RaisePropertyChanged("TotalElectricalCost");
            }
        }

        private double _totalAssociatedHours;
        public double TotalAssociatedHours
        {
            get { return _totalAssociatedHours; }
            set
            {
                _totalAssociatedHours = value;
                RaisePropertyChanged("TotalAssociatedHours");
                RaisePropertyChanged("TotalElectricalHours");
            }
        }

        public double TotalElectricalCost
        {
            get { return (TotalWireCost + TotalConduitCost + TotalAssociatedCost + TotalMiscWiring); }
        }

        public double TotalElectricalHours
        {
            get { return (TotalWireHours + TotalConduitHours + TotalAssociatedHours); }
        }
        #endregion

        public ElectricalMaterialSummaryViewModel(TECBid bid)
        {
            reinitialize(bid);
        }

        public void Refresh(TECBid bid)
        {
            reinitialize(bid);
        }

        private void reinitialize(TECBid bid)
        {
            WireSummaryItems = new ObservableCollection<LengthSummaryItem>();
            ConduitSummaryItems = new ObservableCollection<LengthSummaryItem>();
            AssociatedCostSummaryItems = new ObservableCollection<AssociatedCostSummaryItem>();
            MiscWiring = new ObservableCollection<TECMiscWiring>();

            wireDictionary = new Dictionary<Guid, LengthSummaryItem>();
            conduitDictionary = new Dictionary<Guid, LengthSummaryItem>();
            associatedCostDictionary = new Dictionary<Guid, AssociatedCostSummaryItem>();
            
            TotalWireCost = 0;
            TotalWireHours = 0;
            TotalConduitCost = 0;
            TotalConduitHours = 0;
            TotalAssociatedCost = 0;
            TotalAssociatedHours = 0;

            foreach(TECController controller in bid.Controllers)
            {
                addController(controller);
            }
            foreach(TECMiscWiring miscWiring in bid.MiscWiring)
            {
                addMiscWiring(miscWiring);
            }

            changeWatcher = new ChangeWatcher(bid);
        }
        
        #region Event Handlers
        private void bidChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e is PropertyChangedExtendedEventArgs<Object>)
            {
                PropertyChangedExtendedEventArgs<Object> args = e as PropertyChangedExtendedEventArgs<Object>;
                var targetObject = args.NewValue;
                var referenceObject = args.OldValue;

                if (args.PropertyName == "Add")
                {
                    if (targetObject is TECController && referenceObject is TECBid)
                    {
                        addController(targetObject as TECController);
                    }
                    else if (targetObject is TECConnection && referenceObject is TECController)
                    {
                        addConnection(targetObject as TECConnection);
                    }
                    else if (targetObject is TECDevice && referenceObject is TECSubScope)
                    {
                        if ((referenceObject as TECSubScope).Connection != null)
                        {
                            foreach(TECConnectionType type in (targetObject as TECDevice).ConnectionTypes)
                            {
                                addLengthToWireType((referenceObject as TECSubScope).Connection.Length, type);
                            }
                        }
                    }
                    else if (targetObject is TECAssociatedCost && referenceObject is TECConnectionType)
                    {
                        addAssociatedCost(targetObject as TECAssociatedCost);
                    }
                    else if (targetObject is TECAssociatedCost && referenceObject is TECConduitType)
                    {
                        addAssociatedCost(targetObject as TECAssociatedCost);
                    }
                    else if (targetObject is TECMiscWiring && referenceObject is TECBid) 
                    {
                        addMiscWiring(targetObject as TECMiscWiring);
                    }
                }
                else if (args.PropertyName == "Remove")
                {
                    if (targetObject is TECController && referenceObject is TECBid)
                    {
                        removeController(targetObject as TECController);
                    }
                    else if (targetObject is TECConnection && referenceObject is TECController)
                    {
                        removeConnection(targetObject as TECConnection);
                    }
                    else if (targetObject is TECDevice && referenceObject is TECSubScope)
                    {
                        if ((referenceObject as TECSubScope).Connection != null)
                        {
                            foreach (TECConnectionType type in (targetObject as TECDevice).ConnectionTypes)
                            {
                                removeLengthFromWireType((referenceObject as TECSubScope).Connection.Length, type);
                            }
                        }
                    }
                    else if (targetObject is TECAssociatedCost && referenceObject is TECConnectionType)
                    {
                        removeAssociatedCost(targetObject as TECAssociatedCost);
                    }
                    else if (targetObject is TECAssociatedCost && referenceObject is TECConduitType)
                    {
                        removeAssociatedCost(targetObject as TECAssociatedCost);
                    }
                    else if (targetObject is TECMiscWiring && referenceObject is TECBid)
                    {
                        removeMiscWiring(targetObject as TECMiscWiring);
                    }
                }
                else if (args.PropertyName == "Length" || args.PropertyName == "ConduitLength" || args.PropertyName == "ConduitType")
                {
                    if (args.OldValue is TECConnection && args.NewValue is TECConnection)
                    {
                        removeConnection(args.OldValue as TECConnection);
                        addConnection(args.NewValue as TECConnection);
                    }
                }
                else if (args.PropertyName == "ConnectionType")
                {
                    if (args.OldValue is TECNetworkConnection && args.NewValue is TECNetworkConnection)
                    {
                        removeConnection(args.OldValue as TECConnection);
                        addConnection(args.NewValue as TECConnection);
                    }
                }
                else if (targetObject is TECMiscWiring && referenceObject is TECMiscWiring)
                {
                    editMiscWiring(targetObject as TECMiscWiring, referenceObject as TECMiscWiring);
                }
            }
        }

        private void editMiscWiring(TECMiscWiring newWiring, TECMiscWiring oldWiring)
        {
            TotalMiscWiring -= oldWiring.Cost * oldWiring.Quantity;
            TotalMiscWiring += newWiring.Cost * newWiring.Quantity;
        }

        private void WireItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e is PropertyChangedExtendedEventArgs<Object>)
            {
                PropertyChangedExtendedEventArgs<Object> args = e as PropertyChangedExtendedEventArgs<Object>;

                if (args.PropertyName == "TotalCost")
                {
                    TotalWireCost -= (double)args.OldValue;
                    TotalWireCost += (double)args.NewValue;
                }
                else if (args.PropertyName == "TotalLabor")
                {
                    TotalWireHours -= (double)args.OldValue;
                    TotalWireHours += (double)args.NewValue;
                }
            }
        }

        private void ConduitItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e is PropertyChangedExtendedEventArgs<Object>)
            {
                PropertyChangedExtendedEventArgs<Object> args = e as PropertyChangedExtendedEventArgs<Object>;

                if (args.PropertyName == "TotalCost")
                {
                    TotalConduitCost -= (double)args.OldValue;
                    TotalConduitCost += (double)args.NewValue;
                }
                else if (args.PropertyName == "TotalLabor")
                {
                    TotalConduitHours -= (double)args.OldValue;
                    TotalConduitHours += (double)args.NewValue;
                }
            }
        }

        private void CostItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e is PropertyChangedExtendedEventArgs<Object>)
            {
                PropertyChangedExtendedEventArgs<Object> args = e as PropertyChangedExtendedEventArgs<Object>;

                if (args.PropertyName == "TotalCost")
                {
                    TotalAssociatedCost -= (double)args.OldValue;
                    TotalAssociatedCost += (double)args.NewValue;
                }
                else if (args.PropertyName == "TotalLabor")
                {
                    TotalAssociatedHours -= (double)args.OldValue;
                    TotalAssociatedHours += (double)args.NewValue;
                }
            }
        }
        #endregion

        #region Add/Remove
        private void addLengthToWireType(double length, TECConnectionType type)
        {
            bool containsWire = wireDictionary.ContainsKey(type.Guid);
            if (containsWire)
            {
                TotalWireCost -= wireDictionary[type.Guid].TotalCost;
                TotalWireHours -= wireDictionary[type.Guid].TotalLabor;
                wireDictionary[type.Guid].Length += length;
                TotalWireCost += wireDictionary[type.Guid].TotalCost;
                TotalWireHours += wireDictionary[type.Guid].TotalLabor;
            }
            else
            {
                LengthSummaryItem wireItem = new LengthSummaryItem(type);
                wireItem.Length = length;
                wireItem.PropertyChanged += WireItem_PropertyChanged;
                wireDictionary.Add(type.Guid, wireItem);
                WireSummaryItems.Add(wireItem);
                TotalWireCost += wireItem.TotalCost;
                TotalWireHours += wireItem.TotalLabor;
            }
        }

        private void removeLengthFromWireType(double length, TECConnectionType type)
        {
            bool containsWire = wireDictionary.ContainsKey(type.Guid);
            if (containsWire)
            {
                TotalWireCost -= wireDictionary[type.Guid].TotalCost;
                TotalWireHours -= wireDictionary[type.Guid].TotalLabor;
                wireDictionary[type.Guid].Length -= length;
                TotalWireCost += wireDictionary[type.Guid].TotalCost;
                TotalWireHours += wireDictionary[type.Guid].TotalLabor;

                if (wireDictionary[type.Guid].Length < 0)
                {
                    wireDictionary[type.Guid].PropertyChanged -= WireItem_PropertyChanged;
                    WireSummaryItems.Remove(wireDictionary[type.Guid]);
                    wireDictionary.Remove(type.Guid);
                }
            }
            else
            {
                throw new InvalidOperationException("Wire not found in wire dictionary");
            }
        }

        private void addLengthToConduitType(double length, TECConduitType type)
        {
            if (type != null)
            {
                bool containsConduit = conduitDictionary.ContainsKey(type.Guid);
                if (containsConduit)
                {
                    TotalConduitCost -= conduitDictionary[type.Guid].TotalCost;
                    TotalConduitHours -= conduitDictionary[type.Guid].TotalLabor;
                    conduitDictionary[type.Guid].Length += length;
                    TotalConduitCost += conduitDictionary[type.Guid].TotalCost;
                    TotalConduitHours += conduitDictionary[type.Guid].TotalLabor;
                }
                else
                {
                    LengthSummaryItem conduitItem = new LengthSummaryItem(type);
                    conduitItem.Length = length;
                    conduitItem.PropertyChanged += ConduitItem_PropertyChanged;
                    conduitDictionary.Add(type.Guid, conduitItem);
                    ConduitSummaryItems.Add(conduitItem);
                    TotalConduitCost += conduitItem.TotalCost;
                    TotalConduitHours += conduitItem.TotalLabor;
                }
            }
        }

        private void removeLengthFromConduitType(double length, TECConduitType type)
        {
            if (type != null)
            {
                bool containsconduit = conduitDictionary.ContainsKey(type.Guid);
                if (containsconduit)
                {
                    TotalConduitCost -= conduitDictionary[type.Guid].TotalCost;
                    TotalConduitHours -= conduitDictionary[type.Guid].TotalLabor;
                    conduitDictionary[type.Guid].Length -= length;
                    TotalConduitCost += conduitDictionary[type.Guid].TotalCost;
                    TotalConduitHours += conduitDictionary[type.Guid].TotalLabor;

                    if (conduitDictionary[type.Guid].Length < 0)
                    {
                        conduitDictionary[type.Guid].PropertyChanged -= ConduitItem_PropertyChanged;
                        ConduitSummaryItems.Remove(conduitDictionary[type.Guid]);
                        conduitDictionary.Remove(type.Guid);
                    }
                }
                else
                {
                    throw new InvalidOperationException("Conduit not found in conduit dictionary");
                }
            }
        }

        private void addAssociatedCost(TECAssociatedCost assCost)
        {
            bool containsAssCost = associatedCostDictionary.ContainsKey(assCost.Guid);
            if (containsAssCost)
            {
                TotalAssociatedCost -= associatedCostDictionary[assCost.Guid].TotalCost;
                TotalAssociatedHours -= associatedCostDictionary[assCost.Guid].TotalLabor;
                associatedCostDictionary[assCost.Guid].Quantity++;
                TotalAssociatedCost += associatedCostDictionary[assCost.Guid].TotalCost;
                TotalAssociatedHours += associatedCostDictionary[assCost.Guid].TotalLabor;
            }
            else
            {
                AssociatedCostSummaryItem costItem = new AssociatedCostSummaryItem(assCost);
                costItem.PropertyChanged += CostItem_PropertyChanged;
                associatedCostDictionary.Add(assCost.Guid, costItem);
                AssociatedCostSummaryItems.Add(costItem);
                TotalAssociatedCost += costItem.TotalCost;
                TotalAssociatedHours += costItem.TotalLabor;
            }
        }

        private void removeAssociatedCost(TECAssociatedCost assCost)
        {
            bool containsAssCost = associatedCostDictionary.ContainsKey(assCost.Guid);
            if (containsAssCost)
            {
                AssociatedCostSummaryItem costItem = associatedCostDictionary[assCost.Guid];
                TotalAssociatedCost -= costItem.TotalCost;
                TotalAssociatedHours -= costItem.TotalLabor;
                costItem.Quantity--;
                TotalAssociatedCost += costItem.TotalCost;
                TotalAssociatedHours += costItem.TotalLabor;

                if (costItem.Quantity < 1)
                {
                    costItem.PropertyChanged -= CostItem_PropertyChanged;
                    AssociatedCostSummaryItems.Remove(costItem);
                    associatedCostDictionary.Remove(assCost.Guid);
                }
            }
            else
            {
                throw new InvalidOperationException("Associated cost not found in associated cost dictionary.");
            }
        }

        private void addConnection(TECConnection connection)
        {
            //Add Wire
            if (connection is TECSubScopeConnection)
            {
                TECSubScopeConnection ssConnect = connection as TECSubScopeConnection;

                foreach(TECConnectionType type in ssConnect.ConnectionTypes)
                {
                    addLengthToWireType(connection.Length, type);
                    foreach(TECAssociatedCost cost in type.AssociatedCosts)
                    {
                        addAssociatedCost(cost);
                    }
                }
            }
            else if (connection is TECNetworkConnection)
            {
                TECNetworkConnection netConnect = connection as TECNetworkConnection;

                addLengthToWireType(connection.Length, netConnect.ConnectionType);
                
                foreach(TECAssociatedCost cost in netConnect.ConnectionType.AssociatedCosts)
                {
                    addAssociatedCost(cost);
                }
            }
            else
            {
                throw new NotImplementedException();
            }

            //Add Conduit
            addLengthToConduitType(connection.ConduitLength, connection.ConduitType);
            if (connection.ConduitType != null)
            {
                foreach (TECAssociatedCost cost in connection.ConduitType.AssociatedCosts)
                {
                    addAssociatedCost(cost);
                }
            }
        }

        private void removeConnection(TECConnection connection)
        {
            //Remove Wire
            if (connection is TECSubScopeConnection)
            {
                TECSubScopeConnection ssConnect = connection as TECSubScopeConnection;

                foreach (TECConnectionType type in ssConnect.ConnectionTypes)
                {
                    removeLengthFromWireType(connection.Length, type);
                    foreach(TECAssociatedCost cost in type.AssociatedCosts)
                    {
                        removeAssociatedCost(cost);
                    }
                }
            }
            else if (connection is TECNetworkConnection)
            {
                TECNetworkConnection netConnect = connection as TECNetworkConnection;

                removeLengthFromWireType(connection.Length, netConnect.ConnectionType);

                foreach(TECAssociatedCost cost in netConnect.ConnectionType.AssociatedCosts)
                {
                    removeAssociatedCost(cost);
                }
            }
            else
            {
                throw new NotImplementedException();
            }

            //Remove Conduit
            removeLengthFromConduitType(connection.ConduitLength, connection.ConduitType);
            if (connection.ConduitType != null)
            {
                foreach (TECAssociatedCost cost in connection.ConduitType.AssociatedCosts)
                {
                    removeAssociatedCost(cost);
                }
            }
        }

        private void addController(TECController controller)
        {
            foreach(TECConnection connection in controller.ChildrenConnections)
            {
                addConnection(connection);
            }
        }

        private void removeController(TECController controller)
        {
            foreach(TECConnection connection in controller.ChildrenConnections)
            {
                removeConnection(connection);
            }
        }

        private void addMiscWiring(TECMiscWiring miscWiring)
        {
            TotalMiscWiring += miscWiring.Cost * miscWiring.Quantity;
            MiscWiring.Add(miscWiring);
        }

        private void removeMiscWiring(TECMiscWiring miscWiring)
        {
            TotalMiscWiring -= miscWiring.Cost * miscWiring.Quantity;
            MiscWiring.Remove(miscWiring);
        }
        #endregion

    }
}