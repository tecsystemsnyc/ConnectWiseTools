using EstimatingLibrary;
using EstimatingUtilitiesLibrary;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TECUserControlLibrary.Utilities;

namespace TECUserControlLibrary.ViewModels
{
    public class ScopeCollectionsTabVM : ViewModelBase, IDropTarget
    {
        #region Properties
        public TECTemplates Templates
        {
            get { return _templates; }
            set
            {
                unsubscribeTemplatesCollections();
                _templates = value;
                subscribeTemplatesCollections();
                RaisePropertyChanged("Templates");
            }
        }
        public ObservableCollection<TECObject> ResultCollection
        {
            get { return _resultCollection; }
            set
            {
                _resultCollection = value;
                RaisePropertyChanged("ResultCollection");
            }

        }
        public AllSearchableObjects ChosenType
        {
            get { return _chosenType; }
            set
            {
                _chosenType = value;
                RaisePropertyChanged("ChosenType");
            }
        }


        private ObservableCollection<TECObject> _resultCollection;
        private TECTemplates _templates;
        private AllSearchableObjects _chosenType;

        public ICommand SearchCollectionCommand { get; private set; }
        public ICommand EndSearchCommand { get; private set; }

        #region Delegates
        public Action<IDropInfo> DragHandler;
        public Action<IDropInfo> DropHandler;
        #endregion
        
        #region Search
        private string _searchString;

        public string SearchString
        {
            get { return _searchString; }
            set
            {
                _searchString = value;
                RaisePropertyChanged("SearchString");
            }
        }
        #endregion
        #endregion

        #region Intializers
        public ScopeCollectionsTabVM(TECTemplates templates)
        {
            Templates = templates;
            SearchCollectionCommand = new RelayCommand(SearchCollectionExecute, SearchCanExecute);
            EndSearchCommand = new RelayCommand(EndSearchExecute);
            populateItemsCollections();
            SearchString = "";
        }
        #endregion

        #region Methods

        public void Refresh(TECTemplates templates)
        {
            Templates = templates;
            populateItemsCollections();
        }

        #region Commands

