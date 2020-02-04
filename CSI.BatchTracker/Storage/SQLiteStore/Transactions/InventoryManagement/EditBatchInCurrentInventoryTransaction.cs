using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.Storage.Contracts;
using System.Collections.Generic;

namespace CSI.BatchTracker.Storage.SQLiteStore.Transactions.InventoryManagement
{
    public sealed class EditBatchInCurrentInventoryTransaction : SQLiteDataSourceTransaction
    {
        SQLiteStoreContext store;
        Entity<InventoryBatch> entity;

        public EditBatchInCurrentInventoryTransaction(Entity<InventoryBatch> entity, SQLiteStoreContext store)
        {
            this.store = store;
            this.entity = entity;
        }

        public override void Execute()
        {
            if (BatchWasDepletedAndDeleted())
            {
                return;
            }

            string query = "UPDATE InventoryBatches SET ColorName = ?, BatchNumber = ?, ActivityDate = ?, QtyOnHand = ? WHERE SystemId = ?";

            List<object> parameters = new List<object>()
            {
                entity.NativeModel.ColorName,
                entity.NativeModel.BatchNumber,
                entity.NativeModel.ActivityDate.FormatForDatabase(),
                entity.NativeModel.Quantity,
                entity.SystemId
            };

            store.ExecuteNonQuery(query, parameters);
        }

        bool BatchWasDepletedAndDeleted()
        {
            if (entity.NativeModel.Quantity == 0)
            {
                ITransaction deleter = new DeleteDepletedInventoryBatchAtId(entity, store);
                deleter.Execute();
                return true;
            }

            return false;
        }
    }
}
