using CSI.BatchTracker.Domain.NativeModels;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Tests.ViewModels.Commands.Behaviors
{
    [TestFixture]
    abstract class ListReceivingRecordsBySpecficDateCommandBehaviorTest : ReceivingHistoryViewModelCommandBehaviorTestingBase
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
        }

        [Test]
        public void CommandWillNotExecuteIfDateIsAtTheMinimumDateTime()
        {
            viewModel.SpecificDate = DateTime.MinValue;
            Assert.False(command.CanExecute(null));
        }

        [Test]
        public void CommandWillExecuteIfDateIsAboveTheMinimumDateTime()
        {
            viewModel.SpecificDate = DateTime.Today;
            Assert.True(command.CanExecute(null));
        }

        [Test]
        public void ExecutedCommandResultsInNoRetreivedRecordsIfTheyDoNotExistInTheSpecificDate()
        {
            viewModel.SpecificDate = DateTime.Today;
            viewModel.SearchCriteriaSelectedIndex = 2;
            command.Execute(null);
        }

        [Test]
        public void ExecutedCommandWillRetreiveRecordsIfExistUnderTheSpecifiedDate()
        {
            DateTime targetDate = DateTime.Today;
            int expectedCount = 1;
            ReceivedBatch inRangeBatch = helper.GetBatchWithSpecificDate(targetDate);
            ReceivedBatch outOfRangeBatch = helper.GetBatchWithSpecificDate(targetDate.AddDays(1));

            receivedBatchSource.SaveReceivedBatch(outOfRangeBatch);
            receivedBatchSource.SaveReceivedBatch(inRangeBatch);
            viewModel.SearchCriteriaSelectedIndex = 2;
            viewModel.SpecificDate = targetDate;

            command.Execute(null);

            Assert.AreEqual(expectedCount, viewModel.RetreivedRecordsLedger.Count);
        }
    }
}
