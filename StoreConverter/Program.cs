using System;
using System.IO;
using System.Collections.Generic;
using CSI.BatchTracker.Storage.MemoryStore;
using CSI.BatchTracker.Storage.SQLiteStore;
using System.Data.SQLite;

namespace StoreConverter
{
    class Program
    {
        static string usage = "Usage: StoreConverter.exe \"MemstoreName.dat\" \"NewSQLiteStore.sqlite3\"";
        static SQLiteDatabaseInstaller installer;

        static void Main(string[] args)
        {
            if (NotCorrectNumberOfArgs(args))
            {
                Console.WriteLine(string.Format("{0}\n{1}",
                    "ERROR: An absolute source memory store and destination SQLite store filenames are required.",
                    usage
                ));

                Environment.ExitCode = 1;
                return;
            }

            if (SourceStoreDoesNotExist(args[0]))
            {
                Console.WriteLine(string.Format("Source memory store does not exist at: {0}\n{1}", args[0], usage));
                Environment.ExitCode = 2;
                return;
            }

            if (DestinationStoreAlreadyExists(args[1]))
            {
                Console.WriteLine(string.Format("Destination SQLite store already present at: {0}\n{1}", args[1], usage));
                Environment.ExitCode = 3;
                return;
            }

            DoStoreConversion(args[0], args[1]);
        }

        private static bool DestinationStoreAlreadyExists(string destination)
        {
            return File.Exists(destination) == true;
        }

        private static bool SourceStoreDoesNotExist(string source)
        {
            return File.Exists(source) == false;
        }

        private static bool NotCorrectNumberOfArgs(string[] args)
        {
            return args.Length != 2;
        }

        private static SQLiteStoreContext GetEmptySQLiteStore(string destination)
        {
            installer = new SQLiteDatabaseInstaller
            {
                DatabaseFile = destination,
                ConnectionString = string.Format("Data Source={0};Version=3;", destination)
            };

            installer.CreateNewDatabase();
            SQLiteStoreContext context = new SQLiteStoreContext(destination);
            return context;
        }

        private static MemoryStoreContext GetPopulatedMemoryStore(string store)
        {
            MemoryStorePersistenceManager manager = new MemoryStorePersistenceManager(store);
            manager.LoadDataSource();
            return manager.Context;
        }

