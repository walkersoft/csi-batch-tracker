using CSI.BatchTracker.DataSource.Contracts;
using CSI.BatchTracker.Domain.DataSource.MemorySource;
using CSI.BatchTracker.Experimental;
using CSI.BatchTracker.Storage.MemoryStore;
using CSI.BatchTracker.ViewModels;
using CSI.BatchTracker.ViewModels.Commands;
using NUnit.Framework;
using System.Windows.Input;

namespace CSI.BatchTracker.Tests.ViewModels.Commands
{
    [TestFixture]
    class DeleteSelectedBatchOperatorCommandTest
    {
        ICommand command;
        BatchOperatorViewModel viewModel;
        MemoryStoreContext store;

        [SetUp]
        public void SetUp()
        {
            store = new MemoryStoreContext();
            IDataSource dataSource = new MemoryDataSource(new DataStore(), store);
            viewModel = new BatchOperatorViewModel(dataSource);
            command = new DeleteSelectedBatchOperatorCommand(viewModel);
        }

        [Test]
        public void CommandCanExecute()
        {
            PutSingleBatchOperatorIntoDataSource();
            viewModel.SelectedBatchOperatorFromListBoxIndex = 0;
            Assert.True(command.CanExecute(null));
        }

        //[Test]
        public void CommandCanNotExecute()
        {
            //Assert.False(command.CanExecute(null));
            /* TODO: once more of the persistence layer is completed and wired
             * up this test needs to be re-activated so the command can be properly
             * tested. Specifically, a BatchOperator cannot be deleted if it is
             * referenced elsewhere in the data source as the record will then
             * become orphaned to the parent.
             */
        }

        [Test]
        public void ExistingBatchOperatorGetsDeleted()
        {
            int expectedCount = 0;
            PutSingleBatchOperatorIntoDataSource();
            viewModel.SelectedBatchOperatorFromListBoxIndex = 0;
            command.Execute(null);
            Assert.AreEqual(expectedCount, viewModel.OperatorRepository.Count);
        }

        void PutSingleBatchOperatorIntoDataSource()
        {
            viewModel.FirstName = "Jane";
            viewModel.LastName = "Doe";
            viewModel.PersistBatchOperator();
        }
    }
}
