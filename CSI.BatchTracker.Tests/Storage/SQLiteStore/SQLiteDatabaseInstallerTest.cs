﻿using CSI.BatchTracker.Storage.SQLiteStore;
using NUnit.Framework;
using System.Data.SQLite;
using System.IO;

namespace CSI.BatchTracker.Tests.Storage.SQLiteStore
{
    [TestFixture]
    class SQLiteDatabaseInstallerTest
    {
        SQLiteDatabaseInstaller installer;
        string temporaryDatabaseFile;

        [SetUp]
        public void SetUp()
        {
            temporaryDatabaseFile = Path.GetTempPath() + "temp_database.sqlite3";
            installer = new SQLiteDatabaseInstaller
            {
                DatabaseFile = temporaryDatabaseFile,
                ConnectionString = string.Format("Data Source={0};Version=3;New=True;", temporaryDatabaseFile)
            };
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(temporaryDatabaseFile))
            {
                File.Delete(temporaryDatabaseFile);
            }
        }

        [Test]
        public void DatabaseInstallerCanCreatePhysicalDatabase()
        {
            Assert.DoesNotThrow(() => installer.CreateNewDatabase());
            Assert.True(File.Exists(temporaryDatabaseFile));
        }

        [Test]
        public void DatabaseInstallerWillNotCreatePhysicalDatabaseWithoutFileName()
        {
            installer.DatabaseFile = string.Empty;

            Assert.DoesNotThrow(() => installer.CreateNewDatabase());
            Assert.False(File.Exists(temporaryDatabaseFile));
        }

        [Test]
        public void NewDatabaseContainsTableForBatchOperators()
        {
            int expectedRowCount = 3;

            Assert.DoesNotThrow(() => installer.CreateNewDatabase());

            using (SQLiteConnection connection = new SQLiteConnection(installer.ConnectionString))
            {
                connection.Open();
                string query = "PRAGMA table_info(BatchOperators)";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        Assert.True(reader.HasRows);
                        int rows = 0;

                        while (reader.Read())
                        {
                            rows++;
                        }

                        Assert.AreEqual(expectedRowCount, rows);
                    }
                }

                connection.Close();
            }
        }

        [Test]
        public void NewDatabaseContainsTableForReceivedBatches()
        {
            int expectedRowCount = 7;

            Assert.DoesNotThrow(() => installer.CreateNewDatabase());

            using (SQLiteConnection connection = new SQLiteConnection(installer.ConnectionString))
            {
                connection.Open();
                string query = "PRAGMA table_info(ReceivedBatches)";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        Assert.True(reader.HasRows);
                        int rows = 0;

                        while (reader.Read())
                        {
                            rows++;
                        }

                        Assert.AreEqual(expectedRowCount, rows);
                    }
                }

                connection.Close();
            }
        }

        [Test]
        public void NewDatabaseContainsTableForInventoryBatches()
        {
            int expectedRowCount = 5;

            Assert.DoesNotThrow(() => installer.CreateNewDatabase());

            using (SQLiteConnection connection = new SQLiteConnection(installer.ConnectionString))
            {
                connection.Open();
                string query = "PRAGMA table_info(InventoryBatches)";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        Assert.True(reader.HasRows);
                        int rows = 0;

                        while (reader.Read())
                        {
                            rows++;
                        }

                        Assert.AreEqual(expectedRowCount, rows);
                    }
                }

                connection.Close();
            }
        }

        [Test]
        public void NewDatabaseContainsTableForImplementedBatches()
        {
            int expectedRowCount = 5;

            Assert.DoesNotThrow(() => installer.CreateNewDatabase());

            using (SQLiteConnection connection = new SQLiteConnection(installer.ConnectionString))
            {
                connection.Open();
                string query = "PRAGMA table_info(InventoryBatches)";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        Assert.True(reader.HasRows);
                        int rows = 0;

                        while (reader.Read())
                        {
                            rows++;
                        }

                        Assert.AreEqual(expectedRowCount, rows);
                    }
                }

                connection.Close();
            }
        }
    }
}
