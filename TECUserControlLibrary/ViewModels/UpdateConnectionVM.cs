using EstimatingLibrary;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TECUserControlLibrary.Models;

namespace TECUserControlLibrary.ViewModels
{
    public class UpdateConnectionVM : ViewModelBase
    {
        #region Fields
        private Dictionary<TECSubScope, TypicalSubScope> typicalDictionary;
        private Dictionary<TECSubScope, bool> updatedDictionary;

        private readonly List<TECSubScope> _subScope;
        private TECSubScope _selectedInstance;
        private TypicalSubScope _selectedTypical;
        #endregion

        #region Properties
        public List<TECSubScope> SubScope
        {
            get { return _subScope; }
        }
        public TECSubScope SelectedInstance
        {
            get { return _selectedInstance; }
            set
            {
                _selectedInstance = value;
                RaisePropertyChanged("SelectedInstance");
                handleNewSelectedInstance(value);
            }
        }
        public TypicalSubScope SelectedTypical
        {
            get { return _selectedTypical; }
            set
            {
                _selectedTypical = value;
                RaisePropertyChanged("SelectedTypical");
            }
        }

        public ICommand UpdateCommand;
        #endregion

        public UpdateConnectionVM(IEnumerable<TypicalSubScope> subScope)
        {
            List<TECSubScope> instances = new List<TECSubScope>();
            foreach(TypicalSubScope typ in subScope)
            {
                foreach(TECSubScope instance in typ.Instances)
                {
                    instances.Add(instance);
                    typicalDictionary.Add(instance, typ);
                    updatedDictionary.Add(instance, false);
                }
            }
            _subScope = instances;

            UpdateCommand = new RelayCommand(updateExecute, canUpdate);
        }

        private void handleNewSelectedInstance(TECSubScope subScope)
        {
            SelectedTypical = typicalDictionary[subScope];
        }

        private void updateExecute()
        {
            SelectedTypical.UpdateInstance(SelectedInstance);
            updatedDictionary[SelectedInstance] = true;
        }
        private bool canUpdate()
        {
            return !updatedDictionary[SelectedInstance];
        }
    }
}
