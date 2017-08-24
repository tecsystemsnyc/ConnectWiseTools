﻿using EstimatingLibrary;
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
            try
            {
                DatabaseUpdater.Update(path, updates);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool New(TECScopeManager scopeManager)
        {
            try
            {
                DatabaseGenerator.CreateBidDatabase(path);
                List<UpdateItem> newStack = DatabaseNewStacker.NewStack(scopeManager);
                DatabaseUpdater.Update(path, newStack);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public TECScopeManager Load()
        {
            try
            {
                DataTable versionMap = CSVReader.Read(Properties.Resources.VersionDefinition);
                DatabaseVersionManager.CheckAndUpdate(path, versionMap);
                return DatabaseLoader.Load(path);
            }
            catch
            {
                return null; 
            }
        }
    }
}