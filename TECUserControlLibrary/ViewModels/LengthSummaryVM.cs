using EstimatingLibrary;
using EstimatingLibrary.Interfaces;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECUserControlLibrary.Models;

namespace TECUserControlLibrary.ViewModels
{
    public enum LengthType { Wire, Conduit }

    public class LengthSummaryVM : ViewModelBase
    {
        #region Properties
        public LengthType Type { get; private set; }
        private string _title;
        public string Title
        {
            get { return _title; }
            private set
            {
                _title = value;
                RaisePropertyChanged("Title");
            }
        }

        public TECBid Bid { get; private set; }

        private Dictionary<Guid, LengthSummaryItem> lengthDictionary;

        private ObservableCollection<LengthSummaryItem> _lengthSummaryItems;
        public ObservableCollection<LengthSummaryItem> LengthSummaryItems
        {
            get { return _lengthSummaryItems; }
            set
            {
                _lengthSummaryItems = value;
                RaisePropertyChanged("LengthSummaryItems");
            }
        }

        private Dictionary<Guid, CostSummaryItem> assCostDictionary;

        private ObservableCollection<CostSummaryItem> _assCostSummaryItems;
        public ObservableCollection<CostSummaryItem> AssCostSummaryItems
        {
            get { return _assCostSummaryItems; }
            set
            {
                _assCostSummaryItems = value;
                RaisePropertyChanged("AssCostSummaryItems");
            }
        }

        private Dictionary<Guid, RatedCostSummaryItem> ratedCostDictionary;

        private ObservableCollection<RatedCostSummaryItem> _ratedCostSummaryItems;
        public ObservableCollection<RatedCostSummaryItem> RatedCostSummaryItems
        {
            get { return _ratedCostSummaryItems; }
            set
            {
                _ratedCostSummaryItems = value;
                RaisePropertyChanged("RatedCostSummaryItems");
            }
        }

        #region Subtotals
        private double _lengthSubTotalCost;
        public double LengthSubTotalCost
        {
            get { return _lengthSubTotalCost; }
            set
            {
                _lengthSubTotalCost = value;
                RaisePropertyChanged("LengthSubTotalCost");
            }
        }

        private double _lengthSubTotalLabor;
        public double LengthSubTotalLabor
        {
            get { return _lengthSubTotalLabor; }
            set
            {
                _lengthSubTotalLabor = value;
                RaisePropertyChanged("LengthSubTotalLabor");
            }
        }

        private double _assCostSubTotalCost;
        public double AssCostSubTotalCost
        {
            get { return _assCostSubTotalCost; }
            set
            {
                _assCostSubTotalCost = value;
                RaisePropertyChanged("AssCostSubTotalCost");
            }
        }

        private double _assCostSubTotalLabor;
        public double AssCostSubTotalLabor
        {
            get { return _assCostSubTotalLabor; }
            set
            {
                _assCostSubTotalLabor = value;
                RaisePropertyChanged("AssCostSubTotalLabor");
            }
        }

        private double _ratedCostSubTotalCost;
        public double RatedCostSubTotalCost
        {
            get { return _ratedCostSubTotalCost; }
            set
            {
                _ratedCostSubTotalCost = value;
                RaisePropertyChanged("RatedCostSubTotalCost");
            }
        }

        private double _ratedCostSubTotalLabor;
        public double RatedCostSubTotalLabor
        {
            get { return _ratedCostSubTotalLabor; }
            set
            {
                _ratedCostSubTotalLabor = value;
                RaisePropertyChanged("RatedCostSubTotalLabor");
            }
        }
        #endregion
        #endregion

        public LengthSummaryVM(TECBid bid, LengthType lengthType)
        {
            Type = lengthType;
            if (lengthType == LengthType.Wire)
            {
                Title = "Wire Lengths";
            }
            else if (lengthType == LengthType.Conduit)
            {
                Title = "Conduit Lengths";
            }
            reinitialize(bid);
        }

        public void Refresh(TECBid bid)
        {
            reinitialize(bid);
        }

