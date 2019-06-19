using CSI.BatchTracker.Domain.NativeModels;
using NUnit.Framework;

namespace CSI.BatchTracker.Tests.ViewModels.Commands.Behaviors
{
    [TestFixture]
    abstract class ListReceivingRecordsByPONumberCommandBehaviorTest : ReceivingHistoryViewModelCommandBehaviorTestingBase
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
        }

        [Test]
        public void CommandWillNotExecuteIfPOFieldIsEmpty()
        {
            viewModel.PONumber = string.Empty;
            Assert.False(command.CanExecute(null));
        }

        [Test]
        public void CommandWillNotExecuteIfPOFieldDoesNotContainAnIntegerValue()
        {
            viewModel.PONumber = "foo";
            Assert.False(command.CanExecute(null));
        }

        [Test]
        public void CommandWillExecuteIfPOFieldIsPopulatedAndContainsAnIntegerValue()
        {
            viewModel.PONumber = "11111";
            Assert.True(command.CanExecute(null));
        }

        [Test]
        public void ExecutedCommandDoesNotShowAnEntryInTheRetreivedRecordsLedgerIfThePONumberDoesNotExists()
        {
            int expectedCount = 0;
            viewModel.PONumber = "11111";

            viewModel.SearchCriteriaSelectedIndex = 3;
            command.Execute(null);

            Assert.AreEqual(expectedCount, viewModel.RetreivedRecordsLedger.Count);
        }

        [Test]
        public void ExecutedCommandShowsAnEntryInTheRetreivedRecordsLedgerIfThePONumberExists()
        {
            int expectedCount = 1;
            int poNumber = 11111;
            ReceivedBatch batch = helper.GetBatchWithSpecificPO(poNumber);

            receivedBatchSource.SaveReceivedBatch(batch);
            viewModel.SearchCriteriaSelectedIndex = 3;
            viewModel.PONumber = poNumber.ToString();
            command.Execute(null);

            Assert.AreEqual(expectedCount, viewModel.RetreivedRecordsLedger.Count);
            Assert.AreEqual(expectedCount, viewModel.RetreivedRecordsLedger[0].ReceivedBatches.Count);
            Assert.AreEqual(poNumber, viewModel.RetreivedRecordsLedger[0].PONumber);
        }
    }
}
