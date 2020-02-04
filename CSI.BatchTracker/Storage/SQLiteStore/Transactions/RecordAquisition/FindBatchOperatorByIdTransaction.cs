using CSI.BatchTracker.Domain.NativeModels;
using System.Collections.Generic;

namespace CSI.BatchTracker.Storage.SQLiteStore.Transactions.RecordAquisition
{
    public sealed class FindBatchOperatorByIdTransaction : SQLiteDataSourceTransaction
    {
        SQLiteStoreContext store;
        int targetId;

        public FindBatchOperatorByIdTransaction(int targetId, SQLiteStoreContext store)
        {
            this.targetId = targetId;
            this.store = store;
        }

        public override void Execute()
        {
            string query = "SELECT * FROM BatchOperators WHERE SystemId = ?";

            List<object> parameters = new List<object>
            {
                targetId
            };

            store.ExecuteReader(typeof(BatchOperator), query, parameters);
            Results = store.Results;
        }
    }
}
