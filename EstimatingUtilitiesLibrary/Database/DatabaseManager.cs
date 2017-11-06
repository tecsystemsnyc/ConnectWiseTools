using DebugLibrary;
using EstimatingLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingUtilitiesLibrary.Database
{
    public class DatabaseManager<T> where T:TECScopeManager
    {
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
                try
                {
                    DatabaseUpdater.Update(path, updates);
                    return true;
                }
                catch (Exception ex) when (DebugBooleans.CatchSaveDelta)
                {
                    DebugHandler.LogError("Save delta failed. Exception: " + ex.Message);
                    return false;
                }
            }
            else
            {
                DebugHandler.LogError("Could not open file " + path + " File is open elsewhere.");
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
                DatabaseUpdater.Update(path, newStack);
                return true;
            }
            else
            {
                DebugHandler.LogError("Could not open file " + path + " File is open elsewhere.");
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
            DataTable versionMap = CSVReader.Read(Properties.Resources.VersionDefinition);
            if (DatabaseVersionManager.CheckAndUpdate(path, versionMap))
            {
                return DatabaseLoader.Load(path, true) as T;
            }
            else
            {
                return DatabaseLoader.Load(path) as T;
            }
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
    }
}
