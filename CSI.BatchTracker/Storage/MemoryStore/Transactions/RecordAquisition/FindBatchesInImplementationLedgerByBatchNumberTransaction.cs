using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using System.Collections.Generic;

namespace CSI.BatchTracker.Storage.MemoryStore.Transactions.RecordAquisition
{
    public sealed class FindBatchesInImplementationLedgerByBatchNumberTransaction : MemoryDataSourceTransaction
    {
        string batchNumber;
        MemoryStoreContext store;

        public FindBatchesInImplementationLedgerByBatchNumberTransaction(string batchNumber, MemoryStoreContext store)
        {
            this.batchNumber = batchNumber;
            this.store = store;
        }

        public override void Execute()
        {
            Results.Clear();

            foreach (KeyValuePair<int, Entity<LoggedBatch>> entity in store.ImplementedBatchLedger)
            {
                if (entity.Value.NativeModel.BatchNumber == batchNumber)
                {
                    Results.Add(entity.Value);
                }
            }
        }
    }
}
