using CSI.BatchTracker.DataSource;
using CSI.BatchTracker.DataSource.MemoryDataSource;
using CSI.BatchTracker.DataSource.MemoryDataSource.Transactions.InventoryManagement;
using CSI.BatchTracker.DataSource.MemoryDataSource.Transactions.RecordAquisition;
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
    class ListCurrentInventoryTransactionTest
    {
        MemoryStore store;

        [SetUp]
        public void SetUp()
        {
            store = new MemoryStore();
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
