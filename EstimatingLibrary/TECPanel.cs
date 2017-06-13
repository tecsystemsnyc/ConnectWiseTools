using EstimatingLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECPanel : TECScope, CostComponent
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

        public List<TECCost> Costs
        {
            get
            {
                return getCosts();
            }
        }
        private List<TECCost> getCosts()
        {
            var outCosts = new List<TECCost>();
            outCosts.Add(Type);
            foreach(TECCost cost in AssociatedCosts)
            {
                outCosts.Add(cost);
            }
            return outCosts;
        }
        #endregion

        public TECPanel(Guid guid, TECPanelType type) : base(guid)
        {
            _guid = guid;
            _controllers = new ObservableCollection<TECController>();
            _type = type;
            Controllers.CollectionChanged += collectionChanged;
        }
        public TECPanel(TECPanelType type) : this(Guid.NewGuid(), type) { }
        public TECPanel(TECPanel panel, Dictionary<Guid, Guid> guidDictionary = null) : this(panel.Type)
        {
            if (guidDictionary != null)
            { guidDictionary[_guid] = panel.Guid; }
            copyPropertiesFromScope(panel);
            foreach (TECController controller in panel.Controllers)
            {
                _controllers.Add(new TECController(controller, guidDictionary));
            }
        }

        public override object Copy()
        {
            var outPanel = new TECPanel(this);
            outPanel._controllers = new ObservableCollection<TECController>();
            foreach (TECController controller in this.Controllers)
            {
                outPanel.Controllers.Add(controller.Copy() as TECController);
            }
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
                foreach (object item in e.NewItems)
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
    }
}
