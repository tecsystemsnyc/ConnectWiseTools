using EstimatingLibrary.Interfaces;
using EstimatingLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EstimatingLibrary
{
    public class TECPanel : TECLocated, IDragDropable, ITypicalable
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
                notifyCombinedChanged(Change.Edit, "Type", this, value, old);
                notifyCostChanged(value.CostBatch - old.CostBatch);
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
                notifyCombinedChanged(Change.Edit, "Controllers", this, value, old);
                Controllers.CollectionChanged += controllersCollectionChanged;
            }
        }
        
        public bool IsTypical { get; private set; }
        #endregion

        public TECPanel(Guid guid, TECPanelType type, bool isTypical) : base(guid)
        {
            IsTypical = isTypical;
            _guid = guid;
            _controllers = new ObservableCollection<TECController>();
            _type = type;
            Controllers.CollectionChanged += controllersCollectionChanged;
        }
        public TECPanel(TECPanelType type, bool isTypical) : this(Guid.NewGuid(), type, isTypical) { }
        public TECPanel(TECPanel panel, bool isTypical, Dictionary<Guid, Guid> guidDictionary = null) : this(panel.Type, isTypical)
        {
            if (guidDictionary != null)
            { guidDictionary[_guid] = panel.Guid; }
            copyPropertiesFromScope(panel);
            foreach (TECController controller in panel.Controllers)
            {
                _controllers.Add(new TECController(controller, isTypical, guidDictionary));
            }
        }
        public object DragDropCopy(TECScopeManager scopeManager)
        {
            var outPanel = new TECPanel(this, this.IsTypical);
            ModelLinkingHelper.LinkScopeItem(outPanel, scopeManager);
            return outPanel;
        }
        
        protected override CostBatch getCosts()
        {
            CostBatch costs = base.getCosts();
            costs += Type.CostBatch;
            return costs;
        }
        protected override SaveableMap propertyObjects()
        {
            SaveableMap saveList = new SaveableMap();
            saveList.AddRange(base.propertyObjects());
            saveList.Add(this.Type, "Type");
            saveList.AddRange(this.Controllers, "Controllers");
            return saveList;
        }
        protected override SaveableMap linkedObjects()
        {
            SaveableMap saveList = new SaveableMap();
            saveList.AddRange(base.linkedObjects());
            saveList.Add(this.Type, "Type");
            saveList.AddRange(this.Controllers, "Controllers");
            return saveList;
        }
        protected override void notifyCostChanged(CostBatch costs)
        {
            if (!IsTypical)
            {
                base.notifyCostChanged(costs);
            }
        }

        private void controllersCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (object item in e.NewItems)
                {
                    notifyCombinedChanged(Change.Add, "Controllers", this, item);
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    notifyCombinedChanged(Change.Remove, "Controllers", this, item);
                }
            }
        }
    }
}
