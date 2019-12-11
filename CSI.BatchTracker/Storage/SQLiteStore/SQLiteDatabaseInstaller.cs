﻿using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Storage.SQLiteStore
{
    public class SQLiteDatabaseInstaller
    {
        public string DatabaseFile { get; set; }
        public string ConnectionString { get; set; }

        string[] tableSchemas = new string[4];

        public void CreateNewDatabase()
        {
            if (string.IsNullOrEmpty(DatabaseFile) == false)
            {
                SQLiteConnection.CreateFile(DatabaseFile);
                ExecuteTableSchemaQueries();
            }
        }

        void CreateTableSchemas()
        {
            tableSchemas[0] = @"
                CREATE TABLE BatchOperators(
                    SystemId  INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
                    FirstName TEXT NOT NULL,
                    LastName TEXT NOT NULL
                )
            ";

            tableSchemas[1] = @"
                CREATE TABLE ReceivedBatches(
                    SystemId  INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
                    ColorName TEXT NOT NULL,
                    BatchNumber TEXT NOT NULL,
                    ReceivingDate TEXT NOT NULL,
                    QtyReceived INTEGER NOT NULL,
                    PONumber INTEGER NOT NULL,
                    ReceivingOperatorId INTEGER NOT NULL,
                    FOREIGN KEY(ReceivingOperatorId) REFERENCES BatchOperators(SystemId)
                )
            ";

            tableSchemas[2] = @"
                CREATE TABLE InventoryBatches(
                    SystemId  INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
                    ColorName TEXT NOT NULL,
                    BatchNumber TEXT NOT NULL,
                    ActivityDate TEXT NOT NULL,
                    QtyOnHand INTEGER NOT NULL
                )
            ";

            tableSchemas[3] = @"
                CREATE TABLE ImplementedBatches(
                    SystemId  INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
                    ColorName TEXT NOT NULL,
                    BatchNumber TEXT NOT NULL,
                    ImplementationDate TEXT NOT NULL,
                    ImplementingOperatorId INTEGER NOT NULL,
                    FOREIGN KEY(ImplementingOperatorId) REFERENCES BatchOperators(SystemId)
                )
            ";
        }

        void ExecuteTableSchemaQueries()
        {
            CreateTableSchemas();

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                for (int i = 0; i < tableSchemas.Length; i++)
                {
                    using (SQLiteCommand command = new SQLiteCommand(tableSchemas[i].Trim(), connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }                    

                connection.Close();
            }
        }
    }
}