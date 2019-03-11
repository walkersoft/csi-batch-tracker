namespace CSI.BatchTracker.Storage.MemoryStore.Transactions.RecordAquisition
{
    public sealed class FindBatchInImplementedLedgerTransaction : MemoryDataSourceTransaction
    {
        MemoryStoreContext store;
        int systemId;

        public FindBatchInImplementedLedgerTransaction(int systemId, MemoryStoreContext store)
        {
            this.systemId = systemId;
            this.store = store;
        }

        public override void Execute()
        {
            Results.Clear();

            if (store.ImplementedBatchLedger.ContainsKey(systemId))
            {
                Results.Add(store.ImplementedBatchLedger[systemId]);
            }
        }
    }
}
