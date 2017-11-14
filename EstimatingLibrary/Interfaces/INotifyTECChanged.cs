using System;

namespace EstimatingLibrary.Interfaces
{
    public interface InotifyTECChanged
    {
        event Action<TECChangedEventArgs> TECChanged;
    }
}
