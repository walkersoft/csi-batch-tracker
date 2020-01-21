using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Storage.SQLiteStore.Transactions.InventoryManagement
{
    public sealed class UndoImplementedBatchCommittedToLedgerTransaction : SQLiteDataSourceTransaction
    {
        SQLiteStoreContext store;
        Entity<LoggedBatch> loggedEntity;
        Entity<InventoryBatch> inventoryEntity;
        AddReceivedBatchToInventoryTransaction inventoryAdder;

        public UndoImplementedBatchCommittedToLedgerTransaction(Entity<LoggedBatch> entity, SQLiteStoreContext store)
        {
            this.loggedEntity = entity;
            this.store = store;
        }

        public override void Execute()
        {
            string query = "DELETE FROM ImplementedBatches WHERE SystemId = ?";
            List<object> parameters = new List<object>() { loggedEntity.SystemId };
            inventoryEntity = RetainLoggedEntityAsInventoryEntity();
            store.ExecuteNonQuery(query, parameters);
            RecommitToInventory();
        }

        Entity<InventoryBatch> RetainLoggedEntityAsInventoryEntity()
        {
            return new Entity<InventoryBatch>(
                new InventoryBatch(
                    loggedEntity.NativeModel.ColorName,
                    loggedEntity.NativeModel.BatchNumber,
                    loggedEntity.NativeModel.ActivityDate,
                    1
                )
            );
        }

        void RecommitToInventory()
        {
            inventoryAdder = new AddReceivedBatchToInventoryTransaction(inventoryEntity, store);
            inventoryAdder.Execute();
        }
    }
}
