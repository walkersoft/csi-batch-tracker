using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using System.Collections.Generic;

namespace CSI.BatchTracker.Storage.MemoryStore.Transactions.RecordAquisition
{
    public sealed class FindBatchInImplementedLedgerTransaction : MemoryDataSourceTransaction
    {
        MemoryStoreContext store;
        string batchNumber;

        public FindBatchInImplementedLedgerTransaction(string batchNumber, MemoryStoreContext store)
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
