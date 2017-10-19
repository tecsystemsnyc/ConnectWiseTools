using EstimatingLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary.Utilities
{
    public class ObservableListDictionary<T>
    {
        #region Fields
        private Dictionary<T, List<T>> dictionary;
        #endregion

        //Constructor
        public ObservableListDictionary()
        {
            dictionary = new Dictionary<T, List<T>>();
        }

        #region Events
        public event Action<Tuple<Change, T, T>> CollectionChanged;
        #endregion

        #region Methods
        public void AddItem(T key, T value)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary[key] = new List<T>();
            }
            dictionary[key].Add(value);
            CollectionChanged?.Invoke(new Tuple<Change, T, T>(Change.Add, key, value));
        }
        public void RemoveItem(T key, T value)
        {
            dictionary[key].Remove(value);
            CollectionChanged?.Invoke(new Tuple<Change, T, T>(Change.Remove, key, value));
        }

        public Y GetTypical<Y>(Y instance) where Y : class
        {
            if (instance is T tInstance)
            {
                foreach (KeyValuePair<T, List<T>> pair in dictionary)
                {
                    if (pair.Value.Contains(tInstance))
                    {
                        if (pair.Key is Y yKey)
                        {
                            return yKey;
                        }
                        else
                        {
                            throw new TypeLoadException("Key type doesn't match value type.");
                        }
                    }
                }
                return null;
            }
            else
            {
                return null;
            }
        }

        public List<T> GetInstances(T key)
        {
            return dictionary[key];
        }
        public List<Y> GetInstancesOfType<Y>(Y key)
        {
            List<Y> outList = new List<Y>();
            if (key is T tKey)
            {
                var subList = dictionary[tKey];
                foreach(T item in subList)
                {
                    if(item is Y yItem)
                    {
                        outList.Add(yItem);
                    }
                }
                return outList;
            }
            else
            {
                return null;
            }
        }

        public bool ContainsKey(T key)
        {
            if (key == null)
            {
                return false;
            }
            else
            {
                return dictionary.ContainsKey(key);
            }

        }
        public bool ContainsValue(T value)
        {
            foreach (KeyValuePair<T, List<T>> item in dictionary)
            {
                if (value.GetType() == item.Key.GetType())
                {
                    if (item.Value.Contains(value)) return true;
                }
            }
            return false;
        }

        public Dictionary<T, List<T>> GetFullDictionary()
        {
            return dictionary;
        }

        public int Count
        {
            get { return dictionary.Count; }
        }
        #endregion
    }
}
