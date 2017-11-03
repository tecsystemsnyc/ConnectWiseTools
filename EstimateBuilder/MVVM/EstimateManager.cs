using EstimatingLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECUserControlLibrary.BaseVMs;

namespace EstimateBuilder.MVVM
{
    public class EstimateManager : AppManager
    {
        private TECBid bid;
        private TECTemplates templates;

        public EstimateManager() : base(new EstimateMainVM())
        {

        }
    }
}
