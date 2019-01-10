using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Storage.MemoryStore.Transactions.RecordAquisition
{
    public class FindBatchInReceivingLedgerByIdTransaction : MemoryDataSourceTransaction
    {
        MemoryStoreContext memoryStore;
        int id;

        public FindBatchInReceivingLedgerByIdTransaction(int id, MemoryStoreContext memoryStore)
        {
            this.id = id;
            this.memoryStore = memoryStore;
        }

        public override void Execute()
        {
            if (memoryStore.ReceivingLedger.ContainsKey(id))
            {
                Results.Add(memoryStore.ReceivingLedger[id]);
            }
        }
    }
}
