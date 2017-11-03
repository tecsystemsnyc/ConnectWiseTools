using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimateBuilder.MVVM
{
    public class EstimateMainVM : TECUserControlLibrary.BaseVMs.MainViewModel
    {

        public EstimateMainVM() : base(new EstimateSplashVM(), new EstimateMenuVM(), new EstimateEditorVM())
        {

        }
    }
}
