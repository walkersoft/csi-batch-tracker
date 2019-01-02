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
    class ListImplementedBatchTransactionTest
    {
        MemoryStoreContext store;

        [Test]
        public void ListImplementedBatchesInRepository()
        {
            int expectedQty = 5;
            store = new MemoryStoreContext();

            AddFiveBatchesToImplementationLedger();
            ITransaction finder = new ListImplementedBatchTransaction(store);
            finder.Execute();

            Assert.AreEqual(expectedQty, finder.Results.Count);
        }

        void AddFiveBatchesToImplementationLedger()
        {
            BatchOperator batchOperator = new BatchOperator("Jane", "Doe");
            InventoryBatch inventoryBatch = new InventoryBatch("White", "872881201202", DateTime.Now, 5);
            LoggedBatch loggedBatch = new LoggedBatch("White", "872881201202", DateTime.Now, batchOperator);
            Entity<LoggedBatch> loggedEntity = new Entity<LoggedBatch>(loggedBatch);
            Entity<InventoryBatch> inventoryEntity = new Entity<InventoryBatch>(inventoryBatch);

            ITransaction inventoryAdder = new AddReceivedBatchToInventoryTransaction(inventoryEntity, store);
            inventoryAdder.Execute();

            ITransaction loggedAdder = new AddBatchToImplementedBatchLedgerTransaction(loggedEntity, store);

            for (int i = 0; i < 5; i++)
            {
                loggedAdder.Execute();
            }
        }
    }
}
