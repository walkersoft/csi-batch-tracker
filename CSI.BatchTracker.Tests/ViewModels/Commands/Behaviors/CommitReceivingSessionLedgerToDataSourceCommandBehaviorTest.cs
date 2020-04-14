using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.ViewModels;
using CSI.BatchTracker.ViewModels.Commands;
using NUnit.Framework;
using System;

namespace CSI.BatchTracker.Tests.ViewModels.Commands.Behaviors
{
    [TestFixture]
    abstract class CommitReceivingSessionLedgerToDataSourceCommandBehaviorTest : ReceivingManagementViewModelCommandTestingBase
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            viewModel = new ReceivingManagementViewModel(validator, colorList, receivingSource, operatorSource, inventorySource);
            command = new CommitReceivingSessionLedgerToDataSourceCommand(viewModel);
        }

        [Test]
        public void UnableToCommitSessionLedgerIfNoEntriesArePresent()
        {
            Assert.False(command.CanExecute(null));
        }

        [Test]
        public void SessionLedgerIsReadyToBeCommitted()
        {
            SetupValidReceivedBatchInViewModel();
            InjectTwoOperatorsIntoRepository();
            AddReceivedBatchToSessionLedger();

            Assert.True(command.CanExecute(null));
        }

        [Test]
        public void SuccessfullyCommitSessionLedgerWithOneItem()
        {
            int expectedCount = 1;
            int colorListSelectedIndex = 0;
            SetupValidReceivedBatchInViewModel();
            InjectTwoOperatorsIntoRepository();
            AddReceivedBatchToSessionLedger();

            command.Execute(null);

            Assert.AreEqual(expectedCount, viewModel.ReceivedBatchRepository.Count);
            Assert.AreEqual(colorListSelectedIndex, viewModel.ColorSelectionComboBoxIndex);
        }

        [Test]
        public void ReceivingDataIsClearedAfterCommitingSessionLedgerToDataSource()
        {
            DateTime expectedDate = DateTime.Today;
            int expectedOperatorComboBoxIndex = -1;
            int expectedColorComboBoxIndex = 0;
            int expectedSessionLedgerCount = 0;

            SetupValidReceivedBatchInViewModel();
            InjectTwoOperatorsIntoRepository();
            AddReceivedBatchToSessionLedger();

            command.Execute(null);

            Assert.True(string.IsNullOrEmpty(viewModel.PONumber));
            Assert.True(string.IsNullOrEmpty(viewModel.BatchNumber));
            Assert.True(string.IsNullOrEmpty(viewModel.Quantity));
            Assert.AreEqual(expectedDate, viewModel.ReceivingDate);
            Assert.AreEqual(expectedOperatorComboBoxIndex, viewModel.ReceivingOperatorComboBoxIndex);
            Assert.AreEqual(expectedColorComboBoxIndex, viewModel.ColorSelectionComboBoxIndex);
            Assert.AreEqual(expectedSessionLedgerCount, viewModel.SessionLedger.Count);
        }

        [Test]
        public void SelectedOperatorUponCommitIsAssignedToAllReceivedBatchesInSessionLedger()
        {
            int expectedStartingSessionLedgerCount = 1;
            int expectedDataSourceCountAfterCommitting = 2;
            InjectTwoOperatorsIntoRepository();
            BatchOperator originalOperator = operatorSource.FindBatchOperator(1);
            BatchOperator expectedOperator = operatorSource.FindBatchOperator(2);

            SetupValidReceivedBatchInViewModel();
            AddReceivedBatchToSessionLedger();

            Assert.AreEqual(expectedStartingSessionLedgerCount, viewModel.SessionLedger.Count);

            foreach (ReceivedBatch batch in viewModel.SessionLedger)
            {
                Assert.AreEqual(originalOperator.FullName, batch.ReceivingOperator.FullName);
            }

            SetupValidReceivedBatchInViewModel();
            viewModel.ReceivingOperatorComboBoxIndex = 1;
            AddReceivedBatchToSessionLedger();

            command.Execute(null);

            Assert.AreEqual(expectedDataSourceCountAfterCommitting, receivingSource.ReceivedBatchRepository.Count);

            foreach (ReceivedBatch batch in receivingSource.ReceivedBatchRepository)
            {
                Assert.AreEqual(expectedOperator.FullName, batch.ReceivingOperator.FullName);
            }
        }

        [Test]
        public void AddingRecordWithBatchNumberThatIsSimilarToAnotherBatchOfDifferentColorWillNotCommitToReceivingLedger()
        {
            int expectedLedgerCount = 1;
            InjectTwoOperatorsIntoRepository();
            SetupValidReceivedBatchInViewModel();

            ReceivedBatch receivedBatch = new ReceivedBatch(
                "White",
                viewModel.BatchNumber,
                DateTime.Now,
                1,
                55555,
                operatorSource.FindBatchOperator(1)
            );

            receivingSource.SaveReceivedBatch(receivedBatch);

            viewModel.ColorSelectionComboBoxIndex = 2;
            viewModel.BatchNumber = "872880501302";
            viewModel.Quantity = "1";
            AddReceivedBatchToSessionLedger();
            command.Execute(null);

            Assert.AreEqual(expectedLedgerCount, receivingSource.ReceivedBatchRepository.Count);
        }
    }
}
