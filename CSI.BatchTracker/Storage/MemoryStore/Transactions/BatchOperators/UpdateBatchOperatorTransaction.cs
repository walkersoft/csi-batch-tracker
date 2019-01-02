using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Storage.MemoryStore.Transactions.BatchOperators
{
    public sealed class UpdateBatchOperatorTransaction : MemoryDataSourceTransaction
    {
        Entity<BatchOperator> entity;
        MemoryStoreContext store;

        public UpdateBatchOperatorTransaction(Entity<BatchOperator> entity, MemoryStoreContext store)
        {
            this.entity = entity;
            this.store = store;
        }

        public override void Execute()
        {
            if (store.BatchOperators.ContainsKey(entity.SystemId))
            {
                store.BatchOperators[entity.SystemId].NativeModel = entity.NativeModel;
            }
        }
    }
}
