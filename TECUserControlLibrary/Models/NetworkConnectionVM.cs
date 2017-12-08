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
        private readonly TECElectricalMaterial noneConduit;

        public TECNetworkConnection Connection { get; }
        public List<TECElectricalMaterial> ConduitTypes { get; }

        public TECElectricalMaterial SelectedConduitType
        {
            get
            {
                if (Connection.ConduitType == null)
                {
                    return noneConduit;
                }
                else
                {
                    return Connection.ConduitType;
                }
            }
            set
            {
                if (value == noneConduit)
                {
                    Connection.ConduitType = null;
                }
                else if (value != null)
                {
                    Connection.ConduitType = value;
                }
            }
        }

        public NetworkConnectionVM(TECNetworkConnection connection, IEnumerable<TECElectricalMaterial> conduitTypes)
        {
            noneConduit = new TECElectricalMaterial();
            noneConduit.Name = "None";

            Connection = connection;
            Connection.PropertyChanged += handleConnectionChanged;
            ConduitTypes = conduitTypes != null ? new List<TECElectricalMaterial>(conduitTypes) : new List<TECElectricalMaterial>();
            ConduitTypes.Insert(0, noneConduit);
        }

        private void handleConnectionChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ConduitType")
            {
                RaisePropertyChanged("SelectedConduitType");
            }
        }
    }
}
