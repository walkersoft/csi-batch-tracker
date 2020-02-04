using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.Storage.Contracts;
using CSI.BatchTracker.Storage.SQLiteStore.Transactions.RecordAquisition;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace CSI.BatchTracker.Storage.SQLiteStore
{
    public class SQLiteStoreContext
    {
        string databaseSourceFile;
        string connectionString;

        public List<IEntity> Results { get; private set; }

        public SQLiteStoreContext(string databaseSourceFile)
        {
            this.databaseSourceFile = databaseSourceFile;
            connectionString = string.Format("Data Source={0};Version=3;", this.databaseSourceFile);
        }

        public void ExecuteNonQuery(string query, List<object> parameters)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.Clear();

                    for (int i = 0; i < parameters.Count; i++)
                    {
                        string paramId = string.Format("@param{0}", i + 1);
                        command.Parameters.Add(new SQLiteParameter(paramId, parameters[i]));
                    }

                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        public void ExecuteReader(Type nativeModelType, string query, List<object> parameters)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.Clear();

                    for (int i = 0; i < parameters.Count; i++)
                    {
                        string paramId = string.Format("@param{0}", i + 1);
                        command.Parameters.Add(new SQLiteParameter(paramId, parameters[i]));
                    }

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        Results = ProcessReaderResults(reader, nativeModelType);
                    }                        
                }

                connection.Close();
            }
        }

        public void ExecuteReader(Type nativeModelType, string query)
        {
            ExecuteReader(nativeModelType, query, new List<object>());
        }

        List<IEntity> ProcessReaderResults(SQLiteDataReader reader, Type nativeModelType)
        {
            List<IEntity> entities = new List<IEntity>();

            if (nativeModelType == typeof(BatchOperator))
            {
                entities = ProcessBatchOperators(reader);
            }

            if (nativeModelType == typeof(InventoryBatch))
            {
                entities = ProcessInventoryBatches(reader);
            }

            if (nativeModelType == typeof(ReceivedBatch))
            {
                entities = ProcessReceivedBatches(reader);
            }

            if (nativeModelType == typeof(LoggedBatch))
            {
                entities = ProcessLoggedBatches(reader);
            }

            return entities;
        }

        List<IEntity> ProcessBatchOperators(SQLiteDataReader reader)
        {
            List<IEntity> entities = new List<IEntity>();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    BatchOperator batchOperator;

                    batchOperator = new BatchOperator(
                        reader.GetString(1),
                        reader.GetString(2)
                    );

                    Entity<BatchOperator> entity = new Entity<BatchOperator>(reader.GetInt32(0), batchOperator);
                    entities.Add(entity);
                }
            }

            return entities;
        }

        List<IEntity> ProcessInventoryBatches(SQLiteDataReader reader)
        {
            List<IEntity> entities = new List<IEntity>();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    InventoryBatch inventoryBatch;
                    inventoryBatch = new InventoryBatch(
                        reader.GetString(1),
                        reader.GetString(2),
                        DateTime.ParseExact(reader.GetString(3), "yyyy-MM-dd HH:mm:ss", null),
                        reader.GetInt32(4)
                    );

                    Entity<InventoryBatch> entity = new Entity<InventoryBatch>(reader.GetInt32(0), inventoryBatch);
                    entities.Add(entity);
                }
            }

            return entities;
        }

        List<IEntity> ProcessReceivedBatches(SQLiteDataReader reader)
        {
            Dictionary<int, Entity<BatchOperator>> batchOperatorEntities = GetBatchOperatorEntities();
            List<IEntity> entities = new List<IEntity>();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    ReceivedBatch receivedBatch;
                    int batchOperatorId = reader.GetInt32(6);

                    receivedBatch = new ReceivedBatch(
                        reader.GetString(1),
                        reader.GetString(2),
                        DateTime.ParseExact(reader.GetString(3), "yyyy-MM-dd HH:mm:ss", null),
                        reader.GetInt32(4),
                        reader.GetInt32(5),
                        batchOperatorEntities[batchOperatorId].NativeModel
                    );

                    Entity<ReceivedBatch> entity = new Entity<ReceivedBatch>(reader.GetInt32(0), receivedBatch);
                    entities.Add(entity);
                }
            }

            return entities;
        }

        List<IEntity> ProcessLoggedBatches(SQLiteDataReader reader)
        {
            Dictionary<int, Entity<BatchOperator>> batchOperatorEntities = GetBatchOperatorEntities();
            List<IEntity> entities = new List<IEntity>();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    LoggedBatch loggedBatch;
                    int batchOperatorId = reader.GetInt32(4);
                    string date = reader.GetString(3);

                    loggedBatch = new LoggedBatch(
                        reader.GetString(1),
                        reader.GetString(2),
                        DateTime.ParseExact(reader.GetString(3), "yyyy-MM-dd HH:mm:ss", null),
                        batchOperatorEntities[batchOperatorId].NativeModel
                    );

                    Entity<LoggedBatch> entity = new Entity<LoggedBatch>(reader.GetInt32(0), loggedBatch);
                    entities.Add(entity);
                }
            }

            return entities;
        }

        Dictionary<int, Entity<BatchOperator>> GetBatchOperatorEntities()
        {
            Dictionary<int, Entity<BatchOperator>> batchOperatorEntities = new Dictionary<int, Entity<BatchOperator>>();
            ITransaction finder = new ListBatchOperatorsTransaction(this);
            finder.Execute();

            foreach (IEntity operatorEntity in finder.Results)
            {
                Entity<BatchOperator> currentOperatorEntity = operatorEntity as Entity<BatchOperator>;
                batchOperatorEntities.Add(currentOperatorEntity.SystemId, currentOperatorEntity);
            }

            return batchOperatorEntities;
        }
    }
}
