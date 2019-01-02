using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Storage.MemoryStore.Transactions.RecordAquisition
{
    public sealed class ListBatchOperatorsTransaction : MemoryDataSourceTransaction
    {
        MemoryStoreContext store;

        public ListBatchOperatorsTransaction(MemoryStoreContext store)
        {
            this.store = store;
        }

        public override void Execute()
        {
            Results.Clear();

            foreach (KeyValuePair<int, Entity<BatchOperator>> entity in store.BatchOperators)
            {
                Results.Add(entity.Value);
            }
        }
    }
}
