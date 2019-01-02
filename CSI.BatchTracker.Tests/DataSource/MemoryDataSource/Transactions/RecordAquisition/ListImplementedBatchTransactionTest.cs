using CSI.BatchTracker.Storage;
using CSI.BatchTracker.Storage.Contracts;
using CSI.BatchTracker.Storage.MemoryStore;
using CSI.BatchTracker.Storage.MemoryStore.Transactions.InventoryManagement;
using CSI.BatchTracker.Storage.MemoryStore.Transactions.RecordAquisition;
using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Tests.DataSource.MemoryDataSource.Transactions.RecordAquisition
{
    [TestFixture]
    class ListImplementedBatchTransactionTest
    {
        MemoryStore store;

        [Test]
        public void ListImplementedBatchesInRepository()
        {
            int expectedQty = 5;
            store = new MemoryStore();

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
