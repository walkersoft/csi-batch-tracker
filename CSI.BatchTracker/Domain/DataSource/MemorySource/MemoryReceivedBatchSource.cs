using CSI.BatchTracker.Domain.DataSource.Contracts;
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
    public class MemoryReceivedBatchSource : IReceivedBatchSource
    {
        public Dictionary<int, int> ReceivedBatchIdMappings { get; private set; }

        MemoryStoreContext memoryStore;
        IActiveInventorySource inventorySource;

        public MemoryReceivedBatchSource(MemoryStoreContext memoryStore, IActiveInventorySource inventorySource)
        {
            this.memoryStore = memoryStore;
            this.inventorySource = inventorySource;
            ReceivedBatchIdMappings = new Dictionary<int, int>();
            ReceivedBatchRepository = new ObservableCollection<ReceivedBatch>();
        }

        ObservableCollection<ReceivedBatch> receivedBatchRepository;
        public ObservableCollection<ReceivedBatch> ReceivedBatchRepository
        {
            get
            {
                return receivedBatchRepository;
            }
            private set
            {
                receivedBatchRepository = value;
            }
        }

        public void SaveReceivedBatch(ReceivedBatch receivedBatch)
        {
            Entity<ReceivedBatch> entity = new Entity<ReceivedBatch>(RecreateReceivedBatch(receivedBatch));
            ITransaction adder = new AddReceivedBatchToReceivingLedgerTransaction(entity, memoryStore);
            adder.Execute();
            UpdateReceivedBatchRepository();
            inventorySource.AddReceivedBatchToInventory(receivedBatch);
        }

        ReceivedBatch RecreateReceivedBatch(ReceivedBatch batch)
        {
            ReceivedBatch newBatch = new ReceivedBatch(
                batch.ColorName,
                batch.BatchNumber,
                batch.ActivityDate,
                batch.Quantity,
                batch.PONumber,
                batch.ReceivingOperator
            );

            return newBatch;
        }

        void UpdateReceivedBatchRepository()
        {
            ITransaction finder = new ListReceivingLedgerTransaction(memoryStore);
            ExecuteFinderTransactionAndPopulateRepositoryAndMappings(finder);
        }

        void ExecuteFinderTransactionAndPopulateRepositoryAndMappings(ITransaction finder)
        {
            finder.Execute();
            PopulateRepositoryAndMappingsFromTransactionResults(finder);
        }

        void PopulateRepositoryAndMappingsFromTransactionResults(ITransaction transaction)
        {
            receivedBatchRepository.Clear();
            ReceivedBatchIdMappings.Clear();
            int i = 0;

            foreach (Entity<ReceivedBatch> received in transaction.Results)
            {
                receivedBatchRepository.Add(received.NativeModel);
                ReceivedBatchIdMappings.Add(i, received.SystemId);
                i++;
            }
        }

        public ReceivedBatch FindReceivedBatchById(int id)
        {
            ITransaction finder = new FindBatchInReceivingLedgerByIdTransaction(id, memoryStore);
            finder.Execute();
            Entity<ReceivedBatch> entity = (Entity<ReceivedBatch>)finder.Results[0];

            return entity.NativeModel;
        }

        public void UpdateReceivedBatch(int id, ReceivedBatch batch)
        {
            Entity<ReceivedBatch> entity = new Entity<ReceivedBatch>(id, batch);
            ITransaction updater = new EditBatchInReceivingLedgerTransaction(entity, memoryStore);
            updater.Execute();
        }

        public void DeleteReceivedBatch(int id)
        {
            ITransaction remover = new DeleteReceivedBatchAtIdTransaction(id, memoryStore);
            remover.Execute();
            UpdateReceivedBatchRepository();
        }

        public void FindReceivedBatchesByPONumber(int poNumber)
        {
            ITransaction finder = new FindBatchesInReceivingLedgerByPONumberTransaction(poNumber, memoryStore);
            ExecuteFinderTransactionAndPopulateRepositoryAndMappings(finder);
        }

        public void FindReceivedBatchesByDate(DateTime date)
        {
            ITransaction finder = new FindBatchesInReceivingLedgerByDateTransaction(date, memoryStore);
            ExecuteFinderTransactionAndPopulateRepositoryAndMappings(finder);
        }

        public void FindAllReceivedBatches()
        {
            UpdateReceivedBatchRepository();
        }

        public ObservableCollection<ReceivedBatch> GetReceivedBatchesByBatchNumber(string batchNumber)
        {
            ITransaction finder = new FindBatchesInReceivingLedgerByBatchNumberTransaction(batchNumber, memoryStore);
            return ExecuteFinderAndBuildObservableCollectionFromTransactionResults(finder);
        }

        ObservableCollection<ReceivedBatch> ExecuteFinderAndBuildObservableCollectionFromTransactionResults(ITransaction transaction)
        {
            ObservableCollection<ReceivedBatch> batches = new ObservableCollection<ReceivedBatch>();
            transaction.Execute();

            foreach (IEntity entity in transaction.Results)
            {
                Entity<ReceivedBatch> batch = entity as Entity<ReceivedBatch>;
                batches.Add(batch.NativeModel);
            }

            return batches;
        }

        public ObservableCollection<ReceivedBatch> GetReceivedBatchesWithinDateRange(DateTime startDate, DateTime endDate)
        {
            ITransaction finder = new FindBatchesInReceivingLedgerByDateRangeTransaction(startDate, endDate, memoryStore);
            return ExecuteFinderAndBuildObservableCollectionFromTransactionResults(finder);
        }

        public ObservableCollection<ReceivedBatch> GetReceivedBatchesByPONumber(int poNumber)
        {
            ITransaction finder = new FindBatchesInReceivingLedgerByPONumberTransaction(poNumber, memoryStore);
            return ExecuteFinderAndBuildObservableCollectionFromTransactionResults(finder);
        }

        public ObservableCollection<ReceivedBatch> GetReceivedBatchesbySpecificDate(DateTime specificDate)
        {
            ITransaction finder = new FindBatchesInReceivingLedgerByDateTransaction(specificDate, memoryStore);
            return ExecuteFinderAndBuildObservableCollectionFromTransactionResults(finder);
        }
    }
}
