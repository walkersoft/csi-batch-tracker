using CSI.BatchTracker.Contracts;
using CSI.BatchTracker.DataSource;
using CSI.BatchTracker.DataSource.MemoryDataSource;
using CSI.BatchTracker.DataSource.MemoryDataSource.Transactions.BatchOperators;
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

        public IRepository<Entity<InventoryBatch>> InventoryRepository { get; private set; }
        public ObservableCollection<LoggedBatch> BatchLedger { get; private set; }

        public DataSourceRepository(DataStore store, MemoryStore memoryStore)
        {
            this.store = store;
            this.memoryStore = memoryStore;
            InventoryRepository = new InventoryBatchRepository(store);
            BatchLedger = store.LoggedBatches;
        }

        public ObservableCollection<BatchOperator> OperatorRepository
        {
            get
            {
                if (operatorRepository == null)
                {
                    operatorRepository = new ObservableCollection<BatchOperator>();
                }

                UpdateOperatorRepository();

                return operatorRepository;
            }
            set
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
            store.ReceivedBatches.Add(batch);
            store.CalculateInventory();
        }

        public void SaveOperator(BatchOperator batchOperator)
        {
            ITransaction adder = new AddBatchOperatorTransaction(new Entity<BatchOperator>(batchOperator), memoryStore);
            adder.Execute();
            UpdateOperatorRepository();
        }

        public void ImplementBatch(string batchNumber, DateTime implementationDate, BatchOperator batchOperator)
        {
            List<Entity<InventoryBatch>> inventoryBatches = InventoryRepository.FindAll();

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
                InventoryRepository.Delete(entity.SystemId);
            }
        }
    }
}
