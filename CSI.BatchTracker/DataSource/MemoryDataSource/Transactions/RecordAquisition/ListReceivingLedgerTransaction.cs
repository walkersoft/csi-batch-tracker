using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.DataSource.MemoryDataSource.Transactions.RecordAquisition
{
    public sealed class ListReceivingLedgerTransaction : MemoryDataSourceTransaction
    {
        MemoryStore store;

        public ListReceivingLedgerTransaction(MemoryStore store)
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
