using CSI.BatchTracker.Domain.Contracts;
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
    abstract class DisplayBatchHistoryFromBatchNumberCommandBehaviorTest
    {
        protected BatchHistoryViewModel viewModel;
        protected ICommand command;
        protected IBatchNumberValidator validator;
        protected IActiveInventorySource inventorySource;
        protected IReceivedBatchSource receivedBatchSource;
        protected IImplementedBatchSource implementedBatchSource;
        ReceivedBatchTestHelper receivedBatchHelper;
        BatchOperatorTestHelper batchOperatorHelper;
        string validBatchNumber;
        string invalidBatchNumber;

        [SetUp]
        public virtual void SetUp()
        {
            validBatchNumber = "872890105803";
            invalidBatchNumber = "foobar";
            receivedBatchHelper = new ReceivedBatchTestHelper();
            batchOperatorHelper = new BatchOperatorTestHelper();
        }

        [Test]
        public void CommandWillExecuteIfBatchNumberInViewModelIsValid()
        {
            viewModel.BatchNumber = validBatchNumber;
            Assert.True(command.CanExecute(null));
        }

        [Test]
        public void CommandWillFailIfBatchNumberInViewModelIsInvalid()
        {
            viewModel.BatchNumber = invalidBatchNumber;
            Assert.False(command.CanExecute(null));
        }

        [Test]
        public void CommandWillFailIfBatchNumberInViewModelIsEmpty()
        {
            viewModel.BatchNumber = string.Empty;
            Assert.False(command.CanExecute(null));
        }

        [Test]
        public void ExecutedCommandPopulatesGrids()
        {
            int expectedInventoryQuantity = 1;
            int expectedReceivingHistoryCount = 2;
            int expectedImplementationHistoryCount = 4;

            viewModel.BatchNumber = SetupValidDataSourceStateForProperPopulationOfHistoryGridsAndReturnTargetBatchNumber();
            command.Execute(null);

            Assert.AreEqual(expectedInventoryQuantity, int.Parse(viewModel.AmountInInventory));
            Assert.AreEqual(expectedReceivingHistoryCount, viewModel.ReceivingHistoryGrid.Count);
            Assert.AreEqual(expectedImplementationHistoryCount, viewModel.ImplementationHistoryGrid.Count);
        }

        string SetupValidDataSourceStateForProperPopulationOfHistoryGridsAndReturnTargetBatchNumber()
        {
            ReceivedBatch receivedBatch = receivedBatchHelper.GetBatchWithSpecificDate(DateTime.Today);
            ReceiveFiveBatchesOfTheSameBatchIntoInventory(receivedBatch);
            ImplementAllButOneBatch(receivedBatch.BatchNumber);

            return receivedBatch.BatchNumber;
        }

        void ReceiveFiveBatchesOfTheSameBatchIntoInventory(ReceivedBatch receivedBatch)
        {
            receivedBatch.Quantity = 3;
            receivedBatchSource.SaveReceivedBatch(receivedBatch);

            receivedBatch.Quantity = 2;
            receivedBatch.ActivityDate = receivedBatch.ActivityDate.AddDays(7);
            receivedBatchSource.SaveReceivedBatch(receivedBatch);
        }

        void ImplementAllButOneBatch(string batchNumber)
        {
            BatchOperator batchOperator = batchOperatorHelper.GetJaneDoeOperator();
            DateTime date = DateTime.Today;

            for (int i = 0; i < 4; i++)
            {
                implementedBatchSource.AddBatchToImplementationLedger(batchNumber, date, batchOperator);
                date = date.AddDays(i);
            }
        }
    }
}
