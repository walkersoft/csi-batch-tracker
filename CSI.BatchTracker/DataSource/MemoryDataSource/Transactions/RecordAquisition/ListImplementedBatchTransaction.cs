﻿using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.DataSource.MemoryDataSource.Transactions.RecordAquisition
{
    public sealed class ListImplementedBatchTransaction : MemoryDataSourceTransaction
    {
        MemoryStore store;

        public ListImplementedBatchTransaction(MemoryStore store)
        {
            this.store = store;
        }

        public override void Execute()
        {
            Results.Clear();

            foreach (KeyValuePair<int, Entity<LoggedBatch>> batches in store.ImplementedBatchLedger)
            {
                Results.Add(batches.Value);
            }
        }
    }
}
