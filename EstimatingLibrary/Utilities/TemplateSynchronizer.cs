using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EstimatingLibrary.Interfaces;

namespace EstimatingLibrary.Utilities
{
    public class TemplateSynchronizer<T> : INotifyTECChanged where T : TECObject
    {
        readonly private Func<T, T> copy;
        readonly private Action<T> remove;
        readonly private Action<TemplateSynchronizer<T>, T, T, TECChangedEventArgs> sync;
        readonly private Dictionary<T, List<T>> dictionary;
        readonly private Dictionary<(T item, bool isKey), Action> unsubscribeDictionary;
        private List<T> currentlySyncing = new List<T>();

        /// <summary>
        /// A manager for keeping properties of templates in sync with respective instances
        /// </summary>
        /// <param name="copy">Creates a new object with the properties of the argument</param>
        /// <param name="remove">Invoked upon removal of an item</param>
        /// <param name="sync">Writes the properties of the first argument to the second argument</param>
        public TemplateSynchronizer(Func<T, T> copy, Action<T> remove,
            Action<TemplateSynchronizer<T>, T, T, TECChangedEventArgs> sync, TECTemplates templates)
        {
            this.copy = copy;
            this.sync = sync;
            this.remove = remove;
            dictionary = new Dictionary<T, List<T>>();
            unsubscribeDictionary = new Dictionary<(T, bool), Action>();
            ChangeWatcher watcher = new ChangeWatcher(templates);
            watcher.Changed += handleTemplatesChanged;
        }

        private void handleTemplatesChanged(TECChangedEventArgs obj)
        {
            if(obj.Value is T item && obj.Change == Change.Remove)
            {
                if(obj.Sender is TECTemplates && dictionary.ContainsKey(item))
                {
                    RemoveGroup(item);
                } else
                {
                    RemoveItem(item);
                }
            }
        }

        public event Action<TECChangedEventArgs> TECChanged;

        public void NewGroup(T template)
        {
            if (!dictionary.ContainsKey(template))
            {
                subscribe(template, template, true);
                dictionary.Add(template, new List<T>());
            }
        }
        public void RemoveGroup(T template)
        {
            List<T> group = new List<T>(dictionary[template]);
            foreach (T item in group)
            {
                RemoveItem(template, item);
            }
            unsubscribeDictionary[(template, true)].Invoke();
            dictionary.Remove(template);
            remove.Invoke(template);
            unsubscribeDictionary.Remove((template, true));

        }
        public T NewItem(T template)
        {
            T newItem = copy(template);
            subscribe(template, newItem, false);
            if (!dictionary.ContainsKey(template))
            {
                NewGroup(template);
            }
            dictionary[template].Add(newItem);
            notifyTECChanged(Change.Add, template, newItem);
            return newItem;
        }
        public void RemoveItem(T item)
        {
            foreach (T key in dictionary.Keys)
            {
                if (dictionary[key].Contains(item))
                {
                    RemoveItem(key, item);
                }
            }
        }
        public void RemoveItem(T template, T item)
        {
            dictionary[template].Remove(item);
            notifyTECChanged(Change.Remove, template, item);
            unsubscribeDictionary[(item, false)].Invoke();
            remove.Invoke(item);
            unsubscribeDictionary.Remove((item, false));
        }
        public void LinkExisting(T template, T item)
        {
            subscribe(template, item, false);
            if (!dictionary.ContainsKey(template))
            {
                NewGroup(template);
            }
            dictionary[template].Add(item);
        }
        public void LinkExisting(T template, IEnumerable<T> items)
        {
            foreach(T item in items)
            {
                LinkExisting(template, item);
            }
        }
        public void LinkNew(T template, T item)
        {
            notifyTECChanged(Change.Add, template, item);
            LinkExisting(template, item);
        }

        public bool Contains(T item)
        {
            foreach(KeyValuePair<T, List<T>> entry in dictionary)
            {
                if(entry.Key == item || entry.Value.Contains(item))
                {
                    return true;
                }
            }
            return false;
        }
        public Dictionary<T, List<T>> GetFullDictionary()
        {
            return dictionary;
        }
        public T GetTemplate(T item)
        {
            if (dictionary.ContainsKey(item))
            {
                return item;
            }
            else
            {
                foreach(var pair in dictionary)
                {
                    if (pair.Value.Contains(item))
                    {
                        return pair.Key;
                    }
                }
            }
            return null;
        }
        public T GetParent(T item)
        {
            foreach (var pair in dictionary)
            {
                if (pair.Value.Contains(item))
                {
                    return pair.Key;
                }
            }
            return null;
        }
        
        private void handleTChanged(T template, T changed, TECChangedEventArgs args)
        {
            if (!currentlySyncing.Contains(template))
            {
                currentlySyncing.Add(template);
                sync(this, template, changed, args);
                currentlySyncing.Remove(template);
            }
            
        }
        private void notifyTECChanged(Change change, T template, T item)
        {
            TECChanged?.Invoke(new TECChangedEventArgs(change, "TemplateRelationship", template, item, null));
        }
        private void subscribe(T template, T item, bool isKey)
        {
            ChangeWatcher watcher = new ChangeWatcher(item);
            watcher.Changed += handleChange;
            unsubscribeDictionary.Add((item, isKey),
                () => { watcher.Changed -= handleChange; });

            void handleChange(TECChangedEventArgs args)
            {
                handleTChanged(template, item, args);
            }
        }
    }
}
