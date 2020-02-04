using CSI.BatchTracker.Domain.NativeModels;

namespace CSI.BatchTracker.Storage.SQLiteStore.Transactions.RecordAquisition
{
    public sealed class ListCurrentInventoryTransaction : SQLiteDataSourceTransaction
    {
        SQLiteStoreContext store;

        public ListCurrentInventoryTransaction(SQLiteStoreContext store)
        {
            this.store = store;
        }

        public override void Execute()
        {
            string query = "SELECT * FROM InventoryBatches";
            store.ExecuteReader(typeof(InventoryBatch), query);
            Results = store.Results;
        }
    }
}