        private void reinitialize(TECBid bid)
        {
            Bid = bid;

            LengthSummaryItems = new ObservableCollection<LengthSummaryItem>();
            AssCostSummaryItems = new ObservableCollection<CostSummaryItem>();
            RatedCostSummaryItems = new ObservableCollection<RatedCostSummaryItem>();

            lengthDictionary = new Dictionary<Guid, LengthSummaryItem>();
            assCostDictionary = new Dictionary<Guid, CostSummaryItem>();
            ratedCostDictionary = new Dictionary<Guid, RatedCostSummaryItem>();

            LengthSubTotalCost = 0;
            LengthSubTotalLabor = 0;
            AssCostSubTotalCost = 0;
            AssCostSubTotalLabor = 0;
            RatedCostSubTotalCost = 0;
            RatedCostSubTotalLabor = 0;

            foreach(TECSystem typical in bid.Systems)
            {
                foreach(TECSystem instance in typical.SystemInstances)
                {
                    AddInstanceSystem(instance);
                }
            }
            foreach(TECController controller in bid.Controllers)
            {
                AddController(controller);
            }
        }

        #region Add/Remove
        public void AddInstanceSystem(TECSystem system)
        {
            foreach (TECController controller in system.Controllers)
            {
                AddController(controller);
            }
        }
        public void RemoveInstanceSystem(TECSystem system)
        {
            foreach (TECController controller in system.Controllers)
            {
                RemoveController(controller);
            }
        }

        public void AddController(TECController controller)
        {
            foreach (TECConnection connection in controller.ChildrenConnections)
            {
                if (!(connection is TECSubScopeConnection) || (!isTypicalConnection(connection as TECSubScopeConnection)))
                {
                    AddConnection(connection);
                }
            }
        }
        public void RemoveController(TECController controller)
        {
            foreach (TECConnection connection in controller.ChildrenConnections)
            {
                RemoveConnection(connection);
            }
        }

