using CSI.BatchTracker.Domain.DataSource.Contracts;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.Tests.TestHelpers.NativeModels;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Tests.Domain.DataSource
{
    [TestFixture]
    abstract class ActiveInventorySourceBehaviorTest
    {
        protected IActiveInventorySource dataSource;
        BatchOperatorTestHelper operatorHelper;

        [SetUp]
        public virtual void SetUp()
        {
            operatorHelper = new BatchOperatorTestHelper();
        }

        ReceivedBatch SetupReceivedBatch()
        {
            BatchOperator batchOperator = operatorHelper.GetJaneDoeOperator();
            return new ReceivedBatch("White", "872881202303", DateTime.Now, 5, 55555, batchOperator);
        }

        [Test]
        public void AddingReceivedBatchToActiveInventory()
        {
            int expectedCount = 1;
            ReceivedBatch batch = SetupReceivedBatch();

            dataSource.AddReceivedBatchToInventory(batch);

            Assert.AreEqual(expectedCount, dataSource.CurrentInventory.Count);
        }

        [Test]
        public void AddingReceivedBatchesWithDifferentBatchNumbersCreatedDifferentEntries()
        {
            int expectedCount = 2;
            ReceivedBatch batch = SetupReceivedBatch();

            batch.BatchNumber = "872891203202";
            dataSource.AddReceivedBatchToInventory(batch);

            batch.BatchNumber = "872890802305";
            dataSource.AddReceivedBatchToInventory(batch);

            Assert.AreEqual(expectedCount, dataSource.CurrentInventory.Count);
        }

        [Test]
        public void AddingReceivedBatchesWithSameBatchNumbersAreMerged()
        {
            int expectedCount = 1;
            ReceivedBatch batch = SetupReceivedBatch();

            dataSource.AddReceivedBatchToInventory(batch);
            dataSource.AddReceivedBatchToInventory(batch);

            Assert.AreEqual(expectedCount, dataSource.CurrentInventory.Count);
        }

        [Test]
        public void MergedInventoryRecordsHaveCorrectQuantity()
        {
            int expectedQty = 5;
            ReceivedBatch batch = SetupReceivedBatch();

            batch.Quantity = 3;
            dataSource.AddReceivedBatchToInventory(batch);

            batch.Quantity = 2;
            dataSource.AddReceivedBatchToInventory(batch);

            InventoryBatch found = dataSource.FindInventoryBatchByBatchNumber(batch.BatchNumber);

            Assert.AreEqual(expectedQty, found.Quantity);
        }

        [Test]
        public void DeductImplementedBatchFromActiveInventory()
        {
            int expectedQty = 1;
            ReceivedBatch batch = SetupReceivedBatch();
            batch.Quantity = 2;

            dataSource.AddReceivedBatchToInventory(batch);
            dataSource.DeductBatchFromInventory(batch.BatchNumber);

            InventoryBatch found = dataSource.FindInventoryBatchByBatchNumber(batch.BatchNumber);

            Assert.AreEqual(expectedQty, found.Quantity);
        }

        [Test]
        public void DepletedBatchGetsRemovedFromActiveInventory()
        {
            int expectedCountBefore = 1;
            int expectedCountAfter = 0;
            ReceivedBatch batch = SetupReceivedBatch();
            batch.Quantity = 1;

            dataSource.AddReceivedBatchToInventory(batch);
            Assert.AreEqual(expectedCountBefore, dataSource.CurrentInventory.Count);

            dataSource.DeductBatchFromInventory(batch.BatchNumber);
            Assert.AreEqual(expectedCountAfter, dataSource.CurrentInventory.Count);
        }
    }
}
