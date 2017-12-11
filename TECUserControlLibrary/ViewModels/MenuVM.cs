using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TECUserControlLibrary.Models;

namespace TECUserControlLibrary.ViewModels
{
    public class MenuVM : ViewModelBase
    {
        #region Constants
        protected const string BUSY_TEXT = "Please wait for the current process to complete";
        #endregion

        #region Fields
        private Dictionary<string, TECMenuItem> menuItemDictionary;
        private readonly ObservableCollection<TECMenuItem> _menu;
        #endregion

        #region Properties
        public ReadOnlyObservableCollection<TECMenuItem> Menu { get { return new ReadOnlyObservableCollection<TECMenuItem>(_menu); } }
        #endregion

        public MenuVM()
        {
            menuItemDictionary = new Dictionary<string, TECMenuItem>();
            _menu = new ObservableCollection<TECMenuItem>();

            setupMenu();
        }

        #region Methods
        public void SetNewCommand(Action execute, Func<bool> canExecute = null)
        {
            RelayCommand command = new RelayCommand(execute, forceNullToTrue(canExecute));
            setCommand("New", command);
        }
        public void SetLoadCommand(Action execute, Func<bool> canExecute = null)
        {
            RelayCommand command = new RelayCommand(execute, forceNullToTrue(canExecute));
            setCommand("Load", command);
        }
        public void SetSaveDeltaCommand(Action execute, Func<bool> canExecute = null)
        {
            RelayCommand command = new RelayCommand(execute, forceNullToTrue(canExecute));
            setCommand("Save", command);
        }
        public void SetSaveNewCommand(Action execute, Func<bool> canExecute = null)
        {
            RelayCommand command = new RelayCommand(execute, forceNullToTrue(canExecute));
            setCommand("Save As", command);
        }
        public void SetUndoCommand(Action execute, Func<bool> canExecute = null)
        {
            RelayCommand command = new RelayCommand(execute, forceNullToTrue(canExecute));
            setCommand("Undo", command);
        }
        public void SetRedoCommand(Action execute, Func<bool> canExecute = null)
        {
            RelayCommand command = new RelayCommand(execute, forceNullToTrue(canExecute));
            setCommand("Redo", command);
        }
        public void SetRefreshTemplatesCommand(Action execute, Func<bool> canExecute = null)
        {
            RelayCommand command = new RelayCommand(execute, forceNullToTrue(canExecute));
            setCommand("Refresh Templates", command);
        }
        public void SetWikiCommand(Action execute, Func<bool> canExecute = null)
        {
            RelayCommand command = new RelayCommand(execute, forceNullToTrue(canExecute));
            setCommand("Wiki", command);
        }
        public void SetReportBugCommand(Action execute, Func<bool> canExecute = null)
        {
            RelayCommand command = new RelayCommand(execute, forceNullToTrue(canExecute));
            setCommand("Report Bug", command);
        }

        protected TECMenuItem addMenuItem(string newItemName, string disabledText = null, string parentItemName = null)
        {
            if (menuItemDictionary.ContainsKey(newItemName))
            {
                throw new InvalidOperationException("A menu item with the name '" + newItemName + "' already exists.");
            }

            if (parentItemName == null)
            //Add base menu item
            {
                TECMenuItem newBaseItem = new TECMenuItem(newItemName, true);
                if(disabledText != null) { newBaseItem.DisabledText = disabledText; }
                menuItemDictionary.Add(newBaseItem.Name, newBaseItem);
                _menu.Add(newBaseItem);
                return newBaseItem;
            }
            else
            //Add child menu item
            {
                if (!menuItemDictionary.ContainsKey(parentItemName))
                {
                    throw new InvalidOperationException("Menu item with the name '" + parentItemName + "' doesn't exist.");
                }

                TECMenuItem parentItem = menuItemDictionary[parentItemName];
                TECMenuItem newChildItem = new TECMenuItem(newItemName, false);
                if (disabledText != null) { newChildItem.DisabledText = disabledText; }
                menuItemDictionary.Add(newChildItem.Name, newChildItem);
                parentItem.AddMenuItem(newChildItem);
                return newChildItem;
            }
        }
        protected void setCommand(string itemName, RelayCommand command)
        {
            if (!menuItemDictionary.ContainsKey(itemName))
            {
                throw new InvalidOperationException("No menu item with the name '" + itemName + "' exists in the menu.");
            }

            TECMenuItem item = menuItemDictionary[itemName];
            item.Command = command;
        }

        protected Func<bool> forceNullToTrue(Func<bool> func)
        {
            if (func == null)
            {
                return () => true;
            }
            else
            {
                return func;
            }
        }

        private void setupMenu()
        {
            //Main menu items
            addMenuItem("File");
            addMenuItem("Edit");
            addMenuItem("Templates");
            addMenuItem("Help");

            //File menu items
            addMenuItem("New", BUSY_TEXT, parentItemName:"File");
            addMenuItem("Load", BUSY_TEXT, parentItemName: "File");
            addMenuItem("Save", "Nothing to save", parentItemName: "File");
            addMenuItem("Save As", BUSY_TEXT, parentItemName: "File");

            //Edit menu items
            addMenuItem("Undo", "Nothing to undo", parentItemName: "Edit");
            addMenuItem("Redo", "Nothing to redo", parentItemName: "Edit");

            //Templates menu items
            addMenuItem("Refresh Templates", BUSY_TEXT, parentItemName: "Templates");

            //Help menu items
            addMenuItem("Wiki", parentItemName: "Help");
            addMenuItem("Report Bug", parentItemName: "Help");
        }
        #endregion
    }
}