        public void AddConnection(TECConnection connection)
        {
            connection.PropertyChanged += Connection_PropertyChanged;
            if (connection is TECSubScopeConnection ssConnect)
            {
                ssConnect.SubScope.PropertyChanged += SubScope_PropertyChanged;
            }
            if (Type == LengthType.Conduit)
            {
                if (connection.ConduitType != null)
                {
                    AddLength(connection.ConduitType, connection.ConduitLength);
                    foreach(TECCost cost in connection.ConduitType.AssociatedCosts)
                    {
                        AddAssCost(cost);
                    }
                    foreach(TECCost cost in connection.ConduitType.RatedCosts)
                    {
                        AddRatedCost(cost, connection.ConduitLength);
                    }
                }
            }
            else if (Type == LengthType.Wire)
            {
                if (connection is TECNetworkConnection)
                {
                    TECNetworkConnection netConnect = connection as TECNetworkConnection;
                    AddLength(netConnect.ConnectionType, connection.Length);
                    foreach(TECCost cost in netConnect.ConnectionType.AssociatedCosts)
                    {
                        AddAssCost(cost);
                    }
                    foreach(TECCost cost in netConnect.ConnectionType.RatedCosts)
                    {
                        AddRatedCost(cost, connection.Length);
                    }
                }
                else if (connection is TECSubScopeConnection)
                {
                    foreach (TECElectricalMaterial connectionType in (connection as TECSubScopeConnection).ConnectionTypes)
                    {
                        AddLength(connectionType, connection.Length);
                        foreach(TECCost cost in connectionType.AssociatedCosts)
                        {
                            AddAssCost(cost);
                        }
                        foreach(TECCost cost in connectionType.RatedCosts)
                        {
                            AddRatedCost(cost, connection.Length);
                        }
                    }
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }
        public void RemoveConnection(TECConnection connection)
        {
            connection.PropertyChanged -= Connection_PropertyChanged;
            if (connection is TECSubScopeConnection ssConnect)
            {
                ssConnect.SubScope.PropertyChanged -= SubScope_PropertyChanged;
            }
            if (Type == LengthType.Conduit)
            {
                if (connection.ConduitType != null)
                {
                    RemoveLength(connection.ConduitType, connection.ConduitLength);
                    foreach (TECCost cost in connection.ConduitType.AssociatedCosts)
                    {
                        RemoveAssCost(cost);
                    }
                    foreach (TECCost cost in connection.ConduitType.RatedCosts)
                    {
                        RemoveRatedCost(cost, connection.ConduitLength);
                    }
                }
            }
            else if (Type == LengthType.Wire)
            {
                if (connection is TECNetworkConnection)
                {
                    TECNetworkConnection netConnect = connection as TECNetworkConnection;
                    RemoveLength(netConnect.ConnectionType, connection.Length);
                    foreach(TECCost cost in netConnect.ConnectionType.AssociatedCosts)
                    {
                        RemoveAssCost(cost);
                    }
                    foreach(TECCost cost in netConnect.ConnectionType.RatedCosts)
                    {
                        RemoveRatedCost(cost, connection.Length);
                    }
                }
                else if (connection is TECSubScopeConnection)
                {
                    foreach (TECElectricalMaterial connectionType in (connection as TECSubScopeConnection).ConnectionTypes)
                    {
                        RemoveLength(connectionType, connection.Length);
                        foreach(TECCost cost in connectionType.AssociatedCosts)
                        {
                            RemoveAssCost(cost);
                        }
                        foreach(TECCost cost in connectionType.RatedCosts)
                        {
                            RemoveRatedCost(cost, connection.Length);
                        }
                    }
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        public void AddLength(TECElectricalMaterial component, double length)
        {
            bool containsLength = lengthDictionary.ContainsKey(component.Guid);
            if (containsLength)
            {
                LengthSubTotalCost -= lengthDictionary[component.Guid].TotalCost;
                LengthSubTotalLabor -= lengthDictionary[component.Guid].TotalLabor;
                lengthDictionary[component.Guid].Length += length;
                LengthSubTotalCost += lengthDictionary[component.Guid].TotalCost;
                LengthSubTotalLabor += lengthDictionary[component.Guid].TotalLabor;
            }
            else
            {
                LengthSummaryItem lengthItem = new LengthSummaryItem(component);
                lengthItem.Length = length;
                lengthDictionary.Add(component.Guid, lengthItem);
                LengthSummaryItems.Add(lengthItem);
                LengthSubTotalCost += lengthItem.TotalCost;
                LengthSubTotalLabor += lengthItem.TotalLabor;
            }
        }
        public void RemoveLength(TECElectricalMaterial component, double length)
        {
            bool containsLength = lengthDictionary.ContainsKey(component.Guid);
            if (containsLength)
            {
                LengthSubTotalCost -= lengthDictionary[component.Guid].TotalCost;
                LengthSubTotalLabor -= lengthDictionary[component.Guid].TotalLabor;
                lengthDictionary[component.Guid].Length -= length;
                LengthSubTotalCost += lengthDictionary[component.Guid].TotalCost;
                LengthSubTotalLabor += lengthDictionary[component.Guid].TotalLabor;

                if (lengthDictionary[component.Guid].Length < 0)
                {
                    LengthSummaryItems.Remove(lengthDictionary[component.Guid]);
                    lengthDictionary.Remove(component.Guid);
                }
            }
            else
            {
                throw new InvalidOperationException("Component not found in length dictionary.");
            }
        }

        public void AddAssCost(TECCost cost)
        {
            if (cost.Type == CostType.Electrical)
            {
                Tuple<double, double> delta = TECMaterialSummaryVM.AddCost(cost, assCostDictionary, AssCostSummaryItems);
                AssCostSubTotalCost += delta.Item1;
                AssCostSubTotalLabor += delta.Item2;
            }
        }
        public void RemoveAssCost(TECCost cost)
        {
            if (cost.Type == CostType.Electrical)
            {
                Tuple<double, double> delta = TECMaterialSummaryVM.RemoveCost(cost, assCostDictionary, AssCostSummaryItems);
                AssCostSubTotalCost += delta.Item1;
                AssCostSubTotalLabor += delta.Item2;
            }
        }

        public void AddRatedCost(TECCost cost, double length)
        {
            if (cost.Type == CostType.Electrical)
            {
                bool containsCost = ratedCostDictionary.ContainsKey(cost.Guid);
                if (containsCost)
                {
                    RatedCostSubTotalCost -= ratedCostDictionary[cost.Guid].TotalCost;
                    RatedCostSubTotalLabor -= ratedCostDictionary[cost.Guid].TotalLabor;
                    ratedCostDictionary[cost.Guid].Length += length;
                    RatedCostSubTotalCost += ratedCostDictionary[cost.Guid].TotalCost;
                    RatedCostSubTotalLabor += ratedCostDictionary[cost.Guid].TotalLabor;
                }
                else
                {
                    RatedCostSummaryItem ratedItem = new RatedCostSummaryItem(cost, length);
                    ratedCostDictionary.Add(cost.Guid, ratedItem);
                    RatedCostSummaryItems.Add(ratedItem);
                    RatedCostSubTotalCost += ratedItem.TotalCost;
                    RatedCostSubTotalLabor += ratedItem.TotalLabor;
                }
            }
        }
        public void RemoveRatedCost(TECCost cost, double length)
        {
            if (cost.Type == CostType.Electrical)
            {
                bool containsCost = ratedCostDictionary.ContainsKey(cost.Guid);
                if (containsCost)
                {
                    RatedCostSubTotalCost -= ratedCostDictionary[cost.Guid].TotalCost;
                    RatedCostSubTotalLabor -= ratedCostDictionary[cost.Guid].TotalLabor;
                    ratedCostDictionary[cost.Guid].Length -= length;
                    RatedCostSubTotalCost += ratedCostDictionary[cost.Guid].TotalCost;
                    RatedCostSubTotalLabor += ratedCostDictionary[cost.Guid].TotalLabor;

                    if (ratedCostDictionary[cost.Guid].Length < 0)
                    {
                        RatedCostSummaryItems.Remove(ratedCostDictionary[cost.Guid]);
                        ratedCostDictionary.Remove(cost.Guid);
                    }
                }
                else
                {
                    throw new InvalidOperationException("Cost not found in ratedCost dicionary.");
                }
            }
        }
        #endregion

        private bool isTypicalConnection(TECSubScopeConnection ssConnect)
        {
            foreach(TECSystem system in Bid.Systems)
            {
                if (system.SubScope.Contains(ssConnect.SubScope))
                {
                    return true;
                }
            }
            return false;
        }

        #region Event Handlers
        private void Connection_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e is PropertyChangedExtendedEventArgs args)
            {
                TECConnection connection = sender as TECConnection;
                if (args.PropertyName == "Length" && Type == LengthType.Wire)
                {
                    double lengthDelta = (double)args.Value - (double)args.OldValue;
                    if (connection is TECNetworkConnection netConnect)
                    {
                        AddLength(netConnect.ConnectionType, lengthDelta);
                    }
                    else if (connection is TECSubScopeConnection ssConnect)
                    {
                        foreach (TECElectricalMaterial connectionType in ssConnect.ConnectionTypes)
                        {
                            AddLength(connectionType, lengthDelta);
                        }
                    }
                    else
                    {
                        throw new NotImplementedException("Connection type not recognized.");
                    }
                }
                else if (args.PropertyName == "ConduitLength" && Type == LengthType.Conduit)
                {
                    if (connection.ConduitType != null)
                    {
                        double lengthDelta = (double)args.Value - (double)args.OldValue;
                        AddLength(connection.ConduitType, lengthDelta);
                    }
                }
                else if (args.PropertyName == "ConnectionType" && Type == LengthType.Wire)
                {
                    if (connection is TECNetworkConnection netConnect)
                    {
                        RemoveLength((args.OldValue as TECElectricalMaterial), netConnect.Length);
                        AddLength((args.Value as TECElectricalMaterial), netConnect.Length);
                    }
                    else
                    {
                        throw new InvalidCastException("ConnectionType changed but sender isn't TECNetworkConnection.");
                    }
                }
                else if (args.PropertyName == "ConduitType" && Type == LengthType.Conduit)
                {
                    TECElectricalMaterial oldConduit = args.OldValue as TECElectricalMaterial;
                    TECElectricalMaterial newConduit = args.Value as TECElectricalMaterial;
                    if (oldConduit != null)
                    {
                        RemoveLength(oldConduit, connection.Length);
                    }
                    if (newConduit != null)
                    {
                        AddLength(newConduit, connection.Length);
                    }
                }
            }
        }

        private void SubScope_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e is PropertyChangedExtendedEventArgs args)
            {
                TECSubScopeConnection ssConnect = (sender as TECSubScope).Connection;
                if (args.PropertyName == "Devices")
                {
                    TECDevice device = args.Value as TECDevice;
                    if (args.Change == EstimatingLibrary.Utilities.Change.Add)
                    {
                        foreach(TECElectricalMaterial connectionType in device.ConnectionTypes)
                        {
                            AddLength(connectionType, ssConnect.Length);
                        }
                    }
                    else if (args.Change == EstimatingLibrary.Utilities.Change.Remove)
                    {
                        foreach(TECElectricalMaterial connectionType in device.ConnectionTypes)
                        {
                            RemoveLength(connectionType, ssConnect.Length);
                        }
                    }
                }

            }
        }
        #endregion
    }
}
