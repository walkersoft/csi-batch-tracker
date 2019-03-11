using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.Storage.Contracts;

namespace CSI.BatchTracker.Storage.MemoryStore.Transactions.InventoryManagement
{
    public sealed class EditBatchInCurrentInventoryTransaction : MemoryDataSourceTransaction
    {
        MemoryStoreContext store;
        Entity<InventoryBatch> entity;

        public EditBatchInCurrentInventoryTransaction(Entity<InventoryBatch> entity, MemoryStoreContext store)
        {
            this.entity = entity;
            this.store = store;
        }

        public override void Execute()
        {
            store.CurrentInventory[entity.SystemId] = entity;
            DeleteIfDepleted();
        }

        void DeleteIfDepleted()
        {
            ITransaction deleter = new DeleteDepletedInventoryBatchAtId(entity, store);
            deleter.Execute();
        }
    }
}
