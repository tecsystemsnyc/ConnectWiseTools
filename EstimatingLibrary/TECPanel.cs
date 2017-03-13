using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECPanel : TECScope
    {
        #region Properties
        private TECPanelType _type;
        public TECPanelType Type
        {
            get { return _type; }
            set
            {
                var temp = this.Copy();
                _type = value;
                NotifyPropertyChanged("Type", temp, this);
            }
        }

        private ObservableCollection<TECController> _controllers;
        public ObservableCollection<TECController> Controllers
        {
            get { return _controllers; }
            set
            {
                var temp = this.Copy();
                _controllers = value;
                NotifyPropertyChanged("Controllers", temp, this);
                Controllers.CollectionChanged += collectionChanged;
            }
        }

        #endregion

        public TECPanel(Guid guid) : base(guid)
        {
            _guid = guid;

            _controllers = new ObservableCollection<TECController>();
        }
        public TECPanel() : this(Guid.NewGuid()) { }
        public TECPanel(TECPanel panel) : this()
        {
            copyPropertiesFromScope(panel);
            _type = panel.Type;
        }

        public override object Copy()
        {
            var outPanel = new TECPanel(this);
            outPanel._guid = _guid;
            return outPanel;
        }

        public override object DragDropCopy()
        {
            throw new NotImplementedException();
        }


        private void collectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach(object item in e.NewItems)
                {
                    NotifyPropertyChanged("Add", this, item);
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    NotifyPropertyChanged("Remove", this, item);
                }
            }
        }

    }
}
