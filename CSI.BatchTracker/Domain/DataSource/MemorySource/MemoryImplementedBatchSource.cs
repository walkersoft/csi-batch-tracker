﻿using CSI.BatchTracker.Domain.DataSource.Contracts;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.Storage.Contracts;
using CSI.BatchTracker.Storage.MemoryStore;
using CSI.BatchTracker.Storage.MemoryStore.Transactions.InventoryManagement;
using CSI.BatchTracker.Storage.MemoryStore.Transactions.RecordAquisition;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CSI.BatchTracker.Domain.DataSource.MemorySource
{
    public class MemoryImplementedBatchSource : IImplementedBatchSource
    {
        public ObservableCollection<LoggedBatch> ImplementedBatchLedger { get; private set; }
        public Dictionary<int, int> ImplementedBatchIdMappings { get; private set; }

        MemoryStoreContext memoryStore;
        IActiveInventorySource inventorySource;

        public MemoryImplementedBatchSource(MemoryStoreContext memoryStore, IActiveInventorySource inventorySource)
        {
            this.memoryStore = memoryStore;
            this.inventorySource = inventorySource;
            ImplementedBatchLedger = new ObservableCollection<LoggedBatch>();
            ImplementedBatchIdMappings = new Dictionary<int, int>();
        }

        public void AddBatchToImplementationLedger(string batchNumber, DateTime date, BatchOperator batchOperator)
        {
            InventoryBatch inventoryBatch = inventorySource.FindInventoryBatchByBatchNumber(batchNumber);
            date = CreateDateTimeWithPrecisionToMinutes(date);

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
                inventorySource.DeductBatchFromInventory(batchNumber);
                UpdateImplementationLedger();
            }
        }

        DateTime CreateDateTimeWithPrecisionToMinutes(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, 0);
        }

        void UpdateImplementationLedger()
        {
            ImplementedBatchLedger.Clear();
            ITransaction finder = new ListImplementedBatchTransaction(memoryStore);
            finder.Execute();
            PopulatedImplementedBatchLedgerFromTransactionResults(finder);            
        }

        void PopulatedImplementedBatchLedgerFromTransactionResults(ITransaction transaction)
        {
            ImplementedBatchLedger.Clear();
            ImplementedBatchIdMappings.Clear();
            int i = 0;

            foreach (IEntity entity in transaction.Results)
            {
                Entity<LoggedBatch> loggedEntity = entity as Entity<LoggedBatch>;
                ImplementedBatchIdMappings.Add(i, loggedEntity.SystemId);
                ImplementedBatchLedger.Add(loggedEntity.NativeModel);
                i++;
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

            inventorySource.UpdateActiveInventory();
        }

        public ObservableCollection<LoggedBatch> GetImplementedBatchesByBatchNumber(string batchNumber)
        {
            ITransaction finder = new FindBatchesInImplementationLedgerByBatchNumberTransaction(batchNumber, memoryStore);
            finder.Execute();

            ObservableCollection<LoggedBatch> batches = new ObservableCollection<LoggedBatch>();

            foreach(IEntity entity in finder.Results)
            {
                Entity<LoggedBatch> batch = entity as Entity<LoggedBatch>;
                batches.Add(batch.NativeModel);
            }

            return batches;
        }
    }
}
