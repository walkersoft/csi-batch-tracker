using CSI.BatchTracker.Domain.DataSource.Contracts;
using CSI.BatchTracker.Domain.DataSource.MemorySource;
using CSI.BatchTracker.Storage.MemoryStore;
using CSI.BatchTracker.ViewModels;
using CSI.BatchTracker.ViewModels.Commands;
using NUnit.Framework;
using System.Windows.Input;

namespace CSI.BatchTracker.Tests.ViewModels.Commands.Behaviors
{
    [TestFixture]
    abstract class BatchOperatorListBoxChangedCommandBehaviorTest : BatchOperatorViewModelCommandTestingBase
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
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
