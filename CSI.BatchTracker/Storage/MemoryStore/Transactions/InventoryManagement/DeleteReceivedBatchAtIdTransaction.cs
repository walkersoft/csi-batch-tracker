using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.Storage.Contracts;
using System.Collections.Generic;

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
                ReceivedBatch batch = memoryStore.ReceivingLedger[targetId].NativeModel;
                string batchNumber = batch.BatchNumber;
                int quantity = batch.Quantity;
                memoryStore.ReceivingLedger.Remove(targetId);
                RemoveBatchQuantityFromInventory(batchNumber, quantity);
            }
        }

        void RemoveBatchQuantityFromInventory(string batchNumber, int quantity)
        {
            foreach (KeyValuePair<int, Entity<InventoryBatch>> batch in memoryStore.CurrentInventory)
            {
                if (batch.Value.NativeModel.BatchNumber == batchNumber)
                {
                    batch.Value.NativeModel.Quantity -= quantity;
                    ITransaction deleter = new DeleteDepletedInventoryBatchAtId(batch.Value, memoryStore);
                    deleter.Execute();
                    break;
                }
            }
        }
    }
}
