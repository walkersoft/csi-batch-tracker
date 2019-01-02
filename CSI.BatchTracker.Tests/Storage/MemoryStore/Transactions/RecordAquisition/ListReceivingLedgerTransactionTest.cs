using CSI.BatchTracker.Domain.DataSource;
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
    class ListReceivingLedgerTransactionTest
    {
        MemoryStoreContext store;

        [Test]
        public void ListAllReceivedBatchesInReceivingLedger()
        {
            int expectedCount = 5;
            store = new MemoryStoreContext();
            ITransaction finder = new ListReceivingLedgerTransaction(store);

            PopulateDataSourceWithFiveLedgerTransactions();
            finder.Execute();

            Assert.AreEqual(expectedCount, finder.Results.Count);
        }

        void PopulateDataSourceWithFiveLedgerTransactions()
        {
            BatchOperator batchOperator = new BatchOperator("Jane", "Doe");
            ReceivedBatch batch1 = new ReceivedBatch("Yellow", "872880102101", DateTime.Now, 3, 42219, batchOperator);
            ReceivedBatch batch2 = new ReceivedBatch("White", "872880102201", DateTime.Now, 3, 42219, batchOperator);
            ReceivedBatch batch3 = new ReceivedBatch("Red", "872880102301", DateTime.Now, 3, 42219, batchOperator);
            ReceivedBatch batch4 = new ReceivedBatch("Blue Red", "872880102401", DateTime.Now, 3, 42219, batchOperator);
            ReceivedBatch batch5 = new ReceivedBatch("Bright Yellow", "872880102501", DateTime.Now, 3, 42219, batchOperator);

            Entity<ReceivedBatch> entity;
            ITransaction adder;

            entity = new Entity<ReceivedBatch>(batch1);
            adder = new AddReceivedBatchToReceivingLedgerTransaction(entity, store);
            adder.Execute();

            entity = new Entity<ReceivedBatch>(batch2);
            adder = new AddReceivedBatchToReceivingLedgerTransaction(entity, store);
            adder.Execute();

            entity = new Entity<ReceivedBatch>(batch3);
            adder = new AddReceivedBatchToReceivingLedgerTransaction(entity, store);
            adder.Execute();

            entity = new Entity<ReceivedBatch>(batch4);
            adder = new AddReceivedBatchToReceivingLedgerTransaction(entity, store);
            adder.Execute();

            entity = new Entity<ReceivedBatch>(batch4);
            adder = new AddReceivedBatchToReceivingLedgerTransaction(entity, store);
            adder.Execute();
        }
    }
}
