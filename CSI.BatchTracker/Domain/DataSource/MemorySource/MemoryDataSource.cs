using CSI.BatchTracker.DataSource.Contracts;
using CSI.BatchTracker.Domain.DataSource.Contracts;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.Experimental;
using CSI.BatchTracker.Storage.Contracts;
using CSI.BatchTracker.Storage.MemoryStore;
using CSI.BatchTracker.Storage.MemoryStore.Transactions.InventoryManagement;
using CSI.BatchTracker.Storage.MemoryStore.Transactions.RecordAquisition;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CSI.BatchTracker.Domain.DataSource.MemorySource
{
    public class MemoryDataSource : IDataSource
    {
        DataStore store;
        MemoryStoreContext memoryStore;
        ObservableCollection<InventoryBatch> inventoryRepository;
        ObservableCollection<ReceivedBatch> receivedBatchRepository;

        IBatchOperatorSource batchOperatorSource;
        IReceivedBatchSource receivedBatchSource;

        public Dictionary<int, int> CurrentInventoryIdMappings { get; private set; }

        public ObservableCollection<LoggedBatch> BatchLedger { get; private set; }
        public ObservableCollection<ReceivedBatch> ReceivingLedger { get; private set; }

        public MemoryDataSource(DataStore store, MemoryStoreContext memoryStore)
        {
            this.store = store;
            this.memoryStore = memoryStore;
            batchOperatorSource = new MemoryBatchOperatorSource(this.memoryStore);
            inventoryRepository = new ObservableCollection<InventoryBatch>();
            receivedBatchSource = new MemoryReceivedBatchSource(this.memoryStore);
            receivedBatchRepository = new ObservableCollection<ReceivedBatch>();
            CurrentInventoryIdMappings = new Dictionary<int, int>();
            BatchLedger = store.LoggedBatches;
        }

        public ObservableCollection<BatchOperator> OperatorRepository { get { return batchOperatorSource.OperatorRepository; } }
        public Dictionary<int, int> BatchOperatorIdMappings { get { return batchOperatorSource.BatchOperatorIdMappings; } }
        public ObservableCollection<ReceivedBatch> ReceivedBatchRepository { get { return receivedBatchSource.ReceivedBatchRepository; } }
        public Dictionary<int, int> ReceivedBatchIdMappings { get { return receivedBatchSource.ReceivedBatchIdMappings; } }

        public void ReceiveInventory(ReceivedBatch batch)
        {
            Entity<ReceivedBatch> entity = new Entity<ReceivedBatch>(batch);
            UpdateReceivingLedger(entity);
            UpdateActiveIventoryFromReceivedBatch(entity);
        }

        void UpdateReceivingLedger(Entity<ReceivedBatch> entity)
        {
            ITransaction adder = new AddReceivedBatchToReceivingLedgerTransaction(entity, memoryStore);
            adder.Execute();
        }

        void UpdateActiveIventoryFromReceivedBatch(Entity<ReceivedBatch> receivedEntity)
        {
            InventoryBatch batch = new InventoryBatch(
                receivedEntity.NativeModel.ColorName,
                receivedEntity.NativeModel.BatchNumber,
                receivedEntity.NativeModel.ActivityDate,
                receivedEntity.NativeModel.Quantity
            );

            Entity<InventoryBatch> entity = new Entity<InventoryBatch>(batch);
            ITransaction adder = new AddReceivedBatchToInventoryTransaction(entity, memoryStore);
            adder.Execute();
        }

        public ObservableCollection<InventoryBatch> InventoryRepository
        {
            get
            {
                UpdateInventoryRepository();
                return inventoryRepository;
            }
            private set
            {
                inventoryRepository = value;
            }
        }

        void UpdateInventoryRepository()
        {
            ITransaction finder = new ListCurrentInventoryTransaction(memoryStore);
            finder.Execute();

            inventoryRepository.Clear();
            CurrentInventoryIdMappings.Clear();
            int i = 0;

            foreach (Entity<InventoryBatch> entity in finder.Results)
            {
                inventoryRepository.Add(entity.NativeModel);
                CurrentInventoryIdMappings.Add(i, entity.SystemId);
                i++;
            }
        }

        public void ImplementBatch(string batchNumber, DateTime implementationDate, BatchOperator batchOperator)
        {
            ITransaction finder = new ListCurrentInventoryTransaction(memoryStore);
            finder.Execute();

            List<Entity<InventoryBatch>> inventoryBatches = BuildInventoryList(finder.Results);

            for (int i = 0; i < inventoryBatches.Count; ++i)
            {
                InventoryBatch batch = inventoryBatches[i].NativeModel;

                if (batch.BatchNumber == batchNumber)
                {
                    LogBatchToLedger(batch, implementationDate, batchOperator);
                    DeductInventory(inventoryBatches[i]);
                }
            }            
        }

        List<Entity<InventoryBatch>> BuildInventoryList(List<IEntity> entityList)
        {
            List<Entity<InventoryBatch>> inventoryList = new List<Entity<InventoryBatch>>();

            foreach (Entity<InventoryBatch> entity in entityList)
            {
                inventoryList.Add(entity);
            }

            return inventoryList;
        }

        void LogBatchToLedger(InventoryBatch batch, DateTime implementationDate, BatchOperator batchOperator)
        {
            LoggedBatch logged = new LoggedBatch(
                batch.ColorName,
                batch.BatchNumber,
                implementationDate,
                batchOperator
            );

            BatchLedger.Add(logged);
        }

        void DeductInventory(Entity<InventoryBatch> entity)
        {
            entity.NativeModel.DeductQuantity(1);
            RemoveBatchFromInventoryIfDepleted(entity);
        }

        void RemoveBatchFromInventoryIfDepleted(Entity<InventoryBatch> entity)
        {
            if (entity.NativeModel.Quantity == 0)
            {
                UpdateInventoryRepository();
                memoryStore.CurrentInventory.Remove(entity.SystemId);
            }
        }

        public void SaveOperator(BatchOperator batchOperator)
        {
            batchOperatorSource.SaveOperator(batchOperator);
        }

        public void UpdateOperator(int id, BatchOperator batchOperator)
        {
            batchOperatorSource.UpdateOperator(id, batchOperator);
        }

        public void DeleteBatchOperator(int id)
        {
            batchOperatorSource.DeleteBatchOperator(id);
        }

        public BatchOperator FindBatchOperator(int id)
        {
            return batchOperatorSource.FindBatchOperator(id);
        }

        public void FindAllBatchOperators()
        {
            batchOperatorSource.FindAllBatchOperators();
        }

        public void SaveReceivedBatch(ReceivedBatch receivedBatch)
        {
            receivedBatchSource.SaveReceivedBatch(receivedBatch);
        }

        public void UpdateReceivedBatch(int id, ReceivedBatch batch)
        {
            receivedBatchSource.UpdateReceivedBatch(id, batch);
        }

        public void DeleteReceivedBatch(int id)
        {
            receivedBatchSource.DeleteReceivedBatch(id);
        }

        public ReceivedBatch FindReceivedBatchById(int id)
        {
            return receivedBatchSource.FindReceivedBatchById(id);
        }

        public void FindReceivedBatchesByPONumber(int poNumber)
        {
            receivedBatchSource.FindReceivedBatchesByPONumber(poNumber);
        }

        public void FindReceivedBatchesByDate(DateTime date)
        {
            receivedBatchSource.FindReceivedBatchesByDate(date);
        }

        public void FindAllReceivedBatches()
        {
            receivedBatchSource.FindAllReceivedBatches();
        }
    }
}
