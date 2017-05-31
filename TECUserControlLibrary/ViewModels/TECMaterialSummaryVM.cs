using EstimateBuilder.Model;
using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using TECUserControlLibrary.Models;

namespace TECUserControlLibrary.ViewModels
{
    public class TECMaterialSummaryVM : ViewModelBase
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

        #region View Models
        public DeviceSummaryVM DeviceSummaryVM { get; private set; }
        #endregion

        #region Controller View Properties
        private Dictionary<Guid, AssociatedCostSummaryItem> controllerAssCostDictionary;

        private ObservableCollection<AssociatedCostSummaryItem> _controllerAssCostSummaryItems;
        public ObservableCollection<AssociatedCostSummaryItem> ControllerAssCostSummaryItems
        {
            get
            {
                return _controllerAssCostSummaryItems;
            }
            set
            {
                _controllerAssCostSummaryItems = value;
                RaisePropertyChanged("ControllerAssCostSummaryItems");
            }
        }

        private double _controllerSubTotal;
        public double ControllerSubTotal
        {
            get { return _controllerSubTotal; }
            set
            {
                _controllerSubTotal = value;
                RaisePropertyChanged("ControllerSubTotal");
                RaisePropertyChanged("TotalControllerCost");
                RaisePropertyChanged("TotalCost");
            }
        }

        private double _controllerAssCostSubTotalCost;
        public double ControllerAssCostSubTotalCost
        {
            get { return _controllerAssCostSubTotalCost; }
            set
            {
                _controllerAssCostSubTotalCost = value;
                RaisePropertyChanged("ControllerAssCostSubTotalCost");
                RaisePropertyChanged("TotalControllerCost");
                RaisePropertyChanged("TotalCost");
            }
        }

        private double _controllerAssCostSubTotalLabor;
        public double ControllerAssCostSubTotalLabor
        {
            get { return _controllerAssCostSubTotalLabor; }
            set
            {
                _controllerAssCostSubTotalLabor = value;
                RaisePropertyChanged("ControllerAssCostSubTotalLabor");
                RaisePropertyChanged("TotalControllerLabor");
                RaisePropertyChanged("TotalLabor");
            }
        }
        #endregion

        #region Panel View Properties
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

        #region Misc Cost View Properties
        private ObservableCollection<TECMisc> _miscCosts;
        public ObservableCollection<TECMisc> MiscCosts
        {
            get { return _miscCosts; }
            set
            {
                _miscCosts = value;
                RaisePropertyChanged("MiscCosts");
            }
        }

        private double _miscCostSubTotalCost;
        public double MiscCostSubTotalCost
        {
            get { return _miscCostSubTotalCost; }
            set
            {
                _miscCostSubTotalCost = value;
                RaisePropertyChanged("MiscCostSubTotalCost");
                RaisePropertyChanged("TotalMiscCost");
                RaisePropertyChanged("TotalCost");
            }
        }

        private double _miscCostSubTotalLabor;
        public double MiscCostSubTotalLabor
        {
            get { return _miscCostSubTotalLabor; }
            set
            {
                _miscCostSubTotalLabor = value;
                RaisePropertyChanged("MiscCostSubTotalLabor");
                RaisePropertyChanged("TotalMiscLabor");
                RaisePropertyChanged("TotalLabor");
            }
        }
        #endregion

        #region Summary Totals
        public double TotalDeviceCost
        {
            get
            {
                return (DeviceSummaryVM.DeviceSubTotal + DeviceSummaryVM.DeviceAssCostSubTotalCost);
            }
        }

        public double TotalDeviceLabor
        {
            get
            {
                return (DeviceSummaryVM.DeviceAssCostSubTotalLabor);
            }
        }

        public double TotalControllerCost
        {
            get
            {
                return (ControllerSubTotal + ControllerAssCostSubTotalCost);
            }
        }

        public double TotalControllerLabor
        {
            get { return ControllerAssCostSubTotalLabor; }
        }

