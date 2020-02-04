using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using System.Collections.Generic;

namespace CSI.BatchTracker.Storage.SQLiteStore.Transactions.InventoryManagement
{
    public class AddReceivedBatchToReceivingLedgerTransaction : SQLiteDataSourceTransaction
    {
        SQLiteStoreContext store;
        Entity<ReceivedBatch> entity;

        public AddReceivedBatchToReceivingLedgerTransaction(Entity<ReceivedBatch> entity, SQLiteStoreContext store)
        {
            this.store = store;
            this.entity = entity;
        }

        public override void Execute()
        {
            string query = string.Format(
                "INSERT INTO ReceivedBatches {0} VALUES {1}",
                "(ColorName, BatchNumber, ReceivingDate, QtyReceived, PONumber, ReceivingOperatorId)",
                "(?, ?, ?, ?, ?, ?)"
            );

            List<object> parameters = new List<object>()
            {
                entity.NativeModel.ColorName,
                entity.NativeModel.BatchNumber,
                entity.NativeModel.ActivityDate.FormatForDatabase(),
                entity.NativeModel.Quantity,
                entity.NativeModel.PONumber,
                GetReceivingOperatorId()
            };

            store.ExecuteNonQuery(query, parameters);
        }

        int GetReceivingOperatorId()
        {
            string query = "SELECT * FROM BatchOperators WHERE FirstName = ? AND LastName = ?";

            List<object> parameters = new List<object>()
            {
                entity.NativeModel.ReceivingOperator.FirstName,
                entity.NativeModel.ReceivingOperator.LastName
            };

            store.ExecuteReader(typeof(BatchOperator), query, parameters);
            Entity<BatchOperator> operatorEntity = store.Results[0] as Entity<BatchOperator>;

            return operatorEntity.SystemId;
        }
    }
}
