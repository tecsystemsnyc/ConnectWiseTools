using GalaSoft.MvvmLight.CommandWpf;
using System;
using TECUserControlLibrary.ViewModels;

namespace TemplateBuilder.MVVM
{
    public class TemplatesMenuVM : MenuVM
    {
        public TemplatesMenuVM() : base()
        {
            setupMenu();
        }

        public void SetExportTemplatesCommand (Action execute, Func<bool> canExecute = null)
        {
            RelayCommand command = new RelayCommand(execute, forceNullToTrue(canExecute));
            setCommand("Templates Spreadsheet", command);
        }

        private void setupMenu()
        {
            //Export menu items
            addMenuItem("Templates Spreadsheet", parentItemName: "Export");
        }
    }
}
