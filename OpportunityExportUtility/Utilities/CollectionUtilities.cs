﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectWiseInformationInterface.Utilities
{
    public static class CollectionUtilities
    {
        public static void ObservablyClear<T>(this ObservableCollection<T> collection)
        {
            List<T> toRemove = new List<T>();
            toRemove.AddRange(collection);
            foreach (T item in toRemove)
            {
                collection.Remove(item);
            }
        }
    }
}
