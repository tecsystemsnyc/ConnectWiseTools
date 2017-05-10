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

        private Dictionary<Guid, LengthSummaryItem> wireDictionary;
        private Dictionary<Guid, LengthSummaryItem> conduitDictionary;

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
        public double TotalAsscociatedHours
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
            get { return (TotalWireCost + TotalConduitCost + TotalAssociatedCost); }
        }

        public double TotalElectricalHours
        {
            get { return (TotalWireHours + TotalConduitHours + TotalAsscociatedHours); }
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

            wireDictionary = new Dictionary<Guid, LengthSummaryItem>();
            conduitDictionary = new Dictionary<Guid, LengthSummaryItem>();

            TotalWireCost = 0;
            TotalWireHours = 0;
            TotalConduitCost = 0;
            TotalConduitHours = 0;
            TotalAssociatedCost = 0;
            TotalAsscociatedHours = 0;

            foreach(TECController controller in bid.Controllers)
            {
                addController(controller);
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
                    
                }
                else if (args.PropertyName == "Remove")
                {
                    
                }
            }
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

        private void addConnection(TECConnection connection)
        {
            //Add Wire
            if (connection is TECSubScopeConnection)
            {
                TECSubScopeConnection ssConnect = connection as TECSubScopeConnection;

                foreach(TECConnectionType type in ssConnect.ConnectionTypes)
                {
                    addLengthToWireType(connection.Length, type);
                }
            }
            else if (connection is TECNetworkConnection)
            {
                TECNetworkConnection netConnect = connection as TECNetworkConnection;

                addLengthToWireType(connection.Length, netConnect.ConnectionType);
            }
            else
            {
                throw new NotImplementedException();
            }

            //Add Conduit
            addLengthToConduitType(connection.ConduitLength, connection.ConduitType);
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
                }
            }
            else if (connection is TECNetworkConnection)
            {
                TECNetworkConnection netConnect = connection as TECNetworkConnection;

                removeLengthFromWireType(connection.Length, netConnect.ConnectionType);
            }
            else
            {
                throw new NotImplementedException();
            }

            //Remove Conduit
            removeLengthFromConduitType(connection.ConduitLength, connection.ConduitType);
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
        #endregion

    }
}