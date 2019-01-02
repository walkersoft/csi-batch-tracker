using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.Storage.MemoryStore;
using CSI.BatchTracker.Storage.MemoryStore.Transactions.InventoryManagement;
using NUnit.Framework;
using System;

namespace CSI.BatchTracker.Tests.Storage.MemoryStore.Transactions.InventoryManagement
{
    [TestFixture]
    class AddReceivedBatchToReceivingLedgerTransactionTest
    {
        MemoryStoreContext store;
        Entity<ReceivedBatch> entity;
        AddReceivedBatchToReceivingLedgerTransaction adder;

        [Test]
        public void AddingReceivedBatchToLedgerIncreasesEntryCount()
        {
            int expectedCount = 1;
            int expectedSystemId = 1;
            store = new MemoryStoreContext();
            ReceivedBatch received = new ReceivedBatch("White", "872881201308", DateTime.Now, 5, 22392, new BatchOperator("Jane", "Doe"));
            entity = new Entity<ReceivedBatch>(received);

            adder = new AddReceivedBatchToReceivingLedgerTransaction(entity, store);
            adder.Execute();

            Assert.AreEqual(expectedCount, store.ReceivingLedger.Count);
            Assert.AreEqual(expectedSystemId, adder.LastSystemId);
        }
    }
}
