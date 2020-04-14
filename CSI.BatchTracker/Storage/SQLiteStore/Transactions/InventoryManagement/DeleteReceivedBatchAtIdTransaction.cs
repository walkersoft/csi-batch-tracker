using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.Storage.Contracts;
using CSI.BatchTracker.Storage.SQLiteStore.Transactions.RecordAquisition;
using System.Collections.Generic;

namespace CSI.BatchTracker.Storage.SQLiteStore.Transactions.InventoryManagement
{
    public sealed class DeleteReceivedBatchAtIdTransaction : SQLiteDataSourceTransaction
    {
        SQLiteStoreContext store;
        int targetId;

        public DeleteReceivedBatchAtIdTransaction(int targetId, SQLiteStoreContext store)
        {
            this.store = store;
            this.targetId = targetId;
        }

        public override void Execute()
        {
            DeductBatchQuantityFromInventory();
            string query = "DELETE FROM ReceivedBatches WHERE SystemId = ?";
            List<object> parameters = new List<object> { targetId };
            store.ExecuteNonQuery(query, parameters);
        }

        void DeductBatchQuantityFromInventory()
        {
            string query = "SELECT * FROM ReceivedBatches WHERE SystemId = ?";
            List<object> parameters = new List<object> { targetId };
            store.ExecuteReader(typeof(ReceivedBatch), query, parameters);

            if (store.Results.Count > 0)
            {
                Entity<ReceivedBatch> targetInventoryEntity = store.Results[0] as Entity<ReceivedBatch>;
                ITransaction finder = new ListCurrentInventoryTransaction(store);
                finder.Execute();

                foreach (IEntity entity in finder.Results)
                {
                    Entity<InventoryBatch> inventoryEntity = entity as Entity<InventoryBatch>;

                    if (inventoryEntity.NativeModel.BatchNumber == targetInventoryEntity.NativeModel.BatchNumber)
                    {
                        inventoryEntity.NativeModel.DeductQuantity(targetInventoryEntity.NativeModel.Quantity);
                        ITransaction updater = new EditBatchInCurrentInventoryTransaction(inventoryEntity, store);
                        updater.Execute();
                        break;
                    }
                }
            }
        }
    }
}
