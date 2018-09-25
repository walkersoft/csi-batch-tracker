using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.DataSource.MemoryDataSource.Transactions.InventoryManagement
{
    public sealed class AddBatchToImplementedBatchLedgerTransaction : MemoryDataSourceTransaction
    {
        Entity<LoggedBatch> entity;
        MemoryStore store;
        public int LastSystemId { get; private set; }

        public AddBatchToImplementedBatchLedgerTransaction(Entity<LoggedBatch> entity, MemoryStore store)
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
