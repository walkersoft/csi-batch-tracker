using CSI.BatchTracker.Domain;
using CSI.BatchTracker.Domain.Contracts;
using CSI.BatchTracker.Domain.DataSource.Contracts;
using CSI.BatchTracker.Domain.DataSource.MemorySource;
using CSI.BatchTracker.Storage.MemoryStore;
using CSI.BatchTracker.Tests.TestHelpers.NativeModels;
using CSI.BatchTracker.ViewModels;
using CSI.BatchTracker.ViewModels.Commands;
using NUnit.Framework;
using System;
using System.Windows.Input;

namespace CSI.BatchTracker.Tests.ViewModels.Commands
{
    [TestFixture]
    class AddReceivedBatchToReceivingSessionLedgerCommandTest : ReceivingManagementViewModelCommandTestingBase
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            operatorHelper = new BatchOperatorTestHelper();
            viewModel = new ReceivingManagementViewModel(validator, colorList, receivingSource, operatorSource);
            command = new AddReceivedBatchToReceivingSessionLedgerCommand(viewModel);
        }

        [Test]
        public void ReceivedBatchIsReadyToBeAddedToTheSessionLedger()
        {
            SetupValidReceivedBatchInViewModel();
            Assert.True(command.CanExecute(null));
        }

        [Test]
        public void UnableToAddReceivedBatchToSessionLedgerIfPoNumberIsNotValid()
        {
            SetupValidReceivedBatchInViewModel();
            viewModel.PONumber = "";

            Assert.False(command.CanExecute(null));
        }

        [Test]
        public void UnableToAddReceivedBatchToSessionLedgerIfPoNumberIsNotAParseableInteger()
        {
            SetupValidReceivedBatchInViewModel();
            viewModel.PONumber = "foo";

            Assert.False(command.CanExecute(null));
        }

        [Test]
        public void UnableToAddReceivedBatchToSessionLedgerIfReceivingDateIsNotSelected()
        {
            SetupValidReceivedBatchInViewModel();
            viewModel.ReceivingDate = DateTime.MinValue;

            Assert.False(command.CanExecute(null));
        }

        [Test]
        public void UnableToAddReceivedBatchToSessionLedgerIfReceivingOperatorIsNotSelected()
        {
            SetupValidReceivedBatchInViewModel();
            viewModel.ReceivingOperatorComboBoxIndex = -1;

            Assert.False(command.CanExecute(null));
        }

        [Test]
        public void UnableToAddReceivedBatchToSessionLedgerIfColorIsNotSelected()
        {
            SetupValidReceivedBatchInViewModel();
            viewModel.ColorSelectionComboBoxIndex = -1;

            Assert.False(command.CanExecute(null));
        }

        [Test]
        public void UnableToAddReceivedBatchToSessionLedgerIfBatchNumberIsNotValid()
        {
            SetupValidReceivedBatchInViewModel();
            viewModel.BatchNumber = string.Empty;

            Assert.False(command.CanExecute(null));
        }

        [Test]
        public void UnableToAddReceivedBatchToSessionLedgerIfQuantityIsLessThanOne()
        {
            SetupValidReceivedBatchInViewModel();
            viewModel.Quantity = "0";

            Assert.False(command.CanExecute(null));
        }

        [Test]
        public void UnableToAddReceivedBatchToSessionLedgerIfQuantityIsNotValid()
        {
            SetupValidReceivedBatchInViewModel();
            viewModel.Quantity = string.Empty;

            Assert.False(command.CanExecute(null));
        }

        [Test]
        public void UnableToAddReceivedBatchToSessionLedgerIfQuantityIsNotParseableInteger()
        {
            SetupValidReceivedBatchInViewModel();
            viewModel.Quantity = "foo";

            Assert.False(command.CanExecute(null));
        }

        [Test]
        public void ValidReceivedBatchWillBeAddedToSessionLedgerWhenCommandExecutes()
        {
            int expectedCount = 1;
            SetupValidReceivedBatchInViewModel();
            InjectTwoOperatorsIntoRepository();
            command.Execute(null);

            Assert.AreEqual(expectedCount, viewModel.SessionLedger.Count);
        }

        [Test]
        public void LineItemDataIsResetAfterAddingBatchToSessionLedger()
        {
            int expectedColorIndex = -1;
            string expectedBatchNumber = string.Empty;
            string expectedQuantity = string.Empty;

            SetupValidReceivedBatchInViewModel();
            InjectTwoOperatorsIntoRepository();
            viewModel.ColorSelectionComboBoxIndex = 1;
            command.Execute(null);

            Assert.AreEqual(expectedColorIndex, viewModel.ColorSelectionComboBoxIndex);
            Assert.AreEqual(expectedBatchNumber, viewModel.BatchNumber);
            Assert.AreEqual(expectedQuantity, viewModel.Quantity);
        }

        [Test]
        public void HeaderDataRemainsTheSameAfterAddingBatchToSessionLedger()
        {
            SetupValidReceivedBatchInViewModel();
            InjectTwoOperatorsIntoRepository();
            string expectedPoNumber = viewModel.PONumber;
            DateTime expectedDate = viewModel.ReceivingDate;
            int expectedOperatorIndex = viewModel.ReceivingOperatorComboBoxIndex;

            command.Execute(null);

            Assert.AreEqual(expectedPoNumber, viewModel.PONumber);
            Assert.AreEqual(expectedDate, viewModel.ReceivingDate);
            Assert.AreEqual(expectedOperatorIndex, viewModel.ReceivingOperatorComboBoxIndex);
        }
    }
}
