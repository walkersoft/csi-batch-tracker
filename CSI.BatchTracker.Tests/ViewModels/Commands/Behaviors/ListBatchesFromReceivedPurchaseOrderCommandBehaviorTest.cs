using CSI.BatchTracker.Domain.NativeModels;
using NUnit.Framework;

namespace CSI.BatchTracker.Tests.ViewModels.Commands.Behaviors
{
    [TestFixture]
    abstract class ListBatchesFromReceivedPurchaseOrderCommandBehaviorTest : ReceivingHistoryViewModelCommandBehaviorTestingBase
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
        }

        [Test]
        public void DefaultCommandSetupWillNotExecute()
        {
            Assert.False(command.CanExecute(null));
        }

        [Test]
        public void CommandWillNotExecuteIfNoPurchaseOrderRecordIsSelected()
        {
            viewModel.RetreivedRecordsLedgerSelectedIndex = -1;
            Assert.False(command.CanExecute(null));
        }

        [Test]
        public void CommandWillNotExecuteIfRetrievedRecordLedgerIsEmpty()
        {
            viewModel.RetreivedRecordsLedger.Clear();
            viewModel.RetreivedRecordsLedgerSelectedIndex = 0;
            Assert.False(command.CanExecute(null));
        }

        [Test]
        public void CommandWillExecuteIfRetreivedRecordLedgerIsPopulatedAndPurchaseOrderIsSelected()
        {
            viewModel.RetreivedRecordsLedger.Add(CreatePurchaseOrderWithSingleEntry());
            viewModel.RetreivedRecordsLedgerSelectedIndex = 0;
            Assert.True(command.CanExecute(null));
        }

        [Test]
        public void ExecutedCommandWillPopulateReceivedBatchCollectionForTheSelectedPurchaseOrderRecord()
        {
            int expectedLedgerCount = 1;
            int expectedPreviewCount = 2;
            ReceivedPurchaseOrder receivedPO = CreatePurchaseOrderWithSingleEntry();

            viewModel.RetreivedRecordsLedger.Add(receivedPO);
            viewModel.RetreivedRecordsLedgerSelectedIndex = 0;

            command.Execute(null);
            
            Assert.AreEqual(expectedLedgerCount, viewModel.RetreivedRecordsLedger.Count);
            Assert.AreEqual(expectedPreviewCount, viewModel.SelectedPurchaseOrderReceivedBatches.Count);
        }

        [Test]
        public void CommandThatCanNoLongerExecuteWillClearTheReceivedBatchesCollection()
        {
            int expectedLedgerCountBefore = 1;
            int expectedPreviewCountBefore = 2;
            int expectedLedgerCountAfter = 0;
            int expectedPreviewCountAfter = 0;
            ReceivedPurchaseOrder receivedPO = CreatePurchaseOrderWithSingleEntry();

            viewModel.RetreivedRecordsLedger.Add(receivedPO);
            viewModel.RetreivedRecordsLedgerSelectedIndex = 0;
            command.Execute(null);

            Assert.AreEqual(expectedLedgerCountBefore, viewModel.RetreivedRecordsLedger.Count);
            Assert.AreEqual(expectedPreviewCountBefore, viewModel.SelectedPurchaseOrderReceivedBatches.Count);

            viewModel.RetreivedRecordsLedger.Clear();
            viewModel.RetreivedRecordsLedgerSelectedIndex = -1;
            command.CanExecute(null);

            Assert.AreEqual(expectedLedgerCountAfter, viewModel.RetreivedRecordsLedger.Count);
            Assert.AreEqual(expectedPreviewCountAfter, viewModel.SelectedPurchaseOrderReceivedBatches.Count);
        }

        ReceivedPurchaseOrder CreatePurchaseOrderWithSingleEntry()
        {
            ReceivedBatch firstBatch = helper.GetUniqueBatch1();
            ReceivedBatch secondBatch = helper.GetUniqueBatch2();

            ReceivedPurchaseOrder receivedPO = new ReceivedPurchaseOrder(
                firstBatch.PONumber,
                firstBatch.ActivityDate,
                firstBatch.ReceivingOperator
            );

            receivedPO.AddBatch(firstBatch);
            receivedPO.AddBatch(secondBatch);

            return receivedPO;
        }
    }
}
