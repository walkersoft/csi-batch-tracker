using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
