using EstimatingLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EstimatingLibrary;
using DebugLibrary;
using System.Collections;

namespace EstimatingUtilitiesLibrary
{
    public class DoStacker
    {
        private List<PropertyChangedExtendedEventArgs> undoStack;
        private List<PropertyChangedExtendedEventArgs> redoStack;
        private ChangeWatcher watcher;
        private bool isDoing = false;

        #region Constructors
        public DoStacker(ChangeWatcher changeWatcher)
        {
            watcher = changeWatcher;
            watcher.ExtendedChanged += objectChanged;

            undoStack = new List<PropertyChangedExtendedEventArgs>();
            redoStack = new List<PropertyChangedExtendedEventArgs>();
        }
        
        #endregion

        private void objectChanged(object sender, PropertyChangedExtendedEventArgs e)
        {
            undoStack.Add(e);
        }

        public void Undo()
        {
            isDoing = true;
            PropertyChangedExtendedEventArgs item = undoStack.Last();
            DebugHandler.LogDebugMessage("Undoing:       " + item.Change.ToString() + "    #Undo: " + undoStack.Count() + "    #Redo: " + redoStack.Count(), DebugBooleans.Stack);
            if (item.Change == Change.Add)
            {
                handleAdd(item);
                undoStack.Remove(item);
                undoStack.Remove(undoStack.Last());
                redoStack.Add(item);
            }
            else if (item.Change == Change.Remove)
            {
                handleRemove(item);
                undoStack.Remove(item);
                undoStack.Remove(undoStack.Last());
                redoStack.Add(item);
            }
            else if (item.Change == Change.Edit)
            {
                int index = undoStack.IndexOf(item);
                redoStack.Add(new PropertyChangedExtendedEventArgs(Change.Edit, item.PropertyName, item.Sender, item.OldValue, item.Value));
                handleEdit(item);
                for (int x = (undoStack.Count - 1); x >= index; x--)
                {
                    undoStack.RemoveAt(x);
                }
            }

            string message = "After Undoing: " + item.Change.ToString() + "    #Undo: " + undoStack.Count() + "    #Redo: " + redoStack.Count() + "\n";
            DebugHandler.LogDebugMessage(message, DebugBooleans.Stack);

            isDoing = false;
        }
        public void Redo()
        {
            isDoing = true;
            PropertyChangedExtendedEventArgs item = redoStack.Last();

            string message = "Redoing:       " + item.Change.ToString() + "    #Undo: " + undoStack.Count() + "    #Redo: " + redoStack.Count();
            DebugHandler.LogDebugMessage(message, DebugBooleans.Stack);

            if (item.Change == Change.Add)
            {
                handleRemove(item);
                redoStack.Remove(item);
            }
            else if (item.Change == Change.Remove)
            {
                handleAdd(item);
                redoStack.Remove(item);
            }
            else if (item.Change == Change.Edit)
            {
                int index = 0;
                if (undoStack.Count > 0)
                {
                    index = undoStack.IndexOf(undoStack.Last());
                }

                handleEdit(item);
                redoStack.Remove(item);
                for (int x = (undoStack.Count - 2); x > index; x--)
                {
                    undoStack.RemoveAt(x);
                }
            }

            message = "After Redoing: " + item.Change.ToString() + "    #Undo: " + undoStack.Count() + "    #Redo: " + redoStack.Count() + "\n";
            DebugHandler.LogDebugMessage(message, DebugBooleans.Stack);

            isDoing = false;
        }

        private void handleAdd(PropertyChangedExtendedEventArgs item)
        {
            try
            {
                var property = item.Sender.GetType().GetProperty(item.PropertyName);
                var parentCollection = property.GetValue(item.Sender);
                ((IList)parentCollection).Remove(item.Value);
            }
            catch
            {
                string message = "Target object: " + item.Sender + " and reference object " + item.Value + " not handled in add";
                DebugHandler.LogDebugMessage(message, DebugBooleans.Stack);
            }
        }
        private void handleRemove(PropertyChangedExtendedEventArgs item)
        {
            try
            {
                var property = item.Sender.GetType().GetProperty(item.PropertyName);
                var parentCollection = property.GetValue(item.Sender);
                ((IList)parentCollection).Add(item.Value);
            }
            catch
            {
                string message = "Target object: " + item.Sender + " and reference object " + item.Value + " not handled in remove";
                DebugHandler.LogDebugMessage(message, DebugBooleans.Stack);
            }
        }
        private void handleEdit(PropertyChangedExtendedEventArgs item)
        {
            var sender = item.Sender;
            var oldValue = item.OldValue;
            var property = sender.GetType().GetProperty(item.PropertyName);

            if (property.GetSetMethod() != null)
            { property.SetValue(sender, oldValue); }
            else
            {
                string message = "Property could not be set: " + property.Name;
                DebugHandler.LogDebugMessage(message, DebugBooleans.Properties);
            }
        }

        public int UndoCount()
        {
            return undoStack.Count;
        }
       
    }
}
