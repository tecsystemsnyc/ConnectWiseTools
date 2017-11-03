using EstimatingLibrary;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace TECUserControlLibrary.Models
{
    public class TECMenuItem : INotifyPropertyChanged
    {
        private readonly string _name;
        private readonly bool _isMainMenu;
        private readonly ObservableCollection<TECMenuItem> _items;
        private ICommand _command;

        public string Name
        {
            get { return _name; }
        }
        public bool IsMainMenu
        {
            get { return _isMainMenu; }
        }
        public ReadOnlyObservableCollection<TECMenuItem> Items
        {
            get
            {
                return new ReadOnlyObservableCollection<TECMenuItem>(_items);
            }
        }
        public ICommand Command
        {
            get { return _command; }
            set
            {
                _command = value;
                raisePropertyChanged("Command");
            }
        }

        public TECMenuItem(string name, bool isMainMenu)
        {
            _name = name;
            _isMainMenu = isMainMenu;
            _items = new ObservableCollection<TECMenuItem>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void AddMenuItem(TECMenuItem item)
        {
            _items.Add(item);
        }

        private void raisePropertyChanged(string propertyName)
        {
            PropertyChangedEventArgs e = new PropertyChangedEventArgs(propertyName);
            PropertyChanged?.Invoke(this, e);
        }
    }
}
