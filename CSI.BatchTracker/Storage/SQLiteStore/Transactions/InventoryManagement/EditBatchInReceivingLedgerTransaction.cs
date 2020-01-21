using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Storage.SQLiteStore.Transactions.InventoryManagement
{
    public sealed class EditBatchInReceivingLedgerTransaction : SQLiteDataSourceTransaction
    {
        SQLiteStoreContext store;
        Entity<ReceivedBatch> entity;

        public EditBatchInReceivingLedgerTransaction(Entity<ReceivedBatch> entity, SQLiteStoreContext store)
        {
            this.store = store;
            this.entity = entity;
        }

        public override void Execute()
        {
            string query = string.Format(
                "UPDATE ReceivedBatches SET {0} WHERE {1}",
                "ColorName = ?, BatchNumber = ?, ReceivingDate = ?, QtyReceived = ?, PONumber = ?, ReceivingOperatorId = ?",
                "SystemId = ?"
            );

            List<object> parameters = new List<object>()
            {
                entity.NativeModel.ColorName,
                entity.NativeModel.BatchNumber,
                entity.NativeModel.ActivityDate.FormatForDatabase(),
                entity.NativeModel.Quantity,
                entity.NativeModel.PONumber,
                GetBatchOperatorId(),
                entity.SystemId
            };
        }

        int GetBatchOperatorId()
        {
            string query = "SELECT * FROM BatchOperators WHERE FirstName = ? AND LastName = ?";
            List<object> parameters = new List<object>()
            {
                entity.NativeModel.ReceivingOperator.FirstName,
                entity.NativeModel.ReceivingOperator.LastName
            };

            store.ExecuteReader(typeof(BatchOperator), query, parameters);
            Entity<BatchOperator> batchOperator = store.Results[0] as Entity<BatchOperator>;

            return batchOperator.SystemId;
        }
    }
}
