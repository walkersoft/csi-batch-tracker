using CSI.BatchTracker.Domain.DataSource.Contracts;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.Tests.TestHelpers.NativeModels;
using NUnit.Framework;
using System;
using System.Collections.ObjectModel;

namespace CSI.BatchTracker.Tests.Domain.DataSource.Behaviors
{
    [TestFixture]
    abstract class ImplementedBatchSourceBehaviorTest
    {
        protected IBatchOperatorSource operatorSource;
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
            receivedBatchHelper = new ReceivedBatchTestHelper(operatorSource);
            batchOperatorHelper = new BatchOperatorTestHelper(operatorSource);
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

        [Test]
        public void FindingMultipleRecordsOfSpecificBatchInHaystackOfBatches()
        {
            int expectedCount = 2;
            string targetBatch = receivedBatch.BatchNumber;
            ReceivedBatch paddingBatch = receivedBatchHelper.GetUniqueBatch2();

            for (int i = 0; i < 5; i++)
            {
                DateTime newDate = DateTime.Today.AddDays(i);
                receivedBatch.ActivityDate = newDate;
                paddingBatch.ActivityDate = newDate;

                if (i % 2 == 0)
                {
                    inventorySource.AddReceivedBatchToInventory(paddingBatch);
                    implementedBatchSource.AddBatchToImplementationLedger(paddingBatch.BatchNumber, newDate, batchOperator);
                }
                else
                {
                    inventorySource.AddReceivedBatchToInventory(receivedBatch);
                    implementedBatchSource.AddBatchToImplementationLedger(receivedBatch.BatchNumber, newDate, batchOperator);
                }
            }

            ObservableCollection<LoggedBatch> batches = implementedBatchSource.GetImplementedBatchesByBatchNumber(targetBatch);

            Assert.AreEqual(expectedCount, batches.Count);
        }

        [Test]
        public void TriggerUpdatingOfActiveInventoryWhenImplementationLedgerManipulatesInventoryStore()
        {
            int expectedCountBefore = 0;
            int expectedCountAfter = 1;
            receivedBatch.Quantity = 1;

            inventorySource.AddReceivedBatchToInventory(receivedBatch);
            implementedBatchSource.AddBatchToImplementationLedger(receivedBatch.BatchNumber, DateTime.Now, receivedBatch.ReceivingOperator);

            Assert.AreEqual(expectedCountBefore, inventorySource.CurrentInventory.Count);

            implementedBatchSource.UndoImplementedBatch(implementedBatchSource.ImplementedBatchIdMappings[0]);

            Assert.AreEqual(expectedCountAfter, inventorySource.CurrentInventory.Count);
        }

        [Test]
        public void ReceivedBatchTimeStampSecondsAreAtZero()
        {
            int expectedSeconds = 0;

            inventorySource.AddReceivedBatchToInventory(receivedBatch);
            implementedBatchSource.AddBatchToImplementationLedger(
                receivedBatch.BatchNumber,
                new DateTime(2019, 1, 1, 1, 1, 1),
                receivedBatch.ReceivingOperator
            );

            Assert.AreEqual(expectedSeconds, implementedBatchSource.ImplementedBatchLedger[0].ActivityDate.Second);
        }

        [Test]
        public void RemovingAndReimplementingBatchFromImplementationLedgerWillNotTryToAddAtUsedKeyInLedger()
        {
            receivedBatch.Quantity = 5;
            inventorySource.AddReceivedBatchToInventory(receivedBatch);
            implementedBatchSource.AddBatchToImplementationLedger(receivedBatch.BatchNumber, DateTime.Now, receivedBatch.ReceivingOperator);
            implementedBatchSource.AddBatchToImplementationLedger(receivedBatch.BatchNumber, DateTime.Now, receivedBatch.ReceivingOperator);
            implementedBatchSource.AddBatchToImplementationLedger(receivedBatch.BatchNumber, DateTime.Now, receivedBatch.ReceivingOperator);

            implementedBatchSource.UndoImplementedBatch(1);

            Assert.DoesNotThrow(() => implementedBatchSource.AddBatchToImplementationLedger(receivedBatch.BatchNumber, DateTime.Now, receivedBatch.ReceivingOperator));
        }
    }
}
