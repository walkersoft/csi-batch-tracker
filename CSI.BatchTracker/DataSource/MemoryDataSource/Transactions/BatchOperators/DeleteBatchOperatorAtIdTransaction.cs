using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.DataSource.MemoryDataSource.Transactions.BatchOperators
{
    public sealed class DeleteBatchOperatorAtIdTransaction : MemoryDataSourceTransaction
    {
        MemoryStore store;
        int targetId;

        public DeleteBatchOperatorAtIdTransaction(int targetId, MemoryStore store)
        {
            this.targetId = targetId;
            this.store = store;
        }

        public override void Execute()
        {
            if (store.BatchOperators.ContainsKey(targetId))
            {
                store.BatchOperators.Remove(targetId);
            }
        }
    }
}
