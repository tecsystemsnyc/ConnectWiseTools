using EstimatingLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingUtilitiesLibrary.Database
{
    public class DatabaseManager
    {
        private string path;

        public event Action<bool> SaveComplete;
        public event Action<TECScopeManager> LoadComplete;

        public DatabaseManager(string databasePath)
        {
            path = databasePath;
        }
        
        public bool Save(List<UpdateItem> updates)
        {
            DatabaseUpdater.Update(path, updates);
            return true;
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
                SaveComplete?.Invoke(true);
            };
            worker.RunWorkerAsync();
        }

        public bool New(TECScopeManager scopeManager)
        {
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
        public void AsyncNew(TECScopeManager scopeManager)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (s, e) =>
            {
                New(scopeManager);
            };
            worker.RunWorkerCompleted += (s, e) =>
            {
                SaveComplete?.Invoke(true);
            };
            worker.RunWorkerAsync();
        }

        public TECScopeManager Load()
        {
            DataTable versionMap = CSVReader.Read(Properties.Resources.VersionDefinition);
            if (DatabaseVersionManager.CheckAndUpdate(path, versionMap))
            {
                return DatabaseLoader.Load(path, true);
            }
            else
            {
                return DatabaseLoader.Load(path);
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
                if(e.Result is TECScopeManager scopeManager)
                {
                    LoadComplete?.Invoke(scopeManager);
                } else
                {
                    LoadComplete?.Invoke(null);
                }
            };
            worker.RunWorkerAsync();
        }
    }
}
