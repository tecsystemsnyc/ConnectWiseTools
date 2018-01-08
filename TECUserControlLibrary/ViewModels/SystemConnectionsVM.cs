using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using TECUserControlLibrary.Interfaces;
using TECUserControlLibrary.Models;
using TECUserControlLibrary.Utilities;

namespace TECUserControlLibrary.ViewModels
{
    public class SystemConnectionsVM : ViewModelBase, IDropTarget
    {
        #region Fields
        private TECSystem system;
        private ChangeWatcher watcher;
        private TECElectricalMaterial noneConduitType;

        private ObservableCollection<TECController> _controllers;
        private ObservableCollection<ISubScopeConnectionItem> _connectedSubScope;
        private ObservableCollection<TECSubScope> _unconnectedSubScope;
        private TECController _selectedController;
        private TECSubScope _selectedUnconnectedSubScope;
        private ISubScopeConnectionItem _selectedConnection;
        private TECEquipment _selectedEquipment;

        private UpdateConnectionVM _updateConnectionVM;

        private string _cannotConnectMessage;
        #endregion

        #region Properties
        public IUserConfirmable ConfirmationObject { get; set; }

        public ObservableCollection<TECElectricalMaterial> ConduitTypes { get; }

        public ObservableCollection<TECController> Controllers
        {
            get
            {
                return _controllers;
            }
            set
            {
                _controllers = value;
                RaisePropertyChanged("Controllers");
            }
        }
        public ObservableCollection<TECEquipment> Equipment { get; }
        public ObservableCollection<ISubScopeConnectionItem> ConnectedSubScope
        {
            get { return _connectedSubScope; }
            set
            {
                _connectedSubScope = value;
                RaisePropertyChanged("ConnectedSubScope");
            }
        }

        public ObservableCollection<TECSubScope> UnconnectedSubScope
        {
            get
            {
                return _unconnectedSubScope;
            }
            set
            {
                _unconnectedSubScope = value;
                RaisePropertyChanged("UnconnectedSubScope");
            }
        }
        public TECController SelectedController
        {
            get
            {
                return _selectedController;
            }
            set
            {
                if (!anItemNeedsUpdate())
                {
                    setController();
                }
                else
                {
                    string checkUpdateString = "Some connections haven't been updated. Would you like to update these connections?";
                    MessageBoxResult result = ConfirmationObject.Show(checkUpdateString, "Update?", MessageBoxButton.YesNoCancel, MessageBoxImage.Exclamation);
                    if (result == MessageBoxResult.Yes)
                    {
                        updateWhatNeedsUpdate();
                    }
                    else if (result == MessageBoxResult.No)
                    {
                        setController();
                    }
                    else
                    {
                        RaisePropertyChanged("SelectedController");
                    }
                }

                void setController()
                {
                    _selectedController = value;
                    RaisePropertyChanged("SelectedController");
                    handleControllerSelected(value);
                    Selected?.Invoke(value);
                }
            }
        }
        public TECSubScope SelectedUnconnectedSubScope
        {
            get { return _selectedUnconnectedSubScope; }
            set
            {
                _selectedUnconnectedSubScope = value;
                RaisePropertyChanged("SelectedUnconnectedSubScope");
                Selected?.Invoke(value);
            }
        }
        public ISubScopeConnectionItem SelectedConnection
        {
            get { return _selectedConnection; }
            set
            {
                _selectedConnection = value;
                RaisePropertyChanged("SelectedConnection");
                Selected?.Invoke(value?.SubScope?.Connection as TECObject);
            }
        }
        public TECEquipment SelectedEquipment
        {
            get { return _selectedEquipment; }
            set
            {
                _selectedEquipment = value;
                RaisePropertyChanged("SelectedEquipment");
                handleEquipmentSelected();
            }
        }

        public UpdateConnectionVM UpdateConnectionVM
        {
            get { return _updateConnectionVM; }
            private set
            {
                _updateConnectionVM = value;
                RaisePropertyChanged("UpdateConnectionVM");
                UpdateVM?.Invoke(value);
            }
        }

        public ICommand UpdateAllCommand { get; private set; }
        public ICommand UpdateItemCommand { get; private set; }
        public bool CanLeave
        {
            get { return !anItemNeedsUpdate(); }
        }

        public string CannotConnectMessage
        {
            get { return _cannotConnectMessage; }
            set
            {
                _cannotConnectMessage = value;
                RaisePropertyChanged("CannotConnectMessage");
            }
        }
        #endregion

