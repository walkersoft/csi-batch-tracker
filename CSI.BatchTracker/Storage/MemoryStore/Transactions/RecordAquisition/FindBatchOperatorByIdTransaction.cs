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
        MemoryStoreContext store;

        public FindBatchOperatorByIdTransaction(int targetId, MemoryStoreContext store)
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
