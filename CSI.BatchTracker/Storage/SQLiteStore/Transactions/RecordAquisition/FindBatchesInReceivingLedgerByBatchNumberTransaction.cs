using CSI.BatchTracker.Domain.NativeModels;
using System.Collections.Generic;

namespace CSI.BatchTracker.Storage.SQLiteStore.Transactions.RecordAquisition
{
    public sealed class FindBatchesInReceivingLedgerByBatchNumberTransaction : SQLiteDataSourceTransaction
    {
        string batchNumber;
        SQLiteStoreContext store;

        public FindBatchesInReceivingLedgerByBatchNumberTransaction(string batchNumber, SQLiteStoreContext store)
        {
            this.batchNumber = batchNumber;
            this.store = store;
        }

        public override void Execute()
        {
            string query = "SELECT * FROM ReceivedBatches WHERE BatchNumber = ?";
            List<object> parameters = new List<object> { batchNumber };
            store.ExecuteReader(typeof(ReceivedBatch), query, parameters);
            Results = store.Results;
        }
    }
}
