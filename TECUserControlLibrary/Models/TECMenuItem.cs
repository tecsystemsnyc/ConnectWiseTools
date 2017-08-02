using EstimatingLibrary;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

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
        public SolidColorBrush TextBrush { get; set; }
        public ICommand Command { get; set; }
        public ObservableCollection<TECMenuItem> Items { get; set; }

        public TECMenuItem(string name, SolidColorBrush textBrush) : base(Guid.NewGuid())
        {
            Name = name;
            TextBrush = textBrush;
            Items = new ObservableCollection<TECMenuItem>();
        }
    }
}
