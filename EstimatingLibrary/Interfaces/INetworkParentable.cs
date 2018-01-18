using System.Collections.Generic;

namespace EstimatingLibrary.Interfaces
{
    public interface INetworkParentable : INetworkConnectable
    {
        bool IsServer { get; }
        bool IsTypical { get; }
        string Name { get; }
        
        IEnumerable<TECNetworkConnection> ChildNetworkConnections { get; }

        bool CanAddNetworkConnection(IOType ioType);
        TECNetworkConnection AddNetworkConnection(bool isTypical, IEnumerable<TECConnectionType> connectionTypes, IOType ioType);
        void RemoveNetworkConnection(TECNetworkConnection connection);
        
    }
}
