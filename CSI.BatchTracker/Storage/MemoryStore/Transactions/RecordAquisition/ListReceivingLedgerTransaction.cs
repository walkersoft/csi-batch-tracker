using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using System.Collections.Generic;

namespace CSI.BatchTracker.Storage.MemoryStore.Transactions.RecordAquisition
{
    public sealed class ListReceivingLedgerTransaction : MemoryDataSourceTransaction
    {
        MemoryStoreContext store;

        public ListReceivingLedgerTransaction(MemoryStoreContext store)
        {
            this.store = store;
        }

        public override void Execute()
        {
            Results.Clear();

            foreach (KeyValuePair<int, Entity<ReceivedBatch>> entity in store.ReceivingLedger)
            {
                Results.Add(entity.Value);
            }
        }
    }
}
