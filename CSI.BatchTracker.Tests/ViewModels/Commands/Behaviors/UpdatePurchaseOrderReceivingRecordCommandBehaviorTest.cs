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
        public void CommandWillNotExecuteIfQuantityChangeIsGreaterThanTheAmountAvialableInInventory()
        {
            viewModel.ReceivedBatchesSelectedIndex = 0;
        }
    }
}
