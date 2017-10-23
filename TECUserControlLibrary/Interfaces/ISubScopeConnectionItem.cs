using System;
using EstimatingLibrary;

namespace TECUserControlLibrary.Interfaces
{
    public interface ISubScopeConnectionItem
    {
        bool NeedsUpdate { get; }
        TECSubScope SubScope { get; }

        event Action NeedsUpdateChanged;
    }
}