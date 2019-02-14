namespace CSI.BatchTracker.Storage.MemoryStore.Transactions.InventoryManagement
{
    public sealed class DeleteReceivedBatchAtIdTransaction : MemoryDataSourceTransaction
    {
        MemoryStoreContext memoryStore;
        int targetId;

        public DeleteReceivedBatchAtIdTransaction(int targetId, MemoryStoreContext memoryStore)
        {
            this.memoryStore = memoryStore;
            this.targetId = targetId;
        }

        public override void Execute()
        {
            if (memoryStore.ReceivingLedger.ContainsKey(targetId))
            {
                memoryStore.ReceivingLedger.Remove(targetId);
            }
        }
    }
}
