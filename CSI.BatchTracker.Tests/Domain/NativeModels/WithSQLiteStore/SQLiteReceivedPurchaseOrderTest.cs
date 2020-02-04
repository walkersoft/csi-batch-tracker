using CSI.BatchTracker.Domain.DataSource.SQLiteStore;
using CSI.BatchTracker.Storage.SQLiteStore;
using CSI.BatchTracker.Tests.TestHelpers.Storage.SQLiteStore;
using NUnit.Framework;

namespace CSI.BatchTracker.Tests.Domain.NativeModels.WithSQLiteStore
{
    [TestFixture]
    class SQLiteReceivedPurchaseOrderTest : ReceivedPurchaseOrderTest
    {
        SQLiteDatabaseHelper databaseHelper;

        [SetUp]
        public override void SetUp()
        {
            databaseHelper = new SQLiteDatabaseHelper();
            databaseHelper.CreateTestDatabase();
            SQLiteStoreContext context = new SQLiteStoreContext(databaseHelper.DatabaseFile);
            operatorSource = new SQLiteBatchOperatorSource(context);
            base.SetUp();
        }

        [TearDown]
        public void TearDown()
        {
            databaseHelper.DestroyTestDatabase();
        }
    }
}
