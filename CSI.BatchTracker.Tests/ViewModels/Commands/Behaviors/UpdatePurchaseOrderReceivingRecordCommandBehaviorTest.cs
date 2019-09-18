using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.Exceptions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Tests.ViewModels.Commands.Behaviors
{
    [TestFixture]
    abstract class UpdatePurchaseOrderReceivingRecordCommandBehaviorTest : ReceivedPurchaseOrderEditorViewModelTestingBase
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
        }

        [Test]
        public void CommandWillNotExecuteIfReceivedRecordIsNotSelected()
        {
            viewModel.ReceivedBatchesSelectedIndex = -1;
            Assert.False(command.CanExecute(null));
        }

        [Test]
        public void CommandWillNotExecuteIfSelectedRecordDoesNotHaveAValidBatchNumber()
        {
            viewModel.ReceivedBatchesSelectedIndex = 0;
            viewModel.BatchNumber = string.Empty;

            Assert.False(command.CanExecute(null));
        }

        [Test]
        public void CommandWillNotExecuteIfSelectedRecordsDoesNotHaveAPositiveQuantity()
        {
            viewModel.ReceivedBatchesSelectedIndex = 0;
            viewModel.BatchNumber = whiteBatch;
            viewModel.Quantity = "0";

            Assert.False(command.CanExecute(null));
        }

        [Test]
        public void CommandWillNotExecuteIfSelectedRecordQuantityIsNotANumber()
        {
            viewModel.ReceivedBatchesSelectedIndex = 0;
            viewModel.BatchNumber = whiteBatch;
            viewModel.Quantity = "foo";

            Assert.False(command.CanExecute(null));
        }

        [Test]
        public void CommandWillNotExecuteIfQuantityIsNotGreaterThanOrEqualToAmountAlreadyImplemented()
        {
            BatchOperator batchOperator = operatorSource.FindBatchOperator(originalBatchOperatorId);

            implementedBatchSource.AddBatchToImplementationLedger(whiteBatch, DateTime.Now, batchOperator);
            implementedBatchSource.AddBatchToImplementationLedger(whiteBatch, DateTime.Now, batchOperator);
            implementedBatchSource.AddBatchToImplementationLedger(whiteBatch, DateTime.Now, batchOperator);

            viewModel.ReceivedBatchesSelectedIndex = 0;
            viewModel.Quantity = "2";

            Assert.False(command.CanExecute(null));            
        }

        [Test]
        public void CommandWillNotExecuteIfEditedRecordExistsInReceivingAndTheColorIsDifferent()
        {
            viewModel.ReceivedBatchesSelectedIndex = 0;
            viewModel.SelectedColorIndex = 1;

            Assert.False(command.CanExecute(null));
        }

        [Test]
        public void CommandWillExecuteIfEditedRecordHasBatchNumberThatMatchesExistingColor()
        {
            viewModel.ReceivedBatchesSelectedIndex = 0;
            viewModel.SelectedColorIndex = 0;
            viewModel.PopulateSelectedReceivedRecord();

            Assert.True(command.CanExecute(null));
        }

        [Test]
        public void ExecutedCommandWithChangedBatchNumberUpdatesAllLedgers()
        {
            viewModel.ReceivedBatchesSelectedIndex = 0;
            string batchNumber = viewModel.ReceivedBatches[viewModel.ReceivedBatchesSelectedIndex].BatchNumber;
            string expectedBatchNumber = "872894502301";
            BatchOperator batchOperator = viewModel.ReceivedBatches[viewModel.ReceivedBatchesSelectedIndex].ReceivingOperator;

            implementedBatchSource.AddBatchToImplementationLedger(yellowBatch, DateTime.Now, batchOperator);
            implementedBatchSource.AddBatchToImplementationLedger(batchNumber, DateTime.Now, batchOperator);
            viewModel.PopulateSelectedReceivedRecord();
            viewModel.BatchNumber = expectedBatchNumber;
            command.Execute(null);

            Assert.AreEqual(expectedBatchNumber, receivedBatchSource.GetReceivedBatchesByBatchNumber(expectedBatchNumber)[0].BatchNumber);
            Assert.AreEqual(expectedBatchNumber, implementedBatchSource.GetImplementedBatchesByBatchNumber(expectedBatchNumber)[0].BatchNumber);
            Assert.AreEqual(expectedBatchNumber, inventorySource.FindInventoryBatchByBatchNumber(expectedBatchNumber).BatchNumber);
        }

        [Test]
        public void ExecutedCommandWithChangedColorUpdatesAllLedgers()
        {
            viewModel.ReceivedBatchesSelectedIndex = 0;
            string batchNumber = viewModel.ReceivedBatches[viewModel.ReceivedBatchesSelectedIndex].BatchNumber;
            string expectedColorName = "Red";
            BatchOperator batchOperator = viewModel.ReceivedBatches[viewModel.ReceivedBatchesSelectedIndex].ReceivingOperator;

            implementedBatchSource.AddBatchToImplementationLedger(batchNumber, DateTime.Now, batchOperator);
            viewModel.PopulateSelectedReceivedRecord();
            viewModel.SelectedColorIndex = 3;
            command.Execute(null);

            Assert.AreEqual(expectedColorName, receivedBatchSource.GetReceivedBatchesByBatchNumber(batchNumber)[0].ColorName);
            Assert.AreEqual(expectedColorName, implementedBatchSource.GetImplementedBatchesByBatchNumber(batchNumber)[0].ColorName);
            Assert.AreEqual(expectedColorName, inventorySource.FindInventoryBatchByBatchNumber(batchNumber).ColorName);
        }

        [Test]
        public void ExecutedCommandWithIncreaseInQuantityWillAddAppropriateAmountInInventoryLedger()
        {
            viewModel.ReceivedBatchesSelectedIndex = 0;
            string batchNumber = viewModel.ReceivedBatches[viewModel.ReceivedBatchesSelectedIndex].BatchNumber;
            int expectedQuantity = 5;
            BatchOperator batchOperator = viewModel.ReceivedBatches[viewModel.ReceivedBatchesSelectedIndex].ReceivingOperator;

            implementedBatchSource.AddBatchToImplementationLedger(batchNumber, DateTime.Now, batchOperator);
            viewModel.PopulateSelectedReceivedRecord();
            viewModel.Quantity = "6";
            command.Execute(null);

            Assert.AreEqual(expectedQuantity, inventorySource.FindInventoryBatchByBatchNumber(batchNumber).Quantity);
        }

        [Test]
        public void ExecutedCommandWithDecreaseInQuantityWillDeductAppropriateAmountInInventoryLedger()
        {
            viewModel.ReceivedBatchesSelectedIndex = 0;
            string batchNumber = viewModel.ReceivedBatches[viewModel.ReceivedBatchesSelectedIndex].BatchNumber;
            int expectedQuantity = 3;
            BatchOperator batchOperator = viewModel.ReceivedBatches[viewModel.ReceivedBatchesSelectedIndex].ReceivingOperator;

            implementedBatchSource.AddBatchToImplementationLedger(batchNumber, DateTime.Now, batchOperator);
            viewModel.PopulateSelectedReceivedRecord();
            viewModel.Quantity = "4";
            command.Execute(null);

            Assert.AreEqual(expectedQuantity, inventorySource.FindInventoryBatchByBatchNumber(batchNumber).Quantity);
        }

        [Test]
        public void ExecutedCommandWithDecreaseInQuantityResultingInZeroInventoryWillRemoveRecordFromActiveInventory()
        {
            viewModel.ReceivedBatchesSelectedIndex = 0;
            string batchNumber = viewModel.ReceivedBatches[viewModel.ReceivedBatchesSelectedIndex].BatchNumber;
            int expectedBeforeCount = 3;
            int expectedAfterCount = 2;
            BatchOperator batchOperator = viewModel.ReceivedBatches[viewModel.ReceivedBatchesSelectedIndex].ReceivingOperator;

            implementedBatchSource.AddBatchToImplementationLedger(batchNumber, DateTime.Now, batchOperator);
            viewModel.PopulateSelectedReceivedRecord();
            viewModel.Quantity = "1";

            Assert.AreEqual(expectedBeforeCount, inventorySource.CurrentInventory.Count);

            command.Execute(null);

            Assert.AreEqual(expectedAfterCount, inventorySource.CurrentInventory.Count);
        }

        [Test]
        public void ExecutedCommandWithIncreaseInQuantityButNothingInInventoryResultsInInventoryRecordBeingCreated()
        {
            int expectedBeforeCount = 2;
            int expectedAfterCount = 3;
            int expectedInventoryQuantity = 2;
            BatchOperator batchOperator = operatorSource.FindBatchOperator(originalBatchOperatorId);

            while (inventorySource.CurrentInventory.Count > expectedBeforeCount)
            {
                implementedBatchSource.AddBatchToImplementationLedger(whiteBatch, DateTime.Now, batchOperator);
            }

            Assert.AreEqual(expectedBeforeCount, inventorySource.CurrentInventory.Count);

            viewModel.ReceivedBatchesSelectedIndex = 0;
            viewModel.PopulateSelectedReceivedRecord();
            viewModel.Quantity = "7";

            command.Execute(null);

            Assert.AreEqual(expectedAfterCount, inventorySource.CurrentInventory.Count);
            Assert.AreEqual(expectedInventoryQuantity, inventorySource.FindInventoryBatchByBatchNumber(whiteBatch).Quantity);
        }

        [Test]
        public void ExecutedCommandThatChangesColorAndBatchNumberOfExistingInventoryItemsWillMergeLikeBatches()
        {
            int expectedInventoryCountBefore = 3;
            int expectedInventoryCountAfter = 2;
            int expectedBatchQuantity = 10;

            viewModel.ReceivedBatchesSelectedIndex = 0;
            viewModel.PopulateSelectedReceivedRecord();

            Assert.AreEqual(expectedInventoryCountBefore, inventorySource.CurrentInventory.Count);

            viewModel.BatchNumber = blackBatch;
            viewModel.SelectedColorIndex = 1;
            command.Execute(null);

            Assert.AreEqual(expectedInventoryCountAfter, inventorySource.CurrentInventory.Count);
            Assert.AreEqual(expectedBatchQuantity, inventorySource.FindInventoryBatchByBatchNumber(blackBatch).Quantity);
        }
    }
}
