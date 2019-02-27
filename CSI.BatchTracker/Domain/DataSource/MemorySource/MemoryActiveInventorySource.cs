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
        public Dictionary<string, int> CurrentInventoryBatchNumberToIdMappings { get; private set; }

        MemoryStoreContext memoryStore;

        public MemoryActiveInventorySource(MemoryStoreContext memoryStore)
        {
            this.memoryStore = memoryStore;
            CurrentInventory = new ObservableCollection<InventoryBatch>();
            CurrentInventoryBatchNumberToIdMappings = new Dictionary<string, int>();
        }

        public void AddReceivedBatchToInventory(ReceivedBatch batch)
        {
            InventoryBatch inventoryBatch = new InventoryBatch(
                batch.ColorName,
                batch.BatchNumber,
                batch.ActivityDate,
                batch.Quantity
            );

            Entity<InventoryBatch> entity = new Entity<InventoryBatch>(inventoryBatch);
            ITransaction adder = new AddReceivedBatchToInventoryTransaction(entity, memoryStore);
            adder.Execute();
            UpdateActiveInventory();
        }

        void UpdateActiveInventory()
        {
            ITransaction finder = new ListCurrentInventoryTransaction(memoryStore);
            finder.Execute();

            CurrentInventory.Clear();
            CurrentInventoryBatchNumberToIdMappings.Clear();

            foreach (KeyValuePair<int, Entity<InventoryBatch>> entity in memoryStore.CurrentInventory)
            {
                CurrentInventory.Add(entity.Value.NativeModel);
                CurrentInventoryBatchNumberToIdMappings.Add(entity.Value.NativeModel.BatchNumber, entity.Value.SystemId);
            }
        }

        public InventoryBatch FindInventoryBatchByBatchNumber(string batchNumber)
        {
            UpdateActiveInventory();
            InventoryBatch batch = new InventoryBatch();

            foreach (InventoryBatch inventoryBatch in CurrentInventory)
            {
                if (inventoryBatch.BatchNumber == batchNumber)
                {
                    batch = inventoryBatch;
                }
            }

            return batch;
        }

        public void DeductBatchFromInventory(string batchNumber)
        {
            InventoryBatch batch = FindInventoryBatchByBatchNumber(batchNumber);
            batch.DeductQuantity(1);

            int mappedId = CurrentInventoryBatchNumberToIdMappings[batchNumber];
            Entity<InventoryBatch> entity = new Entity<InventoryBatch>(mappedId, batch);

            ITransaction updater = new EditBatchInCurrentInventoryTransaction(entity, memoryStore);
            updater.Execute();
            UpdateActiveInventory();
        }
    }
}
