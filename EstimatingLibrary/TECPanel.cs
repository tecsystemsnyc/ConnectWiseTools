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
                NotifyPropertyChanged("ChildChanged", (object)this, (object)value);
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
                Controllers.CollectionChanged -= collectionChanged;
                NotifyPropertyChanged("Controllers", temp, this);
                Controllers.CollectionChanged += collectionChanged;
            }
        }


        public double MaterialCost
        {
            get { return getMaterialCost(); }
        }
        #endregion

        public TECPanel(Guid guid) : base(guid)
        {
            _guid = guid;
            _controllers = new ObservableCollection<TECController>();
            Controllers.CollectionChanged += collectionChanged;
        }
        public TECPanel() : this(Guid.NewGuid()) { }
        public TECPanel(TECPanel panel, Dictionary<Guid, Guid> guidDictionary = null) : this()
        {
            if (guidDictionary != null)
            { guidDictionary[_guid] = panel.Guid; }
            copyPropertiesFromScope(panel);
            foreach(TECController controller in panel.Controllers)
            {
                _controllers.Add(new TECController(controller, guidDictionary));
            }
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
            var outPanel = new TECPanel(this);
            return outPanel;

        }
        
        private void collectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach(object item in e.NewItems)
                {
                    NotifyPropertyChanged("AddRelationship", this, item);
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    NotifyPropertyChanged("RemoveRelationship", this, item);
                }
            }
        }


        private double getMaterialCost()
        {
            double matCost = 0;

            if(Type != null)
            {
                matCost += Type.Cost;
            }
            
            foreach (TECAssociatedCost cost in this.AssociatedCosts)
            {
                matCost += cost.Cost;
            }

            return matCost;
        }
    }
}
