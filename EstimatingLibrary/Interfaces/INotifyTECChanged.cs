using System;

namespace EstimatingLibrary.Interfaces
{
    public interface INotifyTECChanged
    {
        event Action<TECChangedEventArgs> TECChanged;
    }
}
