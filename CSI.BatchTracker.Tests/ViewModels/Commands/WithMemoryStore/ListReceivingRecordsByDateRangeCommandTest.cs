using CSI.BatchTracker.Domain.DataSource.MemorySource;
using CSI.BatchTracker.Storage.MemoryStore;
using CSI.BatchTracker.Tests.ViewModels.Commands.Behaviors;
using CSI.BatchTracker.ViewModels;
using CSI.BatchTracker.ViewModels.Commands;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Tests.ViewModels.Commands.WithMemoryStore
{
    [TestFixture]
    class ListReceivingRecordsByDateRangeCommandTest : ReceivingHistoryViewModelCommandBehaviorTestingBase
    {
        [SetUp]
        public void SetUp()
        {
            MemoryStoreContext context = new MemoryStoreContext();
            inventorySource = new MemoryActiveInventorySource(context);
            receivedBatchSource = new MemoryReceivedBatchSource(context, inventorySource);
            viewModel = new ReceivingHistoryViewModel(receivedBatchSource, inventorySource);
            command = new ListReceivingRecordsByDateRangeCommand(viewModel);
        }

        [Test]
        public void CommandWillNotExecuteIfDateRangeIsNotSet()
        {
            Assert.False(command.CanExecute(null));
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
    }
}
