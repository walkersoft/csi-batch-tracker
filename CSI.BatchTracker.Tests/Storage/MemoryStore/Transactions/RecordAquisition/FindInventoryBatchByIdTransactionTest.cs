﻿using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.Storage.Contracts;
using CSI.BatchTracker.Storage.MemoryStore;
using CSI.BatchTracker.Storage.MemoryStore.Transactions.InventoryManagement;
using CSI.BatchTracker.Storage.MemoryStore.Transactions.RecordAquisition;
using NUnit.Framework;
using System;

namespace CSI.BatchTracker.Tests.Storage.MemoryStore.Transactions.RecordAquisition
{
    [TestFixture]
    class FindInventoryBatchByIdTransactionTest
    {
        MemoryStoreContext store;

        [Test]
        public void FindEntityInInventory()
        {
            int targetId = 3;
            store = new MemoryStoreContext();
            ITransaction finder = new FindInventoryBatchByIdTransaction(targetId, store);

            PutThreeBatchesInInventory();
            finder.Execute();

            Assert.AreEqual(targetId, finder.Results[0].SystemId);
        }

        void PutThreeBatchesInInventory()
        {
            InventoryBatch batch1 = new InventoryBatch("White", "872881801202", DateTime.Now, 3);
            InventoryBatch batch2 = new InventoryBatch("White", "872881801502", DateTime.Now, 3);
            InventoryBatch batch3 = new InventoryBatch("White", "872881801208", DateTime.Now, 3);

            Entity<InventoryBatch> entity;
            ITransaction adder;

            entity = new Entity<InventoryBatch>(batch1);
            adder = new AddReceivedBatchToInventoryTransaction(entity, store);
            adder.Execute();

            entity = new Entity<InventoryBatch>(batch2);
            adder = new AddReceivedBatchToInventoryTransaction(entity, store);
            adder.Execute();

            entity = new Entity<InventoryBatch>(batch3);
            adder = new AddReceivedBatchToInventoryTransaction(entity, store);
            adder.Execute();
        }
    }
}