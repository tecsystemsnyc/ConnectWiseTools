using System.Collections.Generic;

namespace EstimatingLibrary.Interfaces
{
    public interface INetworkParentable
    {
        bool IsServer { get; }
        string Name { get; }
        
        IEnumerable<TECNetworkConnection> ChildNetworkConnections { get; }

        bool CanAddNetworkConnection(IOType ioType);
        TECNetworkConnection AddNetworkConnection(bool isTypical, IEnumerable<TECElectricalMaterial> connectionTypes, IOType ioType);
    }
}
