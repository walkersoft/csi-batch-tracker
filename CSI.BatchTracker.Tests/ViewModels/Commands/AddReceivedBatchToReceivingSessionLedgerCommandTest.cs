﻿using CSI.BatchTracker.Domain;
using CSI.BatchTracker.Domain.Contracts;
using CSI.BatchTracker.ViewModels;
using CSI.BatchTracker.ViewModels.Commands;
using NUnit.Framework;
using System;
using System.Windows.Input;

namespace CSI.BatchTracker.Tests.ViewModels.Commands
{
    [TestFixture]
    class AddReceivedBatchToReceivingSessionLedgerCommandTest
    {
        ICommand command;
        ReceivingManagementViewModel viewModel;

        [SetUp]
        public void SetUp()
        {
            IBatchNumberValidator validator = new DuracolorIntermixBatchNumberValidator();
            viewModel = new ReceivingManagementViewModel(validator);
            command = new AddReceivedBatchToReceivingSessionLedgerCommand(viewModel);
        }

        [Test]
        public void ReceivedBatchIsReadyToBeAddedToTheSessionLedger()
        {
            SetupValidReceivedBatchInViewModel();
            Assert.True(command.CanExecute(null));
        }

        void SetupValidReceivedBatchInViewModel()
        {
            viewModel.PONumber = "11111";
            viewModel.ReceivingDate = DateTime.Now;
            viewModel.ReceivingOperatorComboBoxIndex = 0;
            viewModel.ColorSelectionComboBoxIndex = 0;
            viewModel.BatchNumber = "872880501302";
            viewModel.Quantity = "1";
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
            viewModel.BatchNumber = "";

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
            viewModel.Quantity = "";

            Assert.False(command.CanExecute(null));
        }

        [Test]
        public void UnableToAddReceivedBatchToSessionLedgerIfQuantityIsNotParseableInteger()
        {
            SetupValidReceivedBatchInViewModel();
            viewModel.Quantity = "foo";

            Assert.False(command.CanExecute(null));
        }
    }
}
