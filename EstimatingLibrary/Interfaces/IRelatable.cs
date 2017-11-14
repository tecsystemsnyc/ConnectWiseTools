using System;
using System.Collections.Generic;

namespace EstimatingLibrary.Interfaces
{
    public interface IRelatable
    {
        SaveableMap PropertyObjects { get; }
        SaveableMap LinkedObjects { get; }
    }

    public class SaveableMap
    {
        public List<TECObject> Objects;
        public List<string> PropertyNames;
        Dictionary<string, List<TECObject>> nameDictionary;

        public SaveableMap()
        {
            Objects = new List<TECObject>();
            PropertyNames = new List<string>();
            nameDictionary = new Dictionary<string, List<TECObject>>();
        }

        public bool Contains(TECObject item)
        {
            return Objects.Contains(item);
        }
        public bool Contains(string name)
        {
            return PropertyNames.Contains(name);
        }
        public object Property(object parent, string propertyName)
        {
            return parent.GetType().GetProperty(propertyName).GetValue(parent);
        }
        public void Add(TECObject item, string name)
        {
            Objects.Add(item);
            this.Add(name);
            addToDictionary(name, item);
        }
        public void Add(string name)
        {
            PropertyNames.Add(name);
        }
        public void AddRange(IEnumerable<TECObject> items, string name)
        {
            Objects.AddRange(items);
            this.Add(name);

            foreach(TECObject item in items)
            {
                addToDictionary(name, item);
            }
        }
        public void AddRange(IEnumerable<string> names)
        {
            PropertyNames.AddRange(names);
        }
        public void AddRange(SaveableMap map)
        {
            foreach(KeyValuePair<string, List<TECObject>> pair in map.nameDictionary)
            {
                if (nameDictionary.ContainsKey(pair.Key))
                {
                    nameDictionary[pair.Key].AddRange(pair.Value);
                    Objects.AddRange(pair.Value);
                }
                else
                {
                    nameDictionary[pair.Key] = pair.Value;
                    Objects.AddRange(pair.Value);
                    PropertyNames.Add(pair.Key);
                }
            }
            foreach(string name in map.PropertyNames)
            {
                if (!PropertyNames.Contains(name))
                {
                    PropertyNames.Add(name);
                }
            }
        }
        public List<Tuple<string, TECObject>> ChildList()
        {
            List<Tuple<string, TECObject>> outList = new List<Tuple<string, TECObject>>();
            foreach(KeyValuePair<string, List<TECObject>> pair in nameDictionary)
            {
                foreach(TECObject item in pair.Value)
                {
                    outList.Add(new Tuple<string, TECObject>(pair.Key, item));
                }
            }
            return outList;
        }

        private void addToDictionary(string name, TECObject item)
        {
            if (!nameDictionary.ContainsKey(name))
            {
                nameDictionary[name] = new List<TECObject>() { item };
            }
            else
            {
                nameDictionary[name].Add(item);
            }
        }
        
    }
    
}
