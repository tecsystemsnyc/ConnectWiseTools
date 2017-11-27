using EstimatingLibrary;
using System;

namespace TECUserControlLibrary.Interfaces
{
    public interface ISubScopeConnectionItem
    {
        bool NeedsUpdate { get; set; }
        TECSubScope SubScope { get; }

        /// <summary>
        /// A property changed that may want to get propagated to instances.
        /// </summary>
        event Action<ISubScopeConnectionItem> PropagationPropertyChanged;
    }
}