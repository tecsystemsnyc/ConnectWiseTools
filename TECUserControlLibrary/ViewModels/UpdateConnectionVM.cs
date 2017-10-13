using EstimatingLibrary;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECUserControlLibrary.Models;

namespace TECUserControlLibrary.ViewModels
{
    public class UpdateConnectionVM : ViewModelBase
    {
        #region Fields
        private TECSubScopeConnection _subScopeConnection;
        #endregion

        public UpdateConnectionVM(IEnumerable<TypicalSubScope> subScope)
        {
        }
    }
}
