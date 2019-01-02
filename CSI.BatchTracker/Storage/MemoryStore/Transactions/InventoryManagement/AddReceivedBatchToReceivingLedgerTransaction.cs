using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Storage.MemoryStore.Transactions.InventoryManagement
{
    public sealed class AddReceivedBatchToReceivingLedgerTransaction : MemoryDataSourceTransaction
    {
        MemoryStoreContext store;
        Entity<ReceivedBatch> entity;
        public int LastSystemId { get; private set; }

        public AddReceivedBatchToReceivingLedgerTransaction(Entity<ReceivedBatch> entity, MemoryStoreContext store)
        {
            this.entity = entity;
            this.store = store;
            LastSystemId = store.ReceivingLedger.Count;
        }

        public override void Execute()
        {
            LastSystemId++;
            entity = new Entity<ReceivedBatch>(LastSystemId, entity.NativeModel);
            store.ReceivingLedger.Add(LastSystemId, entity);
        }
    }
}
