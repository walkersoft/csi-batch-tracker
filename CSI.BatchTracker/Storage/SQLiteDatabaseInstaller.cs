using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Storage
{
    public class SQLiteDatabaseInstaller
    {
        public string DatabaseFile { get; set; }
        public string ConnectionString { get; set; }

        public void CreateNewDatabase()
        {
            if (string.IsNullOrEmpty(DatabaseFile) == false)
            {
                SQLiteConnection.CreateFile(DatabaseFile);
                CreateBatchOperatorsTable();
            }
        }

        void CreateBatchOperatorsTable()
        {
            string query = @"
                CREATE TABLE BatchOperators(
                    SystemId  INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
                    FirstName TEXT NOT NULL,
                    LastName  TEXT NOT NULL
                )
            ";

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(query.Trim(), connection))
                {
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }
    }
}
