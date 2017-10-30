using EstimatingLibrary;
using EstimatingLibrary.Interfaces;
using EstimatingLibrary.Utilities;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECUserControlLibrary.Models;
using TECUserControlLibrary.ViewModels.Interfaces;

namespace TECUserControlLibrary.ViewModels
{
    public class LengthSummaryVM : ViewModelBase, IComponentSummaryVM
    {
        #region Fields
        private Dictionary<Guid, LengthSummaryItem> lengthDictionary;
        private Dictionary<Guid, CostSummaryItem> assocCostDictionary;
        private Dictionary<Guid, RatedCostSummaryItem> ratedCostDictionary;

        private ObservableCollection<LengthSummaryItem> _lengthSummaryItems;
        private ObservableCollection<CostSummaryItem> _assocTECItems;
        private ObservableCollection<CostSummaryItem> _assocElecItems;
        private ObservableCollection<RatedCostSummaryItem> _ratedTECItems;
        private ObservableCollection<RatedCostSummaryItem> _ratedElecItems;

        private double _lengthCostTotal;
        private double _lengthLaborTotal;
        private double _assocTECCostTotal;
        private double _assocTECLaborTotal;
        private double _assocElecCostTotal;
        private double _assocElecLaborTotal;
        private double _ratedTECCostTotal;
        private double _ratedTECLaborTotal;
        private double _ratedElecCostTotal;
        private double _ratedElecLaborTotal;
        #endregion

        //Constructor
        public LengthSummaryVM()
        {
            initialize();
        }

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
        public ObservableCollection<CostSummaryItem> AssocTECItems
        {
            get { return _assocTECItems; }
            private set
            {
                _assocTECItems = value;
                RaisePropertyChanged("AssocTECItems");
            }
        }
        public ObservableCollection<CostSummaryItem> AssocElecItems
        {
            get { return _assocElecItems; }
            private set
            {
                _assocElecItems = value;
                RaisePropertyChanged("AssocElecItems");
            }
        }
        public ObservableCollection<RatedCostSummaryItem> RatedTECItems
        {
            get { return _ratedTECItems; }
            private set
            {
                _ratedTECItems = value;
                RaisePropertyChanged("RatedTECItems");
            }
        }
        public ObservableCollection<RatedCostSummaryItem> RatedElecItems
        {
            get { return _ratedElecItems; }
            private set
            {
                _ratedElecItems = value;
                RaisePropertyChanged("RatedElecItems");
            }
        }

        public double LengthCostTotal
        {
            get { return _lengthCostTotal; }
            private set
            {
                _lengthCostTotal = value;
                RaisePropertyChanged("LengthCostTotal");
                RaisePropertyChanged("TotalElecCost");
            }
        }
        public double LengthLaborTotal
        {
            get { return _lengthLaborTotal; }
            private set
            {
                _lengthLaborTotal = value;
                RaisePropertyChanged("LengthLaborTotal");
                RaisePropertyChanged("TotalElecLabor");
            }
        }
        public double AssocTECCostTotal
        {
            get { return _assocTECCostTotal; }
            private set
            {
                _assocTECCostTotal = value;
                RaisePropertyChanged("AssocTECCostTotal");
                RaisePropertyChanged("TotalTECCost");
            }
        }
        public double AssocTECLaborTotal
        {
            get { return _assocTECLaborTotal; }
            private set
            {
                _assocTECLaborTotal = value;
                RaisePropertyChanged("AssocTECLaborTotal");
                RaisePropertyChanged("TotalTECLabor");
            }
        }
        public double AssocElecCostTotal
        {
            get { return _assocElecCostTotal; }
            private set
            {
                _assocElecCostTotal = value;
                RaisePropertyChanged("AssocElecCostTotal");
                RaisePropertyChanged("TotalElecCost");
            }
        }
        public double AssocElecLaborTotal
        {
            get { return _assocElecLaborTotal; }
            private set
            {
                _assocElecLaborTotal = value;
                RaisePropertyChanged("AssocElecLaborTotal");
                RaisePropertyChanged("TotalElecLabor");
            }
        }
        public double RatedTECCostTotal
        {
            get { return _ratedTECCostTotal; }
            private set
            {
                _ratedTECCostTotal = value;
                RaisePropertyChanged("RatedTECCostTotal");
                RaisePropertyChanged("TotalTECCost");
            }
        }
        public double RatedTECLaborTotal
        {
            get { return _ratedTECLaborTotal; }
            private set
            {
                _ratedTECLaborTotal = value;
                RaisePropertyChanged("RatedTECLaborTotal");
                RaisePropertyChanged("TotalTECLabor");
            }
        }
        public double RatedElecCostTotal
        {
            get { return _ratedElecCostTotal; }
            private set
            {
                _ratedElecCostTotal = value;
                RaisePropertyChanged("RatedElecCostTotal");
                RaisePropertyChanged("TotalElecCost");
            }
        }
        public double RatedElecLaborTotal
        {
            get { return _ratedElecLaborTotal; }
            private set
            {
                _ratedElecLaborTotal = value;
                RaisePropertyChanged("RatedElecLaborTotal");
                RaisePropertyChanged("TotalElecLabor");
            }
        }

