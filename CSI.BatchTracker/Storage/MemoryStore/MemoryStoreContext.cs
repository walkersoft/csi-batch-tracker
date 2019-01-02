using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using System.Collections.Generic;

namespace CSI.BatchTracker.Storage.MemoryStore
{
    public sealed class MemoryStoreContext
    {
        public Dictionary<int, Entity<BatchOperator>> BatchOperators { get; private set; }
        public Dictionary<int, Entity<InventoryBatch>> CurrentInventory { get; private set; }
        public Dictionary<int, Entity<LoggedBatch>> ImplementedBatchLedger { get; private set; }
        public Dictionary<int, Entity<ReceivedBatch>> ReceivingLedger { get; private set; }

        public MemoryStoreContext()
        {
            BatchOperators = new Dictionary<int, Entity<BatchOperator>>();
            CurrentInventory = new Dictionary<int, Entity<InventoryBatch>>();
            ImplementedBatchLedger = new Dictionary<int, Entity<LoggedBatch>>();
            ReceivingLedger = new Dictionary<int, Entity<ReceivedBatch>>();
        }
    }
}
