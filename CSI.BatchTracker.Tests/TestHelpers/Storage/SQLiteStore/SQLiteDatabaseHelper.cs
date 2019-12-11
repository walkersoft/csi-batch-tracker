using CSI.BatchTracker.Storage.SQLiteStore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Tests.TestHelpers.Storage.SQLiteStore
{
    public class SQLiteDatabaseHelper
    {
        SQLiteDatabaseInstaller installer;

        public string DatabaseFile
        {
            get { return installer.DatabaseFile; }
        }

        public SQLiteDatabaseHelper()
        {
            string databaseFile = Path.GetTempPath() + "temp_database.sqlite3";
            string connectionString = string.Format("Data Source={0};Version=3;", databaseFile);

            installer = new SQLiteDatabaseInstaller
            {
                DatabaseFile = databaseFile,
                ConnectionString = connectionString
            };
        }

        public void CreateTestDatabase()
        {
            installer.CreateNewDatabase();
        }

        public void DestroyTestDatabase()
        {
            if (File.Exists(installer.DatabaseFile))
            {
                File.Delete(installer.DatabaseFile);
            }
        }
    }
}
