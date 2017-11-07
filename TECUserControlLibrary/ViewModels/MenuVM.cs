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

        protected TECMenuItem addMenuItem(string newItemName, string parentItemName = null)
        {
            if (menuItemDictionary.ContainsKey(newItemName))
            {
                throw new InvalidOperationException("A menu item with the name '" + newItemName + "' already exists.");
            }

            if (parentItemName == null)
            //Add base menu item
            {
                TECMenuItem newBaseItem = new TECMenuItem(newItemName, true);
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

            //File menu items
            addMenuItem("New", "File");
            addMenuItem("Load", "File");
            addMenuItem("Save", "File");
            addMenuItem("Save As", "File");

            //Edit menu items
            addMenuItem("Undo", "Edit");
            addMenuItem("Redo", "Edit");

            //Templates menu items
            addMenuItem("Refresh Templates", "Templates");
        }
        #endregion
    }
}