using System;
using EstimatingLibrary;

namespace TECUserControlLibrary.Interfaces
{
    public interface ISubScopeConnectionItem
    {
        bool NeedsUpdate { get; set; }
        TECSubScope SubScope { get; }

        event Action NeedsUpdateChanged;
    }
}