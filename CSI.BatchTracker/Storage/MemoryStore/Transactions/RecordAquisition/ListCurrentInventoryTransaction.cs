using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using System.Collections.Generic;

namespace CSI.BatchTracker.Storage.MemoryStore.Transactions.RecordAquisition
{
    public sealed class ListCurrentInventoryTransaction : MemoryDataSourceTransaction
    {
        MemoryStoreContext store;

        public ListCurrentInventoryTransaction(MemoryStoreContext store)
        {
            this.store = store;
        }

        public override void Execute()
        {
            Results.Clear();

            foreach (KeyValuePair<int, Entity<InventoryBatch>> entity in store.CurrentInventory)
            {
                Results.Add(entity.Value);
            }
        }
    }
}
