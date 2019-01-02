using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Storage.MemoryStore.Transactions.InventoryManagement
{
    public class UndoImplementedBatchCommittedToLedgerTransaction : MemoryDataSourceTransaction
    {
        MemoryStore store;
        Entity<LoggedBatch> loggedEntity;
        Entity<InventoryBatch> inventoryEntity;
        AddReceivedBatchToInventoryTransaction inventoryAdder;

        public UndoImplementedBatchCommittedToLedgerTransaction(Entity<LoggedBatch> entity, MemoryStore store)
        {
            this.loggedEntity = entity;
            this.store = store;
        }

        public override void Execute()
        {
            if (store.ImplementedBatchLedger.ContainsKey(loggedEntity.SystemId))
            {
                inventoryEntity = RetainLoggedEntityAsInventoryEntity();
                store.ImplementedBatchLedger.Remove(loggedEntity.SystemId);
                RecommitToInventory();
            }
        }

        Entity<InventoryBatch> RetainLoggedEntityAsInventoryEntity()
        {
            return new Entity<InventoryBatch>(
                new InventoryBatch(
                    loggedEntity.NativeModel.ColorName,
                    loggedEntity.NativeModel.BatchNumber,
                    loggedEntity.NativeModel.ActivityDate,
                    1
                )
            );
        }

        void RecommitToInventory()
        {
            inventoryAdder = new AddReceivedBatchToInventoryTransaction(inventoryEntity, store);
            inventoryAdder.Execute();
        }
    }
}
