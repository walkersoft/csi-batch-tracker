using CSI.BatchTracker.Domain.DataSource.Contracts;
using CSI.BatchTracker.Domain.DataSource.MemorySource;
using CSI.BatchTracker.Storage.MemoryStore;
using CSI.BatchTracker.ViewModels;
using CSI.BatchTracker.ViewModels.Commands;
using NUnit.Framework;
using System.Windows.Input;

namespace CSI.BatchTracker.Tests.ViewModels.Commands
{
    [TestFixture]
    class BatchOperatorComboBoxChangedCommandTest
    {
        ICommand command;
        BatchOperatorViewModel viewModel;
        
        [SetUp]
        public void SetUp()
        {
            MemoryStoreContext store = new MemoryStoreContext();
            IBatchOperatorSource operatorSource = new MemoryBatchOperatorSource(store);
            viewModel = new BatchOperatorViewModel(operatorSource);
            command = new BatchOperatorComboBoxChangedCommand(viewModel);
        }

        [Test]
        public void CommandCanExecuteIfComboBoxIndexIsZero()
        {
            viewModel.SelectedBatchOperatorFromComboBoxIndex = 0;
            Assert.True(command.CanExecute(null));
        }

        [Test]
        public void CommandCanExecuteIfComboBoxIndexIsGreaterThanZero()
        {
            viewModel.SelectedBatchOperatorFromComboBoxIndex = 1;
            Assert.True(command.CanExecute(null));
        }

        [Test]
        public void CommandCanNotExecuteIfComboBoxIndexIsLessThanZero()
        {
            viewModel.SelectedBatchOperatorFromComboBoxIndex = -1;
            Assert.False(command.CanExecute(null));
        }

        [Test]
        public void ActiveBatchOperatorGetsResetIfComboBoxIndexIsZero()
        {
            viewModel.FirstName = "Jane";
            viewModel.LastName = "Doe";
            viewModel.SelectedBatchOperatorFromComboBoxIndex = 0;
            command.Execute(null);

            Assert.IsEmpty(viewModel.FirstName);
            Assert.IsEmpty(viewModel.LastName);
        }

        [Test]
        public void ActiveBatchOperatorGetUpdatedWhenComboBoxIsChangedToExistingBatchOperator()
        {
            string expectedFirstName = "Jane";
            string expectedLastName = "Doe";
            PutSingleBatchOperatorIntoDataSource();
            viewModel.FirstName = "John";
            viewModel.LastName = "Doe";

            viewModel.SelectedBatchOperatorFromComboBoxIndex = 1;
            command.Execute(null);

            Assert.AreEqual(expectedFirstName, viewModel.FirstName);
            Assert.AreEqual(expectedLastName, viewModel.LastName);
        }

        void PutSingleBatchOperatorIntoDataSource()
        {
            viewModel.FirstName = "Jane";
            viewModel.LastName = "Doe";
            viewModel.PersistBatchOperator();
        }
    }
}
