using EstimatingLibrary;
using GalaSoft.MvvmLight;
using System;
using System.Windows.Input;

namespace TECUserControlLibrary.ViewModels.AddVMs
{
    public abstract class AddVM : ViewModelBase
    {
        public ICommand AddCommand { get; protected set; }
        public PropertiesVM PropertiesVM { get; }

        public AddVM(TECScopeManager scopeManager)
        {
            PropertiesVM = new PropertiesVM(scopeManager.Catalogs, scopeManager);
        }

        public Action<object> Added { get; protected set; }

    }
}
