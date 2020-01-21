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
            ITransaction finder = new FindBatchInReceivingLedgerByIdTransaction(id, sqliteStore);
            finder.Execute();

            if (finder.Results.Count > 0)
            {
                Entity<ReceivedBatch> originalEntity = finder.Results[0] as Entity<ReceivedBatch>;
                Entity<ReceivedBatch> updatedEntity = CreateUpdatedEntityFromOriginal(originalEntity, batch);
                ITransaction updater = new EditBatchInReceivingLedgerTransaction(updatedEntity, sqliteStore);
                UpdateInventoryBatches(originalEntity, updatedEntity);
                UpdateImplementedBatches(originalEntity, updatedEntity);
                updater.Execute();
            }
        }

        Entity<ReceivedBatch> CreateUpdatedEntityFromOriginal(Entity<ReceivedBatch> original, ReceivedBatch updated)
        {
            ReceivedBatch updatedBatch = new ReceivedBatch(
                updated.ColorName,
                updated.BatchNumber,
                updated.ActivityDate,
                updated.Quantity,
                updated.PONumber,
                updated.ReceivingOperator
            );

            return new Entity<ReceivedBatch>(original.SystemId, updatedBatch);
        }

        void UpdateInventoryBatches(Entity<ReceivedBatch> original, Entity<ReceivedBatch> updated)
        {
            ITransaction deleter = null;
            ProcessColorAndBatchNumberUpdates(original, updated);

            if (BatchDoesNotExistInInventory(updated.NativeModel.BatchNumber))
            {
                InventoryBatch inventoryBatch = new InventoryBatch(
                    updated.NativeModel.ColorName,
                    updated.NativeModel.BatchNumber,
                    updated.NativeModel.ActivityDate,
                    updated.NativeModel.Quantity
                );

                ITransaction adder = new AddReceivedBatchToInventoryTransaction(new Entity<InventoryBatch>(inventoryBatch), sqliteStore);
                adder.Execute();
            }

            ITransaction finder = new ListCurrentInventoryTransaction(sqliteStore);
            finder.Execute();

            foreach (IEntity entity in finder.Results)
            {
                Entity<InventoryBatch> inventoryEntity = entity as Entity<InventoryBatch>;

                if (inventoryEntity.NativeModel.BatchNumber == original.NativeModel.BatchNumber)
                {
                    inventoryEntity.NativeModel.Quantity = GetAdjustedInventoryQuantity(
                        inventoryEntity.NativeModel.BatchNumber,
                        original.NativeModel.Quantity,
                        updated.NativeModel.Quantity
                    );

                    if (inventoryEntity.NativeModel.Quantity == 0)
                    {
                        deleter = new DeleteDepletedInventoryBatchAtId(inventoryEntity, sqliteStore);
                    }
                }
            }

            if (deleter != null)
            {
                deleter.Execute();
            }

            MergeCommonBatches(finder);
        }

        void MergeCommonBatches(ITransaction finder)
        {
            List<InventoryBatch> merged = new List<InventoryBatch>();

            foreach (IEntity result in finder.Results)
            {
                Entity<InventoryBatch> currentEntity = result as Entity<InventoryBatch>;
                InventoryBatch currentBatch = currentEntity.NativeModel;
                bool batchNotFound = true;

                for (int i = 0; i < merged.Count; i++)
                {
                    if (merged[i].BatchNumber == currentBatch.BatchNumber)
                    {
                        merged[i].AddQuantity(currentBatch.Quantity);
                        batchNotFound = false;
                        continue;
                    }
                }

                if (batchNotFound)
                {
                    merged.Add(currentBatch);
                }
            }

            foreach (InventoryBatch batch in merged)
            {
                ITransaction deleter = new DeleteFromActiveInventoryAtBatchNumberTransaction(batch.BatchNumber, sqliteStore);
                deleter.Execute();
                ITransaction adder = new AddReceivedBatchToInventoryTransaction(new Entity<InventoryBatch>(batch), sqliteStore);
                adder.Execute();
            }
        }

        void ProcessColorAndBatchNumberUpdates(Entity<ReceivedBatch> original, Entity<ReceivedBatch> updated)
        {
            ITransaction finder = new ListCurrentInventoryTransaction(sqliteStore);
            finder.Execute();

            foreach (IEntity entity in finder.Results)
            {
                Entity<InventoryBatch> inventoryEntity = entity as Entity<InventoryBatch>;

                if (inventoryEntity.NativeModel.BatchNumber == original.NativeModel.BatchNumber)
                {
                    inventoryEntity.NativeModel.BatchNumber = updated.NativeModel.BatchNumber;
                    inventoryEntity.NativeModel.ColorName = updated.NativeModel.ColorName;
                    ITransaction updater = new EditBatchInCurrentInventoryTransaction(inventoryEntity, sqliteStore);
                    updater.Execute();
                }
            }
        }

        bool BatchDoesNotExistInInventory(string batchNumber)
        {
            ITransaction finder = new ListCurrentInventoryTransaction(sqliteStore);
            finder.Execute();

            foreach (IEntity result in finder.Results)
            {
                Entity<InventoryBatch> entity = result as Entity<InventoryBatch>;
                
                if (entity.NativeModel.BatchNumber == batchNumber)
                {
                    return false;
                }
            }

            return true;
        }

        int GetAdjustedInventoryQuantity(string batchNumber, int currentQuantity, int newQuantity)
        {
            ITransaction finder = new FindBatchesInImplementationLedgerByBatchNumberTransaction(batchNumber, sqliteStore);
            finder.Execute();
            ITransaction received = new FindBatchesInReceivingLedgerByBatchNumberTransaction(batchNumber, sqliteStore);
            received.Execute();

            int implementedQuantity = finder.Results.Count;
            int receivedQuantity = 0;

            for (int i = 0; i < received.Results.Count; i++)
            {
                Entity<ReceivedBatch> entity = received.Results[i] as Entity<ReceivedBatch>;
                receivedQuantity += entity.NativeModel.Quantity;
            }

            int updatedQuantity = currentQuantity >= newQuantity
                ? receivedQuantity - (currentQuantity - newQuantity)
                : receivedQuantity + (newQuantity - currentQuantity);

            return updatedQuantity - implementedQuantity;
        }

        void UpdateImplementedBatches(Entity<ReceivedBatch> original, Entity<ReceivedBatch> updated)
        {
            ITransaction finder = new FindBatchesInImplementationLedgerByBatchNumberTransaction(original.NativeModel.ColorName, sqliteStore);
            finder.Execute();

            foreach (IEntity result in finder.Results)
            {
                Entity<LoggedBatch> entity = result as Entity<LoggedBatch>;
                entity.NativeModel.BatchNumber = updated.NativeModel.BatchNumber;
                entity.NativeModel.ColorName = updated.NativeModel.ColorName;
                ITransaction updater = new UpdateBatchInImplementationLedgerAtIdTransaction(entity, sqliteStore);
                updater.Execute();
            }
        }
    }
}
