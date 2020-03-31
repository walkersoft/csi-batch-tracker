using CSI.BatchTracker.Storage.SQLiteStore;
using System.IO;

namespace CSI.BatchTracker.Tests.TestHelpers.Storage.SQLiteStore
{
    internal class SQLiteDatabaseHelper
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
            File.Delete(installer.DatabaseFile);
        }
    }
}
