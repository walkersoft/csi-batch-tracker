using CSI.BatchTracker.Domain.DataSource.SQLiteStore;
using CSI.BatchTracker.Storage.SQLiteStore;
using CSI.BatchTracker.Tests.TestHelpers.Storage.SQLiteStore;
using CSI.BatchTracker.ViewModels;
using CSI.BatchTracker.ViewModels.Commands;
using NUnit.Framework;

namespace CSI.BatchTracker.Tests.ViewModels.Commands.WithSQLiteStore
{
    [TestFixture]
    class OpenConnectedBatchInquiryViewCommandTest : OpenConnectedBatchInquiryViewCommandBehaviorTest
    {
        SQLiteDatabaseHelper sqliteHelper;

        [SetUp]
        public override void SetUp()
        {
            sqliteHelper = new SQLiteDatabaseHelper();
            sqliteHelper.CreateTestDatabase();
            SQLiteStoreContext context = new SQLiteStoreContext(sqliteHelper.DatabaseFile);
            inventorySource = new SQLiteActiveInventorySource(context);
            receivedBatchSource = new SQLiteReceivedBatchSource(context, inventorySource);
            implementedBatchSource = new SQLiteImplementedBatchSource(context, inventorySource);
            operatorSource = new SQLiteBatchOperatorSource(context);
            viewModel = new MainWindowViewModel(inventorySource, receivedBatchSource, implementedBatchSource, operatorSource);
            command = new OpenConnectedBatchInquiryViewCommand(viewModel);
            base.SetUp();
        }

        [TearDown]
        public void TearDown()
        {
            sqliteHelper.DestroyTestDatabase();
        }
    }
}
