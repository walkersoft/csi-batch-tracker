using CSI.BatchTracker.Domain.DataSource.Contracts;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.Storage.Contracts;
using CSI.BatchTracker.Storage.SQLiteStore;
using CSI.BatchTracker.Storage.SQLiteStore.Transactions.InventoryManagement;
using CSI.BatchTracker.Storage.SQLiteStore.Transactions.RecordAquisition;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Domain.DataSource.SQLiteStore
{
    public sealed class SQLiteReceivedBatchSource : IReceivedBatchSource
    {
        public ObservableCollection<ReceivedBatch> ReceivedBatchRepository { get; private set; }
        public Dictionary<int, int> ReceivedBatchIdMappings { get; private set; }
        SQLiteStoreContext sqliteStore;
        IActiveInventorySource inventorySource;

        public SQLiteReceivedBatchSource(SQLiteStoreContext sqliteStore, IActiveInventorySource inventorySource)
        {
            this.sqliteStore = sqliteStore;
            this.inventorySource = inventorySource;
            ReceivedBatchRepository = new ObservableCollection<ReceivedBatch>();
            ReceivedBatchIdMappings = new Dictionary<int, int>();
        }

        void UpdateRepositoryAndMappings()
        {
            ITransaction finder = new ListReceivingLedgerTransaction(sqliteStore);
            finder.Execute();
            ReceivedBatchRepository.Clear();

            for (int i = 0; i < finder.Results.Count; i++)
            { 
                Entity<ReceivedBatch> entity = finder.Results[i] as Entity<ReceivedBatch>;
                ReceivedBatchRepository.Add(entity.NativeModel);
            }

            UpdateMappingsFromTransactionResults(finder.Results);
        }

        void UpdateMappingsFromTransactionResults(List<IEntity> results)
        {
            ReceivedBatchIdMappings.Clear();

            for (int i = 0; i < results.Count; i++)
            {
                Entity<ReceivedBatch> entity = results[i] as Entity<ReceivedBatch>;
                ReceivedBatchIdMappings.Add(i, entity.SystemId);
            }
        }

        public void DeleteReceivedBatch(int id)
        {
            ITransaction deleter = new DeleteReceivedBatchAtIdTransaction(id, sqliteStore);
            deleter.Execute();
            inventorySource.UpdateActiveInventory();
            UpdateRepositoryAndMappings();
        }

        public void FindAllReceivedBatches()
        {
            UpdateRepositoryAndMappings();
        }

        public ReceivedBatch FindReceivedBatchById(int id)
        {
            ITransaction finder = new FindBatchInReceivingLedgerByIdTransaction(id, sqliteStore);
            finder.Execute();
            Entity<ReceivedBatch> entity = finder.Results[0] as Entity<ReceivedBatch>;

            return entity.NativeModel;
        }

        public void FindReceivedBatchesByDate(DateTime date)
        {
            ITransaction finder = new FindBatchesInReceivingLedgerByDateTransaction(date, sqliteStore);
            ReceivedBatchRepository = ExecuteTransactionAndBuildReceivedBatchObservableCollection(finder);
            UpdateMappingsFromTransactionResults(finder.Results);
        }

        public void FindReceivedBatchesByPONumber(int poNumber)
        {
            ITransaction finder = new FindBatchesInReceivingLedgerByPONumberTransaction(poNumber, sqliteStore);
            ReceivedBatchRepository = ExecuteTransactionAndBuildReceivedBatchObservableCollection(finder);
            UpdateMappingsFromTransactionResults(finder.Results);
        }

        public EditablePurchaseOrder GetPurchaseOrderForEditing(int poNumber)
        {
            ITransaction finder = new FindBatchesInReceivingLedgerByPONumberTransaction(poNumber, sqliteStore);
            ReceivedBatchRepository = ExecuteTransactionAndBuildReceivedBatchObservableCollection(finder);
            UpdateMappingsFromTransactionResults(finder.Results);
            EditablePurchaseOrder editablePo = new EditablePurchaseOrder(ReceivedBatchRepository, ReceivedBatchIdMappings);

            return editablePo;
        }

        ObservableCollection<ReceivedBatch> ExecuteTransactionAndBuildReceivedBatchObservableCollection(ITransaction transaction)
        {
            ObservableCollection<ReceivedBatch> receivedBatches = new ObservableCollection<ReceivedBatch>();
            transaction.Execute();

            foreach (IEntity result in transaction.Results)
            {
                Entity<ReceivedBatch> entity = result as Entity<ReceivedBatch>;
                receivedBatches.Add(entity.NativeModel);
            }

            return receivedBatches;
        }

        public ObservableCollection<ReceivedBatch> GetReceivedBatchesByBatchNumber(string batchNumber)
        {
            ITransaction finder = new FindBatchesInReceivingLedgerByBatchNumberTransaction(batchNumber, sqliteStore);
            return ExecuteTransactionAndBuildReceivedBatchObservableCollection(finder);
        }

        public ObservableCollection<ReceivedBatch> GetReceivedBatchesByPONumber(int poNumber)
        {
            ITransaction finder = new FindBatchesInReceivingLedgerByPONumberTransaction(poNumber, sqliteStore);
            return ExecuteTransactionAndBuildReceivedBatchObservableCollection(finder);
        }

        public ObservableCollection<ReceivedBatch> GetReceivedBatchesbySpecificDate(DateTime specificDate)
        {
            ITransaction finder = new FindBatchesInReceivingLedgerByDateTransaction(specificDate, sqliteStore);
            return ExecuteTransactionAndBuildReceivedBatchObservableCollection(finder);
        }

        public ObservableCollection<ReceivedBatch> GetReceivedBatchesWithinDateRange(DateTime startDate, DateTime endDate)
        {
            ITransaction finder = new FindBatchesInReceivingLedgerByDateRangeTransaction(startDate, endDate, sqliteStore);
            return ExecuteTransactionAndBuildReceivedBatchObservableCollection(finder);
        }

        public void SaveReceivedBatch(ReceivedBatch receivedBatch)
        {
            Entity<ReceivedBatch> entity = new Entity<ReceivedBatch>(receivedBatch);
            ITransaction adder = new AddReceivedBatchToReceivingLedgerTransaction(entity, sqliteStore);
            adder.Execute();
            inventorySource.AddReceivedBatchToInventory(receivedBatch);
            UpdateRepositoryAndMappings();
        }

        public void UpdateReceivedBatch(int id, ReceivedBatch batch)
        {
            throw new NotImplementedException();
        }
    }
}
