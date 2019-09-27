using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using System;
using System.Collections.Generic;

namespace CSI.BatchTracker.Storage.MemoryStore
{
    [Serializable]
    public sealed class MemoryStoreContext
    {
        public Dictionary<int, Entity<BatchOperator>> BatchOperators { get; set; }
        public Dictionary<int, Entity<InventoryBatch>> CurrentInventory { get; set; }
        public Dictionary<int, Entity<LoggedBatch>> ImplementedBatchLedger { get; set; }
        public Dictionary<int, Entity<ReceivedBatch>> ReceivingLedger { get; set; }
        public int HashCode { get { return GetHashCode(); } }

        public MemoryStoreContext()
        {
            BatchOperators = new Dictionary<int, Entity<BatchOperator>>();
            CurrentInventory = new Dictionary<int, Entity<InventoryBatch>>();
            ImplementedBatchLedger = new Dictionary<int, Entity<LoggedBatch>>();
            ReceivingLedger = new Dictionary<int, Entity<ReceivedBatch>>();
        }
    }
}
