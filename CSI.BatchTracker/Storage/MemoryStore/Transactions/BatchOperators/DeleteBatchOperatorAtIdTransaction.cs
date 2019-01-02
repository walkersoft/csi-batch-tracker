﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Storage.MemoryStore.Transactions.BatchOperators
{
    public sealed class DeleteBatchOperatorAtIdTransaction : MemoryDataSourceTransaction
    {
        MemoryStoreContext store;
        int targetId;

        public DeleteBatchOperatorAtIdTransaction(int targetId, MemoryStoreContext store)
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