using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;

namespace CSI.BatchTracker.Storage.MemoryStore.Transactions.InventoryManagement
{
    public sealed class EditBatchInReceivingLedgerTransaction : MemoryDataSourceTransaction
    {
        MemoryStoreContext store;
        Entity<ReceivedBatch> entity;

        public EditBatchInReceivingLedgerTransaction(Entity<ReceivedBatch> entity, MemoryStoreContext store)
        {
            this.entity = entity;
            this.store = store;
        }

        public override void Execute()
        {
            if (store.ReceivingLedger.ContainsKey(entity.SystemId))
            {
                store.ReceivingLedger[entity.SystemId] = entity;
            }
        }
    }
}
