using EstimatingLibrary;
using GalaSoft.MvvmLight;
using System;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;

namespace TECUserControlLibrary.ViewModels.AddVMs
{
    public abstract class AddVM : ViewModelBase
    {
        protected bool _asReference = false;
        protected TECTemplates templates;

        public ICommand AddCommand { get; protected set; }
        public PropertiesVM PropertiesVM { get; }
        public bool AsReference
        {
            get { return _asReference; }
            set
            {
                _asReference = value;
                RaisePropertyChanged("AsReference");
            }
        }
        public bool IsTemplates
        {
            get;
        }

        public AddVM(TECScopeManager scopeManager)
        {
            IsTemplates = scopeManager is TECTemplates;
            templates = templates as TECTemplates;
            PropertiesVM = new PropertiesVM(scopeManager.Catalogs, scopeManager);
        }

        public Action<object> Added { get; protected set; }

    }
}
