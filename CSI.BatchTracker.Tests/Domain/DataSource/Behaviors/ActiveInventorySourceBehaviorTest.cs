using CSI.BatchTracker.Domain.DataSource.Contracts;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.Tests.TestHelpers.NativeModels;
using NUnit.Framework;
using System;

namespace CSI.BatchTracker.Tests.Domain.DataSource.Behaviors
{
    [TestFixture]
    abstract class ActiveInventorySourceBehaviorTest
    {
        protected IActiveInventorySource inventorySource;
        protected IBatchOperatorSource operatorSource;
        BatchOperatorTestHelper operatorHelper;

        [SetUp]
        public virtual void SetUp()
        {
            operatorHelper = new BatchOperatorTestHelper(operatorSource);
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

            inventorySource.AddReceivedBatchToInventory(batch);

            Assert.AreEqual(expectedCount, inventorySource.CurrentInventory.Count);
        }

        [Test]
        public void AddingReceivedBatchesWithDifferentBatchNumbersCreatedDifferentEntries()
        {
            int expectedCount = 2;
            ReceivedBatch batch = SetupReceivedBatch();

            batch.BatchNumber = "872891203202";
            inventorySource.AddReceivedBatchToInventory(batch);

            batch.BatchNumber = "872890802305";
            inventorySource.AddReceivedBatchToInventory(batch);

            Assert.AreEqual(expectedCount, inventorySource.CurrentInventory.Count);
        }

        [Test]
        public void AddingReceivedBatchesWithSameBatchNumbersAreMerged()
        {
            int expectedCount = 1;
            ReceivedBatch batch = SetupReceivedBatch();

            inventorySource.AddReceivedBatchToInventory(batch);
            inventorySource.AddReceivedBatchToInventory(batch);

            Assert.AreEqual(expectedCount, inventorySource.CurrentInventory.Count);
        }

        [Test]
        public void MergedInventoryRecordsHaveCorrectQuantity()
        {
            int expectedQty = 5;
            ReceivedBatch batch = SetupReceivedBatch();

            batch.Quantity = 3;
            inventorySource.AddReceivedBatchToInventory(batch);

            batch.Quantity = 2;
            inventorySource.AddReceivedBatchToInventory(batch);

            InventoryBatch found = inventorySource.FindInventoryBatchByBatchNumber(batch.BatchNumber);

            Assert.AreEqual(expectedQty, found.Quantity);
        }

        [Test]
        public void DeductImplementedBatchFromActiveInventory()
        {
            int expectedQty = 1;
            ReceivedBatch batch = SetupReceivedBatch();
            batch.Quantity = 2;

            inventorySource.AddReceivedBatchToInventory(batch);
            inventorySource.DeductBatchFromInventory(batch.BatchNumber);

            InventoryBatch found = inventorySource.FindInventoryBatchByBatchNumber(batch.BatchNumber);

            Assert.AreEqual(expectedQty, found.Quantity);
        }

        [Test]
        public void DepletedBatchGetsRemovedFromActiveInventory()
        {
            int expectedCountBefore = 1;
            int expectedCountAfter = 0;
            ReceivedBatch batch = SetupReceivedBatch();
            batch.Quantity = 1;

            inventorySource.AddReceivedBatchToInventory(batch);
            Assert.AreEqual(expectedCountBefore, inventorySource.CurrentInventory.Count);

            inventorySource.DeductBatchFromInventory(batch.BatchNumber);
            Assert.AreEqual(expectedCountAfter, inventorySource.CurrentInventory.Count);
        }

        [Test]
        public void FindSpecificBatchFromMultipleBatchesInActiveInventory()
        {
            ReceivedBatch paddingBatch = SetupReceivedBatch();
            ReceivedBatch targetBatch = SetupReceivedBatch();
            targetBatch.BatchNumber = "872894501202";

            inventorySource.AddReceivedBatchToInventory(paddingBatch);
            inventorySource.AddReceivedBatchToInventory(targetBatch);
            InventoryBatch found = inventorySource.FindInventoryBatchByBatchNumber(targetBatch.BatchNumber);

            Assert.AreEqual(targetBatch.BatchNumber, found.BatchNumber);
        }

        [Test]
        public void GetInventoryTotalWithBatchesAcrossMultiplePurchaseOrders()
        {
            int expectedQuantity = 5;
            ReceivedBatch batch = SetupReceivedBatch();
            batch.Quantity = 1;

            batch.PONumber = 11111;
            batch.ColorName = "White";
            batch.BatchNumber = "872891111111";
            inventorySource.AddReceivedBatchToInventory(batch);

            batch.PONumber = 22222;
            batch.ColorName = "White";
            batch.BatchNumber = "872891111111";
            inventorySource.AddReceivedBatchToInventory(batch);

            batch.ColorName = "Yellow";
            batch.BatchNumber = "872892222222";
            inventorySource.AddReceivedBatchToInventory(batch);

            batch.ColorName = "Black";
            batch.BatchNumber = "872893333333";
            inventorySource.AddReceivedBatchToInventory(batch);

            batch.PONumber = 33333;
            batch.ColorName = "Black";
            batch.BatchNumber = "87289333333";
            inventorySource.AddReceivedBatchToInventory(batch);

            Assert.AreEqual(expectedQuantity, inventorySource.TotalInventoryCount);
        }
    }
}
