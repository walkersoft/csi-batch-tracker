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
        public void CommandWillNotExecuteIfSelectedRecordDoesNotHaveAValidColor()
        {
            viewModel.ReceivedBatchesSelectedIndex = 0;
            viewModel.SelectedColorIndex = -1;

            Assert.False(command.CanExecute(null));
        }

        [Test]
        public void CommandWillNotExecuteIfSelectedRecordDoesNotHaveAValidBatchNumber()
        {
            viewModel.ReceivedBatchesSelectedIndex = 0;
            viewModel.SelectedColorIndex = 1;
            viewModel.BatchNumber = string.Empty;

            Assert.False(command.CanExecute(null));
        }

        [Test]
        public void CommandWillNotExecuteIfSelectedRecordsDoesNotHaveAPositibeQuantity()
        {
            viewModel.ReceivedBatchesSelectedIndex = 0;
            viewModel.SelectedColorIndex = 1;
            viewModel.BatchNumber = blackBatch;
            viewModel.Quantity = "0";

            Assert.False(command.CanExecute(null));
        }

        [Test]
        public void CommandWillNotExecuteIfQuantityIsNotGreaterThanOrEqualToAmountAlreadyImplemented()
        {
            viewModel.ReceivedBatchesSelectedIndex = 0;
            viewModel.SelectedColorIndex = 1;
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
            viewModel.SelectedColorIndex = 1;
            string batchNumber = viewModel.ReceivedBatches[viewModel.ReceivedBatchesSelectedIndex].BatchNumber;
            string newBatchNumber = "872894502301";
            BatchOperator batchOperator = viewModel.ReceivedBatches[viewModel.ReceivedBatchesSelectedIndex].ReceivingOperator;
            viewModel.ReceivedBatch = viewModel.ReceivedBatches[viewModel.ReceivedBatchesSelectedIndex];

            implementedBatchSource.AddBatchToImplementationLedger(batchNumber, DateTime.Now, batchOperator);
            viewModel.ReceivedBatch.BatchNumber = newBatchNumber;
            command.Execute(null);

            Assert.AreEqual(newBatchNumber, receivedBatchSource.GetReceivedBatchesByBatchNumber(newBatchNumber)[0].BatchNumber);
            Assert.AreEqual(newBatchNumber, implementedBatchSource.GetImplementedBatchesByBatchNumber(newBatchNumber)[0].BatchNumber);
            Assert.AreEqual(newBatchNumber, inventorySource.FindInventoryBatchByBatchNumber(newBatchNumber).BatchNumber);
        }
    }
}
