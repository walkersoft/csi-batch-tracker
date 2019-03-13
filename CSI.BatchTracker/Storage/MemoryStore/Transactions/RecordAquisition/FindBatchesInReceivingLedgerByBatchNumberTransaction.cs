using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Storage.MemoryStore.Transactions.RecordAquisition
{
    public sealed class FindBatchesInReceivingLedgerByBatchNumberTransaction : MemoryDataSourceTransaction
    {
        string batchNumber;
        MemoryStoreContext store;

        public FindBatchesInReceivingLedgerByBatchNumberTransaction(string batchNumber, MemoryStoreContext store)
        {
            this.batchNumber = batchNumber;
            this.store = store;
        }

        public override void Execute()
        {
            Results.Clear();

            foreach (KeyValuePair<int, Entity<ReceivedBatch>> entity in store.ReceivingLedger)
            {
                if (entity.Value.NativeModel.BatchNumber == batchNumber)
                {
                    Results.Add(entity.Value);
                }
            }
        }
    }
}
