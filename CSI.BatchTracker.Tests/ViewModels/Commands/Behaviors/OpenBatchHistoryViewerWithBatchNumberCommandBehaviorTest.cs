using CSI.BatchTracker.Tests.Views;
using NUnit.Framework;

namespace CSI.BatchTracker.Tests.ViewModels.Commands.Behaviors
{
    [TestFixture]
    abstract class OpenBatchHistoryViewerWithBatchNumberCommandBehaviorTest : MainWindowViewModelCommandBehaviorTestingBase
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
        }

        [Test]
        public void CommandWillNotExecuteIfViewIsNotSet()
        {
            Assert.False(command.CanExecute(null));
        }

        [Test]
        public void CommandWillNotExecuteIfViewCannotShowItself()
        {
            SetupInventoryStateAndReceiveSingleBatchAndReturnBatchNumber();
            viewModel.ImplementedBatchSelectedIndex = 0;
            viewModel.BatchHistoryViewer = new IBatchHistoryViewerTestStub();
            Assert.False(command.CanExecute(null));
        }

        [Test]
        public void CommandWillNotExecuteIfImplementationLedgerIsEmpty()
        {
            viewModel.ImplementedBatchSelectedIndex = 0;
            viewModel.BatchHistoryViewer = new IBatchHistoryViewerTestStub();
            Assert.False(command.CanExecute(null));
        }

        [Test]
        public void CommandWillNotExecuteIfItemIsNotSelectedInImplementationLedger()
        {
            SetupInventoryStateAndReceiveSingleBatchAndReturnBatchNumber();
            viewModel.ImplementedBatchSelectedIndex = -1;
            Assert.False(command.CanExecute(null));
        }

        [Test]
        public void CommandWillNotExecuteIfViewIsNotSetEvenWhenImplementationLedgerIsSetAndAnItemIsSelected()
        {
            SetupInventoryStateAndReceiveSingleBatchAndReturnBatchNumber();
            viewModel.ImplementedBatchSelectedIndex = 0;
            viewModel.BatchHistoryViewer = new IBatchHistoryViewerTestStub();

            Assert.False(command.CanExecute(null));
        }

        [Test]
        public void CommandExecutesIfViewCanShowItselfAndImplementedBatchIsSelected()
        {
            SetupInventoryStateAndReceiveSingleBatchAndReturnBatchNumber();
            viewModel.ImplementedBatchSelectedIndex = 0;
            viewModel.BatchHistoryViewer = new PassableIBatchHistoryViewerTestStub();
            Assert.True(command.CanExecute(null));
        }

        [Test]
        public void ExecutedCommandWillCallIViewShowViewMethod()
        {
            SetupInventoryStateAndReceiveSingleBatchAndReturnBatchNumber();
            viewModel.ImplementedBatchSelectedIndex = 0;
            viewModel.BatchHistoryViewer = new IBatchHistoryViewerTestStub();
            Assert.DoesNotThrow(() => command.Execute(null));
        }
    }
}
