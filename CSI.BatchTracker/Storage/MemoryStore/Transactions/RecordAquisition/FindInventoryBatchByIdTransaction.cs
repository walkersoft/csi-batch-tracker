using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Storage.MemoryStore.Transactions.RecordAquisition
{
    public sealed class FindInventoryBatchByIdTransaction : MemoryDataSourceTransaction
    {
        int TargetId { get; set; }
        MemoryStore store;

        public FindInventoryBatchByIdTransaction(int targetId, MemoryStore store)
        {
            TargetId = targetId;
            this.store = store;
        }

        public override void Execute()
        {
            Results.Clear();

            if (store.CurrentInventory.ContainsKey(TargetId))
            {
                Results.Add(store.CurrentInventory[TargetId]);
            }
        }
    }
}
