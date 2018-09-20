using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.DataSource.MemoryDataSource.Transactions
{
    public sealed class AddBatchOperatorTransaction : MemoryDataSourceTransaction
    {
        Entity<BatchOperator> entity;
        MemoryStore store;
        int nextSystemId;

        public AddBatchOperatorTransaction(Entity<BatchOperator> entity, MemoryStore store)
        {
            this.entity = entity;
            this.store = store;
            nextSystemId = 0;
        }

        public override void Execute()
        {
            nextSystemId++;
            entity = new Entity<BatchOperator>(nextSystemId, entity.NativeModel);
            store.BatchOperators.Add(nextSystemId, entity);
        }
    }
}
