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
        readonly private Action<T, T, TECChangedEventArgs> sync;
        readonly private Dictionary<T, List<T>> dictionary;
        readonly private Dictionary<(T item, bool isKey), Action> unsubscribeDictionary;
        List<T> currentlySyncing = new List<T>();

        /// <summary>
        /// A manager for keeping properties of templates in sync with respective instances
        /// </summary>
        /// <param name="copy">Creates a new object with the properties of the argument</param>
        /// <param name="sync">Writes the properties of the first argument to the second argument</param>
        public TemplateSynchronizer(Func<T, T> copy, Action<T, T, TECChangedEventArgs> sync, TECTemplates templates)
        {
            this.copy = copy;
            this.sync = sync;
            dictionary = new Dictionary<T, List<T>>();
            unsubscribeDictionary = new Dictionary<(T, bool), Action>();
            ChangeWatcher watcher = new ChangeWatcher(templates);
            watcher.Changed += handleTemplatesChanged;
        }

        private void handleTemplatesChanged(TECChangedEventArgs obj)
        {
            if(obj.Value is T item && !(obj.Sender is TECTemplates)
                && !(obj.Sender is T) && obj.Change == Change.Remove)
            {
                RemoveItem(item);
            }
        }

        public event Action<TECChangedEventArgs> TECChanged;

        public void NewGroup(T template)
        {
            if (!dictionary.ContainsKey(template))
            {
                ChangeWatcher watcher = new ChangeWatcher(template);
                watcher.Changed += handleChange;
                unsubscribeDictionary.Add((template, true),
                    () => { watcher.Changed -= handleChange; });
                dictionary.Add(template, new List<T>());
            }

            void handleChange(TECChangedEventArgs args)
            {
                handleTChanged(template, template, args);
            }

        }
        public void RemoveGroup(T template)
        {
            foreach(T item in dictionary[template])
            {
                notifyTECChanged(Change.Remove, template, item);
                unsubscribeDictionary[(item, false)].Invoke();
            }
            unsubscribeDictionary[(template, true)].Invoke();
            dictionary.Remove(template);
        }
        public T NewItem(T template)
        {
            T newItem = copy(template);
            ChangeWatcher watcher = new ChangeWatcher(newItem);
            watcher.Changed += handleChange;
            unsubscribeDictionary.Add((newItem, false),
                () => { watcher.Changed -= handleChange; });
            if (!dictionary.ContainsKey(template))
            {
                NewGroup(template);
            }
            dictionary[template].Add(newItem);
            notifyTECChanged(Change.Add, template, newItem);
            return newItem;

            void handleChange(TECChangedEventArgs args)
            {
                handleTChanged(template, newItem, args);
            }
        }
        public void RemoveItem(T item)
        {
            foreach (T key in dictionary.Keys)
            {
                if (dictionary[key].Contains(item))
                {
                    dictionary[key].Remove(item);
                    notifyTECChanged(Change.Remove, key, item);
                    unsubscribeDictionary[(item, false)].Invoke();
                }
            }
        }

        public Dictionary<T, List<T>> GetFullDictionary()
        {
            return dictionary;
        }

        public void LinkExisting(T template, T item)
        {
            ChangeWatcher watcher = new ChangeWatcher(item);
            watcher.Changed += (args) => handleTChanged(template, item, args);
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

        
        private void handleTChanged(T template, T changed, TECChangedEventArgs args)
        {
            if (!currentlySyncing.Contains(template))
            {
                currentlySyncing.Add(template);
                sync(template, changed, args);
                currentlySyncing.Remove(template);
            }
            
        }
        private void notifyTECChanged(Change change, T template, T item)
        {
            TECChanged?.Invoke(new TECChangedEventArgs(change, "TemplateRelationship", template, item, null));
        }

        internal T GetTemplate(T item)
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
    }
}
