using CSI.BatchTracker.Domain.DataSource.Contracts;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.Storage.Contracts;
using CSI.BatchTracker.Storage.SQLiteStore;
using CSI.BatchTracker.Storage.SQLiteStore.Transactions.InventoryManagement;
using CSI.BatchTracker.Storage.SQLiteStore.Transactions.RecordAquisition;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CSI.BatchTracker.Domain.DataSource.SQLiteStore
{
    public sealed class SQLiteImplementedBatchSource : IImplementedBatchSource
    {
        SQLiteStoreContext sqliteStore;
        IActiveInventorySource inventorySource;
        public ObservableCollection<LoggedBatch> ImplementedBatchLedger { get; private set; }
        public Dictionary<int, int> ImplementedBatchIdMappings { get; private set; }

        public SQLiteImplementedBatchSource(SQLiteStoreContext sqliteStore, IActiveInventorySource inventorySource)
        {
            this.sqliteStore = sqliteStore;
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
                ITransaction adder = new TransferInventoryBatchToImplementedBatchLedgerTransaction(entity, sqliteStore);
                adder.Execute();
                inventorySource.DeductBatchFromInventory(batchNumber);
                UpdateImplementationLedger();
            }
        }

        DateTime CreateDateTimeWithPrecisionToMinutes(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, 0);
        }

        public ObservableCollection<LoggedBatch> GetImplementedBatchesByBatchNumber(string batchNumber)
        {
            ITransaction finder = new FindBatchesInImplementationLedgerByBatchNumberTransaction(batchNumber, sqliteStore);
            finder.Execute();
            ObservableCollection<LoggedBatch> found = new ObservableCollection<LoggedBatch>();

            foreach (IEntity result in finder.Results)
            {
                Entity<LoggedBatch> entity = result as Entity<LoggedBatch>;
                found.Add(entity.NativeModel);
            }

            return found;
        }

        public void UndoImplementedBatch(int targetId)
        {
            ITransaction finder = new FindBatchInImplementedLedgerTransaction(targetId, sqliteStore);
            finder.Execute();
            
            if (finder.Results.Count > 0)
            {
                Entity<LoggedBatch> entity = finder.Results[0] as Entity<LoggedBatch>;
                ITransaction updater = new UndoImplementedBatchCommittedToLedgerTransaction(entity, sqliteStore);
                updater.Execute();
                UpdateImplementationLedger();
                inventorySource.UpdateActiveInventory();
            }
        }

        public void UpdateImplementationLedger()
        {
            ImplementedBatchLedger.Clear();
            ITransaction finder = new ListImplementedBatchTransaction(sqliteStore);
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

        public ObservableCollection<LoggedBatch> GetConnectedBatchesAtDate(DateTime date)
        {
            ObservableCollection<LoggedBatch> connectedBatches = new ObservableCollection<LoggedBatch>();
            ITransaction finder = new ListConnectedBatchesAtDateTransaction(date, sqliteStore);
            finder.Execute();

            foreach (IEntity entity in finder.Results)
            {
                Entity<LoggedBatch> connected = entity as Entity<LoggedBatch>;
                connectedBatches.Add(connected.NativeModel);
            }

            return connectedBatches;
        }
    }
}
