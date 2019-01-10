﻿using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.Storage.Contracts;
using CSI.BatchTracker.Storage.MemoryStore;
using CSI.BatchTracker.Storage.MemoryStore.Transactions.InventoryManagement;
using CSI.BatchTracker.Storage.MemoryStore.Transactions.RecordAquisition;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace CSI.BatchTracker.Tests.Storage.MemoryStore.Transactions.RecordAquisition
{
    [TestFixture]
    class ListCurrentInventoryTransactionTest
    {
        MemoryStoreContext store;

        [SetUp]
        public void SetUp()
        {
            store = new MemoryStoreContext();
        }

        [Test]
        public void GetBatchesInLiveInventory()
        {
            int expectedCount = 3;
            ITransaction finder = new ListCurrentInventoryTransaction(store);

            PopulateDataSourceWithThreeInventoryItems();
            finder.Execute();

            Assert.AreEqual(expectedCount, finder.Results.Count);
        }

        void PopulateDataSourceWithThreeInventoryItems()
        {
            Entity<InventoryBatch> entity;

            //Four shown, but the first two will merge as one item
            InventoryBatch batch1 = new InventoryBatch("White", "872882501302", DateTime.Now, 1);
            InventoryBatch batch2 = new InventoryBatch("White", "872882501302", DateTime.Now, 1);
            InventoryBatch batch3 = new InventoryBatch("Black", "872881208202", DateTime.Now, 3);
            InventoryBatch batch4 = new InventoryBatch("White", "872883204201", DateTime.Now, 4);

            List<InventoryBatch> batches = new List<InventoryBatch> { batch1, batch2, batch3, batch4 };

            entity = new Entity<InventoryBatch>(batch1);
            ITransaction adder = new AddReceivedBatchToInventoryTransaction(entity, store);
            adder.Execute();

            entity = new Entity<InventoryBatch>(batch2);
            adder = new AddReceivedBatchToInventoryTransaction(entity, store);
            adder.Execute();

            entity = new Entity<InventoryBatch>(batch3);
            adder = new AddReceivedBatchToInventoryTransaction(entity, store);
            adder.Execute();

            entity = new Entity<InventoryBatch>(batch4);
            adder = new AddReceivedBatchToInventoryTransaction(entity, store);
            adder.Execute();
        }
    }
}