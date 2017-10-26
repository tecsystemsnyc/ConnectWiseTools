using EstimatingLibrary;
using EstimatingLibrary.Interfaces;
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
                    ResultCollection = getResultCollection(Templates.SystemTemplates, searchCriteria);
                    break;
                case AllSearchableObjects.Equipment:
                    ResultCollection = getResultCollection(Templates.EquipmentTemplates, searchCriteria);
                    break;
                case AllSearchableObjects.SubScope:
                    ResultCollection = getResultCollection(Templates.SubScopeTemplates, searchCriteria);
                    break;
                case AllSearchableObjects.Devices:
                    ResultCollection = getResultCollection(Templates.Catalogs.Devices, searchCriteria);
                    break;
                case AllSearchableObjects.Controllers:
                    ResultCollection = getResultCollection(Templates.ControllerTemplates, searchCriteria);
                    break;
                case AllSearchableObjects.AssociatedCosts:
                    ResultCollection = getResultCollection(Templates.Catalogs.AssociatedCosts, searchCriteria);
                    break;
                case AllSearchableObjects.Panels:
                    ResultCollection = getResultCollection(Templates.PanelTemplates, searchCriteria);
                    break;
                case AllSearchableObjects.MiscCosts:
                    ResultCollection = getResultCollection(Templates.MiscCostTemplates.Where(x => x.Type == CostType.TEC), searchCriteria);
                    break;
                case AllSearchableObjects.MiscWiring:
                    ResultCollection = getResultCollection(Templates.MiscCostTemplates.Where(x => x.Type == CostType.Electrical), searchCriteria);
                    break;
                case AllSearchableObjects.ControllerTypes:
                    ResultCollection = getResultCollection(Templates.Catalogs.ControllerTypes, searchCriteria);
                    break;
                case AllSearchableObjects.PanelTypes:
                    ResultCollection = getResultCollection(Templates.Catalogs.PanelTypes, searchCriteria);
                    break;
                case AllSearchableObjects.Tags:
                    ResultCollection = getResultCollection(Templates.Catalogs.Tags, searchCriteria);
                    break;
                case AllSearchableObjects.Wires:
                    ResultCollection = getResultCollection(Templates.Catalogs.ConnectionTypes, searchCriteria);
                    break;
                case AllSearchableObjects.Conduits:
                    ResultCollection = getResultCollection(Templates.Catalogs.ConduitTypes, searchCriteria);
                    break;
                case AllSearchableObjects.Valves:
                    ResultCollection = getResultCollection(Templates.Catalogs.Valves, searchCriteria);
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


        private ObservableCollection<TECObject> getResultCollection(IEnumerable<TECObject> source, string[] searchCriteria)
        {
            var outCollection = new ObservableCollection<TECObject>();
            foreach (TECObject item in source)
            {
                if(item is TECScope scope)
                {
                    if (UtilitiesMethods.StringContainsStrings(scope.Name.ToUpper(), searchCriteria) ||
                                        UtilitiesMethods.StringContainsStrings(scope.Description.ToUpper(), searchCriteria))
                    {
                        outCollection.Add(item);
                    }
                    else if(scope is TECHardware hardware)
                    {
                        if(UtilitiesMethods.StringContainsStrings(hardware.Manufacturer.Label.ToUpper(), searchCriteria))
                        {
                            outCollection.Add(item);
                        }
                    }
                    else
                    {
                        foreach (TECLabeled tag in scope.Tags)
                        {
                            if (UtilitiesMethods.StringContainsStrings(tag.Label.ToUpper(), searchCriteria))
                            {
                                outCollection.Add(item);
                            }
                        }
                    }
                    
                }
                else if (item is TECLabeled labeled)
                {
                    if (UtilitiesMethods.StringContainsStrings(labeled.Label.ToUpper(), searchCriteria))
                    {
                        outCollection.Add(item);
                    }
                }
            }
            return outCollection;
        }
        #endregion

       
    }
}
