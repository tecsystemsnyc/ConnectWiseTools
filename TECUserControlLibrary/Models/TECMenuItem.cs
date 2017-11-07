using GalaSoft.MvvmLight.CommandWpf;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace TECUserControlLibrary.Models
{
    public class TECMenuItem : INotifyPropertyChanged
    {
        private readonly ObservableCollection<TECMenuItem> _items;
        private RelayCommand _command;

        public string Name
        {
            get;
        }
        public bool IsMainMenu
        {
            get;
        }
        public ReadOnlyObservableCollection<TECMenuItem> Items
        {
            get
            {
                return new ReadOnlyObservableCollection<TECMenuItem>(_items);
            }
        }
        public RelayCommand Command
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
            Name = name;
            IsMainMenu = isMainMenu;
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
