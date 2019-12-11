using CSI.BatchTracker.Domain.NativeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Storage.SQLiteStore.Transactions.BatchOperators
{
    public sealed class UpdateBatchOperatorTransaction : SQLiteDataSourceTransaction
    {
        SQLiteStoreContext store;
        int targetId;
        BatchOperator targetOperator;

        public UpdateBatchOperatorTransaction(int targetId, BatchOperator targetOperator, SQLiteStoreContext store)
        {
            this.store = store;
            this.targetId = targetId;
            this.targetOperator = targetOperator;
        }

        public override void Execute()
        {
            string query = "UPDATE BatchOperators SET FirstName = ?, LastName = ? WHERE SystemId = ?";

            List<object> parameters = new List<object>
            {
                targetOperator.FirstName,
                targetOperator.LastName,
                targetId
            };

            store.ExecuteNonQuery(query, parameters);
        }
    }
}
