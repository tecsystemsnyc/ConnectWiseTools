using EstimatingLibrary.Interfaces;
using System;
using System.Collections.Generic;

namespace EstimatingLibrary.Utilities
{
    public class ChangeWatcher
    {
        #region Constructors
        public ChangeWatcher(TECObject item)
        {
            initialize(item);
        }
        #endregion

        #region Events
        public event Action<TECChangedEventArgs> Changed;
        public event Action<TECChangedEventArgs> InstanceChanged;
        public event Action<CostBatch> CostChanged;
        public event Action<int> PointChanged;
        #endregion

        #region Methods
        public void Refresh(TECObject item)
        {
            initialize(item);
        }

        private void initialize(TECObject item)
        {
            register(item);
        }

        #region Registration
        private void registerTECObject(TECObject ob)
        {
            ob.TECChanged += handleTECChanged;
            if (ob is INotifyCostChanged costOb)
            {
                costOb.CostChanged += (e) => handleCostChanged(ob, e);
            }
            if (ob is INotifyPointChanged pointOb)
            {
                pointOb.PointChanged += (e) => handlePointChanged(ob, e);
            }
        }
        private void registerChange(TECChangedEventArgs args)
        {
            if(args.Change == Change.Add && args.Value is TECObject tObj)
            {
                register(tObj);
            }
            else if (args.Change == Change.Edit && args.Sender is ISaveable saveable)
            {
                if (!saveable.RelatedObjects.Contains(args.PropertyName) && args.Value is TECObject tValue)
                {
                    register(tValue);
                }
            }
        }
        #endregion

        #region Event Handlers
        private void handleTECChanged(TECChangedEventArgs obj)
        {
            registerChange(obj);
            Changed?.Invoke(obj);
            bool senderTypical = obj.Sender is ITypicalable sTyp && sTyp.IsTypical;
            bool valueTypical = obj.Value is ITypicalable vTyp && vTyp.IsTypical;
            if(!senderTypical && !valueTypical)
            {
                InstanceChanged?.Invoke(obj);
            }
            
        }
        private void handleCostChanged(TECObject sender, CostBatch obj)
        {
            CostChanged?.Invoke(obj);
        }
        private void handlePointChanged(TECObject sender, int num)
        {
            PointChanged?.Invoke(num);
        }
        #endregion
        
        public void register(TECObject item)
        {
            registerTECObject(item);
            if(item is ISaveable saveable)
            {
                foreach (Tuple<string, TECObject> child in saveable.SaveObjects.ChildList())
                {
                    if (!saveable.RelatedObjects.Contains(child.Item1))
                    {
                        register(child.Item2);
                    }
                }
            }
        }
        #endregion
    }
}