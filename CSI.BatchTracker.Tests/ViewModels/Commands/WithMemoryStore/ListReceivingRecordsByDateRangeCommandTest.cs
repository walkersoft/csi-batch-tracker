using CSI.BatchTracker.Domain.DataSource.MemorySource;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.Storage.MemoryStore;
using CSI.BatchTracker.Tests.ViewModels.Commands.Behaviors;
using CSI.BatchTracker.ViewModels;
using CSI.BatchTracker.ViewModels.Commands;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Tests.ViewModels.Commands.WithMemoryStore
{
    [TestFixture]
    class ListReceivingRecordsByDateRangeCommandTest : ReceivingHistoryViewModelCommandBehaviorTestingBase
    {
        [SetUp]
        public override void SetUp()
        {
            MemoryStoreContext context = new MemoryStoreContext();
            inventorySource = new MemoryActiveInventorySource(context);
            receivedBatchSource = new MemoryReceivedBatchSource(context, inventorySource);
            viewModel = new ReceivingHistoryViewModel(receivedBatchSource, inventorySource);
            command = new ListReceivingRecordsByDateRangeCommand(viewModel);
            base.SetUp();
        }

        [Test]
        public void DefaultCommandSetupWillExecute()
        {
            Assert.True(command.CanExecute(null));
        }

        [Test]
        public void CommandWillNotExecuteIfStartingDateIsAfterEndingDateAndADifferentDay()
        {
            viewModel.DateRangeEndingDate = DateTime.Now;
            viewModel.DateRangeStartingDate = viewModel.DateRangeEndingDate.AddDays(1);

            Assert.False(command.CanExecute(null));
        }
        
        [Test]
        public void CommandWillExecuteIfEndingDateIsBeforeStartingDateButIsStillTheSameDay()
        {
            viewModel.DateRangeStartingDate = DateTime.Today.AddHours(10);
            viewModel.DateRangeEndingDate = viewModel.DateRangeStartingDate.AddHours(-2);

            Assert.True(command.CanExecute(null));
        }

        [Test]
        public void CommandWillExecuteIfStartingDateIsBeforeEndingDateAndNotTheSameDay()
        {
            viewModel.DateRangeStartingDate = DateTime.Today;
            viewModel.DateRangeEndingDate = viewModel.DateRangeStartingDate.AddDays(5);

            Assert.True(command.CanExecute(null));
        }

        [Test]
        public void ExecutedCommandWillPopulateRetreivedRecordsLedger()
        {
            int expectedCount = 2;
            viewModel.DateRangeStartingDate = DateTime.Today;
            viewModel.DateRangeEndingDate = viewModel.DateRangeStartingDate.AddDays(3);
            DateTime firstItemDate = viewModel.DateRangeStartingDate.AddDays(1);

            List<ReceivedBatch> batches = new List<ReceivedBatch>()
            {
                helper.GetBatchWithSpecificDate(viewModel.DateRangeStartingDate.AddDays(-1)),
                helper.GetBatchWithSpecificDate(firstItemDate),
                helper.GetBatchWithSpecificDate(viewModel.DateRangeStartingDate.AddDays(2)),
                helper.GetBatchWithSpecificDate(viewModel.DateRangeEndingDate.AddDays(1))
            };

            foreach (ReceivedBatch batch in batches)
            {
                receivedBatchSource.SaveReceivedBatch(batch);
            }

            if (command.CanExecute(null)) command.Execute(null);

            Assert.AreEqual(expectedCount, viewModel.RetreivedRecordsLedger.Count);
            Assert.AreEqual(firstItemDate, viewModel.RetreivedRecordsLedger[0].ActivityDate);
        }
    }
}
