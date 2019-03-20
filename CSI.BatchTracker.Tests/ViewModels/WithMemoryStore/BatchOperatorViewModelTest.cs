using CSI.BatchTracker.Domain.DataSource.MemorySource;
using CSI.BatchTracker.Storage.MemoryStore;
using CSI.BatchTracker.ViewModels;
using NUnit.Framework;

namespace CSI.BatchTracker.Tests.ViewModels.WithMemoryStore
{
    [TestFixture]
    class BatchOperatorViewModelTest
    {
        BatchOperatorViewModel viewModel;

        [SetUp]
        public void SetUp()
        {
            viewModel = new BatchOperatorViewModel(new MemoryBatchOperatorSource(new MemoryStoreContext()));
        }
        [Test]
        public void BatchOperatorNamesForComboBoxHasAtLeastOneItem()
        {
            int expectedCount = 1;
            Assert.AreEqual(expectedCount, viewModel.OperatorNames.Count);
        }

        [Test]
        public void BatchOperatorNamesForComboBoxIsOneGreaterThanRepositoryCount()
        {
            int firstExpectedCount = 2;
            int secondExpectedCount = 5;

            PutSingleBatchOperatorIntoDataSource();
            Assert.AreEqual(firstExpectedCount, viewModel.OperatorNames.Count);

            PutSingleBatchOperatorIntoDataSource();
            PutSingleBatchOperatorIntoDataSource();
            PutSingleBatchOperatorIntoDataSource();
            Assert.AreEqual(secondExpectedCount, viewModel.OperatorNames.Count);
        }

        void PutSingleBatchOperatorIntoDataSource()
        {
            viewModel.FirstName = "Jane";
            viewModel.LastName = "Doe";
            viewModel.PersistBatchOperator();
        }
    }
}
