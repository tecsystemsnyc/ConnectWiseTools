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

namespace TECUserControlLibrary.ViewModels
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

        private TECBid _bid;
        public TECBid Bid
        {
            get { return _bid; }
            set
            {
                _bid = value;
                RaisePropertyChanged("Bid");
            }
        }

        public ObservableCollection<TECSystem> BudgetedSystems
        {
            get
            {
                ObservableCollection<TECSystem> budgetedSystems = new ObservableCollection<TECSystem>();
                foreach (TECSystem system in Bid.Systems)
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
                foreach (TECSystem system in Bid.Systems)
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

        public BudgetViewModel(TECBid bid)
        {
            Bid = bid;
            ExportBudgetCommand = new RelayCommand(ExportBudgetExecute);

            bid.Systems.CollectionChanged += Systems_CollectionChanged;

            foreach (TECSystem system in bid.Systems)
            {
                registerSystem(system);
            }

            ManualAdjustmentPercentage = 0;
        }

        private void Systems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            raiseBudgetChanges();
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (object item in e.NewItems)
                {
                    registerSystem(item as TECSystem);
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    (item as TECSystem).PropertyChanged -= System_PropertyChanged;
                }
            }
        }

        private void registerSystem(TECSystem system)
        {
            system.PropertyChanged += System_PropertyChanged;
        }

        private void System_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "TotalBudgetPrice")
            {
                raiseBudgetChanges();
            }
        }

        public void ExportBudgetExecute()
        {
            string path = getCSVSavePath();

            if (path != null)
            {
                if (!UtilitiesMethods.IsFileLocked(path))
                {
                    CSVWriter writer = new CSVWriter(path);
                    writer.BudgetToCSV(Bid.Systems, ManualAdjustmentPercentage/100);
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

        private void raiseBudgetChanges()
        {
            RaisePropertyChanged("BudgetedSystems");
            RaisePropertyChanged("UnbudgetedSystems");
            RaisePropertyChanged("BudgetSubTotal");
            RaisePropertyChanged("TotalPrice");
        }
    }
}