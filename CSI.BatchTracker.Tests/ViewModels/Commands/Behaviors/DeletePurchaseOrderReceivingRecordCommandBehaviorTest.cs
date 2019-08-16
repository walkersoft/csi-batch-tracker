using CSI.BatchTracker.Domain.NativeModels;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Tests.ViewModels.Commands.Behaviors
{
    [TestFixture]
    abstract class DeletePurchaseOrderReceivingRecordCommandBehaviorTest : ReceivedPurchaseOrderEditorViewModelTestingBase
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
        }

        [Test]
        public void CommandWillNotExecuteIfNoBatchIsSelectedInReceivedBatchesLedger()
        {
            viewModel.ReceivedBatchesSelectedIndex = -1;
            Assert.False(command.CanExecute(null));
        }

        [Test]
        public void CommandWillNotExecuteIfSelectedBatchForDeletionHasBeenImplemented()
        {
            string batchNumber = viewModel.ReceivedBatches[0].BatchNumber;
            implementedBatchSource.AddBatchToImplementationLedger(batchNumber, DateTime.Now, operatorSource.FindBatchOperator(1));
            viewModel.ReceivedBatchesSelectedIndex = 0;

            Assert.False(command.CanExecute(null));
        }

        [Test]
        public void CommandWillExecuteIfSelectedBatchHasNotBeenImplemented()
        {
            viewModel.ReceivedBatchesSelectedIndex = 0;
            Assert.True(command.CanExecute(null));
        }

        [Test]
        public void ExecutedCommandWillRemoveTheBatchFromTheReceivingLedger()
        {
            int expectedCount = 2;

            viewModel.ReceivedBatchesSelectedIndex = 0;
            command.Execute(null);

            Assert.AreEqual(expectedCount, viewModel.ReceivedBatches.Count);
        }

        [Test]
        public void ExecutedCommandWillRemoveTheBatchFromActiveInventory()
        {
            int expectedBeforeCount = 3;
            int expectedAfterCount = 2;

            Assert.AreEqual(expectedBeforeCount, inventorySource.CurrentInventory.Count);

            viewModel.ReceivedBatchesSelectedIndex = 1;
            command.Execute(null);

            Assert.AreEqual(expectedAfterCount, inventorySource.CurrentInventory.Count);
        }

        [Test]
        public void ExecutedCommandWillRemoveOnlyReceivingQuantityFromActiveInventory()
        {
            int expectedCount = 3;
            int expectedQty = 3;
            BatchOperator batchOperator = operatorSource.FindBatchOperator(originalBatchOperatorId);
            ReceivedBatch newBatchWithSameBatchNumber = new ReceivedBatch("White", whiteBatch, DateTime.Now, expectedQty, 22222, batchOperator);

            receivedBatchSource.SaveReceivedBatch(newBatchWithSameBatchNumber);
            viewModel.ReceivedBatchesSelectedIndex = 0;
            command.Execute(null);

            Assert.AreEqual(expectedCount, inventorySource.CurrentInventory.Count);
            Assert.AreEqual(expectedQty, inventorySource.FindInventoryBatchByBatchNumber(whiteBatch).Quantity);
        }

        [Test]
        public void ExecutedCommandThatDeletesAllRecordsEffectivelyDeletesAllReceivingRecordsForTheGivenPurchaseOrder()
        {
            int expectedCount = 0;
            int loop = 0;

            viewModel.ReceivedBatchesSelectedIndex = 0;

            while (viewModel.ReceivedBatches.Count > 0)
            {
                loop++;
                command.Execute(null);
            }

            Assert.AreEqual(expectedCount, viewModel.ReceivedBatches.Count);
            Assert.AreEqual(expectedCount, receivedBatchSource.GetReceivedBatchesByPONumber(int.Parse(viewModel.PONumber)).Count);
        }
    }
}
