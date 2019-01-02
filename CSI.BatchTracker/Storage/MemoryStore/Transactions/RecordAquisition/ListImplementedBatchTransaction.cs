using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using System.Collections.Generic;

namespace CSI.BatchTracker.Storage.MemoryStore.Transactions.RecordAquisition
{
    public sealed class ListImplementedBatchTransaction : MemoryDataSourceTransaction
    {
        MemoryStoreContext store;

        public ListImplementedBatchTransaction(MemoryStoreContext store)
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
