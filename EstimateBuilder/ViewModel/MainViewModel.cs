using GalaSoft.MvvmLight;
using EstimateBuilder.Model;
using EstimatingLibrary;
using System.IO;
using System;
using EstimatingUtilitiesLibrary;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Deployment.Application;
using System.ComponentModel;
using TECUserControlLibrary.ViewModels;

namespace EstimateBuilder.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class MainViewModel : BidEditorBase
    {

        public MainViewModel()
        {

            if (ApplicationDeployment.IsNetworkDeployed)
            {
                Version = "Version " + ApplicationDeployment.CurrentDeployment.CurrentVersion;
            }
            else
            {
                Version = "Undeployed Version";
            }

            LoadDrawingCommand = new RelayCommand(LoadDrawingExecute);


            pointCSVDirectoryPath = Properties.Settings.Default.PointCSVDirectoryPath;
            scopeDirectoryPath = Properties.Settings.Default.ScopeDirectoryPath;
            documentDirectoryPath = Properties.Settings.Default.DocumentDirectoryPath;

            MessengerInstance.Register<NotificationMessage>(this, processNotification);
            MessengerInstance.Register<NotificationMessage<String>>(this, processNotificationInformation);

        }

        #region Properties
        //Potentially needed in bid setter: MessengerInstance.Send(new GenericMessage<TECBid>(Bid));


        #region Command Properties
        public ICommand LoadDrawingCommand { get; private set; }


        #endregion Command Properties
        #endregion Properties

        #region Methods

        #region Commands Methods
        private void LoadDrawingExecute()
        {
            string path = getLoadDrawingsPath();

            if (path != null)
            {
                Properties.Settings.Default.DrawingDirectoryPath = Path.GetDirectoryName(path);

                if (!UtilitiesMethods.IsFileLocked(path))
                {
                    CurrentStatusText = "Loading Drawings...";
                    var worker = new BackgroundWorker();

                    worker.DoWork += (s, e) => {
                        TECDrawing drawing = PDFConverter.convertPDFToDrawing(path);
                        e.Result = drawing;
                    };
                    worker.RunWorkerCompleted += (s, e) =>
                    {
                        if (e.Result is TECDrawing)
                        {
                            Bid.Drawings.Add((TECDrawing)e.Result);
                            CurrentStatusText = "Done.";
                            string message = "Drawings have finished loading.";
                            MessageBox.Show(message);
                        }
                        else
                        {
                            Console.WriteLine("Load Drawings Failed");
                        }
                    };
                    worker.RunWorkerAsync();
                }
                else
                {
                    string message = "File is open elsewhere.";
                    MessageBox.Show(message);
                }
            }
        }
        #endregion Commands Methods

        #region Helper Methods
        private void getVersion()
        {
            if (ApplicationDeployment.IsNetworkDeployed)
            { Version = "Version " + ApplicationDeployment.CurrentDeployment.CurrentVersion; }
            else
            { Version = "Undeployed Version"; }
        }
        private string getLoadDrawingsPath()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (Properties.Settings.Default.TemplateDirectoryPath != null)
            {
                openFileDialog.InitialDirectory = Properties.Settings.Default.DrawingDirectoryPath;
            }
            else
            {
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }

            openFileDialog.Filter = "PDF files (*.pdf)|*.pdf";
            openFileDialog.DefaultExt = "pdf";
            openFileDialog.AddExtension = true;

            string path = null;

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    path = openFileDialog.FileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Cannot load this file. Original error: " + ex.Message);
                }
            }

            return path;
        }
        #endregion Helper Methods

        private void processNotification(NotificationMessage message)
        {
            if (message.Notification == "ScopeEditorViewModelLoaded")
            {
                Console.WriteLine("Scope Editor View Model Loaded");
                MessengerInstance.Send<GenericMessage<TECTemplates>>(new GenericMessage<TECTemplates>(Templates));
                MessengerInstance.Send<GenericMessage<TECBid>>(new GenericMessage<TECBid>(Bid));
            }
            else if(message.Notification == "LaborViewModelLoaded")
            {
                Console.WriteLine("Labor View Model Loaded");
                MessengerInstance.Send<GenericMessage<TECBid>>(new GenericMessage<TECBid>(Bid));
            }
            else if (message.Notification == "DrawingViewModelLoaded")
            {
                Console.WriteLine("Drawing View Model Loaded");
                MessengerInstance.Send<GenericMessage<TECBid>>(new GenericMessage<TECBid>(Bid));
            }
            else if (message.Notification == "ReviewViewModelLoaded")
            {
                Console.WriteLine("Review View Model Loaded");
                MessengerInstance.Send<GenericMessage<TECBid>>(new GenericMessage<TECBid>(Bid));
            }
        }

        private void processNotificationInformation(NotificationMessage<String> message)
        {
            if (message.Notification == "StatusUpdate")
            {
                CurrentStatusText = message.Content;
            }
        }
        

        #endregion Methods
    }
}