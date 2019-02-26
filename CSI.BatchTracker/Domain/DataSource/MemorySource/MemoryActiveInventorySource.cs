using CSI.BatchTracker.Domain.DataSource.Contracts;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.Storage.Contracts;
using CSI.BatchTracker.Storage.MemoryStore;
using CSI.BatchTracker.Storage.MemoryStore.Transactions.InventoryManagement;
using CSI.BatchTracker.Storage.MemoryStore.Transactions.RecordAquisition;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Domain.DataSource.MemorySource
{
    public class MemoryActiveInventorySource : IActiveInventorySource
    {
        public ObservableCollection<InventoryBatch> CurrentInventory { get; private set; }
        public Dictionary<int, string> CurrentInventoryBatchNumberMappings { get; private set; }

        MemoryStoreContext memoryStore;

        public MemoryActiveInventorySource(MemoryStoreContext memoryStore)
        {
            this.memoryStore = memoryStore;
            CurrentInventory = new ObservableCollection<InventoryBatch>();
            CurrentInventoryBatchNumberMappings = new Dictionary<int, string>();
        }

        public void AddReceivedBatchToInventory(ReceivedBatch batch)
        {
            InventoryBatch inventoryBatch = new InventoryBatch(
                batch.ColorName,
                batch.BatchNumber,
                batch.ActivityDate,
                batch.Quantity
            );

            SubmitToActiveInventory(inventoryBatch);
        }

        void SubmitToActiveInventory(InventoryBatch batch)
        {
            
        }

        public InventoryBatch FindInventoryBatchByBatchNumber(string batchNumber)
        {
            UpdateActiveInventory();
        }

        public void UpdateActiveInventory()
        {
            ITransaction finder = new ListCurrentInventoryTransaction(memoryStore);
            finder.Execute();

            CurrentInventory.Clear();
            CurrentInventoryBatchNumberMappings.Clear();

            foreach (KeyValuePair<int, Entity<InventoryBatch>> entity in memoryStore.CurrentInventory)
            {
                CurrentInventory.Add(entity.Value.NativeModel);
                CurrentInventoryBatchNumberMappings.Add(entity.Value.SystemId, entity.Value.NativeModel.BatchNumber);
            }
        }
    }
}
