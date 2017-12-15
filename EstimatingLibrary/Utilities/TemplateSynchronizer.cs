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
        readonly private Action<T, T> sync;
        readonly private Dictionary<T, List<T>> dictionary;
        bool isSyncing = false;

        /// <summary>
        /// A manager for keeping properties of templates in sync with respective instances
        /// </summary>
        /// <param name="copy">Creates a new object with the properties of the argument</param>
        /// <param name="sync">Writes the properties of the first argument to the second argument</param>
        public TemplateSynchronizer(Func<T, T> copy, Action<T, T> sync, TECTemplates templates)
        {
            this.copy = copy;
            this.sync = sync;
            dictionary = new Dictionary<T, List<T>>();
            ChangeWatcher watcher = new ChangeWatcher(templates);
            watcher.Changed += handleTemplatesChanged;
        }

        private void handleTemplatesChanged(TECChangedEventArgs obj)
        {
            if(obj.Value is T item && !(obj.Sender is TECTemplates) && obj.Change == Change.Remove)
            {
                removeItem(item);
            }
        }

        public event Action<TECChangedEventArgs> TECChanged;

        public void NewGroup(T template)
        {
            ChangeWatcher watcher = new ChangeWatcher(template);
            watcher.Changed += (args)=> handleTChanged(template, template);
            dictionary.Add(template, new List<T>());
        }
        public void RemoveGroup(T template)
        {
            foreach(T item in dictionary[template])
            {
                notifyTECChanged(Change.Remove, template, item);
            }
            dictionary.Remove(template);
        }
        public T NewItem(T template)
        {
            T newItem = copy(template);
            ChangeWatcher watcher = new ChangeWatcher(newItem);
            watcher.Changed += (args) => handleTChanged(template, newItem);
            if (!dictionary.ContainsKey(template))
            {
                NewGroup(template);
            }
            dictionary[template].Add(newItem);
            notifyTECChanged(Change.Add, template, newItem);
            return newItem;
        }
        public void LinkExisting(T template, T item)
        {
            ChangeWatcher watcher = new ChangeWatcher(item);
            watcher.Changed += (args) => handleTChanged(template, item);
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

        private void removeItem(T item)
        {
            foreach(T key in dictionary.Keys)
            {
                if (dictionary[key].Contains(item))
                {
                    dictionary[key].Remove(item);
                    notifyTECChanged(Change.Remove, key, item);
                }
            }
        }
        private void handleTChanged(T template, T changed)
        {
            if (!isSyncing)
            {
                isSyncing = true;
                foreach (T item in dictionary[template].Where(obj => obj != changed))
                {
                    sync(changed, item);
                }
                if(changed != template)
                {
                    sync(changed, template);
                }
                isSyncing = false;
            }
            
        }
        private void notifyTECChanged(Change change, T template, T item)
        {
            TECChanged?.Invoke(new TECChangedEventArgs(change, "TemplateRelationship", template, item, null));
        }
    }
}
