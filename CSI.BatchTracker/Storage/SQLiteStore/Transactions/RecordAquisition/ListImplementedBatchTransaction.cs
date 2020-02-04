using CSI.BatchTracker.Domain.NativeModels;

namespace CSI.BatchTracker.Storage.SQLiteStore.Transactions.RecordAquisition
{
    public sealed class ListImplementedBatchTransaction : SQLiteDataSourceTransaction
    {
        SQLiteStoreContext store;

        public ListImplementedBatchTransaction(SQLiteStoreContext store)
        {
            this.store = store;
        }

        public override void Execute()
        {
            string query = "SELECT * FROM ImplementedBatches";
            store.ExecuteReader(typeof(LoggedBatch), query);
            Results = store.Results;
        }
    }
}
