using CSI.BatchTracker.Domain;
using CSI.BatchTracker.Domain.DataSource.Contracts;
using CSI.BatchTracker.Domain.DataSource.SQLiteStore;
using CSI.BatchTracker.Storage.SQLiteStore;
using CSI.BatchTracker.Tests.TestHelpers.Storage.SQLiteStore;
using CSI.BatchTracker.ViewModels;
using NUnit.Framework;

namespace CSI.BatchTracker.Tests.ViewModels.WithSQLiteStore
{
    [TestFixture]
    class ReceivingManagementViewModelTest
    {
        ReceivingManagementViewModel viewModel;
        SQLiteDatabaseHelper helper;

        [SetUp]
        public void SetUp()
        {
            helper = new SQLiteDatabaseHelper();
            helper.CreateTestDatabase();
            SQLiteStoreContext context = new SQLiteStoreContext(helper.DatabaseFile);
            IActiveInventorySource inventorySource = new SQLiteActiveInventorySource(context);
            viewModel = new ReceivingManagementViewModel(
                new DuracolorIntermixBatchNumberValidator(),
                new DuracolorIntermixColorList(),
                new SQLiteReceivedBatchSource(context, inventorySource),
                new SQLiteBatchOperatorSource(context),
                inventorySource
            );
        }

        [TearDown]
        public void TearDown()
        {
            helper.DestroyTestDatabase();
        }

        [Test]
        public void AttemptingToAddInvalidReceivedBatchToSessionLedgerResultsInNoChanges()
        {
            int expectedLedgerCount = 0;
            viewModel.AddReceivedBatchToSessionLedger();

            Assert.AreEqual(expectedLedgerCount, viewModel.SessionLedger.Count);
        }

        [Test]
        public void ColorListIsInitializedUponViewModelContruction()
        {
            Assert.NotNull(viewModel.Colors);
        }
    }
}
