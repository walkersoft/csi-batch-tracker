using CSI.BatchTracker.ViewModels;
using CSI.BatchTracker.ViewModels.Commands;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CSI.BatchTracker.Tests.ViewModels.Commands
{
    [TestFixture]
    class CommitReceivingSessionLedgerToDataSourceCommandTest : ReceivingManagementViewModelCommandTestingBase
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            viewModel = new ReceivingManagementViewModel(validator, colorList, receivingSource, operatorSource);
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
            SetupValidReceivedBatchInViewModel();
            InjectTwoOperatorsIntoRepository();
            AddReceivedBatchToSessionLedger();

            command.Execute(null);

            Assert.AreEqual(expectedCount, viewModel.ReceivedBatchRepository.Count);
        }

        [Test]
        public void ReceivingDataIsClearedAfterCommitingSessionLedgerToDataSource()
        {
            DateTime expectedDate = DateTime.MinValue;
            int expectedOperatorComboBoxIndex = -1;
            int expectedColorComboBoxIndex = -1;

            SetupValidReceivedBatchInViewModel();
            InjectTwoOperatorsIntoRepository();
            AddReceivedBatchToSessionLedger();

            command.Execute(null);

            Assert.True(string.IsNullOrEmpty(viewModel.PONumber));
            Assert.True(string.IsNullOrEmpty(viewModel.BatchNumber));
            Assert.True(string.IsNullOrEmpty(viewModel.Quantity));
            Assert.AreEqual(DateTime.MinValue, viewModel.ReceivingDate);
            Assert.AreEqual(expectedOperatorComboBoxIndex, viewModel.ReceivingOperatorComboBoxIndex);
            Assert.AreEqual(expectedColorComboBoxIndex, viewModel.ColorSelectionComboBoxIndex);
        }
    }
}
