﻿using DebugLibrary;
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
    public class BudgetVM : ViewModelBase
    {
        #region Properties
        private double _manualAdjustmentAmount;

        private TECBid _bid;
        public TECBid Bid
        {
            get { return _bid; }
            set
            {
                _bid = value;
                raiseBudgetChanges();
                setupBid(value);
            }
        }

        public ObservableCollection<TECSystem> BudgetedSystems
        {
            get
            {
                ObservableCollection<TECSystem> budgetedSystems = new ObservableCollection<TECSystem>();
                foreach (TECSystem system in Bid.Systems)
                {
                    if (system.BudgetUnitPrice >= 0)
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
                    if (system.BudgetUnitPrice < 0)
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
                return (ManualAdjustmentAmount / BudgetSubTotal) * 100;
            }
            set
            {
                _manualAdjustmentAmount = Math.Ceiling(BudgetSubTotal * (value / 100));
                raiseBudgetChanges();
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
                raiseBudgetChanges();
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

        #endregion 

        #region Constructors
        public BudgetVM(TECBid bid)
        {
            Bid = bid;
            setupBid(Bid);
            ExportBudgetCommand = new RelayCommand(ExportBudgetExecute);

            ManualAdjustmentPercentage = 0;
        }

        private void setupBid(TECBid bid)
        {
            bid.Systems.CollectionChanged += Systems_CollectionChanged;

            foreach (TECSystem system in bid.Systems)
            {
                registerSystem(system);
            }
        }
        #endregion

        #region Methods
        public void Refresh(TECBid bid)
        {
            Bid = bid;
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
                    writer.BudgetToCSV(Bid.Systems, ManualAdjustmentPercentage / 100);
                }
                else
                {
                    DebugHandler.LogError("Could not open file " + path + " File is open elsewhere.");
                }
                DebugHandler.LogDebugMessage("Saving budget to csv.");
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
                path = saveFileDialog.FileName;
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

        #endregion
    }
}