using EstimatingLibrary;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using TECUserControlLibrary.Interfaces;

namespace TECUserControlLibrary.ViewModels
{
    public class UpdateConnectionVM : ViewModelBase
    {
        #region Fields
        private TECTypical typical;

        private readonly List<SubScopeUpdatedWrapper> _subScope;
        
        #region SelectedFields
        private SubScopeUpdatedWrapper _selectedInstance;

        private TECSubScope _selectedTypical;

        private TECController instanceController;
        private TECController typicalController;
        private TECController equivalentInstanceController;

        private bool instanceCanUpdate;
        #endregion
        #endregion

        #region Properties
        public List<SubScopeUpdatedWrapper> SubScope
        {
            get { return _subScope; }
        }
        
        public SubScopeUpdatedWrapper SelectedInstance
        {
            get { return _selectedInstance; }
            set
            {
                _selectedInstance = value;
                RaisePropertyChanged("SelectedInstance");
                handleNewSelectedInstance(value);
            }
        }
        public TECSubScope SelectedTypical
        {
            get { return _selectedTypical; }
            set
            {
                _selectedTypical = value;
                RaisePropertyChanged("SelectedTypical");
            }
        }

        public ICommand UpdateCommand { get; private set; }
        public ICommand DoneCommand { get; private set; }

        public event Action UpdatesDone;
        #endregion

        public UpdateConnectionVM(IEnumerable<ISubScopeConnectionItem> subScope, TECTypical typical)
        {
            this.typical = typical;
            List<SubScopeUpdatedWrapper> instances = new List<SubScopeUpdatedWrapper>();
            foreach(ISubScopeConnectionItem typ in subScope)
            {
                List<TECSubScope> ssInstances = typical.TypicalInstanceDictionary.GetInstancesOfType(typ.SubScope);
                foreach(TECSubScope instance in ssInstances)
                {
                    SubScopeUpdatedWrapper wrapped = new SubScopeUpdatedWrapper(instance);
                    instances.Add(wrapped);
                }
            }
            _subScope = instances;

            UpdateCommand = new RelayCommand(updateExecute, canUpdate);
            DoneCommand = new RelayCommand(doneExecute);
        }

        private void handleNewSelectedInstance(SubScopeUpdatedWrapper subScope)
        {
            //Set equivalent typical
            SelectedTypical = typical.TypicalInstanceDictionary.GetTypical(subScope.SubScope);
            //Set controller of SelectedInstance
            instanceController = null;
            if (SelectedInstance.SubScope.Connection != null)
            {
                instanceController = SelectedInstance.SubScope.Connection.ParentController;
            }
            //Set controller of SelectedTypical
            typicalController = null;
            if (SelectedTypical.Connection != null)
            {
                typicalController = SelectedTypical.Connection.ParentController;
            }
            //Set equivalent instance controller for the typical controller of this instance.
            if (typical.TypicalInstanceDictionary.GetTypical(instanceController) == typicalController)
            {
                equivalentInstanceController = instanceController;
            }
            else
            {
                TECController newInstanceController = null;
                foreach (TECSystem instance in typical.Instances)
                {
                    if (instance.GetAllSubScope().Contains(SelectedInstance.SubScope))
                    {
                        foreach (TECController controller in instance.Controllers)
                        {
                            if (typical.TypicalInstanceDictionary.GetTypical(controller) == typicalController)
                            {
                                newInstanceController = controller;
                                break;
                            }
                        }
                    }
                    if (newInstanceController != null) { break; }
                }
                equivalentInstanceController = newInstanceController;
            }
            //Set instanceCanUpdate bool
            if (SelectedInstance == null)
            {
                instanceCanUpdate = false;
            }
            else
            {
                if (SelectedInstance.Updated)
                {
                    instanceCanUpdate = false;
                }
                else
                {
                    if (instanceController == equivalentInstanceController)
                    {
                        bool propertiesEqual = true;
                        if (SelectedInstance.SubScope.Connection != null)
                        {
                            TECConnection instanceConnection = SelectedInstance.SubScope.Connection;
                            TECConnection typicalConnection = SelectedTypical.Connection;
                            if ((instanceConnection.Length != typicalConnection.Length) ||
                                (instanceConnection.ConduitLength != typicalConnection.ConduitLength) ||
                                (instanceConnection.ConduitType != typicalConnection.ConduitType))
                            {
                                propertiesEqual = false;
                            }
                        }
                        else
                        {
                            propertiesEqual = false;
                        }
                        instanceCanUpdate = !propertiesEqual;
                    }
                    else
                    {
                        if (equivalentInstanceController == null)
                        {
                            instanceCanUpdate = true;
                        }
                        else
                        {
                            instanceCanUpdate = equivalentInstanceController.CanConnectSubScope(SelectedInstance.SubScope);
                        }
                    }
                }
            }
        }

        private void updateExecute()
        {
            bool sameController = (instanceController == equivalentInstanceController);

            if (sameController)
            {
                updateProperties(SelectedTypical, SelectedInstance.SubScope);
            }
            else
            {
                if (instanceController != null)
                {
                    instanceController.RemoveSubScope(SelectedInstance.SubScope);
                }

                if (equivalentInstanceController != null)
                {
                    equivalentInstanceController.AddSubScope(SelectedInstance.SubScope);
                    updateProperties(SelectedTypical, SelectedInstance.SubScope);
                }
            }
            

            SelectedInstance.Updated = true;
            instanceCanUpdate = false;

            void updateProperties(TECSubScope typical, TECSubScope toUpdate)
            {
                toUpdate.Connection.Length = typical.Connection.Length;
                toUpdate.Connection.ConduitLength = typical.Connection.ConduitLength;
                toUpdate.Connection.ConduitType = typical.Connection.ConduitType;
            }
        }
        private bool canUpdate()
        {
            return instanceCanUpdate;
        }

        private void doneExecute()
        {
            UpdatesDone?.Invoke();
        }
    }

    public class SubScopeUpdatedWrapper : INotifyPropertyChanged
    {
        private readonly TECSubScope _subScope;
        private bool _updated;

        public TECSubScope SubScope { get { return _subScope; } }
        public bool Updated
        {
            get { return _updated; }
            set
            {
                _updated = value;
                raisePropertyChanged("Updated");
            }
        }

        public SubScopeUpdatedWrapper(TECSubScope subScope)
        {
            _subScope = subScope;
            _updated = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void raisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
