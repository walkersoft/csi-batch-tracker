﻿using CSI.BatchTracker.DataSource.Contracts;
using CSI.BatchTracker.Storage.Contracts;
using CSI.BatchTracker.Storage.MemoryStore;
using CSI.BatchTracker.Domain.DataSource.MemorySource;
using CSI.BatchTracker.Experimental;
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
    class SaveBatchOperatorCommandTest
    {
        ICommand command;
        BatchOperatorViewModel viewModel;

        [SetUp]
        public void SetUp()
        {
            IDataSource dataSource = new MemoryDataSource(new DataStore(), new MemoryStoreContext());
            viewModel = new BatchOperatorViewModel(dataSource);
            command = new SaveBatchOperatorCommand(viewModel);
        }

        [Test]
        public void CommandCanNotExecuteBecauseBatchOperatorFirstNameIsNotPopulated()
        {
            viewModel.LastName = "Doe";
            Assert.False(command.CanExecute(null));
        }

        [Test]
        public void CommandCanNotExecuteBecauseBatchOperatorLastNameIsNotPopulated()
        {
            viewModel.FirstName = "Jane";
            Assert.False(command.CanExecute(null));
        }

        [Test]
        public void CommandCanExecuteWithValidBatchOperatorPopulated()
        {
            viewModel.FirstName = "Jane";
            viewModel.LastName = "Doe";
            Assert.True(command.CanExecute(null));
        }

        [Test]
        public void ExecutedCommandResultsInNewOperatorBeingSaved()
        {
            int expectedCount = 1;
            viewModel.FirstName = "Jane";
            viewModel.LastName = "Doe";
            command.Execute(null);
            Assert.AreEqual(expectedCount, viewModel.OperatorRepository.Count);
        }

        [Test]
        public void ExecutedCommandWithExistingBatchOperatorResultsInOperatorBeingUpdated()
        {
            int expectedCount = 1;
            string expectedFirstName = "John";
            string expectedLastName = "Roe";

            viewModel.FirstName = "Jane";
            viewModel.LastName = "Doe";
            command.Execute(null);
            Assert.AreEqual(expectedCount, viewModel.OperatorRepository.Count);

            viewModel.SelectedBatchOperatorFromComboBoxIndex = 1;
            viewModel.FirstName = expectedFirstName;
            viewModel.LastName = expectedLastName;
            command.Execute(null);
            Assert.AreEqual(expectedCount, viewModel.OperatorRepository.Count);
            Assert.AreEqual(expectedFirstName, viewModel.OperatorRepository[0].FirstName);
            Assert.AreEqual(expectedLastName, viewModel.OperatorRepository[0].LastName);
        }
    }
}
