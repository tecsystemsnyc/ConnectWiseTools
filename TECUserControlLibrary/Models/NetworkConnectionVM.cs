using EstimatingLibrary;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TECUserControlLibrary.Models
{
    public class NetworkConnectionVM : ViewModelBase
    {
        public TECNetworkConnection Connection { get; }
        public List<TECElectricalMaterial> ConduitTypes { get; }

        public NetworkConnectionVM(TECNetworkConnection connection, IEnumerable<TECElectricalMaterial> conduitTypes)
        {
            Connection = connection;
            ConduitTypes = new List<TECElectricalMaterial>(conduitTypes);
        }
    }
}
