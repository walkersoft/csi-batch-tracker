using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.DataSource.MemoryDataSource
{
    public sealed class MemoryStore
    {
        public Dictionary<int, Entity<BatchOperator>> BatchOperators { get; private set; }
        public Dictionary<int, Entity<InventoryBatch>> CurrentInventory { get; private set; }
        public Dictionary<int, Entity<LoggedBatch>> ImplementedBatchLedger { get; private set; }

        public MemoryStore()
        {
            BatchOperators = new Dictionary<int, Entity<BatchOperator>>();
            CurrentInventory = new Dictionary<int, Entity<InventoryBatch>>();
            ImplementedBatchLedger = new Dictionary<int, Entity<LoggedBatch>>();
        }
    }
}
