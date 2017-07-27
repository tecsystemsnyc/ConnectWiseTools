using EstimatingLibrary.Interfaces;
using EstimatingLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECPanel : TECLocated, CostComponent, DragDropComponent
    {
        #region Properties
        private TECPanelType _type;
        public TECPanelType Type
        {
            get { return _type; }
            set
            {
                var old= Type;
                _type = value;
                NotifyPropertyChanged(Change.Edit, "Type", this, value, old);
                //NotifyPropertyChanged("ChildChanged", (object)this, (object)value);
            }
        }

        private ObservableCollection<TECController> _controllers;
        public ObservableCollection<TECController> Controllers
        {
            get { return _controllers; }
            set
            {
                var old = Controllers;
                _controllers = value;
                Controllers.CollectionChanged -= controllersCollectionChanged;
                NotifyPropertyChanged(Change.Edit, "Controllers", this, value, old);
                Controllers.CollectionChanged += controllersCollectionChanged;
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
            Controllers.CollectionChanged += controllersCollectionChanged;
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
        public object DragDropCopy(TECScopeManager scopeManager)
        {
            var outPanel = new TECPanel(this);
            ModelLinkingHelper.LinkScopeItem(outPanel, scopeManager);
            return outPanel;
        }

        private void controllersCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (object item in e.NewItems)
                {
                    NotifyPropertyChanged(Change.Add, "Controllers", this, item);
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    NotifyPropertyChanged(Change.Remove, "Controllers", this, item);
                }
            }
        }
    }
}
