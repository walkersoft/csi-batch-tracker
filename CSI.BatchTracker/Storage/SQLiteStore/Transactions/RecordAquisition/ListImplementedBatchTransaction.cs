using CSI.BatchTracker.Domain.NativeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Storage.SQLiteStore.Transactions.RecordAquisition
{
    public sealed class ListImplementedBatchTransaction : SQLiteDataSourceTransaction
    {
        SQLiteStoreContext store;

        public ListImplementedBatchTransaction(SQLiteStoreContext store)
        {
            this.store = store;
        }

        public override void Execute()
        {
            string query = "SELECT * FROM ImplementedBatches";
            store.ExecuteReader(typeof(LoggedBatch), query);
            Results = store.Results;
        }
    }
}
