using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary.Interfaces
{
    public interface ISaveable
    {
        SaveableMap SaveObjects { get; }
        SaveableMap RelatedObjects { get; }
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
            foreach(Tuple<string, TECObject> item in map.ChildList())
            {
                if (!PropertyNames.Contains(item.Item1))
                {
                    PropertyNames.Add(item.Item1);
                }
                addToDictionary(item.Item1, item.Item2);
                Objects.Add(item.Item2);
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
