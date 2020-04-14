using CSI.BatchTracker.Domain;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.ViewModels;
using CSI.BatchTracker.ViewModels.Commands;
using NUnit.Framework;
using System;

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
        public void CommandWillExecuteIfSelectedBatchForDeletionHasBeenImplementedButHasAvailableQuantityInInventory()
        {
            string batchNumber = viewModel.ReceivedBatches[0].BatchNumber;
            implementedBatchSource.AddBatchToImplementationLedger(batchNumber, DateTime.Now, operatorSource.FindBatchOperator(1));
            receivedBatchSource.SaveReceivedBatch(new ReceivedBatch("White", whiteBatch, activityDate, 5, 22222, operatorSource.FindBatchOperator(1)));

            viewModel = new ReceivedPurchaseOrderEditorViewModel(
                receivedBatchSource.GetPurchaseOrderForEditing(22222),
                new DuracolorIntermixColorList(),
                new DuracolorIntermixBatchNumberValidator(),
                operatorSource,
                inventorySource,
                receivedBatchSource,
                implementedBatchSource
            );

            command = new DeletePurchaseOrderReceivingRecordCommand(viewModel);
            viewModel.ReceivedBatchesSelectedIndex = 0;

            Assert.True(command.CanExecute(null));
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
        public void ExecutedCommandWillResetTheCurrentlySelectedBatch()
        {
            string expectedQuantity = "0";
            string expectedBatchNumber = string.Empty;
            int expectedColorIndex = 0;
            int expectedSelectedIndex = -1;

            viewModel.ReceivedBatchesSelectedIndex = 1;
            command.Execute(null);

            Assert.AreEqual(expectedQuantity, viewModel.Quantity);
            Assert.AreEqual(expectedBatchNumber, viewModel.BatchNumber);
            Assert.AreEqual(expectedColorIndex, viewModel.SelectedColorIndex);
            Assert.AreEqual(expectedSelectedIndex, viewModel.ReceivedBatchesSelectedIndex);
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

            while (viewModel.ReceivedBatches.Count > 0)
            {
                viewModel.ReceivedBatchesSelectedIndex = 0;
                loop++;
                command.Execute(null);
            }

            Assert.AreEqual(expectedCount, viewModel.ReceivedBatches.Count);
            Assert.AreEqual(expectedCount, receivedBatchSource.GetReceivedBatchesByPONumber(int.Parse(viewModel.PONumber)).Count);
        }
    }
}
