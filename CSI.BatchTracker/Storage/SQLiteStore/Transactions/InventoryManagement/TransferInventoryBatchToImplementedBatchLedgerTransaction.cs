using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using System.Collections.Generic;

namespace CSI.BatchTracker.Storage.SQLiteStore.Transactions.InventoryManagement
{
    public sealed class TransferInventoryBatchToImplementedBatchLedgerTransaction : SQLiteDataSourceTransaction
    {
        Entity<LoggedBatch> entity;
        SQLiteStoreContext store;

        public TransferInventoryBatchToImplementedBatchLedgerTransaction(Entity<LoggedBatch> entity, SQLiteStoreContext store)
        {
            this.entity = entity;
            this.store = store;
        }

        public override void Execute()
        {
            string query = string.Format(
                "INSERT INTO ImplementedBatches {0} VALUES {1}",
                "(ColorName, BatchNumber, ImplementationDate, ImplementingOperatorId)",
                "(?, ?, ?, ?)"
            );

            List<object> parameters = new List<object>()
            {
                entity.NativeModel.ColorName,
                entity.NativeModel.BatchNumber,
                entity.NativeModel.ActivityDate.FormatForDatabase(),
                GetReceivingOperatorId()
            };

            store.ExecuteNonQuery(query, parameters);

        }

        int GetReceivingOperatorId()
        {
            string query = "SELECT * FROM BatchOperators WHERE FirstName = ? AND LastName = ?";

            List<object> parameters = new List<object>()
            {
                entity.NativeModel.ImplementingOperator.FirstName,
                entity.NativeModel.ImplementingOperator.LastName
            };

            store.ExecuteReader(typeof(BatchOperator), query, parameters);
            Entity<BatchOperator> operatorEntity = store.Results[0] as Entity<BatchOperator>;

            return operatorEntity.SystemId;
        }
    }
}
