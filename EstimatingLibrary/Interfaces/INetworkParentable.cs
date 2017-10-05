using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary.Interfaces
{
    public interface INetworkParentable
    {
        bool IsServer { get; }
        string Name { get; }
        
        IEnumerable<TECNetworkConnection> ChildNetworkConnections { get; }
    }
}
