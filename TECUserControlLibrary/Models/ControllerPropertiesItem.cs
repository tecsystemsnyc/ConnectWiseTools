using EstimatingLibrary;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace TECUserControlLibrary.Models
{
    public class ControllerPropertiesItem : ViewModelBase
    {
        private List<TECIO> _io;
        private List<TECIO> _availableIO;
        private ObservableCollection<ModuleGroup> _modules;

        public TECController Controller
        {
            get; private set;
        }

        public List<TECIO> IO
        {
            get { return _io; }
            set
            {
                _io = value;
                RaisePropertyChanged("IO");
            }
        }
        public List<TECIO> AvailableIO
        {
            get { return _availableIO; }
            set
            {
                _availableIO = value;
                RaisePropertyChanged("AvailableIO");
            }
        }
        public ObservableCollection<ModuleGroup> Modules
        {
            get { return _modules; }
            set
            {
                _modules = value;
                RaisePropertyChanged("Modules");
            }
        }
        
        public RelayCommand<TECIOModule> AddModuleCommand { get; private set; }
        public RelayCommand<TECIOModule> RemoveModuleCommand { get; private set; }

        public ControllerPropertiesItem(TECController controller)
        {
            Controller = controller;
            AddModuleCommand = new RelayCommand<TECIOModule>(addModuleExecute, canAddModule);
            RemoveModuleCommand = new RelayCommand<TECIOModule>(removeModuleExecute, canRemoveModule);
            populateIO();
            populateModules();

        }

        private void addModuleExecute(TECIOModule obj)
        {
            Controller.IOModules.Add(obj);
            populateModules();
            populateIO();
        }

        private bool canAddModule(TECIOModule arg)
        {
            return Controller.Type.IOModules.Where(item => item == arg).Count() >
                Controller.IOModules.Where(item => item == arg).Count();
        }

        private void removeModuleExecute(TECIOModule obj)
        {
            Controller.IOModules.Remove(obj);
            populateModules();
            populateIO();
        }

        private bool canRemoveModule(TECIOModule arg)
        {
            if(arg == null)
            {
                return false;
            }
            else
            {
                bool hasModule = Controller.IOModules.Contains(arg);
                bool canSpare = true;
                foreach (TECIO io in arg.IO)
                {
                    if (!Controller.AvailableIO.Contains(io))
                    {
                        canSpare = false;
                        break;
                    }
                }
                return hasModule && canSpare;
            }
            
        }

        private void populateIO()
        {
            IO = Controller.TotalIO.ListIO();
            AvailableIO = Controller.AvailableIO.ListIO();
        }
        private void populateModules()
        {
            Modules = new ObservableCollection<ModuleGroup>();
            foreach(TECIOModule module in Controller.Type.IOModules.Distinct())
            {
                Modules.Add(new ModuleGroup(module,
                    Controller.IOModules.Where(item => item == module).Count()));
            }
        }
    }

    public class ModuleGroup
    {
        public TECIOModule Module
        {
            get; private set;
        }
        public int Quantity
        {
            get; private set;
        }
        public ModuleGroup(TECIOModule module, int qty)
        {
            Module = module;
            Quantity = qty;
        }
    }
}
