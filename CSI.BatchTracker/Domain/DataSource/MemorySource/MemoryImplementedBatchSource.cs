﻿using CSI.BatchTracker.Domain.DataSource.Contracts;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.Storage.Contracts;
using CSI.BatchTracker.Storage.MemoryStore;
using CSI.BatchTracker.Storage.MemoryStore.Transactions.InventoryManagement;
using CSI.BatchTracker.Storage.MemoryStore.Transactions.RecordAquisition;
using System;
using System.Collections.ObjectModel;

namespace CSI.BatchTracker.Domain.DataSource.MemorySource
{
    public class MemoryImplementedBatchSource : IImplementedBatchSource
    {
        public ObservableCollection<LoggedBatch> ImplementedBatchLedger { get; private set; }

        MemoryStoreContext memoryStore;
        IActiveInventorySource inventorySource;

        public MemoryImplementedBatchSource(MemoryStoreContext memoryStore, IActiveInventorySource inventorySource)
        {
            this.memoryStore = memoryStore;
            this.inventorySource = inventorySource;
            ImplementedBatchLedger = new ObservableCollection<LoggedBatch>();
        }

        public void AddBatchToImplementationLedger(string batchNumber, DateTime date, BatchOperator batchOperator)
        {
            InventoryBatch inventoryBatch = inventorySource.FindInventoryBatchByBatchNumber(batchNumber);

            if (inventoryBatch.BatchNumber == batchNumber)
            {
                LoggedBatch implemented = new LoggedBatch(
                    inventoryBatch.ColorName,
                    inventoryBatch.BatchNumber,
                    date,
                    batchOperator
                );

                Entity<LoggedBatch> entity = new Entity<LoggedBatch>(implemented);
                ITransaction adder = new TransferInventoryBatchToImplementedBatchLedgerTransaction(entity, memoryStore);
                adder.Execute();
                UpdateImplementationLedger();
            }
        }

        public void UpdateImplementationLedger()
        {
            ImplementedBatchLedger.Clear();
            ITransaction finder = new ListImplementedBatchTransaction(memoryStore);
            finder.Execute();

            foreach (IEntity entity in finder.Results)
            {
                Entity<LoggedBatch> loggedEntity = entity as Entity<LoggedBatch>;
                ImplementedBatchLedger.Add(loggedEntity.NativeModel);
            }
        }

        public void UndoImplementedBatch(int targetId)
        {
            ITransaction finder = new FindBatchInImplementedLedgerTransaction(targetId, memoryStore);
            finder.Execute();

            if (finder.Results.Count > 0)
            {
                Entity<LoggedBatch> implemented = finder.Results[0] as Entity<LoggedBatch>;
                ITransaction undo = new UndoImplementedBatchCommittedToLedgerTransaction(implemented, memoryStore);
                undo.Execute();
                UpdateImplementationLedger();
            }            
        }
    }
}