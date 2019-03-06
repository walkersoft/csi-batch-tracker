using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;

namespace CSI.BatchTracker.Storage.MemoryStore.Transactions.InventoryManagement
{
    public sealed class DeleteDepletedInventoryBatchAtId : MemoryDataSourceTransaction
    {
        MemoryStoreContext store;
        Entity<InventoryBatch> entity;

        public DeleteDepletedInventoryBatchAtId(Entity<InventoryBatch> entity, MemoryStoreContext store)
        {
            this.entity = entity;
            this.store = store;
        }

        public override void Execute()
        {
            if (BatchExistsAndIsDepleted())
            {
                store.CurrentInventory.Remove(entity.SystemId);
            }
        }

        bool BatchExistsAndIsDepleted()
        {
            return store.CurrentInventory.ContainsKey(entity.SystemId)
                && entity.NativeModel.Quantity == 0;
        }
    }
}
