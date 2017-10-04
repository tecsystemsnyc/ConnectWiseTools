using EstimatingLibrary;
using EstimatingLibrary.Interfaces;
using EstimatingLibrary.Utilities;
using GongSolutions.Wpf.DragDrop;
using Microsoft.Win32;
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
                if (sourceItem != null && sourceType == targetType && sourceType is IDragDropable)
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
                        var toAdd = ((IDragDropable)item).DragDropCopy(scopeManager);
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
                        sourceItem = ((IDragDropable)dropInfo.Data).DragDropCopy(scopeManager);
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

        #region File Parameters
        public static FileDialogParameters BidFileParameters
        {
            get
            {
                FileDialogParameters fileParams;
                fileParams.Filter = "Bid Database Files (*.bdb)|*.bdb" + "|All Files (*.*)|*.*";
                fileParams.DefaultExtension = "bdb";
                return fileParams;
            }
        }
        public static FileDialogParameters EstimateFileParameters
        {
            get
            {
                FileDialogParameters fileParams;
                fileParams.Filter = "Estimate Database Files (*.edb)|*.edb" + "|All Files (*.*)|*.*";
                fileParams.DefaultExtension = "edb";
                return fileParams;
            }
        }
        public static FileDialogParameters TemplatesFileParameters
        {
            get
            {
                FileDialogParameters fileParams;
                fileParams.Filter = "Templates Database Files (*.tdb)|*.tdb" + "|All Files (*.*)|*.*";
                fileParams.DefaultExtension = "tdb";
                return fileParams;
            }
        }
        public static FileDialogParameters DocumentFileParameters
        {
            get
            {
                FileDialogParameters fileParams;
                fileParams.Filter = "Rich Text Files (*.rtf)|*.rtf";
                fileParams.DefaultExtension = "rtf";
                return fileParams;
            }
        }
        public static FileDialogParameters WordDocumentFileParameters
        {
            get
            {
                FileDialogParameters fileParams;
                fileParams.Filter = "Word Documents (*.docx)|*.docx";
                fileParams.DefaultExtension = "docx";
                return fileParams;
            }
        }
        public static FileDialogParameters CSVFileParameters
        {
            get
            {
                FileDialogParameters fileParams;
                fileParams.Filter = "Comma Separated Values Files (*.csv)|*.csv";
                fileParams.DefaultExtension = "csv";
                return fileParams;
            }
        }
        #endregion

        #region Get Path Methods
        public static string GetSavePath(FileDialogParameters fileParams, string defaultFileName, string defaultDirectory,
            string initialDirectory = null, bool isNew = false)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (initialDirectory != null && !isNew)
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

    }

    public enum EditIndex { System, Equipment, SubScope, Device, Point, Controller, Panel, PanelType, Nothing };
    public enum GridIndex { Systems = 1, DDC, Location, Proposal, Budget, Misc, Settings };
    public enum TemplateGridIndex { None, Systems, Equipment, SubScope, Devices, DDC, Materials, Constants };
    public enum ScopeCollectionIndex { None, System, Equipment, SubScope, Devices, Tags, Manufacturers, AddDevices, AddControllers, Controllers, AssociatedCosts, Panels, AddPanel, MiscCosts, MiscWiring };
    public enum LocationScopeType { System, Equipment, SubScope };
    public enum MaterialType { Devices, Wiring, Conduit, PanelTypes, AssociatedCosts, IOModules, MiscCosts };
    public enum TypicalSystemIndex { Edit, Instances };
    public enum SystemComponentIndex { Equipment, Controllers, Panels, Electrical, Misc, Proposal };
    public enum TECMaterialIndex { Devices, Controllers, Panels, MiscCosts }
    public enum ElectricalMaterialIndex { Wire, Conduit, MiscCosts }
    public enum ProposalIndex { Scope, Systems, Notes }
    public enum SystemsSubIndex { Typical, Instance, Location}
    public enum ScopeTemplateIndex { System, Equipment, SubScope, Controller, Panel }
    public enum AllSearchableObjects
    {
        SubScope,
        Equipment,
        System,
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
        Conduits
    }
    public enum TypicalInstanceEnum { Typical, Instance }

    


}
