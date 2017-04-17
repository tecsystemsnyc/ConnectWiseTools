using EstimatingLibrary;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TECUserControlLibrary.Models;

namespace TECUserControlLibrary
{
    public static class UIHelpers
    {

        public static void StandardDragOver(IDropInfo dropInfo)
        {
            var sourceItem = dropInfo.Data;
            Type sourceType;
            if (sourceItem is IList && ((IList)sourceItem).Count > 0)
            { sourceType = ((IList)sourceItem)[0].GetType(); }
            else
            { sourceType = sourceItem.GetType(); }

            var targetCollection = dropInfo.TargetCollection;
            if (targetCollection.GetType().GetTypeInfo().GenericTypeArguments.Length > 0)
            {
                Type targetType = targetCollection.GetType().GetTypeInfo().GenericTypeArguments[0];
                if (sourceItem != null && sourceType == targetType)
                {
                    dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                    dropInfo.Effects = DragDropEffects.Copy;
                }
            }
        }
        public static void StandardDrop(IDropInfo dropInfo)
        {
            var sourceItem = dropInfo.Data;
            Type targetType = dropInfo.TargetCollection.GetType().GetTypeInfo().GenericTypeArguments[0];

            if (dropInfo.VisualTarget != dropInfo.DragInfo.VisualSource)
            {
                if (sourceItem is IList)
                {
                    var outSource = new List<object>();
                    foreach (object item in ((IList)sourceItem))
                    {
                        outSource.Add(((TECScope)item).DragDropCopy());
                    }
                    sourceItem = outSource;
                }
                else
                {
                    sourceItem = ((TECScope)dropInfo.Data).DragDropCopy();
                }
                if (dropInfo.InsertIndex > ((IList)dropInfo.TargetCollection).Count)
                {
                    if (sourceItem is IList)
                    {
                        foreach (object item in ((IList)sourceItem))
                        {
                            ((IList)dropInfo.TargetCollection).Add(item);
                        }
                    }
                    else
                    {
                        ((IList)dropInfo.TargetCollection).Add(sourceItem);
                    }
                }
                else
                {
                    if (sourceItem is IList)
                    {
                        var x = dropInfo.InsertIndex;
                        foreach (object item in ((IList)sourceItem))
                        {
                            ((IList)dropInfo.TargetCollection).Insert(x, item);
                            x += 1;
                        }
                    }
                    else
                    {
                        ((IList)dropInfo.TargetCollection).Insert(dropInfo.InsertIndex, sourceItem);

                    }
                }
            }
            else
            {
                handleReorderDrop(dropInfo);
            }
        }

        public static void ControllerInPanelDragOver(IDropInfo dropInfo)
        {
            var sourceItem = dropInfo.Data;
            var targetCollection = dropInfo.TargetCollection;
            if (targetCollection.GetType().GetTypeInfo().GenericTypeArguments.Length > 0)
            {
                Type sourceType = sourceItem.GetType();
                Type targetType = targetCollection.GetType().GetTypeInfo().GenericTypeArguments[0];

                if (sourceItem != null && sourceType == targetType ||
                    (sourceType == typeof(TECController) && targetType == typeof(ControllerInPanel)))
                {
                    dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                    dropInfo.Effects = DragDropEffects.Copy;
                }
            }
        }
        public static void ControllerInPanelDrop(IDropInfo dropInfo, ObservableCollection<TECController> controllers)
        {
            var sourceItem = dropInfo.Data;
            Type sourceType = dropInfo.Data.GetType();
            Type targetType = dropInfo.TargetCollection.GetType().GetTypeInfo().GenericTypeArguments[0];

            if (dropInfo.VisualTarget != dropInfo.DragInfo.VisualSource)
            {
                if ((sourceType == typeof(TECController) && targetType == typeof(ControllerInPanel)))
                {
                    var controllerInPanel = new ControllerInPanel(sourceItem as TECController, null);
                    controllers.Add(sourceItem as TECController);
                    sourceItem = controllerInPanel;
                }
            }
            else
            {
                handleReorderDrop(dropInfo);
            }
        }

        public static void ControlledScopeDrop(IDropInfo dropInfo, object bidOrTemplates)
        {
            var sourceItem = dropInfo.Data;
            if (dropInfo.VisualTarget != dropInfo.DragInfo.VisualSource)
            {
                if (dropInfo.Data is TECControlledScope)
                {
                    Dictionary<Guid, Guid> guidDictionary = new Dictionary<Guid, Guid>();
                    sourceItem = new TECControlledScope((dropInfo.Data as TECControlledScope), guidDictionary);
                    var newControlledScope = sourceItem as TECControlledScope;
                    ModelLinkingHelper.LinkControlledScopeObjects(newControlledScope.Systems,
                        newControlledScope.Controllers, newControlledScope.Panels, bidOrTemplates, guidDictionary);
                    if (dropInfo.InsertIndex > ((IList)dropInfo.TargetCollection).Count)
                    {
                        ((IList)dropInfo.TargetCollection).Add(sourceItem);
                    }
                    else
                    {
                        ((IList)dropInfo.TargetCollection).Insert(dropInfo.InsertIndex, sourceItem);
                    }
                }
            }
            else
            {
                handleReorderDrop(dropInfo);
            }
        }

        private static void handleReorderDrop(IDropInfo dropInfo)
        {
            var sourceItem = dropInfo.Data;
            int currentIndex = ((IList)dropInfo.TargetCollection).IndexOf(sourceItem);
            int finalIndex = dropInfo.InsertIndex;

            if (sourceItem is IList)
            {
                currentIndex = ((IList)dropInfo.TargetCollection).IndexOf(((IList)sourceItem)[0]);
                if (dropInfo.InsertIndex > currentIndex)
                {
                    finalIndex -= 1;
                }
                if (dropInfo.InsertIndex > ((IList)dropInfo.TargetCollection).Count)
                {
                    finalIndex = ((IList)dropInfo.TargetCollection).Count - 1;
                }
                var x = 0;
                foreach (object item in ((IList)sourceItem))
                {
                    currentIndex = ((IList)dropInfo.TargetCollection).IndexOf(((IList)sourceItem)[x]);
                    ((dynamic)dropInfo.TargetCollection).Move(currentIndex, finalIndex);
                    x += 1;
                }
            }
            else
            {
                if (dropInfo.InsertIndex > currentIndex)
                {
                    finalIndex -= 1;
                }
                if (dropInfo.InsertIndex > ((IList)dropInfo.TargetCollection).Count)
                {
                    finalIndex = ((IList)dropInfo.TargetCollection).Count - 1;
                }
                ((dynamic)dropInfo.TargetCollection).Move(currentIndex, finalIndex);
            }
        }
    }
}
