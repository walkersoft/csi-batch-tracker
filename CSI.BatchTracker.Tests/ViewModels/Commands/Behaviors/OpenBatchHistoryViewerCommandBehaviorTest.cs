using CSI.BatchTracker.Tests.Views;
using NUnit.Framework;

namespace CSI.BatchTracker.Tests.ViewModels.Commands.Behaviors
{
    [TestFixture]
    abstract class OpenBatchHistoryViewerCommandBehaviorTest : MainWindowViewModelCommandBehaviorTestingBase
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
        public void CommandExecutesIfViewCanShowItself()
        {
            viewModel.BatchHistoryViewer = new PassableIBatchHistoryViewerTestStub();
            Assert.True(command.CanExecute(null));
        }

        [Test]
        public void ExecutedCommandWillCallIViewShowViewMethod()
        {
            viewModel.BatchHistoryViewer = new IBatchHistoryViewerTestStub();
            Assert.DoesNotThrow(() => command.Execute(null));
        }
    }
}
