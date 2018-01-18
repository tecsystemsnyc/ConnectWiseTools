using System;
using System.Collections.Generic;

namespace EstimatingLibrary.Interfaces
{
    public interface INetworkConnectable : ITECObject
    {
        Guid Guid { get; }

        List<TECIO> AllNetworkIOList { get; }
        IOCollection AvailableNetworkIO { get; }
        TECNetworkConnection ParentConnection { get; set; }
        bool IsNetwork { get; }

        INetworkConnectable Copy(INetworkConnectable item, bool isTypical, Dictionary<Guid, Guid> guidDictionary);

        bool CanConnectToNetwork(TECNetworkConnection netConnect);
    }
}
