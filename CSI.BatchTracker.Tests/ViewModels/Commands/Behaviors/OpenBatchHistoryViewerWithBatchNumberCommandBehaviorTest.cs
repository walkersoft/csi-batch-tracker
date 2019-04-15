using CSI.BatchTracker.Tests.Views;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            viewModel.BatchHistoryViewer = new IBatchHistoryViewerTestStub();
            Assert.False(command.CanExecute(null));
        }

        [Test]
        public void CommandWillNotExecuteIfImplementationLedgerIsEmpty()
        {
            viewModel.BatchHistoryViewer = new IBatchHistoryViewerTestStub();
            Assert.False(command.CanExecute(null));
        }

        [Test]
        public void CommandWillNotExecuteIfItemIsNotSelectedInImplementationLedger()
        {
            SetupInventoryStateAndReceiveSingleBatchAndReturnBatchNumber();
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
