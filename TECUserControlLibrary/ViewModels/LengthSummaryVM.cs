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

namespace TECUserControlLibrary.ViewModels
{
    public class LengthSummaryVM : ViewModelBase
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
            }
        }
        public double LengthLaborTotal
        {
            get { return _lengthLaborTotal; }
            private set
            {
                _lengthLaborTotal = value;
                RaisePropertyChanged("LengthLaborTotal");
            }
        }
        public double AssocTECCostTotal
        {
            get { return _assocTECCostTotal; }
            private set
            {
                _assocTECCostTotal = value;
                RaisePropertyChanged("AssocTECCostTotal");
            }
        }
        public double AssocTECLaborTotal
        {
            get { return _assocTECLaborTotal; }
            private set
            {
                _assocTECLaborTotal = value;
                RaisePropertyChanged("AssocTECLaborTotal");
            }
        }
        public double AssocElecCostTotal
        {
            get { return _assocElecCostTotal; }
            private set
            {
                _assocElecCostTotal = value;
                RaisePropertyChanged("AssocElecCostTotal");
            }
        }
        public double AssocElecLaborTotal
        {
            get { return _assocElecLaborTotal; }
            private set
            {
                _assocElecLaborTotal = value;
                RaisePropertyChanged("AssocElecLaborTotal");
            }
        }
        public double RatedTECCostTotal
        {
            get { return _ratedTECCostTotal; }
            private set
            {
                _ratedTECCostTotal = value;
                RaisePropertyChanged("RatedTECCostTotal");
            }
        }
        public double RatedTECLaborTotal
        {
            get { return _ratedTECLaborTotal; }
            private set
            {
                _ratedTECLaborTotal = value;
                RaisePropertyChanged("RatedTECLaborTotal");
            }
        }
        public double RatedElecCostTotal
        {
            get { return _ratedElecCostTotal; }
            private set
            {
                _ratedElecCostTotal = value;
                RaisePropertyChanged("RatedElecCostTotal");
            }
        }
        public double RatedElecLaborTotal
        {
            get { return _ratedElecLaborTotal; }
            private set
            {
                _ratedElecLaborTotal = value;
                RaisePropertyChanged("RatedElecLaborTotal");
            }
        }
        #endregion

        #region Methods
        public void Reset()
        {
            initialize();
        }

        public List<CostObject> AddRun(TECElectricalMaterial material, double length)
        {
            List<CostObject> deltas = AddLength(material, length);
            foreach (TECCost cost in material.AssociatedCosts)
            {
                deltas.AddRange(addAssocCost(cost));
            }
            return deltas;
        }
        public List<CostObject> RemoveRun(TECElectricalMaterial material, double length)
        {
            List<CostObject> deltas = RemoveLength(material, length);
            foreach (TECCost cost in material.AssociatedCosts)
            {
                deltas.AddRange(removeAssocCost(cost));
            }
            return deltas;
        }
        public List<CostObject> AddLength(TECElectricalMaterial material, double length)
        {
            List<CostObject> deltas = new List<CostObject>();
            bool containsItem = lengthDictionary.ContainsKey(material.Guid);
            if (containsItem)
            {
                LengthSummaryItem item = lengthDictionary[material.Guid];
                CostObject delta = item.AddLength(length);
                LengthCostTotal += delta.Cost;
                LengthLaborTotal += delta.Labor;
                delta.Type = CostType.Electrical;
                deltas.Add(delta);
            }
            else
            {
                LengthSummaryItem item = new LengthSummaryItem(material, length);
                lengthDictionary.Add(material.Guid, item);
                LengthSummaryItems.Add(item);
                LengthCostTotal += item.TotalCost;
                LengthLaborTotal += item.TotalLabor;
                deltas.Add(new CostObject(item.TotalCost, item.TotalLabor, CostType.Electrical));
            }
            foreach(TECCost cost in material.RatedCosts)
            {
                deltas.AddRange(addRatedCost(cost, length));
            }
            return deltas;
        }
        public List<CostObject> RemoveLength(TECElectricalMaterial material, double length)
        {
            bool containsItem = lengthDictionary.ContainsKey(material.Guid);
            if (containsItem)
            {
                List<CostObject> deltas = new List<CostObject>();
                LengthSummaryItem item = lengthDictionary[material.Guid];
                CostObject delta = item.RemoveLength(length);
                LengthCostTotal += delta.Cost;
                LengthLaborTotal += delta.Labor;
                deltas.Add(delta);

                if (item.Length <= 0)
                {
                    LengthSummaryItems.Remove(item);
                    lengthDictionary.Remove(material.Guid);
                }
                foreach(TECCost cost in material.RatedCosts)
                {
                    deltas.AddRange(removeRatedCost(cost, length));
                }
                return deltas;
            }
            else
            {
                throw new NullReferenceException("Length item not present in dictionary.");
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

        private List<CostObject> addAssocCost(TECCost cost)
        {
            List<CostObject> deltas = new List<CostObject>();
            bool containsItem = assocCostDictionary.ContainsKey(cost.Guid);
            if (containsItem)
            {
                CostSummaryItem item = assocCostDictionary[cost.Guid];
                CostObject delta = item.AddQuantity(1);
                deltas.Add(delta);
                if (cost.Type == CostType.TEC)
                {
                    AssocTECCostTotal += delta.Cost;
                    AssocTECLaborTotal += delta.Labor;
                }
                else if (cost.Type == CostType.Electrical)
                {
                    AssocElecCostTotal += delta.Cost;
                    AssocElecLaborTotal += delta.Labor;
                }
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
                deltas.Add(new CostObject(item.TotalCost, item.TotalLabor, cost.Type));
            }
            return deltas;
        }
        private List<CostObject> removeAssocCost(TECCost cost)
        {
            bool containsItem = assocCostDictionary.ContainsKey(cost.Guid);
            if (containsItem)
            {
                List<CostObject> deltas = new List<CostObject>();
                CostSummaryItem item = assocCostDictionary[cost.Guid];
                CostObject delta = item.RemoveQuantity(1);
                if (cost.Type == CostType.TEC)
                {
                    AssocTECCostTotal += delta.Cost;
                    AssocTECLaborTotal += delta.Labor;
                }
                else if (cost.Type == CostType.Electrical)
                {
                    AssocElecCostTotal += delta.Cost;
                    AssocElecLaborTotal += delta.Labor;
                }
                deltas.Add(delta);
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
        private List<CostObject> addRatedCost(TECCost cost, double length)
        {
            List<CostObject> deltas = new List<CostObject>();
            bool containsItem = ratedCostDictionary.ContainsKey(cost.Guid);
            if (containsItem)
            {
                RatedCostSummaryItem item = ratedCostDictionary[cost.Guid];
                CostObject delta = item.AddLength(length);
                deltas.Add(delta);
                if (cost.Type == CostType.TEC)
                {
                    RatedTECCostTotal += delta.Cost;
                    RatedTECLaborTotal += delta.Labor;
                }
                else if (cost.Type == CostType.Electrical)
                {
                    RatedElecCostTotal += delta.Cost;
                    RatedElecLaborTotal += delta.Labor;
                }
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
                deltas.Add(new CostObject(item.TotalCost, item.TotalLabor, cost.Type));
            }
            return deltas;
        }
        private List<CostObject> removeRatedCost(TECCost cost, double length)
        {
            bool containsItem = ratedCostDictionary.ContainsKey(cost.Guid);
            if (containsItem)
            {
                List<CostObject> deltas = new List<CostObject>();
                RatedCostSummaryItem item = ratedCostDictionary[cost.Guid];
                CostObject delta = item.RemoveLength(length);
                deltas.Add(delta);
                if (cost.Type == CostType.TEC)
                {
                    RatedTECCostTotal += delta.Cost;
                    RatedTECLaborTotal += delta.Labor;
                }
                else if (cost.Type == CostType.Electrical)
                {
                    RatedElecCostTotal += delta.Cost;
                    RatedElecLaborTotal += delta.Labor;
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
                return deltas;
            }
            else
            {
                throw new NullReferenceException("Cost item not present in dictionary.");
            }
        }
        #endregion
    }
}
