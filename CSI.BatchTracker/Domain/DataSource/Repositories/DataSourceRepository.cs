﻿using CSI.BatchTracker.Contracts;
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
            for (int i = 0; i < InventoryRepository.Count; ++i)
            {
                InventoryBatch batch = InventoryRepository[i];

                if (batch.BatchNumber == batchNumber)
                {
                    LogBatchToLedger(batch, implementationDate, batchOperator);
                    DeductInventory(batch);
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

        void DeductInventory(InventoryBatch batch)
        {
            batch.DeductQuantity(1);
            RemoveBatchFromInventoryIfDepleted(batch);
        }

        void RemoveBatchFromInventoryIfDepleted(InventoryBatch batch)
        {
            if (batch.Quantity == 0)
            {
                //find all current, match with batch number, and then delete.
            }
        }
    }
}
