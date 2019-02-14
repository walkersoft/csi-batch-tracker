using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using System.Collections.Generic;

namespace CSI.BatchTracker.Storage.MemoryStore.Transactions.RecordAquisition
{
    public sealed class FindBatchesInReceivingLedgerByPONumberTransaction : MemoryDataSourceTransaction
    {
        MemoryStoreContext memoryStore;
        int poNumber;

        public FindBatchesInReceivingLedgerByPONumberTransaction(int poNumber, MemoryStoreContext memoryStore)
        {
            this.memoryStore = memoryStore;
            this.poNumber = poNumber;
        }

        public override void Execute()
        {
            Results.Clear();

            foreach (KeyValuePair<int, Entity<ReceivedBatch>> entity in memoryStore.ReceivingLedger)
            {
                if (entity.Value.NativeModel.PONumber == poNumber)
                {
                    Results.Add(entity.Value);
                }
            }
        }
    }
}
