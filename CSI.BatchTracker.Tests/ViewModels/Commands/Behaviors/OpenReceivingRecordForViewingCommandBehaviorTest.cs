using CSI.BatchTracker.Domain.NativeModels;
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
    abstract class OpenReceivingRecordForViewingCommandBehaviorTest : ReceivingHistoryViewModelCommandBehaviorTestingBase
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
        }

        [Test]
        public void CommandWillNotExecuteIfReceivedPurchaseOrderLedgerIsEmpty()
        {
            Assert.False(command.CanExecute(null));
        }

        [Test]
        public void CommandWillNotExecuteIfReceivedPurchaseOrderIsNotSelected()
        {
            viewModel.RetreivedRecordsLedgerSelectedIndex = -1;
            Assert.False(command.CanExecute(null));
        }

        [Test]
        public void CommandWillNotExecuteIfReceivingHistoryWindowIsNotReady()
        {
            AddPurchaseOrderToRecordsLedgerAndSelect();
            Assert.False(command.CanExecute(null));
        }

        [Test]
        public void CommandWillNotExecuteIfReceivedPurchaseOrderIsSelectedAndReceivingReceivingHistoryWindowIsNotSet()
        {
            AddPurchaseOrderToRecordsLedgerAndSelect();
            viewModel.ReceivingSessionViewer = new IViewTestStub();
            Assert.False(command.CanExecute(null));
        }

        [Test]
        public void CommandWillExecuteIfReceivedPurchaseOrderIsSelectedAndReceivingReceivingHistoryWindowCanShowItself()
        {
            AddPurchaseOrderToRecordsLedgerAndSelect();
            viewModel.ReceivingSessionViewer = new PassableIViewTestStub();
            Assert.True(command.CanExecute(null));
        }

        [Test]
        public void ExecutedCommandDoesNotThrowAnyExceptions()
        {
            AddPurchaseOrderToRecordsLedgerAndSelect();
            viewModel.ReceivingSessionViewer = new PassableIViewTestStub();
            Assert.DoesNotThrow(() => command.Execute(null));
        }

        void AddPurchaseOrderToRecordsLedgerAndSelect()
        {
            ReceivedPurchaseOrder po = new ReceivedPurchaseOrder(11111, DateTime.Now, new BatchOperator("Jane", "Doe"));
            viewModel.RetreivedRecordsLedger.Add(po);
            viewModel.RetreivedRecordsLedgerSelectedIndex = 0;
        }
    }
}
