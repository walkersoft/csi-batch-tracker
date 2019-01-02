using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using System.Collections.Generic;

namespace CSI.BatchTracker.Storage.MemoryStore.Transactions.InventoryManagement
{
    public sealed class AddBatchToImplementedBatchLedgerTransaction : MemoryDataSourceTransaction
    {
        Entity<LoggedBatch> entity;
        MemoryStoreContext store;
        public int LastSystemId { get; private set; }

        public AddBatchToImplementedBatchLedgerTransaction(Entity<LoggedBatch> entity, MemoryStoreContext store)
        {
            this.entity = entity;
            this.store = store;
        }

        public override void Execute()
        {
            Entity<InventoryBatch> inventoryBatch = GetExistingInventoryBatch(entity);

            if (inventoryBatch != null)
            {
                LastSystemId++;
                entity = new Entity<LoggedBatch>(LastSystemId, entity.NativeModel);
                store.ImplementedBatchLedger.Add(LastSystemId, entity);
                inventoryBatch.NativeModel.DeductQuantity(1);
            }
            
        }

        Entity<InventoryBatch> GetExistingInventoryBatch(Entity<LoggedBatch> implemented)
        {
            Entity<InventoryBatch> found = null;

            foreach (KeyValuePair<int, Entity<InventoryBatch>> inventoryBatch in store.CurrentInventory)
            {
                if (inventoryBatch.Value.NativeModel.BatchNumber == implemented.NativeModel.BatchNumber)
                {
                    found = store.CurrentInventory[inventoryBatch.Key];
                }
            }

            return found;
        }
    }
}
