using CSI.BatchTracker.Domain.DataSource.SQLiteStore;
using CSI.BatchTracker.Storage.SQLiteStore;
using CSI.BatchTracker.Tests.Domain.DataSource.Behaviors;
using CSI.BatchTracker.Tests.TestHelpers.Storage.SQLiteStore;
using NUnit.Framework;

namespace CSI.BatchTracker.Tests.Storage.SQLiteStore
{
    [TestFixture]
    class SQLiteReceivedBatchStoreTest : ReceivedBatchSourceBehaviorTest
    {
        SQLiteDatabaseHelper databaseHelper;
        SQLiteStoreContext context;

        [SetUp]
        public override void SetUp()
        {
            databaseHelper = new SQLiteDatabaseHelper();
            databaseHelper.CreateTestDatabase();
            context = new SQLiteStoreContext(databaseHelper.DatabaseFile);
            inventorySource = new SQLiteActiveInventorySource(context);
            operatorSource = new SQLiteBatchOperatorSource(context);
            receivedBatchSource = new SQLiteReceivedBatchSource(context, inventorySource);
            base.SetUp();
        }

        [TearDown]
        public void TearDown()
        {
            databaseHelper.DestroyTestDatabase();
        }
    }
}
