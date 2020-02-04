using CSI.BatchTracker.Domain.NativeModels;

namespace CSI.BatchTracker.Storage.SQLiteStore.Transactions.RecordAquisition
{
    public sealed class ListBatchOperatorsTransaction : SQLiteDataSourceTransaction
    {
        SQLiteStoreContext store;

        public ListBatchOperatorsTransaction(SQLiteStoreContext store)
        {
            this.store = store;
        }

        public override void Execute()
        {
            string query = "SELECT * FROM BatchOperators";
            store.ExecuteReader(typeof(BatchOperator), query);
            Results = store.Results;
        }
    }
}
