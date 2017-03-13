using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using TECUserControlLibrary.Models;

namespace TECUserControlLibrary.ViewModels
{
    public enum MenuType { TB, SB, EB }

    public class MenuViewModel : ViewModelBase
    {

        public MenuViewModel(MenuType type)
        {
            setupMenu(type);

            TemplatesHidden = false;
        }

        #region Properties

        private ObservableCollection<TECMenuItem> _menu;
        public ObservableCollection<TECMenuItem> Menu
        {
            get { return _menu; }
            set
            {
                _menu = value;
                RaisePropertyChanged("Menu");
            }
        }

        private bool _templatesHidden;
        public bool TemplatesHidden
        {
            get { return _templatesHidden; }
            set
            {
                _templatesHidden = value;
                setHideTemplatesString();
            }
        }

        private TECMenuItem newMenuItem;
        public ICommand NewCommand
        {
            set
            {
                newMenuItem.Command = value;
            }
        }

        private TECMenuItem loadMenuItem;
        public ICommand LoadCommand
        {
            set
            {
                loadMenuItem.Command = value;
            }
        }

        private TECMenuItem saveMenuItem;
        public ICommand SaveCommand
        {
            set
            {
                saveMenuItem.Command = value;
            }
        }

        private TECMenuItem saveAsMenuItem;
        public ICommand SaveAsCommand
        {
            set
            {
                saveAsMenuItem.Command = value;
            }
        }

        private TECMenuItem exportProposalMenuItem;
        public ICommand ExportProposalCommand
        {
            set { exportProposalMenuItem.Command = value; }
        }

        private TECMenuItem exportPointsListMenuItem;
        public ICommand ExportPointsListCommand
        {
            set
            {
                exportProposalMenuItem.Command = value;
            }
        }

        private TECMenuItem undoMenuItem;
        public ICommand UndoCommand
        {
            set { undoMenuItem.Command = value; }
        }

        private TECMenuItem redoMenuItem;
        public ICommand RedoCommand
        {
            set
            {
                redoMenuItem.Command = value;
            }
        }

        private TECMenuItem toggleTemplatesMenuItem;
        public ICommand ToggleTemplatesCommand
        {
            set
            {
                toggleTemplatesMenuItem.Command = value;
            }
        }

        private TECMenuItem loadTemplatesMenuItem;
        public ICommand LoadTemplatesCommand
        {
            set
            {
                loadTemplatesMenuItem.Command = value;
            }
        }

        private TECMenuItem refreshTemplatesMenuItem;
        public ICommand RefreshTemplatesCommand
        {
            set
            {
                refreshTemplatesMenuItem.Command = value;
            }
        }
        #endregion

        #region Methods
        private void setupMenu(MenuType type)
        {
            Menu = new ObservableCollection<TECMenuItem>();

            SolidColorBrush lightTextBrush = (SolidColorBrush)Application.Current.Resources["LightTextBrush"];
            SolidColorBrush darkTextBrush = (SolidColorBrush)Application.Current.Resources["DarkTextBrush"];

            //File
            TECMenuItem FileMenu = new TECMenuItem("File", lightTextBrush);

            newMenuItem = new TECMenuItem("New", darkTextBrush);
            FileMenu.Items.Add(newMenuItem);

            loadMenuItem = new TECMenuItem("Load", darkTextBrush);
            FileMenu.Items.Add(loadMenuItem);

            saveMenuItem = new TECMenuItem("Save", darkTextBrush);
            FileMenu.Items.Add(saveMenuItem);

            saveAsMenuItem = new TECMenuItem("Save As", darkTextBrush);
            FileMenu.Items.Add(saveAsMenuItem);

            if (type != MenuType.TB)
            {
                //Export
                TECMenuItem ExportMenu = new TECMenuItem("Export", darkTextBrush);

                exportProposalMenuItem = new TECMenuItem("Proposal", darkTextBrush);
                ExportMenu.Items.Add(exportProposalMenuItem);

                exportPointsListMenuItem = new TECMenuItem("Points List", darkTextBrush);
                ExportMenu.Items.Add(exportPointsListMenuItem);

                FileMenu.Items.Add(ExportMenu);
            }

            Menu.Add(FileMenu);

            //Edit
            TECMenuItem EditMenu = new TECMenuItem("Edit", lightTextBrush);

            undoMenuItem = new TECMenuItem("Undo", darkTextBrush);
            EditMenu.Items.Add(undoMenuItem);

            redoMenuItem = new TECMenuItem("Redo", darkTextBrush);
            EditMenu.Items.Add(redoMenuItem);

            Menu.Add(EditMenu);

            //View
            if (type != MenuType.TB)
            {
                TECMenuItem ViewMenu = new TECMenuItem("View", lightTextBrush);

                toggleTemplatesMenuItem = new TECMenuItem("Show/Hide", darkTextBrush);
                ViewMenu.Items.Add(toggleTemplatesMenuItem);

                Menu.Add(ViewMenu);
            }

            //Templates
            TECMenuItem TemplatesMenu = new TECMenuItem("Templates", lightTextBrush);

            if (type != MenuType.TB)
            {
                loadTemplatesMenuItem = new TECMenuItem("Load", darkTextBrush);
                TemplatesMenu.Items.Add(loadTemplatesMenuItem);
            }

            refreshTemplatesMenuItem = new TECMenuItem("Refresh", darkTextBrush);
            TemplatesMenu.Items.Add(refreshTemplatesMenuItem);

            Menu.Add(TemplatesMenu);
        }

        private void setHideTemplatesString()
        {
            if (toggleTemplatesMenuItem != null)
            {
                if (TemplatesHidden)
                {
                    toggleTemplatesMenuItem.Name = "Show Templates";
                }
                else
                {
                    toggleTemplatesMenuItem.Name = "Hide Templates";
                }
            }
        }
        #endregion
    }
}