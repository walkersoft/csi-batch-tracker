using CSI.BatchTracker.Domain.DataSource.Contracts;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.Tests.TestHelpers.NativeModels;
using CSI.BatchTracker.ViewModels;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CSI.BatchTracker.Tests.ViewModels.Commands.Behaviors
{
    [TestFixture]
    abstract class ImplementationInquiryViewModelTestingBase 
    {
        protected ICommand command;
        protected IBatchOperatorSource operatorSource;
        protected IActiveInventorySource inventorySource;
        protected IReceivedBatchSource receivedBatchSource;
        protected IImplementedBatchSource implementedBatchSource;
        protected ImplementationInquiryViewModel viewModel;

        BatchOperator batchOperator;
        BatchOperatorTestHelper operatorHelper;
        DateTime todaysDate;

        [SetUp]
        public virtual void SetUp()
        {
            operatorHelper = new BatchOperatorTestHelper(operatorSource);
            ReceiveAndImplementOneBatchOfEachColor();
        }

        void ReceiveAndImplementOneBatchOfEachColor()
        {
            todaysDate = DateTime.Today;
            batchOperator = operatorHelper.GetJaneDoeOperator();

            List<ReceivedBatch> received = new List<ReceivedBatch>()
            {
                new ReceivedBatch("White", "872895011111", todaysDate, 1, 11111, batchOperator),
                new ReceivedBatch("Black", "872895022222", todaysDate, 1, 11111, batchOperator),
                new ReceivedBatch("Yellow", "872895033333", todaysDate, 1, 11111, batchOperator),
                new ReceivedBatch("Red", "872895044444", todaysDate, 1, 11111, batchOperator),
                new ReceivedBatch("Blue Red", "872895055555", todaysDate, 1, 11111, batchOperator),
                new ReceivedBatch("Deep Green", "872895066666", todaysDate, 1, 11111, batchOperator),
                new ReceivedBatch("Deep Blue", "872895077777", todaysDate, 1, 11111, batchOperator),
                new ReceivedBatch("Bright Red", "872895088888", todaysDate, 1, 11111, batchOperator),
                new ReceivedBatch("Bright Yellow", "872895099999", todaysDate, 1, 11111, batchOperator),
            };

            foreach (ReceivedBatch batch in received)
            {
                receivedBatchSource.SaveReceivedBatch(batch);
                implementedBatchSource.AddBatchToImplementationLedger(batch.BatchNumber, DateTime.Now, batchOperator);
            }
        }

        [Test]
        public void CommandWillNotExecuteIfDateNotSelected()
        {
            viewModel.SelectedDate = DateTime.MinValue;
            Assert.False(command.CanExecute(null));
        }

        [Test]
        public void CommandWillExecuteAsLongAsNonMinimumDateIsSelected()
        {
            viewModel.SelectedDate = DateTime.Now;
            Assert.True(command.CanExecute(null));
        }

        [Test]
        public void CollectionWillPopulateFromOneOfEachImplementedBatch()
        {
            int expectedCount = 9;
            viewModel.SelectedDate = todaysDate;
            command.Execute(null);

            Assert.AreEqual(expectedCount, viewModel.ImplementedBatches.Count);
        }

        [Test]
        public void MultipleBatchesFromTheSameDateWillReturnAllUniqueConnections()
        {
            int expectedCount = 10;
            viewModel.SelectedDate = todaysDate;
            receivedBatchSource.SaveReceivedBatch(new ReceivedBatch("Black", "872895021212", todaysDate, 1, 11111, batchOperator));
            implementedBatchSource.AddBatchToImplementationLedger("872895021212", todaysDate, batchOperator);

            command.Execute(null);

            Assert.AreEqual(expectedCount, viewModel.ImplementedBatches.Count);
        }

        [Test]
        public void CommonBatchesFromTheSameDateWillOnlyReturnOneConnection()
        {
            int expectedCount = 9;
            viewModel.SelectedDate = todaysDate;

            receivedBatchSource.SaveReceivedBatch(new ReceivedBatch("Black", "872895022222", todaysDate, 1, 11111, batchOperator));
            implementedBatchSource.AddBatchToImplementationLedger("872895022222", todaysDate, batchOperator);

            command.Execute(null);

            Assert.AreEqual(expectedCount, viewModel.ImplementedBatches.Count);
        }

        [Test]
        public void DifferentBatchesFromDifferentDateWillBeOmitted()
        {
            int expectedCount = 9;
            viewModel.SelectedDate = todaysDate;
            receivedBatchSource.SaveReceivedBatch(new ReceivedBatch("Black", "872895021212", todaysDate.AddDays(-1), 1, 11111, batchOperator));
            implementedBatchSource.AddBatchToImplementationLedger("872895021212", todaysDate.AddDays(-1), batchOperator);

            command.Execute(null);

            Assert.AreEqual(expectedCount, viewModel.ImplementedBatches.Count);
        }

        [Test]
        public void BatchesAreCapturedIfTargetDateContainsNoConnections()
        {
            int expectedCount = 9;
            viewModel.SelectedDate = todaysDate.AddDays(1);
            command.Execute(null);

            Assert.AreEqual(expectedCount, viewModel.ImplementedBatches.Count);
        }

        [Test]
        public void MultiplesOfCommonBatchesAreCapturedIfTheyShareADateWithTheLatestBatchFromTargetDate()
        {
            int expectedCount = 10;
            viewModel.SelectedDate = todaysDate.AddDays(1);
            receivedBatchSource.SaveReceivedBatch(new ReceivedBatch("Black", "872895021212", todaysDate, 1, 11111, batchOperator));
            implementedBatchSource.AddBatchToImplementationLedger("872895021212", todaysDate, batchOperator);

            command.Execute(null);

            Assert.AreEqual(expectedCount, viewModel.ImplementedBatches.Count);
        }
    }
}
