﻿using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.Storage.MemoryStore;
using CSI.BatchTracker.Storage.MemoryStore.Transactions.InventoryManagement;
using NUnit.Framework;
using System;

namespace CSI.BatchTracker.Tests.Storage.MemoryStore.Transactions.InventoryManagement
{
    [TestFixture]
    class AddBatchToImplementedBatchLedgerTransactionTest
    {
        MemoryStoreContext store;
        LoggedBatch loggedBatch;
        InventoryBatch inventoryBatch;
        BatchOperator batchOperator;
        AddBatchToImplementedBatchLedgerTransaction adder;
        AddReceivedBatchToInventoryTransaction receiver;
        [Test]
        public void ImplementingBatchRemovesBatchFromLiveInventory()
        {
            int expectedQty = 4;
            int expectedCount = 1;
            store = new MemoryStoreContext();
            batchOperator = new BatchOperator("Jane", "Doe");
            inventoryBatch = new InventoryBatch("White", "8728811303201", DateTime.Now, 5);
            receiver = new AddReceivedBatchToInventoryTransaction(new Entity<InventoryBatch>(inventoryBatch), store);
            receiver.Execute();

            loggedBatch = new LoggedBatch(
                inventoryBatch.ColorName,
                inventoryBatch.BatchNumber,
                DateTime.Now,
                batchOperator
            );

            adder = new AddBatchToImplementedBatchLedgerTransaction(new Entity<LoggedBatch>(loggedBatch), store);
            adder.Execute();

            Assert.AreEqual(expectedQty, store.CurrentInventory[receiver.LastSystemId].NativeModel.Quantity);
            Assert.AreEqual(expectedCount, store.ImplementedBatchLedger.Count);
        }
    }
}