using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class ObservableItemToInstanceList<T> : TECObject
    {
        private Dictionary<T, List<T>> charactersticInstances;

        public ObservableItemToInstanceList()
        {
            charactersticInstances = new Dictionary<T, List<T>>();
        }

        public void AddItem(T key, T value)
        {
            if (!charactersticInstances.ContainsKey(key))
            {
                charactersticInstances[key] = new List<T>();
            }
            charactersticInstances[key].Add(value);
            NotifyPropertyChanged("Add", key, value);
        }

        public void RemoveItem(T key, T value)
        {
            charactersticInstances[key].Remove(value);
            NotifyPropertyChanged("Remove", key, value);
        }

        public List<T> GetInstances(T key)
        {
            return charactersticInstances[key];
        }

        public bool ContainsKey(T key)
        {
            if(key == null)
            {
                return false;
            }
            else
            {
                return charactersticInstances.ContainsKey(key);
            }
            
        }

        public override object Copy()
        {
            throw new NotImplementedException();
        }
    }
}
