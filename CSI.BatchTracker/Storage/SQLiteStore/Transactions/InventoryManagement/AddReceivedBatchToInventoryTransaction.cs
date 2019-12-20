using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.Storage.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Storage.SQLiteStore.Transactions.InventoryManagement
{
    public sealed class AddReceivedBatchToInventoryTransaction : SQLiteDataSourceTransaction
    {
        SQLiteStoreContext store;
        ReceivedBatch receivedBatch;

        public AddReceivedBatchToInventoryTransaction(ReceivedBatch receivedBatch, SQLiteStoreContext store)
        {
            this.store = store;
            this.receivedBatch = receivedBatch;
        }

        public override void Execute()
        {
            if (BatchExistsInInventory())
            {
                Entity<InventoryBatch> entity = store.Results[0] as Entity<InventoryBatch>;
                entity.NativeModel.Quantity += receivedBatch.Quantity;

                ITransaction updater = new EditBatchInCurrentInventoryTransaction(entity, store);
                updater.Execute();
                return;
            }

            string query = "INSERT INTO InventoryBatches (ColorName, BatchNumber, ActivityDate, QtyOnHand) VALUES (?, ?, ?, ?)";

            List<object> parameters = new List<object>
            {
                receivedBatch.ColorName,
                receivedBatch.BatchNumber,
                receivedBatch.ActivityDate.FormatForDatabase(),
                receivedBatch.Quantity
            };

            store.ExecuteNonQuery(query, parameters);
        }

        bool BatchExistsInInventory()
        {
            string query = "SELECT * FROM InventoryBatches WHERE BatchNumber = ?";
            List<object> parameters = new List<object> { receivedBatch.BatchNumber };
            store.ExecuteReader(typeof(InventoryBatch), query, parameters);

            return store.Results.Count > 0;
        }
    }
}
