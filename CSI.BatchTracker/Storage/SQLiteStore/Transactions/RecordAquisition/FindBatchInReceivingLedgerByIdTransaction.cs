using CSI.BatchTracker.Domain.NativeModels;
using System.Collections.Generic;

namespace CSI.BatchTracker.Storage.SQLiteStore.Transactions.RecordAquisition
{
    public sealed class FindBatchInReceivingLedgerByIdTransaction : SQLiteDataSourceTransaction
    {
        SQLiteStoreContext store;
        int targetId;

        public FindBatchInReceivingLedgerByIdTransaction(int targetId, SQLiteStoreContext store)
        {
            this.store = store;
            this.targetId = targetId;
        }

        public override void Execute()
        {
            string query = "SELECT * FROM ReceivedBatches WHERE SystemId = ?";
            List<object> parameters = new List<object> { targetId };
            store.ExecuteReader(typeof(ReceivedBatch), query, parameters);
            Results = store.Results;
        }
    }
}
