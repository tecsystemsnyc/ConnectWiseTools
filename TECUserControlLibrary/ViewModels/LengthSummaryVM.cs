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
    public class LengthSummaryVM : ViewModelBase
    {
        #region Fields
        private LengthType type;

        private Dictionary<Guid, LengthSummaryItem> lengthDictionary;
        private Dictionary<Guid, CostSummaryItem> assCostDictionary;
        private Dictionary<Guid, RatedCostSummaryItem> ratedCostDictionary;

        private ObservableCollection<LengthSummaryItem> _lengthSummaryItems;
        private ObservableCollection<CostSummaryItem> _assCostSummaryItems;
        private ObservableCollection<RatedCostSummaryItem> _ratedCostSummaryItems;

        private double _lengthSubTotalCost;
        private double _lengthSubTotalLabor;
        private double _assCostSubTotalCost;
        private double _assCostSubTotalLabor;
        private double _ratedCostSubTotalCost;
        private double _ratedCostSubTotalLabor;
        #endregion

        //Constructor
        public LengthSummaryVM(LengthType lengthType)
        {
            type = lengthType;
            initialize();
        }
        
        //Enums
        public enum LengthType { Wire, Conduit }

        #region Properties
        public ObservableCollection<LengthSummaryItem> LengthSummaryItems
        {
            get { return _lengthSummaryItems; }
            private set
            {
                _lengthSummaryItems = value;
                RaisePropertyChanged("LengthSummaryItems");
            }
        }
        public ObservableCollection<CostSummaryItem> AssCostSummaryItems
        {
            get { return _assCostSummaryItems; }
            private set
            {
                _assCostSummaryItems = value;
                RaisePropertyChanged("AssCostSummaryItems");
            }
        }
        public ObservableCollection<RatedCostSummaryItem> RatedCostSummaryItems
        {
            get { return _ratedCostSummaryItems; }
            private set
            {
                _ratedCostSummaryItems = value;
                RaisePropertyChanged("RatedCostSummaryItems");
            }
        }

        public double LengthSubTotalCost
        {
            get { return _lengthSubTotalCost; }
            private set
            {
                _lengthSubTotalCost = value;
                RaisePropertyChanged("LengthSubTotalCost");
            }
        }
        public double LengthSubTotalLabor
        {
            get { return _lengthSubTotalLabor; }
            private set
            {
                _lengthSubTotalLabor = value;
                RaisePropertyChanged("LengthSubTotalLabor");
            }
        }
        public double AssCostSubTotalCost
        {
            get { return _assCostSubTotalCost; }
            private set
            {
                _assCostSubTotalCost = value;
                RaisePropertyChanged("AssCostSubTotalCost");
            }
        }
        public double AssCostSubTotalLabor
        {
            get { return _assCostSubTotalLabor; }
            private set
            {
                _assCostSubTotalLabor = value;
                RaisePropertyChanged("AssCostSubTotalLabor");
            }
        }
        public double RatedCostSubTotalCost
        {
            get { return _ratedCostSubTotalCost; }
            private set
            {
                _ratedCostSubTotalCost = value;
                RaisePropertyChanged("RatedCostSubTotalCost");
            }
        }
        public double RatedCostSubTotalLabor
        {
            get { return _ratedCostSubTotalLabor; }
            private set
            {
                _ratedCostSubTotalLabor = value;
                RaisePropertyChanged("RatedCostSubTotalLabor");
            }
        }
        #endregion

        #region Methods
        public void Reset()
        {
            initialize();
        }

        public void AddLength(TECElectricalMaterial material, double length)
        {

        }
        public void RemoveLength(TECElectricalMaterial material, double length)
        {

        }

        private void initialize()
        {
            lengthDictionary = new Dictionary<Guid, LengthSummaryItem>();
            assCostDictionary = new Dictionary<Guid, CostSummaryItem>();
            ratedCostDictionary = new Dictionary<Guid, RatedCostSummaryItem>();

            LengthSummaryItems = new ObservableCollection<LengthSummaryItem>();
            AssCostSummaryItems = new ObservableCollection<CostSummaryItem>();
            RatedCostSummaryItems = new ObservableCollection<RatedCostSummaryItem>();

            LengthSubTotalCost = 0;
            LengthSubTotalLabor = 0;
            AssCostSubTotalCost = 0;
            AssCostSubTotalLabor = 0;
            RatedCostSubTotalCost = 0;
            RatedCostSubTotalLabor = 0;
        }

        private void addAssCost(TECCost cost)
        {

        }
        private void removeAssCost(TECCost cost)
        {

        }
        private void addRatedCost(TECCost cost)
        {

        }
        private void removeRatedCost(TECCost cost)
        {

        }
        #endregion














        
        

        //#region Add/Remove

        //public void AddLength(TECElectricalMaterial component, double length)
        //{
        //    bool containsLength = lengthDictionary.ContainsKey(component.Guid);
        //    if (containsLength)
        //    {
        //        LengthSubTotalCost -= lengthDictionary[component.Guid].TotalCost;
        //        LengthSubTotalLabor -= lengthDictionary[component.Guid].TotalLabor;
        //        lengthDictionary[component.Guid].Length += length;
        //        LengthSubTotalCost += lengthDictionary[component.Guid].TotalCost;
        //        LengthSubTotalLabor += lengthDictionary[component.Guid].TotalLabor;
        //    }
        //    else
        //    {
        //        LengthSummaryItem lengthItem = new LengthSummaryItem(component);
        //        lengthItem.Length = length;
        //        lengthDictionary.Add(component.Guid, lengthItem);
        //        LengthSummaryItems.Add(lengthItem);
        //        LengthSubTotalCost += lengthItem.TotalCost;
        //        LengthSubTotalLabor += lengthItem.TotalLabor;
        //    }
        //}
        //public void RemoveLength(TECElectricalMaterial component, double length)
        //{
        //    bool containsLength = lengthDictionary.ContainsKey(component.Guid);
        //    if (containsLength)
        //    {
        //        LengthSubTotalCost -= lengthDictionary[component.Guid].TotalCost;
        //        LengthSubTotalLabor -= lengthDictionary[component.Guid].TotalLabor;
        //        lengthDictionary[component.Guid].Length -= length;
        //        LengthSubTotalCost += lengthDictionary[component.Guid].TotalCost;
        //        LengthSubTotalLabor += lengthDictionary[component.Guid].TotalLabor;

        //        if (lengthDictionary[component.Guid].Length < 0)
        //        {
        //            LengthSummaryItems.Remove(lengthDictionary[component.Guid]);
        //            lengthDictionary.Remove(component.Guid);
        //        }
        //    }
        //    else
        //    {
        //        throw new InvalidOperationException("Component not found in length dictionary.");
        //    }
        //}

        //public void AddAssCost(TECCost cost)
        //{
        //    if (cost.Type == CostType.Electrical)
        //    {
        //        Tuple<double, double> delta = TECMaterialSummaryVM.AddCost(cost, assCostDictionary, AssCostSummaryItems);
        //        AssCostSubTotalCost += delta.Item1;
        //        AssCostSubTotalLabor += delta.Item2;
        //    }
        //}
        //public void RemoveAssCost(TECCost cost)
        //{
        //    if (cost.Type == CostType.Electrical)
        //    {
        //        Tuple<double, double> delta = TECMaterialSummaryVM.RemoveCost(cost, assCostDictionary, AssCostSummaryItems);
        //        AssCostSubTotalCost += delta.Item1;
        //        AssCostSubTotalLabor += delta.Item2;
        //    }
        //}

        //public void AddRatedCost(TECCost cost, double length)
        //{
        //    if (cost.Type == CostType.Electrical)
        //    {
        //        bool containsCost = ratedCostDictionary.ContainsKey(cost.Guid);
        //        if (containsCost)
        //        {
        //            RatedCostSubTotalCost -= ratedCostDictionary[cost.Guid].TotalCost;
        //            RatedCostSubTotalLabor -= ratedCostDictionary[cost.Guid].TotalLabor;
        //            ratedCostDictionary[cost.Guid].Length += length;
        //            RatedCostSubTotalCost += ratedCostDictionary[cost.Guid].TotalCost;
        //            RatedCostSubTotalLabor += ratedCostDictionary[cost.Guid].TotalLabor;
        //        }
        //        else
        //        {
        //            RatedCostSummaryItem ratedItem = new RatedCostSummaryItem(cost, length);
        //            ratedCostDictionary.Add(cost.Guid, ratedItem);
        //            RatedCostSummaryItems.Add(ratedItem);
        //            RatedCostSubTotalCost += ratedItem.TotalCost;
        //            RatedCostSubTotalLabor += ratedItem.TotalLabor;
        //        }
        //    }
        //}
        //public void RemoveRatedCost(TECCost cost, double length)
        //{
        //    if (cost.Type == CostType.Electrical)
        //    {
        //        bool containsCost = ratedCostDictionary.ContainsKey(cost.Guid);
        //        if (containsCost)
        //        {
        //            RatedCostSubTotalCost -= ratedCostDictionary[cost.Guid].TotalCost;
        //            RatedCostSubTotalLabor -= ratedCostDictionary[cost.Guid].TotalLabor;
        //            ratedCostDictionary[cost.Guid].Length -= length;
        //            RatedCostSubTotalCost += ratedCostDictionary[cost.Guid].TotalCost;
        //            RatedCostSubTotalLabor += ratedCostDictionary[cost.Guid].TotalLabor;

        //            if (ratedCostDictionary[cost.Guid].Length < 0)
        //            {
        //                RatedCostSummaryItems.Remove(ratedCostDictionary[cost.Guid]);
        //                ratedCostDictionary.Remove(cost.Guid);
        //            }
        //        }
        //        else
        //        {
        //            throw new InvalidOperationException("Cost not found in ratedCost dicionary.");
        //        }
        //    }
        //}
        //#endregion

        //private bool isTypicalConnection(TECSubScopeConnection ssConnect)
        //{
        //    foreach(TECSystem system in Bid.Systems)
        //    {
        //        if (system.SubScope.Contains(ssConnect.SubScope))
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        //#region Event Handlers
        //private void Connection_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    if (e is PropertyChangedExtendedEventArgs args)
        //    {
        //        TECConnection connection = sender as TECConnection;
        //        if (args.PropertyName == "Length" && Type == LengthType.Wire)
        //        {
        //            double lengthDelta = (double)args.Value - (double)args.OldValue;
        //            if (connection is TECNetworkConnection netConnect)
        //            {
        //                AddLength(netConnect.ConnectionType, lengthDelta);
        //            }
        //            else if (connection is TECSubScopeConnection ssConnect)
        //            {
        //                foreach (TECElectricalMaterial connectionType in ssConnect.ConnectionTypes)
        //                {
        //                    AddLength(connectionType, lengthDelta);
        //                }
        //            }
        //            else
        //            {
        //                throw new NotImplementedException("Connection type not recognized.");
        //            }
        //        }
        //        else if (args.PropertyName == "ConduitLength" && Type == LengthType.Conduit)
        //        {
        //            if (connection.ConduitType != null)
        //            {
        //                double lengthDelta = (double)args.Value - (double)args.OldValue;
        //                AddLength(connection.ConduitType, lengthDelta);
        //            }
        //        }
        //        else if (args.PropertyName == "ConnectionType" && Type == LengthType.Wire)
        //        {
        //            if (connection is TECNetworkConnection netConnect)
        //            {
        //                RemoveLength((args.OldValue as TECElectricalMaterial), netConnect.Length);
        //                AddLength((args.Value as TECElectricalMaterial), netConnect.Length);
        //            }
        //            else
        //            {
        //                throw new InvalidCastException("ConnectionType changed but sender isn't TECNetworkConnection.");
        //            }
        //        }
        //        else if (args.PropertyName == "ConduitType" && Type == LengthType.Conduit)
        //        {
        //            TECElectricalMaterial oldConduit = args.OldValue as TECElectricalMaterial;
        //            TECElectricalMaterial newConduit = args.Value as TECElectricalMaterial;
        //            if (oldConduit != null)
        //            {
        //                RemoveLength(oldConduit, connection.Length);
        //            }
        //            if (newConduit != null)
        //            {
        //                AddLength(newConduit, connection.Length);
        //            }
        //        }
        //    }
        //}

        //private void SubScope_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    if (e is PropertyChangedExtendedEventArgs args)
        //    {
        //        TECSubScopeConnection ssConnect = (sender as TECSubScope).Connection;
        //        if (args.PropertyName == "Devices")
        //        {
        //            TECDevice device = args.Value as TECDevice;
        //            if (args.Change == EstimatingLibrary.Utilities.Change.Add)
        //            {
        //                foreach(TECElectricalMaterial connectionType in device.ConnectionTypes)
        //                {
        //                    AddLength(connectionType, ssConnect.Length);
        //                }
        //            }
        //            else if (args.Change == EstimatingLibrary.Utilities.Change.Remove)
        //            {
        //                foreach(TECElectricalMaterial connectionType in device.ConnectionTypes)
        //                {
        //                    RemoveLength(connectionType, ssConnect.Length);
        //                }
        //            }
        //        }

        //    }
        //}
        //#endregion
    }
}
