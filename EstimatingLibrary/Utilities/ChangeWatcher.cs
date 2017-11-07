using EstimatingLibrary.Interfaces;
using System;
using System.ComponentModel;

namespace EstimatingLibrary.Utilities
{
    public class ChangeWatcher
    {
        #region Constructors
        public ChangeWatcher(TECObject item)
        {
            register(item);
        }
        #endregion

        #region Events
        public event Action<TECChangedEventArgs> Changed;
        public event Action<TECChangedEventArgs> InstanceChanged;
        public event Action<CostBatch> CostChanged;
        public event Action<int> PointChanged;
        public event Action<object, PropertyChangedEventArgs> PropertyChanged;
        public event Action<Change, TECObject> InstanceConstituentChanged;
        #endregion

        #region Methods
        public void Refresh(TECObject item)
        {
            register(item);
        }
        
        private void register(TECObject item)
        {
            registerTECObject(item);
            if (item is IRelatable saveable)
            {
                foreach (Tuple<string, TECObject> child in saveable.PropertyObjects.ChildList())
                {
                    if (!saveable.LinkedObjects.Contains(child.Item1))
                    {
                        register(child.Item2);
                    }
                }
            }
        }
        private void registerTECObject(TECObject ob)
        {
            ob.TECChanged += handleTECChanged;
            ob.PropertyChanged += raisePropertyChanged;
            if (ob is INotifyCostChanged costOb)
            {
                costOb.CostChanged += (e) => raiseCostChanged(ob, e);
            }
            if (ob is INotifyPointChanged pointOb)
            {
                pointOb.PointChanged += (e) => raisePointChanged(ob, e);
            }
        }
        private void registerChange(TECChangedEventArgs args)
        {
            if(args.PropertyName != "TypicalInstanceDictionary")
            {
                if (args.Change == Change.Add && args.Value is TECObject tObj)
                {
                    register(tObj);
                }
                else if (args.Change == Change.Edit && args.Sender is IRelatable saveable)
                {
                    if (!saveable.LinkedObjects.Contains(args.PropertyName) && args.Value is TECObject tValue)
                    {
                        register(tValue);
                    }
                }
            }
        }
        private void handleTECChanged(TECChangedEventArgs e)
        {
            registerChange(e);
            raiseChanged(e);

            if (e.PropertyName != "TypicalInstanceDictionary")
            {
                if (e.Value is ITypicalable valueTyp)
                {
                    if (!valueTyp.IsTypical)
                    {
                        raiseInstanceChanged(e);
                    }
                }
                else
                {
                    if (e.Sender is ITypicalable senderTyp)
                    {
                        if (!senderTyp.IsTypical)
                        {
                            raiseInstanceChanged(e);
                        }
                    }
                    else
                    {
                        raiseInstanceChanged(e);
                    }
                }
            }
        }

        private void raisePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(sender, e);
        }
        private void raiseChanged(TECChangedEventArgs e)
        {
            Changed?.Invoke(e);
        }
        private void raiseCostChanged(TECObject sender, CostBatch obj)
        {
            CostChanged?.Invoke(obj);
        }
        private void raisePointChanged(TECObject sender, int num)
        {
            PointChanged?.Invoke(num);
        }
        private void raiseInstanceChanged(TECChangedEventArgs e)
        {
            InstanceChanged?.Invoke(e);
            if((e.Change == Change.Add || e.Change == Change.Remove) && e.Sender is IRelatable parent)
            {
                if(!parent.LinkedObjects.Contains(e.PropertyName))
                    raiseConstituents(e.Change, e.Value as TECObject);
            }
        }
        private void raiseConstituents(Change change, TECObject item)
        {
            InstanceConstituentChanged?.Invoke(change, item);
            if(item is IRelatable parent)
            {
                foreach(var child in parent.PropertyObjects.ChildList())
                {
                    if (!parent.LinkedObjects.Contains(child.Item1))
                    {
                        raiseConstituents(change, child.Item2);
                    }
                }
            }
        }
        #endregion
        
    }
}