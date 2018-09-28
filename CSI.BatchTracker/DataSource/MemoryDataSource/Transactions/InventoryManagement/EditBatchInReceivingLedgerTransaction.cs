﻿using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.DataSource.MemoryDataSource.Transactions.InventoryManagement
{
    public sealed class EditBatchInReceivingLedgerTransaction : MemoryDataSourceTransaction
    {
        MemoryStore store;
        Entity<ReceivedBatch> entity;

        public EditBatchInReceivingLedgerTransaction(Entity<ReceivedBatch> entity, MemoryStore store)
        {
            this.entity = entity;
            this.store = store;
        }

        public override void Execute()
        {
            if (store.ReceivingLedger.ContainsKey(entity.SystemId))
            {
                store.ReceivingLedger[entity.SystemId] = entity;
            }
        }
    }
}
