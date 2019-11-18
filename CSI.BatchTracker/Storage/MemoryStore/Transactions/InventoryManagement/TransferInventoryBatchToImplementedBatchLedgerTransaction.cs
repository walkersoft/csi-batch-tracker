﻿using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using System.Collections.Generic;

namespace CSI.BatchTracker.Storage.MemoryStore.Transactions.InventoryManagement
{
    public sealed class TransferInventoryBatchToImplementedBatchLedgerTransaction : MemoryDataSourceTransaction
    {
        Entity<LoggedBatch> entity;
        MemoryStoreContext store;
        public static int LastSystemId { get; private set; }

        public TransferInventoryBatchToImplementedBatchLedgerTransaction(Entity<LoggedBatch> entity, MemoryStoreContext store)
        {
            this.entity = entity;
            this.store = store;
        }

        int GetNextSystemId()
        {
            int systemId = 0;

            foreach (KeyValuePair<int, Entity<LoggedBatch>> entity in store.ImplementedBatchLedger)
            {
                if (entity.Key > systemId)
                {
                    systemId = entity.Key;
                }
            }

            systemId++;

            return systemId;
        }

        public override void Execute()
        {
            Entity<InventoryBatch> inventoryBatch = GetExistingInventoryBatch(entity);
            LastSystemId = GetNextSystemId();
            entity = new Entity<LoggedBatch>(LastSystemId, entity.NativeModel);
            store.ImplementedBatchLedger.Add(LastSystemId, entity);
            EditInventoryRecordAndDeleteIfDepleted(inventoryBatch);
        }
        
        void EditInventoryRecordAndDeleteIfDepleted(Entity<InventoryBatch> entity)
        {
            EditBatchInCurrentInventoryTransaction updater = new EditBatchInCurrentInventoryTransaction(entity, store);
            updater.Execute();
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
