using CSI.BatchTracker.Domain.NativeModels;
using NUnit.Framework;
using System;

namespace CSI.BatchTracker.Tests.ViewModels.Commands.Behaviors
{
    [TestFixture]
    abstract class CommitBatchToImplementationLedgerCommandBehaviorTest : MainWindowViewModelCommandBehaviorTestingBase
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
        }

        [Test]
        public void CommandExecuteStateWillBeFalseIfBatchAndDateAndOperatorAreNotSelected()
        {
            Assert.False(command.CanExecute(null));
        }

        [Test]
        public void CommandExecuteStateWillBeTrueIfBatchAndDateAndOperatorAreSelected()
        {
            SetupViewModelStateToImplementBatch();
            Assert.True(command.CanExecute(null));
        }

        void SetupViewModelStateToImplementBatch()
        {
            viewModel.ImplementableBatchSelectedIndex = 0;
            viewModel.ImplementingBatchOperatorSelectedIndex = 0;
            viewModel.ImplementationDateTime = DateTime.Now;
        }

        [Test]
        public void ExecutedCommandWillAddBatchToImplementationLedgerAndRemoveFromInventory()
        {
            ReceivedBatch receivedBatch = SetupReceivedBatchWithQuantityOfTwo();
            BatchOperator implementingOperator = operatorHelper.GetJaneDoeOperator();
            int expectedQuantity = 1;
            int expectedLedgerCount = 1;
            int expectedInventoryCount = 1;
            SetupInventoryStateToImplementBatch();
            SetupViewModelStateToImplementBatch();

            command.Execute(null);

            InventoryBatch inventoryBatch = viewModel.CurrentInventory[0];

            Assert.AreEqual(expectedQuantity, inventoryBatch.Quantity);
            Assert.AreEqual(expectedLedgerCount, viewModel.ImplementedBatchLedger.Count);
            Assert.AreEqual(expectedInventoryCount, viewModel.TotalInventoryCount);
        }
    }
}
