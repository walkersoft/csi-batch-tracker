using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using System.Collections.Generic;

namespace CSI.BatchTracker.Storage.SQLiteStore.Transactions.BatchOperators
{
    public sealed class AddBatchOperatorTransaction : SQLiteDataSourceTransaction
    {
        SQLiteStoreContext store;
        Entity<BatchOperator> entity;

        public AddBatchOperatorTransaction(Entity<BatchOperator> entity, SQLiteStoreContext store)
        {
            this.entity = entity;
            this.store = store;
        }

        public override void Execute()
        {
            string query = "INSERT INTO BatchOperators (FirstName, LastName) VALUES(?, ?)";
            List<object> parameters = new List<object>()
            {
                entity.NativeModel.FirstName,
                entity.NativeModel.LastName
            };

            store.ExecuteNonQuery(query, parameters);
        }
    }
}
