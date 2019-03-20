using CSI.BatchTracker.Tests.Views;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CSI.BatchTracker.Tests.ViewModels.Commands.Behaviors
{
    [TestFixture]
    abstract class OpenReceivingManagementSessionViewCommandBehaviorTest : MainWindowViewModelCommandBehaviorTestingBase
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
            viewModel.ReceivingManagementSessionViewer = new IViewTestStub();
            Assert.False(command.CanExecute(null));
        }

        [Test]
        public void CommandWillExecuteIfViewIsSetAndCanShowItself()
        {
            viewModel.ReceivingManagementSessionViewer = new PassableIViewTestStub();
            Assert.True(command.CanExecute(null));
        }

        [Test]
        public void ExecutedCommandWillCallIViewShowViewMethod()
        {
            viewModel.ReceivingManagementSessionViewer = new IViewTestStub();
            Assert.DoesNotThrow(() => command.Execute(null));
        }
    }
}
