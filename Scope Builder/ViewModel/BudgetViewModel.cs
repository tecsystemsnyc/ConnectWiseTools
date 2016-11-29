using EstimatingLibrary;
using EstimatingUtilitiesLibrary;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Scope_Builder.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class BudgetViewModel : ViewModelBase
    {

        private double _manualAdjustmentAmount;

        private ObservableCollection<TECSystem> _systems;

        public ObservableCollection<TECSystem> Systems
        {
            get { return _systems; }
            set
            {
                _systems = value;
                RaisePropertyChanged("Systems");
                RaisePropertyChanged("BudgetedSystems");
                RaisePropertyChanged("UnbudgetedSystems");
                RaisePropertyChanged("BudgetSubTotal");
                RaisePropertyChanged("TotalPrice");
            }
        }

        public ObservableCollection<TECSystem> BudgetedSystems
        {
            get
            {
                ObservableCollection<TECSystem> budgetedSystems = new ObservableCollection<TECSystem>();
                foreach (TECSystem system in _systems)
                {
                    if (system.PriceWithEquipment >= 0)
                    {
                        budgetedSystems.Add(system);
                    }
                }
                return budgetedSystems;
            }
        }

        public ObservableCollection<TECSystem> UnbudgetedSystems
        {
            get
            {
                ObservableCollection<TECSystem> budgetedSystems = new ObservableCollection<TECSystem>();
                foreach (TECSystem system in _systems)
                {
                    if (system.PriceWithEquipment < 0)
                    {
                        budgetedSystems.Add(system);
                    }
                }
                return budgetedSystems;
            }
        }

        public double BudgetSubTotal
        {
            get
            {
                double subtotal = 0;
                foreach (TECSystem system in BudgetedSystems)
                {
                    subtotal += system.TotalBudgetPrice;
                }
                return subtotal;
            }
        }

        public double ManualAdjustmentPercentage
        {
            get
            {
                return (ManualAdjustmentAmount/BudgetSubTotal)*100;
            }
            set
            {
                _manualAdjustmentAmount = Math.Ceiling(BudgetSubTotal*(value/100));
                RaisePropertyChanged("ManualAdjustmentPercentage");
                RaisePropertyChanged("ManualAdjustmentAmount");
                RaisePropertyChanged("TotalPrice");
            }
        }

        public double ManualAdjustmentAmount
        {
            get
            {
                return _manualAdjustmentAmount;
            }
            set
            {
                _manualAdjustmentAmount = Math.Ceiling(value);
                RaisePropertyChanged("ManualAdjustmentPercentage");
                RaisePropertyChanged("ManualAdjustmentAmount");
                RaisePropertyChanged("TotalPrice");
            }
        }

        public double TotalPrice
        {
            get
            {
                if (BudgetSubTotal > 0)
                {
                    return Math.Ceiling(BudgetSubTotal * (1 + (ManualAdjustmentPercentage / 100)));
                }
                else
                {
                    return ManualAdjustmentAmount;
                }
                
            }
        }
        
        public ICommand ExportBudgetCommand { get; private set; }

        public BudgetViewModel()
        {
            Systems = new ObservableCollection<TECSystem>();
            MessengerInstance.Register<GenericMessage<ObservableCollection<TECSystem>>>(this, PopulateSystems);
            ExportBudgetCommand = new RelayCommand(ExportBudgetExecute);

            ManualAdjustmentPercentage = 0;
        }

        public void PopulateSystems(GenericMessage<ObservableCollection<TECSystem>> genericMessage)
        {
            Systems = genericMessage.Content;
        }

        public void ExportBudgetExecute()
        {
            string path = getCSVSavePath();

            if (path != null)
            {
                if (!UtilitiesMethods.IsFileLocked(path))
                {
                    CSVWriter writer = new CSVWriter(path);
                    writer.BudgetToCSV(Systems, ManualAdjustmentPercentage/100);
                }
                else
                {
                    string message = "File is open elsewhere";
                    MessageBox.Show(message);
                }
                Console.WriteLine("Finished saving budget CSV.");
            }
        }

        private string getCSVSavePath()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            saveFileDialog.Filter = "Comma Separated Values Files (*.csv)|*.csv";
            saveFileDialog.DefaultExt = "csv";
            saveFileDialog.AddExtension = true;

            string path = null;

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    path = saveFileDialog.FileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Cannot save in this location. Original error: " + ex.Message);
                }
            }

            return path;
        }
    }
}