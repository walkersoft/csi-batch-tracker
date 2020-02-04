using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.Storage.Contracts;
using System.Collections.Generic;

namespace CSI.BatchTracker.Storage.SQLiteStore.Transactions.InventoryManagement
{
    public sealed class AddReceivedBatchToInventoryTransaction : SQLiteDataSourceTransaction
    {
        SQLiteStoreContext store;
        Entity<InventoryBatch> entity;

        public AddReceivedBatchToInventoryTransaction(Entity<InventoryBatch> entity, SQLiteStoreContext store)
        {
            this.store = store;
            this.entity = entity;
        }

        public override void Execute()
        {
            if (BatchExistsInInventory())
            {
                Entity<InventoryBatch> entity = store.Results[0] as Entity<InventoryBatch>;
                entity.NativeModel.Quantity += this.entity.NativeModel.Quantity;

                ITransaction updater = new EditBatchInCurrentInventoryTransaction(entity, store);
                updater.Execute();
                return;
            }

            string query = "INSERT INTO InventoryBatches (ColorName, BatchNumber, ActivityDate, QtyOnHand) VALUES (?, ?, ?, ?)";

            List<object> parameters = new List<object>
            {
                entity.NativeModel.ColorName,
                entity.NativeModel.BatchNumber,
                entity.NativeModel.ActivityDate.FormatForDatabase(),
                entity.NativeModel.Quantity
            };

            store.ExecuteNonQuery(query, parameters);
        }

        bool BatchExistsInInventory()
        {
            string query = "SELECT * FROM InventoryBatches WHERE BatchNumber = ?";
            List<object> parameters = new List<object> { entity.NativeModel.BatchNumber };
            store.ExecuteReader(typeof(InventoryBatch), query, parameters);

            return store.Results.Count > 0;
        }
    }
}
