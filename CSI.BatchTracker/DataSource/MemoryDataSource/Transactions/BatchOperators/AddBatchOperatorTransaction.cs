﻿using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.DataSource.MemoryDataSource.Transactions.BatchOperators
{
    public sealed class AddBatchOperatorTransaction : MemoryDataSourceTransaction
    {
        Entity<BatchOperator> entity;
        MemoryStore store;
        public int LastSystemId { get; private set; }

        public AddBatchOperatorTransaction(Entity<BatchOperator> entity, MemoryStore store)
        {
            this.entity = entity;
            this.store = store;
            LastSystemId = 0;
        }

        public override void Execute()
        {
            LastSystemId++;
            entity = new Entity<BatchOperator>(LastSystemId, entity.NativeModel);
            store.BatchOperators.Add(LastSystemId, entity);
        }
    }
}
