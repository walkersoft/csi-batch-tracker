using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;

namespace CSI.BatchTracker.Storage.MemoryStore.Transactions.RecordAquisition
{
    public sealed class FindBatchOperatorByIdTransaction : MemoryDataSourceTransaction
    {
        int TargetId { get; set; }
        MemoryStoreContext store;

        public FindBatchOperatorByIdTransaction(int targetId, MemoryStoreContext store)
        {
            TargetId = targetId;
            this.store = store;
        }

        public override void Execute()
        {
            Results.Clear();
            Results.Add(store.BatchOperators[TargetId]);
        }
    }
}
