using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.DataSource.MemoryDataSource.Transactions.InventoryManagement
{
    public sealed class AddReceivedBatchToInventoryTransaction : MemoryDataSourceTransaction
    {
        MemoryStore store;
        Entity<InventoryBatch> entity;
        public int LastSystemId { get; private set; }

        public AddReceivedBatchToInventoryTransaction(Entity<InventoryBatch> entity, MemoryStore store)
        {
            this.entity = entity;
            this.store = store;
            LastSystemId = store.CurrentInventory.Count;
        }
        
        public override void Execute()
        {
            if (InventoryStoreIsEmpty())
            {
                CreateNewBatchEntry();
                return;
            }
           
            if (BatchExistsInInventory(entity))
            {
                MergeWithCurrentBatchEntry(entity);
            }
            else
            {
                CreateNewBatchEntry();
            }
        }

        bool InventoryStoreIsEmpty()
        {
            return store.CurrentInventory.Count == 0;
        }

        bool BatchExistsInInventory(Entity<InventoryBatch> existing)
        {
            foreach (KeyValuePair<int, Entity<InventoryBatch>> currentInventory in store.CurrentInventory)
            {
                if (currentInventory.Value.NativeModel.BatchNumber == existing.NativeModel.BatchNumber)
                {
                    return true;
                }
            }

            return false;
        }

        void CreateNewBatchEntry()
        {
            LastSystemId++;
            Entity<InventoryBatch> newEntity = new Entity<InventoryBatch>(LastSystemId, entity.NativeModel);
            store.CurrentInventory.Add(LastSystemId, newEntity);
        }

        void MergeWithCurrentBatchEntry(Entity<InventoryBatch> batch)
        {
            foreach (KeyValuePair<int, Entity<InventoryBatch>> currentInventory in store.CurrentInventory)
            {
                if (currentInventory.Value.NativeModel.BatchNumber == batch.NativeModel.BatchNumber)
                {
                    currentInventory.Value.NativeModel.AddQuantity(batch.NativeModel.Quantity);
                }
            }
        }
    }
}
