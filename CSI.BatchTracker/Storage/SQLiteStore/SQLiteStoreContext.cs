using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.Storage.Contracts;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            string type = nativeModelType.ToString().Split('.').Last();

            switch (type)
            {
                case "BatchOperator":
                    entities = ProcessBatchOperators(reader);
                    break;
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
    }
}
