namespace CSI.BatchTracker.Storage.MemoryStore.Transactions.RecordAquisition
{
    public sealed class FindInventoryBatchByIdTransaction : MemoryDataSourceTransaction
    {
        int TargetId { get; set; }
        MemoryStoreContext store;

        public FindInventoryBatchByIdTransaction(int targetId, MemoryStoreContext store)
        {
            TargetId = targetId;
            this.store = store;
        }

        public override void Execute()
        {
            Results.Clear();

            if (store.CurrentInventory.ContainsKey(TargetId))
            {
                Results.Add(store.CurrentInventory[TargetId]);
            }
        }
    }
}
