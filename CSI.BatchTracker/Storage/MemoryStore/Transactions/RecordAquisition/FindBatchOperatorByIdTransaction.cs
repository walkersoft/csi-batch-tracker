using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Storage.MemoryStore.Transactions.RecordAquisition
{
    public sealed class FindBatchOperatorByIdTransaction : MemoryDataSourceTransaction
    {
        int TargetId { get; set; }
        MemoryStore store;

        public FindBatchOperatorByIdTransaction(int targetId, MemoryStore store)
        {
            TargetId = targetId;
            this.store = store;
        }

        public override void Execute()
        {
            Results.Clear();

            if (store.BatchOperators.ContainsKey(TargetId))
            {
                Results.Add(store.BatchOperators[TargetId]);
            }
        }
    }
}
