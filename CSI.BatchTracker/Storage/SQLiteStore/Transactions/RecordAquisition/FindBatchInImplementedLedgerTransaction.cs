using CSI.BatchTracker.Domain.NativeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