        public double TotalPanelCost
        {
            get { return (PanelTypeSubTotal + PanelAssCostSubTotalCost); }
        }

        public double TotalPanelLabor
        {
            get { return PanelAssCostSubTotalLabor; }
        }

        public double TotalMiscCost
        {
            get { return MiscCostSubTotalCost; }
        }

        public double TotalMiscLabor
        {
            get { return MiscCostSubTotalLabor; }
        }

        public double TotalCost
        {
            get { return (TotalDeviceCost + TotalControllerCost + TotalPanelCost + TotalMiscCost); }
        }

        public double TotalLabor
        {
            get { return (TotalDeviceLabor + TotalControllerLabor + TotalPanelLabor + TotalMiscLabor); }
        }
        #endregion

        #endregion

        public TECMaterialSummaryVM(TECBid bid)
        {
            reinitialize(bid);
            DeviceSummaryVM.PropertyChanged += DeviceSummaryVM_PropertyChanged;
        }

        public void Refresh(TECBid bid)
        {
            reinitialize(bid);
        }

        private void reinitialize(TECBid bid)
        {
            DeviceSummaryVM = new DeviceSummaryVM(bid);

            controllerAssCostDictionary = new Dictionary<Guid, AssociatedCostSummaryItem>();
            ControllerAssCostSummaryItems = new ObservableCollection<AssociatedCostSummaryItem>();

            panelTypeDictionary = new Dictionary<Guid, PanelTypeSummaryItem>();
            PanelTypeSummaryItems = new ObservableCollection<PanelTypeSummaryItem>();
            panelAssCostDictionary = new Dictionary<Guid, AssociatedCostSummaryItem>();
            PanelAssCostSummaryItems = new ObservableCollection<AssociatedCostSummaryItem>();

            MiscCosts = new ObservableCollection<TECMisc>();
            
            ControllerSubTotal = 0;
            ControllerAssCostSubTotalCost = 0;
            ControllerAssCostSubTotalLabor = 0;
            PanelTypeSubTotal = 0;
            PanelAssCostSubTotalCost = 0;
            PanelAssCostSubTotalLabor = 0;
            MiscCostSubTotalCost = 0;
            MiscCostSubTotalLabor = 0;

            foreach (TECController controller in bid.Controllers)
            {
                addController(controller);
            }
            foreach(TECPanel panel in bid.Panels)
            {
                addPanel(panel);
            }
            foreach(TECMisc cost in bid.MiscCosts)
            {
                addMiscCost(cost);
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

                if (args.PropertyName == "Add" || args.PropertyName == "AddCatalog")
                {
                    if (targetObject is TECSystem && referenceObject is TECBid)
                    {
                        DeviceSummaryVM.AddSystem(targetObject as TECSystem);
                    }
                    else if (targetObject is TECEquipment && referenceObject is TECSystem)
                    {
                        DeviceSummaryVM.AddEquipment(targetObject as TECEquipment);
                    }
                    else if (targetObject is TECSubScope && referenceObject is TECEquipment)
                    {
                        DeviceSummaryVM.AddSubScope(targetObject as TECSubScope);
                    }
                    else if (targetObject is TECDevice && referenceObject is TECSubScope)
                    {
                        DeviceSummaryVM.AddDevice(targetObject as TECDevice);
                    }
                    else if (targetObject is TECCost && referenceObject is TECDevice)
                    {
                        DeviceSummaryVM.AddCostToDevices(targetObject as TECCost, referenceObject as TECDevice);
                    }
                    else if (targetObject is TECController && referenceObject is TECBid)
                    {
                        addController(targetObject as TECController);
                    }
                    else if (targetObject is TECPanel && referenceObject is TECBid)
                    {
                        addPanel(targetObject as TECPanel);
                    }
                    else if (targetObject is TECCost && referenceObject is TECController)
                    {
                        addCostToController(targetObject as TECCost);
                    }
                    else if (targetObject is TECCost && referenceObject is TECPanel)
                    {
                        addCostToPanel(targetObject as TECCost);
                    }
                    else if (targetObject is TECMisc && referenceObject is TECBid)
                    {
                        addMiscCost(targetObject as TECMisc);
                    }
 
                }
                else if (args.PropertyName == "Remove" || args.PropertyName == "RemoveCatalog")
                {
                    if (targetObject is TECSystem && referenceObject is TECBid)
                    {
                        DeviceSummaryVM.RemoveSystem(targetObject as TECSystem);
                    }
                    else if (targetObject is TECEquipment && referenceObject is TECSystem)
                    {
                        DeviceSummaryVM.RemoveEquipment(targetObject as TECEquipment);
                    }
                    else if (targetObject is TECSubScope && referenceObject is TECEquipment)
                    {
                        DeviceSummaryVM.RemoveSubScope(targetObject as TECSubScope);
                    }
                    else if (targetObject is TECDevice && referenceObject is TECSubScope)
                    {
                        DeviceSummaryVM.RemoveDevice(targetObject as TECDevice);
                    }
                    else if (targetObject is TECCost && referenceObject is TECDevice)
                    {
                        DeviceSummaryVM.RemoveCostFromDevices(targetObject as TECCost, referenceObject as TECDevice);
                    }
                    else if (targetObject is TECController && referenceObject is TECBid)
                    {
                        removeController(targetObject as TECController);
                    }
                    else if (targetObject is TECPanel && referenceObject is TECBid)
                    {
                        removePanel(targetObject as TECPanel);
                    }
                    else if (targetObject is TECCost && referenceObject is TECController)
                    {
                        removeCostFromController(targetObject as TECCost);
                    }
                    else if (targetObject is TECCost && referenceObject is TECPanel)
                    {
                        removeCostFromPanel(targetObject as TECCost);
                    }
                    else if (targetObject is TECMisc && referenceObject is TECBid)
                    {
                        removeMiscCost(targetObject as TECMisc);
                    }
                }
                else if (args.PropertyName == "Cost" || args.PropertyName == "Quantity")
                {
                    if (args.OldValue is TECMisc && args.NewValue is TECMisc)
                    {
                        removeMiscCost(args.OldValue as TECMisc);
                        addMiscCost(args.NewValue as TECMisc);
                    }
                }
            }
        }

