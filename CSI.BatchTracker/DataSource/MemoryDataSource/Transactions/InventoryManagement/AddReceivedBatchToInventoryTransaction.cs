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
            LastSystemId = 0;
        }
        
        public override void Execute()
        {
            if (InventoryStoreIsEmpty())
            {
                CreateNewBatchEntry();
                return;
            }
           
            foreach (KeyValuePair<int, Entity<InventoryBatch>> currentInventory in store.CurrentInventory)
            {
                if (BatchExistsInInventory(currentInventory.Value))
                {
                    MergeWithCurrentBatchEntry(currentInventory.Key);
                }
                else
                {
                    CreateNewBatchEntry();
                }
            }            
        }

        bool InventoryStoreIsEmpty()
        {
            return store.CurrentInventory.Count == 0;
        }

        bool BatchExistsInInventory(Entity<InventoryBatch> existing)
        {
            return entity.NativeModel.BatchNumber == existing.NativeModel.BatchNumber;
        }

        void CreateNewBatchEntry()
        {
            LastSystemId++;
            Entity<InventoryBatch> newEntity = new Entity<InventoryBatch>(LastSystemId, entity.NativeModel);
            store.CurrentInventory.Add(LastSystemId, newEntity);
        }

        void MergeWithCurrentBatchEntry(int systemId)
        {
            store.CurrentInventory[systemId].NativeModel.AddQuantity(entity.NativeModel.Quantity);
        }
    }
}
