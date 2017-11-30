using EstimatingLibrary;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace EstimatingUtilitiesLibrary.Database
{
    public class DatabaseManager<T> where T:TECScopeManager
    {
        static private Logger logger = LogManager.GetCurrentClassLogger();

        private string path;

        public event Action<bool> SaveComplete;
        public event Action<T> LoadComplete;
        public bool IsBusy = false;
        
        public DatabaseManager(string databasePath)
        {
            path = databasePath;
        }
        
        public bool Save(List<UpdateItem> updates)
        {
            if (!UtilitiesMethods.IsFileLocked(path))
            {
                return catchOnRelease("Save delta failed. Exception: ", () =>
                {
                    bool success = DatabaseUpdater.Update(path, updates);
                    if (!success)
                    {
                        MessageBox.Show("Some items might not have saved properly, check logs for more details.");
                    }
                });
            }
            else
            {
                logger.Error("Could not open file " + path + " File is open elsewhere.");
                return false;
            }
            
        }
        public void AsyncSave(List<UpdateItem> updates)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (s, e) =>
            {
                Save(updates);
            };
            worker.RunWorkerCompleted += (s, e) =>
            {
                notifySaveComplete(true);
            };
            IsBusy = true;
            worker.RunWorkerAsync();
        }

        public bool New(TECScopeManager scopeManager)
        {
            if (!UtilitiesMethods.IsFileLocked(path))
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                if (scopeManager is TECBid)
                {
                    DatabaseGenerator.CreateBidDatabase(path);
                }
                else if (scopeManager is TECTemplates)
                {
                    DatabaseGenerator.CreateTemplateDatabase(path);
                }
                else
                {
                    throw new Exception("Generator can only reate bid or template DBs");
                }
                List<UpdateItem> newStack = DatabaseNewStacker.NewStack(scopeManager);
                bool success =DatabaseUpdater.Update(path, newStack);
                if (!success)
                {
                    MessageBox.Show("Not all items saved properly, check logs for more details.");
                }
                return true;
            }
            else
            {
                logger.Error("Could not open file " + path + " File is open elsewhere.");
                return false;
            }
            
        }
        public void AsyncNew(TECScopeManager scopeManager)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (s, e) =>
            {
                New(scopeManager);
            };
            worker.RunWorkerCompleted += (s, e) =>
            {
                notifySaveComplete(true);
            };
            IsBusy = true;
            worker.RunWorkerAsync();
        }

        public T Load()
        {
            string appFolder = "EstimateBuilder";
            if(Path.GetExtension(path) == ".tdb")
            {
                appFolder = "TemplateBuilder";
            }
            if(!File.Exists(String.Format("{0}\\{1}\\{2}",
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                appFolder,
                "backups"))){
                Directory.CreateDirectory(String.Format("{0}\\{1}\\{2}",
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                appFolder,
                "backups"));
            }

            string backupPath = String.Format("{0}\\{1}\\{2}\\{3} {4}{5}",
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                appFolder,
                "backups",
                Path.GetFileNameWithoutExtension(path),
                String.Format("{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now),
                Path.GetExtension(path));
            File.Copy(path, backupPath);
            DataTable versionMap = CSVReader.Read(Properties.Resources.VersionDefinition);

            bool needsSave;
            TECScopeManager scopeManager;
            if (DatabaseVersionManager.CheckAndUpdate(path, versionMap))
            {
                (scopeManager, needsSave) = DatabaseLoader.Load(path, true);
            }
            else
            {
                (scopeManager, needsSave) = DatabaseLoader.Load(path);
            }

            if (needsSave)
            {
                New(scopeManager);
            }

            return scopeManager as T;
        }
        public void AsyncLoad()
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (s, e) =>
            {
                e.Result = Load();
            };
            worker.RunWorkerCompleted += (s, e) =>
            {
                if(e.Result is T scopeManager)
                {
                    notifyLoadComplete(scopeManager);
                } else
                {
                    notifyLoadComplete(null);
                }
            };
            IsBusy = true;
            worker.RunWorkerAsync();
        }

        private void notifySaveComplete(bool success)
        {
            IsBusy = false;
            SaveComplete?.Invoke(success);
        }
        private void notifyLoadComplete(T loaded)
        {
            IsBusy = false;
            LoadComplete?.Invoke(loaded);
        }

        private bool catchOnRelease(string message, Action action)
        {
#if DEBUG
            action();
            return true;
#else
            try
            {
                action();
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(message + ex.Message);
                return false;
            }
#endif

        }
    }
}
