﻿using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;

namespace CSI.BatchTracker.Storage.MemoryStore.Transactions.BatchOperators
{
    public sealed class FindBatchOperatorTransaction : MemoryDataSourceTransaction
    {
        Entity<BatchOperator> entity;
        MemoryStoreContext store;

        public FindBatchOperatorTransaction(Entity<BatchOperator> entity, MemoryStoreContext store)
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
