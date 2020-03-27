using CSI.BatchTracker.Tests.ViewModels.Commands.Behaviors;
using CSI.BatchTracker.Tests.Views;
using NUnit.Framework;

namespace CSI.BatchTracker.Tests.ViewModels.Commands
{
    [TestFixture]
    abstract class OpenConnectedBatchInquiryViewCommandBehaviorTest : MainWindowViewModelCommandBehaviorTestingBase
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
            viewModel.ConnectedBatchInquiryViewer = new IViewTestStub();
            Assert.False(command.CanExecute(null));
        }

        [Test]
        public void CommandWillExecuteIfViewIsSetAndCanShowItself()
        {
            viewModel.ConnectedBatchInquiryViewer = new PassableIViewTestStub();
            Assert.True(command.CanExecute(null));
        }

        [Test]
        public void ExecutedCommandWillCallIViewShowViewMethod()
        {
            viewModel.ConnectedBatchInquiryViewer = new IViewTestStub();
            Assert.DoesNotThrow(() => command.Execute(null));
        }
    }
}
