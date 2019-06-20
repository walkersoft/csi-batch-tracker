using CSI.BatchTracker.Domain.NativeModels;
using NUnit.Framework;
using System;

namespace CSI.BatchTracker.Tests.ViewModels.Commands.Behaviors
{
    [TestFixture]
    abstract class ListReceivingRecordsByDatePeriodCommandBehaviorTest : ReceivingHistoryViewModelCommandBehaviorTestingBase
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
        }

        [Test]
        public void CommandWillAlwaysBeAbleToExecute()
        {
            Assert.True(command.CanExecute(null));
        }

        [Test]
        public void ExecutedCommandWillReturnResultsFromTheLastThirtyDays()
        {
            int expectedRecordCount = 1;
            int expectedBatchCount = 1;
            DateTime targetDate = DateTime.Today;
            ReceivedBatch inRangeBatch = helper.GetBatchWithSpecificDate(targetDate);
            ReceivedBatch outOfRangeBatch = helper.GetBatchWithSpecificDate(targetDate.AddDays(-31));

            receivedBatchSource.SaveReceivedBatch(inRangeBatch);
            receivedBatchSource.SaveReceivedBatch(outOfRangeBatch);
            viewModel.SearchCriteriaSelectedIndex = 1;
            viewModel.DatePeriodSelectedIndex = 0;
            command.Execute(null);

            Assert.AreEqual(expectedRecordCount, viewModel.RetreivedRecordsLedger.Count);
            Assert.AreEqual(expectedBatchCount, viewModel.RetreivedRecordsLedger[0].ReceivedBatches.Count);
        }

        [Test]
        public void ExecutedCommandWillReturnResolutsFromTheLastNinetyDays()
        {
            int expectedRecordCount = 1;
            int expectedBatchCount = 2;
            DateTime targetDate = DateTime.Today;
            ReceivedBatch inRangeBatch1 = helper.GetBatchWithSpecificDate(targetDate);
            ReceivedBatch inRangeBatch2 = helper.GetBatchWithSpecificDate(targetDate.AddDays(-30));
            ReceivedBatch outOfRangeBatch = helper.GetBatchWithSpecificDate(targetDate.AddDays(-91));

            receivedBatchSource.SaveReceivedBatch(inRangeBatch1);
            receivedBatchSource.SaveReceivedBatch(inRangeBatch2);
            receivedBatchSource.SaveReceivedBatch(outOfRangeBatch);
            viewModel.SearchCriteriaSelectedIndex = 1;
            viewModel.DatePeriodSelectedIndex = 1;
            command.Execute(null);

            Assert.AreEqual(expectedRecordCount, viewModel.RetreivedRecordsLedger.Count);
            Assert.AreEqual(expectedBatchCount, viewModel.RetreivedRecordsLedger[0].ReceivedBatches.Count);
        }

        [Test]
        public void ExecutedCommandWillReturnResultsFromTheThreeHundredSixtyFiveDays()
        {
            int expectedRecordCount = 1;
            int expectedBatchCount = 3;
            DateTime targetDate = DateTime.Today;
            ReceivedBatch inRangeBatch1 = helper.GetBatchWithSpecificDate(targetDate);
            ReceivedBatch inRangeBatch2 = helper.GetBatchWithSpecificDate(targetDate.AddDays(-30));
            ReceivedBatch inRangeBatch3 = helper.GetBatchWithSpecificDate(targetDate.AddDays(-180));
            ReceivedBatch outOfRangeBatch = helper.GetBatchWithSpecificDate(targetDate.AddDays(-366));

            receivedBatchSource.SaveReceivedBatch(inRangeBatch1);
            receivedBatchSource.SaveReceivedBatch(inRangeBatch2);
            receivedBatchSource.SaveReceivedBatch(inRangeBatch3);
            receivedBatchSource.SaveReceivedBatch(outOfRangeBatch);
            viewModel.SearchCriteriaSelectedIndex = 1;
            viewModel.DatePeriodSelectedIndex = 2;
            command.Execute(null);

            Assert.AreEqual(expectedRecordCount, viewModel.RetreivedRecordsLedger.Count);
            Assert.AreEqual(expectedBatchCount, viewModel.RetreivedRecordsLedger[0].ReceivedBatches.Count);
        }
    }
}
