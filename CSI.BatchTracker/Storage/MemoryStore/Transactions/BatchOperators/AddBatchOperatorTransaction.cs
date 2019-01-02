using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Storage.MemoryStore.Transactions.BatchOperators
{
    public sealed class AddBatchOperatorTransaction : MemoryDataSourceTransaction
    {
        Entity<BatchOperator> entity;
        MemoryStoreContext store;
        public int LastSystemId { get; private set; }

        public AddBatchOperatorTransaction(Entity<BatchOperator> entity, MemoryStoreContext store)
        {
            this.entity = entity;
            this.store = store;
            LastSystemId = store.BatchOperators.Count;
        }

        public override void Execute()
        {
            LastSystemId++;
            entity = new Entity<BatchOperator>(LastSystemId, entity.NativeModel);
            store.BatchOperators.Add(LastSystemId, entity);
        }
    }
}
