using CSI.BatchTracker.Contracts;
using CSI.BatchTracker.DataSource;
using CSI.BatchTracker.DataSource.MemoryDataSource;
using CSI.BatchTracker.DataSource.MemoryDataSource.Transactions.BatchOperators;
using CSI.BatchTracker.DataSource.MemoryDataSource.Transactions.InventoryManagement;
using CSI.BatchTracker.DataSource.MemoryDataSource.Transactions.RecordAquisition;
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

        public ObservableCollection<LoggedBatch> BatchLedger { get; private set; }

        public DataSourceRepository(DataStore store, MemoryStore memoryStore)
        {
            this.store = store;
            this.memoryStore = memoryStore;
            operatorRepository = new ObservableCollection<BatchOperator>();
            inventoryRepository = new ObservableCollection<InventoryBatch>();
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

            foreach (Entity<BatchOperator> batchOperator in finder.Results)
            {
                operatorRepository.Add(batchOperator.NativeModel);
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

            foreach (Entity<InventoryBatch> entity in finder.Results)
            {
                inventoryRepository.Add(entity.NativeModel);
            }
        }

        public void SaveOperator(BatchOperator batchOperator)
        {
            ITransaction adder = new AddBatchOperatorTransaction(new Entity<BatchOperator>(batchOperator), memoryStore);
            adder.Execute();
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
    }
}