        public SystemConnectionsVM(TECSystem system, IEnumerable<TECElectricalMaterial> conduitTypes)
        {
            //Construct conduit types collection
            noneConduitType = new TECElectricalMaterial();
            noneConduitType.Name = "None";
            this.ConduitTypes = new ObservableCollection<TECElectricalMaterial>(conduitTypes);
            this.ConduitTypes.Insert(0, noneConduitType);

            this.system = system;
            watcher = new ChangeWatcher(system);
            if(system is TECTypical)
            {
                watcher.TypicalConstituentChanged += handleSystemChanged;
            }
            else
            {
                watcher.InstanceConstituentChanged += handleSystemChanged;
            }
            this.ConfirmationObject = new MessageBoxService();
            initializeCollections();
            foreach (TECSubScope ss in system.GetAllSubScope())
            {
                if (ss.Connection == null && !ss.IsNetwork && SelectedEquipment != null && SelectedEquipment.SubScope.Contains(ss))
                {
                    UnconnectedSubScope.Add(ss);
                }
            }
            foreach (TECController controller in system.Controllers)
            {
                Controllers.Add(controller);
            }
            Equipment = new ObservableCollection<TECEquipment>(system.Equipment);
            UpdateAllCommand = new RelayCommand(updateAllExecute, updateAllCanExecute);
            UpdateItemCommand = new RelayCommand<ISubScopeConnectionItem>(updateItem, canUpdateItem);
        }
        
        public event Action<UpdateConnectionVM> UpdateVM;
        public event Action<TECObject> Selected;

        public void DragOver(IDropInfo dropInfo)
        {
            CannotConnectMessage = "";
            TECSubScope subScope = dropInfo.Data as TECSubScope;
            ISubScopeConnectionItem ssConnectItem = dropInfo.Data as ISubScopeConnectionItem;

            if (subScope != null)
            {
                if (SelectedController == null)
                {
                    CannotConnectMessage = "No Controller to connect to.";
                }
                else if (!SelectedController.CanConnectSubScope(subScope))
                {
                    CannotConnectMessage = "Available Controller IO incompatible with SubScope.";
                }
                else if (UIHelpers.TargetCollectionIsType(dropInfo, typeof(ISubScopeConnectionItem)))
                {
                    dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                    dropInfo.Effects = DragDropEffects.Copy;
                }
            }
            else if (ssConnectItem != null && UIHelpers.TargetCollectionIsType(dropInfo, typeof(TECSubScope)))
            {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                dropInfo.Effects = DragDropEffects.Copy;
            }
        }

        public void Drop(IDropInfo dropInfo)
        {
            TECSubScope subScope = dropInfo.Data as TECSubScope;
            ISubScopeConnectionItem ssConnectItem = dropInfo.Data as ISubScopeConnectionItem;

            if (subScope != null)
            {
                SelectedController.AddSubScope(subScope);
                UnconnectedSubScope.Remove(subScope);
                if (system is TECTypical typ)
                {
                    bool hasInstances = (typ.Instances.Count > 0);
                    addNewConnectedSubScope(subScope, needsUpdate: hasInstances);
                }
                else
                {
                    addNewConnectedSubScope(subScope);
                }
            }
            else if (ssConnectItem != null)
            {
                if (SelectedController != null)
                {
                    if (dropInfo.TargetCollection == UnconnectedSubScope)
                    {
                        SelectedController.RemoveSubScope(ssConnectItem.SubScope);
                        if (system is TECTypical typ && typ.Instances.Count > 0)
                        {
                            updateItem(ssConnectItem);
                        }
                        ConnectedSubScope.Remove(ssConnectItem);
                        if (SelectedEquipment != null && SelectedEquipment.SubScope.Contains(ssConnectItem.SubScope)) {
                            UnconnectedSubScope.Add(ssConnectItem.SubScope);
                        }
                    }
                }
                else
                {
                    throw new DataMisalignedException("Selected Controller is null but SubScopeConnectionItems exist in the collection.");
                }
            }
        }

        private void initializeCollections()
        {
            _controllers = new ObservableCollection<TECController>();
            _connectedSubScope = new ObservableCollection<ISubScopeConnectionItem>();
            _unconnectedSubScope = new ObservableCollection<TECSubScope>();
        }

