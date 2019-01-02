using CSI.BatchTracker.Contracts;
using CSI.BatchTracker.Storage;
using CSI.BatchTracker.Storage.MemoryStore;
using CSI.BatchTracker.Storage.MemoryStore.Transactions.BatchOperators;
using CSI.BatchTracker.Storage.MemoryStore.Transactions.InventoryManagement;
using CSI.BatchTracker.Storage.MemoryStore.Transactions.RecordAquisition;
using CSI.BatchTracker.Domain.DataSource.Repositores;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.Experimental;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Domain.DataSource.Repositories
{
    public class DataSourceRepository : IDataSource
    {
        DataStore store;
        MemoryStore memoryStore;
        ObservableCollection<BatchOperator> operatorRepository;
        ObservableCollection<InventoryBatch> inventoryRepository;

        public Dictionary<int, int> CurrentInventoryIdMappings { get; private set; }
        public Dictionary<int, int> BatchOperatorIdMappings { get; private set; }

        public ObservableCollection<LoggedBatch> BatchLedger { get; private set; }

        public DataSourceRepository(DataStore store, MemoryStore memoryStore)
        {
            this.store = store;
            this.memoryStore = memoryStore;
            operatorRepository = new ObservableCollection<BatchOperator>();
            inventoryRepository = new ObservableCollection<InventoryBatch>();
            BatchOperatorIdMappings = new Dictionary<int, int>();
            CurrentInventoryIdMappings = new Dictionary<int, int>();
            BatchLedger = store.LoggedBatches;
        }

        public ObservableCollection<BatchOperator> OperatorRepository
        {
            get
            {
                UpdateOperatorRepository();
                return operatorRepository;
            }
            private set
            {
                operatorRepository = value;
            }
        }

        void UpdateOperatorRepository()
        {
            ITransaction finder = new ListBatchOperatorsTransaction(memoryStore);
            finder.Execute();

            operatorRepository.Clear();
            BatchOperatorIdMappings.Clear();
            int i = 0;

            foreach (Entity<BatchOperator> batchOperator in finder.Results)
            {
                operatorRepository.Add(batchOperator.NativeModel);
                BatchOperatorIdMappings.Add(i, batchOperator.SystemId);
                i++;
            }
        }

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

        public void SaveOperator(BatchOperator batchOperator)
        {
            ITransaction adder = new AddBatchOperatorTransaction(new Entity<BatchOperator>(batchOperator), memoryStore);
            adder.Execute();
            UpdateOperatorRepository();
        }

        public void UpdateOperator(int id, BatchOperator batchOperator)
        {
            Entity<BatchOperator> entity = new Entity<BatchOperator>(id, batchOperator);
            ITransaction updater = new UpdateBatchOperatorTransaction(entity, memoryStore);
            updater.Execute();
            UpdateOperatorRepository();
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

        public BatchOperator FindBatchOperatorById(int id)
        {
            ITransaction finder = new FindBatchOperatorByIdTransaction(id, memoryStore);
            finder.Execute();
            Entity<BatchOperator> entity = (Entity<BatchOperator>)finder.Results[0];

            return entity.NativeModel;
        }

        public void DeleteBatchOperatorAtId(int id)
        {
            ITransaction remover = new DeleteBatchOperatorAtIdTransaction(id, memoryStore);
            remover.Execute();
            UpdateOperatorRepository();
        }
    }
}