        private void raiseTotals()
        {
            RaisePropertyChanged("TotalCost");
            RaisePropertyChanged("TotalLabor");
        }
        #endregion

        #region Add/Remove
        private void addController(TECController controller)
        {
            ControllerSubTotal += controller.Cost * controller.Manufacturer.Multiplier;
            foreach(TECCost cost in controller.AssociatedCosts)
            {
                addCostToController(cost);
            }
        }

        private void removeController(TECController controller)
        {
            ControllerSubTotal -= controller.Cost * controller.Manufacturer.Multiplier;
            foreach(TECCost cost in controller.AssociatedCosts)
            {
                removeCostFromController(cost);
            }
        }

        private void addCostToController(TECCost cost)
        {
            Tuple<double, double> delta = AddCost(cost, controllerAssCostDictionary, ControllerAssCostSummaryItems);
            ControllerAssCostSubTotalCost += delta.Item1;
            ControllerAssCostSubTotalLabor += delta.Item2;
        }

        private void removeCostFromController(TECCost cost)
        {
            Tuple<double, double> delta = RemoveCost(cost, controllerAssCostDictionary, ControllerAssCostSummaryItems);
            ControllerAssCostSubTotalCost += delta.Item1;
            ControllerAssCostSubTotalLabor += delta.Item2;
        }

