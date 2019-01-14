using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
