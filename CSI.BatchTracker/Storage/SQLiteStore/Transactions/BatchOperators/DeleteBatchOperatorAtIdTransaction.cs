using System.Collections.Generic;

namespace CSI.BatchTracker.Storage.SQLiteStore.Transactions.BatchOperators
{
    public sealed class DeleteBatchOperatorAtIdTransaction : SQLiteDataSourceTransaction
    {
        SQLiteStoreContext store;
        int targetId;

        public DeleteBatchOperatorAtIdTransaction(int targetId, SQLiteStoreContext store)
        {
            this.store = store;
            this.targetId = targetId;
        }

        public override void Execute()
        {
            string query = "DELETE FROM BatchOperators WHERE SystemId = ?";
            List<object> parameters = new List<object> { targetId };
            store.ExecuteNonQuery(query, parameters);
        }
    }
}
