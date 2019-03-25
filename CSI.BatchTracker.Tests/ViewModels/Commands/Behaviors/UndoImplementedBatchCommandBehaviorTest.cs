using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Tests.ViewModels.Commands.Behaviors
{
    [TestFixture]
    abstract class UndoImplementedBatchCommandBehaviorTest : MainWindowViewModelCommandBehaviorTestingBase
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
        }

        [Test]
        public void CommandWillNotExecuteIfImplementationLedgerIsEmpty()
        {
            Assert.False(command.CanExecute(null));
        }

        [Test]
        public void CommandWillNotExecuteIfNoBatchIsSelectedInLedger()
        {
            SetupInventoryStateAndReceiveSingleBatchAndReturnBatchNumber();
            Assert.False(command.CanExecute(null));
        }

        [Test]
        public void CommandWillExecuteIfBatchIsPresentInImplementationLedgerAndIsSelected()
        {
            SetupInventoryStateAndReceiveSingleBatchAndReturnBatchNumber();
            viewModel.ImplementedBatchSelectedIndex = 0;
            Assert.True(command.CanExecute(null));
        }

        [Test]
        public void ExecutedCommandWillTakeSelectedItemFromImplementationLedgerAndReturnItToInventory()
        {
            int expectedLedgerCount = 0;
            int expectedInventoryCount = 1;
            int expectedBatchQuantity = 2;

            string batchNumber = SetupInventoryStateAndReceiveSingleBatchAndReturnBatchNumber();
            viewModel.ImplementedBatchSelectedIndex = 0;
            command.Execute(null);

            Assert.AreEqual(expectedLedgerCount, viewModel.ImplementedBatchLedger.Count);
            Assert.AreEqual(expectedInventoryCount, viewModel.CurrentInventory.Count);
            Assert.AreEqual(expectedBatchQuantity, inventorySource.FindInventoryBatchByBatchNumber(batchNumber).Quantity);
        }
    }
}
