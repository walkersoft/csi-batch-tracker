using CSI.BatchTracker.DataSource.MemoryDataSource;
using CSI.BatchTracker.DataSource.MemoryDataSource.Transactions.InventoryManagement;
using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Tests.Domain.DataSource.MemoryDataSource.Transactions.InventoryManagement
{
    [TestFixture]
    class AddReceivedBatchToInventoryTransactionTest
    {
        MemoryStore store;
        Entity<InventoryBatch> entity;
        InventoryBatch receiveableBatch;
        BatchOperator batchOperator;
        AddReceivedBatchToInventoryTransaction adder;

        [SetUp]
        public void SetUp()
        {
            store = new MemoryStore();
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
    }
}
