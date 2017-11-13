using EstimatingLibrary;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TECUserControlLibrary.Models
{
    public class IOModulePropertiesItem : ViewModelBase
    {
        private IOType _selectedIO;

        public TECIOModule IOModule { get; }
        public IOType SelectedIO
        {
            get { return _selectedIO; }
            set
            {
                _selectedIO =value;
                RaisePropertyChanged("SelectedIO");
            }
        }
        public ICommand AddIOCommand { get; private set; }
        public IOModulePropertiesItem(TECIOModule module)
        {
            IOModule = module;
            AddIOCommand = new RelayCommand(addIOExecute, canAddIO);
        }

        private void addIOExecute()
        {
            bool added = false;

            foreach (TECIO io in IOModule.IO)
            {
                if(io.Type == SelectedIO)
                {
                    io.Quantity++;
                    added = true;
                    break;
                }
            }
            if (!added)
            {
                IOModule.IO.Add(new TECIO(SelectedIO));
            }
        }

        private bool canAddIO()
        {
            return true;
        }
    }


}
