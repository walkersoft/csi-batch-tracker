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
            if (memoryStore.ReceivingLedger.ContainsKey(id))
            {
                Entity<ReceivedBatch> originalEntity = GetOriginalReceivedBatchEntity(id);
                Entity<ReceivedBatch> updatedEntity = CreateUpdatedEntityFromOriginal(originalEntity, batch);
                ITransaction updater = new EditBatchInReceivingLedgerTransaction(updatedEntity, memoryStore);
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

        Entity<ReceivedBatch> GetOriginalReceivedBatchEntity(int systemId)
        {
            ITransaction finder = new FindBatchInReceivingLedgerByIdTransaction(systemId, memoryStore);
            finder.Execute();

            return finder.Results[0] as Entity<ReceivedBatch>;
        }

        void UpdateImplementedBatches(Entity<ReceivedBatch> original, Entity<ReceivedBatch> updated)
        {
            foreach (KeyValuePair<int, Entity<LoggedBatch>> implementedEntity in memoryStore.ImplementedBatchLedger)
            {
                if (implementedEntity.Value.NativeModel.BatchNumber == original.NativeModel.BatchNumber)
                {
                    implementedEntity.Value.NativeModel.BatchNumber = updated.NativeModel.BatchNumber;
                    implementedEntity.Value.NativeModel.ColorName = updated.NativeModel.ColorName;
                }
            }
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

                ITransaction adder = new AddReceivedBatchToInventoryTransaction(new Entity<InventoryBatch>(inventoryBatch), memoryStore);
                adder.Execute();
            }

            foreach (KeyValuePair<int, Entity<InventoryBatch>> inventoryBatch in memoryStore.CurrentInventory)
            {
                if (inventoryBatch.Value.NativeModel.BatchNumber == original.NativeModel.BatchNumber)
                {
                    InventoryBatch current = inventoryBatch.Value.NativeModel;
                    current.Quantity = GetAdjustedInventoryQuantity(current.BatchNumber, current.Quantity, updated.NativeModel.Quantity);

                    if (current.Quantity == 0)
                    {
                        deleter = new DeleteDepletedInventoryBatchAtId(inventoryBatch.Value, memoryStore);
                    }
                }
            }

            if (deleter != null)
            {
                deleter.Execute();
            }

            MergeCommonBatches();
        }

        void MergeCommonBatches()
        {
            List<InventoryBatch> merged = new List<InventoryBatch>();

            foreach (KeyValuePair<int, Entity<InventoryBatch>> currentEntity in memoryStore.CurrentInventory)
            {
                InventoryBatch currentBatch = currentEntity.Value.NativeModel;
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

            int systemId = 0;
            memoryStore.CurrentInventory.Clear();

            for (int i = 0; i < merged.Count; i++)
            {
                systemId++;
                memoryStore.CurrentInventory.Add(systemId, new Entity<InventoryBatch>(systemId, merged[i]));
            }
        }

        void ProcessColorAndBatchNumberUpdates(Entity<ReceivedBatch> original, Entity<ReceivedBatch> updated)
        {
            foreach (KeyValuePair<int, Entity<InventoryBatch>> inventoryBatch in memoryStore.CurrentInventory)
            {
                if (inventoryBatch.Value.NativeModel.BatchNumber == original.NativeModel.BatchNumber)
                {
                    InventoryBatch current = inventoryBatch.Value.NativeModel;
                    current.BatchNumber = updated.NativeModel.BatchNumber;
                    current.ColorName = updated.NativeModel.ColorName;
                }
            }
        }

        bool BatchDoesNotExistInInventory(string batchNumber)
        {
            foreach (KeyValuePair<int, Entity<InventoryBatch>> inventoryBatch in memoryStore.CurrentInventory)
            {
                if (inventoryBatch.Value.NativeModel.BatchNumber == batchNumber)
                {
                    return false;
                }
            }

            return true;
        }

        int GetAdjustedInventoryQuantity(string batchNumber, int currentQuantity, int newQuantity)
        {
            ITransaction finder = new FindBatchesInImplementationLedgerByBatchNumberTransaction(batchNumber, memoryStore);
            finder.Execute();
            int implementedQuantity = finder.Results.Count;

            return currentQuantity + (newQuantity - currentQuantity) - implementedQuantity;
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

        ObservableCollection<ReceivedBatch> ExecuteFinderAndBuildObservableCollectionFromTransactionResults(ITransaction finder)
        {
            ObservableCollection<ReceivedBatch> batches = new ObservableCollection<ReceivedBatch>();
            finder.Execute();

            foreach (IEntity entity in finder.Results)
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

        public EditablePurchaseOrder GetPurchaseOrderForEditing(int poNumber)
        {
            ITransaction finder = new FindBatchesInReceivingLedgerByPONumberTransaction(poNumber, memoryStore);
            ObservableCollection<ReceivedBatch> batches = RebuildObservableCollectionWithNewReceivedBatchInstances(
                ExecuteFinderAndBuildObservableCollectionFromTransactionResults(finder)
            );
            
            Dictionary<int, int> systemIdsForMappedBatches = new Dictionary<int, int>();

            for (int i = 0; i < finder.Results.Count; i++)
            {
                systemIdsForMappedBatches.Add(i, finder.Results[i].SystemId);
            }

            return new EditablePurchaseOrder(batches, systemIdsForMappedBatches); 
        }

        ObservableCollection<ReceivedBatch> RebuildObservableCollectionWithNewReceivedBatchInstances(ObservableCollection<ReceivedBatch> receivedBatches)
        {
            ObservableCollection<ReceivedBatch> rebuilt = new ObservableCollection<ReceivedBatch>();
            
            foreach (ReceivedBatch batch in receivedBatches)
            {
                rebuilt.Add(RecreateReceivedBatch(batch));
            }

            return rebuilt;
        }
    }
}
