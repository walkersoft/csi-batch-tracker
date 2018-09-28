using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.DataSource.MemoryDataSource.Transactions.InventoryManagement
{
    public sealed class EditBatchInCurrentInventoryTransaction : MemoryDataSourceTransaction
    {
        MemoryStore store;
        Entity<InventoryBatch> entity;

        public EditBatchInCurrentInventoryTransaction(Entity<InventoryBatch> entity, MemoryStore store)
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
