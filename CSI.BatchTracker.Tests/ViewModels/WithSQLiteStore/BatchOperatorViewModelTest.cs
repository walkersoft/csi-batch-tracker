using CSI.BatchTracker.Domain.DataSource.SQLiteStore;
using CSI.BatchTracker.Storage.SQLiteStore;
using CSI.BatchTracker.Tests.TestHelpers.Storage.SQLiteStore;
using CSI.BatchTracker.ViewModels;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Tests.ViewModels.WithSQLiteStore
{
    [TestFixture]
    class BatchOperatorViewModelTest
    {
        BatchOperatorViewModel viewModel;
        SQLiteDatabaseHelper helper;
        SQLiteStoreContext context;

        [SetUp]
        public void SetUp()
        {
            helper = new SQLiteDatabaseHelper();
            helper.CreateTestDatabase();
            context = new SQLiteStoreContext(helper.DatabaseFile);
            viewModel = new BatchOperatorViewModel(new SQLiteBatchOperatorSource(context));
        }

        [TearDown]
        public void TearDown()
        {
            helper.DestroyTestDatabase();
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
