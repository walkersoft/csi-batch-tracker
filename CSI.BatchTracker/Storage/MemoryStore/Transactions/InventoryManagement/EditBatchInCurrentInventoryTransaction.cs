﻿using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;

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
            if (store.CurrentInventory.ContainsKey(entity.SystemId))
            {
                store.CurrentInventory[entity.SystemId] = entity;
            }
        }
    }
}