        private void updateAllExecute()
        {
            updateSubScope(ConnectedSubScope);
        }
        private bool updateAllCanExecute()
        {
            if (system is TECTypical typ && SelectedController != null)
            {
                return (typ.Instances.Count > 0 && SelectedController.ChildrenConnections.Count > 0);
            }
            else
            {
                return false;
            }
        }
        private void updateWhatNeedsUpdate()
        {
            List<ISubScopeConnectionItem> ssNeedsUpdate = new List<ISubScopeConnectionItem>();
            foreach (ISubScopeConnectionItem item in ConnectedSubScope)
            {
                if (item.NeedsUpdate)
                {
                    ssNeedsUpdate.Add(item);
                }
            }
            updateSubScope(ssNeedsUpdate);
        }
        private void updateItem(ISubScopeConnectionItem item)
        {
            updateSubScope(new List<ISubScopeConnectionItem>() { item });
        }
        private bool canUpdateItem(ISubScopeConnectionItem item)
        {
            return item.NeedsUpdate;
        }
        private void updateSubScope(IEnumerable<ISubScopeConnectionItem> subScope)
        {
            if (system is TECTypical typical)
            {
                foreach(ISubScopeConnectionItem item in subScope)
                {
                    item.NeedsUpdate = false;
                }
                UpdateConnectionVM = new UpdateConnectionVM(subScope, typical);
                UpdateConnectionVM.UpdatesDone += handleUpdatesDone;
            }
            else
            {
                throw new InvalidOperationException("Can not update when system is not typical.");
            }
        }

        private bool anItemNeedsUpdate()
        {
            foreach (ISubScopeConnectionItem item in ConnectedSubScope)
            {
                if (item.NeedsUpdate)
                {
                    return true;
                }
            }
            return false;
        }
        private void handleControllerSelected(TECController controller)
        {
            ConnectedSubScope = new ObservableCollection<ISubScopeConnectionItem>();
            if(controller != null)
            {
                foreach (TECConnection connection in controller.ChildrenConnections)
                {
                    if (connection is TECSubScopeConnection ssConnect)
                    {
                        addNewConnectedSubScope(ssConnect.SubScope);
                    }
                }
            }
            else
            {
                UpdateConnectionVM = null;
            }
            RaisePropertyChanged("CanLeave");
        }
        private void handleEquipmentSelected()
        {
            UnconnectedSubScope.Clear();
            if(SelectedEquipment == null)
            {
                return;
            }
            foreach(TECSubScope ss in 
                SelectedEquipment.SubScope.Where(ss => ss.Connection == null && !ss.IsNetwork))
            {
                UnconnectedSubScope.Add(ss);
            }
            
        }

        private void handleSystemChanged(Change change, TECObject obj)
        {
            if (obj is TECController controller)
            {
                if (change == Change.Add)
                {
                    Controllers.Add(controller);
                }
                else if (change == Change.Remove)
                {
                    Controllers.Remove(controller);
                }
            }
            else if (obj is TECEquipment equip)
            {
                if (change == Change.Add)
                {
                    Equipment.Add(equip);
                }
                else if (change == Change.Remove)
                {
                    Equipment.Remove(equip);
                }
            }
            else if (obj is TECSubScope subScope)
            {
                if (change == Change.Add)
                {
                    if (subScope.Connection == null && !subScope.IsNetwork && 
                        SelectedEquipment != null && SelectedEquipment.SubScope.Contains(subScope))
                    {
                        UnconnectedSubScope.Add(subScope);
                    }
                }
                else if (change == Change.Remove)
                {
                    UnconnectedSubScope.Remove(subScope);
                    ISubScopeConnectionItem ssItem = null;
                    foreach(ISubScopeConnectionItem item in ConnectedSubScope)
                    {
                        if (item.SubScope == subScope && !item.SubScope.IsNetwork)
                        {
                            ssItem = item;
                            break;
                        }
                    }
                    if (ssItem != null)
                    {
                        ConnectedSubScope.Remove(ssItem);
                    }
                }
            }
        }

        private void addNewConnectedSubScope(TECSubScope ss, bool needsUpdate = false)
        {
            SubScopeConnectionItem ssConnectItem = new SubScopeConnectionItem(ss, noneConduitType, ss.FindParentEquipment(system), needsUpdate);
            ssConnectItem.PropagationPropertyChanged += handlePropagationPropertyChanged;
            ConnectedSubScope.Add(ssConnectItem);
        }

        private void handlePropagationPropertyChanged(ISubScopeConnectionItem connected)
        {
            if (system is TECTypical typ)
            {
                if (typ.Instances.Count > 0)
                {
                    connected.NeedsUpdate = true;
                    RaisePropertyChanged("CanLeave");
                }
            }
        }

        private void handleUpdatesDone()
        {
            RaisePropertyChanged("CanLeave");
        }
    }
}
