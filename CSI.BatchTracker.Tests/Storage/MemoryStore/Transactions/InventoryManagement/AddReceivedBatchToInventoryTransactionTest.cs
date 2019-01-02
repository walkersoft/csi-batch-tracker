﻿using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.Storage.MemoryStore;
using CSI.BatchTracker.Storage.MemoryStore.Transactions.InventoryManagement;
using NUnit.Framework;
using System;

namespace CSI.BatchTracker.Tests.Storage.MemoryStore.Transactions.InventoryManagement
{
    [TestFixture]
    class AddReceivedBatchToInventoryTransactionTest
    {
        MemoryStoreContext store;
        Entity<InventoryBatch> entity;
        InventoryBatch receiveableBatch;
        BatchOperator batchOperator;
        AddReceivedBatchToInventoryTransaction adder;

        [SetUp]
        public void SetUp()
        {
            store = new MemoryStoreContext();
            batchOperator = new BatchOperator("Jane", "Doe");
            receiveableBatch = new InventoryBatch("White", "872881502902", DateTime.Now, 5);
        }

        [Test]
        public void AddingNewBatchCreatesANewEntityInStore()
        {
            int expectedCount = 1;
            entity = new Entity<InventoryBatch>(receiveableBatch);
            adder = new AddReceivedBatchToInventoryTransaction(entity, store);

            adder.Execute();

            Assert.AreEqual(expectedCount, store.CurrentInventory.Count);
        }

        [Test]
        public void AddingExistingBatchMergesInventory()
        {
            int expectedCount = 1;
            int expectedQty = 10;
            entity = new Entity<InventoryBatch>(receiveableBatch);

            adder = new AddReceivedBatchToInventoryTransaction(entity, store);
            adder.Execute();

            int lastId = adder.LastSystemId;

            adder = new AddReceivedBatchToInventoryTransaction(entity, store);
            adder.Execute();

            Assert.AreEqual(expectedCount, store.CurrentInventory.Count);
            Assert.AreEqual(expectedQty, store.CurrentInventory[lastId].NativeModel.Quantity);
        }
    }
}
