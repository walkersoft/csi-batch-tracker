using CSI.BatchTracker.Domain;
using CSI.BatchTracker.Domain.DataSource.Contracts;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.Tests.Views;
using CSI.BatchTracker.ViewModels;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Tests.ViewModels.Commands.Behaviors
{
    [TestFixture]
    abstract class OpenPurchaseOrderEditorCommandBehaviorTest : ReceivingHistoryViewModelCommandBehaviorTestingBase
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
            viewModel.PurchaseOrderEditorViewer = new IViewTestStub();
            Assert.False(command.CanExecute(null));
        }

        [Test]
        public void CommandWillExecuteIfViewIsSetAndCanShowItself()
        {
            viewModel.PurchaseOrderEditorViewer = new PassableIViewTestStub();
            Assert.True(command.CanExecute(null));
        }

        [Test]
        public void ExecutedCommandWillCallIViewShowViewMethod()
        {
            viewModel.PurchaseOrderEditorViewer = new IViewTestStub();
            Assert.DoesNotThrow(() => command.Execute(null));
        }
    }
}