        private void addPanel(TECPanel panel)
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
                Tuple<double, double> delta = AddCost(cost, panelAssCostDictionary, PanelAssCostSummaryItems);
                PanelAssCostSubTotalCost += delta.Item1;
                PanelAssCostSubTotalLabor += delta.Item2;
            }
        }

        private void removePanel(TECPanel panel)
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
                    Tuple<double, double> delta = RemoveCost(cost, panelAssCostDictionary, PanelAssCostSummaryItems);
                    PanelAssCostSubTotalCost += delta.Item1;
                    PanelAssCostSubTotalLabor += delta.Item2;
                }
            }
            else
            {
                throw new InvalidOperationException("Panel type not found in panel type dictionary.");
            }
        }

        private void addCostToPanel(TECCost cost)
        {
            Tuple<double, double> delta = AddCost(cost, panelAssCostDictionary, PanelAssCostSummaryItems);
            PanelAssCostSubTotalCost += delta.Item1;
            PanelAssCostSubTotalLabor += delta.Item2;
        }

        private void removeCostFromPanel(TECCost cost)
        {
            Tuple<double, double> delta = RemoveCost(cost, panelAssCostDictionary, PanelAssCostSummaryItems);
            PanelAssCostSubTotalCost += delta.Item1;
            PanelAssCostSubTotalLabor += delta.Item2;
        }

        private void addMiscCost(TECMisc cost)
        {
            MiscCosts.Add(cost);
            MiscCostSubTotalCost += cost.Cost * cost.Quantity;
            MiscCostSubTotalLabor += cost.Labor * cost.Quantity;
        }

        private void removeMiscCost(TECMisc cost)
        {
            MiscCosts.Remove(cost);
            MiscCostSubTotalCost -= cost.Cost * cost.Quantity;
            MiscCostSubTotalLabor -= cost.Labor * cost.Quantity;
        }

        static public Tuple<double, double> AddCost(TECCost cost, Dictionary<Guid, AssociatedCostSummaryItem> dictionary, ObservableCollection<AssociatedCostSummaryItem> collection)
        {
            double costChange = 0;
            double laborChange = 0;
            bool containsCost = dictionary.ContainsKey(cost.Guid);
            if (containsCost)
            {
                costChange -= dictionary[cost.Guid].TotalCost;
                laborChange -= dictionary[cost.Guid].TotalLabor;
                dictionary[cost.Guid].Quantity++;
                costChange += dictionary[cost.Guid].TotalCost;
                laborChange += dictionary[cost.Guid].TotalLabor;
            }
            else
            {
                AssociatedCostSummaryItem costItem = new AssociatedCostSummaryItem(cost);
                dictionary.Add(cost.Guid, costItem);
                collection.Add(costItem);
                costChange += dictionary[cost.Guid].TotalCost;
                laborChange += dictionary[cost.Guid].TotalLabor;
            }
            return Tuple.Create(costChange, laborChange);
        }

        static public Tuple<double, double> RemoveCost(TECCost cost, Dictionary<Guid, AssociatedCostSummaryItem> dictionary, ObservableCollection<AssociatedCostSummaryItem> collection)
        {
            double costChange = 0;
            double laborChange = 0;
            bool containsCost = dictionary.ContainsKey(cost.Guid);
            if (containsCost)
            {
                costChange -= dictionary[cost.Guid].TotalCost;
                laborChange -= dictionary[cost.Guid].TotalLabor;
                dictionary[cost.Guid].Quantity--;
                costChange += dictionary[cost.Guid].TotalCost;
                laborChange += dictionary[cost.Guid].TotalLabor;

                if (dictionary[cost.Guid].Quantity < 1)
                {
                    collection.Remove(dictionary[cost.Guid]);
                    dictionary.Remove(cost.Guid);
                }

                return Tuple.Create(costChange, laborChange);
            }
            else
            {
                throw new InvalidOperationException("Associated Cost not found in dictionary.");
            }
        }
        #endregion

        #region Event Handlers
        private void DeviceSummaryVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "DeviceSubTotal" || e.PropertyName == "DeviceAssCostSubTotalCost")
            {
                RaisePropertyChanged("TotalDeviceCost");
                raiseTotals();
            }
            else if (e.PropertyName == "DeviceAssCostSubTotalLabor")
            {
                RaisePropertyChanged("TotalDeviceLabor");
                raiseTotals();
            }
        }
        #endregion
    }
}