using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary.Utilities
{
    public static class ScopeExtensions
    {
        public static void CopyPropertiesFromScope(this TECScope scope, TECScope otherScope)
        {
            scope.Name = otherScope.Name;
            scope.Description = otherScope.Description;
            foreach (TECLabeled tag in otherScope.Tags)
            {
                scope.Tags.Add(tag);
            }
            foreach (TECCost cost in otherScope.AssociatedCosts)
            {
                scope.AssociatedCosts.Add(cost);
            }
        }
    }
}
