namespace CSI.BatchTracker.Storage.MemoryStore.Transactions.RecordAquisition
{
    public class FindBatchInReceivingLedgerByIdTransaction : MemoryDataSourceTransaction
    {
        MemoryStoreContext memoryStore;
        int systemId;

        public FindBatchInReceivingLedgerByIdTransaction(int systemId, MemoryStoreContext memoryStore)
        {
            this.systemId = systemId;
            this.memoryStore = memoryStore;
        }

        public override void Execute()
        {
            Results.Add(memoryStore.ReceivingLedger[systemId]);
        }
    }
}
