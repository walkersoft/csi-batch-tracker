using System.Collections.Generic;

namespace CSI.BatchTracker.Storage.SQLiteStore.Transactions.InventoryManagement
{
    public sealed class DeleteFromActiveInventoryAtBatchNumberTransaction : SQLiteDataSourceTransaction
    {
        SQLiteStoreContext store;
        string batchNumber;

        public DeleteFromActiveInventoryAtBatchNumberTransaction(string batchNumber, SQLiteStoreContext store)
        {
            this.store = store;
            this.batchNumber = batchNumber;
        }

        public override void Execute()
        {
            string query = "DELETE FROM InventoryBatches WHERE BatchNumber = ?";
            store.ExecuteNonQuery(query, new List<object>() { batchNumber });
        }
    }
}