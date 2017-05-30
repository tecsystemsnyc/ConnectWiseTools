using EstimatingLibrary;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TECUserControlLibrary.ViewModels 
{
    public class ConstantsVM : ViewModelBase
    {
        public TECLabor Labor { get; private set; }


        public ConstantsVM(TECLabor labor)
        {
            this.Labor = labor;
        }

        public void Refresh(TECLabor labor)
        {
            this.Labor = labor;
            RaisePropertyChanged("Labor");
        }
    }
}
