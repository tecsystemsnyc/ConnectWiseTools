using EstimateBuilder.Model;
using EstimatingLibrary;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TECUserControlLibrary.Models;

namespace TECUserControlLibrary.ViewModels
{
    public class PanelTypeSummaryVM : ViewModelBase
    {
        #region Properties
        private Dictionary<Guid, PanelTypeSummaryItem> panelTypeDictionary;

        private ObservableCollection<PanelTypeSummaryItem> _panelTypeSummaryItems;
        public ObservableCollection<PanelTypeSummaryItem> PanelTypeSummaryItems
        {
            get
            {
                return _panelTypeSummaryItems;
            }
            set
            {
                _panelTypeSummaryItems = value;
                RaisePropertyChanged("PanelTypeSummaryItems");
            }
        }

        private Dictionary<Guid, AssociatedCostSummaryItem> panelAssCostDictionary;

        private ObservableCollection<AssociatedCostSummaryItem> _panelAssCostSummaryItems;
        public ObservableCollection<AssociatedCostSummaryItem> PanelAssCostSummaryItems
        {
            get { return _panelAssCostSummaryItems; }
            set
            {
                _panelAssCostSummaryItems = value;
                RaisePropertyChanged("PanelAssCostSummaryItems");
            }
        }

        private double _panelTypeSubTotal;
        public double PanelTypeSubTotal
        {
            get { return _panelTypeSubTotal; }
            set
            {
                _panelTypeSubTotal = value;
                RaisePropertyChanged("PanelTypeSubTotal");
                RaisePropertyChanged("TotalPanelCost");
                RaisePropertyChanged("TotalCost");
            }
        }

        private double _panelAssCostSubTotalCost;
        public double PanelAssCostSubTotalCost
        {
            get { return _panelAssCostSubTotalCost; }
            set
            {
                _panelAssCostSubTotalCost = value;
                RaisePropertyChanged("PanelAssCostSubTotalCost");
                RaisePropertyChanged("TotalPanelCost");
                RaisePropertyChanged("TotalCost");
            }
        }

        private double _panelAssCostSubTotalLabor;
        public double PanelAssCostSubTotalLabor
        {
            get { return _panelAssCostSubTotalLabor; }
            set
            {
                _panelAssCostSubTotalLabor = value;
                RaisePropertyChanged("PanelAssCostSubTotalLabor");
                RaisePropertyChanged("TotalPanelLabor");
                RaisePropertyChanged("TotalLabor");
            }
        }
        #endregion

        public PanelTypeSummaryVM(TECBid bid)
        {
            reinitialize(bid);
        }

        public void Refresh(TECBid bid)
        {
            reinitialize(bid);
        }

        private void reinitialize(TECBid bid)
        {
            panelTypeDictionary = new Dictionary<Guid, PanelTypeSummaryItem>();
            PanelTypeSummaryItems = new ObservableCollection<PanelTypeSummaryItem>();
            panelAssCostDictionary = new Dictionary<Guid, AssociatedCostSummaryItem>();
            PanelAssCostSummaryItems = new ObservableCollection<AssociatedCostSummaryItem>();

            PanelTypeSubTotal = 0;
            PanelAssCostSubTotalCost = 0;
            PanelAssCostSubTotalLabor = 0;

            foreach (TECPanel panel in bid.Panels)
            {
                AddPanel(panel);
            }
        }

        #region Add/Remove
        public void AddPanel(TECPanel panel)
        {
            bool containsPanelType = panelTypeDictionary.ContainsKey(panel.Type.Guid);
            if (containsPanelType)
            {
                PanelTypeSubTotal -= panelTypeDictionary[panel.Type.Guid].Total;
                panelTypeDictionary[panel.Type.Guid].Quantity++;
                PanelTypeSubTotal += panelTypeDictionary[panel.Type.Guid].Total;
            }
            else
            {
                PanelTypeSummaryItem panelTypeItem = new PanelTypeSummaryItem(panel.Type);
                panelTypeDictionary.Add(panel.Type.Guid, panelTypeItem);
                PanelTypeSummaryItems.Add(panelTypeItem);
                PanelTypeSubTotal += panelTypeItem.Total;
            }
            foreach (TECCost cost in panel.AssociatedCosts)
            {
                Tuple<double, double> delta = TECMaterialSummaryVM.AddCost(cost, panelAssCostDictionary, PanelAssCostSummaryItems);
                PanelAssCostSubTotalCost += delta.Item1;
                PanelAssCostSubTotalLabor += delta.Item2;
            }
        }

        public void RemovePanel(TECPanel panel)
        {
            bool containsPanelType = panelTypeDictionary.ContainsKey(panel.Type.Guid);
            if (containsPanelType)
            {
                PanelTypeSubTotal -= panelTypeDictionary[panel.Type.Guid].Total;
                panelTypeDictionary[panel.Type.Guid].Quantity--;
                PanelTypeSubTotal += panelTypeDictionary[panel.Type.Guid].Total;

                if (panelTypeDictionary[panel.Type.Guid].Quantity < 1)
                {
                    PanelTypeSummaryItems.Remove(panelTypeDictionary[panel.Type.Guid]);
                    panelTypeDictionary.Remove(panel.Type.Guid);
                }

                foreach (TECCost cost in panel.AssociatedCosts)
                {
                    Tuple<double, double> delta = TECMaterialSummaryVM.RemoveCost(cost, panelAssCostDictionary, PanelAssCostSummaryItems);
                    PanelAssCostSubTotalCost += delta.Item1;
                    PanelAssCostSubTotalLabor += delta.Item2;
                }
            }
            else
            {
                throw new InvalidOperationException("Panel type not found in panel type dictionary.");
            }
        }

        public void AddCostToPanel(TECCost cost)
        {
            Tuple<double, double> delta = TECMaterialSummaryVM.AddCost(cost, panelAssCostDictionary, PanelAssCostSummaryItems);
            PanelAssCostSubTotalCost += delta.Item1;
            PanelAssCostSubTotalLabor += delta.Item2;
        }

        public void RemoveCostFromPanel(TECCost cost)
        {
            Tuple<double, double> delta = TECMaterialSummaryVM.RemoveCost(cost, panelAssCostDictionary, PanelAssCostSummaryItems);
            PanelAssCostSubTotalCost += delta.Item1;
            PanelAssCostSubTotalLabor += delta.Item2;
        }
        #endregion
    }
}
