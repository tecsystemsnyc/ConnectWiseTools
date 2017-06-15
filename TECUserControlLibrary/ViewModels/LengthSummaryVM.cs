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

        private Dictionary<Guid, AssociatedCostSummaryItem> assCostDictionary;

        private ObservableCollection<AssociatedCostSummaryItem> _assCostSummaryItems;
        public ObservableCollection<AssociatedCostSummaryItem> AssCostSummaryItems
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
            AssCostSummaryItems = new ObservableCollection<AssociatedCostSummaryItem>();
            RatedCostSummaryItems = new ObservableCollection<RatedCostSummaryItem>();

            lengthDictionary = new Dictionary<Guid, LengthSummaryItem>();
            assCostDictionary = new Dictionary<Guid, AssociatedCostSummaryItem>();
            ratedCostDictionary = new Dictionary<Guid, RatedCostSummaryItem>();

            LengthSubTotalCost = 0;
            LengthSubTotalLabor = 0;
            AssCostSubTotalCost = 0;
            AssCostSubTotalLabor = 0;
            RatedCostSubTotalCost = 0;
            RatedCostSubTotalLabor = 0;

        }

        #region Add/Remove
        public void AddLength(ElectricalMaterialComponent component, double length)
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
            foreach (TECCost cost in component.AssociatedCosts)
            {
                Tuple<double, double> delta = TECMaterialSummaryVM.AddCost(cost, assCostDictionary, AssCostSummaryItems);
                AssCostSubTotalCost += delta.Item1;
                AssCostSubTotalLabor += delta.Item2;
            }
            foreach (TECCost cost in component.RatedCosts)
            {
                AddRatedCost(cost, length);
            }
        }

        public void RemoveLength(ElectricalMaterialComponent component, double length)
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

                foreach (TECCost cost in component.AssociatedCosts)
                {
                    Tuple<double, double> delta = TECMaterialSummaryVM.RemoveCost(cost, assCostDictionary, AssCostSummaryItems);
                    AssCostSubTotalCost += delta.Item1;
                    AssCostSubTotalLabor += delta.Item2;
                }
                foreach (TECCost cost in component.RatedCosts)
                {
                    RemoveRatedCost(cost, length);
                }
            }
            else
            {
                throw new InvalidOperationException("Component not found in length dictionary.");
            }
        }

        public void AddRatedCost(TECCost cost, double length)
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

        public void RemoveRatedCost(TECCost cost, double length)
        {
            bool containsCost = ratedCostDictionary.ContainsKey(cost.Guid);
            if (containsCost)
            {
                RatedCostSubTotalCost -= lengthDictionary[cost.Guid].TotalCost;
                RatedCostSubTotalLabor -= lengthDictionary[cost.Guid].TotalLabor;
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

        public void AddConnection(TECConnection connection)
        {
            if (Type == LengthType.Conduit)
            {
                if (connection.ConduitType != null)
                {
                    AddLength(connection.ConduitType, connection.ConduitLength);
                }
            }
            else if (Type == LengthType.Wire)
            {
                if (connection is TECNetworkConnection)
                {
                    AddLength((connection as TECNetworkConnection).ConnectionType, connection.Length);
                }
                else if (connection is TECSubScopeConnection)
                {
                    foreach(TECConnectionType connectionType in (connection as TECSubScopeConnection).ConnectionTypes)
                    {
                        AddLength(connectionType, connection.Length);
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
            if (Type == LengthType.Conduit)
            {
                if (connection.ConduitType != null)
                {
                    RemoveLength(connection.ConduitType, connection.ConduitLength);
                }
            }
            else if (Type == LengthType.Wire)
            {
                if (connection is TECNetworkConnection)
                {
                    RemoveLength((connection as TECNetworkConnection).ConnectionType, connection.Length);
                }
                else if (connection is TECSubScopeConnection)
                {
                    foreach (TECConnectionType connectionType in (connection as TECSubScopeConnection).ConnectionTypes)
                    {
                        RemoveLength(connectionType, connection.Length);
                    }
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        public void AddController(TECController controller)
        {
            foreach(TECConnection connection in controller.ChildrenConnections)
            {
                AddConnection(connection);
            }
        }

        public void RemoveController(TECController controller)
        {
            foreach(TECConnection connection in controller.ChildrenConnections)
            {
                RemoveConnection(connection);
            }
        }

        public void AddSystem(TECSystem system)
        {
            foreach(TECController controller in system.Controllers)
            {
                AddController(controller);
            }
        }

        public void RemoveSystem(TECSystem system)
        {
            foreach(TECController controller in system.Controllers)
            {
                RemoveController(controller);
            }
        }
        #endregion
    }
}