        public double TotalTECCost
        {
            get
            {
                return (AssocTECCostTotal + RatedTECCostTotal);
            }
        }
        public double TotalTECLabor
        {
            get
            {
                return (AssocTECLaborTotal + RatedTECLaborTotal);
            }
        }
        public double TotalElecCost
        {
            get
            {
                return (LengthCostTotal + AssocElecCostTotal + RatedElecCostTotal);
            }
        }
        public double TotalElecLabor
        {
            get
            {
                return (LengthLaborTotal + AssocElecLaborTotal + RatedElecLaborTotal);
            }
        }
        #endregion

        #region Methods
        public void Reset()
        {
            initialize();
        }

        public CostBatch AddRun(TECElectricalMaterial material, double length)
        {
            CostBatch deltas = AddLength(material, length);
            foreach (TECCost cost in material.AssociatedCosts)
            {
                deltas += addAssocCost(cost);
            }
            return deltas;
        }
        public CostBatch RemoveRun(TECElectricalMaterial material, double length)
        {
            CostBatch deltas = RemoveLength(material, length);
            foreach (TECCost cost in material.AssociatedCosts)
            {
                deltas += removeAssocCost(cost);
            }
            return deltas;
        }
        public CostBatch AddLength(TECElectricalMaterial material, double length)
        {
            CostBatch deltas = new CostBatch();
            bool containsItem = lengthDictionary.ContainsKey(material.Guid);
            if (containsItem)
            {
                LengthSummaryItem item = lengthDictionary[material.Guid];
                CostBatch delta = item.AddLength(length);
                LengthCostTotal += delta.GetCost(CostType.Electrical);
                LengthLaborTotal += delta.GetLabor(CostType.Electrical);
                deltas += delta;
            }
            else
            {
                LengthSummaryItem item = new LengthSummaryItem(material, length);
                lengthDictionary.Add(material.Guid, item);
                LengthSummaryItems.Add(item);
                LengthCostTotal += item.TotalCost;
                LengthLaborTotal += item.TotalLabor;
                deltas += new CostBatch(item.TotalCost, item.TotalLabor, CostType.Electrical);
            }
            foreach(TECCost cost in material.RatedCosts)
            {
                deltas += addRatedCost(cost, length);
            }
            return deltas;
        }
        public CostBatch RemoveLength(TECElectricalMaterial material, double length)
        {
            if (length > 0)
            {
                bool containsItem = lengthDictionary.ContainsKey(material.Guid);
                if (containsItem)
                {
                    CostBatch deltas = new CostBatch();
                    LengthSummaryItem item = lengthDictionary[material.Guid];
                    CostBatch delta = item.RemoveLength(length);
                    LengthCostTotal += delta.GetCost(CostType.Electrical);
                    LengthLaborTotal += delta.GetLabor(CostType.Electrical);
                    deltas += delta;

                    if (item.Length <= 0)
                    {
                        LengthSummaryItems.Remove(item);
                        lengthDictionary.Remove(material.Guid);
                    }
                    foreach (TECCost cost in material.RatedCosts)
                    {
                        deltas += removeRatedCost(cost, length);
                    }
                    return deltas;
                }
                else
                {
                    throw new NullReferenceException("Length item not present in dictionary.");
                }
            }
            else
            {
                return new CostBatch();
            }
        }

        private void initialize()
        {
            lengthDictionary = new Dictionary<Guid, LengthSummaryItem>();
            assocCostDictionary = new Dictionary<Guid, CostSummaryItem>();
            ratedCostDictionary = new Dictionary<Guid, RatedCostSummaryItem>();

            LengthSummaryItems = new ObservableCollection<LengthSummaryItem>();
            AssocTECItems = new ObservableCollection<CostSummaryItem>();
            AssocElecItems = new ObservableCollection<CostSummaryItem>();
            RatedTECItems = new ObservableCollection<RatedCostSummaryItem>();
            RatedElecItems = new ObservableCollection<RatedCostSummaryItem>();

            LengthCostTotal = 0;
            LengthLaborTotal = 0;
            AssocTECCostTotal = 0;
            AssocTECLaborTotal = 0;
            AssocElecCostTotal = 0;
            AssocElecLaborTotal = 0;
            RatedTECCostTotal = 0;
            RatedTECLaborTotal = 0;
            RatedElecCostTotal = 0;
            RatedElecLaborTotal = 0;
        }

