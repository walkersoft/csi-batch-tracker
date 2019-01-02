using CSI.BatchTracker.Storage.MemoryStore;
using CSI.BatchTracker.Domain.DataSource.Repositories;
using CSI.BatchTracker.Experimental;
using CSI.BatchTracker.ViewModels;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Tests.ViewModels
{
    [TestFixture]
    class BatchOperatorViewModelTest
    {
        BatchOperatorViewModel viewModel;

        [SetUp]
        public void SetUp()
        {
            viewModel = new BatchOperatorViewModel(new DataSourceRepository(new DataStore(), new MemoryStore()));
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
