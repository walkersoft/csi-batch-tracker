using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Storage.SQLiteStore.Transactions.InventoryManagement
{
    public sealed class UpdateBatchInImplementationLedgerAtIdTransaction : SQLiteDataSourceTransaction
    {
        Entity<LoggedBatch> entity;
        SQLiteStoreContext store;

        public UpdateBatchInImplementationLedgerAtIdTransaction(Entity<LoggedBatch> entity, SQLiteStoreContext store)
        {
            this.entity = entity;
            this.store = store;
        }

        public override void Execute()
        {
            string query = "UPDATE ImplementedBatches SET ColorName = ?, BatchNumber = ? WHERE SystemId = ?";
            List<object> parameters = new List<object>()
            {
                entity.NativeModel.ColorName,
                entity.NativeModel.BatchNumber,
                entity.SystemId
            };

            store.ExecuteNonQuery(query, parameters);
        }
    }
}
