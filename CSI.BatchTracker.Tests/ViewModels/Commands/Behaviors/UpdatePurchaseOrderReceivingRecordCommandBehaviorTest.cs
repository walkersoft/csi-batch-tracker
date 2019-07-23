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
        public void CommandWillNotExecuteIfSelectedRecordsDoesNotHaveAPositibeQuantity()
        {
            viewModel.ReceivedBatchesSelectedIndex = 0;
            viewModel.BatchNumber = whiteBatch;
            viewModel.Quantity = "0";

            Assert.False(command.CanExecute(null));
        }

        [Test]
        public void CommandWillNotExecuteIfQuantityIsNotGreaterThanOrEqualToAmountAlreadyImplemented()
        {
            viewModel.ReceivedBatchesSelectedIndex = 0;
            viewModel.ReceivedBatch.Quantity = 2;
            string batchNumber = viewModel.ReceivedBatches[viewModel.ReceivedBatchesSelectedIndex].BatchNumber;
            BatchOperator batchOperator = viewModel.ReceivedBatches[viewModel.ReceivedBatchesSelectedIndex].ReceivingOperator;

            implementedBatchSource.AddBatchToImplementationLedger(batchNumber, DateTime.Now, batchOperator);
            implementedBatchSource.AddBatchToImplementationLedger(batchNumber, DateTime.Now, batchOperator);
            implementedBatchSource.AddBatchToImplementationLedger(batchNumber, DateTime.Now, batchOperator);

            Assert.False(command.CanExecute(null));            
        }

        [Test]
        public void ExecutedCommandWithChangedBatchNumberUpdatesAllLedgers()
        {
            viewModel.ReceivedBatchesSelectedIndex = 0;
            string batchNumber = viewModel.ReceivedBatches[viewModel.ReceivedBatchesSelectedIndex].BatchNumber;
            string expectedBatchNumber = "872894502301";
            BatchOperator batchOperator = viewModel.ReceivedBatches[viewModel.ReceivedBatchesSelectedIndex].ReceivingOperator;
            viewModel.ReceivedBatch = viewModel.ReceivedBatches[viewModel.ReceivedBatchesSelectedIndex];

            implementedBatchSource.AddBatchToImplementationLedger(batchNumber, DateTime.Now, batchOperator);
            viewModel.ReceivedBatch.BatchNumber = expectedBatchNumber;
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
            string expectedColorName = "Black";
            BatchOperator batchOperator = viewModel.ReceivedBatches[viewModel.ReceivedBatchesSelectedIndex].ReceivingOperator;
            viewModel.ReceivedBatch = viewModel.ReceivedBatches[viewModel.ReceivedBatchesSelectedIndex];

            implementedBatchSource.AddBatchToImplementationLedger(batchNumber, DateTime.Now, batchOperator);
            viewModel.SelectedColorIndex = 1;
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
            viewModel.ReceivedBatch = viewModel.ReceivedBatches[viewModel.ReceivedBatchesSelectedIndex];

            implementedBatchSource.AddBatchToImplementationLedger(batchNumber, DateTime.Now, batchOperator);
            viewModel.ReceivedBatch.Quantity = 6;
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
            viewModel.ReceivedBatch = viewModel.ReceivedBatches[viewModel.ReceivedBatchesSelectedIndex];

            implementedBatchSource.AddBatchToImplementationLedger(batchNumber, DateTime.Now, batchOperator);
            viewModel.ReceivedBatch.Quantity = 4;
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
            viewModel.ReceivedBatch = viewModel.ReceivedBatches[viewModel.ReceivedBatchesSelectedIndex];

            implementedBatchSource.AddBatchToImplementationLedger(batchNumber, DateTime.Now, batchOperator);
            viewModel.ReceivedBatch.Quantity = 1;

            Assert.AreEqual(expectedBeforeCount, inventorySource.CurrentInventory.Count);

            command.Execute(null);

            Assert.AreEqual(expectedAfterCount, inventorySource.CurrentInventory.Count);
        }
    }
}
