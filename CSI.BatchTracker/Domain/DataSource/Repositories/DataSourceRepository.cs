using CSI.BatchTracker.Contracts;
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

        public IRepository<Entity<InventoryBatch>> InventoryRepository { get; private set; }
        public IRepository<Entity<BatchOperator>> OperatorRepository { get; private set; }
        public ObservableCollection<LoggedBatch> BatchLedger { get; private set; }

        public DataSourceRepository(DataStore store)
        {
            this.store = store;
            InventoryRepository = new InventoryBatchRepository(store);
            OperatorRepository = new BatchOperatorRepository(store);
            BatchLedger = store.LoggedBatches;
        }

        public void ReceiveInventory(ReceivedBatch batch)
        {
            store.ReceivedBatches.Add(batch);
            store.CalculateInventory();
        }

        public void SaveOperator(BatchOperator batchOperator)
        {
            OperatorRepository.Save(new Entity<BatchOperator>(batchOperator));
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
