using CSI.BatchTracker.Tests.TestHelpers.NativeModels;
using CSI.BatchTracker.Tests.ViewModels.Commands.Behaviors;
using CSI.BatchTracker.ViewModels;
using CSI.BatchTracker.ViewModels.Commands;
using NUnit.Framework;
using System.Windows.Input;

namespace CSI.BatchTracker.Tests.ViewModels.Commands
{
    [TestFixture]
    abstract class RemoveReceivableBatchFromSessionLedgerCommandBehaviorTest : ReceivingManagementViewModelCommandTestingBase
    {
        ReceivedBatchTestHelper helper;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            helper = new ReceivedBatchTestHelper(operatorSource);
            viewModel = new ReceivingManagementViewModel(validator, colorList, receivingSource, operatorSource, inventorySource);
            command = new RemoveReceivableBatchFromSessionLedgerCommand(viewModel);
        }

        [Test]
        public void UnableToExecuteWhenSessionLedgerIsEmpty()
        {
            Assert.False(command.CanExecute(null));
        }

        [Test]
        public void UnableToExecuteIfSessionLedgerDoesNotHaveItemSelected()
        {
            AddItemToSessionLedger();
            Assert.False(command.CanExecute(null));
        }

        void AddItemToSessionLedger()
        {
            SetupValidReceivedBatchInViewModel();
            InjectTwoOperatorsIntoRepository();
            ICommand addEntry = new AddReceivedBatchToReceivingSessionLedgerCommand(viewModel);
            addEntry.Execute(null);
        }

        [Test]
        public void AbleToExecuteIfEntryIsInSessionLedgerAndEntryIsSelected()
        {
            AddItemToSessionLedger();
            viewModel.SessionLedgerSelectedIndex = 0;
            Assert.True(command.CanExecute(null));
        }

        [Test]
        public void EntryIsRemovedFromSessionLedgerSuccessfully()
        {
            int expectedCountBefore = 1;
            int expectedCountAfter = 0;
            AddItemToSessionLedger();

            Assert.AreEqual(expectedCountBefore, viewModel.SessionLedger.Count);

            viewModel.SessionLedgerSelectedIndex = 0;
            command.Execute(null);

            Assert.AreEqual(expectedCountAfter, viewModel.SessionLedger.Count);
        }
    }
}
