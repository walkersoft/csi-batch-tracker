using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.DataSource.MemoryDataSource.Transactions.BatchOperators
{
    public sealed class FindBatchOperatorTransaction : MemoryDataSourceTransaction
    {
        Entity<BatchOperator> entity;
        MemoryStore store;

        public FindBatchOperatorTransaction(Entity<BatchOperator> entity, MemoryStore store)
        {
            this.entity = entity;
            this.store = store;
        }

        public override void Execute()
        {
            Results.Clear();

            if (store.BatchOperators.ContainsKey(entity.SystemId))
            {
                Results.Add(entity);
            }
        }
    }
}
