﻿using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.Storage.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            string query = "UPDATE InventoryBatches SET ColorName = ?, BatchNumber = ?, ActivityDate = ?, QtyOnHand = ? WHERE SystemId = ?";
            List<object> parameters = new List<object>
            {
                entity.NativeModel.ColorName,
                entity.NativeModel.BatchNumber,
                entity.NativeModel.ActivityDate.ToString(),
                entity.NativeModel.Quantity,
                entity.SystemId
            };

            store.ExecuteNonQuery(query, parameters);
            DeleteIfDepleted();
        }

        void DeleteIfDepleted()
        {
            ITransaction deleter = new DeleteDepletedInventoryBatchAtId(entity, store);
            deleter.Execute();
        }
    }
}
