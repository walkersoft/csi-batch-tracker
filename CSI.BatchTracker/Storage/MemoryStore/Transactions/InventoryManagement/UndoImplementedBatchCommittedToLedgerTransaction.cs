using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;

namespace CSI.BatchTracker.Storage.MemoryStore.Transactions.InventoryManagement
{
    public class UndoImplementedBatchCommittedToLedgerTransaction : MemoryDataSourceTransaction
    {
        MemoryStoreContext store;
        Entity<LoggedBatch> loggedEntity;
        Entity<InventoryBatch> inventoryEntity;
        AddReceivedBatchToInventoryTransaction inventoryAdder;

        public UndoImplementedBatchCommittedToLedgerTransaction(Entity<LoggedBatch> entity, MemoryStoreContext store)
        {
            this.loggedEntity = entity;
            this.store = store;
        }

        public override void Execute()
        {
            inventoryEntity = RetainLoggedEntityAsInventoryEntity();
            store.ImplementedBatchLedger.Remove(loggedEntity.SystemId);
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
