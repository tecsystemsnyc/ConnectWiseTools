using EstimatingLibrary;
using GalaSoft.MvvmLight;
using System;
using System.Windows.Input;

namespace TECUserControlLibrary.ViewModels.AddVMs
{
    public abstract class AddVM : ViewModelBase
    {
        protected bool _asReference = false;

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
            PropertiesVM = new PropertiesVM(scopeManager.Catalogs, scopeManager);
        }

        public Action<object> Added { get; protected set; }

    }
}