        private CostBatch addAssocCost(TECCost cost)
        {
            CostBatch deltas = new CostBatch();
            bool containsItem = assocCostDictionary.ContainsKey(cost.Guid);
            if (containsItem)
            {
                CostSummaryItem item = assocCostDictionary[cost.Guid];
                CostBatch delta = item.AddQuantity(1);
                AssocTECCostTotal += delta.GetCost(CostType.TEC);
                AssocTECLaborTotal += delta.GetLabor(CostType.TEC);
                AssocElecCostTotal += delta.GetCost(CostType.Electrical);
                AssocElecLaborTotal += delta.GetLabor(CostType.Electrical);
                deltas += delta;
            }
            else
            {
                CostSummaryItem item = new CostSummaryItem(cost);
                assocCostDictionary.Add(cost.Guid, item);
                if (cost.Type == CostType.TEC)
                {
                    AssocTECItems.Add(item);
                    AssocTECCostTotal += item.TotalCost;
                    AssocTECLaborTotal += item.TotalLabor;
                }
                else if (cost.Type == CostType.Electrical)
                {
                    AssocElecItems.Add(item);
                    AssocElecCostTotal += item.TotalCost;
                    AssocElecLaborTotal += item.TotalLabor;
                }
                deltas += new CostBatch(item.TotalCost, item.TotalLabor, cost.Type);
            }
            return deltas;
        }
        private CostBatch removeAssocCost(TECCost cost)
        {
            bool containsItem = assocCostDictionary.ContainsKey(cost.Guid);
            if (containsItem)
            {
                CostBatch deltas = new CostBatch();
                CostSummaryItem item = assocCostDictionary[cost.Guid];
                CostBatch delta = item.RemoveQuantity(1);
                AssocTECCostTotal += delta.GetCost(CostType.TEC);
                AssocTECLaborTotal += delta.GetLabor(CostType.TEC);
                AssocElecCostTotal += delta.GetCost(CostType.Electrical);
                AssocElecLaborTotal += delta.GetLabor(CostType.Electrical);
                deltas += delta;

                if (item.Quantity < 1)
                {
                    assocCostDictionary.Remove(cost.Guid);
                    if (cost.Type == CostType.TEC)
                    {
                        AssocTECItems.Remove(item);
                    }
                    else if (cost.Type == CostType.Electrical)
                    {
                        AssocElecItems.Remove(item);
                    }
                }
                return deltas;
            }
            else
            {
                throw new NullReferenceException("Cost item not present in dictionary.");
            }
        }
        private CostBatch addRatedCost(TECCost cost, double length)
        {
            bool containsItem = ratedCostDictionary.ContainsKey(cost.Guid);
            if (containsItem)
            {
                RatedCostSummaryItem item = ratedCostDictionary[cost.Guid];
                CostBatch delta = item.AddLength(length);
                if (cost.Type == CostType.TEC)
                {
                    RatedTECCostTotal += delta.GetCost(CostType.TEC);
                    RatedTECLaborTotal += delta.GetLabor(CostType.TEC);
                }
                else if (cost.Type == CostType.Electrical)
                {
                    RatedElecCostTotal += delta.GetCost(CostType.Electrical);
                    RatedElecLaborTotal += delta.GetLabor(CostType.Electrical);
                }
                return delta;
            }
            else
            {
                RatedCostSummaryItem item = new RatedCostSummaryItem(cost, length);
                ratedCostDictionary.Add(cost.Guid, item);
                if (cost.Type == CostType.TEC)
                {
                    RatedTECItems.Add(item);
                    RatedTECCostTotal += item.TotalCost;
                    RatedTECLaborTotal += item.TotalLabor;
                }
                else if (cost.Type == CostType.Electrical)
                {
                    RatedElecItems.Add(item);
                    RatedElecCostTotal += item.TotalCost;
                    RatedElecLaborTotal += item.TotalLabor;
                }
                return new CostBatch(item.TotalCost, item.TotalLabor, cost.Type);
            }
        }
        private CostBatch removeRatedCost(TECCost cost, double length)
        {
            if (length > 0)
            {
                bool containsItem = ratedCostDictionary.ContainsKey(cost.Guid);
                if (containsItem)
                {
                    RatedCostSummaryItem item = ratedCostDictionary[cost.Guid];
                    CostBatch delta = item.RemoveLength(length);
                    if (cost.Type == CostType.TEC)
                    {
                        RatedTECCostTotal += delta.GetCost(CostType.TEC);
                        RatedTECLaborTotal += delta.GetLabor(CostType.TEC);
                    }
                    else if (cost.Type == CostType.Electrical)
                    {
                        RatedElecCostTotal += delta.GetCost(CostType.Electrical);
                        RatedElecLaborTotal += delta.GetLabor(CostType.Electrical);
                    }
                    if (item.Length <= 0)
                    {
                        ratedCostDictionary.Remove(cost.Guid);
                        if (cost.Type == CostType.TEC)
                        {
                            RatedTECItems.Remove(item);
                        }
                        else if (cost.Type == CostType.Electrical)
                        {
                            RatedElecItems.Remove(item);
                        }
                    }
                    return delta;
                }
                else
                {
                    throw new NullReferenceException("Cost item not present in dictionary.");
                }
            }
            else
            {
                return new CostBatch();
            }
        }
        #endregion
    }
}
