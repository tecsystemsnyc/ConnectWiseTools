﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary.Interfaces
{
    public interface INetworkConnectable
    {
        Guid Guid { get; }

        List<TECIO> AllNetworkIOList { get; }
        IOCollection AvailableNetworkIO { get; }
        TECNetworkConnection ParentConnection { get; set; }

        INetworkConnectable Copy(INetworkConnectable item, bool isTypical, Dictionary<Guid, Guid> guidDictionary);

        bool CanConnectToNetwork(TECNetworkConnection netConnect);
    }
}