        private void SearchCollectionExecute()
        {
            char dilemeter = ',';
            string[] searchCriteria = SearchString.ToUpper().Split(dilemeter);
            switch (ChosenType)
            {
                case AllSearchableObjects.System:
                    ResultCollection = new ObservableCollection<TECObject>();
                    foreach (TECSystem item in Templates.SystemTemplates)
                    {
                        if (UtilitiesMethods.StringContainsStrings(item.Name.ToUpper(), searchCriteria) ||
                            UtilitiesMethods.StringContainsStrings(item.Description.ToUpper(), searchCriteria))
                        {
                            ResultCollection.Add(item);
                        }
                        else
                        {
                            foreach (TECLabeled tag in item.Tags)
                            {
                                if (UtilitiesMethods.StringContainsStrings(tag.Label.ToUpper(), searchCriteria))
                                {
                                    ResultCollection.Add(item);
                                }
                            }
                        }
                    }
                    break;
                case AllSearchableObjects.Equipment:
                    ResultCollection = new ObservableCollection<TECObject>();
                    foreach (TECEquipment item in Templates.EquipmentTemplates)
                    {
                        if (UtilitiesMethods.StringContainsStrings(item.Name.ToUpper(), searchCriteria) ||
                            UtilitiesMethods.StringContainsStrings(item.Description.ToUpper(), searchCriteria))
                        {
                            ResultCollection.Add(item);
                        }
                        else
                        {
                            foreach (TECLabeled tag in item.Tags)
                            {
                                if (UtilitiesMethods.StringContainsStrings(tag.Label.ToUpper(), searchCriteria))
                                {
                                    ResultCollection.Add(item);
                                }
                            }
                        }
                    }
                    break;
                case AllSearchableObjects.SubScope:
                    ResultCollection = new ObservableCollection<TECObject>();
                    foreach (TECSubScope item in Templates.SubScopeTemplates)
                    {
                        if (UtilitiesMethods.StringContainsStrings(item.Name.ToUpper(), searchCriteria) ||
                            UtilitiesMethods.StringContainsStrings(item.Description.ToUpper(), searchCriteria))
                        {
                            ResultCollection.Add(item);
                        }
                        else
                        {
                            foreach (TECLabeled tag in item.Tags)
                            {
                                if (UtilitiesMethods.StringContainsStrings(tag.Label.ToUpper(), searchCriteria))
                                {
                                    ResultCollection.Add(item);
                                }
                            }
                        }
                    }
                    break;
                case AllSearchableObjects.Devices:
                    ResultCollection = new ObservableCollection<TECObject>();
                    foreach (TECDevice item in Templates.Catalogs.Devices)
                    {
                        if (UtilitiesMethods.StringContainsStrings(item.Name.ToUpper(), searchCriteria) ||
                            UtilitiesMethods.StringContainsStrings(item.Description.ToUpper(), searchCriteria))
                        {
                            ResultCollection.Add(item);
                        }
                        else
                        {
                            foreach (TECLabeled tag in item.Tags)
                            {
                                if (UtilitiesMethods.StringContainsStrings(tag.Label.ToUpper(), searchCriteria))
                                {
                                    ResultCollection.Add(item);
                                }
                            }
                        }
                    }
                    break;
                case AllSearchableObjects.Controllers:
                    ResultCollection = new ObservableCollection<TECObject>();
                    foreach (TECController item in Templates.ControllerTemplates)
                    {
                        if (UtilitiesMethods.StringContainsStrings(item.Name.ToUpper(), searchCriteria) ||
                            UtilitiesMethods.StringContainsStrings(item.Description.ToUpper(), searchCriteria))
                        {
                            ResultCollection.Add(item);
                        }
                        else
                        {
                            foreach (TECLabeled tag in item.Tags)
                            {
                                if (UtilitiesMethods.StringContainsStrings(tag.Label.ToUpper(), searchCriteria))
                                {
                                    ResultCollection.Add(item);
                                }
                            }
                        }
                    }
                    break;
                case AllSearchableObjects.AssociatedCosts:
                    ResultCollection = new ObservableCollection<TECObject>();
                    foreach (TECCost item in Templates.Catalogs.AssociatedCosts)
                    {
                        if (UtilitiesMethods.StringContainsStrings(item.Name.ToUpper(), searchCriteria) ||
                            UtilitiesMethods.StringContainsStrings(item.Description.ToUpper(), searchCriteria))
                        {
                            ResultCollection.Add(item);
                        }
                        else
                        {
                            foreach (TECLabeled tag in item.Tags)
                            {
                                if (UtilitiesMethods.StringContainsStrings(tag.Label.ToUpper(), searchCriteria))
                                {
                                    ResultCollection.Add(item);
                                }
                            }
                        }
                    }
                    break;
                case AllSearchableObjects.Panels:
                    ResultCollection = new ObservableCollection<TECObject>();
                    foreach (TECPanel item in Templates.PanelTemplates)
                    {
                        if (UtilitiesMethods.StringContainsStrings(item.Name.ToUpper(), searchCriteria) ||
                            UtilitiesMethods.StringContainsStrings(item.Description.ToUpper(), searchCriteria))
                        {
                            ResultCollection.Add(item);
                        }
                        else
                        {
                            foreach (TECLabeled tag in item.Tags)
                            {
                                if (UtilitiesMethods.StringContainsStrings(tag.Label.ToUpper(), searchCriteria))
                                {
                                    ResultCollection.Add(item);
                                }
                            }
                        }
                    }
                    break;
                case AllSearchableObjects.MiscCosts:
                    ResultCollection = new ObservableCollection<TECObject>();
                    foreach (TECMisc item in Templates.MiscCostTemplates)
                    {
                        if (item.Type == CostType.TEC)
                        {
                            if (UtilitiesMethods.StringContainsStrings(item.Name.ToUpper(), searchCriteria) ||
                            UtilitiesMethods.StringContainsStrings(item.Description.ToUpper(), searchCriteria))
                            {
                                ResultCollection.Add(item);
                            }
                            else
                            {
                                foreach (TECLabeled tag in item.Tags)
                                {
                                    if (UtilitiesMethods.StringContainsStrings(tag.Label.ToUpper(), searchCriteria))
                                    {
                                        ResultCollection.Add(item);
                                    }
                                }
                            }
                        }
                    }
                    break;
                case AllSearchableObjects.MiscWiring:
                    ResultCollection = new ObservableCollection<TECObject>();
                    foreach (TECMisc item in Templates.MiscCostTemplates)
                    {
                        if(item.Type == CostType.Electrical)
                        {
                            if (UtilitiesMethods.StringContainsStrings(item.Name.ToUpper(), searchCriteria) ||
                                                            UtilitiesMethods.StringContainsStrings(item.Description.ToUpper(), searchCriteria))
                            {
                                ResultCollection.Add(item);
                            }
                            else
                            {
                                foreach (TECLabeled tag in item.Tags)
                                {
                                    if (UtilitiesMethods.StringContainsStrings(tag.Label.ToUpper(), searchCriteria))
                                    {
                                        ResultCollection.Add(item);
                                    }
                                }
                            }
                        }
                    }
                    break;
                default:
                    break;

            }
        }
        private bool SearchCanExecute()
        {
            return true;
        }
        private void EndSearchExecute()
        {
            populateItemsCollections();
            SearchString = "";
        }
        