        private static void DoStoreConversion(string source, string destination)
        {
            MemoryStoreContext memstore;
            SQLiteStoreContext sqliteStore;
            Console.WriteLine("\nBeginning Store Conversion\n===========================");

            Console.Write("Reading memory store...");
            memstore = GetPopulatedMemoryStore(source);
            Console.WriteLine("done!");

            Console.Write("Creating new database...");
            sqliteStore = GetEmptySQLiteStore(destination);
            Console.WriteLine("done!");

            // -- BATCH OPERATORS IMPORT -- //
            Console.WriteLine(string.Format("\n--BATCH OPERATORS--\nCopying {0} batch operators from memory store.", memstore.BatchOperators.Count));

            string query = string.Format("INSERT INTO BatchOperators {0} VALUES{1}",
                "(SystemId, FirstName, LastName)",
                "(?, ?, ?)"
            );

            using (SQLiteConnection connection = new SQLiteConnection(installer.ConnectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    foreach (var entity in memstore.BatchOperators)
                    {
                        List<object> parameters = new List<object>
                        {
                            entity.Value.SystemId,
                            entity.Value.NativeModel.FirstName,
                            entity.Value.NativeModel.LastName
                        };

                        for (int i = 0; i < parameters.Count; i++)
                        {
                            string paramId = string.Format("@param{0}", i + 1);
                            command.Parameters.Add(new SQLiteParameter(paramId, parameters[i]));
                        }

                        command.ExecuteNonQuery();
                    }
                }

                string verification = "SELECT count(*) FROM BatchOperators";

                using (SQLiteCommand command = new SQLiteCommand(verification, connection))
                {
                    Console.WriteLine(string.Format("Updated batch operators table with {0} entries.\n",
                        Int32.Parse(command.ExecuteScalar().ToString()))
                    );
                }
            }

            // -- RECEIVED BATCHES IMPORT -- //
            Console.WriteLine(string.Format("--RECEIVED BATCHES--\nCopying {0} received batches from memory store.", memstore.ReceivingLedger.Count));

            query = string.Format("INSERT INTO ReceivedBatches {0} VALUES{1}",
                "(SystemId, ColorName, BatchNumber, ReceivingDate, QtyReceived, PONumber, ReceivingOperatorId)",
                "(?, ?, ?, ?, ?, ?, ?)"
            );

            using (SQLiteConnection connection = new SQLiteConnection(installer.ConnectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    foreach (var entity in memstore.ReceivingLedger)
                    {
                        int foreignSystemId = 0;

                        foreach (var operatorEntity in memstore.BatchOperators)
                        {
                            if (entity.Value.NativeModel.ReceivingOperator.FullName == operatorEntity.Value.NativeModel.FullName)
                            {
                                foreignSystemId = operatorEntity.Value.SystemId;
                            }
                        }

                        List<object> parameters = new List<object>
                        {
                            entity.Value.SystemId,
                            entity.Value.NativeModel.ColorName,
                            entity.Value.NativeModel.BatchNumber,
                            entity.Value.NativeModel.ActivityDate.FormatForDatabase(),
                            entity.Value.NativeModel.Quantity,
                            entity.Value.NativeModel.PONumber,
                            foreignSystemId,
                        };

                        for (int i = 0; i < parameters.Count; i++)
                        {
                            string paramId = string.Format("@param{0}", i + 1);
                            command.Parameters.Add(new SQLiteParameter(paramId, parameters[i]));
                        }

                        command.ExecuteNonQuery();
                    }
                }

                string verification = "SELECT count(*) FROM ReceivedBatches";

                using (SQLiteCommand command = new SQLiteCommand(verification, connection))
                {
                    Console.WriteLine(string.Format("Updated received batches table with {0} entries.\n",
                        Int32.Parse(command.ExecuteScalar().ToString()))
                    );
                }
            }

            // -- IMPLEMENTED BATCHES IMPORT -- //
            Console.WriteLine(string.Format("--IMPLEMENTED BATCHES--\nCopying {0} implemented batches from memory store.", memstore.ImplementedBatchLedger.Count));

            query = string.Format("INSERT INTO ImplementedBatches {0} VALUES{1}",
                "(SystemId, ColorName, BatchNumber, ImplementationDate, ImplementingOperatorId)",
                "(?, ?, ?, ?, ?)"
            );

            using (SQLiteConnection connection = new SQLiteConnection(installer.ConnectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    foreach (var entity in memstore.ImplementedBatchLedger)
                    {
                        int foreignSystemId = 0;

                        foreach (var operatorEntity in memstore.BatchOperators)
                        {
                            if (entity.Value.NativeModel.ImplementingOperator.FullName == operatorEntity.Value.NativeModel.FullName)
                            {
                                foreignSystemId = operatorEntity.Value.SystemId;
                            }
                        }

                        List<object> parameters = new List<object>
                        {
                            entity.Value.SystemId,
                            entity.Value.NativeModel.ColorName,
                            entity.Value.NativeModel.BatchNumber,
                            entity.Value.NativeModel.ActivityDate.FormatForDatabase(),
                            foreignSystemId,
                        };

                        for (int i = 0; i < parameters.Count; i++)
                        {
                            string paramId = string.Format("@param{0}", i + 1);
                            command.Parameters.Add(new SQLiteParameter(paramId, parameters[i]));
                        }

                        command.ExecuteNonQuery();
                    }
                }

                string verification = "SELECT count(*) FROM ImplementedBatches";

                using (SQLiteCommand command = new SQLiteCommand(verification, connection))
                {
                    Console.WriteLine(string.Format("Updated implemented batches table with {0} entries.\n",
                        Int32.Parse(command.ExecuteScalar().ToString()))
                    );
                }
            }

            // -- ACTIVE INVENTORY IMPORT -- //
            Console.WriteLine(string.Format("--INVENTORY BATCHES--\nCopying {0} inventory batches from memory store.", memstore.CurrentInventory.Count));

            query = string.Format("INSERT INTO InventoryBatches {0} VALUES{1}",
                "(SystemId, ColorName, BatchNumber, ActivityDate, QtyOnHand)",
                "(?, ?, ?, ?, ?)"
            );

            using (SQLiteConnection connection = new SQLiteConnection(installer.ConnectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    foreach (var entity in memstore.CurrentInventory)
                    {

                        List<object> parameters = new List<object>
                        {
                            entity.Value.SystemId,
                            entity.Value.NativeModel.ColorName,
                            entity.Value.NativeModel.BatchNumber,
                            entity.Value.NativeModel.ActivityDate.FormatForDatabase(),
                            entity.Value.NativeModel.Quantity
                        };

                        for (int i = 0; i < parameters.Count; i++)
                        {
                            string paramId = string.Format("@param{0}", i + 1);
                            command.Parameters.Add(new SQLiteParameter(paramId, parameters[i]));
                        }

                        command.ExecuteNonQuery();
                    }
                }

                string verification = "SELECT count(*) FROM InventoryBatches";

                using (SQLiteCommand command = new SQLiteCommand(verification, connection))
                {
                    Console.WriteLine(string.Format("Updated inventory batches table with {0} entries.\n",
                        Int32.Parse(command.ExecuteScalar().ToString()))
                    );
                }
            }

            Console.WriteLine("\nConversion completed. SQLite database can be found at: " + destination);
            Console.WriteLine("\n--END OF OPERATION--\n");

            Environment.ExitCode = 0;
        }
    }
}
