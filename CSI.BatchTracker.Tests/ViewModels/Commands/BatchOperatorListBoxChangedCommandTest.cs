using CSI.BatchTracker.DataSource.Contracts;
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
    class BatchOperatorListBoxChangedCommandTest
    {
        ICommand command;
        BatchOperatorViewModel viewModel;

        [SetUp]
        public void SetUp()
        {
            MemoryStoreContext store = new MemoryStoreContext();
            IDataSource dataSource = new DataSourceRepository(new DataStore(), store);
            viewModel = new BatchOperatorViewModel(dataSource);
            command = new BatchOperatorListBoxChangedCommand(viewModel);
        }

        [Test]
        public void CommandCanNotExecuteIfListBoxIndexIsLessThanZero()
        {
            viewModel.SelectedBatchOperatorFromListBoxIndex = -1;
            Assert.False(command.CanExecute(null));
        }

        [Test]
        public void CommandCanExecuteIfListBoxIndexIsGreaterThanNegativeOne()
        {
            viewModel.SelectedBatchOperatorFromListBoxIndex = 0;
            Assert.True(command.CanExecute(null));
        }

        [Test]
        public void ComboBoxIndexIsOneMoreThanListBoxIndexWhenListBoxIndexIsGreaterThanNegativeOne()
        {
            int expectedIndexValue = 1;
            PutSingleBatchOperatorIntoDataSource();
            viewModel.SelectedBatchOperatorFromListBoxIndex = 0;
            command.Execute(null);

            Assert.AreEqual(expectedIndexValue, viewModel.SelectedBatchOperatorFromComboBoxIndex);
        }

        void PutSingleBatchOperatorIntoDataSource()
        {
            viewModel.FirstName = "Jane";
            viewModel.LastName = "Doe";
            viewModel.PersistBatchOperator();
        }
    }
}
