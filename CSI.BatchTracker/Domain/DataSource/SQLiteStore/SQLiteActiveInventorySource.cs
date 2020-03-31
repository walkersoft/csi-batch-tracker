using CSI.BatchTracker.Domain.DataSource.Contracts;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.Storage.Contracts;
using CSI.BatchTracker.Storage.SQLiteStore;
using CSI.BatchTracker.Storage.SQLiteStore.Transactions.InventoryManagement;
using CSI.BatchTracker.Storage.SQLiteStore.Transactions.RecordAquisition;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CSI.BatchTracker.Domain.DataSource.SQLiteStore
{
    public sealed class SQLiteActiveInventorySource : IActiveInventorySource
    {
        SQLiteStoreContext sqliteStore;
        public ObservableCollection<InventoryBatch> CurrentInventory { get; private set; }
        public Dictionary<string, int> CurrentInventoryBatchNumberToIdMappings { get; private set; }

        public int TotalInventoryCount
        {
            get
            {
                int count = 0;
                ITransaction finder = new ListCurrentInventoryTransaction(sqliteStore);
                finder.Execute();

                foreach (IEntity entity in finder.Results)
                {
                    Entity<InventoryBatch> inventoryEntity = entity as Entity<InventoryBatch>;
                    count += inventoryEntity.NativeModel.Quantity;
                }

                return count;
            }
        }

        public SQLiteActiveInventorySource(SQLiteStoreContext sqliteStore)
        {
            this.sqliteStore = sqliteStore;
            CurrentInventory = new ObservableCollection<InventoryBatch>();
            CurrentInventoryBatchNumberToIdMappings = new Dictionary<string, int>();
        }

        public void AddReceivedBatchToInventory(ReceivedBatch batch)
        {
            InventoryBatch inventoryBatch = new InventoryBatch(
                batch.ColorName,
                batch.BatchNumber,
                batch.ActivityDate,
                batch.Quantity
            );

            ITransaction adder = new AddReceivedBatchToInventoryTransaction(new Entity<InventoryBatch>(inventoryBatch), sqliteStore);
            adder.Execute();
            UpdateActiveInventory();
        }

        public void DeductBatchFromInventory(string batchNumber)
        {
            ITransaction finder = new ListCurrentInventoryTransaction(sqliteStore);
            finder.Execute();

            foreach(IEntity result in finder.Results)
            {
                Entity<InventoryBatch> entity = result as Entity<InventoryBatch>;

                if (entity.NativeModel.BatchNumber == batchNumber)
                {
                    entity.NativeModel.DeductQuantity(1);
                    ITransaction updater = new EditBatchInCurrentInventoryTransaction(entity, sqliteStore);
                    updater.Execute();
                    break;
                }
            }

            UpdateActiveInventory();
        }

        public void UpdateActiveInventory()
        {
            ITransaction finder = new ListCurrentInventoryTransaction(sqliteStore);
            finder.Execute();
            CurrentInventory.Clear();
            CurrentInventoryBatchNumberToIdMappings.Clear();
            BubbleSortEntitiesByBatchDisplayName(finder);

            for (int i = 0; i < finder.Results.Count; i++)
            {
                Entity<InventoryBatch> entity = finder.Results[i] as Entity<InventoryBatch>;
                CurrentInventory.Add(entity.NativeModel);
                CurrentInventoryBatchNumberToIdMappings.Add(entity.NativeModel.BatchNumber, entity.SystemId);
            }
        }

        void BubbleSortEntitiesByBatchDisplayName(ITransaction transaction)
        {
            for (int i = 0; i < transaction.Results.Count; i++)
            {
                for (int j = 0; j < transaction.Results.Count - 1; j++)
                {
                    Entity<InventoryBatch> current = transaction.Results[j] as Entity<InventoryBatch>;
                    Entity<InventoryBatch> next = transaction.Results[j + 1] as Entity<InventoryBatch>;

                    if (string.Compare(current.NativeModel.DisplayName, next.NativeModel.DisplayName) > 0)
                    {
                        IEntity swap = transaction.Results[j + 1];
                        transaction.Results[j + 1] = transaction.Results[j];
                        transaction.Results[j] = swap;
                    }
                }
            }
        }

        public InventoryBatch FindInventoryBatchByBatchNumber(string batchNumber)
        {
            InventoryBatch batch = new InventoryBatch();
            ITransaction finder = new ListCurrentInventoryTransaction(sqliteStore);
            finder.Execute();

            foreach (IEntity result in finder.Results)
            {
                Entity<InventoryBatch> entity = result as Entity<InventoryBatch>;

                if (entity.NativeModel.BatchNumber == batchNumber)
                {
                    batch = entity.NativeModel;
                    break;
                }
            }

            return batch;
        }
    }
}
