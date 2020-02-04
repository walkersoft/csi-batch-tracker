using CSI.BatchTracker.Domain.NativeModels;

namespace CSI.BatchTracker.Storage.SQLiteStore.Transactions.RecordAquisition
{
    public sealed class ListReceivingLedgerTransaction : SQLiteDataSourceTransaction
    {
        SQLiteStoreContext store;

        public ListReceivingLedgerTransaction(SQLiteStoreContext store)
        {
            this.store = store;
        }

        public override void Execute()
        {
            string query = "SELECT * FROM ReceivedBatches";
            store.ExecuteReader(typeof(ReceivedBatch), query);
            Results = store.Results;
        }
    }
}
