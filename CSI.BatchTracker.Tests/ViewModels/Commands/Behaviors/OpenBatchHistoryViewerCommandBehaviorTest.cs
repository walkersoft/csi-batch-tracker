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
            viewModel.BatchHistoryViewer = new IViewTestStub();
            Assert.False(command.CanExecute(null));
        }

        [Test]
        public void CommandExecutesIfViewCanShowItself()
        {
            viewModel.BatchHistoryViewer = new PassableIViewTestStub();
            Assert.True(command.CanExecute(null));
        }

        [Test]
        public void ExecutedCommandWillCallIViewShowViewMethod()
        {
            viewModel.BatchHistoryViewer = new IViewTestStub();
            Assert.DoesNotThrow(() => command.Execute(null));
        }
    }
}
