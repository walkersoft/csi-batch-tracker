﻿using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.DataSource.MemoryDataSource.Transactions.RecordAquisition
{
    public sealed class FindBatchInImplementedLedgerTransaction : MemoryDataSourceTransaction
    {
        MemoryStore store;
        string batchNumber;

        public FindBatchInImplementedLedgerTransaction(string batchNumber, MemoryStore store)
        {
            this.batchNumber = batchNumber;
            this.store = store;
        }

        public override void Execute()
        {
            Results.Clear();

            foreach (KeyValuePair<int, Entity<LoggedBatch>> batch in store.ImplementedBatchLedger)
            {
                if (batch.Value.NativeModel.BatchNumber == batchNumber)
                {
                    Results.Add(batch.Value);
                }
            }
        }
    }
}