        #endregion
        

        public void populateItemsCollections()
        {
            ResultCollection = new ObservableCollection<TECObject>();
            switch (ChosenType)
            {
                case AllSearchableObjects.System:
                    foreach (TECSystem sys in Templates.SystemTemplates)
                    {
                        ResultCollection.Add(sys);
                    }
                    break;
                case AllSearchableObjects.Equipment:
                    foreach (TECEquipment equip in Templates.EquipmentTemplates)
                    {
                        ResultCollection.Add(equip);
                    }
                    break;
                case AllSearchableObjects.SubScope:
                    foreach (TECSubScope ss in Templates.SubScopeTemplates)
                    {
                        ResultCollection.Add(ss);
                    }
                    break;
                case AllSearchableObjects.Devices:
                    foreach (TECDevice dev in Templates.Catalogs.Devices)
                    {
                        ResultCollection.Add(dev);
                    }
                    break;
                case AllSearchableObjects.Controllers:
                    foreach (TECController control in Templates.ControllerTemplates)
                    {
                        ResultCollection.Add(control);
                    }
                    break;
                case AllSearchableObjects.Panels:
                    foreach (TECPanel panel in Templates.PanelTemplates)
                    {
                        ResultCollection.Add(panel);
                    }
                    break;
                case AllSearchableObjects.MiscCosts:
                    foreach (TECMisc cost in Templates.MiscCostTemplates)
                    {
                        if (cost.Type == CostType.TEC)
                        {
                            ResultCollection.Add(cost);
                        }
                    }
                    break;
                case AllSearchableObjects.MiscWiring:
                    foreach (TECMisc cost in Templates.MiscCostTemplates)
                    {
                        if (cost.Type == CostType.Electrical)
                        {
                            ResultCollection.Add(cost);
                        }
                    }
                    break;
                case AllSearchableObjects.Wires:
                    foreach(TECElectricalMaterial wire in Templates.Catalogs.ConnectionTypes)
                    {
                        ResultCollection.Add(wire);
                    }
                    break;
                case AllSearchableObjects.Conduits:
                    foreach (TECElectricalMaterial conduit in Templates.Catalogs.ConduitTypes)
                    {
                        ResultCollection.Add(conduit);
                    }
                    break;
                default:
                    break;
            }
            
            
            
        }

        private void unsubscribeTemplatesCollections()
        {
            //if (Templates != null)
            //{
            //    Templates.SystemTemplates.CollectionChanged -= SystemTemplates_CollectionChanged;
            //    Templates.EquipmentTemplates.CollectionChanged -= EquipmentTemplates_CollectionChanged;
            //    Templates.SubScopeTemplates.CollectionChanged -= SubScopeTemplates_CollectionChanged;
            //    Templates.Catalogs.Devices.CollectionChanged -= Devices_CollectionChanged;
            //    Templates.ControllerTemplates.CollectionChanged -= ControllerTemplates_CollectionChanged;
            //    Templates.Catalogs.AssociatedCosts.CollectionChanged -= AssociatedCosts_CollectionChanged;
            //    Templates.PanelTemplates.CollectionChanged -= PanelTemplates_CollectionChanged;
            //    Templates.MiscCostTemplates.CollectionChanged -= MiscCostTemplates_CollectionChanged;
            //}
        }

        private void subscribeTemplatesCollections()
        {
            //Templates.SystemTemplates.CollectionChanged += SystemTemplates_CollectionChanged;
            //Templates.EquipmentTemplates.CollectionChanged += EquipmentTemplates_CollectionChanged;
            //Templates.SubScopeTemplates.CollectionChanged += SubScopeTemplates_CollectionChanged;
            //Templates.Catalogs.Devices.CollectionChanged += Devices_CollectionChanged;
            //Templates.ControllerTemplates.CollectionChanged += ControllerTemplates_CollectionChanged;
            //Templates.Catalogs.AssociatedCosts.CollectionChanged += AssociatedCosts_CollectionChanged;
            //Templates.PanelTemplates.CollectionChanged += PanelTemplates_CollectionChanged;
            //Templates.MiscCostTemplates.CollectionChanged += MiscCostTemplates_CollectionChanged;
        }

        public void DragOver(IDropInfo dropInfo)
        {
            DragHandler(dropInfo);
        }
        public void Drop(IDropInfo dropInfo)
        {
            DropHandler(dropInfo);
        }

        #endregion

       
    }
}
