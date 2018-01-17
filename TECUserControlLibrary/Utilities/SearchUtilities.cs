using EstimatingLibrary;
using EstimatingLibrary.Interfaces;
using EstimatingUtilitiesLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TECUserControlLibrary.Utilities
{
    public static class SearchUtilities
    {
        public static List<T> GetSearchResult<T>(this IEnumerable<T> source, string searchString)
            where T : ITECObject
        {
            bool isOr = false;
            if (searchString.Length > 0 && searchString[0] == '*')
            {
                isOr = true;
                searchString = searchString.Remove(0, 1);
            }
            char[] dilemeters = { ',', ' ' };
            string[] searchCriteria = searchString.ToUpper().Split(dilemeters, StringSplitOptions.RemoveEmptyEntries);

            var outCollection = new List<T>();
            foreach (T item in source)
            {
                if (item is TECScope scope)
                {
                    string[] references = { scope.Name.ToUpper(), scope.Description.ToUpper() };
                    foreach (TECTag tag in scope.Tags)
                    {
                        references.Append(tag.Label);
                    }
                    if (scope is TECHardware hardware)
                    {
                        references.Append(hardware.Manufacturer.Label.ToUpper());
                    }
                    if (isOr)
                    {
                        if (UtilitiesMethods.StringsContainsAnyStrings(references, searchCriteria))
                        {
                            outCollection.Add(item);
                        }
                    }
                    else
                    {
                        if (UtilitiesMethods.StringsContainStrings(references, searchCriteria))
                        {
                            outCollection.Add(item);
                        }
                    }
                }
                else if (item is TECLabeled labeled)
                {
                    string[] references = { labeled.Label.ToUpper() };
                    if (isOr)
                    {
                        if (UtilitiesMethods.StringsContainsAnyStrings(references, searchCriteria))
                        {
                            outCollection.Add(item);
                        }
                    }
                    else
                    {
                        if (UtilitiesMethods.StringsContainStrings(references, searchCriteria))
                        {
                            outCollection.Add(item);
                        }
                    }
                }
            }
            return outCollection;
        }
    }
}
