using EstimatingLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingUtilitiesLibrary.Database
{
    public class DatabaseManager
    {
        private string path;

        public DatabaseManager(string databasePath)
        {
            path = databasePath;
        }
        
        public bool Save(List<UpdateItem> updates)
        {
            DatabaseUpdater.Update(path, updates);
            return true;
        }
        public bool New(TECScopeManager scopeManager)
        {
            if(scopeManager is TECBid)
            {
                DatabaseGenerator.CreateBidDatabase(path);
            } else if (scopeManager is TECTemplates)
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
    }
}
