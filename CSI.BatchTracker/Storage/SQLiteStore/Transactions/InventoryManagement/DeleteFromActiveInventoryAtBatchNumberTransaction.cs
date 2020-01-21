using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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