using CSI.BatchTracker.Domain.DataSource.Contracts;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.Tests.TestHelpers.NativeModels;
using NUnit.Framework;
using System;

namespace CSI.BatchTracker.Tests.Domain.DataSource.Behaviors
{
    [TestFixture]
    abstract class ImplementedBatchSourceBehaviorTest
    {
        protected IImplementedBatchSource implementedBatchSource;
        protected IActiveInventorySource inventorySource;
        ReceivedBatchTestHelper receivedBatchHelper;
        BatchOperatorTestHelper batchOperatorHelper;
        ReceivedBatch receivedBatch;
        BatchOperator batchOperator;
        DateTime date;

        [SetUp]
        public virtual void SetUp()
        {
            receivedBatchHelper = new ReceivedBatchTestHelper();
            batchOperatorHelper = new BatchOperatorTestHelper();
            SetupBatchForReceiving();
        }

        void SetupBatchForReceiving()
        {
            receivedBatch = receivedBatchHelper.GetUniqueBatch1();
            batchOperator = batchOperatorHelper.GetJaneDoeOperator();
            date = DateTime.Today;
        }

        [Test]
        public void AddingBatchToImplementationLedgerWorksIfBatchExistsInInventory()
        {
            int expectedCount = 1;

            inventorySource.AddReceivedBatchToInventory(receivedBatch);
            implementedBatchSource.AddBatchToImplementationLedger(receivedBatch.BatchNumber, date, batchOperator);

            Assert.AreEqual(expectedCount, implementedBatchSource.ImplementedBatchLedger.Count);
        }

        [Test]
        public void CorrectBatchIsDeductedWhenSendingToImplementationLedger()
        {
            int expectedQuantity = 2;
            string targetBatch = string.Empty;
            receivedBatch.Quantity = expectedQuantity + 1;

            inventorySource.AddReceivedBatchToInventory(receivedBatch);
            receivedBatch.BatchNumber = receivedBatchHelper.GetUniqueBatch2().BatchNumber;

            targetBatch = receivedBatch.BatchNumber;
            inventorySource.AddReceivedBatchToInventory(receivedBatch);
            implementedBatchSource.AddBatchToImplementationLedger(targetBatch, date, batchOperator);
            InventoryBatch found = inventorySource.FindInventoryBatchByBatchNumber(targetBatch);

            Assert.AreEqual(expectedQuantity, found.Quantity);
        }

        [Test]
        public void AddingBatchToImplementationDoesNotWorkIfTheBatchIsNotInInventory()
        {
            int expectedCount = 0;
            implementedBatchSource.AddBatchToImplementationLedger(receivedBatch.BatchNumber, date, batchOperator);

            Assert.AreEqual(expectedCount, implementedBatchSource.ImplementedBatchLedger.Count);
        }

        [Test]
        public void UndoingImplementedBatchWillReturnItToInventory()
        {
            int targetId = 1;
            InventoryBatch found;
            int expectedQuantityBefore = 1;
            int expectedQuantityAfter = 2;
            receivedBatch.Quantity = expectedQuantityAfter;

            inventorySource.AddReceivedBatchToInventory(receivedBatch);
            implementedBatchSource.AddBatchToImplementationLedger(receivedBatch.BatchNumber, date, batchOperator);

            found = inventorySource.FindInventoryBatchByBatchNumber(receivedBatch.BatchNumber);
            Assert.AreEqual(expectedQuantityBefore, inventorySource.CurrentInventory[0].Quantity);

            implementedBatchSource.UndoImplementedBatch(targetId);

            found = inventorySource.FindInventoryBatchByBatchNumber(receivedBatch.BatchNumber);
            Assert.AreEqual(expectedQuantityAfter, inventorySource.CurrentInventory[0].Quantity);
        }

        [Test]
        public void UndoingImplementedBatchThatHasNotBeenImplementedDoesNothing()
        {
            int targetId = 1;
            int expectedQuantity = 2;
            int expectedCount = 0;

            receivedBatch.Quantity = expectedQuantity;
            inventorySource.AddReceivedBatchToInventory(receivedBatch);
            implementedBatchSource.UndoImplementedBatch(targetId);

            Assert.AreEqual(expectedQuantity, inventorySource.CurrentInventory[0].Quantity);
            Assert.AreEqual(expectedCount, implementedBatchSource.ImplementedBatchLedger.Count);
        }
    }
}
