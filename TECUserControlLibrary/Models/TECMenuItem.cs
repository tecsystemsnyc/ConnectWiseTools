using EstimatingLibrary;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TECUserControlLibrary.Models
{
    public class TECMenuItem : TECObject
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged("Name");
            }
        }
        public ICommand Command { get; set; }
        public ObservableCollection<TECMenuItem> Items { get; set; }

        public TECMenuItem(string name)
        {
            Name = name;
            Items = new ObservableCollection<TECMenuItem>();
        }

        public override object Copy()
        {
            throw new NotImplementedException();
        }
    }
}
