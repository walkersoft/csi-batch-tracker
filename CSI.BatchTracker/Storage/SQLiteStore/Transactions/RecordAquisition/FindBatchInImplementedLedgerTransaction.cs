using CSI.BatchTracker.Domain.NativeModels;
using System.Collections.Generic;

namespace CSI.BatchTracker.Storage.SQLiteStore.Transactions.RecordAquisition
{
    public sealed class FindBatchInImplementedLedgerTransaction : SQLiteDataSourceTransaction
    {
        int targetId;
        SQLiteStoreContext store;

        public FindBatchInImplementedLedgerTransaction(int targetId, SQLiteStoreContext store)
        {
            this.targetId = targetId;
            this.store = store;
        }

        public override void Execute()
        {
            string query = "SELECT * FROM ImplementedBatches WHERE SystemId = ?";
            List<object> parameters = new List<object>() { targetId };
            store.ExecuteReader(typeof(LoggedBatch), query, parameters);
            Results = store.Results;
        }
    }
}
