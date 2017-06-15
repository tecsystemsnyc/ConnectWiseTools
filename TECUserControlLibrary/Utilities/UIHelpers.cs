using EstimatingLibrary;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TECUserControlLibrary.Models;

namespace TECUserControlLibrary.Utilities
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
        public static void StandardDrop(IDropInfo dropInfo, TECScopeManager scopeManager, bool newDevice = false)
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
                        var toAdd = ((TECScope)item).DragDropCopy() as TECScope;
                        ModelLinkingHelper.LinkScopeCopy(toAdd, scopeManager);
                        outSource.Add(toAdd);
                     }
                    sourceItem = outSource;
                }
                else
                {
                    if (newDevice && dropInfo.Data is TECDevice)
                    {
                        sourceItem = new TECDevice(dropInfo.Data as TECDevice);
                    }
                    else
                    {
                        sourceItem = ((TECScope)dropInfo.Data).DragDropCopy();
                        ModelLinkingHelper.LinkScopeCopy(sourceItem as TECScope, scopeManager);

                    }
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
        public static void ControllerInPanelDrop(IDropInfo dropInfo, ObservableCollection<TECController> controllers, bool isGlobal = true)
        {
            var sourceItem = (dropInfo.Data as TECScope).DragDropCopy();
            Type sourceType = dropInfo.Data.GetType();
            Type targetType = dropInfo.TargetCollection.GetType().GetTypeInfo().GenericTypeArguments[0];

            if (dropInfo.VisualTarget != dropInfo.DragInfo.VisualSource)
            {
                if ((sourceType == typeof(TECController) && targetType == typeof(ControllerInPanel)))
                {
                    var controller = sourceItem as TECController;
                    controller.IsGlobal = isGlobal;
                    var controllerInPanel = new ControllerInPanel(controller, null);
                    controllers.Add(sourceItem as TECController);
                    sourceItem = controllerInPanel;
                }
            }
            else
            {
                handleReorderDrop(dropInfo);
            }
        }

        public static void FileDragOver(IDropInfo dropInfo)
        {
            if (dropInfo.Data is DataObject)
            {
                var dragFileList = ((DataObject)dropInfo.Data).GetFileDropList().Cast<string>();
                if (dragFileList.Count() == 1)
                {
                    if (Path.GetExtension(dragFileList.First()) == ".bdb")
                    {
                        dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                        dropInfo.Effects = DragDropEffects.Copy;
                    }
                }
            }
        }
        public static string FileDrop(IDropInfo dropInfo)
        {
            if(dropInfo.Data is IEnumerable)
            {
                var dragFileList = ((DataObject)dropInfo.Data).GetFileDropList().Cast<string>();
                if (dragFileList.Count() == 1)
                {
                    if (Path.GetExtension(dragFileList.First()) == ".bdb")
                    {
                        return dragFileList.First();
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }else
            {
                return null;
            }
            
        }

        public static void ControlledScopeDrop(IDropInfo dropInfo, TECScopeManager scopeManager)
        {
            var sourceItem = dropInfo.Data;
            if (dropInfo.VisualTarget != dropInfo.DragInfo.VisualSource)
            {
                if (dropInfo.Data is TECSystem)
                {
                    Dictionary<Guid, Guid> guidDictionary = new Dictionary<Guid, Guid>();
                    sourceItem = new TECSystem((dropInfo.Data as TECSystem), guidDictionary);
                    var newControlledScope = sourceItem as TECSystem;
                    ModelLinkingHelper.LinkSystem(newControlledScope, scopeManager, guidDictionary);
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

    public enum EditIndex { System, Equipment, SubScope, Device, Point, Controller, Panel, PanelType, Nothing };
    public enum GridIndex { Systems = 1, DDC, Location, Proposal, Budget, Misc, Settings };
    public enum TemplateGridIndex { None, Systems, Equipment, SubScope, Devices, DDC, Materials, Constants };
    public enum ScopeCollectionIndex { None, System, Equipment, SubScope, Devices, Tags, Manufacturers, AddDevices, AddControllers, Controllers, AssociatedCosts, Panels, AddPanel, MiscCosts, MiscWiring };
    public enum LocationScopeType { System, Equipment, SubScope };
    public enum MaterialType { Wiring, Conduit, PanelTypes, AssociatedCosts, IOModules, MiscCosts };
    public enum TypicalSystemIndex { Edit, Instances };
    public enum SystemComponentIndex { Equipment, Controllers, Electrical, Misc, Proposal };
    public enum TECMaterialIndex { Devices, Controllers, Panels, MiscCosts }
    public enum ElectricalMaterialIndex { Wire, Conduit, MiscCosts }
    public enum ProposalIndex { Scope, Systems, Notes }
    public enum SystemsSubIndex { Typical, Instance, Location}
}
