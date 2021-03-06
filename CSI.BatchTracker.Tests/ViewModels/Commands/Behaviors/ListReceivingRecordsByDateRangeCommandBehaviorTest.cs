﻿using CSI.BatchTracker.Domain.NativeModels;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace CSI.BatchTracker.Tests.ViewModels.Commands.Behaviors
{
    [TestFixture]
    abstract class ListReceivingRecordsByDateRangeCommandBehaviorTest : ReceivingHistoryViewModelCommandBehaviorTestingBase
    {
        [SetUp]
        public override void SetUp()
        {
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
            int expectedCount = 1;
            viewModel.DateRangeStartingDate = DateTime.Today;
            viewModel.DateRangeEndingDate = viewModel.DateRangeStartingDate.AddDays(3);
            DateTime firstItemDate = viewModel.DateRangeStartingDate.AddDays(2);

            List<ReceivedBatch> batches = new List<ReceivedBatch>()
            {
                helper.GetBatchWithSpecificDate(viewModel.DateRangeStartingDate.AddDays(-1)),
                helper.GetBatchWithSpecificDate(viewModel.DateRangeStartingDate.AddDays(1)),
                helper.GetBatchWithSpecificDate(firstItemDate),
                helper.GetBatchWithSpecificDate(viewModel.DateRangeEndingDate.AddDays(1))
            };

            foreach (ReceivedBatch batch in batches)
            {
                receivedBatchSource.SaveReceivedBatch(batch);
            }

            command.Execute(null);

            Assert.AreEqual(expectedCount, viewModel.RetreivedRecordsLedger.Count);
            Assert.AreEqual(firstItemDate, viewModel.RetreivedRecordsLedger[0].ActivityDate);
        }

        [Test]
        public void ExecutedCommandWillNotThrowErrorsCriteraIsSetButNoRecordsAreFound()
        {
            viewModel.DateRangeStartingDate = DateTime.Today;
            viewModel.DateRangeEndingDate = viewModel.DateRangeStartingDate.AddDays(1);

            Assert.DoesNotThrow(() => command.Execute(null));
        }
    }
}
