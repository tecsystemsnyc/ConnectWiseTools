using EstimatingLibrary;
using EstimatingLibrary.Interfaces;
using GongSolutions.Wpf.DragDrop;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using TECUserControlLibrary.Models;

namespace TECUserControlLibrary.Utilities
{
    public static class UIHelpers
    {

        public static void StandardDragOver(IDropInfo dropInfo, Func<Type, bool> typeMeetsCondition = null)
        {
            if(typeMeetsCondition == null)
            {
                typeMeetsCondition = type => { return false; };
            }
            if (dropInfo.TargetCollection == dropInfo.DragInfo.SourceCollection)
            {
                return;
            }
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
                bool isDragDropable = sourceItem is IDragDropable;
                bool sourceNotNull = sourceItem != null;
                bool sourceMatchesTarget = targetType.IsInstanceOfType(sourceItem);
                if (sourceNotNull && (sourceMatchesTarget || typeMeetsCondition(targetType)) && isDragDropable)
                {
                    dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                    dropInfo.Effects = DragDropEffects.Copy;
                }
            }
        }
        public static void StandardDrop(IDropInfo dropInfo, TECScopeManager scopeManager, Func<object, object> dropMethod = null)
        {
            var sourceItem = dropInfo.Data;
            Type targetType = dropInfo.TargetCollection.GetType().GetTypeInfo().GenericTypeArguments[0];

            if(dropMethod == null)
            {
                dropMethod = item =>
                {
                    return ((IDragDropable)item).DragDropCopy(scopeManager);
                };
            }

            if (dropInfo.VisualTarget != dropInfo.DragInfo.VisualSource)
            {
                if (sourceItem is IList)
                {
                    var outSource = new List<object>();
                    foreach (object item in ((IList)sourceItem))
                    {
                        var toAdd = dropMethod(item);
                        outSource.Add(toAdd);
                     }
                    sourceItem = outSource;
                }
                else
                {
                    sourceItem = dropMethod(dropInfo.Data);
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

        public static void DragOver(IDropInfo dropInfo, Func<object, Type, bool> dropCondition)
        {
            if (dropInfo.TargetCollection == dropInfo.DragInfo.SourceCollection)
            {
                return;
            }
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
                bool sourceNotNull = sourceItem != null;
                bool allowDrop = dropCondition(sourceItem, targetType);
                bool sourceMatchesTarget = targetType.IsInstanceOfType(sourceItem);
                if (sourceNotNull && (allowDrop || sourceMatchesTarget))
                {
                    dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                    dropInfo.Effects = DragDropEffects.Copy;
                }
            }
        }
        public static void Drop(IDropInfo dropInfo, TECScopeManager scopeManager, Func<object, object> dropObject)
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
                        var toAdd = dropObject(item);
                        outSource.Add(toAdd);
                    }
                    sourceItem = outSource;
                }
                else
                {
                    sourceItem = dropObject(dropInfo.Data);
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
        
        public static void SystemToTypicalDragOver(IDropInfo dropInfo)
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
                bool isDragDropable = sourceItem is IDragDropable;
                bool sourceNotNull = sourceItem != null;
                bool sourceMatchesTarget = sourceType ==  typeof(TECSystem) && (targetType == typeof(TECTypical) || targetType == typeof(TECSystem));
                if (sourceNotNull && sourceMatchesTarget && isDragDropable)
                {
                    dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                    dropInfo.Effects = DragDropEffects.Copy;
                }
            }
        }
        public static void SystemToTypicalDrop(IDropInfo dropInfo, TECBid bid)
        {
            TECSystem sourceItem = (TECSystem)dropInfo.Data;
            Type targetType = dropInfo.TargetCollection.GetType().GetTypeInfo().GenericTypeArguments[0];

            if (dropInfo.VisualTarget != dropInfo.DragInfo.VisualSource)
            {
                TECSystem copiedSystem = (sourceItem).DragDropCopy(bid) as TECSystem;
                sourceItem = new TECTypical(copiedSystem, bid);

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
        public static void ControllerInPanelDrop(IDropInfo dropInfo, Action<TECController> addMethod, TECScopeManager scopeManager, bool isGlobal = true)
        {
            var sourceItem = (dropInfo.Data as IDragDropable).DragDropCopy(scopeManager);
            Type sourceType = dropInfo.Data.GetType();
            Type targetType = dropInfo.TargetCollection.GetType().GetTypeInfo().GenericTypeArguments[0];

            if (dropInfo.VisualTarget != dropInfo.DragInfo.VisualSource)
            {
                if ((sourceType == typeof(TECController) && targetType == typeof(ControllerInPanel)))
                {
                    var controller = sourceItem as TECController;
                    var controllerInPanel = new ControllerInPanel(controller, null);
                    addMethod(sourceItem as TECController);
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

        public static bool TargetCollectionIsType(IDropInfo dropInfo, Type type)
        {
            var targetCollection = dropInfo.TargetCollection;
            if (targetCollection.GetType().GetTypeInfo().GenericTypeArguments.Length > 0)
            {
                Type targetType = targetCollection.GetType().GetTypeInfo().GenericTypeArguments[0];
                return targetType == type;
            } else
            {
                return false;
            }
        }

        #region Get Path Methods
        public static string GetSavePath(FileDialogParameters fileParams, string defaultFileName, string defaultDirectory,
            string initialDirectory = null)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (initialDirectory != null)
            {
                saveFileDialog.InitialDirectory = initialDirectory;
            }
            else
            {
                saveFileDialog.InitialDirectory = defaultDirectory;
            }
            saveFileDialog.FileName = defaultFileName;
            saveFileDialog.Filter = fileParams.Filter;
            saveFileDialog.DefaultExt = fileParams.DefaultExtension;
            saveFileDialog.AddExtension = true;

            string savePath = null;
            if (saveFileDialog.ShowDialog() == true)
            {
                savePath = saveFileDialog.FileName;
            }
            return savePath;
        }
        public static string GetLoadPath(FileDialogParameters fileParams, string defaultDirectory, string initialDirectory = null)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (initialDirectory != null)
            {
                openFileDialog.InitialDirectory = initialDirectory;
            }
            else
            {
                openFileDialog.InitialDirectory = defaultDirectory;
            }
            openFileDialog.Filter = fileParams.Filter;
            openFileDialog.DefaultExt = fileParams.DefaultExtension;
            openFileDialog.AddExtension = true;

            string savePath = null;
            if (openFileDialog.ShowDialog() == true)
            {
                savePath = openFileDialog.FileName;
            }
            return savePath;
        }
        #endregion

        public static List<Tuple<string, TypicalInstanceEnum>> TypicalInstanceSelectorList = new List<Tuple<string, TypicalInstanceEnum>>
        {
            new Tuple<string, TypicalInstanceEnum>("Typical System Defintions", TypicalInstanceEnum.Typical),
            new Tuple<string, TypicalInstanceEnum>("Physical Instances", TypicalInstanceEnum.Instance)
        };

        public static List<Tuple<string, MaterialType>> MaterialSelectorList = new List<Tuple<string, MaterialType>>
        {
            new Tuple<string, MaterialType>("Devices", MaterialType.Device),
            new Tuple<string, MaterialType>("Wiring", MaterialType.ConnectionType),
            new Tuple<string, MaterialType>("Conduit", MaterialType.ConduitType),
            new Tuple<string, MaterialType>("Controller Types", MaterialType.ControllerType),
            new Tuple<string, MaterialType>("Panel Types", MaterialType.PanelType),
            new Tuple<string, MaterialType>("Associated Costs", MaterialType.AssociatedCost),
            new Tuple<string, MaterialType>("IO Modules", MaterialType.IOModule),
            new Tuple<string, MaterialType>("Valves", MaterialType.Valve),
            new Tuple<string, MaterialType>("Manufacturer", MaterialType.Manufacturer),
            new Tuple<string, MaterialType>("Tag", MaterialType.Tag)
        };

        public static List<Tuple<string, Confidence>> ConfidenceSelectorList = new List<Tuple<string, Confidence>>
        {
            new Tuple<string, Confidence>("33%", Confidence.ThirtyThree),
            new Tuple<string, Confidence>("50%", Confidence.Fifty),
            new Tuple<string, Confidence>("66%", Confidence.SixtySix),
            new Tuple<string, Confidence>("80%", Confidence.Eighty),
            new Tuple<string, Confidence>("90%", Confidence.Ninety),
            new Tuple<string, Confidence>("95%", Confidence.NinetyFive)
        };

        public static List<Tuple<string, AllSearchableObjects>> SearchSelectorList = new List<Tuple<string, AllSearchableObjects>>
        {
            new Tuple<string, AllSearchableObjects>("Systems", AllSearchableObjects.System),
            new Tuple<string, AllSearchableObjects>("Equipment", AllSearchableObjects.Equipment),
            new Tuple<string, AllSearchableObjects>("Points", AllSearchableObjects.SubScope),
            new Tuple<string, AllSearchableObjects>("Devices", AllSearchableObjects.Devices),
            new Tuple<string, AllSearchableObjects>("Controllers", AllSearchableObjects.Controllers),
            new Tuple<string, AllSearchableObjects>("Panels", AllSearchableObjects.Panels),
            new Tuple<string, AllSearchableObjects>("Valves", AllSearchableObjects.Valves),
            new Tuple<string, AllSearchableObjects>("Wire Types", AllSearchableObjects.Wires),
            new Tuple<string, AllSearchableObjects>("Conduit Types", AllSearchableObjects.Conduits),
            new Tuple<string, AllSearchableObjects>("Associated Costs", AllSearchableObjects.AssociatedCosts),
            new Tuple<string, AllSearchableObjects>("Misc. Costs", AllSearchableObjects.MiscCosts),
            new Tuple<string, AllSearchableObjects>("Misc. Wiring", AllSearchableObjects.MiscWiring),
            new Tuple<string, AllSearchableObjects>("Tags", AllSearchableObjects.Tags),
            new Tuple<string, AllSearchableObjects>("Controller Types", AllSearchableObjects.ControllerTypes),
            new Tuple<string, AllSearchableObjects>("Panel Types", AllSearchableObjects.PanelTypes),
            new Tuple<string, AllSearchableObjects>("IO Modules", AllSearchableObjects.IOModules)
        };

        public static List<Tuple<string, IOType>> IOSelectorList = new List<Tuple<string, IOType>>
        {
            new Tuple<string, IOType>("AI", IOType.AI),
            new Tuple<string, IOType>("AO", IOType.AO),
            new Tuple<string, IOType>("DI", IOType.DI),
            new Tuple<string, IOType>("DO", IOType.DO),
            new Tuple<string, IOType>("UI", IOType.UI),
            new Tuple<string, IOType>("UO", IOType.UO),
            new Tuple<string, IOType>("BACnetIP", IOType.BACnetIP),
            new Tuple<string, IOType>("BACnetMSTP", IOType.BACnetMSTP),
            new Tuple<string, IOType>("LonWorks", IOType.LonWorks),
            new Tuple<string, IOType>("ModbusRTU", IOType.ModbusRTU),
            new Tuple<string, IOType>("ModbusTCP", IOType.ModbusTCP)
        };

        public static List<Tuple<string, ScopeTemplateIndex>> ScopeTemplateSelectorList = new List<Tuple<string, ScopeTemplateIndex>>
        {
            new Tuple<string, ScopeTemplateIndex>("Systems", ScopeTemplateIndex.System),
            new Tuple<string, ScopeTemplateIndex>("Equipment", ScopeTemplateIndex.Equipment),
            new Tuple<string, ScopeTemplateIndex>("Points", ScopeTemplateIndex.SubScope),
            new Tuple<string, ScopeTemplateIndex>("Controllers and Panels", ScopeTemplateIndex.Controller),
            new Tuple<string, ScopeTemplateIndex>("Miscellaneous", ScopeTemplateIndex.Misc)
        };

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj)
       where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        public static childItem FindVisualChild<childItem>(DependencyObject obj)
            where childItem : DependencyObject
        {
            foreach (childItem child in FindVisualChildren<childItem>(obj))
            {
                return child;
            }

            return null;
        }

    }

    public enum EditIndex { System, Equipment, SubScope, Device, Point, Controller, Panel, PanelType, Nothing };
    public enum GridIndex { Systems = 1, DDC, Location, Proposal, Budget, Misc, Settings };
    public enum TemplateGridIndex { None, Systems, Equipment, SubScope, Devices, DDC, Materials, Constants };
    public enum ScopeCollectionIndex { None, System, Equipment, SubScope, Devices, Tags, Manufacturers, AddDevices, AddControllers, Controllers, AssociatedCosts, Panels, AddPanel, MiscCosts, MiscWiring };
    public enum LocationScopeType { System, Equipment, SubScope };
    public enum MaterialType { Device, ConnectionType, ConduitType, ControllerType,
        PanelType, AssociatedCost, IOModule, Valve, Manufacturer, Tag};
    public enum TypicalSystemIndex { Edit, Instances };
    public enum SystemComponentIndex { Equipment, Controllers, Electrical, Network, Misc, Proposal };
    public enum ProposalIndex { Scope, Systems, Notes }
    public enum SystemsSubIndex { Typical, Instance, Location}
    public enum ScopeTemplateIndex { System, Equipment, SubScope, Controller, Misc }
    public enum AllSearchableObjects
    {
        System,
        Equipment,
        SubScope,
        Controllers,
        Panels,
        ControllerTypes,
        PanelTypes,
        Devices,
        MiscCosts,
        MiscWiring,
        Valves,
        Tags,
        AssociatedCosts,
        Wires,
        Conduits,
        IOModules
    }
    public enum TypicalInstanceEnum { Typical, Instance }
    public enum MaterialSummaryIndex { Devices, Controllers, Panels, Wire, Conduit, Misc }



}
