using CSI.BatchTracker.Domain.NativeModels;
using System.Collections.Generic;

namespace CSI.BatchTracker.Storage.SQLiteStore.Transactions.RecordAquisition
{
    public sealed class FindBatchesInImplementationLedgerByBatchNumberTransaction : SQLiteDataSourceTransaction
    {
        string batchNumber;
        SQLiteStoreContext store;

        public FindBatchesInImplementationLedgerByBatchNumberTransaction(string batchNumber, SQLiteStoreContext store)
        {
            this.batchNumber = batchNumber;
            this.store = store;
        }

        public override void Execute()
        {
            string query = "SELECT * FROM ImplementedBatches WHERE BatchNumber = ?";
            List<object> parameters = new List<object>() { batchNumber };
            store.ExecuteReader(typeof(LoggedBatch), query, parameters);
            Results = store.Results;
        }
    }
}
