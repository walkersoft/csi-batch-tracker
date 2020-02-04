using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Storage.SQLiteStore.Transactions.InventoryManagement
{
    public sealed class DeleteDepletedInventoryBatchAtId : SQLiteDataSourceTransaction
    {
        SQLiteStoreContext store;
        Entity<InventoryBatch> entity;

        public DeleteDepletedInventoryBatchAtId(Entity<InventoryBatch> entity, SQLiteStoreContext store)
        {
            this.store = store;
            this.entity = entity;
        }

        public override void Execute()
        {
            if (BatchExistsInInventoryAndIsDepleted())
            {
                string query = "DELETE FROM InventoryBatches WHERE SystemId = ?";
                List<object> parameters = new List<object> { entity.SystemId };
                store.ExecuteNonQuery(query, parameters);
            }
            
        }

        bool BatchExistsInInventoryAndIsDepleted()
        {
            string query = "SELECT * FROM InventoryBatches WHERE SystemId = ?";
            List<object> parameters = new List<object> { entity.SystemId };
            store.ExecuteReader(typeof(InventoryBatch), query, parameters);

            if (store.Results.Count > 0)
            {
                Entity<InventoryBatch> inventoryEntity = store.Results[0] as Entity<InventoryBatch>;
                return inventoryEntity.NativeModel.Quantity > 0;
            }

            return false;
        }
    }
}
