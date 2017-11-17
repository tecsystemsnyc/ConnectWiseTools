using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TECUserControlLibrary.Utilities
{
    public class QuantityCollection<T> : ObservableCollection<QuantityItem<T>>
    {
        private Dictionary<T, QuantityItem<T>> itemDictionary;

        public QuantityCollection()
        {
            itemDictionary = new Dictionary<T, QuantityItem<T>>();
        }
        public QuantityCollection(IEnumerable<T> items) : base()
        {
            foreach (T item in items)
            {
                Add(item);
            }
        }

        public void Add(T item, int quantity = 1)
        {
            if (quantity < 0)
            {
                throw new ArgumentOutOfRangeException("Quantity cannot be less than 0");
            }
            else if (quantity == 0)
            {
                return;
            }

            if (itemDictionary.ContainsKey(item))
            {
                itemDictionary[item].Quantity += quantity;
            }
            else
            {
                QuantityItem<T> quantItem = new QuantityItem<T>(item, quantity);
                itemDictionary.Add(item, quantItem);
                base.Add(quantItem);
            }
        }
        public bool Remove(T item, int quantity = 1)
        {
            if (quantity < 0)
            {
                throw new ArgumentOutOfRangeException("Quantity cannot be less than 0");
            }
            else if (quantity == 0)
            {
                return true;
            }

            if (itemDictionary.ContainsKey(item))
            {
                QuantityItem<T> quantItem = itemDictionary[item];
                quantItem.Quantity -= quantity;
                if (quantItem.Quantity < 1)
                {
                    itemDictionary.Remove(item);
                    base.Remove(quantItem);
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool RemoveAll(T item)
        {
            if (itemDictionary.ContainsKey(item))
            {
                QuantityItem<T> quantityItem = itemDictionary[item];
                itemDictionary.Remove(item);
                base.Remove(quantityItem);
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public class QuantityItem<T> : INotifyPropertyChanged
    {
        private int _quantity;

        public T Item { get; }
        public int Quantity
        {
            get
            {
                return _quantity;
            }
            set
            {
                _quantity = value;
                raisePropertyChanged("Quantity");
            }
        }

        public QuantityItem(T item, int quantity = 1)
        {
            Item = item;
            _quantity = quantity;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void raisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}